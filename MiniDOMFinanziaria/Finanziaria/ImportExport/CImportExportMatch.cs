using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Rappresenta una corrispondenza tra i due sistemi
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CImportExportMatch : IDMDXMLSerializable
        {
            public string Tipo;
            public int IDOrigine;
            public int IDDestinazione;

            public CImportExportMatch()
            {
                DMDObject.IncreaseCounter(this);
                Tipo = "";
                IDOrigine = 0;
                IDDestinazione = 0;
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Tipo":
                        {
                            Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOrigine":
                        {
                            IDOrigine = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDestinazione":
                        {
                            IDDestinazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Tipo", Tipo);
                writer.WriteAttribute("IDOrigine", IDOrigine);
                writer.WriteAttribute("IDDestinazione", IDDestinazione);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            ~CImportExportMatch()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}