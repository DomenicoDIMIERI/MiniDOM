using System;
using System.Data;
using DMD;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public sealed class CProfiliClass : CModulesClass<Finanziaria.CProfilo>
        {
            internal CProfiliClass() : base("modPreventivatori", typeof(Finanziaria.CProfiliCursor), -1)
            {
            }



            // Public Function CreateNew(ByVal profilo As String) As CProfilo
            // Dim item As New CProfilo
            // item.Profilo = profilo
            // item.ProfiloVisibile = profilo
            // item.Save()
            // Return item
            // End Function

            public void SetPreventivatoreUtente(int userID, int prevID, bool allow)
            {
                string dbSQL;
                dbSQL = "SELECT * FROM [tbl_PreventivatoriXUser] WHERE ([Utente]=" + userID + ") And ([Preventivatore]=" + prevID + ");";
                using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL))
                {
                    if (dbRis.Read() == false)
                    {
                        dbRis.Dispose();
                        dbSQL = "INSERT INTO [tbl_PreventivatoriXUser] ([Utente], [Preventivatore], [Allow]) VALUES (" + userID + ", " + prevID + ", " + DBUtils.DBBool(allow) + ");";
                        Finanziaria.Database.ExecuteCommand(dbSQL);
                    }
                    else
                    {
                        dbSQL = "UPDATE [tbl_PreventivatoriXUser] SET [Allow]=" + DBUtils.DBBool(allow) + " WHERE ([Utente]=" + userID + ") And ([Preventivatore]=" + prevID + ");";
                        Finanziaria.Database.ExecuteCommand(dbSQL);
                    }

                }
            }

            public Finanziaria.CProfilo GetItemByName(string value)
            {
                value = Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CProfilo ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.Nome, value, true) == 0)
                        return ret;
                }

                return null;
            }

            /// <summary>
            /// Restituisce un oggetto CCollection contenente gli oggetti preventivatori consentiti all'utente ed ai gruppi a cui appartiene l'utente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Finanziaria.CProfilo> GetPreventivatoriUtente(bool onlyValid = true)
            {
                var items = new CCollection<Finanziaria.CProfilo>();
                foreach (Finanziaria.CProfilo p in LoadAll())
                {
                    if (p.IsVisibleToUser(Sistema.Users.CurrentUser))
                    {
                        if (onlyValid)
                        {
                            if (p.IsValid())
                                items.Add(p);
                        }
                        else
                        {
                            items.Add(p);
                        }
                    }
                }

                return items;
            }

            /// <summary>
            /// Restituisce un oggetto CCollection contenente gli oggetti preventivatori consentiti all'utente ed ai gruppi a cui appartiene l'utente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Finanziaria.CProfilo> GetPreventivatoriUtente(DateTime dataDecorrenza, bool onlyValid = true)
            {
                var items = new CCollection<Finanziaria.CProfilo>();
                foreach (Finanziaria.CProfilo p in LoadAll())
                {
                    if (p.IsVisibleToUser(Sistema.Users.CurrentUser))
                    {
                        if (onlyValid)
                        {
                            if (p.IsValid(dataDecorrenza))
                                items.Add(p);
                        }
                        else
                        {
                            items.Add(p);
                        }
                    }
                }

                return items;
            }

            public CCollection<Finanziaria.CProfilo> GetPreventivatoriUtenteOffline()
            {
                var items = GetPreventivatoriUtente();
                var ret = new CCollection<Finanziaria.CProfilo>();
                for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                {
                    var item = items[i];
                    if (item.IsOffline())
                        ret.Add(item);
                }

                return ret;
            }

            /// <summary>
            /// Restituisce un array contenente l'elenco dei preventivatori consentiti o negati al solo utente (senza considerare i gruppi
            /// </summary>
            /// <param name="user"></param>
            /// <param name="items"></param>
            /// <param name="arrIDs"></param>
            /// <param name="arrAllow"></param>
            /// <param name="arrNegate"></param>
            /// <remarks></remarks>
            public void GetPreventivatoriPerUtente(Sistema.CUser user, ref Finanziaria.CProfilo[] items, ref int[] arrIDs, ref bool[] arrAllow, ref bool[] arrNegate)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                GetPreventivatoriPerUtente(DBUtils.GetID(user), ref items, ref arrIDs, ref arrAllow, ref arrNegate);
            }

            /// <summary>
            /// Restituisce un array contenente l'elenco dei preventivatori consentiti o negati al solo utente (senza considerare i gruppi
            /// </summary>
            /// <param name="userID"></param>
            /// <param name="items"></param>
            /// <param name="arrIDs"></param>
            /// <param name="arrAllow"></param>
            /// <param name="arrNegate"></param>
            /// <remarks></remarks>
            public void GetPreventivatoriPerUtente(int userID, ref Finanziaria.CProfilo[] items, ref int[] arrIDs, ref bool[] arrAllow, ref bool[] arrNegate)
            {
                items = null;
                arrIDs = null;
                arrAllow = null;
                arrNegate = null;
                if (userID == 0)
                    return;
                string dbSQL = "SELECT Count(*) FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" + userID + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                int numItems = Sistema.Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM (" + dbSQL + ")"));
                if (numItems > 0)
                {
                    items = new Finanziaria.CProfilo[numItems];
                    arrAllow = new bool[numItems];
                    arrNegate = new bool[numItems];
                    arrIDs = new int[numItems];
                    dbSQL = "SELECT * FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" + userID + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();

                    using (var dbRis1 = Finanziaria.Database.ExecuteReader(dbSQL + " ORDER BY [NomeCessionario] ASC, [Nome] ASC, [Profilo] ASC"))
                    {
                        var reader = new DBReader(dbRis1);
                        int i = 0;
                        while (reader.Read())
                        {
                            items[i] = GetItemById(Sistema.Formats.ToInteger(dbRis1["Preventivatore"])); // CProfilo
                            if (items[i].Stato == ObjectStatus.OBJECT_VALID)
                            {
                                // Finanziaria.Database.Load(items(i), reader)
                                arrIDs[i] = reader.Read("IDAuth", arrIDs[i]);
                                arrAllow[i] = reader.Read("Allow", arrAllow[i]);
                                arrNegate[i] = reader.Read("Negate", arrNegate[i]);
                                i = i + 1;
                            }
                        }

                        reader.Dispose();
                        reader = null;
                    }
                }
            }

            internal void InvalidateProdottiProfilo()
            {
                foreach (CacheItem ci in CachedItems)
                    ci.Item.InvalidateProdottiProfilo();
            }
        }
    }

    public partial class Finanziaria
    {
        private static CProfiliClass m_Profili = null;

        public static CProfiliClass Profili
        {
            get
            {
                if (m_Profili is null)
                    m_Profili = new CProfiliClass();
                return m_Profili;
            }
        }
    }
}