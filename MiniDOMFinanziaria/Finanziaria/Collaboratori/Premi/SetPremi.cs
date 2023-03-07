using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using System.Collections;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum TipoCalcoloSetPremi : int
        {
            SU_PROVVIGIONEATTIVA = 0,
            SU_MONTANTELORDO = 1,
            SU_NETTORICAVO = 2
        }

        public enum TipoIntervalloSetPremi : int
        {
            Mensile = 0,
            Settimanale = 1,
            Annuale = 2
        }

        [Serializable]
        public class CSetPremi : Databases.DBObject, IComparer
        {
            private bool m_AScaglioni;
            private TipoIntervalloSetPremi m_TipoIntervallo;
            private TipoCalcoloSetPremi m_TipoCalcolo;
            private SogliePremioCollection m_Items;

            public CSetPremi()
            {
                m_AScaglioni = true;
                m_Items = null;
                m_TipoIntervallo = TipoIntervalloSetPremi.Mensile;
                m_TipoCalcolo = TipoCalcoloSetPremi.SU_PROVVIGIONEATTIVA;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public TipoIntervalloSetPremi TipoIntervallo
            {
                get
                {
                    return m_TipoIntervallo;
                }

                set
                {
                    var oldValue = m_TipoIntervallo;
                    if (oldValue == value)
                        return;
                    m_TipoIntervallo = value;
                    DoChanged("TipoIntervallo", value, oldValue);
                }
            }

            public TipoCalcoloSetPremi TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }

                set
                {
                    var oldValue = m_TipoCalcolo;
                    m_TipoCalcolo = value;
                    DoChanged("TipoCalcolo", value, oldValue);
                }
            }

            public bool AScaglioni
            {
                get
                {
                    return m_AScaglioni;
                }

                set
                {
                    if (m_AScaglioni == value)
                        return;
                    m_AScaglioni = value;
                    DoChanged("AScaglioni", value, !value);
                }
            }

            public SogliePremioCollection Scaglioni
            {
                get
                {
                    if (m_Items is null)
                        m_Items = new SogliePremioCollection(this);
                    return m_Items;
                }
            }

            public int Compare(object a, object b)
            {
                int ret = 0;
                CSogliaPremio item1 = (CSogliaPremio)a;
                CSogliaPremio item2 = (CSogliaPremio)b;
                if (item1.Soglia < item2.Soglia)
                {
                    ret = -1;
                }
                else if (item1.Soglia > item2.Soglia)
                {
                    ret = 1;
                }

                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (m_Items is object)
                    m_Items.Save(force);
                return ret;
            }

            public override string GetTableName()
            {
                return "tbl_TeamManagers_SetPremi";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_TipoIntervallo = (TipoIntervalloSetPremi)reader.GetValue<int>("TipoIntervallo", 0);
                m_TipoCalcolo = (TipoCalcoloSetPremi)reader.GetValue<int>("TipoCalcolo", 0);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("TipoIntervallo", m_TipoIntervallo);
                writer.Write("TipoCalcolo", m_TipoCalcolo);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("TipoIntervallo", (int?)m_TipoIntervallo);
                writer.WriteAttribute("TipoCalcolo", (int?)m_TipoCalcolo);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "TipoIntervallo":
                        {
                            m_TipoIntervallo = (TipoIntervalloSetPremi)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCalcolo":
                        {
                            m_TipoCalcolo = (TipoCalcoloSetPremi)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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