using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Comm.Constants
{
    public class LcCmds
    {
        /// <summary>
        /// AUTO Mode Start
        /// Notification of the beginning of AUTO operation
        /// </summary>
        public const string _0001 = "0001";

        /// <summary>
        /// AUTO Mode Termination
        /// Notification of the termination of AUTO operation
        /// </summary>
        public const string _0002 = "0002";

        /// <summary>
        /// Lane-through Control
        /// Notify to open all gates
        /// </summary>
        public const string _0003 = "0003";

        /// <summary>
        /// Operation Mode Inquiry
        /// Inquire for Operation Mode
        /// </summary>
        public const string _0004 = "0004";

        /// <summary>
        /// Buzzer Control
        /// Generate Buzzer Alarm and turn the lamp on
        /// </summary>
        public const string _0005 = "0005";

        /// <summary>
        /// Bar-code Reading Retry Request
        /// Request to retry reading sample bar-code
        /// </summary>
        public const string _0006 = "0006";

        /// <summary>
        /// Sorting Information （Take-in）
        /// Request to take in a sample
        /// </summary>
        public const string _0011 = "0011";

        /// <summary>
        /// Pass-through
        /// Request to pass a sample through
        /// </summary>
        public const string _0012 = "0012";

        /// <summary>
        /// Clot Data Notification
        /// Send the information of clot height for Primary tube and request for serum volume
        /// </summary>
        public const string _0013 = "0013";

        /// <summary>
        /// Aspiration Order
        /// Send primary tube aspiration volume
        /// </summary>
        public const string _0014 = "0014";

        /// <summary>
        /// Aliquoting Order
        /// Send aliquoting volume to aliquot to secondary tubes
        /// </summary>
        public const string _0015 = "0015";

        /// <summary>
        /// Sample Label Information
        /// Send label printing information for secondary tube
        /// </summary>
        public const string _0016 = "0016";

        /// <summary>
        /// Sample Storage Position
        /// Send sample storage position
        /// </summary>
        public const string _0017 = "0017";

        /// <summary>
        /// Rack Change Request
        /// Request to replace sample racks
        /// </summary>
        public const string _0018 = "0018";

        /// <summary>
        /// Retrieve / Retest Order
        /// Request to retrieve samples from Stockyard
        /// </summary>
        public const string _0019 = "0019";

        /// <summary>
        /// Sorting Command
        /// Respond to the sorting query and request to sort
        /// </summary>
        public const string _001A = "001A";

        /// <summary>
        /// Aliquoting Command
        /// Send Aliquot volume to Aliquoter
        /// </summary>
        public const string _0020 = "0020";

        /// <summary>
        /// Aliquot Tip Tray Change Request
        /// Request to replace Aliquot Tip Tray
        /// </summary>
        public const string _0021 = "0021";

        /// <summary>
        /// Conveyor Control
        /// Control Conveyor condition
        /// </summary>
        public const string _0023 = "0023";

        /// <summary>
        /// Conveyor Stop Preparation Request
        /// Request to be prepare to stop the conveyor
        /// </summary>
        public const string _0025 = "0025";

        /// <summary>
        /// Holder Feeding Order
        /// Request to feed holders
        /// </summary>
        public const string _0026 = "0026";

        /// <summary>
        /// Floor Control
        /// Control Stockyard Floor,  Disable/Enable
        /// </summary>
        public const string _0028 = "0028";



        /// <summary>
        /// All received commands array
        /// </summary>
        public static readonly List<string> ReceivedCmds = new List<string> {_0001,
                                                                             _0002,
                                                                             _0003,
                                                                             _0004,
                                                                             _0005,
                                                                             _0006,
                                                                             _0011,
                                                                             _0012,
                                                                             _0013,
                                                                             _0014,
                                                                             _0015,
                                                                             _0016,
                                                                             _0017,
                                                                             _0018,
                                                                             _0019,
                                                                             _001A,
                                                                             _0020,
                                                                             _0021,
                                                                             _0023,
                                                                             _0025,
                                                                             _0026,
                                                                             _0028};
    }

    public class UnitCmds
    {
        /// <summary>
        /// Operation Mode Notification
        /// Notification of Operation Mode
        /// </summary>
        public const string _1001 = "1001";

        /// <summary>
        /// Key Pad Operation
        /// Notification of the result of Unit Key Pad Operation
        /// </summary>
        public const string _1002 = "1002";

        /// <summary>
        /// Arrival Information
        /// Send the result of bar-code reading
        /// </summary>
        public const string _1011 = "1011";

        /// <summary>
        /// Clot Data
        /// Send the result of Clot Height Measurement
        /// </summary>
        public const string _1012 = "1012";

        /// <summary>
        /// Serum Data
        /// Send the serum height
        /// </summary>
        public const string _1013 = "1013";

        /// <summary>
        /// Aspiration Result
        /// Send the aspiration result
        /// </summary>
        public const string _1014 = "1014";

        /// <summary>
        /// Rack Mapping Information II
        /// Notification of Sample Storage Position and send the routing result to Analyzer
        /// </summary>
        public const string _1015 = "1015";

        /// <summary>
        /// Rack Change Completion
        /// Send the result of Rack change
        /// </summary>
        public const string _1016 = "1016";

        /// <summary>
        /// Holder Queue Notification
        /// Notification of Holder Queue condition
        /// </summary>
        public const string _1017 = "1017";

        /// <summary>
        /// Rack Mapping Information I
        /// Notification of Sample Storage Position
        /// </summary>
        public const string _1018 = "1018";

        /// <summary>
        /// Sample Retrieval Completion
        /// Send the result of sample retrieval
        /// </summary>
        public const string _1019 = "1019";

        /// <summary>
        /// Sorting Query
        /// Send Sorting Query
        /// </summary>
        public const string _101A = "101A";

        /// <summary>
        /// Centrifuge Status Information
        /// Notification of Centrifuge status
        /// </summary>
        public const string _1022 = "1022";

        /// <summary>
        /// Arrival Information with Flag
        /// Send Bar-code information, Serum Volume and tube type
        /// </summary>
        public const string _1024 = "1024";

        /// <summary>
        /// Conveyor Status
        /// Notification of Conveyor STOP/RUN status
        /// </summary>
        public const string _1025 = "1025";

        /// <summary>
        /// Holder Request
        /// Request for holders
        /// </summary>
        public const string _1026 = "1026";

        /// <summary>
        /// Error Notification
        /// Notification of occurrence of error
        /// </summary>
        public const string _10E0 = "10E0";

        /// <summary>
        /// Illegal
        /// Notification of the receipt of invalid message
        /// </summary>
        public const string _10E1 = "10E1";
    }
}
