using static minidom;
using static minidom.Databases;
using static minidom.Sistema;

namespace minidom
{ 
    public class MiniDOMInstaller
    {
    
        public const string = "/minidom/widgets";

        private string _context = "website";


        public MiniDOMInstaller()
        { 

        }


        public string Context
        {
            get {
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        private CGroup _grpAdministrators = null;

        public CGroup GrpAdministrators
        {
            get {
                if (this._grpAdministrators is null) this._grpAdministrators = EnsureGroup("Administrators");
                return this._grpAdministrators;
            }
        }

        private CGroup _grpCrm = null;

        public CGroup GrpCrm
        { 
            get {
                if (this._grpCrm is null) this._grpCrm = EnsureGroup("CRM");
                return this._grpCrm;
            }
        }

        private CGroup _grpAmministrativo = null;

        public CGroup GrpAmministrativo
        { 
            get {
                if (this._grpAmministrativo is null) this._grpAmministrativo = EnsureGroup("Amministrazione");
                return this._grpAmministrativo;
            }
        }

        private CGroup _grpDirettivo = null;

        public CGroup GrpDirettivo
        { 
            get {
                if (this._grpDirettivo is null) this._grpDirettivo = EnsureGroup("Direzione");
                return this._grpDirettivo;
            }
        }

        private CGroup _grpUsers = null;

        public  CGroup GrpUsers
        {
            get {
                if (this._grpUsers is null) this._grpUsers = EnsureGroup("Users");
                return this._grpUsers;
            }
        }

        public virtual CGroup[] admins()
        { 
            return new CGroup[] { GrpAdministrators };
        }

        public virtual CGroup[] officeAdmins()
        {
            return new CGroup[] { GrpAdministrators, GrpDirettivo, GrpAmministrativo };
        }
        protected virtual void CopiaDB()
        {
             
        }

        protected virtual void CopiaDB(string src, string target)
        {
            string pS = this.AC.MapPath(this.AC.DBURL + "/" + src);
            string pT = this.AC.MapPath(this.AC.DBURL + "/" + target);
            if (!System.IO.File.Exists(pT)) 
                System.IO.File.Copy(pS, pT);
        }


        protected virtual void DropAllTablesBut(CDBConnection db, string[] tablesToKeep)
        {
            var tbls = new CCollection<CDBTable>();
            foreach (var t in db.Tables)
            {
                if (DMD.Arrays.IndexOf(tablesToKeep, t.Name) < 0)
                    tbls.Add(t);
            }

            foreach (var t in tbls) {
#if (!DEBUG)
                try {
#endif
                t.Drop()
#if (!DEBUG)
                } catch (System.Exception ex) {

                }
#endif
            }

            foreach (var t in tbls) {
#if (!DEBUG)
                try {
#endif
                t.Drop();
#if (!DEBUG)
            } catch (System.Exception ex)
            {

            }
#endif
            }

             
        }

        public void Install()
        {
            this.InternalInstall();
        }

        protected virtual void InternalInstall()
        { 
            this.CopiaDB()

            CDBConnection db;
            string dbSQL;

            var modMotiviCommissioni = this.RegisterModule("modOfficeMotiviCommissioni", "Motivi Commissioni", Office.Module, "MotiviCommissioniHandler");


            CCollegamento lnkUfficio = this.lnkUfficio
            CCollegamento lnkUfficioAnagrafiche = this.RegisterLink("lnkUfficioAnagrafiche", "Anagrafiche", null, lnkUfficio, officeAdmins);
            CCollegamento lnkUfficioMagazzino = this.RegisterLink("lnkOfficeMagazzino", "Magazzino", null, lnkUfficio, officeAdmins);
            CCollegamento lnkUfficioMotiviCommissioni = this.RegisterLink("modOfficeMotiviCommissioni", "Motivi Commissioni", modMotiviCommissioni, lnkUfficioAnagrafiche, new CGroup[] {GrpAdministrators, GrpDirettivo, GrpAmministrativo});

            db = APPConn;


            this.AddColumn(APPConn, "tbl_PostazioniLavoro", "Categoria", "text(255)");
            this.AddColumn(APPConn, "tbl_PostazioniLavoro", "SottoCategoria", "text(255)");
            this.CreateIndex(db, "idxCATEG", "tbl_PostazioniLavoro", { "Categoria", "SottoCategoria"});

            this.AddColumn(db, "tbl_Users", "Flags", "int");

            this.RegisterModule("modPostazioni", "Postazioni di Lavoro", minidom.Office.Module, "CPostazioniModuleHandler")
            this.RegisterLink("lnkAnaPostazioni", "Postazioni di Lavoro", Anagrafica.Postazioni.Module, lnkUfficio, { GrpAdministrators, GrpAmministrativo, GrpDirettivo})
            this.GrantActions(Anagrafica.Postazioni.Module, { "list", "edit", "delete", "create"}, { GrpAdministrators, GrpAmministrativo, GrpDirettivo})
            this.RegisterPropertyPage("CPostazione", "PostazionePropPage", { GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 0)
            this.RegisterPropertyPage("CPostazione", "PostazioneContatoriPropPage", { GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 1)
            this.RegisterPropertyPage("CPostazione", "CTicketsPerObjectPropPage", { GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 5)
            this.RegisterPropertyPage("CPostazione", "CTabAttachments", { GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 5)
            this.RegisterPropertyPage("CPostazione", "CTabAnnotazioni", { GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 5)

            dbSQL = "create table [tbl_CRMMotiviAppuntamento]" +
                    "(" +
                    "[ID] Counter Primary Key," +
                    "[Nome] Text(255)," +
                    "[Flags] Int," +
                    "[TipoAppuntamento] Text(255)," +
                    "[Descrizione] Memo," +
                    "[CreatoDa] Int," +
                    "[CreatoIl] Date," +
                    "[ModificatoDa] Int," +
                    "[ModificatoIl] Date," +
                    "[Stato] Int" +
                    ")"
            this.CreateTable(db, dbSQL)
            this.CreateIndex(db, "idxCRMMOtiviRicNM", "tbl_CRMMotiviAppuntamento", { "Nome"})
            this.CreateIndex(db, "idxCRMMOtiviRicTPAPP", "tbl_CRMMotiviAppuntamento", { "TipoAppuntamento"})
            this.CreateIndex(db, "idxCRMMOtiviRicFLG", "tbl_CRMMotiviAppuntamento", { "Flags", "Descrizione"})
            this.CreateIndex(db, "idxCRMMOtiviRicCTDA", "tbl_CRMMotiviAppuntamento", { "CreatoDa", "CreatoIl"})
            this.CreateIndex(db, "idxCRMMOtiviRicSTAT", "tbl_CRMMotiviAppuntamento", { "Stato"})


            Dim m  CModule = this.RegisterModule("modCRMMotiviAppuntam", "Motivi Appuntamento", Anagrafica.Module, "MotiviAppuntamentoModuleHandler")
            m.DefinedActions.GetItemByKey("list").SetGroupAllowNegate(GrpCrm, True)
            this.RegisterLink("lnkMotiviAppuntamento", "Motivi Appuntamento", Anagrafica.MotiviAppuntamento.Module, lnkUfficioAnagrafiche, admins)
            this.RegisterPropertyPage("MotivoAppuntamento", "MotivoAppuntamentoPropertyPage", admins)

            '-----------------------
            dbSQL = "create table [tbl_Syncs]" +
                    "(" +
                    "[ID] Counter Primary Key," +
                    "[RemoteSite] Text(255)," +
                    "[ItemType] Text(255)," +
                    "[LocalID] Int," +
                    "[RemoteID] Int," +
                    "[SyncDate] Date," +
                    "[CreatoDa] Int," +
                    "[CreatoIl] Date," +
                    "[ModificatoDa] Int," +
                    "[ModificatoIl] Date," +
                    "[Stato] Int" +
                    ")"
            this.CreateTable(Sistema.SyncItems.GetConnection(), dbSQL)
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMRemoteS", "tbl_Syncs", { "RemoteSite"})
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMItemT", "tbl_Syncs", { "ItemType"})
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMLocalID", "tbl_Syncs", { "LocalID"})
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMRemoteID", "tbl_Syncs", { "RemoteID"})
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMCreatoDa", "tbl_Syncs", { "CreatoDa", "CreatoIl"})
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMModificatoDa", "tbl_Syncs", { "ModificatoDa", "ModificatoIl"})
            this.CreateIndex(Sistema.SyncItems.GetConnection(), "idxCRMStato", "tbl_Syncs", { "Stato"})

            this.CreateIndex(Sistema.Notifiche.GetConnection, "idxContext", "tbl_SYSNotify", "Context")
            this.CreateIndex(WebSite.Collegamenti.GetConnection, "idxDescrizione", "bl_Collegamenti", "Descrizione")

            '-----------------------
            this.RegisterPropertyPage("COffertaCQS", "OffertaCQSPropPage", { GrpAdministrators})
            this.RegisterPropertyPage("COffertaCQS", "CTabObjectInfo", { GrpAdministrators}, 50)


            db = Anagrafica.ListeRicontatto.Database
            this.AddColumn(db, "tbl_ListeRicontattoItems", "DettaglioStato1", "text(255)")

            db = APPConn
            this.AddColumn(db, "tbl_Ricontatti", "DettaglioStato1", "text(255)")


            '

            db = Anagrafica.ListeRicontatto.Database
            this.CreateIndex(db, "idxLstRic", "tbl_ListeRicontatto", { "Stato"})

            '***********
            db = minidom.Office.Database
            this.CreateIndex(db, "idxChatRoomsStato", "tbl_ChatRooms", { "Stato"})


            dbSQL = "create table [tbl_OfficeCartucceToner]" +
                    "(" +
                    "[ID] Counter Primary Key," +
                    "[IDArticolo] int, " +
                    "[NomeArticolo] Text(255)," +
                    "[CodiceArticolo] Text(255)," +
                    "[Modello] Text(255)," +
                    "[Descrizione] Memo," +
                    "[IDPostazione] int, " +
                    "[NomePostazione] Text(255)," +
                    "[DataAcquisto] date," +
                    "[DataInstallazione] date," +
                    "[DataEsaurimento] date," +
                    "[DataRimozione] date," +
                    "[StampeDisponibili] int," +
                    "[StampeEffettuate] int," +
                    "[Flags] Int," +
                    "[IDPuntoOperativo] Int," +
                    "[NomePuntoOperativo] Text(255)," +
                    "[CreatoDa] Int," +
                    "[CreatoIl] Date," +
                    "[ModificatoDa] Int," +
                    "[ModificatoIl] Date," +
                    "[Stato] Int" +
                    ")"
            this.CreateTable(db, dbSQL)
            this.CreateIndex(db, "idxOFFCTNomeArt", "tbl_OfficeCartucceToner", { "IDArticolo", "NomeArticolo"})
            this.CreateIndex(db, "idxOFFCTNomeCod", "tbl_OfficeCartucceToner", { "CodiceArticolo", "Modello"})
            this.CreateIndex(db, "idxOFFCTNomeDesc", "tbl_OfficeCartucceToner", { "Flags", "Descrizione"})
            this.CreateIndex(db, "idxOFFCTNomePost", "tbl_OfficeCartucceToner", { "IDPostazione", "NomePostazione"})
            this.CreateIndex(db, "idxOFFCTNomeDate", "tbl_OfficeCartucceToner", { "DataAcquisto", "DataInstallazione", "DataEsaurimento", "DataRimozione"})
            this.CreateIndex(db, "idxOFFCTCont", "tbl_OfficeCartucceToner", { "StampeDisponibili", "StampeEffettuate"})
            this.CreateIndex(db, "idxOFFCTNomeIDPO", "tbl_OfficeCartucceToner", { "IDPuntoOperativo", "NomePuntoOperativo"})
            this.CreateIndex(db, "idxCRMMOtiviRicCTDA", "tbl_OfficeCartucceToner", { "CreatoDa", "CreatoIl"})
            this.CreateIndex(db, "idxCRMMOtiviRicSTAT", "tbl_OfficeCartucceToner", { "Stato"})

            this.RegisterModule("modCartucceToner", "Toner e Cartucce", Office.Module, "CartucceTonerHandler")
            this.RegisterLink("lnkOfficeCartucce", "Gestione Toner e Cartucce", Office.CartucceToners.Module, lnkUfficioMagazzino, officeAdmins)
            this.RegisterPropertyPage("CartucciaToner", "CartucciaTonerPropPage", officeAdmins)

            'Tickets
            dbSQL = ""
            dbSQL &= "CREATE TABLE [tbl_SupportTicketsAnswares] ("
            dbSQL &= "[ID] Counter Primary Key, "
            dbSQL &= "[IDTicket] int, "
            dbSQL &= "[IDOperatore] int, "
            dbSQL &= "[NomeOperatore] text(255), "
            dbSQL &= "[Data] Date, "
            dbSQL &= "[Messaggio] Memo, "
            dbSQL &= "[StatoTicket] Int, "
            dbSQL &= "[Attachments] Memo, "
            dbSQL &= "[CreatoIl] Date, "
            dbSQL &= "[CreatoDa] Int, "
            dbSQL &= "[ModificatoIl] Date, "
            dbSQL &= "[ModificatoDa] Int, "
            dbSQL &= "[Stato] Int"
            dbSQL &= ")"

            this.CreateTable(db, dbSQL)
            this.CreateIndex(db, "idxSPTCKMSGAST", "tbl_SupportTicketsAnswares", { "Stato"})
            this.CreateIndex(db, "idxSPTCKMSGACTDA", "tbl_SupportTicketsAnswares", { "CreatoDa", "CreatoIl"})
            this.CreateIndex(db, "idxSPTCKMSGACTTCK", "tbl_SupportTicketsAnswares", { "IDTicket"})
            this.CreateIndex(db, "idxSPTCKMSGACTSCK", "tbl_SupportTicketsAnswares", { "StatoTicket", "Data"})
            this.CreateIndex(db, "idxSPTCKMSGACTOP", "tbl_SupportTicketsAnswares", { "IDOperatore", "NomeOperatore"})
            'this.CreateIndex(db, "idxSPTCKMSGACTOP", "tbl_SupportTicketsAnswares", {"Messaggio"})

            'TELDB STATS
            dbSQL = ""
            dbSQL &= "CREATE TABLE [tbl_UltimaChiamata] ("
            dbSQL &= "[ID] Counter Primary Key, "
            dbSQL &= "[IDPersona] int, "
            dbSQL &= "[IDTelefonata] int, "
            dbSQL &= "[IDTelefonataOk] int, "
            dbSQL &= "[DataUltimaTelefonataOk] date, "
            dbSQL &= "[ConteggioRisp] int, "
            dbSQL &= "[ConteggioNoRispo] int, "
            dbSQL &= "[Flags] Int, "
            dbSQL &= "[DataUltimaTelefonata] date, "
            dbSQL &= "[Note] memo, "
            dbSQL &= "[NomePersona] text(255), "
            dbSQL &= "[IDPuntoOperativo] Int, "
            dbSQL &= "[NomePuntoOperativo] text(255), "
            dbSQL &= "[MotivoProssimoRicontatto] text(255), "
            dbSQL &= "[DataProssimoRicontatto] date, "
            dbSQL &= "[IconURL] text(255), "
            dbSQL &= "[DataAggiornamento] date, "
            dbSQL &= "[DataUltimaOperazione] date, "
            dbSQL &= "[Gravita] int, "
            dbSQL &= "[Condizioni] Memo, "
            dbSQL &= "[ContaCondizioni] int, "
            dbSQL &= "[IDUltimaVisita] Int, "
            dbSQL &= "[DataUltimaVisita] Date, "
            dbSQL &= "[DataAggiornamento1] text(14), "
            dbSQL &= "[DettaglioEsito] text(255), "
            dbSQL &= "[DettaglioEsito1] text(255) "
            dbSQL &= ")"

            db = CustomerCalls.CRM.StatsDB
            this.CreateTable(db, dbSQL)
            this.CreateIndex(db, "idxUCIDPERSONA", "tbl_UltimaChiamata", { "IDPersona", "NomePersona"})
            this.CreateIndex(db, "idxUCIDTEL", "tbl_UltimaChiamata", { "IDTelefonata", "DataUltimaTelefonata"})
            this.CreateIndex(db, "idxUCIDTELOK", "tbl_UltimaChiamata", { "IDTelefonataOk", "DataUltimaTelefonataOk"})
            this.CreateIndex(db, "idxUCIDTECOUNT", "tbl_UltimaChiamata", { "ConteggioRisp", "ConteggioNoRispo"})
            this.CreateIndex(db, "idxUCIDPO", "tbl_UltimaChiamata", { "IDPuntoOperativo"})
            this.CreateIndex(db, "idxUCIDMOTRIC", "tbl_UltimaChiamata", { "DataProssimoRicontatto", "MotivoProssimoRicontatto"})
            this.CreateIndex(db, "idxUCIDAGG", "tbl_UltimaChiamata", { "DataAggiornamento", "DataUltimaOperazione"})
            this.CreateIndex(db, "idxUCIDVISIT", "tbl_UltimaChiamata", { "IDUltimaVisita", "DataUltimaVisita"})
            this.CreateIndex(db, "idxUCIDAGG1", "tbl_UltimaChiamata", { "DataAggiornamento1"})
            this.CreateIndex(db, "idxUCIDESIT", "tbl_UltimaChiamata", { "DettaglioEsito", "DettaglioEsito1"})

            this.EnsureTableField(db, "tbl_CRMStats", "NomeOperatore", GetType(String), 255)
            this.EnsureTableField(db, "tbl_CRMStats", "NomePuntoOperativo", GetType(String), 255)
            this.CreateIndex(db, "idxCRMStatsRIC", "tbl_CRMStats", "Ricalcola")

            '*********************
            db = minidom.Office.Mails.Database
            this.CreateIndex(db, "idxMLSACCIDAPP", "tbl_EmailAccounts", { "ApplicationId"})
            this.CreateIndex(db, "idxMLSACCSTATO", "tbl_EmailAccounts", { "Stato"})

            this.CreateIndex(db, "idxMLSACCSTATO", "tbl_eMailAttachments", { "Stato"})

            this.CreateIndex(db, "idxOFFOBJINVSRIAL", "tbl_OfficeOggettiInventariati", { "Seriale"})
            this.CreateIndex(db, "idxOFFOBJINVCAGTE", "tbl_OfficeOggettiInventariati", { "Categoria"})

            this.AddColumn(APPConn, "tbl_Persone", "IDStatoAttuale", "int")
            this.CreateIndex(APPConn, "idxPERSONSTATT", "tbl_Persone", { "IDStatoAttuale"})

            'Anagrafica.Aziende.Module.Settings.SetValueInt("AziendaPrincipale", -211898857)
            'Response.Write(Anagrafica.Aziende.Module.Settings.GetValueInt("AziendaPrincipale", 0))
            this.AddColumn(Anagrafica.TasksDiLavorazione.Database, "tbl_TaskLavorazioneStati", "SiApplicaA", "int")
            this.AddColumn(Anagrafica.TasksDiLavorazione.Database, "tbl_TaskLavorazioneStati", "NomeHandler", "text(255)")
            this.AddColumn(Anagrafica.TasksDiLavorazione.Database, "tbl_TaskLavorazioneStati", "Descrizione2", "text(255)")
            'tbl_TaskLavorazioneRegole
            this.AddColumn(Anagrafica.TasksDiLavorazione.Database, "tbl_TaskLavorazioneRegole", "Ordine", "int")

            this.CreateIndex(Anagrafica.TasksDiLavorazione.Database, "idxTSKLAVSIAPP", "tbl_TaskLavorazioneStati", { "SiApplicaA"})
            this.CreateIndex(Anagrafica.TasksDiLavorazione.Database, "idxTSKLAVDesc2", "tbl_TaskLavorazioneStati", { "Descrizione2"})
            this.CreateIndex(Anagrafica.TasksDiLavorazione.Database, "idxTSKLAVStato", "tbl_TaskLavorazioneStati", { "Stato"})

            this.CreateIndex(Anagrafica.TasksDiLavorazione.Database, "idxREGTSKLAVSTAT", "tbl_TaskLavorazioneRegole", { "Stato"})


            this.CreateIndex(Anagrafica.TasksDiLavorazione.Database, "idxTSKLAVDEST", "tbl_TaskLavorazione", { "IDTaskDestinazione"})
            this.CreateIndex(Anagrafica.TasksDiLavorazione.Database, "idxTSKSTATO", "tbl_TaskLavorazione", { "Stato"})



            '
            m = this.RegisterModule("modAnaTaskLavorazione", "Stati di Lavorazione", Anagrafica.Module, "TasksLavorazioneHandler")
            this.RegisterPropertyPage("TaskLavorazione", "TaskLavorazionePropPage", admins)
            this.GrantActions(m, { "list", "create", "edit", "delete"}, { GrpCrm, GrpAmministrativo, GrpDirettivo})


            this.RegisterModule("modAnaStatoTaskLavorazione", "Avanzamento Stati di Lavorazione", Anagrafica.Module, "StatiTasksLavorazioneHandler")
            this.RegisterPropertyPage("StatoTaskLavorazione", "StatoTaskLavorazionePropPage", admins)

            this.RegisterLink("lnkAnaTaskLavorazione", "Stati CRM", Anagrafica.StatiTasksLavorazione.Module, lnkUfficioAnagrafiche, admins)

            For Each p  CRegisteredPropertyPage In Sistema.PropertyPages.LoadAll
                if (p.Context = "") 
                    p.Context = this.Context
                    p.Save()
                End If
                p.SetGroupAllowNegate(Sistema.Groups.KnownGroups.Administrators, True)
            Next



            Dim col  CCollection = DBUtils.GetAllOpenedConnections()
            For Each db In col
                Try
                    db.CloseDB()
                    System.Threading.Thread.Sleep(1000)
                Catch ex  Exception
                    this.Log("CloseDB -> " + ex.Message & vbCrLf & ex.StackTrace)
                End Try

                Try
                    db.OpenDB()
                Catch ex  Exception
                    this.Log("OpenDB -> " + ex.Message & vbCrLf & ex.StackTrace)
                End Try


            Next

            Sistema.Modules.Module.InitializeStandardActions()
            this.GrantActions(Sistema.Modules.Module, { "create", "list", "edit", "delete"}, { GrpAdministrators})

            Sistema.Users.Module.InitializeStandardActions()
            this.GrantActions(Sistema.Users.Module, { "create", "list", "edit", "delete"}, { GrpAdministrators})

            Sistema.Groups.Module.InitializeStandardActions()
            this.GrantActions(Sistema.Groups.Module, { "create", "list", "edit", "delete"}, { GrpAdministrators})

            Anagrafica.Module.DefinedActions.RegisterAction("list_assegnati")
            this.GrantActions(Anagrafica.Module, { "list_assegnati"}, { GrpCrm})

            Anagrafica.Persone.Module.DefinedActions.RegisterAction("list_assegnati")
            this.GrantActions(Anagrafica.Persone.Module, { "list_assegnati"}, { GrpCrm})



            Anagrafica.Module.DefinedActions.RegisterAction("changeoperatore")
            Anagrafica.Module.DefinedActions.RegisterAction("transfer")
            this.GrantActions(Anagrafica.Module, { "transfer", "changeoperatore"}, { GrpCrm, GrpAmministrativo, GrpDirettivo})

            'allow_fullsearch
            CustomerCalls.CRM.Module.DefinedActions.RegisterAction("allow_fullsearch")
            this.GrantActions(CustomerCalls.CRM.Module, { "allow_fullsearch"}, { GrpAdministrators})

            this.RegisterModule("modCRMMotiviAppuntamento", "Motivi Apputamento", CustomerCalls.CRM.Module, "MotiviAppuntamentoModuleHandler")
            this.GrantActions(Anagrafica.MotiviAppuntamento.Module, { "list"}, { GrpAdministrators, GrpAmministrativo, GrpDirettivo, GrpCrm, GrpUsers})
            this.GrantActions(Anagrafica.MotiviAppuntamento.Module, { "edit"}, { GrpAdministrators})
            this.GrantActions(Anagrafica.MotiviAppuntamento.Module, { "create"}, { GrpAdministrators})
            this.GrantActions(Anagrafica.MotiviAppuntamento.Module, { "delete"}, { GrpAdministrators})
            this.FillMotiviRicontattoStandard()

            dbSQL = "create table [tbl_CRMMotiviContatto]" +
                    "(" +
                    "[ID] Counter Primary Key, " +
                    "[Nome] Text(255), " +
                    "[Descrizione] Memo, " +
                    "[Flags] Int, " +
                    "[TipoContatto] Text(255), " +
                    "[CreatoDa] Int, " +
                    "[CreatoIl] Date, " +
                    "[ModificatoDa] Int, " +
                    "[ModificatoIl] Date, " +
                    "[Stato] Int" +
                    ")"
            this.CreateTable(APPConn, dbSQL)
            this.CreateIndex(APPConn, "idxCRMMOtiviConNM", "tbl_CRMMotiviContatto", { "Nome"})
            this.CreateIndex(APPConn, "idxCRMMOtiviConTPAPP", "tbl_CRMMotiviContatto", { "TipoContatto"})
            this.CreateIndex(APPConn, "idxCRMMOtiviConFLG", "tbl_CRMMotiviContatto", { "Flags", "Descrizione"})
            this.CreateIndex(APPConn, "idxCRMMOtiviConCTDA", "tbl_CRMMotiviContatto", { "CreatoDa", "CreatoIl"})
            this.CreateIndex(APPConn, "idxCRMMOtiviConSTAT", "tbl_CRMMotiviContatto", { "Stato"})
            APPConn.CloseDB()
            APPConn.OpenDB()

            this.RegisterModule("modCRMMotiviContatto", "Motivi Contatto", CustomerCalls.CRM.Module, "MotiviContattoModuleHandler")
            this.GrantActions(Anagrafica.MotiviContatto.Module, { "list"}, { GrpAdministrators, GrpAmministrativo, GrpDirettivo, GrpCrm, GrpUsers})
            this.GrantActions(Anagrafica.MotiviContatto.Module, { "edit"}, { GrpAdministrators})
            this.GrantActions(Anagrafica.MotiviContatto.Module, { "create"}, { GrpAdministrators})
            this.GrantActions(Anagrafica.MotiviContatto.Module, { "delete"}, { GrpAdministrators})
            this.FillMotiviContattoStandard()
            this.RegisterPropertyPage("MotivoContatto", "MotivoContattoPropPage", { GrpAdministrators}, 0)
            this.RegisterLink("lnkAnaMotiviContatto", "Motivi Contatto", Anagrafica.MotiviContatto.Module, lnkUfficioAnagrafiche, admins)


            this.RegisterPropertyPage("CPersonaFisica", "PersonaFisicaInfoPropPage1", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, "website.2", 1)
            this.RegisterPropertyPage("CPersonaFisica", "PersonaFisicaInfoPropPage3", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, "website.2", 1)
            this.RegisterPropertyPage("CPersonaFisica", "PersonaFisicaInfoPropPage1", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, "website.1", 1)
            this.RegisterPropertyPage("CPersonaFisica", "PersonaFisicaInfoLavCQSPD", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, "website.1", 1)
            this.RegisterPropertyPage("CPersonaFisica", "TabPersonaAltro", { GrpAdministrators}, 10)

            this.RegisterPropertyPage("CAzienda", "PersonaGiuridicaInfoPropPage1", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, "website.1", 1)
            this.RegisterPropertyPage("CAzienda", "PersonaGiuridicaInfoPropPage2", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, "website.1", 0)
            this.RegisterPropertyPage("CAzienda", "TelefonatePersonaPropPage", { GrpAdministrators, GrpCrm, GrpAmministrativo, GrpDirettivo}, 1)
            this.RegisterPropertyPage("CAzienda", "CTabAttachments", { GrpAdministrators, GrpCrm}, 5)
            this.RegisterPropertyPage("CAzienda", "CTabImpiegatiAzienda", { GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 5)
            this.RegisterPropertyPage("CAzienda", "TabPersonaAltro", { GrpAdministrators}, 10)

            this.DropAllTablesBut(Anagrafica.Luoghi.Database, { "tbl_Luoghi_Comuni", "tbl_Luoghi_Nazioni", "tbl_Luoghi_Province", "tbl_Luoghi_Regioni", "tbl_LuoghiCAP"})

            this.RegisterLinkJS("lnkCreditoVRichApprovNew", "Richieste Approvazione", "javascript:CQSPDUtils.ShowRichiesteApprovazione()", this.GetCollegamento("lnkCreditoV"), { GrpAmministrativo, GrpAdministrators, GrpDirettivo})

            this.EnsureAziendaPrincipale()

            this.CheckLinks()

            this.InstallCampagneCRM()

            this.EnsureTableField(Finanziaria.Database, "tbl_EstinzioniXEstintore", "PenaleEstinzione", GetType(Double), 0)
        }


        Protected Overridable Function CreateAziendaPrincipale()  CAziendaPrincipale
            Dim az  New CAziendaPrincipale()
            az.RagioneSociale = "Azienda Principale"
            az.Stato = ObjectStatus.OBJECT_VALID
            az.Save(True)
            return az
        }

        private _aziendaPrincipale  CAziendaPrincipale = null

        public virtual Function EnsureAziendaPrincipale()  CAziendaPrincipale
            Dim az  CAziendaPrincipale
            Try
                az = minidom.Anagrafica.Aziende.AziendaPrincipale
            Catch ex  Exception
                az = null
            End Try

            if (minidom.Anagrafica.Aziende.IDAziendaPrincipale = 0 OrElse az is null) 
                this.ExecuteCommand(minidom.Anagrafica.Persone.GetConnection(), "UPDATE tbl_Persone SET [Attributi]=''")
                minidom.Anagrafica.Aziende.AziendaPrincipale = this.CreateAziendaPrincipale()
                minidom.WebSite.Instance.Configuration.Save()

            End If

            if (this._aziendaPrincipale is null) 
                this._aziendaPrincipale = minidom.Anagrafica.Aziende.AziendaPrincipale

            End If

            return this._aziendaPrincipale
        }

        private _lnkUfficio  CCollegamento = null

        public lnkUfficio  CCollegamento
            get {
                if (this._lnkUfficio is null)  this._lnkUfficio = this.RegisterLink("lnkUfficio", "Ufficio", null, null, officeAdmins)
                return this._lnkUfficio
            }
        }

        protected virtual void InstallCampagneCRM()
            Dim mp  CModule = Sistema.Modules.GetItemByName("modCRM")
            Dim m  CModule = this.RegisterModule("modCRMCampains", "Campagne CRM", mp, "CCampagnaCRMModuleHandler")
            this.GrantActions(m, { "list"}, { GrpAdministrators, GrpAmministrativo, GrpDirettivo, GrpCrm})
            this.GrantActions(m, { "edit", "create", "delete"}, { GrpAdministrators, GrpAmministrativo, GrpDirettivo})

            Dim lnkUfficio  CCollegamento = this.lnkUfficio
            Dim l  CCollegamento = this.RegisterLink("lnkCampagneCRM", "Campagne CRM", m, lnkUfficio, {GrpAdministrators, GrpAmministrativo, GrpDirettivo, GrpCrm})

            'Dim editor  CModuleEd

            Dim pp  CRegisteredPropertyPage = this.RegisterPropertyPage("CampagnaCRM", "CampagnaCRMPropPage", {GrpAdministrators, GrpAmministrativo, GrpDirettivo}, 0)
        }

        protected virtual void CheckLinks()
            For Each l  CCollegamento In WebSite.Collegamenti.LoadAll
                if (l.CallModule <> "" AndAlso Not l.CallModule.StartsWith("mod")) 
                    l.CallModule = "mod" + l.CallModule
                End If
                l.IconURL = Sistema.ApplicationContext.BaseURL + "websvc/images/default.gif"
                l.Save()
            Next
        }

        protected virtual void GrantActions(ByVal m  CModule, ByVal actionName  String, ByVal group  CGroup)
            m.DefinedActions.GetItemByKey(actionName).SetGroupAllowNegate(group, True)
        }

        protected virtual void GrantActions(ByVal m  CModule, ByVal actionNames  String(), ByVal group  CGroup)
            For Each actionName  String In actionNames
                m.DefinedActions.GetItemByKey(actionName).SetGroupAllowNegate(group, True)
            Next
        }


        protected virtual void GrantActions(ByVal m  CModule, ByVal actionNames  String(), ByVal groups  CGroup[])
            For Each grp  CGroup In groups
                this.GrantActions(m, actionNames, grp)
            Next
        }

#if 0 

