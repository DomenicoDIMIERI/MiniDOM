using System;
using System.Collections;
using DMD.XML;
using DMD.Databases.Collections;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Tabella importata
        /// </summary>
        [Serializable]
        public class XLSImporterTable
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Nome
            /// </summary>
            public string Name;

            /// <summary>
            /// Colonne
            /// </summary>
            public CCollection<XLSImporterColumn> Columns;


            /// <summary>
            /// Costruttore
            /// </summary>
            public XLSImporterTable()
            {
                Name = "";
                Columns = new CCollection<XLSImporterColumn>();
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", Name);
                writer.WriteTag("Columns", Columns);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Name":
                        {
                            Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Columns":
                        {
                            Columns.AddRange((IEnumerable)fieldValue);
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