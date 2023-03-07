using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Statistiche operazione
        /// </summary>
        [Serializable]
        public class CStatisticheOperazione 
            : DMD.XML.DMDBaseXMLObject
        {

            /// <summary>
            /// Numero di contatti
            /// </summary>
            /// <remarks></remarks>
            public int Numero;

            /// <summary>
            /// Parametro
            /// </summary>
            public decimal? Valore;

            /// <summary>
            /// Durata minima (in secondi)
            /// </summary>
            /// <remarks></remarks>
            public double MinLen;

            /// <summary>
            /// Durata massima (in secondi)
            /// </summary>
            /// <remarks></remarks>
            public double MaxLen;

            /// <summary>
            /// Somma delle durate (in secondi)
            /// </summary>
            /// <remarks></remarks>
            public double TotalLen;

            /// <summary>
            /// Somma delle attese (in secondi)
            /// </summary>
            /// <remarks></remarks>
            public double TotalWait;

            /// <summary>
            /// Tempo di attesa minimo (in secondi)
            /// </summary>
            public double MinWait;

            /// <summary>
            /// Tempo di attesa massimo (in secondi)
            /// </summary>
            public double MaxWait;

            /// <summary>
            /// Costo totale
            /// </summary>
            /// <remarks></remarks>
            public decimal TotalCost;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CStatisticheOperazione()
            {
                this.Numero = 0;
                this.Valore = default;
                this.MinLen = 0d;
                this.MaxLen = 0d;
                this.TotalLen = 0d;
                this.TotalWait = 0d;
                this.MinWait = 0d;
                this.MaxWait = 0d;
                this.TotalCost = 0m;
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Numero":
                        {
                            Numero = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            Valore = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MinLen":
                        {
                            MinLen = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MaxLen":
                        {
                            MaxLen = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TotalLen":
                        {
                            TotalLen = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TotalWait":
                        {
                            TotalWait = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MinWait":
                        {
                            MinWait = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MaxWait":
                        {
                            MaxWait = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TotalCost":
                        {
                            TotalCost = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Restitusce un clone dell'oggetto
            /// </summary>
            /// <returns></returns>
            public  CStatisticheOperazione Clone()
            {
                return (CStatisticheOperazione) this.MemberwiseClone();
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Numero", Numero);
                writer.WriteAttribute("Valore", Valore);
                writer.WriteAttribute("MinLen", MinLen);
                writer.WriteAttribute("MaxLen", MaxLen);
                writer.WriteAttribute("TotalLen", TotalLen);
                writer.WriteAttribute("TotalWait", TotalWait);
                writer.WriteAttribute("MinWait", MinWait);
                writer.WriteAttribute("MaxWait", MaxWait);
                writer.WriteAttribute("TotalCost", TotalCost);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Somma i contatori e aggiorna le medie ed i minimi e massimi
            /// </summary>
            /// <param name="altro"></param>
            public void AggregaCon(CStatisticheOperazione altro)
            {
                this.Numero = DMD.Maths.Sum(this.Numero, altro.Numero);
                this.Valore = DMD.Maths.Sum(this.Valore, altro.Valore);
                this.MinLen = DMD.Maths.Min(this.MinLen, altro.MinLen);
                this.MaxLen = DMD.Maths.Max(this.MaxLen, altro.MaxLen);
                this.TotalLen = DMD.Maths.Sum(this.TotalLen, altro.TotalLen);
                this.TotalWait = DMD.Maths.Sum(this.TotalWait, altro.TotalWait);
                this.MinWait = DMD.Maths.Min(this.MinWait, altro.MinWait);
                this.MaxWait = DMD.Maths.Max(this.MaxWait, altro.MaxWait);
                this.TotalCost = DMD.Maths.Sum(this.TotalCost, altro.TotalCost);             
            }
        }
    }
}