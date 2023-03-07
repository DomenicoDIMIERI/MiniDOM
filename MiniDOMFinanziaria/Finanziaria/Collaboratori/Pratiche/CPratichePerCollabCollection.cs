
namespace minidom
{
    public partial class Finanziaria
    {
        public class CPratichePerCollabCollection : CPraticheCollection
        {
            private CCollaboratore m_Collaboratore;

            public CPratichePerCollabCollection()
            {
            }

            public CCollaboratore Collaboratore
            {
                get
                {
                    return m_Collaboratore;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                // If (Me.m_Collaboratore IsNot Nothing) Then DirectCast(value, CPraticaCQSPD).colla.Info.Produttore = Me.m_Collaboratore
                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CCollaboratore value)
            {
                var cursor = new CPraticheCQSPDCursor();
                Clear();
                m_Collaboratore = value;
                // cursor.IDProduttore.Value = GetID(value)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.CreatoIl.SortOrder = SortEnum.SORT_ASC;
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return true;
            }
        }
    }
}