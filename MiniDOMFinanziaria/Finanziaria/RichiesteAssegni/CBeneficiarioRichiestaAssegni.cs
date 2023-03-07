using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CBeneficiarioRichiestaAssegni : Databases.DBObjectBase
        {
            private int m_RichiestaID;
            private CRichiestaAssegni m_Richiesta;
            private string m_Nome;
            private string m_Field;
            private decimal m_Importo;
            private int m_Position;

            public CBeneficiarioRichiestaAssegni()
            {
                m_Richiesta = null;
            }

            public override CModulesClass GetModule()
            {
                return RichiesteAssegni.Module;
            }

            protected internal void SetRichiesta(CRichiestaAssegni value)
            {
                m_Richiesta = value;
                m_RichiestaID = DBUtils.GetID(value);
            }

            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            public string Field
            {
                get
                {
                    return m_Field;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Field;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Field = value;
                    DoChanged("Field", value, oldValue);
                }
            }

            public decimal Importo
            {
                get
                {
                    return m_Importo;
                }

                set
                {
                    decimal oldValue = m_Importo;
                    if (oldValue == value)
                        return;
                    m_Importo = value;
                    DoChanged("Importo", value, oldValue);
                }
            }

            public int Position
            {
                get
                {
                    return m_Position;
                }

                set
                {
                    int oldValue = m_Position;
                    if (oldValue == value)
                        return;
                    m_Position = value;
                    DoChanged("Position", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_RichiestaAssegniCircolariBeneficiari";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("NomeBeneficiario", this.m_Nome);
                this.m_Field = reader.Read("Campo1", this.m_Field);
                this.m_Importo = reader.Read("Importo", this.m_Importo);
                this.m_Position = reader.Read("Posizione", this.m_Position);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("NomeBeneficiario", m_Nome);
                writer.Write("Campo1", m_Field);
                writer.Write("Importo", m_Importo);
                writer.Write("Posizione", m_Position);
                writer.Write("Richiesta", DBUtils.GetID(m_Richiesta, m_RichiestaID));
                return base.SaveToRecordset(writer);
            }
        }
    }
}