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
using static minidom.CustomerCalls;

namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="BlackListAddress"/>
        /// </summary>
        [Serializable]
        public sealed class CBlackListAddressesClass
          : CModulesClass<BlackListAddress>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CBlackListAddressesClass() 
                : base("modBlackListAddresses", typeof(BlackListAddressCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce null se l'indirizzo é bloccato
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public BlackListAddress CheckBlocked(string tipo, string value)
            {
                var c = new Anagrafica.CContatto(tipo, value);
                foreach (BlackListAddress item in LoadAll())
                {
                    if (
                           (string.IsNullOrEmpty(c.Tipo) || (DMD.Strings.Compare(item.TipoContatto, c.Tipo, true) == 0))
                        && item.IsNegated(c.Valore)
                        )
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce una collezione contenente solo gli indirizzi non bloccati
            /// </summary>
            /// <param name="list"></param>
            /// <returns></returns>
            public CCollection<BlackListAddress> CheckBlocked(CCollection<Anagrafica.CContatto> list)
            {
                var ret = new CCollection<BlackListAddress>();
                foreach (Anagrafica.CContatto rec in list)
                {
                    var b = CheckBlocked(rec.Tipo, rec.Valore);
                    ret.Add(b);
                }

                return ret;
            }

            /// <summary>
            /// Blocca l'indirizzo
            /// </summary>
            /// <param name="tipoContatto"></param>
            /// <param name="valoreContatto"></param>
            /// <param name="tipoRegola"></param>
            /// <param name="motivo"></param>
            /// <returns></returns>
            public BlackListAddress BlockAddress(
                                        string tipoContatto, 
                                        string valoreContatto, 
                                        BlackListType tipoRegola, 
                                        string motivo
                                        )
            {
                var item = new BlackListAddress();
                item.TipoContatto = tipoContatto;
                item.ValoreContatto = valoreContatto;
                item.TipoRegola = tipoRegola;
                item.MotivoBlocco = motivo;
                item.DataBlocco = DMD.DateUtils.Now();
                item.BloccatoDa = Sistema.Users.CurrentUser;
                item.Save();
                // Me.CachedItems.Add(item)
                return item;
            }

            /// <summary>
            /// Sblocca l'indirizzo
            /// </summary>
            /// <param name="tipoContatto"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public BlackListAddress UnblockAddress(string tipoContatto, string value)
            {
                var c = new Anagrafica.CContatto(tipoContatto, value);
                foreach (BlackListAddress item in LoadAll())
                {
                    if (
                           (DMD.Strings.Compare(item.TipoContatto, c.Tipo, true) == 0)
                        && item.IsNegated(c.Valore)
                        )
                    {
                        item.Delete();
                        return item;
                    }
                }

                return null;
            }
        }

    }

    public partial class CustomerCalls
    {
      
        private static CBlackListAddressesClass m_BlackListAdresses = null;

        /// <summary>
        /// Repository di oggetti <see cref="BlackListAddress"/>
        /// </summary>
        public static CBlackListAddressesClass BlackListAdresses
        {
            get
            {
                if (m_BlackListAdresses is null)
                    m_BlackListAdresses = new CBlackListAddressesClass();
                return m_BlackListAdresses;
            }
        }
    }
}