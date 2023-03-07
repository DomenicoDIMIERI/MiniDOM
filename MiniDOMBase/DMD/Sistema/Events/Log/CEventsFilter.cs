using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Filtro sugli eventi
        /// </summary>
        [Serializable]
        public class CEventsFilter 
            : DMD.XML.DMDBaseXMLObject 
        {
            /// <summary>
            /// Data iniziale
            /// </summary>
            public DateTime? fromDate;

            /// <summary>
            /// Data finale
            /// </summary>
            public DateTime? toDate;

            /// <summary>
            /// ID dell'Operatore che ha generato l'evento
            /// </summary>
            public int operatorID;

            /// <summary>
            /// Nome dell'operatore che ha generato l'evento
            /// </summary>
            public string operatorName;

            /// <summary>
            /// Nome del modulo su cui è stato generato l'evento
            /// </summary>
            public string moduleName;

            /// <summary>
            /// Descrizione dell'evento
            /// </summary>
            public string text;

            /// <summary>
            /// Numero massimo di risultati da mostrare
            /// </summary>
            public int maxCount;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEventsFilter()
            {
                var t = DMD.DateUtils.Now();
                fromDate = DMD.DateUtils.MakeDate(DMD.DateUtils.Year(t), DMD.DateUtils.Month(t), DMD.DateUtils.Day(t), DMD.DateUtils.Hour(t) - 8, 0, 0);
                toDate = default;
                operatorID = 0;
                operatorName = "";
                moduleName = "";
                text = "";
                maxCount = 10;
            }

           
            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("fromDate", fromDate);
                writer.WriteAttribute("toDate", toDate);
                writer.WriteAttribute("operatorID", operatorID);
                writer.WriteAttribute("operatorName", operatorName);
                writer.WriteAttribute("moduleName", moduleName);
                writer.WriteAttribute("text", text);
                writer.WriteAttribute("maxCount", maxCount);
                base.XMLSerialize(writer);
                
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (DMD.Strings.LCase(DMD.Strings.Trim(fieldName)) ?? "")
                {
                    case "fromdate":
                        {
                            fromDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "todate":
                        {
                            toDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "operatorid":
                        {
                            operatorID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "operatorname":
                        {
                            operatorName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "modulename":
                        {
                            moduleName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "text":
                        {
                            text = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "maxcount":
                        {
                            maxCount = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
 
         
        }
    }
}