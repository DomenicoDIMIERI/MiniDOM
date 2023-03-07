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
        /// Cursore di oggetti di tipo <see cref="IDBObject"/>
        /// </summary>
        [Serializable]
        public abstract class DBObjectCursor 
            : minidom.Databases.DBObjectCursorBase
        {

            private DBCursorField<int> m_CreatoDa;
            private DBCursorField<DateTime> m_CreatoIl;
            private DBCursorField<int> m_ModificatoDa;
            private DBCursorField<DateTime> m_ModificatoIl;
            private DBCursorField<ObjectStatus> m_Stato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectCursor()
            {
                m_CreatoDa = new DBCursorField<int>("CreatoDa");
                m_CreatoIl = new DBCursorField<DateTime>("CreatoIl");
                m_ModificatoDa = new DBCursorField<int>("ModificatoDa");
                m_ModificatoIl = new DBCursorField<DateTime>("ModificatoIl");
                m_Stato = new DBCursorField<ObjectStatus>("Stato");
            }

            /// <summary>
            /// Apre il cursore
            /// </summary>
            /// <returns></returns>
            public override bool Open()
            {
                this.GetWherePartLimit();

                return base.Open();
            }

            ///// <summary>
            ///// Applica i limiti al cursore in funzione delle azioni concesse all'utente corrente
            ///// </summary>
            //protected virtual void GetWherePartLimit()
            //{
            //    if (!this.Module.UserCanDoAction("list"))
            //    {
            //        if (this.Module.UserCanDoAction("list_own"))
            //        {
            //            this.CreatoDa.Value = DBUtils.GetID(Sistema.Users.CurrentUser, 0);
            //            this.CreatoDa.Operator = OP.OP_EQ;
            //            this.CreatoDa.IncludeNulls = false;
            //        }
            //        else
            //        {
            //            this.ID.Value = 0;
            //            this.ID.Operator = OP.OP_EQ;
            //            this.ID.IncludeNulls = false;
            //        }
                     
            //    } 
            //}

            /// <summary>
            /// Campo CreatoDa
            /// </summary>
            public DBCursorField<int> CreatoDa
            {
                get
                {
                    return m_CreatoDa;
                }
            }

            /// <summary>
            /// Campo CreatoIl
            /// </summary>
            public DBCursorField<DateTime> CreatoIl
            {
                get
                {
                    return m_CreatoIl;
                }
            }

            /// <summary>
            /// Campo ModificatoDa
            /// </summary>
            public DBCursorField<int> ModificatoDa
            {
                get
                {
                    return m_ModificatoDa;
                }
            }

            /// <summary>
            /// Campo ModificatoIl
            /// </summary>
            public DBCursorField<DateTime> ModificatoIl
            {
                get
                {
                    return m_ModificatoIl;
                }
            }

            /// <summary>
            /// Campo Stato
            /// </summary>
            public DBCursorField<ObjectStatus> Stato
            {
                get
                {
                    return m_Stato;
                }
            }

            /// <summary>
            /// Vincoli aggiuntivi
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = DBCursorFieldBase.True;

                if (!this.Module.UserCanDoAction("list"))
                {
                    ret = DBCursorFieldBase.False;

                    var user = Sistema.Users.CurrentUser;
                    if (this.Module.UserCanDoAction("list_own"))
                    {
                        var f = new DBCursorField<int>("CreatoDa", OP.OP_EQ, true);
                        f.Value = DBUtils.GetID(user, 0);
                        ret = ret.Or(f);
                    }
                     
                }

                return ret;
            }
        }
    }
}