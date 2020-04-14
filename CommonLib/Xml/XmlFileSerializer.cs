using System.IO;
using System.Xml.Serialization;

namespace CommonLib.Xml
{
    public sealed class XmlFileSerializer<T> where T : class
    {
        public T DeserializedInstance { get; set; }

        private readonly System.Type[] mDerivedTypes;

        public XmlFileSerializer()
        {
        }

        public XmlFileSerializer(System.Type[] derivedTypes)
        {
            mDerivedTypes = derivedTypes;
        }

        /// <summary>
        /// Read/deserialize the XML from the specified file path name.
        /// </summary>
        /// <param name="xmlFilePathNameArg"></param>
        /// <returns></returns>
        public T ReadFromXmlFile(string xmlFilePathNameArg)
        {
            FileStream fileStream = null;
            T deserializedObject = null;

            try
            {
                var fileInfo = new FileInfo(xmlFilePathNameArg);
                if (fileInfo.Exists)
                {
                    fileStream = fileInfo.OpenRead();
                    if (mDerivedTypes == null)
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        deserializedObject = (T)(serializer.Deserialize(fileStream));
                    }
                    else
                    {
                        var serializer = new XmlSerializer(typeof(T), mDerivedTypes);
                        deserializedObject = (T)(serializer.Deserialize(fileStream));

                    }
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            DeserializedInstance = deserializedObject;
            return deserializedObject;
        }

        /// <summary>
        /// Save/serialize the object to the specified XML file.
        /// </summary>
        /// <param name="xmlFilePathNameArg"></param>
        public void SaveToXmlFile(string xmlFilePathNameArg)
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(xmlFilePathNameArg, false);
                if (mDerivedTypes == null)
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, DeserializedInstance);
                }
                else
                {
                    var serializer = new XmlSerializer(typeof(T), mDerivedTypes);
                    serializer.Serialize(writer, DeserializedInstance);
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
