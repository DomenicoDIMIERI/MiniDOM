
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Insieme delle righe della tabella dei TEG Massimi
    /// </summary>
    /// <remarks></remarks>
        public class CQSPDRigheTEGMax : CCollection<CRigaTEGMax>
        {
            private CTabellaTEGMax m_Owner;

            public CQSPDRigheTEGMax()
            {
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CRigaTEGMax)value).Tabella = m_Owner;
                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CTabellaTEGMax owner)
            {
                string dbSQL;
                CRigaTEGMax item;
                Clear();
                m_Owner = owner;
                dbSQL = "SELECT * FROM [tbl_FIN_TEGMaxI] WHERE ([Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + ") And ([Tabella]=" + DBUtils.GetID(m_Owner) + ");";
                var reader = new DBReader(Database.Tables["tbl_FIN_TEGMaxI"], dbSQL);
                while (reader.Read())
                {
                    item = new CRigaTEGMax();
                    if (Database.Load(item, reader))
                        Add(item);
                }

                reader.Dispose();
                return true;
            }

            public bool Check(COffertaCQS offerta)
            {
                bool ret = true;
                int i = 0;
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