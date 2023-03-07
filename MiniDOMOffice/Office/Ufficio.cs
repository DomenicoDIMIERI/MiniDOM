using System;
using System.Data;

namespace minidom
{
    public sealed partial class Office
    {
        private static Sistema.CModule m_Module = null;
        private static bool m_Initialized = false;
       
        static Office()
        {

            // Initialize()
        }
 
        private static void HandlePersonaMerged(Anagrafica.MergePersonaEventArgs e)
        {
            lock (Anagrafica.@lock)
            {
                

                  
                finally
                {
                    if (dbRis is object)
                    {
                        dbRis.Dispose();
                        dbRis = null;
                    }
                }

                 
                try
                {
                    // Tabella tbl_OfficeSpedizioni 
                    dbSQL = "SELECT [ID], [NomeMittente] FROM [tbl_OfficeSpedizioni] WHERE [IDMittente]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_OfficeSpedizioni";
                        rec.FieldName = "IDMittente";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        rec.@params.SetItemByKey("NomeMittente", Sistema.Formats.ToString(dbRis["NomeMittente"]));
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    dbRis = null;
                    Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDMittente]=" + DBUtils.GetID(persona) + ", [NomeMittente]=" + DBUtils.DBString(persona.Nominativo) + " WHERE [IDMittente]=" + DBUtils.GetID(persona1));
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }
                finally
                {
                    if (dbRis is object)
                    {
                        dbRis.Dispose();
                        dbRis = null;
                    }
                }

                try
                {
                    // Tabella tbl_OfficeSpedizioni 
                    dbSQL = "SELECT [ID], [NomeDestinatario] FROM [tbl_OfficeSpedizioni] WHERE [IDDestinatario]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_OfficeSpedizioni";
                        rec.FieldName = "IDDestinatario";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        rec.@params.SetItemByKey("NomeDestinatario", Sistema.Formats.ToString(dbRis["NomeDestinatario"]));
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    dbRis = null;
                    Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDDestinatario]=" + DBUtils.GetID(persona) + ", [NomeDestinatario]=" + DBUtils.DBString(persona.Nominativo) + " WHERE [IDDestinatario]=" + DBUtils.GetID(persona1));
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }
                finally
                {
                    if (dbRis is object)
                    {
                        dbRis.Dispose();
                        dbRis = null;
                    }
                }
            }
        }

        private static string GetAffectedRecors(Anagrafica.CMergePersona mi, string tableName, string fieldName)
        {
            var ret = new System.Text.StringBuilder();
            foreach (Anagrafica.CMergePersonaRecord rec in mi.TabelleModificate)
            {
                if ((rec.NomeTabella ?? "") == (tableName ?? "") && rec.RecordID != 0 && (rec.FieldName ?? "") == (fieldName ?? ""))
                {
                    if (ret.Length > 0)
                        ret.Append(",");
                    ret.Append(DBUtils.DBNumber(rec.RecordID));
                }
            }

            return ret.ToString();
        }

        private static void HandlePeronaUnMerged(Anagrafica.MergePersonaEventArgs e)
        {
            lock (Anagrafica.@lock)
            {
                var mi = e.MI;
                var persona = mi.Persona1;
                var persona1 = mi.Persona2;
                string items;
                if (persona is Anagrafica.CPersonaFisica)
                {
                    try
                    {
                        // Tabella Login
                        items = GetAffectedRecors(mi, "tbl_OfficeCommissioni", "IDPersona");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_OfficeCommissioni] SET [IDPersona]=" + DBUtils.GetID(persona1) + ", [NomePersona]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }

                    try
                    {
                        // Tabella tbl_OfficeEstrattiC 
                        items = GetAffectedRecors(mi, "tbl_OfficeEstrattiC", "IDCliente");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_OfficeEstrattiC] SET [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }

                    try
                    {
                        // Tabella tbl_CQSPDRichCERQ 
                        items = GetAffectedRecors(mi, "tbl_CQSPDRichCERQ", "IDCliente");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] SET [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }
                }
                else
                {
                    try
                    {
                        // Tabella tbl_OfficeCommissioni 
                        items = GetAffectedRecors(mi, "tbl_OfficeCommissioni", "IDAzienda");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_OfficeCommissioni] SET [IDAzienda]=" + DBUtils.GetID(persona1) + ", [NomeAzienda]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }

                    try
                    {
                        // Tabella tbl_OfficeEstrattiC 
                        items = GetAffectedRecors(mi, "tbl_OfficeEstrattiC", "IDAmministrazione");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_OfficeEstrattiC] SET [IDAmministrazione]=" + DBUtils.GetID(persona1) + ", [NomeAmministrazione]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }

                    try
                    {
                        // Tabella tbl_CQSPDRichCERQ 
                        items = GetAffectedRecors(mi, "tbl_CQSPDRichCERQ", "IDAmministrazione");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] SET [IDAmministrazione]=" + DBUtils.GetID(persona1) + ", [NomeAmministrazione]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }
                }

                try
                {
                    // Tabella tbl_OfficeCandidature 
                    items = GetAffectedRecors(mi, "tbl_OfficeCandidature", "IDCandidato");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_OfficeCandidature] SET [IDCandidato]=" + DBUtils.GetID(persona1) + ", [NomeCandidato]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }

                try
                {
                    // Tabella tbl_OfficeCurricula 
                    items = GetAffectedRecors(mi, "tbl_OfficeCurricula", "IDPersona");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_OfficeCurricula] SET [IDPersona]=" + DBUtils.GetID(persona1) + ", [NomePersona]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }

                try
                {
                    // Tabella tbl_SupportTickets 
                    items = GetAffectedRecors(mi, "tbl_SupportTickets", "IDCliente");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_SupportTickets] SET [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }

                try
                {
                    // Tabella tbl_OfficeSpedizioni 
                    items = GetAffectedRecors(mi, "tbl_OfficeSpedizioni", "IDMittente");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDMittente]=" + DBUtils.GetID(persona1) + ", [NomeMittente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }

                try
                {
                    // Tabella tbl_OfficeSpedizioni 
                    items = GetAffectedRecors(mi, "tbl_OfficeSpedizioni", "IDDestinatario");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDDestinatario]=" + DBUtils.GetID(persona1) + ", [NomeDestinatario]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }
            }
        }

        private static void Initialize()
        {
            if (m_Initialized)
                return;
            m_Initialized = true;
            Sistema.Types.RegisteredTypeProviders.Add("Commissione", Commissioni.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("Uscita", Uscite.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("Utenza", Utenze.GetItemById);
            Magazzini.Initialize();
            CategorieArticoli.Initialize();
            Listini.Initialize();
            MarcheArticoli.Initialize();
            Articoli.Initialize();
            OggettiDaSpedire.Initialize();
            Spedizioni.Initialize();
            Anagrafica.PersonaMerged += HandlePersonaMerged;
            Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
        }

        private static void Terminate()
        {
            if (!m_Initialized)
                return;
            Anagrafica.PersonaMerged -= HandlePersonaMerged;
            Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
            Magazzini.Terminate();
            CategorieArticoli.Terminate();
            Listini.Terminate();
            MarcheArticoli.Terminate();
            Articoli.Terminate();
            OggettiDaSpedire.Terminate();
            Spedizioni.Terminate();
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("Commissione");
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("Uscita");
            m_Initialized = false;
        }

        public static Sistema.CModule Module
        {
            get
            {
                if (m_Module is null)
                    m_Module = InitModule();
                return m_Module;
            }
        }

        private static Sistema.CModule InitModule()
        {
            var ret = Sistema.Modules.GetItemByName("modOffice");
            if (ret is null)
            {
                ret = new Sistema.CModule("modOffice");
                ret.DisplayName = "Ufficio";
                ret.Description = "Ufficio";
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.Save();
            }

            return ret;
        }
    }
}