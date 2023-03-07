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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
        /// Relazione Prodotto - Tabella Finanziaria
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CProdottoXTabellaFin
            : Databases.DBObject 
        {
            private int m_ProdottoID;    // ID del prodotto associato
            [NonSerialized] private CCQSPDProdotto m_Prodotto; // Oggetto Prodotto Associato
            private int m_TabellaID;  // ID della tabella Finanziaria associata
            [NonSerialized] private CTabellaFinanziaria m_Tabella; // Oggetto tabella Finanziaria associata
            [NonSerialized] private CVincoliProdottoTabellaFin m_Vincoli;    // Collezione di vincoli

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProdottoXTabellaFin()
            {
                m_ProdottoID = 0;
                m_Prodotto = null;
                m_TabellaID = 0;
                m_Tabella = null;
                m_Vincoli = null;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return null;
            }

            /// <summary>
            /// ID del prodotto associato
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
                    int oldValue = IDProdotto;
                    if (oldValue == value)
                        return;
                    m_ProdottoID = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            /// <summary>
            /// Oggetto Prodotto Associato
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
                    var oldValue = Prodotto;
                    if (oldValue == value)
                        return;
                    m_Prodotto = value;
                    m_ProdottoID = DBUtils.GetID(value, 0);
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            /// <summary>
            /// ID della tabella Finanziaria associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDTabella
            {
                get
                {
                    return DBUtils.GetID(m_Tabella, m_TabellaID);
                }

                set
                {
                    int oldValue = IDTabella;
                    if (oldValue == value)
                        return;
                    m_TabellaID = value;
                    m_Tabella = null;
                    DoChanged("IDTabella", value, oldValue);
                }
            }

            /// <summary>
            /// Oggetto tabella Finanziaria associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CTabellaFinanziaria Tabella
            {
                get
                {
                    if (m_Tabella is null)
                        m_Tabella = TabelleFinanziarie.GetItemById(m_TabellaID);
                    return m_Tabella;
                }

                set
                {
                    var oldValue = Tabella;
                    if (oldValue == value)
                        return;
                    m_Tabella = value;
                    m_TabellaID = DBUtils.GetID(value, 0);
                    DoChanged("Tabella", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la tabella finanziaria
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetTabella(CTabellaFinanziaria value)
            {
                m_Tabella = value;
                m_TabellaID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Vincoli
            /// </summary>
            public CVincoliProdottoTabellaFin Vincoli
            {
                get
                {
                    if (m_Vincoli is null)
                    {
                        m_Vincoli = new CVincoliProdottoTabellaFin();
                        m_Vincoli.Initialize(this);
                    }

                    return m_Vincoli;
                }
            }

            /// <summary>
            /// Controlla che la relazione sia applicazione
            /// </summary>
            /// <param name="offerta"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public virtual bool Check(COffertaCQS offerta)
            {
                return Vincoli.Check(offerta);
            }

            /// <summary>
            /// Calcola
            /// </summary>
            /// <param name="offerta"></param>
            public void Calcola(COffertaCQS offerta)
            {
                if (offerta is null)
                    throw new ArgumentNullException("offerta");
                var pCoeffBase = Tabella.get_CoefficienteBase(offerta.Durata);
                var pCommissBanc = Tabella.get_CommissioniBancarie(offerta.Durata);
                // Calcoliamo le commissioni bancarie
                if (pCommissBanc.HasValue)
                {
                    offerta.CommissioniBancarie = (decimal)((double)offerta.MontanteLordo * pCommissBanc.Value / 100d);
                }
                else
                {
                    offerta.CommissioniBancarie = 0m;
                }
                // Calcoliamo gli interessi come differenza rispetto al coefficiente base 
                if (pCoeffBase.HasValue)
                {
                    offerta.Interessi = (decimal)((double)offerta.MontanteLordo * pCoeffBase.Value / 100d - (double)offerta.CommissioniBancarie);
                }
                // Aggiungiamo lo spread base alle commissioni bancarie
                offerta.CommissioniBancarie = offerta.CommissioniBancarie + (decimal)((double)offerta.MontanteLordo * (double)offerta.SpreadBase / 100d);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabFin";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (!ret && m_Vincoli is object)
                    ret = DBUtils.IsChanged(m_Vincoli);
                return ret;
            }

            /// <summary>
            /// Salve l'ogetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (ret && m_Vincoli is object)
                    m_Vincoli.Save(force);
                return ret;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_ProdottoID = reader.Read("Prodotto", this.m_ProdottoID);
                m_TabellaID = reader.Read("Tabella", this.m_TabellaID);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Prodotto", DBUtils.GetID(m_Prodotto, m_ProdottoID));
                writer.Write("Tabella", DBUtils.GetID(m_Tabella, m_TabellaID));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDProdotto", IDProdotto);
                writer.WriteAttribute("IDTabella", IDTabella);
                base.XMLSerialize(writer);
                writer.WriteTag("Vincoli", Vincoli.ToArray());
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
                    case "IDProdotto": this.m_ProdottoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "IDTabella": this.m_TabellaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "Vincoli":
                        {
                            Vincoli.Clear();
                            if (DMD.Arrays.IsArray(fieldValue))
                            {
                                Vincoli.AddRange((IEnumerable)fieldValue);
                            }
                            else if (fieldValue is CProdTabFinConstraint)
                            {
                                Vincoli.Add((CProdTabFinConstraint)fieldValue);
                            }

                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Imposta il prodotto
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                m_ProdottoID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CProdottoXTabellaFin Clone()
            {
                return (CProdottoXTabellaFin)base._Clone();
            }
        }
    }
}