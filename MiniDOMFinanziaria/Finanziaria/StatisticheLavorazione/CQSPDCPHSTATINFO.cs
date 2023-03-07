using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CQSPDCPHSTATINFO
            : IDMDXMLSerializable
        {
            public int IDOperatore;
            public string NomeOperatore;
            public int Numero;
            public decimal ML;
            public double Minimo;
            public double Massimo;
            public double Media;

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }


            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
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

                    case "Numero":
                        {
                            Numero = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ML":
                        {
                            ML = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Minimo":
                        {
                            Minimo = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Massimo":
                        {
                            Massimo = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Media":
                        {
                            Media = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", NomeOperatore);
                writer.WriteAttribute("Numero", Numero);
                writer.WriteAttribute("ML", ML);
                writer.WriteAttribute("Minimo", Minimo);
                writer.WriteAttribute("Massimo", Massimo);
                writer.WriteAttribute("Media", Media);
            }
        }
    }
}