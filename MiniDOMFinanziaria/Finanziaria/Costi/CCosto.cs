
namespace minidom
{
    public partial class Finanziaria
    {
        public class CCosto : Databases.DBObjectBase
        {
            private double m_FinoA;
            private decimal m_Costo;

            public CCosto()
            {
                m_FinoA = 0d;
                m_Costo = 0m;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public double FinoA
            {
                get
                {
                    return m_FinoA;
                }

                set
                {
                    double oldValue = m_FinoA;
                    if (oldValue == value)
                        return;
                    m_FinoA = value;
                    DoChanged("FinoA", value, oldValue);
                }
            }

            public decimal Costo
            {
                get
                {
                    return m_Costo;
                }

                set
                {
                    decimal oldValue = m_Costo;
                    if (oldValue == value)
                        return;
                    m_Costo = value;
                    DoChanged("Costo", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "Costi";
            }

            protected override string GetIDFieldName()
            {
                return "idcosto";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_FinoA = reader.Read("FinoA", this.m_FinoA);
                this.m_Costo = reader.Read("Costo", this.m_Costo);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("FinoA", m_FinoA);
                writer.Write("Costo", m_Costo);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return "FinoA: " + m_FinoA + " -> " + Sistema.Formats.FormatValuta(m_Costo);
            }
        }
    }
}