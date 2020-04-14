using System.Text;
using CommonLib.TcpSocket;

namespace DcSimCom
{
    /// <summary>
    /// This message is sent to DcSim when a tube is sorted to a dynamic analyzer.
    /// </summary>
    public class LoadSampleOnAnalyzerMessage : ICommMessage
    {
        /// <summary>
        /// Dynamic connection unit number to be send to
        /// </summary>
        public int UnitNumber { get; set; }
        /// <summary>
        /// Sample id in the Token message
        /// </summary>
        public string SampleId { get; set; }
        /// <summary>
        /// DCSim token id
        /// </summary>
        public string DInstrTokenId { get; set; }

        /// <summary>
        /// Build xml string with 
        /// </summary>
        /// <returns></returns>
        public string ToXml ()
        {
            var result = new StringBuilder();
            result.AppendLine("<xml>");
            result.AppendLine(" <LoadSampleOnAnalyzer>");
            result.AppendFormat("  <UnitNumber>{0}</UnitNumber>\n", UnitNumber);
            result.AppendFormat("  <SampleId>{0}</SampleId>\n", SampleId);
            result.AppendFormat("  <TokenId>{0}</TokenId>\n", DInstrTokenId);
            result.AppendLine(" </LoadSampleOnAnalyzer>");
            result.AppendLine("</xml>");
            return result.ToString();
        }

        /// <summary>
        /// Convert XML message to bytes array
        /// </summary>
        /// <returns></returns>
        public byte[] ToMessageBytes ()
        {
            var buffer = Encoding.UTF8.GetBytes(ToXml());
            return buffer;
        }
    }
}