     'db1.ExecuteCommand(
                '                   "CREATE TABLE [tbl_ListeRicontattoItems] (" +
                '                   "[ID] COUNTER PRIMARY KEY, " +
                '                   "[DataPrevista] Date, " +
                '                   "[IDAssegnatoA] int, " +
                '                   "[NomeAssegnatoA] Text(255), " +
                '                   "[IDPuntoOperativo] int, " +
                '                   "[NomePuntoOperativo] text(255), " +
                '                   "[Note] Memo, " +
                '                   "[DataRicontatto] Date, " +
                '                   "[StatoRicontatto] int, " +
                '                   "[IDOperatore] int, " +
                '                   "[NomeOperatore] text(255), " +
                '                   "[TipoContatto] text(255), " +
                '                   "[IDContatto] int, " +
                '                   "[IDPersona] int, " +
                '                   "[NomePersona] text(255), " +
                '                   "[SourceName] text(255), " +
                '                   "[SourceParam] text(255), " +
                '                   "[CreatoDa] int, " +
                '                   "[CreatoIl] Date, " +
                '                   "[ModificatoDa] int, " +
                '                   "[ModificatoIl] Date, " +
                '                   "[Stato] int, " +
                '                   "[Promemoria] int, " +
                '                   "[NomeLista] text(255), " +
                '                   "[NFlags] int, " +
                '                   "[Categoria] text(255), " +
                '                   "[DettaglioStato] text(255), " +
                '                   "[IDProduttore] int, " +
                '                   "[TipoAppuntamento] text(255), " +
                '                   "[NumeroOIndirizzo] text(255), " +
                '                   "[IDRicPrec] int, " +
                '                   "[IDRicSucc] int, " +
                '                   "[Priorita] int, " +
                '                   "[DataPrevistaStr] text(255), " +
                '                   "[DettaglioStato1] text(255) " +
                '                   ")"
                '                   )

