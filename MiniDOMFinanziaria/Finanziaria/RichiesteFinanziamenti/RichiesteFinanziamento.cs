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
        /// Repository di oggetti di tipo <see cref="CRichiestaFinanziamento"/>
        /// </summary>
        [Serializable]
        public sealed class CRichiesteFinanziamentoClass 
            : CModulesClass<CRichiestaFinanziamento>
        {
            
            
               
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRichiesteFinanziamentoClass() 
                : base("modCQSPDRichieste", typeof(CRichiesteFinanziamentoCursor))
            {
            }

            /// <summary>
            /// Genera l'evento ItemCreated
            /// </summary>
            /// <param name="e"></param>
            protected override void OnItemCreated(ItemEventArgs<CRichiestaFinanziamento> e)
            {
                base.OnItemCreated(e);

                var richiesta = e.Item;
                var cliente = richiesta.Cliente;
                
                if (
                      (!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue 
                    || 
                        cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true
                   )
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                var info = minidom.CustomerCalls.CRM.GetContattoInfo(cliente);
                info.isClienteInAcquisizione = true;
                
                if (richiesta.AssegnatoA is null)
                    info.AggiungiAttenzione(richiesta, "Richiesta Finanziamento non ancora presa in carico", "Richiesta Fin " + DBUtils.GetID(richiesta) + " In Carico");
                if (richiesta.IDModulo == 0)
                    info.AggiungiAttenzione(richiesta, "Modulo Richiesta Finanziamento non caricato", "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo richiesta");
                if (richiesta.IDPrivacy == 0)
                    info.AggiungiAttenzione(richiesta, "Modulo Privacy non caricato", "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo privacy");
                info.AggiornaOperazione(richiesta, "Nuova Richiesta di Finanziamento");
                
                //Module.DispatchEvent(new Sistema.EventDescription("Create", "Richiesta di Finanziamento Inserita: " + richiesta.NomeCliente, richiesta));
            }

            /// <summary>
            /// Genera l'evento
            /// </summary>
            /// <param name="e"></param>
            protected override void OnItemModified(ItemEventArgs<CRichiestaFinanziamento> e)
            {
                base.OnItemModified(e);

                var richiesta = e.Item;
                var cliente = richiesta.Cliente;
                
                var info = minidom.CustomerCalls.CRM.GetContattoInfo(cliente);
                info.isClienteInAcquisizione = true;
                if (richiesta.AssegnatoA is null)
                {
                    info.AggiungiAttenzione(richiesta, "Richiesta Finanziamento non ancora presa in carico", "Richiesta Fin " + DBUtils.GetID(richiesta) + " In Carico");
                }
                else
                {
                    info.RimuoviAttenzione(richiesta, "Richiesta Fin " + DBUtils.GetID(richiesta) + " In Carico");
                }

                if (richiesta.IDModulo == 0)
                {
                    info.AggiungiAttenzione(richiesta, "Modulo Richiesta Finanziamento non caricato", "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo richiesta");
                }
                else
                {
                    info.RimuoviAttenzione(richiesta, "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo richiesta");
                }

                if (richiesta.IDPrivacy == 0)
                {
                    info.AggiungiAttenzione(richiesta, "Modulo Privacy non caricato", "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo privacy");
                }
                else
                {
                    info.RimuoviAttenzione(richiesta, "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo privacy");
                }

                info.Save();
                //RichiestaModificata?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("Edit", "Richiesta di Finanziamento Modificata: " + richiesta.NomeCliente, richiesta));
            }

            /// <summary>
            /// Genera l'evento
            /// </summary>
            /// <param name="e"></param>
            protected override void OnItemDeleted(ItemEventArgs<CRichiestaFinanziamento> e)
            {
                base.OnItemDeleted(e);

                var richiesta = e.Item;
                var cliente = richiesta.Cliente;
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = minidom.CustomerCalls.CRM.GetContattoInfo(cliente);
                info.RimuoviAttenzione(richiesta, "Richiesta Fin " + DBUtils.GetID(richiesta) + " In Carico");
                info.RimuoviAttenzione(richiesta, "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo richiesta");
                info.RimuoviAttenzione(richiesta, "Richiesta Fin " + DBUtils.GetID(richiesta) + " modulo privacy");
                info.Save();
                //RichiestaEliminata?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("Delete", "Richiesta di Finanziamento Eliminata: " + richiesta.NomeCliente, richiesta));
            }

            /// <summary>
            /// Restituice un l'elenco delle richieste di finanziamento registrate con qualche anomalia (es. mancanti di documentazione o inserite oltre un certo tempo senza che sia stato effettuato uno studio di fattibilità per la richiesta)
            /// </summary>
            /// <param name="idUfficio">[in] ID dell'ufficio a cui limitare la ricerca. Se 0 la ricerca procederà su tutti gli uffici</param>
            /// <param name="idOperatore">[in] ID dell'operatore a cui limitare la ricerca. Se 0 la ricerca procederà su tutti gli operatori</param>
            /// <param name="dal">[in] Data da cui far partire la ricerca</param>
            /// <param name="al">[in] Data a cui terminare la ricerca</param>
            /// <param name="ritardoConsentito"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<OggettoAnomalo> GetAnomalie(
                                                int idUfficio, 
                                                int idOperatore, 
                                                DateTime? dal, 
                                                DateTime? al, 
                                                int ritardoConsentito = 1
                                                )
            {
                var ret = new CCollection<OggettoAnomalo>();
                OggettoAnomalo oggetto;
                CRichiestaFinanziamento richiesta;

                var richieste = new Dictionary<int, CRichiestaFinanziamento>();
                //var lst = new List<int>();
                using(var cursor = new CRichiesteFinanziamentoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Data.Between(dal, al);
                    cursor.IDPuntoOperativo.Value = idUfficio;
                    cursor.IDPresaInCaricoDa.Value = idOperatore;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        //lst.Add(DBUtils.GetID(cursor.Item, 0));
                        richieste.Add(DBUtils.GetID(cursor.Item, 0), cursor.Item);
                    }
                }


                using (var cursor = new CQSPDConsulenzaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataConsulenza.Value = dal; cursor.DataConsulenza.Operator = OP.OP_GE;
                    cursor.IDRichiesta.ValueIn(new List<int>(richieste.Keys).ToArray()); cursor.IDRichiesta.Operator = OP.OP_NOTIN;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        var consulenza = cursor.Item;
                        richiesta = richieste[consulenza.IDRichiesta];
                        int ritardo = (int)DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Day, richiesta.Data, DMD.DateUtils.Now());
                        if (ritardo > ritardoConsentito || richiesta.IDModulo == 0 || richiesta.IDPrivacy == 0)
                        {
                            oggetto = new OggettoAnomalo();
                            oggetto.Oggetto = richiesta;
                            // oggetto.Gruppo = richiesta.PresaInCaricoDa
                            // If (oggetto.Operatore Is Nothing) Then oggetto.Operatore = richiesta.CreatoDa
                            if (ritardo > ritardoConsentito)
                                oggetto.AggiungiAnomalia("La richiesta è stata inserita da " + ritardo + " giorni", 0);
                            if (richiesta.IDModulo == 0)
                                oggetto.AggiungiAnomalia("Non è stato caricato il modulo di richiesta di finanziamento", 1);
                            if (richiesta.IDPrivacy == 0)
                                oggetto.AggiungiAnomalia("Non è stata caricata la firma della privacy", 1);
                            ret.Add(oggetto);
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'ultima richiesta di finanziamento fatta dal cliente specificato
            /// </summary>
            /// <param name="pid">[ID] del cliente</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CRichiestaFinanziamento GetUltimaRichiesta(int pid)
            {
                if (pid == 0)
                    return null;
                using (var cursor = new CRichiesteFinanziamentoCursor())
                {
                    cursor.IDCliente.Value = pid;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Data.SortOrder = SortEnum.SORT_DESC;
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Carica le richieste fatte da un cliente
            /// </summary>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            public CCollection<CRichiestaFinanziamento> GetRichiesteByPersona(int idPersona)
            {
                var ret = new CCollection<CRichiestaFinanziamento>();
                if (idPersona != 0)
                {
                    return GetRichiesteByPersona(Anagrafica.Persone.GetItemById(idPersona));
                }

                return ret;
            }

            /// <summary>
            /// Carica le richieste fatte da un cliente
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public CCollection<CRichiestaFinanziamento> GetRichiesteByPersona(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                var ret = new CCollection<CRichiestaFinanziamento>();
                if (DBUtils.GetID(persona, 0) == 0)
                    return ret;

                using (var cursor = new CRichiesteFinanziamentoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = DBUtils.GetID(persona);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        var rich = cursor.Item;
                        rich.SetCliente((Anagrafica.CPersonaFisica)persona);
                        ret.Add(cursor.Item);
                    }

                }
                
                return ret;
            }

            /// <summary>
            /// Elabora il template
            /// </summary>
            /// <param name="template"></param>
            /// <param name="richiesta"></param>
            /// <param name="baseURL"></param>
            /// <returns></returns>
            public string ParseTemplate(string template, CRichiestaFinanziamento richiesta, string baseURL)
            {
                string ret = template;
                ret = Strings.Replace(ret, "%%USERNAME%%", Sistema.Users.CurrentUser.UserName);
                ret = Strings.Replace(ret, "%%NOMEASSEGNATOA%%", richiesta.NomeAssegnatoA);
                ret = Strings.Replace(ret, "%%NOMEPUNTOOPERATIVO%%", richiesta.NomePuntoOperativo);
                ret = Strings.Replace(ret, "%%NOTE%%", richiesta.Note);
                ret = Strings.Replace(ret, "%%DATA%%", DMD.Strings.CStr(richiesta.Data));
                ret = ParseNib(ret, "%%IMPORTORICHIESTO1%%", richiesta.ImportoRichiesto);
                ret = ParseNib(ret, "%%IMPORTORICHIESTO2%%", richiesta.ImportoRichiesto1);
                ret = ParseNib(ret, "%%RATAMASSIMA%%", richiesta.RataMassima);
                ret = ParseNib(ret, "%%DURATAMASSIMA%%", richiesta.DurataMassima);
                if (richiesta.Privacy is null)
                {
                    ret = Strings.Replace(ret, "%%URLPRIVACY%%", "");
                }
                else
                {
                    ret = Strings.Replace(ret, "%%URLPRIVACY%%", richiesta.Privacy.URL);
                }

                ret = Strings.Replace(ret, "%%NOMECLIENTE%%", richiesta.NomeCliente);
                ret = Strings.Replace(ret, "%%IDCLIENTE%%", richiesta.IDCliente.ToString());
                ret = Strings.Replace(ret, "%%ID%%", DBUtils.GetID(richiesta).ToString());
                ret = Strings.Replace(ret, "%%BASEURL%%", Sistema.ApplicationContext.BaseURL);
                return ret;
            }

            private string ParseNib(string text, string nib, decimal? value)
            {
                if (value.HasValue)
                {
                    return Strings.Replace(text, nib, Sistema.Formats.FormatValuta(value.Value));
                }
                else
                {
                    return Strings.Replace(text, nib, "");
                }
            }

            private string ParseNib(string text, string nib, int? value)
            {
                if (value.HasValue)
                {
                    return Strings.Replace(text, nib, Sistema.Formats.FormatInteger(value.Value));
                }
                else
                {
                    return Strings.Replace(text, nib, "");
                }
            }

            /// <summary>
            /// Restituisce le richieste pendenti
            /// </summary>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <param name="di"></param>
            /// <param name="df"></param>
            /// <returns></returns>
            public CCollection<CRichiestaFinanziamento> GetRichiestePendenti(
                                Anagrafica.CUfficio ufficio, 
                                Sistema.CUser operatore, 
                                DateTime? di, 
                                DateTime? df
                                )
            {
                var ret = new CCollection<CRichiestaFinanziamento>();

                //var lst = new List<int>();
                using (var cursor = new CRichiesteFinanziamentoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Data.Between(di, df);
                    if (ufficio is object)
                        cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio, 0);
                    if (operatore is object)
                        cursor.IDAssegnatoA.Value = DBUtils.GetID(operatore, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }
                 
              
                return ret;
            }
        }

    }

    public partial class Finanziaria
    {
      
        private static CRichiesteFinanziamentoClass m_RichiesteFinanziamento;

        public static CRichiesteFinanziamentoClass RichiesteFinanziamento
        {
            get
            {
                if (m_RichiesteFinanziamento is null)
                    m_RichiesteFinanziamento = new CRichiesteFinanziamentoClass();
                return m_RichiesteFinanziamento;
            }
        }
    }
}