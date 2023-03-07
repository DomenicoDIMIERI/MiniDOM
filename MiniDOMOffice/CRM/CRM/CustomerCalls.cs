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
using static minidom.CustomerCalls;
using DMD.FAX;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Evento relativo ad un contatto utente
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        public delegate void ContattoEventHandler(object sender, ItemEventArgs<CContattoUtente> e);

        /// <summary>
        /// Repository di contatti
        /// </summary>
        [Serializable]
        public sealed class CCRMClass 
            : CModulesClass<CustomerCalls.CContattoUtente>
        {

            /// <summary>
            /// Evento generato quando inizia un contatto (inizio chiamata, inizio ricezione fax, ecc)
            /// </summary>
            /// <remarks></remarks>
            public event ContattoEventHandler ContattoIniziato;

            /// <summary>
            /// Evento generato quando termina un contatto (fine chiamata, fine ricezione fax, ecc)
            /// </summary>
            /// <remarks></remarks>
            public event ContattoEventHandler ContattoConcluso;


            /// <summary>
            /// Evento generato quando viene modificata la configurazione del CRM
            /// </summary>
            /// <remarks></remarks>
            public event DMDEventHandler ConfigurationChanged;


           
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CCRMClass() 
                : base("modCustomerCalls", typeof(CustomerCalls.CContattoUtenteCursor))
            {
                minidom.Anagrafica.PersonaMerged += HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
                minidom.Anagrafica.PersonaModified += HandlePersonaModified;
                minidom.Anagrafica.PersonaCreated += HandlePersonaModified;

                minidom.Anagrafica.Ricontatti.ItemModified += HandleRicontattoModified;
                minidom.Anagrafica.Ricontatti.ItemCreated += HandleRicontattoModified;


                minidom.Sistema.FaxService.FaxDelivered += handleFaxDelivered;
                minidom.Sistema.FaxService.FaxFailed += handleFaxFailed;
                minidom.Sistema.FaxService.FaxReceived += handleFaxReceived;

                minidom.Sistema.SMSService.+= handleFaxDelivered;
                minidom.Sistema.SMSService.FaxFailed += handleFaxFailed;
                minidom.Sistema.SMSService.SMSReceived += handleFaxReceived;

                //Sistema.Types.RegisteredTypeProviders.Add("CTelefonata", GetItemById);
                //Sistema.Types.RegisteredTypeProviders.Add("CVisita", GetItemById);
                //Sistema.Types.RegisteredTypeProviders.Add("SMSMessage", GetItemById);
                //Sistema.Types.RegisteredTypeProviders.Add("FaxDocument", GetItemById);
                //Sistema.Types.RegisteredTypeProviders.Add("CAppunto", GetItemById);
            }

            /// <summary>
            /// Restituisce tutti i contatti relativi alla persona
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.CContattoUtente> GetContattiByPersona(Anagrafica.CPersona p)
            {
                var ret = new CCollection<CustomerCalls.CContattoUtente>();
                using (var cursor = new CustomerCalls.CContattoUtenteCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.IDPersona.Value = DBUtils.GetID(p, 0);
                    while (cursor.Read())
                    {
                        var c = cursor.Item;
                        c.SetPersona(p);
                        ret.Add(c);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'ultimo contatto
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public CustomerCalls.CContattoUtente GetUltimoContatto(Anagrafica.CPersona p)
            {
                var info = this.GetContattoInfo(p);
                if (info is null)
                    return null;
                return info.UltimoContattoOk;
            }

            
            /// <summary>
            /// Restituisce le info per la persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public CustomerCalls.CRMStatistichePersona GetContattoInfo(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                using(var cursor = new CRMStatistichePersonaCursor())
                {
                    //string dbSQL = "SELECT * FROM [tbl_UltimaChiamata] WHERE [IDPersona]=" + pID;
                    cursor.IDPersona.Value = DBUtils.GetID(persona, 0);
                    var ret = cursor.Item;
                    if (ret is object)
                        ret.SetPersona(persona);
                    return ret;
                }
            }

            /// <summary>
        /// Aggiorna la tabella delle statistiche impostando il contatto come ultimo contatto
        /// </summary>
        /// <param name="persona"></param>
        /// <param name="telefonata"></param>
        /// <remarks></remarks>
            public void SetUltimoContatto(Anagrafica.CPersona persona, CustomerCalls.CContattoUtente telefonata)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                if (telefonata is null)
                    throw new ArgumentNullException("telefonata");
                var info = GetContattoInfo(persona);
                info.AggiornaContatto(telefonata);
            }
              
            private void handleFaxDelivered(object sender, FaxDeliverEventArgs e)
            {
                var job = e.Job;
                if (string.IsNullOrEmpty(job.JobID))
                    throw new ArgumentNullException("jobid");

                FaxDocument fax = null;
                using (var cursor = new CustomerCalls.FaxDocumentsCursor())
                {
                    cursor.MessageID.Value = job.JobID;
                    cursor.IgnoreRights = true;
                    cursor.Data.SortOrder = SortEnum.SORT_DESC;
                    fax = cursor.Item;
                }

                if (fax is object)
                {
                    fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                    fax.Esito = EsitoChiamata.OK;
                    fax.DettaglioEsito = "Inviato";
                    fax.DataRicezione = e.Job.Date;
                    fax.Save();
                }

            }

            

            private void handleFaxFailed(object sender, FaxJobEventArgs e)
            {
                var job = e.Job;
                CustomerCalls.FaxDocument fax = null;
                if (string.IsNullOrEmpty(job.JobID))
                    throw new ArgumentNullException("jobid");

                using (var cursor = new CustomerCalls.FaxDocumentsCursor())
                {
                    cursor.MessageID.Value = job.JobID;
                    cursor.IgnoreRights = true;
                    cursor.Data.SortOrder = SortEnum.SORT_DESC;
                    fax = cursor.Item;
                }

                if (fax is object)
                {
                    fax.DettaglioEsito = e.Job.JobStatusMessage;
                    fax.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                    fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                    fax.DataRicezione = e.Job.Date;
                    fax.Save();
                }
            }

           

            private object faxLock = new object();

            private void handleFaxReceived(object sender, FaxReceivedEventArgs e)
            {
                var fax = new CustomerCalls.FaxDocument();
                var job = e.Job;
                var att = new Sistema.CAttachment();
                att.Stato = ObjectStatus.OBJECT_VALID;
                att.Testo = "Fax Ricevuto da " + job.Options.SenderName + " (" + job.Options.SenderNumber + ")";
                att.Tipo = "Fax";
                string path = "/public/Received Fax Documents/";
                string name;
                string ext = Sistema.FileSystem.GetExtensionName(job.Options.FileName);
                lock (faxLock)
                {
                    Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(path));
                    name = Strings.GetRandomString(25);
                    while (Sistema.FileSystem.FileExists(Sistema.ApplicationContext.MapPath(path + name + "." + ext)))
                        name = Strings.GetRandomString(25);
                    Sistema.FileSystem.MoveFile(job.Options.FileName, Sistema.ApplicationContext.MapPath(path + name + "." + ext));
                }

                att.URL = path + name + "." + ext;
                att.Save();
                var driver = e.Job.Driver;
                string modemName = e.Job.Options.ModemName;
                var modem = driver.GetModem(modemName);
                if (modem is null && driver.Modems.Count > 0)
                    modem = driver.Modems[0];
                fax.Stato = ObjectStatus.OBJECT_VALID;
                fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                fax.Ricevuta = true;
                fax.NomeIndirizzo = job.Options.SenderName;
                fax.NumeroOIndirizzo = Sistema.Formats.ParsePhoneNumber(job.Options.SenderNumber);
                fax.Data = job.Date;
                fax.Operatore = Sistema.Users.KnownUsers.SystemUser;
                fax.Azienda = Anagrafica.Aziende.AziendaPrincipale;
                fax.Attachments.Add(att);
                fax.AccoltoDa = Sistema.Users.KnownUsers.SystemUser;
                fax.DataRicezione = DMD.DateUtils.Now();
                fax.Note = "Fax ricevuto da " + job.Options.SenderName;
                fax.Esito = CustomerCalls.EsitoChiamata.OK;
                fax.DettaglioEsito = "Ricevuto";
                fax.Parameters.SetItemByKey("FaxDriver", driver.GetUniqueID());
                fax.Parameters.SetItemByKey("FaxModemName", modemName);
                fax.Parameters.SetItemByKey("FaxJob", e.Job.JobID);
                fax.Parameters.SetItemByKey("FaxRecipient", e.Job.Options.RecipientName);
                fax.Parameters.SetItemByKey("FaxSenderName", e.Job.Options.SenderName);
                fax.Parameters.SetItemByKey("FaxSenderNumber", e.Job.Options.SenderNumber);
                if (modem is object)
                    fax.PuntoOperativo = modem.PuntoOperativo;
                fax.SetChanged(true);
                fax.Save();

                // Dim cursor As New CContattoCursor
                // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                // cursor.Valore.Value = Formats.ParsePhoneNumber(fax.NumeroOIndirizzo)
                // cursor.IgnoreRights = True
                // Dim contatto As CContatto = cursor.Item
                // cursor.Dispose()

                // If (contatto IsNot Nothing) Then
                // Dim p As CPersona = contatto.Persona
                // If (p.Stato = ObjectStatus.OBJECT_VALID) Then

                // End If
                // End If

                 
            }

         

            private void handleNuovoContatto(ItemEventArgs<CContattoUtente> e)
            {
                var ufficio = e.Item.PuntoOperativo;
                var operatore = e.Item.Operatore;
                DateTime data = (DateTime)DMD.DateUtils.GetDatePart(e.Item.Data);
                var item = CustomerCalls.CRMStatisticheOperatore.GetStats(ufficio, operatore, data);
                item.NotifyNew(e.Item);
                item.Save(true);
            }

            private void handleContattoModificato(ItemEventArgs<CContattoUtente> e)
            {
                DateTime data = (DateTime)DMD.DateUtils.GetDatePart(e.Item.Data);
                int idpo = e.Item.IDPuntoOperativo;
                int op = e.Item.IDOperatore;
                StatsDB.ExecuteCommand("UPDATE [tbl_CRMStats] SET [Ricalcola]=TRUE WHERE [IDPuntoOperativo]=" + DBUtils.DBNumber(idpo) + " AND [idOperatore]=" + DBUtils.DBNumber(op) + " AND [Data]=" + DBUtils.DBDate(data));
            }

            private void handleEliminaContatto(ItemEventArgs<CContattoUtente> e)
            {
                var item = new CustomerCalls.CRMStatisticheOperatore();
                int idPO = e.Item.IDPuntoOperativo;
                int idOperatore = e.Item.IDOperatore;
                DateTime data = (DateTime)DMD.DateUtils.GetDatePart(e.Item.Data);
                using (var dbRis = StatsDB.ExecuteReader("SELECT * FROM [tbl_CRMStats] WHERE [IDPuntoOperativo]=" + DBUtils.DBNumber(idPO) + " AND [idOperatore]=" + DBUtils.DBNumber(idOperatore) + " AND [Data]=" + DBUtils.DBDate(data)))
                {
                    if (dbRis.Read())
                        StatsDB.Load(item, dbRis);
                }
                item.IDOperatore = idOperatore;
                item.Data = data;
                item.Ricalcola = true;
                item.Save(true);
            }

            /// <summary>
            /// Restituisce l'oggetto in base all'id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public override CustomerCalls.CContattoUtente GetItemById(int id)
            {
                if (id == 0)
                    return null;

                using (var cursor = new CContattoUtenteCursor())
                {
                    cursor.ID.Value = id;
                    return cursor.Item;
                }
            }

            private Sistema.CGroup m_CRMGroup = null;

            /// <summary>
            /// Restituisce il gruppo CRM in cui vengono inseriti gli utenti che hanno accesso al CRM
            /// </summary>
            public Sistema.CGroup CRMGroup
            {
                get
                {
                    lock (this)
                    {
                        if (m_CRMGroup is null)
                        {
                            m_CRMGroup = Sistema.Groups.GetItemByName("CRM");
                        }

                        if (m_CRMGroup is null)
                        {
                            m_CRMGroup = new Sistema.CGroup("CRM");
                            m_CRMGroup.Stato = ObjectStatus.OBJECT_VALID;
                            m_CRMGroup.Save();
                        }

                        return m_CRMGroup;
                    }
                }
            }

            /// <summary>
            /// Genera l'evento ContattoIniziato
            /// </summary>
            /// <param name="e"></param>
            internal void OnContattoIniziato(ItemEventArgs<CContattoUtente> e)
            {
                if (e.Item.Persona is object)
                {
                    CustomerCalls.CRM.SetUltimoContatto(e.Item.Persona, e.Item);
                }

                ContattoIniziato?.Invoke(this, e);
            }


            /// <summary>
            /// Genera l'evento ContattoConcluso
            /// </summary>
            /// <param name="e"></param>
            internal void OnContattoConcluso(ItemEventArgs<CContattoUtente> e)
            {
                if (e.Item.Persona is object)
                {
                    CustomerCalls.CRM.SetUltimoContatto(e.Item.Persona, e.Item);
                }

                ContattoConcluso?.Invoke(this, e);
            }




            /// <summary>
            /// Restituisce tutti i contatti creati nel contesto specificato
            /// </summary>
            /// <param name="tipoContesto"></param>
            /// <param name="idContesto"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.CContattoUtente> GetContattiByContesto(string tipoContesto, int idContesto)
            {
                var ret = new CCollection<CustomerCalls.CContattoUtente>();
                
                if (string.IsNullOrEmpty(tipoContesto) && idContesto == 0)
                    return ret;

                using (var cursor = new CustomerCalls.CContattoUtenteCursor())
                {
                    if (!string.IsNullOrEmpty(tipoContesto))
                        cursor.Contesto.Value = tipoContesto;
                    if (idContesto != 0)
                        cursor.IDContesto.Value = idContesto;
                    
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Data.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }
                return ret;
            }

            /// <summary>
            /// Conta i contatti in attesa (in sede)
            /// </summary>
            /// <param name="po"></param>
            /// <param name="op"></param>
            /// <returns></returns>
            public int ContaContattiInAttesa(int? po, int? op)
            {
                return GetVisiteInAttesa(po, op).Count;
            }

            /// <summary>
            /// Conta le telefonate in attesa
            /// </summary>
            /// <param name="po"></param>
            /// <param name="op"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.ContattoInAttesaInfo> GetTelefonateInAttesa(int? po, int? op)
            {
                var ret = new CCollection<CustomerCalls.ContattoInAttesaInfo>();
                var items = CustomerCalls.Telefonate.InAttesa;
                Sistema.CUser user;
                if (op.HasValue == false)
                {
                    user = Sistema.Users.CurrentUser;
                }
                else
                {
                    user = Sistema.Users.GetItemById(op.Value);
                }

                if (po.HasValue)
                {
                    foreach (CustomerCalls.CTelefonata v in items)
                    {
                        if (v.IDPuntoOperativo == 0 || po.Value == v.IDPuntoOperativo && user.Uffici.HasOffice(v.IDPuntoOperativo))
                        {
                            ret.Add(new CustomerCalls.ContattoInAttesaInfo(v));
                        }
                    }
                }
                else
                {
                    foreach (CustomerCalls.CTelefonata v in items)
                        ret.Add(new CustomerCalls.ContattoInAttesaInfo(v));
                }

                return ret;
            }

            /// <summary>
            /// Conta le persone in attesa (in sede)
            /// </summary>
            /// <param name="po"></param>
            /// <param name="op"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.ContattoInAttesaInfo> GetVisiteInAttesa(int? po, int? op)
            {
                var ret = new CCollection<CustomerCalls.ContattoInAttesaInfo>();
                var items = CustomerCalls.Visite.InAttesa;
                Sistema.CUser user;
                if (op.HasValue == false)
                {
                    user = Sistema.Users.CurrentUser;
                }
                else
                {
                    user = Sistema.Users.GetItemById(op.Value);
                }

                if (po.HasValue)
                {
                    foreach (CustomerCalls.CVisita v in items)
                    {
                        if (v.IDPuntoOperativo == 0 || po.Value == v.IDPuntoOperativo && user.Uffici.HasOffice(v.IDPuntoOperativo))
                        {
                            ret.Add(new CustomerCalls.ContattoInAttesaInfo(v));
                        }
                    }
                }
                else
                {
                    foreach (CustomerCalls.CVisita v in items)
                        ret.Add(new CustomerCalls.ContattoInAttesaInfo(v));
                }

                return ret;
            }

            /// <summary>
            /// Gestisce l'evento PersonaMerged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaMerged(Anagrafica.MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;

                    using (var cursor = new CContattoUtenteCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.IDPersona.Value = mi.IDPersona2;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Telefonate";
                            rec.FieldName = "IDPersona";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.SetPersona(mi.Persona1);
                            cursor.Item.Save();
                        }
                    }

                    using (var cursor = new CContattoUtenteCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.IDPerContoDi.Value = mi.IDPersona2;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Telefonate";
                            rec.FieldName = "IDPerContoDi";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.SetPerContoDi(mi.Persona1);
                            cursor.Item.Save();
                        }
                    }

                    var statsPersona = minidom.CustomerCalls.CRM.GetContattoInfo(mi.Persona1);
                    if (statsPersona is object)
                    {
                        statsPersona.Ricalcola();
                    }
                }
                 
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaUnMerged(Anagrafica.MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    // Tabella tbl_Annotazioni
                    var items = mi.GetAffectedRecorsIDs("tbl_Telefonate", "IDPersona");
                    using (var cursor = new CContattoUtenteCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.SetPersona(mi.Persona2);
                            cursor.Item.Save();
                        }
                    }

                    items = mi.GetAffectedRecorsIDs("tbl_Telefonate", "IDPerContoDi");
                    using (var cursor = new CContattoUtenteCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.SetPerContoDi(mi.Persona2);
                            cursor.Item.Save();
                        }
                    }

                    var c1 = CustomerCalls.CRM.GetContattoInfo(mi.Persona1);
                    var c2 = CustomerCalls.CRM.GetContattoInfo(mi.Persona2);
                    if (c1 is object)
                        c1.Ricalcola();
                    if (c2 is object)
                        c2.Ricalcola();
                }

                 
            }

            /// <summary>
            /// Gestisce l'evento PersonaModificata
            /// </summary>
            /// <param name="e"></param>
            private void HandlePersonaModified(Anagrafica.PersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var p = e.Persona;
                    var c = CustomerCalls.CRM.GetContattoInfo(p);
                    if (c is object)
                    {
                        c.AggiornaPersona();
                        c.Save();
                    }
                }
            }

            private void HandleRicontattoModified(object sender, ItemEventArgs<CRicontatto> e)
            {
                var ric = e.Item;
                if (ric.Persona is null)
                    return;
                var c = CustomerCalls.CRM.GetContattoInfo(ric.Persona);
                if (c is object)
                {
                    c.AggiornaPersona();
                    c.AggiornaAppuntamenti();
                    c.Save();
                }
            }

            private CustomerCalls.CCRMConfig m_Config = null;


            /// <summary>
            /// Restituisce la configurazione del CRM
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CustomerCalls.CCRMConfig Config
            {
                get
                {
                    lock (this)
                    {
                        if (m_Config is null)
                        {
                            m_Config = new CustomerCalls.CCRMConfig();
                            m_Config.Load();
                        }

                        return m_Config;
                    }
                }
            }

            internal void SetConfig(CustomerCalls.CCRMConfig value)
            {
                lock (this)
                    m_Config = value;
                doConfigurationChanged(new DMDEventArgs());
            }

            /// <summary>
            /// Genera l'evento configurationChanged
            /// </summary>
            /// <param name="e"></param>
            protected void doConfigurationChanged(DMDEventArgs e)
            {
                ConfigurationChanged?.Invoke(this, e);
            }

            private List<CustomerCalls.IStoricoHandler> m_Handlers;

            /// <summary>
            /// Handlers usati per creare le voci nello storico del cliente
            /// </summary>
            public List<CustomerCalls.IStoricoHandler> Handlers
            {
                get
                {
                    lock (this)
                    {
                        if (m_Handlers is null)
                            m_Handlers = new List<CustomerCalls.IStoricoHandler>();
                        return m_Handlers;
                    }
                }
            }

            /// <summary>
            /// Restituisce lo s torico 
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.StoricoAction> GetStoricoContatti(CustomerCalls.CRMFindFilter filter)
            {
                var items = new CCollection<CustomerCalls.StoricoAction>();
                if (filter.Dal.HasValue)
                    filter.Dal = DMD.DateUtils.GetDatePart(filter.Dal);
                if (filter.Al.HasValue)
                    filter.Al = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 24 * 3600 - 1, DMD.DateUtils.GetDatePart(filter.Al));
                foreach (CustomerCalls.IStoricoHandler h in Handlers)
                {
                    var t1 = DMD.DateUtils.Now();
                    h.Aggiungi(items, filter);
                    var t2 = DMD.DateUtils.Now();
                    if ((t2 - t1).TotalMilliseconds > 1000d)
                    {
                        Sistema.ApplicationContext.Log(DMD.RunTime.vbTypeName(filter) + " - " + filter.ToString() + ": Troppo tempo " + (t2 - t1).TotalMilliseconds / 1000d + " s");
                    }
                }

                items.Sort();
                return items;
            }

            /// <summary>
            /// Restituisce la collezione dei tipi di oggetto supportati da GetStoricoContatti.
            /// Le coppie restituite sono del indicano la classe ed il nome "amichevole" della classe
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CKeyCollection<string> GetHandledTypes()
            {
                lock (this)
                {
                    var ret = new CKeyCollection<string>();
                    foreach (CustomerCalls.IStoricoHandler h in Handlers)
                    {
                        var tmp = h.GetHandledTypes();
                        foreach (string k in tmp.Keys)
                        {
                            if (ret.ContainsKey(k) == false)
                                ret.Add(k, tmp[k]);
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce la collezione dei contatti fatti dall'operatore nel periodo indicato suddivisi per persona e raggruppati per tipoPeriodo
            /// </summary>
            /// <param name="tipoPeriodo"></param>
            /// <param name="cursor"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CKeyCollection<CustomerCalls.CPersonaXPeriodo> AggregatoContatti(CustomerCalls.CContattoUtenteCursor cursor, string tipoPeriodo)
            {
                var tmp = new CKeyCollection<CustomerCalls.CPersonaXPeriodo>();

                // cursor.ClassName .Value = "CTelefonata"
                switch (Strings.LCase(Strings.Trim(tipoPeriodo)) ?? "")
                {
                    case "anno":
                        {
                            using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona], [NomePersona], [Esito], Year([Data]) as [Anno], Count(*) AS [CNT] FROM (" + cursor.GetSQL() + ") GROUP BY [IDPersona], [NomePersona], [Esito], Year([Data])"))
                            {
                                while (dbRis.Read())
                                {
                                    int pid = Sistema.Formats.ToInteger(dbRis["IDPersona"]);
                                    string nome = Sistema.Formats.ToString(dbRis["NomePersona"]);
                                    int esito = Sistema.Formats.ToInteger(dbRis["Esito"]);
                                    int anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                                    int cnt = Sistema.Formats.ToInteger(dbRis["CNT"]);
                                    string key = "P" + pid + "_" + anno;
                                    var item = tmp.GetItemByKey(key);
                                    if (item is null)
                                    {
                                        item = new CustomerCalls.CPersonaXPeriodo();
                                        item.IDPersona = pid;
                                        item.NomePersona = nome;
                                        item.Data = DMD.DateUtils.MakeDate(anno, 1, 1);
                                        tmp.Add(key, item);
                                    }

                                    if (esito == 1)
                                        item.ConteggioRisposte += cnt;
                                    item.ConteggioChiamate += cnt;
                                }
                            }

                            break;
                        }

                    case "mese":
                        {
                            using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona], [NomePersona], [Esito], Left([DataStr], 6) AS [Mese], Count(*) AS [CNT] FROM (" + cursor.GetSQL() + ") GROUP BY [IDPersona], [NomePersona],[Esito], Left([DataStr], 6)"))
                            {
                                while (dbRis.Read())
                                {
                                    int pid = Sistema.Formats.ToInteger(dbRis["IDPersona"]);
                                    string nome = Sistema.Formats.ToString(dbRis["NomePersona"]);
                                    int esito = Sistema.Formats.ToInteger(dbRis["Esito"]);
                                    string annomese = Sistema.Formats.ToString(dbRis["Mese"]);
                                    int anno = Sistema.Formats.ToInteger(Strings.Left(annomese, 4));
                                    int mese = Sistema.Formats.ToInteger(Strings.Mid(annomese, 5));
                                    int cnt = Sistema.Formats.ToInteger(dbRis["CNT"]);
                                    string key = "P" + pid + "_" + annomese;
                                    var item = tmp.GetItemByKey(key);
                                    if (item is null)
                                    {
                                        item = new CustomerCalls.CPersonaXPeriodo();
                                        item.IDPersona = pid;
                                        item.NomePersona = nome;
                                        item.Data = DMD.DateUtils.MakeDate(anno, mese, 1);
                                        tmp.Add(key, item);
                                    }

                                    if (esito == 1)
                                        item.ConteggioRisposte += cnt;
                                    item.ConteggioChiamate += cnt;
                                }
                            }

                            break;
                        }

                    case "giorno":
                        {
                            using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona], [NomePersona], [Esito], Left([DataStr], 8) as [Data], Count(*) as [CNT] FROM (" + cursor.GetSQL() + ") GROUP BY [IDPersona], [NomePersona],[Esito], Left([DataStr], 8)"))
                            {
                                while (dbRis.Read())
                                {
                                    int pid = Sistema.Formats.ToInteger(dbRis["IDPersona"]);
                                    string nome = Sistema.Formats.ToString(dbRis["NomePersona"]);
                                    int esito = Sistema.Formats.ToInteger(dbRis["Esito"]);
                                    string annomesegiorno = Sistema.Formats.ToString(dbRis["Data"]);
                                    int anno = Sistema.Formats.ToInteger(Strings.Left(annomesegiorno, 4));
                                    int mese = Sistema.Formats.ToInteger(Strings.Mid(annomesegiorno, 5, 2));
                                    int giorno = Sistema.Formats.ToInteger(Strings.Mid(annomesegiorno, 7));
                                    int cnt = Sistema.Formats.ToInteger(dbRis["CNT"]);
                                    string key = "P" + pid + "_" + annomesegiorno;
                                    var item = tmp.GetItemByKey(key);
                                    if (item is null)
                                    {
                                        item = new CustomerCalls.CPersonaXPeriodo();
                                        item.IDPersona = pid;
                                        item.NomePersona = nome;
                                        item.Data = DMD.DateUtils.MakeDate(anno, mese, giorno);
                                        tmp.Add(key, item);
                                    }

                                    if (esito == 1)
                                        item.ConteggioRisposte += cnt;
                                    item.ConteggioChiamate += cnt;
                                }
                            }

                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException("tipoPeriodo non supportato");
                        }
                }

                return tmp; // New CCollection(Of CPersonaXPeriodo)(tmp)
            }

            /// <summary>
            /// Restituisce la collezione del utenti abilitati al CRM e visibili all'utente specificato
            /// </summary>
            /// <returns></returns>
            public CCollection<Sistema.CUser> GetOperatoriCRM(Sistema.CUser u)
            {
                var CRMGroup = Sistema.Groups.GetItemByName("CRM");
                var ret = new CKeyCollection();
                foreach (Anagrafica.CUfficio ufficio in u.Uffici)
                {
                    foreach (var user in ufficio.Utenti)
                    {
                        if (user.Stato == ObjectStatus.OBJECT_VALID && !ret.ContainsKey(user.UserName))
                        {
                            if (CRMGroup.Members.GetItemById(DBUtils.GetID(user)) is object)
                                ret.Add(user.UserName, user);
                        }
                    }
                }

                ret.Sort();
                return new CCollection<Sistema.CUser>(ret);
            }

            /// <summary>
            /// Restituisce la collezione del utenti abilitati al CRM e visibili all'utente corrente
            /// </summary>
            /// <returns></returns>
            public CCollection<Sistema.CUser> GetOperatoriCRM()
            {
                return GetOperatoriCRM(Sistema.Users.CurrentUser);
            }

            /// <summary>
            /// Restituisce le statistiche per operatore
            /// </summary>
            /// <param name="cursor"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.CRMStatisticheOperatore> GetStatistichePerOperatore(
                                        CRMStatisticheOperatoreCursor cursor
                                        )
            {
                var users = GetOperatoriCRM();
                var lst = new List<int>(users.Count + 1);
                for (int i = 0, loopTo = users.Count - 1; i <= loopTo; i++)
                    lst.Add(DBUtils.GetID(users[i], 0));
                CustomerCalls.CRMStatisticheOperatore sum;
                var ret = new CKeyCollection<CustomerCalls.CRMStatisticheOperatore>();
                cursor.IDOperatore.ValueIn(lst.ToArray());
                while (cursor.Read())
                {
                    var item = cursor.Item;
                    string key = "K" + item.IDOperatore + "_" + item.Data;
                    sum = ret.GetItemByKey(key);
                    if (sum is null)
                    {
                        ret.Add(key, item);
                    }
                    else
                    {
                        sum = sum.AggregaCon(item);
                        ret[key] = sum;
                    }
                }

                cursor.Reset1();
                return new CCollection<CustomerCalls.CRMStatisticheOperatore>(ret);
            }

            /// <summary>
            /// Aggrega le statistiche operatore
            /// </summary>
            /// <param name="cursor"></param>
            /// <returns></returns>
            public CCollection<CustomerCalls.CRMStatisticheOperatore> AggregaStatisticheOperatorePerData(
                                                    CustomerCalls.CRMStatisticheOperatoreCursor cursor
                                                    )
            {
                var users = GetOperatoriCRM();
                var lst = new List<int>(users.Count + 1);
                for (int i = 0, loopTo = users.Count - 1; i <= loopTo; i++)
                    lst.Add(DBUtils.GetID(users[i], 0));
                CustomerCalls.CRMStatisticheOperatore sum;
                var ret = new CKeyCollection<CustomerCalls.CRMStatisticheOperatore>();
                cursor.IDOperatore.ValueIn(lst.ToArray());
                while (cursor.Read())
                {
                    var item = cursor.Item;
                    string key = "K" + item.IDOperatore;
                    sum = ret.GetItemByKey(key);
                    if (sum is null)
                    {
                        ret.Add(key, item);
                    }
                    else
                    {
                        sum = sum.AggregaCon(item);
                        ret[key] = sum;
                    }

                }

                cursor.Reset1();
                return new CCollection<CustomerCalls.CRMStatisticheOperatore>(ret);
            }

            /// <summary>
            /// Comparer
            /// </summary>
            private class SortCPXP 
                : IComparer, IComparer<CPersonaXPeriodo> 
            {

                /// <summary>
                /// Costruttore
                /// </summary>
                public SortCPXP()
                {
                     
                }

                /// <summary>
                /// Compara due oggetti
                /// </summary>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <returns></returns>
                public int Compare(CPersonaXPeriodo a, CPersonaXPeriodo b)
                {
                    return a.IDPersona.CompareTo(b.IDPersona);
                }

                public int Compare(object x, object y)
                {
                    return this.Compare((CPersonaXPeriodo)x, (CPersonaXPeriodo)y);
                }
            }

            /// <summary>
            /// Ricalcola le statistiche CRM per la data specificata
            /// </summary>
            /// <param name="data"></param>
            /// <remarks></remarks>
            public void RicalcolaStatistiche(DateTime data)
            {
                data = (DateTime)DMD.DateUtils.GetDatePart(data);
                var toDate = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 3600 * 24 - 1, data);
                string dbSQL = "SELECT [IDPuntoOperativo], [IDOperatore], [ClassName], [Ricevuta], Count(*) As [Num],  Min([Durata]) As [MinLen], Max([Durata]) As [MaxLen], Sum([Durata]) As [TotLen], Min([Attesa]) As [MinWait], Max([Attesa]) As [MaxWait], Sum([Attesa]) As [TotWait] FROM [tbl_Telefonate] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [DataStr]>='" + DBUtils.ToDBDateStr(data) + "' AND [DataStr]<'" + DBUtils.ToDBDateStr(toDate) + "' GROUP BY [IDPuntoOperativo], [IDOperatore], [ClassName], [Ricevuta]";
                using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL))
                {
                    while (dbRis.Read())
                    {
                        int IDPuntoOperativo = Sistema.Formats.ToInteger(dbRis["IDPuntoOperativo"]);
                        int IDOperatore = Sistema.Formats.ToInteger(dbRis["IDOperatore"]);
                        string className = Sistema.Formats.ToString(dbRis["ClassName"]);
                        bool ricevuta = Sistema.Formats.ToBool(dbRis["Ricevuta"]);
                        int num = Sistema.Formats.ToInteger(dbRis["Num"]);
                        double MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                        double MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                        double TotLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                        double MinWait = Sistema.Formats.ToDouble(dbRis["MinWait"]);
                        double MaxWait = Sistema.Formats.ToDouble(dbRis["MaxWait"]);
                        double TotWait = Sistema.Formats.ToDouble(dbRis["TotWait"]);
                        CustomerCalls.CRMStatisticheOperatore item = null;
                        using (var dbRis1 = StatsDB.ExecuteReader("SELECT * FROM [tbl_CRMStats] WHERE [IDPuntoOperativo]=" + DBUtils.DBNumber(IDPuntoOperativo) + " AND [IDOperatore]=" + DBUtils.DBNumber(IDOperatore) + " AND [Data]=" + DBUtils.DBDate(data)))
                        {
                            item = new CustomerCalls.CRMStatisticheOperatore();
                            if (dbRis1.Read())
                                StatsDB.Load(item, dbRis1);
                        }

                        if (item is null)
                            continue;
                        item.IDPuntoOperativo = IDPuntoOperativo;
                        item.IDOperatore = IDOperatore;
                        item.Data = data;
                        if (ricevuta)
                        {
                            switch (className ?? "")
                            {
                                case "CTelefonata":
                                    {
                                        item.InCallMaxLen = MaxLen;
                                        item.InCallMaxWait = MaxWait;
                                        item.InCallMinLen = MinLen;
                                        item.InCallMinWait = MinWait;
                                        item.InCallNum = num;
                                        break;
                                    }

                                case "CVisita":
                                    {
                                        item.InDateMaxLen = MaxLen;
                                        item.InDateMaxWait = MaxWait;
                                        item.InDateMinLen = MinLen;
                                        item.InDateMinWait = MinWait;
                                        item.InDateNum = num;
                                        break;
                                    }

                                case "SMSMessage":
                                    {
                                        item.InSMSNum = num;
                                        break;
                                    }

                                case "FaxDocument":
                                    {
                                        item.InFAXNum = num;
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            switch (className ?? "")
                            {
                                case "CTelefonata":
                                    {
                                        item.OutCallMaxLen = MaxLen;
                                        item.OutCallMaxWait = MaxWait;
                                        item.OutCallMinLen = MinLen;
                                        item.OutCallMinWait = MinWait;
                                        item.OutCallNum = num;
                                        break;
                                    }

                                case "CVisita":
                                    {
                                        item.OutDateMaxLen = MaxLen;
                                        item.OutDateMaxWait = MaxWait;
                                        item.OutDateMinLen = MinLen;
                                        item.OutDateMinWait = MinWait;
                                        item.OutDateNum = num;
                                        break;
                                    }

                                case "SMSMessage":
                                    {
                                        item.OutSMSNum = num;
                                        break;
                                    }

                                case "FaxDocument":
                                    {
                                        item.OutFAXNum = num;
                                        break;
                                    }
                            }
                        }

                        item.Save(true);
                    }
                }
            }
        }
    }

    public sealed partial class CustomerCalls
    {
        static CustomerCalls()
        {
        }

        private static CCRMClass m_CRM = null;

        /// <summary>
        /// Repository di contatti
        /// </summary>
        public static CCRMClass CRM
        {
            get
            {
                if (m_CRM is null)
                    m_CRM = new CCRMClass();
                return m_CRM;
            }
        }
    }
}