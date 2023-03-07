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
    /// Valori enumerativi utilizzati per definire la relazione tra un profilo ed un prodotto
    /// </summary>
    /// <remarks></remarks>
        public enum IncludeModes : int
        {
            /// <summary>
        /// Eredita il prodotto dal genitore
        /// </summary>
        /// <remarks></remarks>
            Eredita = 0,
            /// <summary>
        /// Forza l'inclusione del prodotto
        /// </summary>
        /// <remarks></remarks>
            Include = 1,
            /// <summary>
        /// Forza l'esclusione del prodotto
        /// </summary>
        /// <remarks></remarks>
            Escludi = -1
        }

        /// <summary>
    /// Rappresenta l'associazione tra un prodotto ed un listino
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CProdottoProfilo 
            : Databases.DBObject
        {
            private int m_ProfiloID;
            [NonSerialized] private CProfilo m_Profilo;
            private int m_ProdottoID;
            [NonSerialized] private CCQSPDProdotto m_Prodotto;
            private IncludeModes m_Azione;
            private double m_Spread;

            public CProdottoProfilo()
            {
                m_ProdottoID = 0;
                m_Profilo = null;
                m_ProdottoID = 0;
                m_Prodotto = null;
                m_Azione = IncludeModes.Include;
                m_Spread = 0d;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            /// <summary>
        /// Restituisce o imposta l'ID del profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDProfilo
            {
                get
                {
                    return DBUtils.GetID(m_Profilo, m_ProfiloID);
                }

                set
                {
                    int oldValue = IDProfilo;
                    if (oldValue == value)
                        return;
                    m_ProfiloID = value;
                    m_Profilo = null;
                    DoChanged("IDProfilo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto Profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfilo Profilo
            {
                get
                {
                    if (m_Profilo is null)
                        m_Profilo = Profili.GetItemById(m_ProfiloID);
                    return m_Profilo;
                }

                set
                {
                    var oldValue = m_Profilo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Profilo = value;
                    m_ProfiloID = DBUtils.GetID(value);
                    DoChanged("Profilo", value, oldValue);
                }
            }

            protected internal void SetProfilo(CProfilo value)
            {
                m_Profilo = value;
                m_ProfiloID = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID del prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDProdotto
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_ProdottoID);
                }

                set
                {
                    int oldValue = IDProfilo;
                    if (oldValue == value)
                        return;
                    m_ProdottoID = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto Prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDProdotto Prodotto
            {
                get
                {
                    if (m_Prodotto is null)
                        m_Prodotto = Prodotti.GetItemById(m_ProdottoID);
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = m_Prodotto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Prodotto = value;
                    m_ProdottoID = DBUtils.GetID(value);
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            protected internal void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                m_ProdottoID = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il tipo di relazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public IncludeModes Azione
            {
                get
                {
                    return m_Azione;
                }

                set
                {
                    var oldValue = m_Azione;
                    if (oldValue == value)
                        return;
                    m_Azione = value;
                    DoChanged("Azione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo spread da aggiugnere al prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Spread
            {
                get
                {
                    return m_Spread;
                }

                set
                {
                    double oldValue = m_Spread;
                    if (oldValue == value)
                        return;
                    m_Spread = value;
                    DoChanged("Spread", value, oldValue);
                }
            }

            public string NomeProdotto
            {
                get
                {
                    if (Prodotto is null)
                        return "IVALID: ID=" + IDProdotto;
                    return Prodotto.Nome;
                }
            }

            public string NomeProfilo
            {
                get
                {
                    if (Profilo is null)
                        return "IVALID: ID=" + IDProfilo;
                    return Profilo.Nome;
                }
            }

            public override string ToString()
            {
                switch (Azione)
                {
                    case IncludeModes.Eredita:
                        {
                            return "Eredita " + NomeProdotto + " (" + Spread + ")";
                        }

                    case IncludeModes.Include:
                        {
                            return "Includi " + NomeProdotto + " (" + Spread + ")";
                        }

                    case IncludeModes.Escludi:
                        {
                            return "Escludi " + NomeProdotto;
                        }

                    default:
                        {
                            return "???";
                        }
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_PreventivatoriXProdotto";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_ProfiloID = reader.Read("Preventivatore", this.m_ProfiloID);
                m_ProdottoID = reader.Read("Prodotto", this.m_ProdottoID);
                m_Azione = reader.Read("Azione", this.m_Azione);
                m_Spread = reader.Read("Spread", this.m_Spread);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Preventivatore", DBUtils.GetID(m_Profilo, m_ProfiloID));
                writer.Write("Prodotto", DBUtils.GetID(m_Prodotto, m_ProdottoID));
                writer.Write("Azione", m_Azione);
                writer.Write("Spread", m_Spread);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDProfilo", IDProfilo);
                writer.WriteAttribute("IDProdotto", IDProdotto);
                writer.WriteAttribute("Azione", (int?)m_Azione);
                writer.WriteAttribute("Spread", m_Spread);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDProfilo":
                        {
                            m_ProfiloID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDProdotto":
                        {
                            m_ProdottoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Azione":
                        {
                            m_Azione = (IncludeModes)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Spread":
                        {
                            m_Spread = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                // Dim p As CProfilo = Me.Profilo
                // p.ProdottiXProfiloRelations.Update(Me)
                Profili.InvalidateProdottiProfilo();
            }
        }
    }
}