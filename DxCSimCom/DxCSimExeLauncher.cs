using System;
using System.Diagnostics;
using System.IO;

namespace DxCSimCom
{
    /// <summary>
    /// Class used to launch DXCSim
    /// </summary>
    public static class DxCSimExeLauncher
    {
        /// <summary>
        /// DXCSim process
        /// </summary>
        public static Process DxCSimProcess { get; set; }

        // Methods(s) - Public =========================================================

        /// <summary>
        /// Launch DxSim
        /// </summary>
        /// <param name="dxCSimCommandLine">DxCSim command Line</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Launch (DxCSimCommandLine dxCSimCommandLine)
        {
            KillRunningDxCSim();

            var dxCSimExePath = dxCSimCommandLine.DxCSimExePath;
            if (string.IsNullOrEmpty(dxCSimExePath) || !File.Exists(dxCSimExePath))
            {
                throw new InvalidOperationException("DXCSim EXE file specified in App Settings does not exist: " +
                                                    dxCSimExePath);
            }

            DxCSimProcess = new Process();
            DxCSimProcess.StartInfo.FileName = dxCSimExePath;
            DxCSimProcess.StartInfo.Arguments = dxCSimCommandLine.ToString();
            DxCSimProcess.Start();
        }

        /// <summary>
        /// Terminate DXCSim application
        /// </summary>
        public static void Stop ()
        {
            if(DxCSimProcess != null)
            {
                DxCSimProcess.Close();
                DxCSimProcess = null;
                KillRunningDxCSim();
            }
        }

        // Methods(s) - Private =========================================================

        private static void KillRunningDxCSim ()
        {
            var dxCSimProcess = GetProcess("LX20Sim");
            if(dxCSimProcess != null)
            {
                dxCSimProcess.Kill();
            }
        }

        private static Process GetProcess (string processNameArg)
        {
            var process = Process.GetProcessesByName(processNameArg);
            return process.Length > 0 ? process[0] : null;
        }
    }
}
