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
        /// Statistiche sulle uscite di un operatore per un punto operativo
        /// </summary>
        [Serializable]
        public class RUStatItem 
            : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<RUStatItem>
        {
            /// <summary>
            /// ID dell'operatore
            /// </summary>
            public int UserID;

            /// <summary>
            /// Nome dell'operatore
            /// </summary>
            public string UserName;

            /// <summary>
            /// Nome dell'operatore
            /// </summary>
            public string NomeOperatore;

            /// <summary>
            /// Collezione di uscite
            /// </summary>
            [NonSerialized] public CCollection<Uscita> Uscite;

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="operatore"></param>
            public RUStatItem(Sistema.CUser operatore)
            {
                this.UserID = DBUtils.GetID(operatore, 0);
                this.UserName = operatore.UserName;
                this.NomeOperatore = operatore.Nominativo;
                this.Uscite = new CCollection<Uscita>();
            }

            /// <summary>
            /// Compara due elementi in base al NomeOperatore
            /// </summary>
            /// <param name="withBlock"></param>
            /// <returns></returns>
            int CompareTo(RUStatItem withBlock)
            {
                return DMD.Strings.Compare(NomeOperatore, withBlock.NomeOperatore, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((RUStatItem)obj);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "UserID":
                        {
                            UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserName":
                        {
                            UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Uscite":
                        {
                            Uscite = (CCollection<Uscita>)DMD.XML.Utils.Serializer.ToObject(fieldValue);
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
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("UserName", UserName);
                writer.WriteAttribute("NomeOperatore", NomeOperatore);
                base.XMLSerialize(writer);
                writer.WriteTag("Uscite", Uscite);
            }

        }
    }
}