using System;
using DMD;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public sealed class CConsulentiClass : CModulesClass<Finanziaria.CConsulentePratica>
        {
            internal CConsulentiClass() : base("modCQSPDConsulenti", typeof(Finanziaria.CConsulentiPraticaCursor), -1)
            {
            }

            public Finanziaria.CConsulentePratica GetItemByUser(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                return GetItemByUser(DBUtils.GetID(user));
            }

            public Finanziaria.CConsulentePratica GetItemByUser(int id)
            {
                if (id == 0)
                    return null;
                foreach (Finanziaria.CConsulentePratica item in LoadAll())
                {
                    if (item.IDUser == id)
                        return item;
                }

                return null;
            }

            public Finanziaria.CConsulentePratica GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (Finanziaria.CConsulentePratica c in LoadAll())
                {
                    if (c.Stato == ObjectStatus.OBJECT_VALID && DMD.Strings.Compare(c.Nome, nome, true) == 0)
                        return c;
                }

                return null;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CConsulentiClass m_Consulenti = null;

        public static CConsulentiClass Consulenti
        {
            get
            {
                if (m_Consulenti is null)
                    m_Consulenti = new CConsulentiClass();
                return m_Consulenti;
            }
        }
    }
}