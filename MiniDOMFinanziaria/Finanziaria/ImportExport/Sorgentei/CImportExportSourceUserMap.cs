using System;
using DMD;
using minidom;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CImportExportSourceUserMap : CReadOnlyCollection<CImportExportSourceUserMatc>
        {
            private CImportExportSource m_Owner;

            public CImportExportSourceUserMap()
            {
                m_Owner = null;
            }

            public CImportExportSourceUserMap(CImportExportSource owner) : this()
            {
                m_Owner = owner;
            }

            protected internal void SetOwner(CImportExportSource owner)
            {
                m_Owner = owner;
            }

            public CImportExportSourceUserMatc FindByRemoteUserID(int remoteUserID)
            {
                lock (this)
                {
                    foreach (CImportExportSourceUserMatc m in this)
                    {
                        if (m.RemoteUserID == remoteUserID)
                            return m;
                    }

                    return null;
                }
            }

            public CImportExportSourceUserMatc FindByRemoteUserName(string remoteUserName)
            {
                lock (this)
                {
                    remoteUserName = DMD.Strings.Trim(remoteUserName);
                    foreach (CImportExportSourceUserMatc m in this)
                    {
                        if (DMD.Strings.Compare(m.RemoteUserName, remoteUserName, true) == 0)
                            return m;
                    }

                    return null;
                }
            }

            public CImportExportSourceUserMatc SetUserMapping(Sistema.CUser user, int remoteUserID, string remoteUserName)
            {
                lock (this)
                {
                    CImportExportSourceUserMatc m = null;
                    foreach (var currentM in this)
                    {
                        m = currentM;
                        if (m.LocalUserID == DBUtils.GetID(user))
                        {
                            m.RemoteUserID = remoteUserID;
                            m.RemoteUserName = DMD.Strings.Trim(remoteUserName);
                            m_Owner.SetChanged(DMD.Booleans.ValueOf("UserMapping"));
                            return m;
                        }
                    }

                    m = new CImportExportSourceUserMatc();
                    m.LocalUser = user;
                    m.RemoteUserID = remoteUserID;
                    m.RemoteUserName = DMD.Strings.Trim(remoteUserName);
                    InternalAdd(m);
                    m_Owner.SetChanged(DMD.Booleans.ValueOf("UserMapping"));
                    return m;
                }
            }

            public CImportExportSourceUserMatc RemoveUserMapping(Sistema.CUser user)
            {
                lock (this)
                {
                    int j = -1;
                    CImportExportSourceUserMatc m = null;
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        m = this[i];
                        if (m.LocalUserID == DBUtils.GetID(user))
                        {
                            j = i;
                            break;
                        }
                    }

                    if (j >= 0)
                    {
                        InternalRemoveAt(j);
                        m_Owner.SetChanged(DMD.Booleans.ValueOf("UserMapping"));
                    }

                    return m;
                }
            }

            public CImportExportSourceUserMatc MapToRemoteUser(Sistema.CUser user)
            {
                lock (this)
                {
                    CImportExportSourceUserMatc m = null;
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        m = this[i];
                        if (m.LocalUserID == DBUtils.GetID(user))
                        {
                            break;
                        }
                    }

                    return m;
                }
            }

            public CImportExportSourceUserMatc MapToLocalUser(int remoteID)
            {
                lock (this)
                {
                    foreach (CImportExportSourceUserMatc m in this)
                    {
                        if (m.RemoteUserID == remoteID)
                        {
                            return m;
                        }
                    }

                    return null;
                }
            }

            public CImportExportSourceUserMatc MapToLocalUserByName(string remoteName)
            {
                lock (this)
                {
                    remoteName = DMD.Strings.Trim(remoteName);
                    foreach (CImportExportSourceUserMatc m in this)
                    {
                        if ((m.RemoteUserName ?? "") == (remoteName ?? ""))
                        {
                            return m;
                        }
                    }

                    return null;
                }
            }
        }
    }
}