                db1.ExecuteCommand("SELECT * INTO [tbl_ListeRicontattoItems] from [tbl_Ricontatti] where NomeLista<>'' and Stato=1")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMDtP] ON [tbl_ListeRicontattoItems] (DataPrevista)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMDtPS] ON [tbl_ListeRicontattoItems] (DataPrevistaStr)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMIDASS] ON [tbl_ListeRicontattoItems] (IDAssegnatoA)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMIDPO] ON [tbl_ListeRicontattoItems] (IDPuntoOperativo)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMDtR] ON [tbl_ListeRicontattoItems] (DataRicontatto)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMIDOP] ON [tbl_ListeRicontattoItems] (IDOperatore)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMIDCOnt] ON [tbl_ListeRicontattoItems] (TipoContatto, IDContatto)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMIDPER] ON [tbl_ListeRicontattoItems] (IDPersona)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMSrc] ON [tbl_ListeRicontattoItems] (SourceName, SourceParam)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMCrtD] ON [tbl_ListeRicontattoItems] (CreatoDa)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMStato] ON [tbl_ListeRicontattoItems] (Stato)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMCat] ON [tbl_ListeRicontattoItems] (Categoria)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMAPP] ON [tbl_ListeRicontattoItems] (TipoAppuntamento)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMIND] ON [tbl_ListeRicontattoItems] (NumeroOIndirizzo)")
                db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMDTSTA] ON [tbl_ListeRicontattoItems] (DettaglioStato, DettaglioStato1)")
                db1.ExecuteCommand("ALTER TABLE [tbl_ListeRicontattoItems] ADD PRIMARY KEY ([ID])")
                'db1.ExecuteCommand("CREATE INDEX [idxTBLLRICITMDTSTA] ON [tbl_ListeRicontattoItems] (IDRicPrec)")


