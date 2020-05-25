using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Communication.Execption
{
    public class InvalidCommandExecption : Exception
    {
        public string InvalidCommand { get; private set; }

        public InvalidCommandExecption(string cmd)
        {
            InvalidCommand = cmd;
        }
    }

    public class InvalidDataLengthExecption : Exception
    {
        public int ExpectLength { get; private set; }

        public int ActualLength { get; private set; }

        public InvalidDataLengthExecption(int expect, int actual)
        {
            ExpectLength = expect;
            ActualLength = actual;
        }
    }
}
