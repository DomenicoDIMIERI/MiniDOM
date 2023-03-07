
namespace minidom
{
    public partial class Finanziaria
    {
        public class CProdTabAssConstraintCursor : CTableConstraintCursor
        {
            private DBCursorField<int> m_OwnerID = new DBCursorField<int>("Owner");

            public CProdTabAssConstraintCursor()
            {
            }

            public DBCursorField<int> OwnerID
            {
                get
                {
                    return m_OwnerID;
                }
            }

            public new CProdTabAssConstraint Item
            {
                get
                {
                    return (CProdTabAssConstraint)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabAssConstr";
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProdTabAssConstraint();
            }
        }
    }
}