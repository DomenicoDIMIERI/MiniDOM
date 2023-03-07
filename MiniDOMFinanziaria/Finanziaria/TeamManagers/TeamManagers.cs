using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public sealed class CTeamManagersClass : CModulesClass<CTeamManager>
        {
            private CSetPremi m_DefaultSetPremi;

            internal CTeamManagersClass() : base("modTeamManager", typeof(CTeamManagersCursor), -1)
            {
            }

            public CSetPremi GetSetPremiById(int id)
            {
                if (id == 0)
                    return null;
                var cursor = new CSetPremiCursor();
                CSetPremi ret;
                cursor.PageSize = 1;
                cursor.IgnoreRights = true;
                cursor.ID.Value = id;
                ret = (CSetPremi)cursor.Item;
                cursor.Dispose();
                return ret;
            }

            public CSetPremi DefaultSetPremi
            {
                get
                {
                    if (m_DefaultSetPremi is null)
                    {
                        int idSet = Finanziaria.Module.Settings.GetValueInt("TeamManagers_DefSetPremi", 0);
                        m_DefaultSetPremi = TeamManagers.GetSetPremiById(idSet);
                        if (m_DefaultSetPremi is null)
                        {
                            m_DefaultSetPremi = new CSetPremi();
                            m_DefaultSetPremi.Stato = ObjectStatus.OBJECT_VALID;
                            m_DefaultSetPremi.Save();
                        }
                    }

                    return m_DefaultSetPremi;
                }

                set
                {
                    if (DBUtils.GetID(value) == 0)
                        throw new ArgumentNullException("Il set premi predefinito non può essere Null");
                    Finanziaria.Module.Settings.SetValueInt("TeamManagers_DefSetPremi", DBUtils.GetID(value));
                    m_DefaultSetPremi = value;
                }
            }

            public CTeamManager GetItemByName(string nominativo)
            {
                nominativo = Strings.Trim(nominativo);
                if (string.IsNullOrEmpty(nominativo))
                    return null;
                foreach (CTeamManager tm in LoadAll())
                {
                    if (tm.Stato == ObjectStatus.OBJECT_VALID 
                        && DMD.Strings.Compare(tm.Nominativo, nominativo, true) == 0)
                    {
                        return tm;
                    }
                }

                return null;
            }

            public CTeamManager GetItemByUser(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException();
                if (DBUtils.GetID(user) == 0)
                    return null;
                foreach (CTeamManager tm in LoadAll())
                {
                    if (tm.Stato == ObjectStatus.OBJECT_VALID && tm.UserID == DBUtils.GetID(user))
                    {
                        return tm;
                    }
                }

                return null;
            }

            public CTeamManager GetItemByUser(int userID)
            {
                if (userID == 0)
                    return null;
                return GetItemByUser(Sistema.Users.GetItemById(userID));
            }

            public CTeamManager GetItemByPersona(int personID)
            {
                if (personID == 0)
                    return null;
                foreach (CTeamManager tm in LoadAll())
                {
                    if (tm.Stato == ObjectStatus.OBJECT_VALID && tm.PersonaID == personID)
                    {
                        return tm;
                    }
                }

                return null;
            }
        }

        private static CTeamManagersClass m_TeamManagers = null;

        public static CTeamManagersClass TeamManagers
        {
            get
            {
                if (m_TeamManagers is null)
                    m_TeamManagers = new CTeamManagersClass();
                return m_TeamManagers;
            }
        }
    }
}