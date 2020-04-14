using System.Text;
using CommonLib.TcpSocket;

namespace DxCSimCom.ToDxCSimMessage
{
    /// <summary>
    /// This message is sent to DxCSim when a tube is sorted to a DXC.
    /// </summary>
    public class LoadSampleOnDxCMessage : ICommMessage
    {
        /// <summary>
        /// The DXC dynamic connection number to be communicted with
        /// </summary>
        public int UnitNumber { get; set; }

        /// <summary>
        /// Sample id of the Token 
        /// </summary>
        public string SampleId { get; set; }

        /// <summary>
        /// DxCSim token ID
        /// </summary>
        public string DxCInstrTokenId { get; set; }

        /// <summary>
        /// Build xml string with the token properties
        /// </summary>
        /// <returns></returns>
        public string ToXml ()
        {
            var result = new StringBuilder();
            result.AppendLine("<xml>");
            result.AppendLine(" <LoadSampleOnDxC>");
            result.AppendFormat("  <UnitNumber>{0}</UnitNumber>\n", UnitNumber);
            result.AppendFormat("  <SampleId>{0}</SampleId>\n", SampleId);
            result.AppendFormat("  <TokenId>{0}</TokenId>\n", DxCInstrTokenId);
            result.AppendLine(" </LoadSampleOnDxC>");
            result.AppendLine("</xml>");
            return result.ToString();
        }

        /// <summary>
        /// Convert token message string to Bytes array
        /// </summary>
        /// <returns></returns>
        public byte[] ToMessageBytes ()
        {
            var buffer = Encoding.UTF8.GetBytes(ToXml());
            return buffer;
        }
    }
}
