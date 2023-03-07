using System;
using System.Data;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CBeneficiariCollection : CCollection<CBeneficiarioRichiestaAssegni>
        {
            private CRichiestaAssegni m_Richiesta;

            public CBeneficiariCollection()
            {
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Richiesta is object)
                    ((CBeneficiarioRichiestaAssegni)value).SetRichiesta(m_Richiesta);
                base.OnInsert(index, value);
            }

            protected internal virtual bool Load(CRichiestaAssegni richiesta)
            {
                if (richiesta is null)
                    throw new ArgumentNullException("richiesta");
                Clear();
                m_Richiesta = richiesta;
                if (DBUtils.GetID(richiesta) != 0)
                {
                    string dbSQL = "SELECT * FROM [tbl_RichiestaAssegniCircolariBeneficiari] WHERE [Richiesta]=" + DBUtils.GetID(richiesta) + " ORDER BY [Posizione] ASC";
                    var reader = new DBReader(Database.Tables["tbl_RichiestaAssegniCircolariBeneficiari"], dbSQL);
                    while (reader.Read())
                    {
                        var item = new CBeneficiarioRichiestaAssegni();
                        if (Database.Load(item, reader))
                            Add(item);
                    }

                    reader.Dispose();
                }

                return true;
            }

            private IDbConnection Appcon()
            {
                throw new NotImplementedException();
            }
        }
    }
}