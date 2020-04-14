using System;
using System.Diagnostics;
using System.IO;

namespace DcSimCom
{
    /// <summary>
    /// Used to launch DCSim
    /// </summary>
    public static class DcSimExeLauncher
    {
        /// <summary>
        /// DcSim process name
        /// </summary>
        public const string PROCESS_NAME = "DcSim";

        /// <summary>
        /// DCSim process
        /// </summary>
        public static Process DcSimProcess { get; set; }

        // Methods(s) - Public =========================================================

        /// <summary>
        /// Start DCSim
        /// </summary>
        /// <param name="dcSimCommandLineArg">DCSim command line</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Launch(DcSimCommandLine dcSimCommandLineArg)
        {
            KillRunningDcSim();

            var dcSimExePath = dcSimCommandLineArg.DcSimExePath;
            if (string.IsNullOrEmpty(dcSimExePath) || !File.Exists(dcSimExePath))
            {
                throw new InvalidOperationException("DCSim EXE file specified in App Settings does not exist: " + dcSimExePath);
            }

            DcSimProcess = new Process();
            DcSimProcess.StartInfo.FileName = dcSimExePath;
            DcSimProcess.StartInfo.Arguments = dcSimCommandLineArg.ToString();
            DcSimProcess.Start();
        }

        /// <summary>
        /// Terminate DCSim application
        /// </summary>
        public static void Stop()
        {
            if (DcSimProcess != null)
            {
                DcSimProcess.Close();
                DcSimProcess = null;
                KillRunningDcSim();
            }
        }

        // Methods(s) - Private =========================================================

        private static void KillRunningDcSim()
        {
            var dcSimprocess = GetProcess(PROCESS_NAME);
            if (dcSimprocess != null)
            {
                dcSimprocess.Kill();
            }
        }

        private static Process GetProcess(string processNameArg)
        {
            var process = Process.GetProcessesByName(processNameArg);
            return process.Length > 0 ? process[0] : null;
        }
    }
}