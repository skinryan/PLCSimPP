using System;
using CommonLib.CommandDispatching.Command;

namespace CommonLib.CommandDispatching.Dispatcher
{
    /// <summary>
    /// CommandDispatcher that executes commands in sorted order.  The commands all implement IComparable.
    /// </summary>
    public abstract class ComparableCommandDispatcher : CommandDispatcherBase<ComparableCommand>
    {
        protected ComparableCommandDispatcher(string descriptionArg)
            : base(descriptionArg)
        {
        }

        protected override void Insert(ComparableCommand commandArg)
        {
            try
            {
                int indexPosition = pCommandList.BinarySearch(commandArg);
                //Console.WriteLine("insert index: " + indexPosition + "; count = " + m_CommandList.Count);
                if (indexPosition < 0)
                { // no match
                  //bitwise complement (~) to this negative integer 
                  //to get the index of the first element that is larger 
                  //than the search value
                    indexPosition = ~indexPosition;
                }
                else if (indexPosition >= 0)
                { // match--insert after same order value items
                    int count = pCommandList.Count;

                    while (++indexPosition < count)
                    {
                        ComparableCommand nextCommand = pCommandList[indexPosition];
                        if (commandArg.CompareTo(nextCommand) != 0)
                        {
                            break;
                        }
                    }
                }
                //Console.WriteLine("...insert index: " + indexPosition);

                pCommandList.Insert(indexPosition, commandArg);
            }
            catch (Exception)
            {
                //if exception,than add command to end
                pCommandList.Add(commandArg);
            }

        }
    }
}
