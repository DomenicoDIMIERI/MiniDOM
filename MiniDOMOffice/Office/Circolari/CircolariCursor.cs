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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Cursore di oggetti <see cref="Circolare"/>
        /// TODO modificare in DBObjectCursorPO in js
        /// </summary>
        [Serializable]
        public class CircolariCursor
            : Databases.DBObjectCursorPO<Circolare>
        {
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorStringField m_Titolo = new DBCursorStringField("Titolo"); //TODO js
            private DBCursorStringField m_Testo = new DBCursorStringField("Testo"); //TODO js
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_Sottocategoria = new DBCursorStringField("Sottocategoria"); //TODO js
            private DBCursorStringField m_Url = new DBCursorStringField("path");
            private DBCursorField<bool> m_ShowInMainPage = new DBCursorField<bool>("PrimaPagina");
            private DBCursorField<int> m_Progressivo = new DBCursorField<int>("Progressivo");
            private DBCursorField<PriorityEnum> m_Priorita = new DBCursorField<PriorityEnum>("Priorita");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolariCursor()
            {

            }

            /// <summary>
            /// DettaglioStato
            /// </summary>
            public DBCursorStringField DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Titolo;
                }
            }

            /// <summary>
            /// Testo
            /// </summary>
            public DBCursorStringField Testo
            {
                get
                {
                    return m_Testo;
                }
            }

            /// <summary>
            /// DataInizio
            /// </summary>
            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            /// <summary>
            /// Progressivo
            /// </summary>
            public DBCursorField<int> Progressivo
            {
                get
                {
                    return m_Progressivo;
                }
            }

            /// <summary>
            /// Priorita
            /// </summary>
            public DBCursorField<PriorityEnum> Priorita
            {
                get
                {
                    return m_Priorita;
                }
            }

            /// <summary>
            /// DataFine
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// Sottocategoria
            /// </summary>
            public DBCursorStringField Sottocategoria
            {
                get
                {
                    return m_Sottocategoria;
                }
            }

            /// <summary>
            /// URL
            /// </summary>
            public DBCursorStringField URL
            {
                get
                {
                    return m_Url;
                }
            }

            /// <summary>
            /// ShowInMainPage
            /// </summary>
            public DBCursorField<bool> ShowInMainPage
            {
                get
                {
                    return m_ShowInMainPage;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Circolari;
            }

            //// Public Overrides Function GetWherePartLimit() As String
            //// Dim ret As String = MyBase.GetWherePartLimit()
            //// If (ret <> "") Then
            //// Dim wherePart As String

            //// If Me.Module.UserCanDoAction("list_assigned") Then
            //// wherePart = "([IDOperatore] = " & GetID(Users.CurrentUser) & " OR [IDAssegnataA] = " & GetID(Users.CurrentUser) & ")"
            //// ret = Strings.Combine(ret, wherePart, " OR ")
            //// End If
            //// If Me.Module.UserCanDoAction("list_own") Then
            //// ret = Strings.Combine(ret, "([IDAssegnataDa]=" & GetID(Users.CurrentUser) & ")", " OR ")
            //// End If
            //// End If
            //// Return ret
            //// End Function
            //private string GetTxtGruppi()
            //{
            //    var ret = new System.Text.StringBuilder();
            //    lock (Sistema.Users.CurrentUser)
            //    {
            //        foreach (Sistema.CGroup grp in Sistema.Users.CurrentUser.Groups)
            //        {
            //            if (ret.Length > 0)
            //                ret.Append(",");
            //            ret.Append(DBUtils.GetID(grp));
            //        }
            //    }

            //    return ret.ToString();
            //}

            private class UserAN
            {
                public int Circolare; 
                //public CUser User = null;
                public bool UserAllow = false;
                public bool UserNegate = false;
                //public CGroup Group = null;
                public bool GroupAllow = false;
                public bool GroupNegate = false;
            }

            private int[] GetAffectedDocuments()
            {
                var dic = new Dictionary<int, UserAN>();
                var u = Sistema.Users.CurrentUser;
                int uID = DBUtils.GetID(u, 0);
                using(var cursor = new CircolareXUserCursor())
                {
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        var item = cursor.Item;
                        UserAN an = null;
                        if (!dic.TryGetValue(item.ItemID, out an))
                        {
                            an = new UserAN();
                            an.Circolare = item.ItemID;
                            dic.Add(item.ItemID, an);
                        }
                        an.UserAllow = an.UserAllow || item.Allow;
                        an.UserNegate = an.UserNegate || item.Negate;
                    }
                }

                foreach (var grp in u.Groups)
                {
                    if (grp.Stato != ObjectStatus.OBJECT_VALID)
                        continue;

                    using (var cursor = new CircolareXGroupCursor())
                    {
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            var item = cursor.Item;
                            UserAN an = null;
                            if (!dic.TryGetValue(item.ItemID, out an))
                            {
                                an = new UserAN();
                                an.Circolare = item.ItemID;
                                dic.Add(item.ItemID, an);
                            }
                            an.GroupAllow = an.GroupAllow || item.Allow;
                            an.GroupNegate = an.GroupNegate || item.Negate;
                        }
                    }
                }

                var ret = new List<int>();
                foreach (var item in dic.Values)
                {
                    if ((item.UserAllow || item.GroupAllow) && !(item.UserNegate || item.GroupNegate))
                        ret.Add(item.Circolare);
                }
                return ret.ToArray();
            }

            /// <summary>
            /// Where Part
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                DBCursorFieldBase wherePart = base.GetWherePartLimit();

                if (!Module.UserCanDoAction("list"))
                {
                    //string dbSQL = "";
                    //string txtGruppi = GetTxtGruppi();
                    //if (string.IsNullOrEmpty(txtGruppi))
                    //{
                    //    dbSQL = "SELECT Comunicazione FROM (";
                    //    dbSQL += "SELECT Comunicazione, Min(Allow) As MAllow FROM (";
                    //    dbSQL += "SELECT Comunicazione, Allow FROM tbl_ComunicazioniXUtente WHERE [Utente]=" + DBUtils.GetID(Sistema.Users.CurrentUser) + " ";
                    //    dbSQL += ")  GROUP BY [Comunicazione] AS T1 ";
                    //    dbSQL += ") WHERE MAllow=True  ";
                    //}
                    //else
                    //{
                    //    dbSQL += "SELECT Comunicazione FROM (";
                    //    dbSQL += "SELECT Comunicazione, Min(Allow) AS MAllow FROM (";
                    //    dbSQL += "SELECT Comunicazione, Allow FROM tbl_ComunicazioniXGruppo WHERE [Gruppo] In (" + GetTxtGruppi() + ") ";
                    //    dbSQL += "UNION ";
                    //    dbSQL += "SELECT Comunicazione, Allow FROM tbl_ComunicazioniXUtente WHERE [Utente]=" + DBUtils.GetID(Sistema.Users.CurrentUser);
                    //    dbSQL += ") AS [T1] GROUP BY [T1].Comunicazione ";
                    //    dbSQL += ") WHERE MAllow=True ";
                    //}
                    if ( Module.UserCanDoAction("list_assigned"))
                    {
                        wherePart += this.Field("ID").In(this.GetAffectedDocuments());
                    }
                    else
                    {
                        wherePart *= DBCursorFieldBase.False;
                    }

                }

                return wherePart;
            }
        }
    }
}