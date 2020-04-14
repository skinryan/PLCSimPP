//#define DiagnosticOutput

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CommonLib.CommandDispatching.Command;

namespace CommonLib.CommandDispatching.Dispatcher
{
    /// <summary>
    /// Abstract superclass for the command dispatchers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandDispatcherBase<T> where T : CommandBase
    {
        protected Object pLock = new Object();
        private Thread mThread;
        protected List<T> pCommandList = new List<T>();
        private readonly string mDescription;

        protected CommandDispatcherBase(string descriptionArg)
        {
            mDescription = descriptionArg;
            StartThread();
        }

        protected void StartThread()
        {
            mThread = new Thread(Run)
            {
                Name = mDescription,
                IsBackground = true
            };
            mThread.Start();
        }

        internal int CommandCount
        {
            get
            {
                return pCommandList.Count;
            }
        }

        protected virtual void OnEnqueueCommandEnter()
        {
        }
        protected virtual void OnEnqueueCommandExit()
        {
        }
        internal bool Enqueue(T commandArg)
        {
            OnEnqueueCommandEnter();
            lock( pLock )
            {
                if( mStop )
                {
                    return false;
                }
                Insert(commandArg);
                Monitor.Pulse(pLock);
            }
            OnEnqueueCommandExit();
            return true;
        }

        protected abstract void Insert(T commandArg);

        protected virtual void OnCommandDepletion()
        {
        }
        protected virtual void OnNextCommandIsAvailable()
        {
        }
        protected virtual void OnCommandExecutionCompletion()
        {
        }
        private void Run()
        {
            T command = default(T);

            for( ; ; )
            {
                try
                {
                    lock( pLock )
                    {
                        if( mStop && pCommandList.Count <= 0 )
                        {
#if DiagnosticOutput
                            System.Diagnostics.Trace.WriteLine("Stopping...");
#endif
                            return;
                        }

                        while( (command = PeekNextCommand()) == null )
                        {
                            OnCommandDepletion();
#if DiagnosticOutput
                            System.Diagnostics.Trace.WriteLine("Wait on empty...");
#endif
                            Monitor.Wait(pLock);
                            if( mStop )
                            {
#if DiagnosticOutput
                                System.Diagnostics.Trace.WriteLine("Stopping @ empty...");
#endif
                                return;
                            }
                        }
                        OnNextCommandIsAvailable();
                        command = pCommandList[0];
                        pCommandList.Remove(command);
                    } // lock

#if DiagnosticOutput
                    System.Diagnostics.Trace.WriteLine("Executing...");
#endif
                    command.Execute();
                    OnCommandExecutionCompletion();
                }
                catch( Exception ex )
                {
                    StringBuilder message = new StringBuilder("Command Execution Exception");
                    if( command != null )
                    {
                        message.Append(": ").AppendLine(command.ToString());
                    }
                    message.AppendLine(ex.ToString());

                    Console.Write(message.ToString());
                    //Logger.Log(Logger.Level.Fatal, message, ex);
                }
            }
        }

        protected T PeekNextCommand()
        {
            T command = default(T);
            if( pCommandList.Count > 0 )
            {
                command = pCommandList[0];
            }
            return command;
        }

        private bool mStop;
        public void Stop()
        {
#if DiagnosticOutput
            System.Diagnostics.Trace.WriteLine("Stop request...");
#endif
            lock( pLock )
            {
                mStop = true;
                Monitor.Pulse(pLock);
#if DiagnosticOutput
                System.Diagnostics.Trace.WriteLine("Stop request done...");
#endif
            }
        }

        public bool CanStop()
        {
            lock( pLock )
            {
                return (pCommandList.Count <= 0);
            }
        }
    }
}
