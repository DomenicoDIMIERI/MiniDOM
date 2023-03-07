using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using System.Collections.Generic;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella degli utenti
        /// </summary>
        [Serializable]
        public class CUserCursor 
            : minidom.Databases.DBObjectCursorPO<CUser>
        {
            private int m_GroupID;
            private DBCursorStringField m_UserName = new DBCursorStringField("UserName");
            private DBCursorStringField m_Nominativo = new DBCursorStringField("Nominativo");
            private DBCursorStringField m_eMail = new DBCursorStringField("eMail");
            // Private m_Visible As New DBCursorField(Of Boolean)("Visible")
            private DBCursorField<UserFlags> m_Flags = new DBCursorField<UserFlags>("Flags");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("Persona");
            private DBCursorField<DateTime> m_PasswordExpire = new DBCursorField<DateTime>("PasswordExpire");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserCursor()
            {
            }

            /// <summary>
            /// IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// PasswordExpire
            /// </summary>
            public DBCursorField<DateTime> PasswordExpire
            {
                get
                {
                    return m_PasswordExpire;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<UserFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// UserName
            /// </summary>
            public DBCursorStringField UserName
            {
                get
                {
                    return m_UserName;
                }
            }

            /// <summary>
            /// Nominativo
            /// </summary>
            public DBCursorStringField Nominativo
            {
                get
                {
                    return m_Nominativo;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del gruppo in cui si ricerca l'utente
            /// </summary>
            public int GroupID
            {
                get
                {
                    return m_GroupID;
                }

                set
                {
                    m_GroupID = value;
                }
            }

            /// <summary>
            /// eMail
            /// </summary>
            public DBCursorStringField eMail
            {
                get
                {
                    return m_eMail;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("GroupID", m_GroupID);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "GroupID":
                        {
                            m_GroupID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Users";
            //}

            /// <summary>
            /// Applica ulteriori limiti
            /// </summary>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();

                if (this.GroupID != 0)
                {
                    List<int?> lst = new List<int?>();
                    using (var cursor = new CUserXGroupCursor())
                    {
                        cursor.GroupID.Value = this.GroupID;
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.UserID);
                        }
                    }
                    var ids = lst.ToArray();
                     
                    ret *= Field("ID").In(ids);
                }

                return ret;
            }

            //public override string GetWherePart()
            //{
            //    string ret;
            //    ret = base.GetWherePart();
            //    if (m_GroupID != 0)
            //        ret = DMD.Strings.Combine(ret, "([ID] In (SELECT [User] FROM [tbl_UsersXGroup] WHERE [Group]=" + m_GroupID + "))", " AND ");
            //    return ret;
            //}

            /// <summary>
            /// Inizializza il nuovo oggetto
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(object item)
            {
                {
                    var withBlock = (CUser)item;
                    withBlock.SetUserName(minidom.Sistema.Users.GetAvailableUserName("Utente"));
                }

                base.OnInitialize(item);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Users; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}