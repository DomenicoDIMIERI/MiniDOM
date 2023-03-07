using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public class ImportaFinMakerHandler 
        : CBaseModuleHandler
    {
        private const int TOLLERANZAMONTANTE = 1000;     // 1000 euro di tolleranza per il montante
        private const int TOLLERANZADATA = 200;          // 200 giorni per la data di caricamento
        private static readonly int[] ID_TipoDocumento = new[] { 1, 2, 3, 4, 5, 6 };
        private static readonly string[] Nome_TipoDocumento = new[] { "Carta d'Identità", "Patente di Guida", "Passaporto", "Porto d'armi", "Tessera Postale", "Tessera Ministeriale" };
        private System.Text.StringBuilder message;
        private Anagrafica.CListaRicontatti lista;
        private string tipoFonte;
        private IFonte fonte;
        private CKeyCollection<Anagrafica.CUfficio> m_PuntiOperativi = null;
        private string nomeLista = "";
        private CKeyCollection<Anagrafica.CFonte> m_Fonti = null;
        private string NomePO = "";

        public ImportaFinMakerHandler() : base()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return null;
        }

        public Anagrafica.CUfficio GetPuntoOperativo(string nome)
        {
            nome = Strings.LCase(Strings.Trim(nome));
            if (string.IsNullOrEmpty(nome))
                return null;
            if (m_PuntiOperativi is null)
            {
                m_PuntiOperativi = new CKeyCollection<Anagrafica.CUfficio>();
                foreach (Anagrafica.CUfficio u in Anagrafica.Uffici.GetPuntiOperativi())
                    m_PuntiOperativi.Add(Strings.LCase(u.Nome), u);
            }

            return m_PuntiOperativi.GetItemByKey(nome);
        }

        public Anagrafica.CFonte GetFonteAltro(string nomeFonte)
        {
            nomeFonte = Strings.Trim(nomeFonte);
            if (string.IsNullOrEmpty(nomeFonte))
                return null;
            if (m_Fonti is null)
            {
                m_Fonti = new CKeyCollection<Anagrafica.CFonte>();
                var cursor = new Anagrafica.CFontiCursor();
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.Tipo.Value = "Altro";
                cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
                while (!cursor.EOF())
                {
                    m_Fonti.Add(cursor.Item.Nome, cursor.Item);
                    cursor.MoveNext();
                }

                cursor.Dispose();
            }

            var ret = m_Fonti.GetItemByKey(Strings.LCase(nomeFonte));
            if (ret is null)
            {
                ret = new Anagrafica.CFonte();
                ret.Tipo = "Altro";
                ret.Nome = nomeFonte;
                ret.Stato = Databases.ObjectStatus.OBJECT_VALID;
                ret.Save();
                m_Fonti.Add(nomeFonte, ret);
            }

            return ret;
        }

        public string ImportList(string fileName, Anagrafica.CUfficio po, string nomeLista, string tipoFonte, int idFonte)
        {
            NomePO = po.Nome;
            this.nomeLista = nomeLista;
            if (!string.IsNullOrEmpty(tipoFonte) && idFonte != 0)
                fonte = Anagrafica.Fonti.GetItemById(tipoFonte, tipoFonte, idFonte);
            Databases.CXlsDBConnection xlsConn;
            if (string.IsNullOrEmpty(this.nomeLista))
                this.nomeLista = "Lista FinMaker";
            lista = Anagrafica.ListeRicontatto.GetItemByName(nomeLista);
            if (lista is null)
            {
                lista = new Anagrafica.CListaRicontatti();
                lista.Name = nomeLista;
                lista.Stato = Databases.ObjectStatus.OBJECT_VALID;
                lista.Save();
            }

            message = new System.Text.StringBuilder();
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName));
            xlsConn.OpenDB();
            Sistema.Events.StopEvents = true;
            Databases.DBUtils.StopStatistics = true;
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            if (xlsConn.Tables.ContainsKey("Anagrafe"))
                ImportaAnagrafe(xlsConn);
            if (xlsConn.Tables.ContainsKey("Amminis"))
                ImportaAmministrazioni(xlsConn);
            if (xlsConn.Tables.ContainsKey("Finanziarie"))
                ImportaFinanziarie(xlsConn);
            if (xlsConn.Tables.ContainsKey("Teledata"))
                ImportaTelefonate(xlsConn);
            if (xlsConn.Tables.ContainsKey("Pratiche"))
                ImportaPratiche(xlsConn);
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Databases.DBUtils.StopStatistics = false;
            Sistema.Events.StopEvents = false;
            xlsConn.Dispose();
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            return message.ToString();
        }

        public override string ImportList(object renderer)
        {
            if (!Module.UserCanDoAction("import"))
                throw new PermissionDeniedException(Module, "import");
            WebSite.ASP_Server.ScriptTimeout = 120000000;
            string nomeLista = Strings.Trim(this.n2str(renderer, "lr", ""));
            string fileName = Strings.Trim(this.n2str(renderer, "fp", ""));
            int idPO = (int)this.n2int(renderer, "po", 0);
            var po = Anagrafica.Uffici.GetItemById(idPO);
            tipoFonte = Strings.Trim(this.n2str(renderer, "tf", ""));
            var idFonte = this.n2int(renderer, "if");
            string ret = ImportList(fileName, po, nomeLista, tipoFonte, (int)idFonte);
            message.Length = 0;
            Module.DispatchEvent(new Sistema.EventDescription("list_imported", "Lista importata dal file " + fileName, ret));
            return ret;
        }

        private void ImportaFinanziarie(Databases.CXlsDBConnection xlsConn)
        {
            Log("Inizio l'importazione del foglio [Finanziarie]");
            var xlsTable = xlsConn.Tables["Finanziarie"];
            var xlsRis = new Databases.DBReader(xlsTable);
            while (xlsRis.Read())
            {
                string AC_CAP = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("AC_CAP", "")));
                string AC_INDIRIZZO_1 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("AC_INDIRIZZO_1", "")));
                string AC_LOCALITA = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("AC_LOCALITA", "")));
                string AC_PROVINCIA = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("AC_PROVINCIA", "")));
                string QFIN_BANCA_ABI = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_BANCA_ABI", "")));
                string QFIN_BANCA_CAB = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_BANCA_CAB", "")));
                string QFIN_CODICE = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_CODICE", "")));
                string QFIN_DENOMINAZIONE = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_DENOMINAZIONE", "")));
                if (string.IsNullOrEmpty(QFIN_DENOMINAZIONE))
                    continue;
                Anagrafica.CAzienda azienda;
                azienda = Anagrafica.Aziende.GetItemByName(QFIN_DENOMINAZIONE);
                if (azienda is null)
                {
                    Log("Creo l'amministrazione [" + QFIN_DENOMINAZIONE + "]");
                    azienda = new Anagrafica.CAzienda();
                    azienda.RagioneSociale = QFIN_DENOMINAZIONE;
                }

                if (string.IsNullOrEmpty(azienda.ResidenteA.ToponimoViaECivico))
                    azienda.ResidenteA.ToponimoViaECivico = AC_INDIRIZZO_1;
                if (string.IsNullOrEmpty(azienda.ResidenteA.CAP))
                    azienda.ResidenteA.CAP = AC_CAP;
                if (string.IsNullOrEmpty(azienda.ResidenteA.Provincia))
                    azienda.ResidenteA.Provincia = AC_PROVINCIA;
                if (string.IsNullOrEmpty(azienda.ResidenteA.Citta))
                    azienda.ResidenteA.Provincia = AC_LOCALITA;
                azienda.Stato = Databases.ObjectStatus.OBJECT_VALID;
                azienda.Save();
                AddAnnotazione("ImportaFinMaker", QFIN_DENOMINAZIONE + "_Note", azienda, xlsRis, null);
            }

            xlsRis.Dispose();
            Log("Finisco l'importazione del foglio [Finanziarie]");
            Log("------------------------------------------");
        }

        private void ParseNomeCliente(Anagrafica.CPersonaFisica persona, string value)
        {
            int p;
            string nome, cognome;
            value = Strings.Trim(Strings.Replace(value, "  ", " "));
            p = Strings.InStr(value, " ");
            if (p > 0)
            {
                cognome = DMD.Strings.Left(value, p - 1);
                nome = DMD.Strings.Mid(value, p + 1);
                switch (Strings.UCase(cognome) ?? "")
                {
                    case "DI":
                    case "DE":
                    case "DELLO":
                        {
                            int p1;
                            p1 = Strings.InStr(nome, " ");
                            if (p1 > 0)
                            {
                                cognome = cognome + " " + Strings.Left(nome, p1 - 1);
                                nome = Strings.Mid(nome, p1 + 1);
                            }

                            break;
                        }
                }
            }
            else
            {
                cognome = value;
                nome = "";
            }

            persona.Nome = nome;
            persona.Cognome = cognome;
        }

        private Finanziaria.CPraticaCQSPD CercaPraticaVicina(Anagrafica.CPersonaFisica cliente, decimal? montante, DateTime? dataCaricamento, Finanziaria.CCQSPDProdotto prodotto)
        {
            bool montanteOk, dataOk, okProdotto;
            CCollection<Finanziaria.CPraticaCQSPD> items;

            // Cerco tutte le pratiche del cliente
            items = Finanziaria.Pratiche.GetPraticheByPersona(cliente);
            // Aggiungo eventuali pratiche con lo stesso codice fiscale
            if (!string.IsNullOrEmpty(cliente.CodiceFiscale))
            {
                var cursor = new Finanziaria.CPraticheCQSPDCursor();
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.CodiceFiscale.Value = cliente.CodiceFiscale;
                cursor.IDCliente.Value = Databases.GetID(cliente);
                cursor.IDCliente.Operator = OP.OP_NE;
                while (!cursor.EOF())
                {
                    items.Add(cursor.Item);
                    Log("La pratica " + cursor.Item.NumeroPratica + " è associabile al cliente " + cliente.Nominativo + " tramite il CF (" + cliente.CodiceFiscale + ") ma appartiene ad un oggetto diverso" + DMD.Strings.vbNewLine);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
            }

            foreach (Finanziaria.CPraticaCQSPD item in items)
            {
                if ((item.StatoAttuale.MacroStato.HasValue && ((int?)item.StatoAttuale.MacroStato == (int?)Finanziaria.StatoPraticaEnum.STATO_LIQUIDATA || (int?)item.StatoAttuale.MacroStato == (int?)Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA)) == true)
                {
                    if (item.IDProdotto == Databases.GetID(prodotto))
                    {
                        okProdotto = true;
                    }
                    else if (prodotto is object && (item.NomeProdotto ?? "") == (prodotto.Nome ?? "") && (item.NomeCessionario ?? "") == (prodotto.NomeCessionario ?? ""))
                    {
                        okProdotto = true;
                    }
                    else if (prodotto is null)
                    {
                        okProdotto = true;
                    }
                    else
                    {
                        okProdotto = false;
                    }

                    if ((montante.HasValue && item.MontanteLordo > 0) == true)
                    {
                        montanteOk = Maths.Abs((double)(montante.Value - item.MontanteLordo)) <= TOLLERANZAMONTANTE;
                    }
                    else
                    {
                        montanteOk = false;
                    }

                    var dRef = item.StatoRichiestaDelibera.Data;
                    if (dRef.HasValue == false)
                        dRef = item.StatoPreventivo.Data;
                    if (dRef.HasValue == false)
                        dRef = item.StatoLiquidata.Data;
                    if (dRef.HasValue == false)
                        dRef = item.DataDecorrenza;
                    if (dataCaricamento.HasValue && dRef.HasValue)
                    {
                        dataOk = Maths.Abs(DMD.DateUtils.DateDiff(DateTimeInterval.Day, dataCaricamento.Value, dRef.Value)) <= TOLLERANZADATA;
                    }
                    else
                    {
                        dataOk = false;
                    }

                    if (okProdotto && montanteOk && dataOk)
                        return item;
                }
            }

            return null;
        }

        private string RemoveInvalidCharsFromName(string value)
        {
            string ret = Strings.Replace(value, DMD.Strings.vbCr, " ");
            ret = Strings.Replace(ret, DMD.Strings.vbLf, " ");
            ret = Strings.Replace(ret, DMD.Strings.vbNullChar, " ");
            return ret;
        }

        private void ImportaAmministrazioni(Databases.CXlsDBConnection xlsConn)
        {
            Log("Inizio l'importazione del foglio [Amminis]");
            var xlsTable = xlsConn.Tables["Amminis"];
            var xlsRis = new Databases.DBReader(xlsTable);
            while (xlsRis.Read())
            {
                string Residcitta = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residcitta", "")));
                string Amministrazione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Amministrazione", "")));
                string ResidVia = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("ResidVia", "")));
                string ResidCAP = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("ResidCAP", "")));
                string Residprov = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residprov", "")));
                string Residntel = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residntel", "")));
                string ntelrespons = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("n°tel  respons", "")));
                string Residnfax = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residnfax", "")));
                var Datainserim = Sistema.Formats.ParseDate(xlsRis.GetValue("Data inserim"), true);
                var IDamministrazione = Sistema.Formats.ParseInteger(xlsRis.GetValue("IDamministrazione"), true);
                string RelazionePratiche = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Relazione Pratiche", "")));
                string Note = xlsRis.GetValue("Note", "");
                string codfisc = Sistema.Formats.ParseCodiceFiscale(RemoveInvalidCharsFromName(xlsRis.GetValue("cod fisc", "")));
                string Respons = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Respons", "")));
                string Segnalatorepraticaincorso = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Segnalatore pratica in corso", "")));
                string Namministr = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("N° amministr", "")));
                var CapitaleSoc = Sistema.Formats.ParseValuta(xlsRis.GetValue("Capitale soc"), true);
                var Ndipendenti = Sistema.Formats.ParseInteger(xlsRis.GetValue("N° dipendenti"), true);
                Anagrafica.CAzienda azienda = null;
                if (!string.IsNullOrEmpty(codfisc))
                    azienda = Anagrafica.Aziende.GetItemByCF(codfisc);
                if (azienda is null)
                    azienda = Anagrafica.Aziende.GetItemByName(Amministrazione);
                if (azienda is null && !string.IsNullOrEmpty(Amministrazione))
                {
                    Log("Creo l'amministrazione [" + Amministrazione + "], C.F.: " + codfisc);
                    azienda = new Anagrafica.CAzienda();
                    azienda.RagioneSociale = Amministrazione;
                }

                if (azienda is null)
                    continue;
                if (string.IsNullOrEmpty(azienda.CodiceFiscale))
                    azienda.CodiceFiscale = codfisc;
                if (string.IsNullOrEmpty(azienda.ResidenteA.ToponimoViaECivico))
                    azienda.ResidenteA.ToponimoViaECivico = ResidVia;
                if (string.IsNullOrEmpty(azienda.ResidenteA.CAP))
                    azienda.ResidenteA.CAP = ResidCAP;
                if (string.IsNullOrEmpty(azienda.ResidenteA.Provincia))
                    azienda.ResidenteA.Provincia = Residprov;
                if (!string.IsNullOrEmpty(Residntel))
                    azienda.Recapiti.Add("Telefono", Residntel, Residntel);
                if (!string.IsNullOrEmpty(ntelrespons))
                    azienda.Recapiti.Add("Telefono", "Telefono Responsabile (" + Respons + ")", ntelrespons);
                if (!string.IsNullOrEmpty(Residnfax))
                    azienda.Recapiti.Add("Fax", Residnfax, "Fax");
                if ((azienda.CapitaleSociale == 0 && CapitaleSoc.HasValue) == true)
                    azienda.CapitaleSociale = CapitaleSoc;
                if ((azienda.NumeroDipendenti == 0 && Ndipendenti.HasValue) == true)
                    azienda.NumeroDipendenti = Ndipendenti;
                if (azienda.Fonte is null && fonte is object)
                {
                    azienda.TipoFonte = tipoFonte;
                    azienda.Fonte = fonte;
                }

                azienda.Stato = Databases.ObjectStatus.OBJECT_VALID;
                azienda.Save();
                AddAnnotazione("ImportaFinMaker", Amministrazione + IDamministrazione + "Note", azienda, xlsRis, null);
                if (!string.IsNullOrEmpty(Note))
                    AddAnnotazione("ImportaFinMaker", Amministrazione + IDamministrazione, azienda, Note, null);
            }

            xlsRis.Dispose();
            Log("Finisco l'importazione del foglio [Amminis]");
            Log("------------------------------------------");
        }

        private void Log(string message)
        {
            this.message.Append(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - " + message + DMD.Strings.vbNewLine);
            OnMessageLogged(message);
        }

        private void ImportaAnagrafe(Databases.CXlsDBConnection xlsConn)
        {
            bool ricontattare;
            Log("Inizio l'importazione del foglio [Anagrafe]");
            var xlsTable = xlsConn.Tables["Anagrafe"];
            var xlsRis = new Databases.DBReader(xlsTable);
            while (xlsRis.Read())
            {
                ricontattare = false;
                string key = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("key", "")));
                string D01A_TipologiaOperatoreRegistrante = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D01A_TipologiaOperatoreRegistrante", "")));
                string D01B_CodiceOperatore = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D01B_CodiceOperatore", "")));
                string D03_TipoDiIdentificazione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D03_TipoDiIdentificazione", "")));
                string D09_CodiceIdentificativoCliente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D09_CodiceIdentificativoCliente", "")));
                string UserD09_CodiceIdentificativoCliente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserD09_CodiceIdentificativoCliente", "")));
                string User2D09_CodiceIdentificativoCliente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("User2D09_CodiceIdentificativoCliente", "")));
                string User3D09_CodiceIdentificativoCliente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("User3D09_CodiceIdentificativoCliente", "")));
                string User4D09_CodiceIdentificativoCliente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("User4D09_CodiceIdentificativoCliente", "")));
                var D10_DataDiIdentificazione = Sistema.Formats.ParseDate(xlsRis.GetValue("D10_DataDiIdentificazione"), true);
                string D11_CognomeNome_RagioneSocialeA = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D11_CognomeNome_RagioneSocialeA", "")));
                string D11_CognomeNome_RagioneSocialeB = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D11_CognomeNome_RagioneSocialeB", "")));
                string D13_PaeseEsteroDiResidenza = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D13_PaeseEsteroDiResidenza", "")));
                string D14_ComuneDiResidenzaAnagrafica = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D14_ComuneDiResidenzaAnagrafica", "")));
                string D14_B_ComuneDiResidenzaAnagrafica_DescrizioneInChiaro = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D14.B_ComuneDiResidenzaAnagrafica_DescrizioneInChiaro", "")));
                string D14_C_ComuneDiResidenzaAnagrafica_Provincia = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D14.C_ComuneDiResidenzaAnagrafica_Provincia", "")));
                string D16_Domicilio_Sede_CapDiResidenza = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D16_Domicilio_Sede_CapDiResidenza", "")));
                string D17_CodiceFiscale = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D17_CodiceFiscale", "")));
                var D18_DataDiNascita = Sistema.Formats.ParseDate(xlsRis.GetValue("D18_DataDiNascita"), true);
                string D19_ComuneDiNascita = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D19_ComuneDiNascita", "")));
                string D19_B_ProvinciaDiNascita = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D19.B_ProvinciaDiNascita", "")));
                string D19_C_StatoDiNascita = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D19.C_StatoDiNascita", "")));
                string D41_TipoDocumentoPresentato = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D41_TipoDocumentoPresentato", "")));
                string D42_NumeroDocumentoPresentato = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D42_NumeroDocumentoPresentato", "")));
                var D43_DataRilascioDocumentoPresentato = Sistema.Formats.ParseDate(xlsRis.GetValue("D43_DataRilascioDocumentoPresentato"), true);
                string D44_AutoritaeLocalitaDiRilascioDocumentoPresentato = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D44_AutoritaeLocalitaDiRilascioDocumentoPresentato", "")));
                string D45_Sesso = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D45_Sesso", "")));
                string D54_StatoDellaAnagrafica = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D54_StatoDellaAnagrafica", "")));
                string D54_A_StatoDellaAnagraficaCodiceStato = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D54.A_StatoDellaAnagraficaCodiceStato", "")));
                string D54_B_StatoDellaAnagraficaDataDellaRettifica = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D54.B_StatoDellaAnagraficaDataDellaRettifica", "")));
                string UserKeyCI = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyCI", "")));
                string UserKeySR = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeySR", "")));
                string UserKeyTR = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTR", "")));
                string UserKeyTI = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTI", "")));
                string UserKeyVE = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyVE", "")));
                string UserKeySS = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeySS", "")));
                string UserKeyNG = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyNG", "")));
                string UserKeyTL = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTL", "")));
                string UserKeyPC = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyPC", "")));
                string UserKeyXX = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyXX", "")));
                string UserKeyTP = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTP", "")));
                string UserKeyPR = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyPR", "")));
                string UserRelNG = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelNG", "")));
                string UserRelTI = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelTI", "")));
                string UserRelCI = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelCI", "")));
                string UserRelSR = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelSR", "")));
                string UserRelSS = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelSS", "")));
                string UserEtiMR = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserEtiMR", "")));
                string StatoRecordUser = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("StatoRecordUser", "")));
                string StatoRecordCalc = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("StatoRecordCalc", "")));
                string UserRelComuniEE = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelComuniEE", "")));
                string relazionefissaa = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("relazionefissaa", "")));
                string Eticmod = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Eticmod", "")));
                var DataArchiviazione = Sistema.Formats.ParseDate(xlsRis.GetValue("DataArchiviazione"), true);
                string UserZeri = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserZeri", "")));
                string DescrizioneSoggettoPerElenco = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("DescrizioneSoggettoPerElenco", "")));
                string UserNuovoPer = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserNuovoPer", "")));
                string UserRicercaPerAttivita = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserRicercaPerAttivita", "")));
                string UserSceltaSoggetto = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("UserSceltaSoggetto", "")));
                string D09_CodiceIdentificativoClientePerRelazione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D09_CodiceIdentificativoClientePerRelazione", "")));
                string D45_SessoInchiaro = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("D45_SessoInchiaro", "")));
                string PartitaIva = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("PartitaIva", "")));
                string ComuneoStatoEsterodiNascita = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("ComuneoStatoEsterodiNascita", "")));
                string ComuneoStatoEsterodiResidenza_Copia = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("ComuneoStatoEsterodiResidenza Copia", "")));
                string KeyPerRelazione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("KeyPerRelazione", "")));
                if (string.IsNullOrEmpty(D11_CognomeNome_RagioneSocialeA) && string.IsNullOrEmpty(D11_CognomeNome_RagioneSocialeB))
                    continue;

                // Elaboro la telefonata
                Anagrafica.CPersona persona;
                persona = FindPersonaByCF(D17_CodiceFiscale);
                if (persona is null)
                {
                    Log("Creo l'anagrafica della persona [" + DMD.Strings.ToNameCase(D11_CognomeNome_RagioneSocialeB) + " " + DMD.Strings.UCase(D11_CognomeNome_RagioneSocialeA));
                    // Elaboro 
                    persona = new Anagrafica.CPersonaFisica();
                    {
                        var withBlock = (Anagrafica.CPersonaFisica)persona;
                        withBlock.Cognome = D11_CognomeNome_RagioneSocialeA;
                        withBlock.Nome = D11_CognomeNome_RagioneSocialeB;
                        ricontattare = true;
                        withBlock.Save();
                    }
                }

                if (string.IsNullOrEmpty(persona.ResidenteA.Citta))
                    persona.ResidenteA.Citta = D14_ComuneDiResidenzaAnagrafica;
                if (string.IsNullOrEmpty(persona.ResidenteA.Provincia))
                    persona.ResidenteA.Citta = D14_C_ComuneDiResidenzaAnagrafica_Provincia;
                if (string.IsNullOrEmpty(persona.ResidenteA.ToponimoViaECivico))
                    persona.ResidenteA.ToponimoViaECivico = D14_B_ComuneDiResidenzaAnagrafica_DescrizioneInChiaro;
                if (string.IsNullOrEmpty(persona.ResidenteA.CAP))
                    persona.ResidenteA.CAP = D16_Domicilio_Sede_CapDiResidenza;
                if (string.IsNullOrEmpty(persona.CodiceFiscale))
                    persona.CodiceFiscale = D17_CodiceFiscale;
                if (persona.DataNascita.HasValue == false)
                    persona.DataNascita = D18_DataDiNascita;
                if (string.IsNullOrEmpty(persona.NatoA.Citta))
                    persona.NatoA.Citta = Strings.Trim(D19_C_StatoDiNascita + " &" + D19_ComuneDiNascita);
                if (string.IsNullOrEmpty(persona.NatoA.Provincia))
                    persona.NatoA.Provincia = D19_B_ProvinciaDiNascita;
                if (string.IsNullOrEmpty(persona.Sesso))
                    persona.Sesso = D45_SessoInchiaro;
                persona.Stato = Databases.ObjectStatus.OBJECT_VALID;
                persona.Save();
                AddAnnotazione("ImportaFinMaker[Anagrafe]", key, persona, xlsRis);

                // Elaboro il ricontatto
                Anagrafica.CRicontatto ricontatto;
                if (ricontattare)
                {
                    ricontatto = Anagrafica.Ricontatti.GetRicontattoBySource("ImportaFinMaker[Anagrafe]", key);
                    if (ricontatto is null)
                        ricontatto = Anagrafica.Ricontatti.ProgrammaRicontatto(persona, DMD.DateUtils.Now(), "Ricontatto importato da FinMaker", "ImportaFinMaker[Anagrafe]", key, nomeLista, null, null);
                }
            }

            xlsRis.Dispose();
            Log("Finisco l'importazione del foglio [Anagrafe]");
            Log("------------------------------------------");
        }

        private void ImportaTelefonate(Databases.CXlsDBConnection xlsConn)
        {
            bool ricontattare;
            Log("Inizio l'importazione del foglio [Teledata]");
            var xlsTable = xlsConn.Tables["Teledata"];
            var xlsRis = new Databases.DBReader(xlsTable);
            while (xlsRis.Read())
            {
                ricontattare = false;
                string Nprot = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("N° PROT", "")));
                string Cognome = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Cognome", "")));
                string Eta = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Età", "")));
                string Nome = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Nome", "")));
                string TipoPosto = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Tipo posto", "")));
                var Data = Sistema.Formats.ParseDate(xlsRis.GetValue("Data"), true);
                var Ora = Sistema.Formats.ParseDate(xlsRis.GetValue("Ora"), true);
                string Utente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Utente", "")));
                string Motivo = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Motivo", "")));
                string Tipotelefonata = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Tipo telefonata", "")));
                string Risposta = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Risposta", "")));
                string Contatto = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto", "")));
                string Contatto2 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto 2", "")));
                string Contatto3 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto 3", "")));
                string Dovelavora = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Dove lavora", "")));
                string Anzianitadiservizio = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Anzianità di servizio", "")));
                string Trattenute = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute", "")));
                var Importo = Sistema.Formats.ParseValuta(xlsRis.GetValue("Importo"), true);
                int? Residuo = (int?)Sistema.Formats.ParseValuta(xlsRis.GetValue("Residuo"), true);
                string Comeciconosce = Strings.Trim(RemoveInvalidCharsFromName(DMD.Strings.CStr(xlsRis.GetValue("Come ci conosce"))));
                string Agente2 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Agente2", "")));
                string Note = xlsRis.GetValue("Note", "");
                string Trattenutecon = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con", "")));
                string Cercava = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Cercava", "")));
                string Visto = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Visto", "")));
                string Contatto4 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto 4", "")));
                string Citta = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Città", "")));
                string Trattenutecon2 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con 2", "")));
                var Importo2 = Sistema.Formats.ParseValuta(xlsRis.GetValue("Importo 2"), true);
                int? Residuo2 = (int?)Sistema.Formats.ParseValuta(xlsRis.GetValue("Residuo 2"), true);
                var datanascita = Sistema.Formats.ParseDate(xlsRis.GetValue("datanascita"), true);
                string indirizzo = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("indirizzo", "")));
                string StipendioMensile = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Stipendio Mensile", "")));
                string ImportoRichiesto = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Importo Richiesto", "")));
                string Motivazione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Motivazione", "")));
                string InizioScadenzarata = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Inizio Scadenza rata", "")));
                var DataAppuntamento = Sistema.Formats.ParseDate(xlsRis.GetValue("Data Appuntamento"), true);
                var OraAppuntamento = Sistema.Formats.ParseDate(xlsRis.GetValue("Ora Appuntamento"), true);
                var DataRicontatto = Sistema.Formats.ParseDate(xlsRis.GetValue("Data Ricontatto"), true);
                string LuogoAppuntamento = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Luogo Appuntamento", "")));
                string Esito = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Esito", "")));
                string Trattenutecon3 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con 3", "")));
                var Importo3 = Sistema.Formats.ParseValuta(xlsRis.GetValue("Importo 3"), true);
                int? Residuo3 = (int?)Sistema.Formats.ParseValuta(xlsRis.GetValue("Residuo 3"), true);
                string Trattenutecon4 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con 4", "")));
                var Importo4 = Sistema.Formats.ParseValuta(xlsRis.GetValue("Importo 4"), true);
                int? Residuo4 = (int?)Sistema.Formats.ParseValuta(xlsRis.GetValue("Residuo 4"), true);
                string CODAZIENDA = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("CODAZIENDA", "")));
                string puntooperativo = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("puntooperativo", "")));
                string Agente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Agente", "")));
                string operatoreuser = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("operatoreuser", "")));
                if (string.IsNullOrEmpty(Nome) && string.IsNullOrEmpty(Cognome))
                    continue;

                // Elaboro la telefonata
                Anagrafica.CPersona persona;
                Anagrafica.CUfficio po;
                CustomerCalls.CTelefonata telefonata;
                telefonata = FindTelefonataByContesto("ImportaFinMaker[Teledata]", DMD.Integers.ValueOf(Nprot));
                if (telefonata is object)
                {
                    Log("Telefonata già importata ID=" + Nprot + " [" + DMD.Strings.ToNameCase(Nome) + " " + DMD.Strings.UCase(Cognome));
                    continue; // Già abbiamo importato questo oggetto
                }

                po = GetPuntoOperativo(puntooperativo);
                Log("Importo la telefonata ID=" + Nprot + " [" + DMD.Strings.ToNameCase(Nome) + " " + DMD.Strings.UCase(Cognome));
                if (telefonata is null)
                {
                    persona = FindByNameAndTelefono(Cognome, Nome, datanascita, Contatto, Contatto2, Contatto3, Contatto4);
                    if (persona is null)
                    {
                        Log("Creo l'anagrafica della persona [" + DMD.Strings.ToNameCase(Nome) + " " + DMD.Strings.UCase(Cognome));

                        // Elaboro 
                        persona = new Anagrafica.CPersonaFisica();
                        {
                            var withBlock = (Anagrafica.CPersonaFisica)persona;
                            withBlock.Cognome = Cognome;
                            withBlock.Nome = Nome;
                            ricontattare = true;
                            withBlock.Save();
                        }
                    }

                    telefonata = new CustomerCalls.CTelefonata();
                    telefonata.Persona = persona;
                    telefonata.Data = (DateTime) DMD.DateUtils.MergeDateTime(Data, Ora);
                    telefonata.Scopo = Motivo;
                    telefonata.NomeOperatore = Utente;
                    telefonata.PuntoOperativo = po;
                    telefonata.Note = Note;
                    telefonata.Contesto = "ImportaFinMaker[Teledata]";
                    telefonata.IDContesto = DMD.Integers.ValueOf(Nprot);
                    telefonata.Stato = Databases.ObjectStatus.OBJECT_VALID;
                    telefonata.Save();
                }
                else
                {
                    persona = telefonata.Persona;
                }

                AddContatto(persona, Contatto);
                AddContatto(persona, Contatto2);
                AddContatto(persona, Contatto3);
                AddContatto(persona, Contatto4);
                double valore;
                if (!string.IsNullOrEmpty(StipendioMensile))
                {
                    try
                    {
                        valore = (double)Sistema.Formats.ParseValuta(StipendioMensile, true);
                        StipendioMensile = valore.ToString();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (Strings.InStr(StipendioMensile, "@") > 0)
                            {
                                persona.Recapiti.Add("e-mail", "e-mail", StipendioMensile);
                            }
                            else
                            {
                                persona.Recapiti.Add("Telefono", StipendioMensile, Sistema.Formats.ParsePhoneNumber(StipendioMensile));
                            }
                        }
                        catch (Exception ex1)
                        {
                            var note1 = new Sistema.CAnnotazione(persona);
                            note1.Valore = "Spazio esaurito per il contatto: " + StipendioMensile;
                            note1.Stato = Databases.ObjectStatus.OBJECT_VALID;
                            note1.Save();
                        }

                        StipendioMensile = DMD.Strings.vbNullString;
                    }
                }

                if (persona.PuntoOperativo is null)
                    persona.PuntoOperativo = po;
                if (string.IsNullOrEmpty(persona.ResidenteA.Citta))
                    persona.ResidenteA.NomeComune = Citta;
                if (string.IsNullOrEmpty(persona.ResidenteA.ToponimoViaECivico))
                    persona.ResidenteA.ToponimoViaECivico = indirizzo;
                if (persona.Fonte is null)
                {
                    persona.TipoFonte = "Altro";
                    persona.Fonte = GetFonteAltro(Comeciconosce);
                }

                persona.Stato = Databases.ObjectStatus.OBJECT_VALID;
                persona.Save();
                AddAnnotazione("ImportaFinMaker[Teledata]", Nprot, persona, xlsRis);


                // Elaboro la posizione lavorativa
                if (persona is Anagrafica.CPersonaFisica)
                {
                    Anagrafica.CImpiegato impiego;
                    {
                        var withBlock1 = (Anagrafica.CPersonaFisica)persona;
                        impiego = withBlock1.ImpiegoPrincipale;
                        if (string.IsNullOrEmpty(impiego.NomeAzienda))
                            impiego.NomeAzienda = Dovelavora;
                        if (string.IsNullOrEmpty(impiego.Posizione))
                            impiego.Posizione = TipoPosto;
                        if (!impiego.StipendioNetto.HasValue || impiego.StipendioNetto.Value == 0m)
                            impiego.StipendioNetto = Sistema.Formats.ParseValuta(StipendioMensile, true);
                        impiego.Stato = Databases.ObjectStatus.OBJECT_VALID;
                        impiego.Save();
                    }
                }



                // Elaboro il ricontatto
                Anagrafica.CRicontatto ricontatto;
                if (ricontattare)
                {
                    if (DataRicontatto.HasValue == false)
                        DataRicontatto = DMD.DateUtils.Now();
                    ricontatto = Anagrafica.Ricontatti.GetRicontattoBySource("ImportaFinMaker[Teledata]", Nprot);
                    if (ricontatto is null)
                        ricontatto = Anagrafica.Ricontatti.ProgrammaRicontatto(persona, (DateTime)DataRicontatto, "Ricontatto importato da FinMaker", "ImportaFinMaker[Teledata]", Nprot, nomeLista, po, null);
                }
                else if (DataRicontatto.HasValue)
                {
                    ricontatto = Anagrafica.Ricontatti.GetProssimoRicontatto(persona);
                    if (ricontatto is null)
                        ricontatto = Anagrafica.Ricontatti.ProgrammaRicontatto(persona, (DateTime)DataRicontatto, "Ricontatto importato da FinMaker", "ImportaFinMaker[Teledata]", Nprot, "", po, null);
                }

                // Elaboro l'appuntamento
                // Dim appuntamento As CRicontatto = Appuntamenti.GetAppuntamentoBySource("ImportaFinMaker[Teledata]", Nprot)
                // If (appuntamento Is Nothing AndAlso ((DataAppuntamento.HasValue AndAlso DataAppuntamento.Value < Today))) Then
                // appuntamento = Appuntamenti.ProgrammaAppuntamento(persona, Calendar.MergeDateTime(DataAppuntamento, OraAppuntamento), "Appuntamento importato da FinMaker", LuogoAppuntamento, "ImportaFinMaker[Teledata]", Nprot, "", po, Nothing)
                // End If

                // Elaboro i finanziamenti in corso
                Finanziaria.CEstinzioniCollection estinzioni = Finanziaria.Estinzioni.GetEstinzioniByPersona(persona);
                if (Data.HasValue == false)
                    Data = DMD.DateUtils.Now();
                var e = FindEstinzione(persona, estinzioni, Trattenutecon, Importo, Residuo, (DateTime)Data);
                e = FindEstinzione(persona, estinzioni, Trattenutecon2, Importo2, Residuo2, (DateTime)Data);
                e = FindEstinzione(persona, estinzioni, Trattenutecon3, Importo3, Residuo3, (DateTime)Data);
                e = FindEstinzione(persona, estinzioni, Trattenutecon4, Importo4, Residuo4, (DateTime)Data);

                // Elabora la richiesta di finanziamento
                if (!string.IsNullOrEmpty(ImportoRichiesto))
                {
                    var richiesta = FindRichiesta(persona, (DateTime)Data, ImportoRichiesto);
                }
            }

            xlsRis.Dispose();
            Log("Finisco l'importazione del foglio [Teledata]");
            Log("------------------------------------------");
        }

        private Finanziaria.CRichiestaFinanziamento FindRichiesta(Anagrafica.CPersona persona, DateTime data, string importoRichiesto)
        {
            var cursor = new Finanziaria.CRichiesteFinanziamentoCursor();
            cursor.Data.Value = data;
            cursor.IDCliente.Value = Databases.GetID(persona);
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            var ret = cursor.Item;
            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            decimal? valore;
            try
            {
                valore = Sistema.Formats.ParseValuta(importoRichiesto, true);
            }
            catch (Exception ex)
            {
                valore = default;
            }

            if (ret is object)
            {
                if (valore.HasValue)
                {
                    if (ret.ImportoRichiesto == valore == true)
                        return ret;
                }
                else if ((ret.Note ?? "") == (importoRichiesto ?? ""))
                    return ret;
            }

            ret = new Finanziaria.CRichiestaFinanziamento();
            ret.Cliente = (Anagrafica.CPersonaFisica)persona;
            ret.Data = data;
            ret.ImportoRichiesto = valore;
            ret.Note = importoRichiesto;
            ret.Stato = Databases.ObjectStatus.OBJECT_VALID;
            ret.StatoRichiesta = Finanziaria.StatoRichiestaFinanziamento.EVASA;
            // ret.NomePresaInCaricoDa = 
            ret.Save();
            return ret;
        }

        private Finanziaria.CEstinzione FindEstinzione(Anagrafica.CPersona persona, CCollection<Finanziaria.CEstinzione> items, string con, decimal? rata, int? residue, DateTime dataTelefonata)
        {
            Finanziaria.CEstinzione e;
            con = Strings.LCase(Strings.Trim(con));
            if (string.IsNullOrEmpty(con) && rata.HasValue == false && residue.HasValue == false)
                return null;
            foreach (var currentE in items)
            {
                e = currentE;
                if (((Strings.LCase(e.NomeIstituto) ?? "") == (con ?? "") && rata.HasValue && e.Rata.HasValue && rata.Value == e.Rata) == true)
                {
                    return e;
                }
            }

            e = new Finanziaria.CEstinzione();
            e.Persona = persona;
            e.NomeIstituto = con;
            e.Rata = rata;
            if (residue.HasValue)
                e.NumeroRateResidue = residue;
            e.Stato = Databases.ObjectStatus.OBJECT_VALID;
            e.Save();
            return e;
        }

        private CustomerCalls.CTelefonata FindTelefonataByPersonaAndContesto(Anagrafica.CPersona persona, string contesto, int idContesto)
        {
            var cursor = new CustomerCalls.CTelefonateCursor();
            cursor.IDPersona.Value = Databases.GetID(persona);
            cursor.Contesto.Value = contesto;
            cursor.IDContesto.Value = idContesto;
            var ret = cursor.Item;
            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            return ret;
        }

        private CustomerCalls.CTelefonata FindTelefonataByContesto(string contesto, int idContesto)
        {
            var cursor = new CustomerCalls.CTelefonateCursor();
            cursor.Contesto.Value = contesto;
            cursor.IDContesto.Value = idContesto;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            var ret = cursor.Item;
            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            return ret;
        }

        private void ImportaPratiche(Databases.CXlsDBConnection xlsConn)
        {
            bool ricontattare;
            Log("Inizio l'importazione del foglio [Pratiche]");
            var xlsTable = xlsConn.Tables["Pratiche"];
            var xlsRis = new Databases.DBReader(xlsTable);
            while (xlsRis.Read())
            {
                ricontattare = false;
                var Datanascitadeb = Sistema.Formats.ParseDate(xlsRis.GetValue("Datanascitadeb"), true);
                string Nomedeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Nomedeb", "")));
                string ResidCAPdeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("ResidCAPdeb", "")));
                string ResidViadeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("ResidViadeb", "")));
                string Cognomedeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Cognomedeb", "")));
                string Residcittadeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residcittadeb", "")));
                string Finanziaria = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Finanziaria", "")));
                string Nprot = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("NProt", "")));
                string luogonascitadeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("luogonascitadeb", "")));
                string Qualificadeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Qualificadeb", "")));
                string provnascdeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("provnascdeb", "")));
                string provdeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("provdeb", "")));
                string Codicefiscaledebitore = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Codicefiscaledebitore", "")));
                string Amministnom = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Amministnom", "")));
                string Tiporapportoesteso = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Tiporapportoesteso", "")));
                string Utente = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Utente", "")));
                string NPrev = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("NPrev", "")));
                string Sesso = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Sesso", "")));
                string Postit = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Postit", "")));
                string Telefonatericevute = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute", "")));
                string Telefonatericevute2 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 2", "")));
                string Telefonatericevute3 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 3", "")));
                string Telefonatericevute4 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 4", "")));
                string Telefonatericevute5 = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 5", "")));
                string Nota = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Nota", "")));
                string Notepratica = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Note pratica", "")));
                string AnnotazioniAmministratore = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Annotazioni Amministratore", "")));
                string email = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("email", "")));
                string codiceprodotto = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("codice prodotto", "")));
                string Telufficiodeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Tel ufficio deb", "")));
                string Residcittàdebperspedizione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Resid città deb per spedizione", "")));
                string ResidViadebperspedizione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Resid Via deb per spedizione", "")));
                string Residprovinciadebperspedizione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Resid provincia deb per spedizione", "")));
                string Residcapdebperspedizione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Resid cap deb per spedizione", "")));
                string ResidNominativoperspedizione = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Resid Nominativo per spedizione", "")));
                string Pubblicita = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Pubblicita", "")));
                string Operatore = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Operatore", "")));
                string Operatorediannullopratica = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Operatore di annullo pratica", "")));
                var ImportoRata = Sistema.Formats.ParseValuta(xlsRis.GetValue("ImportoRata"), true);
                var NRate = Sistema.Formats.ParseInteger(xlsRis.GetValue("NRate"), true);
                string Residtelefonodeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residtelefonodeb", "")));
                string Residcellularedeb = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("Residcellularedeb", "")));
                var Nettoerogatouser = Sistema.Formats.ParseValuta(xlsRis.GetValue("Netto erogato user"), true);
                var Stipendionetto = Sistema.Formats.ParseValuta(xlsRis.GetValue("Stipendio netto"), true);
                string puntooperativo = Strings.Trim(RemoveInvalidCharsFromName(xlsRis.GetValue("puntooperativo", "")));
                var Decorrenza = Sistema.Formats.ParseDate(xlsRis.GetValue("Decorrenza"), true);
                var Dataassunzionedeb = Sistema.Formats.ParseDate(xlsRis.GetValue("Dataassunzionedeb"), true);
                var Dataliquidazione = Sistema.Formats.ParseDate(xlsRis.GetValue("Data liquidazione"), true);
                var Dataannullo = Sistema.Formats.ParseDate(xlsRis.GetValue("Data annullo"), true);
                var Datapresentazionebanca = Sistema.Formats.ParseDate(xlsRis.GetValue("Data presentazione banca"), true);
                var DataPrev = Sistema.Formats.ParseDate(xlsRis.GetValue("Data Prev."), true);
                var po = GetPuntoOperativo(puntooperativo);
                DateTime? dataRicontatto = default;
                if (string.IsNullOrEmpty(Nomedeb) && string.IsNullOrEmpty(Cognomedeb) && string.IsNullOrEmpty(Codicefiscaledebitore))
                {
                    continue;
                }

                Finanziaria.CPraticaCQSPD pratica;
                pratica = minidom.Finanziaria.Pratiche.GetItemByNumeroEsterno(null, "FinMaker(" + NomePO + ", " + Nprot + ")");
                if (pratica is object)
                {
                    Log("Pratica già importata ID=" + Nprot + " [" + DMD.Strings.ToNameCase(Nomedeb) + " " + DMD.Strings.UCase(Cognomedeb));
                    continue; // Già abbiamo importato questa pratica
                }

                Anagrafica.CPersona persona;
                persona = FindPersonaByCF(Codicefiscaledebitore);
                if (persona is null)
                    persona = FindByNameAndTelefono(Cognomedeb, Nomedeb, default, Telufficiodeb, Residtelefonodeb, Residcellularedeb, email);
                Log("Importo la pratica ID=" + Nprot + " [" + DMD.Strings.ToNameCase(Nomedeb) + " " + DMD.Strings.UCase(Cognomedeb));
                if (persona is null)
                {
                    Log("Creo l'anagrafica della persona [" + DMD.Strings.ToNameCase(Nomedeb) + " " + DMD.Strings.UCase(Cognomedeb));

                    // Elaboro 
                    persona = new Anagrafica.CPersonaFisica();
                    {
                        var withBlock = (Anagrafica.CPersonaFisica)persona;
                        withBlock.Cognome = Cognomedeb;
                        withBlock.Nome = Nomedeb;
                        ricontattare = true;
                        dataRicontatto = DMD.DateUtils.Now();
                    }
                }

                AddContatto(persona, Telufficiodeb, "Telefono Ufficio");
                AddContatto(persona, Residtelefonodeb, "Telefono Casa");
                AddContatto(persona, Residcellularedeb, "Cellulare");
                AddContatto(persona, email, "e-mail");
                if (string.IsNullOrEmpty(persona.CodiceFiscale))
                    persona.CodiceFiscale = Codicefiscaledebitore;
                if (!string.IsNullOrEmpty(persona.CodiceFiscale))
                {
                    var cfCalc = new Anagrafica.CFCalculator();
                    try
                    {
                        cfCalc.CodiceFiscale = persona.CodiceFiscale;
                        cfCalc.Inverti();
                        if (persona.DataNascita.HasValue == false)
                            persona.DataNascita = cfCalc.NatoIl;
                        if (string.IsNullOrEmpty(persona.NatoA.Citta))
                            persona.NatoA.NomeComune = Anagrafica.Luoghi.MergeComuneeProvincia(cfCalc.NatoAComune, cfCalc.NatoAProvincia);
                        if (string.IsNullOrEmpty(persona.Sesso))
                            persona.Sesso = cfCalc.Sesso;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (string.IsNullOrEmpty(persona.NatoA.Citta))
                {
                    persona.NatoA.Citta = luogonascitadeb;
                    persona.NatoA.Provincia = provnascdeb;
                }

                if (string.IsNullOrEmpty(persona.Sesso))
                    persona.Sesso = Sesso;
                if (persona.DataNascita.HasValue == false)
                    persona.DataNascita = Datanascitadeb;
                if (string.IsNullOrEmpty(persona.ResidenteA.CAP))
                    persona.ResidenteA.CAP = ResidCAPdeb;
                if (string.IsNullOrEmpty(persona.ResidenteA.Citta))
                    persona.ResidenteA.Citta = Residcittadeb;
                if (string.IsNullOrEmpty(persona.ResidenteA.Provincia))
                    persona.ResidenteA.Citta = provdeb;
                if (string.IsNullOrEmpty(persona.ResidenteA.ToponimoViaECivico))
                    persona.ResidenteA.ToponimoViaECivico = ResidViadeb;
                if (string.IsNullOrEmpty(persona.DomiciliatoA.Citta))
                    persona.DomiciliatoA.Citta = Residcittàdebperspedizione;
                if (string.IsNullOrEmpty(persona.DomiciliatoA.ToponimoViaECivico))
                    persona.DomiciliatoA.ToponimoViaECivico = ResidViadebperspedizione;
                if (string.IsNullOrEmpty(persona.DomiciliatoA.Provincia))
                    persona.DomiciliatoA.Provincia = Residprovinciadebperspedizione;
                if (string.IsNullOrEmpty(persona.DomiciliatoA.CAP))
                    persona.DomiciliatoA.CAP = Residcapdebperspedizione;
                if (!string.IsNullOrEmpty(ResidNominativoperspedizione))
                    persona.DomiciliatoA.Nome = ResidNominativoperspedizione; // persona Dim ResidNominativoperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid Nominativo per spedizione", "")))
                if (persona.PuntoOperativo is null)
                    persona.PuntoOperativo = po;
                if (persona.Fonte is null)
                {
                    persona.TipoFonte = "Altro";
                    persona.Fonte = GetFonteAltro(Pubblicita);
                }

                persona.Stato = Databases.ObjectStatus.OBJECT_VALID;
                persona.Save();


                // Elaboro la posizione lavorativa
                if (persona is Anagrafica.CPersonaFisica)
                {
                    Anagrafica.CImpiegato impiego;
                    {
                        var withBlock1 = (Anagrafica.CPersonaFisica)persona;
                        impiego = withBlock1.ImpiegoPrincipale;
                        if (string.IsNullOrEmpty(impiego.NomeAzienda))
                            impiego.NomeAzienda = Amministnom;
                        if (string.IsNullOrEmpty(impiego.Posizione))
                            impiego.Posizione = Qualificadeb;
                        if (string.IsNullOrEmpty(impiego.TipoRapporto))
                            impiego.TipoRapporto = Tiporapportoesteso;
                        if (!impiego.StipendioNetto.HasValue || impiego.StipendioNetto.Value == 0m)
                            impiego.StipendioNetto = Sistema.Formats.ParseValuta(Stipendionetto, true);
                        impiego.Stato = Databases.ObjectStatus.OBJECT_VALID;
                        impiego.Save();
                    }
                }

                NomePO = puntooperativo;

                // Elabora la pratica
                decimal? montante = default;
                if (ImportoRata.HasValue && NRate.HasValue)
                    montante = ImportoRata.Value * NRate.Value;
                if (pratica is null)
                    pratica = CercaPraticaVicina((Anagrafica.CPersonaFisica)persona, montante, Decorrenza, null);
                if (pratica is null)
                {
                    Log("Creo la pratica esterna N°" + Nprot);
                    pratica = new Finanziaria.CPraticaCQSPD();
                    pratica.Cliente = (Anagrafica.CPersonaFisica)persona;
                    pratica.FromCliente();
                }

                if (string.IsNullOrEmpty(pratica.NumeroEsterno))
                    pratica.NumeroEsterno = "FinMaker(" + NomePO + ", " + Nprot + ")";
                if (pratica.DataDecorrenza.HasValue == false)
                    pratica.DataDecorrenza = Decorrenza;
                if (pratica.NumeroRate.HasValue == false)
                    pratica.NumeroRate = NRate;
                if (pratica.Rata.HasValue == false)
                    pratica.Rata = ImportoRata;
                if (string.IsNullOrEmpty(pratica.NomeProdotto))
                    pratica.NomeProdotto = codiceprodotto;
                if ((pratica.NettoRicavo == 0 && Nettoerogatouser.HasValue) == true)
                    pratica.NettoRicavo = Nettoerogatouser;
                if (string.IsNullOrEmpty(pratica.NomeCessionario))
                    pratica.NomeCessionario = Finanziaria;
                // To DO
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if ((pratica.StatoPreventivo.NomeOperatore ?? "") == (Sistema.Users.CurrentUser.Nominativo ?? ""))
                    pratica.StatoPreventivo.NomeOperatore = Operatore;
                if (string.IsNullOrEmpty(pratica.StatoRichiestaDelibera.NomeOperatore))
                    pratica.StatoRichiestaDelibera.NomeOperatore = Operatore;
                if (string.IsNullOrEmpty(pratica.StatoLiquidata.NomeOperatore))
                    pratica.StatoLiquidata.NomeOperatore = Operatore;
                if (string.IsNullOrEmpty(pratica.StatoAnnullata.NomeOperatore))
                    pratica.StatoAnnullata.NomeOperatore = Operatorediannullopratica;
                pratica.Stato = Databases.ObjectStatus.OBJECT_VALID;
                pratica.Save();
                if (!string.IsNullOrEmpty(Postit))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(PostIt)", persona, "Postit: " + Postit, pratica);
                if (!string.IsNullOrEmpty(Telefonatericevute))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Telefonatericevute)", persona, "Telefonatericevute: " + Telefonatericevute, pratica);
                if (!string.IsNullOrEmpty(Telefonatericevute2))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Telefonatericevute2)", persona, "Telefonatericevute2: " + Telefonatericevute2, pratica);
                if (!string.IsNullOrEmpty(Telefonatericevute3))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Telefonatericevute3)", persona, "Telefonatericevute3: " + Telefonatericevute3, pratica);
                if (!string.IsNullOrEmpty(Telefonatericevute4))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Telefonatericevute4)", persona, "Telefonatericevute4: " + Telefonatericevute4, pratica);
                if (!string.IsNullOrEmpty(Telefonatericevute5))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Telefonatericevute5)", persona, "Telefonatericevute5: " + Telefonatericevute5, pratica);
                if (!string.IsNullOrEmpty(Nota))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Nota)", persona, "Nota: " + Nota, pratica);
                if (!string.IsNullOrEmpty(Notepratica))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(Notepratica)", persona, "Notepratica: " + Notepratica, pratica);
                if (!string.IsNullOrEmpty(AnnotazioniAmministratore))
                    AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot + "(AnnotazioniAmministratore)", persona, "AnnotazioniAmministratore: " + AnnotazioniAmministratore, pratica);
                AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot, persona, xlsRis, pratica);

                // Elaboro il ricontatto
                if (ricontattare)
                {
                    var ricontatto = Anagrafica.Ricontatti.GetRicontattoBySource("ImportaFinMaker[Pratiche]", Nprot);
                    // If (ricontatto Is Nothing) Then ricontatto = Ricontatti.GetRicontattoBySource(pratica)
                    if (ricontatto is null)
                        ricontatto = Anagrafica.Ricontatti.ProgrammaRicontatto(persona, DMD.DateUtils.Now(), "Ricontatto importato da FinMaker", "ImportaFinMaker[Pratiche]", Nprot, nomeLista, po, null);
                }
            }

            xlsRis.Dispose();
            Log("Finisco l'importazione del foglio [Pratiche]");
            Log("------------------------------------------");
        }

        private Anagrafica.CPersonaFisica FindPersonaByCF(string cf)
        {
            var items = Anagrafica.Persone.FindPersoneByCF(cf);
            if (items.Count > 0)
            {
                foreach (Anagrafica.CPersona item in items)
                {
                    if (item.TipoPersona == Anagrafica.TipoPersona.PERSONA_FISICA)
                        return (Anagrafica.CPersonaFisica)item;
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        private bool IsPhoneNumber(string value)
        {
            return !string.IsNullOrEmpty(Sistema.Formats.ParsePhoneNumber(value));
        }

        private Anagrafica.CPersona FindByNameAndTelefono(string Cognome, string Nome, DateTime? datanascita, string telefono1, string telefono2, string telefono3, string telefono4)
        {
            Anagrafica.CPersona ret = null;
            Nome = Strings.Trim(Nome);
            Cognome = Strings.Trim(Cognome);
            telefono1 = Sistema.Formats.ParsePhoneNumber(telefono1);
            telefono2 = Sistema.Formats.ParsePhoneNumber(telefono2);
            telefono3 = Sistema.Formats.ParsePhoneNumber(telefono3);
            telefono4 = Sistema.Formats.ParsePhoneNumber(telefono4);

            // Dim web1 As String = LCase(IIf(Me.IsPhoneNumber(telefono1), "", telefono1))
            // Dim web2 As String = LCase(IIf(Me.IsPhoneNumber(telefono2), "", telefono2))
            // Dim web3 As String = LCase(IIf(Me.IsPhoneNumber(telefono3), "", telefono3))
            // Dim web4 As String = LCase(IIf(Me.IsPhoneNumber(telefono4), "", telefono4))


            var cursor = new Anagrafica.CPersonaCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            if (!string.IsNullOrEmpty(Cognome))
                cursor.Cognome.Value = Cognome;
            if (!string.IsNullOrEmpty(Nome))
                cursor.Nome.Value = Nome;
            if (datanascita.HasValue)
                cursor.DataNascita.Value = datanascita;
            while (!cursor.EOF())
            {
                var tmp = cursor.Item;
                foreach (Anagrafica.CContatto c in tmp.Recapiti)
                {
                    if (!string.IsNullOrEmpty(c.Valore) && ((c.Valore ?? "") == (telefono1 ?? "") || (c.Valore ?? "") == (telefono2 ?? "") || (c.Valore ?? "") == (telefono3 ?? "") || (c.Valore ?? "") == (telefono4 ?? "")))
                    {
                        ret = tmp;
                        break;
                    }
                }
                // For Each c As CContatto In tmp.ContattiWeb
                // If (c.Valore <> "") AndAlso (LCase(c.Valore) = web1 OrElse LCase(c.Valore) = web2 OrElse c.Valore = telefono3 OrElse c.Valore = telefono4) Then
                // ret = tmp
                // Exit While
                // End If
                // Next
                cursor.MoveNext();
            }

            cursor.Dispose();
            return ret;
        }

        private void AddContatto(Anagrafica.CPersona persona, string Contatto)
        {
            try
            {
                Contatto = Strings.Trim(Contatto);
                if (string.IsNullOrEmpty(Contatto))
                    return;
                string telefono = Sistema.Formats.ParsePhoneNumber(Contatto);
                if (Strings.InStr(Contatto, "@") <= 0 & !string.IsNullOrEmpty(telefono))
                {
                    persona.Recapiti.Add("Telefono", telefono, telefono);
                }
                else
                {
                    persona.Recapiti.Add("e-mail", Contatto, Contatto);
                }
            }
            catch (Exception ex)
            {
                var note = new Sistema.CAnnotazione(persona);
                note.Valore = "Spazio esaurito per il contatto: " + Contatto;
                note.Stato = Databases.ObjectStatus.OBJECT_VALID;
                note.Save();
            }
        }

        private void AddContatto(Anagrafica.CPersona persona, string Contatto, string nome)
        {
            try
            {
                Contatto = Strings.Trim(Contatto);
                if (string.IsNullOrEmpty(Contatto))
                    return;
                string telefono = Sistema.Formats.ParsePhoneNumber(Contatto);
                if (Strings.InStr(Contatto, "@") <= 0 & !string.IsNullOrEmpty(telefono))
                {
                    persona.Recapiti.Add("Telefono", nome, telefono);
                }
                else
                {
                    persona.Recapiti.Add("e-mail", nome, Contatto);
                }
            }
            catch (Exception ex)
            {
                var note = new Sistema.CAnnotazione(persona);
                note.Valore = "Spazio esaurito per il contatto: " + nome + " = " + Contatto;
                note.Stato = Databases.ObjectStatus.OBJECT_VALID;
                note.Save();
            }
        }

        private Sistema.CAnnotazione AddAnnotazione(string sourceName, string sourceParam, object oggetto, Databases.DBReader xlsRis, object contesto = null)
        {
            var text = new System.Text.StringBuilder();
            foreach (Databases.CDBEntityField field in xlsRis.Schema.Fields)
            {
                if (text.Length > 0)
                    text.Append("<br/>");
                text.Append("<b>" + DMD.WebUtils.HtmlEncode(field.Name) + "</b>: <i>" + DMD.WebUtils.HtmlEncode(Convert.ToString(xlsRis.GetValue(field.Name))) + "</i>");
            }

            var ret = Sistema.Annotazioni.GetItemBySource(sourceName, sourceParam);
            if (ret is null)
            {
                if (contesto is null)
                {
                    ret = new Sistema.CAnnotazione(oggetto);
                }
                else
                {
                    ret = new Sistema.CAnnotazione(oggetto, contesto);
                }

                ret.SourceName = sourceName;
                ret.SourceParam = sourceParam;
            }

            ret.Valore = text.ToString();
            ret.Stato = Databases.ObjectStatus.OBJECT_VALID;
            ret.Save();
            return ret;
        }

        private Sistema.CAnnotazione AddAnnotazione(string sourceName, string sourceParam, object oggetto, string testo, object contesto = null)
        {
            var ret = Sistema.Annotazioni.GetItemBySource(sourceName, sourceParam);
            if (ret is null)
            {
                if (contesto is null)
                {
                    ret = new Sistema.CAnnotazione(oggetto);
                }
                else
                {
                    ret = new Sistema.CAnnotazione(oggetto, contesto);
                }
            }

            ret.Valore = testo;
            ret.Stato = Databases.ObjectStatus.OBJECT_VALID;
            ret.Save();
            return ret;
        }
    }
}