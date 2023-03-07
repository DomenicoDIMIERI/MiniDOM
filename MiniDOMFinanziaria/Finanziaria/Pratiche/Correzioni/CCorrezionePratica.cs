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
    /// Rappresenta dei dati aggiuntivi per la pratica (relazione 1 a 1)
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CCorrezionePratica 
            : DMDObject, IDMDXMLSerializable
        {
            public DateTime Data;
            public int IDOperatore;
            public string NomeOperatore;
            public string NomeCampo;
            public TypeCode TipoValore;
            public string VecchioValore;
            public string NuovoValore;
            public string Note;

            public CCorrezionePratica()
            {
                Data = DMD.DateUtils.Now();
                IDOperatore = 0;
                NomeOperatore = "";
                NomeCampo = "";
                TipoValore = TypeCode.Empty;
                VecchioValore = "";
                NuovoValore = "";
                Note = "";
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer);  }
            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }

            protected void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", Data);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", NomeOperatore);
                writer.WriteAttribute("NomeCampo", NomeCampo);
                writer.WriteAttribute("TipoValore", (int?)TipoValore);
                writer.WriteAttribute("VecchioValore", VecchioValore);
                writer.WriteAttribute("NuovoValore", NuovoValore);
                writer.WriteAttribute("Note", Note);
            }

            protected void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Data":
                        {
                            Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeCampo":
                        {
                            NomeCampo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoValore":
                        {
                            TipoValore = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "VecchioValore":
                        {
                            VecchioValore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NuovoValore":
                        {
                            NuovoValore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}