using System;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Riga importata
        /// </summary>
        [Serializable]
        public class XLSImporterResult 
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Stato
            /// </summary>
            public string RowStatus;


            /// <summary>
            /// Costruttore
            /// </summary>
            public XLSImporterResult()
            {
                RowStatus = "";
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("RowStatus", RowStatus);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "RowStatus":
                        {
                            RowStatus = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }
        }
    }
}