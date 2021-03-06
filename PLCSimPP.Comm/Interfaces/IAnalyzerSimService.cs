﻿namespace BCI.PLCSimPP.Comm.Interfaces
{
    /// <summary>
    /// Analyzer Simulate Service
    /// </summary>
    public interface IAnalyzerSimService
    {
        /// <summary>
        /// Start up the simulator
        /// </summary>
        void StartUp();

        /// <summary>
        /// Shut down the simulator
        /// </summary>
        void ShutDown();

        /// <summary>
        /// Send message to simulator
        /// </summary>
        /// <param name="unitNum">unit number</param>
        /// <param name="token">token</param>
        /// <param name="sampleId">sample id</param>
        void SendMsg(int unitNum, string token, string sampleId);
    }
}