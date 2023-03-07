
namespace minidom
{
    public partial class WebSite
    {
        public class IPADDRESSInfoCursor : Databases.DBObjectCursor<IPADDRESSinfo>
        {
            private Databases.CCursorFieldObj<string> m_IP = new Databases.CCursorFieldObj<string>("IP");
            private Databases.CCursorFieldObj<string> m_NetMask = new Databases.CCursorFieldObj<string>("NetMask");
            private Databases.CCursorFieldObj<string> m_Descrizione = new Databases.CCursorFieldObj<string>("Descrizione");
            private Databases.CCursorField<bool> m_Allow = new Databases.CCursorField<bool>("Allow");
            private Databases.CCursorField<bool> m_Negate = new Databases.CCursorField<bool>("Negate");
            private Databases.CCursorField<bool> m_Interno = new Databases.CCursorField<bool>("Interno");
            private Databases.CCursorFieldObj<string> m_AssociaUfficio = new Databases.CCursorFieldObj<string>("AssociaUfficio");

            public IPADDRESSInfoCursor()
            {
            }

            public Databases.CCursorFieldObj<string> IP
            {
                get
                {
                    return m_IP;
                }
            }

            public Databases.CCursorFieldObj<string> NetMask
            {
                get
                {
                    return m_NetMask;
                }
            }

            public Databases.CCursorFieldObj<string> Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public Databases.CCursorField<bool> Allow
            {
                get
                {
                    return m_Allow;
                }
            }

            public Databases.CCursorField<bool> Negate
            {
                get
                {
                    return m_Negate;
                }
            }

            public Databases.CCursorField<bool> Interno
            {
                get
                {
                    return m_Interno;
                }
            }

            public Databases.CCursorFieldObj<string> AssociaUfficio
            {
                get
                {
                    return m_AssociaUfficio;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            protected override Sistema.CModule GetModule()
            {
                return Instance.Module;
            }

            public override string GetTableName()
            {
                return "tbl_AllowedIPs";
            }
        }
    }
}