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

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CSecurityToken"/>
        /// </summary>
        [Serializable]
        public sealed class CASPSecurityClass
            : CModulesClass<CSecurityToken>
        {
            private const int SESSIONTIMEOUT = 120;
            internal object Lock = new object();

            /// <summary>
            /// Costruttore
            /// </summary>
            public CASPSecurityClass()
                : base("modSecurityTokenss", typeof(CSecurityTokenCursor), 0)
            {
            }

            /// <summary>
            /// Inizializza il modulo
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaCreated += HandlePeronaModified;
                minidom.Anagrafica.PersonaDeleted += HandlePeronaModified;
                minidom.Anagrafica.PersonaModified += HandlePeronaModified;
                minidom.Anagrafica.PersonaMerged += HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
            }

            /// <summary>
            /// Termina il modulo
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaCreated -= HandlePeronaModified;
                minidom.Anagrafica.PersonaDeleted -= HandlePeronaModified;
                minidom.Anagrafica.PersonaModified -= HandlePeronaModified;
                minidom.Anagrafica.PersonaMerged -= HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
                base.Terminate();
            }

            /// <summary>
            /// Gestisce l'evento PersonaMerged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaMerged(MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    // Tabella tbl_SecurityTokens 
                    using(var cursor = new CSecurityTokenCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.TokenSourceName.Value = DMD.RunTime.vbTypeName(mi.Persona2);
                        cursor.TokenSourceID.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = mi.Add("tbl_SecurityTokens", "TokenSourceID", DBUtils.GetID(cursor.Item, 0));
                            cursor.Item.TokenSourceID = mi.IDPersona1;
                            cursor.Item.TokenSourceName = DMD.RunTime.vbTypeName(mi.Persona1);
                            cursor.Item.Save();
                        }
                    }

                }
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaUnMerged(MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;

                    // Tabella tbl_SecurityTokens 
                    var items = mi.GetAffectedRecorsIDs("tbl_SecurityTokens", "TokenSourceName");
                    //if (!string.IsNullOrEmpty(items))
                    //        Databases.APPConn.ExecuteCommand("UPDATE [tbl_SecurityTokens] SET [TokenSourceName]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");
                    using (var cursor = new CSecurityTokenCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.TokenSourceID = mi.IDPersona2;
                            cursor.Item.Save();
                        }
                    }
                }
            }



            /// <summary>
            /// Gestisce l'evento PersonaChanged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaModified(PersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var p = e.Persona;

                }
            }


            /// <summary>
            /// Restituisce il token in base alla stringa che lo identifica univocamente
            /// </summary>
            /// <param name="token"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CSecurityToken GetToken(string token)
            {
                token = DMD.Strings.Trim(token);
                if (string.IsNullOrEmpty(token))
                    return null;

                using (var cursor = new CSecurityTokenCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.TokenID.Value = token;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce il promo valore valido per il token
            /// </summary>
            /// <returns></returns>
            internal string GetAvailableToken()
            {
                string strToken = DMD.Strings.vbNullString;
                bool isNew = false;
                while (!isNew)
                {
                    // Prepariamo il token
                    strToken = GetRandomKey(20);
                    isNew = GetToken(strToken) is null;
                }

                return strToken;
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome del token
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            private Sistema.CSecurityToken FindTokenByName(string name)
            {
                using (var cursor = new CSecurityTokenCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.TokenName.Value = name;
                    return cursor.Item;
                }                     
            }

            /// <summary>
            /// Salva il valore specificato e restituisce un token valido solo all'interno della sessione corrente
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <param name="expireCount"></param>
            /// <param name="expireTime"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CSecurityToken CreateToken(
                                    string name, 
                                    string value, 
                                    int expireCount = 1, 
                                    DateTime? expireTime = default
                                    )
            {
                lock (Sistema.ASPSecurity.Lock)
                {
                    var ret = new Sistema.CSecurityToken();
                    ret.m_TokenID = Sistema.ASPSecurity.GetAvailableToken();
                    ret.m_TokenName = name;
                    ret.m_Valore = value;
                    ret.m_ExpireCount = expireCount;
                    ret.m_ExpireTime = expireTime;
                    ret.Save(true);
                    return ret;
                }                
            }



            /// <summary>
            /// Cerca il token associato al nome specificato e se non esiste lo crea
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <param name="expireCount"></param>
            /// <param name="expireTime"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CSecurityToken FindTokenOrCreate(
                                string name, 
                                string value, 
                                int expireCount = 1, 
                                DateTime? expireTime = default
                                )
            {
                lock (Lock)
                {
                    var token = GetToken(name);
                    if (token is null)
                        token = CreateToken(name, value, expireCount, expireTime);
                    return token;
                }
            }


            /// <summary>
            /// Genera una chiave alfanumerica della lunghezza specificata
            /// </summary>
            /// <param name="keyLen"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            private string GetRandomKey(int keyLen)
            {
                const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string strToken;
                int i, p;
                 
                strToken = "";
                var loopTo = keyLen;
                for (i = 1; i <= loopTo; i++)
                {
                    do
                        p = (int)(DMD.RunTime.GetRandomDouble(0.0, 1.0) * (DMD.Strings.Len(CHARS) + 1));
                    while (!(p >= 1 & p <= DMD.Strings.Len(CHARS)));
                    strToken = strToken + DMD.Strings.Mid(CHARS, p, 1);
                }

                return strToken;
            }
        }
    }

    public partial class Sistema
    {
        private static CASPSecurityClass m_ASPSecurity = null;

        /// <summary>
        /// Repository di oggetti <see cref="CSecurityToken"/>
        /// </summary>
        public static CASPSecurityClass ASPSecurity
        {
            get
            {
                if (m_ASPSecurity is null)
                    m_ASPSecurity = new CASPSecurityClass();
                return m_ASPSecurity;
            }
        }
    }
}