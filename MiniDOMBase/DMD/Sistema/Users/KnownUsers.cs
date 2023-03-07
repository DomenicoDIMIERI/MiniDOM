using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;


namespace minidom.repositories
{
    public partial class CUsersClass
    {

        /// <summary>
        /// Utenti predefiniti
        /// </summary>
        public sealed class CKnownUsersClass
        {
            private readonly object m_Lock = new object();
            private CUser m_GlobalAdmin;
            private CUser m_SystemUser;
            private CUser m_Guest;

            /// <summary>
            /// Costruttore
            /// </summary>
            internal CKnownUsersClass()
            {
                ////DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Restituisce l'utente amministrativo globale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser GlobalAdmin
            {
                get
                {
                    lock (m_Lock)
                    {
                        if (m_GlobalAdmin is null)
                            m_GlobalAdmin = Users.GetItemByName("admin");
                        if (m_GlobalAdmin is null)
                        {
                            m_GlobalAdmin = Users.CreateUser("admin");
                            m_GlobalAdmin.ForceUser(SystemUser);
                            m_GlobalAdmin.ForcePassword("admin");
                            m_GlobalAdmin.Stato = ObjectStatus.OBJECT_VALID;
                            m_GlobalAdmin.Visible = true;
                            m_GlobalAdmin.Save();
                        }

                        return m_GlobalAdmin;
                    }
                }
            }

            /// <summary>
            /// Restituisce di sistema predefinito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser SystemUser
            {
                get
                {
                    lock (m_Lock)
                    {
                        if (m_SystemUser is null)
                            m_SystemUser = Users.GetItemByName("SYSTEM");
                        if (m_SystemUser is null)
                        {
                            m_SystemUser = new CUser("SYSTEM");
                            m_SystemUser.ForceUser(m_SystemUser);
                            m_SystemUser.ForcePassword(DMD.Strings.GetRandomString(25));
                            m_SystemUser.Stato = ObjectStatus.OBJECT_VALID;
                            m_SystemUser.UserStato = UserStatus.USER_DISABLED;
                            m_SystemUser.Visible = false;
                            m_SystemUser.Save();
                        }

                        return m_SystemUser;
                    }
                }
            }

            /// <summary>
            /// Restituisce l'utente predefinito per le sessioni non autenticate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser GuestUser
            {
                get
                {
                    lock (m_Lock)
                    {
                        if (m_Guest is null)
                            m_Guest = Users.GetItemByName("Guest");
                        if (m_Guest is null)
                        {
                            m_Guest = new CUser("Guest");
                            m_Guest.ForceUser(SystemUser);
                            m_Guest.ForcePassword("");
                            m_Guest.Stato = ObjectStatus.OBJECT_VALID;
                            m_Guest.UserStato = UserStatus.USER_DISABLED;
                            m_Guest.Visible = false;
                            m_Guest.Save();
                        }

                        return m_Guest;
                    }
                }
            }

            /// <summary>
            /// Resetta gli ottetti
            /// </summary>
            public void Reset()
            {
                lock (m_Lock)
                {
                    m_GlobalAdmin = null;
                    m_Guest = null;
                    m_SystemUser = null;
                }
            }

             
        }
    }
}