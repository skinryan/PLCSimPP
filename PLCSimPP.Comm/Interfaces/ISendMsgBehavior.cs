namespace BCI.PLCSimPP.Comm.Interfaces
{
    public interface ISendMsgBehavior
    {
        

        /// <summary>
        /// Add a message to the send queue
        /// </summary>
        /// <param name="msg">message entry</param>
        void PushMsg(IMessage msg);

        /// <summary>
        /// Start the send message task
        /// </summary>
        /// <param name="token">script token</param>
        void ActiveSendTask(string token);

        /// <summary>
        /// Stop the send message task
        /// </summary>
        void StopSendTask();
    }
}