using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using CommonLib.CommandDispatching.Dispatcher;

namespace CommonLib.TcpSocket
{
    /// <summary>
    /// Base class for TcpCommClient and TcpCommServer.
    /// </summary>
    public abstract class TcpComm
    {
        public const string RECEIVER_NAME = "TcpCommReceiver";

        /// <summary>
        /// Callback that is invoked when the connection closes.
        /// </summary>
        public Action OnDisconnectedCallback { get; set; }

        /// <summary>
        /// The underlying NetworkStream used for transmission of bytes.
        /// </summary>
        protected NetworkStream NetStreamComm { get; set; }

        /// <summary>
        /// True if a connection has been made.
        /// </summary>
        protected bool IsConnected
        {
            get
            {
                return NetStreamComm != null;
            }
        }

        /// <summary>
        /// Each time bytes are received from the connection, it will be saved into
        /// a buffer (mTempReceiveBuffer).  This specified the buffer size.
        /// </summary>
        protected const int DEFAULT_UNPROCESSED_RECEIVE_BYTES_BUFFER_SIZE = 1024;

        /// <summary>
        /// Contains the unprocessed received bytes.
        /// </summary>
        private byte[] mUnprocessedReceivedBytesBuffer;

        /// <summary>
        /// Contains the bytes received on the latest network read.
        /// </summary>
        private readonly byte[] mTempReceiveBuffer;

        /// <summary>
        /// This dispatcher (with its own thread) is used to send messages.
        /// </summary>
        private QueueCommandDispatcher mSendQueueCommandDispatcher = new QueueCommandDispatcher("CommSender");

        /// <summary>
        /// This dispatcher is used to callback to the client for each received message
        /// (note that these are not the raw bytes, but are the completed messages).
        /// </summary>
        private QueueCommandDispatcher mReceiveQueueCommandDispatcher = new QueueCommandDispatcher("CommReceiver");

        /// <summary>
        /// This thread is used to receive the raw bytes and convert them to messages.
        /// Once a message is created, it is given to mReceiveQueueCommandDispatcher.
        /// </summary>
        private Thread mReceiveThread;

        /// <summary>
        /// List of converter objects used to convert raw bytes into messages.
        /// There is one convert for each message.
        /// </summary>
        private readonly List<IToCommMessageConverterWithCallback> mToCommMessageConverters = new List<IToCommMessageConverterWithCallback>();

        private const string NOT_CONNECTED_MESSAGE = "Not connected!";

        // Constructor(s) ==============================================================

        protected TcpComm(int receiveBufferSizeArg = DEFAULT_UNPROCESSED_RECEIVE_BYTES_BUFFER_SIZE)
        {
            mTempReceiveBuffer = new byte[receiveBufferSizeArg];
        }

        // Methods(s) - Public =========================================================

        /// <summary>
        /// Add another converter to be used to convert raw message bytes to a message.
        /// There is one converter for each message.  The converters will be called one
        /// by one to convert bytes into a message.
        /// </summary>
        /// <param name="toCommMessageConverterWithCallbackArg"></param>
        public void AddCommMessageConverterWithCallback(IToCommMessageConverterWithCallback toCommMessageConverterWithCallbackArg)
        {
            mToCommMessageConverters.Add(toCommMessageConverterWithCallbackArg);
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public virtual void Disconnect()
        {
            if (NetStreamComm != null)
            {
                StopReceivingThread();

                NetStreamComm.Close();
                NetStreamComm = null;

                mSendQueueCommandDispatcher.Stop();
                mSendQueueCommandDispatcher = null;

                mReceiveQueueCommandDispatcher.Stop();
                mReceiveQueueCommandDispatcher = null;
            }
        }

        public void Send(byte[] bytesArg)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException(NOT_CONNECTED_MESSAGE);
            }

            mSendQueueCommandDispatcher.Enqueue(() => NetStreamComm.Write(bytesArg, 0, bytesArg.Length));
        }

        // Methods(s) - Protected =========================================================

