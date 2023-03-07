using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Statistiche
        /// </summary>
        [Serializable]
        public class RUStats 
            : DMD.XML.DMDBaseXMLObject
        {
            /// <summary>
            /// 
            /// </summary>
            public CKeyCollection<RUStatItem> items;

            /// <summary>
            /// Commissioni svolte
            /// </summary>
            public CKeyCollection<Commissione> Commissioni;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RUStats()
            {
                this.items = new CKeyCollection<RUStatItem>();
                this.Commissioni = new CKeyCollection<Commissione>();
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                Commissioni.Clear();
                foreach (RUStatItem item in items)
                {
                    foreach (Uscita u in item.Uscite)
                    {
                        foreach (CommissionePerUscita cxu in u.Commissioni)
                        {
                            if (!Commissioni.ContainsKey("C" + cxu.IDCommissione))
                            {
                                Commissioni.Add("C" + cxu.IDCommissione, cxu.Commissione);
                            }
                        }
                    }
                }
                base.XMLSerialize(writer);
                writer.WriteTag("items", items);
                writer.WriteTag("Commissioni", Commissioni);
                
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
                    case "items":
                        {
                            items.Clear();
                            var tmp = (CKeyCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            foreach (RUStatItem item in tmp)
                                items.Add(item.NomeOperatore, item);
                            break;
                        }

                    case "Commissioni":
                        {
                            Commissioni.Clear();
                            var tmp = (CKeyCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            foreach (string key in tmp.Keys)
                                Commissioni.Add(key, (Commissione)tmp[key]);
                            SincronizzaCommissioni();
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
                
            }

            private void SincronizzaCommissioni()
            {
                foreach (RUStatItem item in items)
                {
                    foreach (Uscita u in item.Uscite)
                    {
                        foreach (CommissionePerUscita cxu in u.Commissioni)
                            cxu.SetCommissione(Commissioni.GetItemByKey("C" + cxu.IDCommissione));
                    }
                }
            }

             
        }
    }
}