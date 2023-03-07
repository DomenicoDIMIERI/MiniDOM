using System;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Colonna importata
        /// </summary>
        [Serializable]
        public class XLSImporterColumn 
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Nome della colonna
            /// </summary>
            public string SourceName;

            /// <summary>
            /// Tipo dei dati nella colonna
            /// </summary>
            public TypeCode SourceDataType;

            /// <summary>
            /// Nome del campo di destinazione suggerito
            /// </summary>
            public string SuggestedTargetField;

            /// <summary>
            /// Nome del campo destinazione
            /// </summary>
            public string TargetField;

            /// <summary>
            /// Se vero indica che la colonna deve essere importata
            /// </summary>
            public bool DoImport;

            /// <summary>
            /// Costruttore
            /// </summary>
            public XLSImporterColumn()
            {
                SourceName = "";
                SourceDataType = TypeCode.Empty;
                SuggestedTargetField = "";
                TargetField = "";
                DoImport = true;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("SourceName", SourceName);
                writer.WriteAttribute("SourceDataType", (int?)SourceDataType);
                writer.WriteAttribute("SuggestedTargetField", SuggestedTargetField);
                writer.WriteAttribute("TargetField", TargetField);
                writer.WriteAttribute("DoImport", DoImport);
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
                    case "SourceName":
                        {
                            SourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceDataType":
                        {
                            SourceDataType = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SuggestedTargetField":
                        {
                            SuggestedTargetField = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TargetField":
                        {
                            TargetField = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DoImport":
                        {
                            DoImport = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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