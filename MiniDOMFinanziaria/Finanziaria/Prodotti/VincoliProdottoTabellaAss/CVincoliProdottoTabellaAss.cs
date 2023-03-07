
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Insieme di vincoli definiti per una triplae di tabelle assicurative
    /// </summary>
    /// <remarks></remarks>
        public class CVincoliProdottoTabellaAss : CCollection<CProdTabAssConstraint>
        {
            private CProdottoXTabellaAss m_Owner;

            public CVincoliProdottoTabellaAss()
            {
                m_Owner = null;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CProdTabAssConstraint)value).SetOwner(m_Owner);
                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CProdottoXTabellaAss owner)
            {
                string dbSQL;
                CProdTabAssConstraint item;
                Clear();
                m_Owner = owner;
                dbSQL = "SELECT * FROM [tbl_FIN_ProdXTabAssConstr] WHERE ([Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + ") And ([Owner]=" + DBUtils.GetID(m_Owner) + ");";
                var reader = new DBReader(Database.Tables["tbl_FIN_ProdXTabAssConstr"], dbSQL);
                while (reader.Read())
                {
                    item = new CProdTabAssConstraint();
                    if (Database.Load(item, reader))
                        Add(item);
                }

                reader.Dispose();
                return true;
            }

            public bool Check(COffertaCQS offerta)
            {
                int i;
                bool ret = true;
                i = 0;
                while (i < Count & ret)
                {
                    ret = ret & this[i].Check(offerta);
                    i = i + 1;
                }

                return ret;
            }
        }
    }
}