using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.EventArguments;
using BCI.PLCSimPP.Communication.Models;
using Prism.Ioc;

namespace BCI.PLCSimPP.Communication.Support
{
    public class SmartConnection
    {
        #region "Variables, Events"

        public delegate void SmartConnectionDataReceivedEventHandler(object sender, SmartConnectionDataReceivedEventArgs e);
        private SmartConnectionDataReceivedEventHandler mSmartConnectionDataReceived;

        public event SmartConnectionDataReceivedEventHandler SmartConnectionDataReceivedEvent
        {
            add
            {
                mSmartConnectionDataReceived = (SmartConnectionDataReceivedEventHandler)System.Delegate.Combine(mSmartConnectionDataReceived, value);
            }
            remove
            {
                mSmartConnectionDataReceived = (SmartConnectionDataReceivedEventHandler)System.Delegate.Remove(mSmartConnectionDataReceived, value);
            }
        }

        public delegate void SmartConnectionClosedEventHandler(object sender, EventArgs e);
        private SmartConnectionClosedEventHandler mSmartConnectionClosed;

        public event SmartConnectionClosedEventHandler SmartConnectionClosedEvent
        {
            add
            {
                mSmartConnectionClosed = (SmartConnectionClosedEventHandler)System.Delegate.Combine(mSmartConnectionClosed, value);
            }
            remove
            {
                mSmartConnectionClosed = (SmartConnectionClosedEventHandler)System.Delegate.Remove(mSmartConnectionClosed, value);
            }
        }


        #endregion

        #region "Variables and Properties"

        private TcpClient mClient;
        private NetworkStream mClientStream;
        private byte[] mClientBuffer;
        private MemoryStream mBufferStream;
        private ILogService logger;

        public bool IsConnected
        {
            get
            {
                if (mClient == null)
                {
                    return false;
                }

                return mClient.Connected;
            }
        }

        #endregion

        #region "Constructor"

        public SmartConnection()
        {

        }

        private void LogBytesData(string msg, byte[] content)
        {
            if (logger == null)
            {
                logger = ServiceLocator.Current.GetInstance<ILogService>();
            }

            Console.WriteLine(msg + EncoderHelper.ToHexString(content));
            logger.LogRawData(msg, content);
        }

        #endregion

        #region "TcpClient Handlers"

        public void ConnectTo(TcpClient client)
        {
            if (client == null)
            {
                throw (new ArgumentNullException("client"));
            }

            try
            {
                mClient = client;
                mClientStream = mClient.GetStream();
                mClientBuffer = new byte[mClient.ReceiveBufferSize];

                // Asynchronous background read process.
                mClientStream.BeginRead(mClientBuffer, 0, mClient.ReceiveBufferSize - 1, new System.AsyncCallback(OnReceive), null);
            }
            catch (System.Exception)
            {
                OnConnectionClosed();
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int count;
                try
                {
                    // Data received.
                    count = mClient.GetStream().EndRead(ar);
                }
                catch (System.Exception wx)
                {
                    // Server closed ungracefully.
                    count = 0;
                }

                if (count > 0)
                {
                    try
                    {
                        HandleData(count);
                    }
                    finally
                    {
                        // Asynchronous background read process.
                        mClientStream.BeginRead(mClientBuffer, 0, mClient.ReceiveBufferSize, new System.AsyncCallback(OnReceive), null);
                    }
                }
                else
                {
                    OnConnectionClosed();
                }
            }
            catch (System.Exception)
            {
                OnConnectionClosed();
            }
        }


