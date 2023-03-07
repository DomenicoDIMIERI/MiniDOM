using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CUfficio" />
        /// </summary>
        [Serializable]
        public class CUfficiClass 
            : CModulesClass<CUfficio>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUfficiClass() 
                : base("modUffici", typeof(Anagrafica.CUfficiCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce un oggetto CUfficio in base al suo nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUfficio GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                var items = GetPuntiOperativi();
                foreach (var item in items)
                {
                    if (DMD.Strings.Compare(item.Nome, value, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce tutti i punti operativi definiti per l'azienda principale
            /// </summary>
            /// <returns></returns>
            public CCollection<CUfficio> GetPuntiOperativi()
            {
                var a = Anagrafica.Aziende.AziendaPrincipale;
                if (a is null) throw new ArgumentNullException("Azienda principale non configurata");
                return new CCollection<CUfficio>(a.Uffici);
            }

            /// <summary>
            /// Restituisce i punti operativi visibili all'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CCollection<CUfficio> GetPuntiOperativiConsentiti(Sistema.CUser user)
            {
                lock (user)
                {
                    var ret = new CCollection<CUfficio>(user.Uffici); // Me.GetPuntiOperativiConsentiti(GetID(user))
                    ret.Sort();
                    return ret;
                }
            }

            /// <summary>
            /// Restituisce l'ufficio in base all'id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public override CUfficio GetItemById(int id)
            {
                var ret = Anagrafica.Aziende.AziendaPrincipale.Uffici.GetItemById(id);
                if (ret is null)
                    ret = base.GetItemById(id);
                return ret;
            }

            /// <summary>
            /// Restituisce i punti operativi visibili all'utente corrente
            /// </summary>
            /// <returns></returns>
            public CCollection<CUfficio> GetPuntiOperativiConsentiti()
            {
                return GetPuntiOperativiConsentiti(Sistema.Users.CurrentUser);
            }

            /// <summary>
            /// Restituisce i punti operativi visibili all'utente
            /// </summary>
            /// <param name="idUtente"></param>
            /// <returns></returns>
            public CCollection<CUfficio> GetPuntiOperativiConsentiti(int idUtente)
            {
                return GetPuntiOperativiConsentiti(Sistema.Users.GetItemById(idUtente));
            }

                
            // End Class

            private CUtentiXUfficioRepository m_UfficiConsentiti = null;

            /// <summary>
            /// Repository di oggetti di tipo CUtentiXUfficioRepository
            /// </summary>
            public CUtentiXUfficioRepository UfficiConsentiti
            {
                get
                {
                    if (m_UfficiConsentiti is null) this.m_UfficiConsentiti = new CUtentiXUfficioRepository();
                    return this.m_UfficiConsentiti;                     
                }
            }

            /// <summary>
            /// Restituisce il punto operativo corrente (da cui si è collegato l'utente corrente)
            /// </summary>
            /// <returns></returns>
            public CUfficio GetCurrentPO()
            {
                var uffici = Anagrafica.Uffici.GetPuntiOperativi();
                string nomeUfficio = Sistema.Users.CurrentUser.CurrentLogin.NomeUfficio;
                if (string.IsNullOrEmpty(nomeUfficio))
                    return null;
                foreach (CUfficio u in uffici)
                {
                    if (DMD.Strings.Compare(nomeUfficio, u.Nome, true) == 0)
                        return u;
                }

                return null;
            }

            /// <summary>
            ///  Restituisce il punto operativo corrente (da cui si è collegato l'utente corrente)
            /// </summary>
            /// <returns></returns>
            public CUfficio GetCurrentPOConsentito()
            {
                var u = Sistema.Users.CurrentUser;
                Sistema.CLoginHistory l = null;
                if (u is object)
                    l = u.CurrentLogin;
                string nomeUfficio = "";
                if (l is object)
                    nomeUfficio = l.NomeUfficio;
                if (string.IsNullOrEmpty(nomeUfficio))
                    return null;
                var uffici = Anagrafica.Uffici.GetPuntiOperativiConsentiti();
                foreach (var uff in uffici)
                {
                    if (DMD.Strings.Compare(nomeUfficio, uff.Nome, true) == 0)
                        return uff;
                }

                return null;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CUfficiClass m_Uffici = null;


        /// <summary>
        /// Repository di oggetti di tipo <see cref="CUfficio" />
        /// </summary>
        public static CUfficiClass Uffici
        {
            get
            {
                if (m_Uffici is null)
                    m_Uffici = new CUfficiClass();
                return m_Uffici;
            }
        }
    }
}