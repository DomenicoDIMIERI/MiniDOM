using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Informazioni aggregate sulle pratiche che verificano un filtro
    /// </summary>
    /// <remarks></remarks>
        public class CQSPDMLInfo 
            : IDMDXMLSerializable
        {

            /// <summary>
        /// Numero di pratiche che verificano il filtro
        /// </summary>
        /// <remarks></remarks>
            public int Conteggio;

            /// <summary>
        /// Somma del montante lordo
        /// </summary>
        /// <remarks></remarks>
            public decimal ML;

            /// <summary>
        /// Somma dello spread
        /// </summary>
        /// <remarks></remarks>
            public decimal Spread;

            /// <summary>
        /// Somma dell'UpFront
        /// </summary>
        /// <remarks></remarks>
            public decimal UpFront;

            /// <summary>
        /// Somma del Running
        /// </summary>
        /// <remarks></remarks>
            public decimal Running;

            /// <summary>
        /// Somma degli sconti applicati
        /// </summary>
        /// <remarks></remarks>
            public decimal Sconto;

            /// <summary>
        /// Somma del Rappel
        /// </summary>
        /// <remarks></remarks>
            public decimal Rappel;

            public CQSPDMLInfo()
            {
                DMDObject.IncreaseCounter(this);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            protected void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Conteggio":
                        {
                            Conteggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ML":
                        {
                            ML = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Spread":
                        {
                            Spread = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "UpFront":
                        {
                            UpFront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Sconto":
                        {
                            Sconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Running":
                        {
                            Running = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rappel":
                        {
                            Rappel = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }
                }
            }

            protected void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Conteggio", Conteggio);
                writer.WriteAttribute("ML", ML);
                writer.WriteAttribute("Spread", Spread);
                writer.WriteAttribute("UpFront", UpFront);
                writer.WriteAttribute("Running", Running);
                writer.WriteAttribute("Sconto", Sconto);
                writer.WriteAttribute("Rappel", Rappel);
            }

            ~CQSPDMLInfo()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}