        private void HandleData(int count)
        {
            // Start of new data, create a new buffer stream.
            mBufferStream = new MemoryStream();

            // Write the data into the buffer stream.
            if (mClientBuffer != null)
            {
                mBufferStream.Write(mClientBuffer, 0, count);
                mBufferStream.Position = 0;
            }

            //unpackage data 
            byte[] buffer = new byte[System.Convert.ToInt32(mBufferStream.Length - 1) + 1];
            mBufferStream.Read(buffer, 0, buffer.Length);

            //LogBytesData(string.Format("{0} Received Data:", this.mClient.Client.LocalEndPoint), buffer);
            //Console.WriteLine("Received Data: {0}", EncoderHelper.ToHexString(buffer));

            //check data valid
            var checkResult = DataHelper.CheckData(buffer);
            switch (checkResult.Result)
            {
                case ResultType.InvalidLength:
                    LogBytesData($"{this.mClient.Client.LocalEndPoint} Received Data:", buffer);
                    //replay 0x50:Invalid Command
                    DoSend(new byte[2] { 0xE0, 0x50 });
                    break;
                case ResultType.InvalidCmd:
                    LogBytesData($"{this.mClient.Client.LocalEndPoint} Received Data:", buffer);
                    //replay 0x52: Invalid data length
                    DoSend(new byte[2] { 0xE0, 0x52 });
                    break;
                case ResultType.Heartbeat:
                    DoSend(new byte[2] { 0xE0, 0x00 });
                    break;
                case ResultType.RawData:
                    //replay E000：correct
                    LogBytesData($"{this.mClient.Client.LocalEndPoint} Received Data:", buffer);
                    DoSend(new byte[2] { 0xE0, 0x00 });
                    break;
                default:
                    //do nothing
                    return;
            }

            if (checkResult.Result == ResultType.RawData)
            {
                var msg = EncoderHelper.ConvertToMsg(buffer);

                // Signal the arrival of an message.
                OnMessageReceived(msg);
            }
        }

        private bool mClientClosedAndDisposed;
        private bool mClientStreamClosedAndDisposed;

        public void Disconnect()
        {
            if ((mClientStream != null) && !mClientStreamClosedAndDisposed)
            {
                mClientStreamClosedAndDisposed = true;
                mClientStream.Close();
                mClientStream.Dispose();
                mClientStream = null;
            }

            if ((mClient != null) && !mClientClosedAndDisposed)
            {
                mClientClosedAndDisposed = true;
                mClient.Close();
                mClient = null;
            }
        }

        #endregion

        #region "Connection Close Handler"

        protected virtual void OnConnectionClosed()
        {
            // Raise event on background thread.
            var args = new EventArgs();
            var handler = mSmartConnectionClosed;
            if (handler != null)
                handler(this, args);
        }

        #endregion

        #region "Rx Msg Handler"

        protected virtual void OnMessageReceived(Comm.Interfaces.IMessage data)
        {
            // Raise event on background thread
            var args = new SmartConnectionDataReceivedEventArgs(data);
            var handler = mSmartConnectionDataReceived;
            if (handler != null)
                handler(this, args);
        }

        #endregion

        #region "Send Data"

        private readonly object mSendLock = new object();


        private void DoSend(object data)
        {
            byte[] dataBytes = (byte[])data;

            //Console.WriteLine("Send Data: {0}", EncoderHelper.ToHexString(dataBytes));
            if (dataBytes.Length > 2)
            {
                LogBytesData($"{this.mClient.Client.LocalEndPoint} Send Data: ", dataBytes);
            }

            using (var buffer = new MemoryStream())
            {
                buffer.Write(dataBytes, 0, dataBytes.Length);

                var synclockObject = mSendLock;
                Monitor.Enter(synclockObject);
                try
                {
                    if ((mClientStream != null) && mClientStream.CanWrite)
                    {
                        if (mClient != null)
                        {
                            if (mClient.Connected)
                            {
                                buffer.WriteTo(mClientStream);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    OnConnectionClosed();
                }
                finally
                {
                    Monitor.Exit(synclockObject);
                }
            }

        }

        public void Send(IMessage data)
        {
            Send(data, true);
        }

        private void Send(IMessage data, bool synchronizeSend)
        {
            if (synchronizeSend)
            {
                var bytesData = DataHelper.BuildSendData(data);
                DoSend(bytesData);
            }
            else
            {
                var bytesData = DataHelper.BuildSendData(data);
                ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DoSend), bytesData);
            }
        }

        #endregion
    }
}
