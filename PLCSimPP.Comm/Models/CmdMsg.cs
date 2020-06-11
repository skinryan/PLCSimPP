using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Comm.Models
{
    /// <summary>
    /// The implementation of IMessage
    /// </summary>
    public class MsgCmd : IMessage
    {
        /// <inheritdoc />
        public string Command { get; set; }

        /// <inheritdoc />
        public string Param { get; set; }

        /// <inheritdoc />
        public string UnitAddr { get; set; }

        /// <inheritdoc />
        public int Port { get; set; }

        /// <summary>
        /// Build message log
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UnitAddr + "|" + Command + "|" + Param;
        }
    }
}
