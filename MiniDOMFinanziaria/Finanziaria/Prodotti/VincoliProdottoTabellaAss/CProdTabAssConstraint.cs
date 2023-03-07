
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Vincoli su una tabella assicurativa
    /// </summary>
    /// <remarks></remarks>
        public class CProdTabAssConstraint : CTableConstraint
        {
            private int m_OwnerID;
            private CProdottoXTabellaAss m_Owner;

            public CProdTabAssConstraint()
            {
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public int OwnerID
            {
                get
                {
                    return DBUtils.GetID(m_Owner, m_OwnerID);
                }

                set
                {
                    int oldValue = OwnerID;
                    if (oldValue == value)
                        return;
                    m_Owner = null;
                    m_OwnerID = value;
                    DoChanged("OwnerID", value, oldValue);
                }
            }

            public CProdottoXTabellaAss Owner
            {
                get
                {
                    if (m_Owner is null)
                        m_Owner = TabelleAssicurative.GetTabellaXProdottoByID(m_OwnerID);
                    return m_Owner;
                }

                set
                {
                    var oldValue = Owner;
                    if (oldValue == value)
                        return;
                    SetOwner(value);
                    DoChanged("Owner", value, oldValue);
                }
            }

            protected internal void SetOwner(CProdottoXTabellaAss value)
            {
                m_Owner = value;
                m_OwnerID = DBUtils.GetID(value);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabAssConstr";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_OwnerID = reader.Read("Owner", this.m_OwnerID);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Owner", DBUtils.GetID(m_Owner, m_OwnerID));
                return base.SaveToRecordset(writer);
            }
        }
    }
}