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
using static minidom.Finanziaria;

namespace minidom
{
    namespace repositories
    {



        /// <summary>
        /// Repository di <see cref="CConteggioEstintivo"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CConteggiEstintiviClass 
            : CModulesClass<CConteggioEstintivo>
        {

            /// <summary>
            /// Evento generato quando si riceve una richiesta di conteggio estintivo
            /// </summary>
            public event ItemEventHandler<CConteggioEstintivo> Segnalata;
             
            /// <summary>
            /// Evento generato quando una richiesta di conteggio estintivo viene segnalata
            /// </summary>
            public event ItemEventHandler<CConteggioEstintivo> PresaInCarico;
             
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public CConteggiEstintiviClass() 
                : base("modCQSPDConteggiEstintivi", typeof(Finanziaria.CConteggioEstintivoCursor), 0)
            {
            }

            /// <summary>
            /// Genera l'evento Segnalata
            /// </summary>
            /// <param name="e"></param>
            internal void doOnSegnalata(ItemEventArgs<CConteggioEstintivo> e)
            {
                var richiesta = e.Item;
                var cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    if ((!cliente.GetFlag(PFlags.Cliente).HasValue || cliente.GetFlag(PFlags.Cliente) == false) == true)
                    {
                        cliente.SetFlag(PFlags.Cliente, true);
                        cliente.Save();
                    }

                    var info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.isClienteInAcquisizione = false;
                    info.isClienteAcquisito = true;
                    info.AggiungiAttenzione(richiesta, "Richiesta CE non ancora presa in carico", "Richiesta CE " + DBUtils.GetID(richiesta));
                    info.AggiornaOperazione(richiesta, "Richiesta CE Segnalata");
                }

                Segnalata?.Invoke(this, e);
                this.DispatchEvent(new Sistema.EventDescription("segnalata", "Richiesta CE Segnalata", e.Item));
            }

            /// <summary>
            /// Genera l'evento PresaInCarico
            /// </summary>
            /// <param name="e"></param>
            internal void doOnPresaInCarico(ItemEventArgs<CConteggioEstintivo> e)
            {
                var richiesta = (CConteggioEstintivo)e.Item;
                var cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                    {
                        cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                        cliente.Save();
                    }

                    CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.isClienteInAcquisizione = false;
                    info.isClienteAcquisito = true;
                    info.RimuoviAttenzione(richiesta, "Richiesta CE " + DBUtils.GetID(richiesta));
                    info.AggiornaOperazione(richiesta, "Richiesta CE Presa in carico da " + richiesta.PresaInCaricoDaNome);
                }

                PresaInCarico?.Invoke(this, e);
                Module.DispatchEvent(new Sistema.EventDescription("presaincarico", "Richiesta CE Presa In Carico", e.Item));
            }

            /// <summary>
            /// Elabora il template
            /// </summary>
            /// <param name="template"></param>
            /// <param name="richiesta"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public string ParseTemplate(
                                string template, 
                                CConteggioEstintivo richiesta, 
                                CKeyCollection context
                                )
            {
                var currentUser = (Sistema.CUser)context["CurrentUser"];
                string baseURL = DMD.Strings.CStr(context["BaseURL"]);
                template = Strings.Replace(template, "%%ID%%", DBUtils.GetID(richiesta).ToString());
                template = Strings.Replace(template, "%%NUMEROPRATICA%%", richiesta.NumeroPratica);
                template = Strings.Replace(template, "%%USERNAME%%", currentUser.Nominativo);
                template = Strings.Replace(template, "%%NOMINATIVOCLIENTE%%", richiesta.NomeCliente);
                template = Strings.Replace(template, "%%NOMEISTITUTO%%", richiesta.NomeIstituto);
                template = Strings.Replace(template, "%%IDCLIENTE%%", richiesta.IDCliente.ToString());
                template = Strings.Replace(template, "%%NOMEPRESAINCARICODA%%", richiesta.PresaInCaricoDaNome);
                template = Strings.Replace(template, "%%BASEURL%%", baseURL);
                return template;
            }
             

            /// <summary>
            /// Restituisce tutte le richieste di conteggio effettuate per il cliente specificato
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public CCollection<CConteggioEstintivo> GetConteggiByPersona(CPersona value)
            {
                if (value is null)
                    throw new ArgumentNullException("Persona");
                var ret = new CCollection<Finanziaria.CConteggioEstintivo>();
                if (DBUtils.GetID(value) == 0)
                    return ret;
                using (var cursor = new Finanziaria.CConteggioEstintivoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = DBUtils.GetID(value);
                    cursor.DataRichiesta.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        var r = cursor.Item;
                        r.SetCliente(value);
                        ret.Add(r);                        
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'ultima richiesta di finanziamento fatta dal cliente specificato
            /// </summary>
            /// <param name="p">[in] Persona</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CConteggioEstintivo GetUltimoConteggio(CPersonaFisica p)
            {
                if (p is null)
                    throw new ArgumentNullException("persona");
                if (DBUtils.GetID(p) == 0)
                    return null;
                using (var cursor = new Finanziaria.CConteggioEstintivoCursor())
                {
                    cursor.IDCliente.Value = DBUtils.GetID(p);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataRichiesta.SortOrder = SortEnum.SORT_DESC;
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    var ret = cursor.Item;
                    if (ret is object)
                        ret.SetCliente(p);
                    return ret;
                }
            }

            //public override void Initialize()
            //{
            //    base.Initialize();
            //    Databases.CDBTable table;
            //    Databases.CDBEntityField col;
            //    table = Finanziaria.Database.Tables.GetItemByKey("tbl_RichiesteFinanziamentiC");
            //    col = table.Fields.Alter("IDEstinzione", typeof(int));
            //    col = table.Fields.Alter("IDDOCConteggio", typeof(int));
            //    table.Update();
            //}
        }
    }

    public partial class Finanziaria
    {
        private static CConteggiEstintiviClass m_ConteggiEstintivi = null;

        /// <summary>
        /// Repository di <see cref="CConteggioEstintivo"/>
        /// </summary>
        /// <returns></returns>
        public static CConteggiEstintiviClass ConteggiEstintivi
        {
            get
            {
                if (m_ConteggiEstintivi is null)
                    m_ConteggiEstintivi = new CConteggiEstintiviClass();
                return m_ConteggiEstintivi;
            }
        }
    }
}