#if usa_tbl_RicontattiQuick
table = APPConn.Tables("tbl_RicontattiQuick")
                if Not(Table.Fields.ContainsKey("DettaglioStato1")) 
                   col = Table.Fields.Add("DettaglioStato1", GetType(String)) : col.MaxLength = 255
                    Table.Update()
                End If
# End If

    
            'if Not APPConn.Tables("tbl_Persone").Fields.ContainsKey("Referente1") 
            'Dim col  CDBEntityField = APPConn.Tables("tbl_Persone").Fields.add
            'APPConn.ExecuteCommand("ALTER TABLE [tbl_Persone] Add Column [Referente1] int, [Referente2] int")
            'APPConn.ExecuteCommand("CREATE INDEX IDXPersoneRef1 On tbl_Persone ([Referente1])")
            'APPConn.ExecuteCommand("CREATE INDEX IDXPersoneRef2 On tbl_Persone ([Referente2])")
            'End If

            'Try
            '    minidom.Databases.APPConn.ExecuteCommand("ALTER TABLE tbl_Persone ADD Column DettaglioEsito Text(255)")
            '    minidom.Databases.APPConn.ExecuteCommand("CREATE INDEX idxPersoneDETTE On tbl_Persone (DettaglioEsito)")
            'Catch ex  Exception

            'End Try

    minidom.Sistema.FileSystem.CopyFile(officeDBPath, pbxDBPath)
                pbxDB = New minidom.Databases.CMdbDBConnection(pbxDBPath)
                pbxDB.OpenDB()
                pbxDB.ExecuteCommand("CREATE TABLE [tbl_OfficePBX] " +
                                                "( " +
                                                "[ID] Counter Primary Key," +
                                                "[Nome] Text(255)," +
                                                "[Tipo] Text(255)," +
                                                "[Versione] Text(255)," +
                                                "[DataInstallazione] Date," +
                                                "[DataDismissione] Date," +
                                                "[Flags] Int," +
                                                "[IDPuntoOperativo] Int," +
                                                "[NomePuntoOperativo] Text(255)," +
                                                "[CreatoDa] Int," +
                                                "[CreatoIl] Date," +
                                                "[ModificatoDa] Int," +
                                                "[ModificatoIl] Date," +
                                                "[Stato] Int" +
                                                ")")
                pbxDB.ExecuteCommand("create table [tbl_OfficeRegCall] (" +
                                                "[ID] Counter Primary Key, " +
                                                "[IDChiamata] Text(255)," +
                                                "[StatoChiamata] Int," +
                                                "[StatoChiamataEx] Text(255)," +
                                                "[EsitoChiamata] Int," +
                                                "[EsitoChiamataEx] Text(255)," +
                                                "[DataInizio] Date," +
                                                "[DataRisposta] Date," +
                                                "[DataFine] Date," +
                                                "[IDPBX] Int," +
                                                "[NomePBX] Text(255)," +
                                                "[IDChiamante] Int," +
                                                "[NomeChiamante] Text(255)," +
                                                "[IDChiamato] Int," +
                                                "[NomeChiamato] Text(255)," +
                                                "[DaNumero] Text(255)," +
                                                "[ANumero] Text(255)," +
                                                "[NomeCanale] Text(255)," +
                                                "[Qualita] Int," +
                                                "[IDPuntoOperativo] Int," +
                                                "[NomePuntoOperativo] Text(255)," +
                                                "[CreatoDa] Int," +
                                                "[CreatoIl] Date," +
                                                "[ModificatoDa] Int," +
                                                "[ModificatoIl] Date," +
                                                "[Stato] Int" +
                                                ")")
            Else
                pbxDB = New minidom.Databases.CMdbDBConnection(pbxDBPath)
                pbxDB.OpenDB()
            End If
