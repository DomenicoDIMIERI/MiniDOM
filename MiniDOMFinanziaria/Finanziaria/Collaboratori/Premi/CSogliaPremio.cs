using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CSogliaPremio : Databases.DBObject
        {
            [NonSerialized]
            private CSetPremi m_SetPremi;
            private int m_SetPremiID;
            private decimal m_Soglia; // Soglia
            private decimal m_Fisso;
            private double m_PercSuML; // Percentuale su montante lordo
            private double m_PercSuProvvAtt; // Percentuale su provvigione attiva
            private double m_PercSuNetto; // Percentuale su netto ricavo

            public CSogliaPremio()
            {
                m_SetPremiID = 0;
                m_SetPremi = null;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public CSetPremi SetPremi
            {
                get
                {
                    return m_SetPremi;
                }
            }

            protected internal void SetSetPremi(CSetPremi value)
            {
                m_SetPremi = value;
                m_SetPremiID = DBUtils.GetID(value);
            }

            public decimal Soglia
            {
                get
                {
                    return m_Soglia;
                }

                set
                {
                    decimal oldValue = m_Soglia;
                    if (oldValue == value)
                        return;
                    m_Soglia = value;
                    DoChanged("Soglia", value, oldValue);
                }
            }

            public decimal Fisso
            {
                get
                {
                    return m_Fisso;
                }

                set
                {
                    decimal oldValue = m_Fisso;
                    if (oldValue == value)
                        return;
                    m_Fisso = value;
                    DoChanged("Fisso", value, oldValue);
                }
            }

            public double PercSuML
            {
                get
                {
                    return m_PercSuML;
                }

                set
                {
                    double oldValue = m_PercSuML;
                    if (oldValue == value)
                        return;
                    m_PercSuML = value;
                    DoChanged("PercSuML", value, oldValue);
                }
            }

            public double PercSuProvvAtt
            {
                get
                {
                    return m_PercSuProvvAtt;
                }

                set
                {
                    double oldValue = m_PercSuProvvAtt;
                    if (oldValue == value)
                        return;
                    m_PercSuProvvAtt = value;
                    DoChanged("PercSuProvvAtt", value, oldValue);
                }
            }

            public double PercSuNetto
            {
                get
                {
                    return m_PercSuNetto;
                }

                set
                {
                    double oldValue = m_PercSuNetto;
                    if (oldValue == value)
                        return;
                    m_PercSuNetto = value;
                    DoChanged("PercSuNetto", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_TeamManagers_SogliePremi";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_SetPremiID = reader.Read("SetPremi", this.m_SetPremiID);
                this.m_Soglia = reader.Read("Soglia", this.m_Soglia);
                this.m_Fisso = reader.Read("Fisso", this.m_Fisso);
                this.m_PercSuML = reader.Read("PercSuML", this.m_PercSuML);
                this.m_PercSuProvvAtt = reader.Read("PercSuProvvAtt", this.m_PercSuProvvAtt);
                this.m_PercSuNetto = reader.Read("PercSuNetto", this.m_PercSuNetto);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("SetPremi", DBUtils.GetID(m_SetPremi, m_SetPremiID));
                writer.Write("Soglia", m_Soglia);
                writer.Write("Fisso", m_Fisso);
                writer.Write("PercSuML", m_PercSuML);
                writer.Write("PercSuProvvAtt", m_PercSuProvvAtt);
                writer.Write("PercSuNetto", m_PercSuNetto);
                return base.SaveToRecordset(writer);
            }
        }
    }
}