        protected void StartReceivingThread()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException(NOT_CONNECTED_MESSAGE);
            }

            mReceiveThread = new Thread(ReceiveLoop) { Name = RECEIVER_NAME, IsBackground = true };
            mReceiveThread.Start();
        }

        protected void StopReceivingThread()
        {
            if (mReceiveThread != null)
            {
                mReceiveThread.Abort();
                mReceiveThread = null;
            }
        }

        // Methods(s) - Private ========================================================

        /// <summary>
        /// Continuously read bytes from the socket.  
        /// This is executed in its own thread.
        /// </summary>
        private void ReceiveLoop()
        {
            for (; ; )
            {
                int numBytesReceived = Receive();
                if (numBytesReceived == 0)
                {
                    if (OnDisconnectedCallback != null)
                    {
                        OnDisconnectedCallback();
                    }
                    break;
                }

                ConvertToMessageAndNotify();
            }
        }

        /// <summary>
        /// Read the next set of available bytes from the network connection.
        /// Returns the number of bytes received; 0 if connection is closed.
        /// Block until bytes are read or connection is closed.
        /// </summary>
        /// <returns></returns>
        private int Receive()
        {
            int numberBytesRead = NetStreamComm.Read(mTempReceiveBuffer, 0, mTempReceiveBuffer.Length);
            if (numberBytesRead != 0)
            {
                AppendReceivedBytesToMessageBuffer(mTempReceiveBuffer, numberBytesRead);
            }

            return numberBytesRead;
        }

        /// <summary>
        /// Append the bytes just read from the network connection to the
        /// internal message buffer.
        /// </summary>
        /// <param name="receivedBytesArg"></param>
        /// <param name="numberReceivedBytesArg"></param>
        private void AppendReceivedBytesToMessageBuffer(byte[] receivedBytesArg, int numberReceivedBytesArg)
        {
            // Expand the receive buffer.
            if (mUnprocessedReceivedBytesBuffer == null)
            {
                mUnprocessedReceivedBytesBuffer = new byte[numberReceivedBytesArg];

                Array.Copy(receivedBytesArg, mUnprocessedReceivedBytesBuffer, numberReceivedBytesArg);
            }
            else
            {
                int currentBufferSize = mUnprocessedReceivedBytesBuffer.Length;
                int newBufferSize = currentBufferSize + numberReceivedBytesArg;
                Array.Resize(ref mUnprocessedReceivedBytesBuffer, newBufferSize);

                // Copy the received bytes to the buffer.
                var source = receivedBytesArg;
                var target = mUnprocessedReceivedBytesBuffer;
                var numberBytesToCopy = numberReceivedBytesArg;
                const int sourceStartIndex = 0;
                var targetStartIndex = currentBufferSize;
                Array.Copy(source, sourceStartIndex, target, targetStartIndex, numberBytesToCopy);
            }
        }

        /// <summary>
        /// Give the internal message bytes buffer to each of the message converter to convert
        /// the raw bytes into messages.  For each created message, the converter will also
        /// invoke its associated callback for processing of the message by the client.
        /// Keep creating messages until the message bytes can no longer generate a message.
        /// </summary>
        private void ConvertToMessageAndNotify()
        {
            bool wasMessageCreated;

            do
            {
                wasMessageCreated = false;

                foreach (var converter in mToCommMessageConverters)
                {
                    var messageWithByteSize = converter.CreateMessageFromBytes(mUnprocessedReceivedBytesBuffer);
                    if (messageWithByteSize != null)
                    {
                        // Removed converted bytes from receive buffer.
                        int sourceStartIndex = messageWithByteSize.MessageByteLength;
                        const int destinationStartIndex = 0;
                        int numberBytesToCopy = mUnprocessedReceivedBytesBuffer.Length - messageWithByteSize.MessageByteLength;
                        Array.Copy(mUnprocessedReceivedBytesBuffer, sourceStartIndex,
                            mUnprocessedReceivedBytesBuffer, destinationStartIndex, numberBytesToCopy);
                        Array.Resize(ref mUnprocessedReceivedBytesBuffer, numberBytesToCopy);

                        // Invoke message received callback.
                        var foundConverter = converter;
                        mReceiveQueueCommandDispatcher.Enqueue(() => foundConverter.OnMessageReceivedCallback(messageWithByteSize.Message));

                        wasMessageCreated = true;
                        break;
                    }
                }
            } while (wasMessageCreated && mUnprocessedReceivedBytesBuffer.Length > 0);
        }
    }
}