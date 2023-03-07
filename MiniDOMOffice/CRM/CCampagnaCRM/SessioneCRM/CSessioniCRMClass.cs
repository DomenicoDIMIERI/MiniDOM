using System;
using DMD.Databases;
using minidom.internals;
using minidom.repositories;
using static minidom.CustomerCalls;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CSessioneCRM"/>
        /// </summary>
        [Serializable]
        public sealed class CSessioniCRMClass 
            : CModulesClass<CustomerCalls.CSessioneCRM>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CSessioniCRMClass() 
                : base("modCRMSessions", typeof(CustomerCalls.CSessioneCRMCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce l'ultima sessione utente iniziata nella pagina
            /// </summary>
            /// <param name="userid"></param>
            /// <param name="dmdpage"></param>
            /// <returns></returns>
            public CustomerCalls.CSessioneCRM GetItemByPage(int userid, string dmdpage)
            {
                dmdpage = DMD.Strings.Trim(dmdpage);
                if (userid == 0 || string.IsNullOrEmpty(dmdpage))
                    return null;
                using (var cursor = new CustomerCalls.CSessioneCRMCursor())
                {
                    cursor.IDUtente.Value = userid;
                    cursor.dmdpage.Value = dmdpage;
                    cursor.IgnoreRights = true;
                    cursor.Inizio.SortOrder = SortEnum.SORT_DESC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce l'elemento in base alla pagina
            /// </summary>
            /// <param name="user"></param>
            /// <param name="dmdpage"></param>
            /// <returns></returns>
            public CustomerCalls.CSessioneCRM GetItemByPage(Sistema.CUser user, string dmdpage)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                return GetItemByPage(DBUtils.GetID(user, 0), dmdpage);
            }

            /// <summary>
            /// Inizializza una nuova sessione crm
            /// </summary>
            /// <param name="user"></param>
            /// <param name="dmdpage"></param>
            /// <returns></returns>
            public CustomerCalls.CSessioneCRM StartNewSession(Sistema.CUser user, string dmdpage)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                if (string.IsNullOrEmpty(dmdpage))
                    throw new ArgumentNullException("dmdpage");
                // Dim ret As CSessioneCRM = Me.GetItemByPage(user, dmdpage)
                var ret = new CustomerCalls.CSessioneCRM();
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.Utente = user;
                ret.dmdpage = dmdpage;
                ret.PuntoOperativo = Anagrafica.Uffici.GetCurrentPOConsentito();
                ret.Inizio = DMD.DateUtils.Now();
                ret.Save();
                return ret;
            }

            /// <summary>
            /// Restituisce la sesione crm attiva per l'utente corrente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CustomerCalls.CSessioneCRM GetCurrentSession(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                using (var cursor = new CustomerCalls.CSessioneCRMCursor())
                {
                    cursor.IDUtente.Value = DBUtils.GetID(user, 0);
                    cursor.Inizio.Value = DMD.DateUtils.ToDay();
                    cursor.Inizio.Operator = OP.OP_GE;
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Fine.Value = default;
                    cursor.Inizio.SortOrder = SortEnum.SORT_DESC;
                    return cursor.Item;
                }
            }
        }
    }

    public partial class CustomerCalls
    {
        private static CSessioniCRMClass m_SessioniCRM = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CSessioneCRM"/>
        /// </summary>
        public static CSessioniCRMClass SessioniCRM
        {
            get
            {
                if (m_SessioniCRM is null)
                    m_SessioniCRM = new CSessioniCRMClass();
                return m_SessioniCRM;
            }
        }
    }
}