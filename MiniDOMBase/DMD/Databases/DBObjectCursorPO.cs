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
    public partial class Databases
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="IDBPOObject"/>
        /// </summary>
        [Serializable]
        public abstract class DBObjectCursorPO
            : minidom.Databases.DBObjectCursor 
        {
            
            private DBCursorField<int> m_IDPuntoOperativo = new DBCursorField<int>("IDPuntoOperativo");
            private DBCursorStringField m_NomePuntoOperativo = new DBCursorStringField("NomePuntoOperativo");

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectCursorPO()
            {
            }

            /// <summary>
            /// Campo IDPuntoOperativo
            /// </summary>
            public DBCursorField<int> IDPuntoOperativo
            {
                get
                {
                    return m_IDPuntoOperativo;
                }
            }

            /// <summary>
            /// Campo NomePuntoOperativo
            /// </summary>
            public DBCursorStringField NomePuntoOperativo
            {
                get
                {
                    return this.m_NomePuntoOperativo;
                }
            }

            //protected override void GetWherePartLimit()
            //{
            //    var tmpSQL = new System.Text.StringBuilder();
            //    if (!Module.UserCanDoAction("list"))
            //    {
            //        if (Module.UserCanDoAction("list_office"))
            //        {
            //            CCollection<CUfficio> items = Sistema.Users.CurrentUser.Uffici;
            //            tmpSQL.Append("[IDPuntoOperativo] In (0, NULL");
            //            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            //            {
            //                tmpSQL.Append(",");
            //                tmpSQL.Append(DBUtils.GetID(items[i], 0));
            //            }

            //            tmpSQL.Append(")");
            //        }

            //        if (Module.UserCanDoAction("list_own"))
            //        {
            //            if (tmpSQL.Length > 0)
            //                tmpSQL.Append(" OR ");
            //            tmpSQL.Append("([CreatoDa]=" + DBUtils.GetID(Sistema.Users.CurrentUser, 0) + ")");
            //        }

            //        if (tmpSQL.Length == 0)
            //            tmpSQL.Append("(0<>0)");
            //    }

            //    this.WhereClauses.Clear();
            //    this.WhereClauses.Add(tmpSQL.ToString());
            //}

            /// <summary>
            /// Restituisce i campi where
            /// </summary>
            /// <returns></returns>
            /// 
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                DBCursorFieldBase wherePart = DBCursorFieldBase.True;

                if (!this.Module.UserCanDoAction("list"))
                {
                    wherePart = DBCursorFieldBase.False;

                    var user = minidom.Sistema.Users.CurrentUser;
                    if (this.Module.UserCanDoAction("list_office"))
                    {
                        var arrPO = new List<int>();
                        foreach (var ufficio in user.Uffici)
                        {
                            arrPO.Add(DBUtils.GetID(ufficio, 0));
                        }
                        
                        var officePart = new DBCursorField<int>("IDPuntoOperativo", OP.OP_IN, true);
                        officePart.ValueIn(arrPO.ToArray());
                        wherePart = wherePart.Or(officePart);
                    }

                    if (this.Module.UserCanDoAction("list_own"))
                    {
                        var userPart = new DBCursorField<int>("CreatoDa", OP.OP_EQ, true);
                        userPart.Value = DBUtils.GetID(user, 0);

                        wherePart = wherePart.Or(userPart);
                    }
                    
                     
                }

                return wherePart;
            }
        }
    }
}