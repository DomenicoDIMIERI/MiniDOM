using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    namespace repositories
    {

         
        public sealed partial class CGroupsClass 
        {

            
            /// <summary>
            /// Gruppi predefiniti
            /// </summary>
            public sealed class CKnownGroupsClass
            {
                private CGroup m_Administrators;
                
                private CGroup m_Users;
                
                private CGroup m_Guests;

                /// <summary>
                /// Costruttore
                /// </summary>
                internal CKnownGroupsClass()
                {
                }

                /// <summary>
                /// Restituisce il gruppo predefinito Administrators
                /// </summary>
                /// <value></value>
                /// <returns></returns>
                /// <remarks></remarks>
                public CGroup Administrators
                {
                    get
                    {
                        if (m_Administrators is null)
                        {
                            m_Administrators = Groups.GetItemByName("Administrators");
                            if (m_Administrators is null)
                            {
                                m_Administrators = new CGroup("Administrators");
                                m_Administrators.IsBuiltIn = true;
                                m_Administrators.Stato = ObjectStatus.OBJECT_VALID;
                                m_Administrators.ForceUser(minidom.Sistema.Users.KnownUsers.SystemUser);
                                m_Administrators.Save();
                            }
                        }

                        return m_Administrators;
                    }
                }

                /// <summary>
                /// Restituisce il gruppo predefinito degli utenti non connessi
                /// </summary>
                /// <value></value>
                /// <returns></returns>
                /// <remarks></remarks>
                public Sistema.CGroup Guests
                {
                    get
                    {
                        if (m_Guests is null)
                        {
                            m_Guests = Sistema.Groups.GetItemByName("Guests");
                            if (m_Guests is null)
                            {
                                m_Guests = new Sistema.CGroup("Guests");
                                m_Guests.IsBuiltIn = true;
                                m_Guests.Stato = ObjectStatus.OBJECT_VALID;
                                m_Guests.ForceUser(Sistema.Users.KnownUsers.SystemUser);
                                m_Guests.Save();
                            }
                        }

                        return m_Guests;
                    }
                }

                /// <summary>
                /// Restituisce il gruppo predefinito degli utenti definiti
                /// </summary>
                /// <value></value>
                /// <returns></returns>
                /// <remarks></remarks>
                public Sistema.CGroup Users
                {
                    get
                    {
                        if (m_Users is null)
                        {
                            m_Users = Sistema.Groups.GetItemByName("Users");
                            if (m_Users is null)
                            {
                                m_Users = new Sistema.CGroup("Users");
                                m_Users.IsBuiltIn = true;
                                m_Users.Stato = ObjectStatus.OBJECT_VALID;
                                m_Users.ForceUser(Sistema.Users.KnownUsers.SystemUser);
                                m_Users.Save();
                            }
                        }

                        return m_Users;
                    }
                }
            }

            private CKnownGroupsClass m_KnownGroups = null;

            /// <summary>
            /// Gruppi predefiniti
            /// </summary>
            public CKnownGroupsClass KnownGroups
            {
                get
                {
                    if (m_KnownGroups is null)
                        m_KnownGroups = new CKnownGroupsClass();
                    return m_KnownGroups;
                }
            }
        }
    }
     
}