using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Communication.Exceptions
{
    public class InvalidCommandException : System.Exception
    {
        public string InvalidCommand { get; private set; }

        public InvalidCommandException(string cmd)
        {
            InvalidCommand = cmd;
        }
    }

    public class InvalidDataLengthException : System.Exception
    {
        public int ExpectLength { get; private set; }

        public int ActualLength { get; private set; }

        public InvalidDataLengthException(int expect, int actual)
        {
            ExpectLength = expect;
            ActualLength = actual;
        }
    }
}
