using System;

namespace CommonLib.CommandDispatching.Command
{
    internal class DelayCommand : ComparableCommand
    {
        internal DateTime ExecutionDateTime;

        internal DelayCommand(CommandBase commandArg, DateTime executionDateTimeArg)
            : base(commandArg)
        {
            ExecutionDateTime = executionDateTimeArg;
        }

        internal int NumMillesecToExecution
        {
            get
            {
                int numMillecsec = (int)(ExecutionDateTime.Subtract(DateTime.Now).TotalMilliseconds);
                if( numMillecsec < 0 )
                {
                    numMillecsec = 0;
                }
                return numMillecsec;
            }
        }

        #region IComparable Members

        public override int CompareTo(object obj)
        {
            DelayCommand delayCommand = obj as DelayCommand;
            if( delayCommand == null )
            {
                throw new ArgumentException("Parameter must be of type TimedCommand!");
            }

            int result = ExecutionDateTime.CompareTo(delayCommand.ExecutionDateTime);
            //Console.WriteLine("CompareTo: " + result + "; " + this.ExecutionDateTime.ToString() + "; " + delayCommand.ExecutionDateTime.ToString());
            if( result == 0 )
            {
                result = base.CompareTo(obj);
            }
            return result;
        }

        #endregion

        public override string ToString()
        {
            return "Execution Time: " + ExecutionDateTime.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