# End If

        protected virtual void AddColumn(CDBConnection db, ByVal tblName  String, ByVal columnName  String, ByVal columnType  String)
            Try
                db.ExecuteCommand("alter table [" + tblName + "] add column [" + columnName + "] " + columnType)
            Catch ex  Exception
                this.Log("AddColumn -> " + ex.Message & vbCrLf & ex.StackTrace)
            End Try
        }

        protected virtual void CreateIndex(CDBConnection db, ByVal indexName  String, ByVal tblName  String, ByVal columnName  String)
            Try
                db.ExecuteCommand("create index [" + indexName + "] on [" + tblName + "] ([" + columnName + "])")
            Catch ex  Exception
                this.Log("CreateIndex -> " + ex.Message & vbCrLf & ex.StackTrace)
            End Try
        }

        protected virtual void CreateTable(CDBConnection db, ByVal dbSQL  String)
            Try
                db.ExecuteCommand(dbSQL)
            Catch ex  Exception
                this.Log("CreateTable -> " + ex.Message & vbCrLf & ex.StackTrace)
            End Try
        }

        protected virtual void ExecuteCommand(CDBConnection db, ByVal cmd  String)
            Try
                db.ExecuteCommand(cmd)
            Catch ex  Exception
                this.Log("ExecuteCommand (" + cmd + ") -> " + ex.Message & vbCrLf & ex.StackTrace)
            End Try
        }


        protected virtual void CreateIndex(CDBConnection db, ByVal indexName  String, ByVal tblName  String, ByVal columnNames  String())
            Try
                Dim tmp  New System.Text.StringBuilder
                Dim i  Integer
                tmp.Append("create index [")
                tmp.Append(indexName)
                tmp.Append("] on [")
                tmp.Append(tblName)
                tmp.Append("] (")
                i = 0
                For Each cn  String In columnNames
                    if (i > 0)  tmp.Append(",")
                    tmp.Append("[")
                    tmp.Append(cn)
                    tmp.Append("]")
                    i += 1
                Next
                tmp.Append(")")
                db.ExecuteCommand(tmp.ToString)
            Catch ex  Exception
                this.Log("CreateIndex -> " + ex.Message & vbCrLf & ex.StackTrace)
            End Try
        }

        protected virtual void DropIndex(CDBConnection db, ByVal tblName  String, ByVal indexName  String)
            Try
                Dim tbl  CDBTable = db.Tables(tblName)
                Dim idx  CDBTableConstraint = tbl.Constraints(indexName)
                idx.Drop()
                'tbl.Constraints.Remove(idx)
            Catch ex  Exception
                this.Log("DropIndex -> " + ex.Message & vbCrLf & ex.StackTrace)
            End Try
        }

        protected virtual void Log(ByVal text  String)
            this.AC.Log(Formats.GetTimeStamp + " " + Sistema.Users.CurrentUser.UserName + " -> " + text)
            RaiseEvent LogMessage(text)
        }

        Protected Overridable Function IsDebug()  Boolean
            return this.AC.IsDebug
        }

        public AC  MyApplicationContext
            get {
                return Sistema.ApplicationContext
            }
        }

        Public Function RegisterModule(ByVal moduleName  String, ByVal descrizione  String, ByVal parent  CModule, ByVal handler  String)  CModule
            this.Log("RegisterModule(" + moduleName + ") -> Start")

            Dim ret  CModule = null
            For Each m  CModule In Sistema.Modules.LoadAll
                if (m.ModuleName = moduleName) 
                    ret = m
                    Exit For
                End If
            Next
            if (ret is null) 
                ret = New CModule
                ret.ModuleName = moduleName
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
            End If

            ret.Description = descrizione
            ret.DisplayName = descrizione
            ret.ClassHandler = handler
            ret.Parent = parent
            ret.Save()
            ret.InitializeStandardActions()

            this.Log("RegisterModule(" + moduleName + ") -> End")
            return ret
        }

        Public Function RegisterPropertyPage(
                                            ByVal className  String,
                                            ByVal pageName  String,
                                            ByVal arrGroups  CGroup[]
                                            )  CRegisteredPropertyPage

            return this.RegisterPropertyPage(className, pageName, arrGroups, this.Context, 0)
        }

        Public Function RegisterPropertyPage(
                                            ByVal className  String,
                                            ByVal pageName  String,
                                            ByVal arrGroups  CGroup[],
                                            ByVal priority  Integer
                                            )  CRegisteredPropertyPage

            return this.RegisterPropertyPage(className, pageName, arrGroups, this.Context, priority)
        }

        Public Function RegisterPropertyPage(
                                            ByVal className  String,
                                            ByVal pageName  String,
                                            ByVal arrGroups  CGroup[],
                                            ByVal context  String,
                                            ByVal priority  Integer
                                            )  CRegisteredPropertyPage
            this.Log("RegisterPropertyPage(" + className + ", " + pageName + ") -> Start")

            Dim ret  CRegisteredPropertyPage = null
            For Each p  CRegisteredPropertyPage In Sistema.PropertyPages.LoadAll
                if (p.ClassName = className AndAlso p.TabPageClass = pageName AndAlso p.Context = context) 
                    ret = p
                    Exit For
                End If
            Next
            if (ret is null) 
                ret = New CRegisteredPropertyPage
                ret.ClassName = className
                ret.TabPageClass = pageName
            End If
            ret.Context = context
            ret.Priority = priority
            ret.Save()

            For Each g  CGroup In arrGroups
                ret.SetGroupAllowNegate(g, True)
            Next

            Sistema.PropertyPages.UpdateCached(ret)

            this.Log("RegisterPropertyPage(" + className + ", " + pageName + ") -> End")

            return ret
        }

        Protected Overridable Function RegisterLink(ByVal lnkName  String, ByVal text  String, ByVal m  CModule, ByVal parent  CCollegamento, ByVal arrGroups  CGroup[])  CCollegamento
            return this.RegisterLink(lnkName, text, m, parent, arrGroups, this.Context)
        }


        Protected Overridable Function RegisterLink(ByVal lnkName  String, ByVal text  String, ByVal m  CModule, ByVal parent  CCollegamento, ByVal arrGroups  CGroup[], ByVal context  String)  CCollegamento
            this.Log("RegisterLink(" + lnkName + ") -> Start")

            Dim ret  CCollegamento = null
            For Each l  CCollegamento In WebSite.Collegamenti.LoadAll
                if (l.Stato = ObjectStatus.OBJECT_VALID AndAlso
                    l.Nome = lnkName) 
                    ret = l
                    Exit For
                End If
            Next
            if (ret is null) 
                ret = New CCollegamento
                ret.Nome = lnkName
                ret.Stato = ObjectStatus.OBJECT_VALID
            End If
            ret.Descrizione = text
            if (m is null) 
                ret.CallModule = ""
            Else
                ret.CallModule = m.ModuleName
            End If
            ret.Parent = parent
            ret.Save()
            For Each grp  CGroup In arrGroups
                ret.SetGroupAllowNegate(grp, True)
            Next
            this.Log("RegisterLink(" + lnkName + ") -> End")
            return ret
        }

        Protected Overridable Function RegisterLinkJS(ByVal lnkName  String, ByVal text  String, ByVal script  String, ByVal parent  CCollegamento, ByVal arrGroups  CGroup[])  CCollegamento
            return this.RegisterLinkJS(lnkName, text, script, parent, arrGroups, this.Context)
        }

        Protected Overridable Function RegisterLinkJS(ByVal lnkName  String, ByVal text  String, ByVal script  String, ByVal parent  CCollegamento, ByVal arrGroups  CGroup[], ByVal context  String)  CCollegamento
            this.Log("RegisterLinkJS(" + lnkName + ") -> Start")

            Dim ret  CCollegamento = null
            For Each l  CCollegamento In WebSite.Collegamenti.LoadAll
                if (l.Stato = ObjectStatus.OBJECT_VALID AndAlso
                    l.Nome = lnkName) 
                    ret = l
                    Exit For
                End If
            Next
            if (ret is null) 
                ret = New CCollegamento
                ret.Nome = lnkName
                ret.Stato = ObjectStatus.OBJECT_VALID
            End If
            ret.Descrizione = text
            ret.Link = script
            ret.Parent = parent
            ret.Save()
            For Each grp  CGroup In arrGroups
                ret.SetGroupAllowNegate(grp, True)
            Next
            this.Log("RegisterLink(" + lnkName + ") -> End")
            return ret
        }

        Protected Overridable Function EnsureGroup(ByVal name  String)  CGroup
            this.Log("EnsureGroup(" + name + ") -> Start")
            Dim ret  CGroup = Sistema.Groups.GetItemByName(name)
            If(ret is null) 
               this.Log("EnsureGroup(" + name + ") -> new CGroup")
                ret = New CGroup(name)
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
            End If
            this.Log("EnsureGroup(" + name + ") -> End")
            return ret
        }

        protected virtual void FillMotiviContattoStandard()
            this.Log("FillMotiviContattoStandard -> Start")
            this.EnsureMotiviContatto({ "Promozione", "Rintracciare"}, "CVisita", TipoPersona.PERSONA_FISICA, False, True)
            this.EnsureMotiviContatto({ "Scopo Generico", "Firma Contratti", "Integrazione Documenti", "Prima Consulenza", "Consulenza Successiva"}, "CVisita", TipoPersona.PERSONA_FISICA, True, True)
            this.EnsureMotiviContatto({ "Richiesta Informazioni"}, "CVisita", TipoPersona.PERSONA_FISICA, True, False)

            this.EnsureMotiviContatto({ "Promozione", "Fissare Appuntamento", ""}, "CTelefonata", TipoPersona.PERSONA_FISICA, False, True)
            this.EnsureMotiviContatto({ "Scopo Generico"}, "CTelefonata", TipoPersona.PERSONA_FISICA, True, True)
            this.EnsureMotiviContatto({ "Richiesta Informazioni"}, "CTelefonata", TipoPersona.PERSONA_FISICA, True, False)

            this.EnsureMotiviContatto({ "Scopo Generico"}, "CTelefonata", TipoPersona.PERSONA_GIURIDICA, True, True)

            this.Log("FillMotiviContattoStandard -> End")
        }

        Public Function EnsureMotiviContatto(ByVal descrizioni  String(), ByVal tipoContatto  String, ByVal tipoPersona  TipoPersona, ByVal isIn  Boolean, ByVal isOut  Boolean)  CCollection(Of MotivoContatto)
            Dim ret  New CCollection(Of MotivoContatto)
            For Each str  String In descrizioni
                Dim m  MotivoContatto = this.EnsureMotivoContatto(tipoContatto & tipoPersona & DMD.Strings.OnlyChars(str), str, tipoContatto, tipoPersona, isIn, isOut)
            Next
            return ret
        }

        Public Function EnsureMotivoContatto(ByVal nome  String, ByVal descrizione  String, ByVal tipoContatto  String, ByVal tipoPersona  TipoPersona, ByVal isIn  Boolean, ByVal isOut  Boolean)  MotivoContatto
            Dim m  MotivoContatto = this.GetMotivoContatto(nome, tipoContatto, tipoPersona, isIn, isOut)
            If(m is null) 
               m = New MotivoContatto
                m.TipoContatto = tipoContatto
                m.Nome = nome
                m.Descrizione = descrizione
                if (tipoPersona = TipoPersona.PERSONA_FISICA) 
                    m.Flags = SetFlag(m.Flags, MotivoContattoFlags.PersoneFisiche, True)
                Else
                    m.Flags = SetFlag(m.Flags, MotivoContattoFlags.PersoneGiuridiche, True)
                End If
                if (isIn)  m.Flags = SetFlag(m.Flags, MotivoContattoFlags.InEntrata, True)
                if (isOut)  m.Flags = SetFlag(m.Flags, MotivoContattoFlags.InUscita, True)
                m.Stato = ObjectStatus.OBJECT_VALID
                m.Save()
            End If
            return m
        }

        protected virtual void FillMotiviRicontattoStandard()
            this.Log("FillMotiviRicontattoStandard -> Start")
            this.EnsureMotiviAppuntamento({ "Da Internet", "Fissare Appuntamento", "Promozione", "Rinnovo", "Comunicare Preventivo", "Richiedere Documenti"}, "Telefonata", TipoPersona.PERSONA_FISICA)
            this.EnsureMotiviAppuntamento({ "Promozione", "Richiedere Documenti", "Consulenza"}, "Appuntamento", TipoPersona.PERSONA_FISICA)
            this.EnsureMotiviAppuntamento({ "Verifica Fattibilità", "Telegramma Inviato", "Inviare Telegramma", "Inviare SMS", "Interessato"}, "Promemoria", TipoPersona.PERSONA_FISICA)
            this.Log("FillMotiviRicontattoStandard -> End")
        }

        Public Function EnsureMotiviAppuntamento(ByVal descrizioni  String(), ByVal tipoContatto  String, ByVal tipoPersona  TipoPersona)  CCollection(Of MotivoAppuntamento)
            Dim ret  New CCollection(Of MotivoAppuntamento)
            For Each str  String In descrizioni
                Dim m  MotivoAppuntamento = this.EnsureMotivoAppuntamento(tipoContatto & tipoPersona & DMD.Strings.OnlyChars(str), str, tipoContatto, tipoPersona)
            Next
            return ret
        }

        Public Function EnsureMotivoAppuntamento(ByVal nome  String, ByVal descrizione  String, ByVal tipoContatto  String, ByVal tipoPersona  TipoPersona)  MotivoAppuntamento
            Dim m  MotivoAppuntamento = this.GetMotivoAppuntamento(nome, tipoContatto, tipoPersona)
            If(m is null) 
               m = New MotivoAppuntamento
                m.TipoAppuntamento = tipoContatto
                m.Nome = nome
                m.Descrizione = descrizione
                if (tipoPersona = TipoPersona.PERSONA_FISICA) 
                    m.Flags = SetFlag(m.Flags, MotivoAppuntamentoFlags.PersoneFisiche, True)
                Else
                    m.Flags = SetFlag(m.Flags, MotivoAppuntamentoFlags.PersoneGiuridiche, True)
                End If
                m.Stato = ObjectStatus.OBJECT_VALID
                m.Save()
            End If
            return m
        }

        Public Function GetMotivoAppuntamento(ByVal nome  String, ByVal tipoContatto  String, ByVal tipoPersona  TipoPersona)  MotivoAppuntamento
            For Each m  MotivoAppuntamento In Anagrafica.MotiviAppuntamento.LoadAll
                if (m.Nome = nome AndAlso m.TipoAppuntamento = tipoContatto) 
                    If(tipoPersona = TipoPersona.PERSONA_FISICA) 
                       If(TestFlag(m.Flags, MotivoAppuntamentoFlags.PersoneFisiche)) 
                          return m
                        End If
                    Else
                        if (TestFlag(m.Flags, MotivoAppuntamentoFlags.PersoneGiuridiche)) 
                            return m
                        End If
                    End If
                End If
            Next
            return null
        }

        Public Function GetMotivoContatto(ByVal nome  String, ByVal tipoContatto  String, ByVal tipoPersona  TipoPersona, ByVal isIn  Boolean, ByVal isOut  Boolean)  MotivoContatto
            For Each m  MotivoContatto In Anagrafica.MotiviContatto.LoadAll
                Dim t  Boolean = False
                if (m.Nome = nome AndAlso m.TipoContatto = tipoContatto) 
                    If(isIn)  t = t And TestFlag(m.Flags, MotivoContattoFlags.InEntrata)
                    If(isOut)  t = t And TestFlag(m.Flags, MotivoContattoFlags.InUscita)

                    If(tipoPersona = TipoPersona.PERSONA_FISICA) 
                       If(TestFlag(m.Flags, MotivoContattoFlags.PersoneFisiche)) 
                          t = t And True
                        End If
                    Else
                        if (TestFlag(m.Flags, MotivoContattoFlags.PersoneGiuridiche)) 
                            t = t And True
                        End If
                    End If
                End If

                if (t)  return m
            Next
            return null
        }

        Protected Overridable Function GetCollegamento(ByVal name  String)  CCollegamento
            For Each l  CCollegamento In Collegamenti.LoadAll
                if (DMD.Strings.Compare(l.Nome, name, True) = 0)  return l
            Next
            return null
        }

        Protected Overridable Function EnsureTableField(ByVal conn  CDBConnection, ByVal tableName  String, ByVal fieldName  String, ByVal fieldType  System.Type, ByVal maxLen  Integer)  CDBEntityField
            Dim tbl  CDBTable = conn.Tables.GetItemByKey(tableName)
            If(tbl is null)  Throw New ArgumentNullException("Table not found " + tableName)
            Dim field  CDBEntityField = tbl.Fields.GetItemByKey(fieldName)
            If(field is null) 
               field = tbl.Fields.Add(fieldName, fieldType)
                field.MaxLength = maxLen
            Else
                field = tbl.Fields.Alter(fieldName, fieldType)
                field.MaxLength = maxLen
            End If
            tbl.Update()
            return field
        }

    End Class


End Namespace
