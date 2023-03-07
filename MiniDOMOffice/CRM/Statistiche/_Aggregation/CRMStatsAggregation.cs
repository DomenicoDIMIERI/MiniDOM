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
        /// Aggregazione delle statistiche crm
        /// </summary>
        [Serializable]
        public class CRMStatsAggregation 
            : DMD.XML.DMDBaseXMLObject
        {

            /// <summary>
            /// ID dell'operatore
            /// </summary>
            public int IDOperatore;

            /// <summary>
            /// ID del punto operativo
            /// </summary>
            public int IDPuntoOperativo;

            /// <summary>
            /// Data delle statistiche
            /// </summary>
            public DateTime Data;

            /// <summary>
            /// Effettuate
            /// </summary>
            public CStatisticheOperazione Effettuate;

            /// <summary>
            /// Ricevute
            /// </summary>
            public CStatisticheOperazione Ricevute;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatsAggregation()
            {
                IDOperatore = 0;
                IDPuntoOperativo = 0;
                Data = DMD.DateUtils.ToDay();
                Effettuate = new CStatisticheOperazione();
                Ricevute = new CStatisticheOperazione();
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
                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativo":
                        {
                            IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Effettuate":
                        {
                            Effettuate = (CStatisticheOperazione)fieldValue;
                            break;
                        }

                    case "Ricevute":
                        {
                            Ricevute = (CStatisticheOperazione)fieldValue;
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("Data", Data);
                base.XMLSerialize(writer);
                writer.WriteTag("Effettuate", Effettuate);
                writer.WriteTag("Ricevute", Ricevute);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", this.IDPuntoOperativo, " - ", this.IDOperatore, " - ", this.Data, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Data);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(DMDBaseXMLObject obj)
            {
                return (obj is CRMStatsAggregation) && this.Equals((CRMStatsAggregation)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRMStatsAggregation obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.IDOperatore, obj.IDOperatore)
                    && DMD.Integers.EQ(this.IDPuntoOperativo, obj.IDPuntoOperativo)
                    && DMD.DateUtils.EQ(this.Data, obj.Data)
                    ;
            }


        }
    }
}