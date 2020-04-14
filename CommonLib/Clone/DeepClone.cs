using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonLib.Clone
{
    public static class DeepClone
    {
        /// <summary>
        /// Makes a deep clone of the specified object.
        /// </summary>
        /// <typeparam name="T">Type of Object need to copy</typeparam>
        /// <param name="objectToClone">Object to make a deep clone of.</param>
        /// <returns>Deep copy of the Object</returns>
        public static T Make<T>(T objectToClone) where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, objectToClone);
                memoryStream.Position = 0;
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}