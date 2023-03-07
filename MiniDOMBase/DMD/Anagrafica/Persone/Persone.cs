using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CPersona"/>
        /// </summary>
        [Serializable]
        public sealed class CPersoneClass 
            : CModulesClass<Anagrafica.CPersona>
        {

            /// <summary>
            /// Numero massimo di parole oltre il quale non viene utilizzato l'algoritmo "intelligente" per la ricerca dei nomi
            /// </summary>
            /// <remarks></remarks>
            private const int MAXWORDSINTELLISEARCH = 10;
            private readonly object m_FindHandlersLock = new object();
            private readonly CKeyCollection<Anagrafica.FindPersonaHandler> m_FindHandlers;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersoneClass() 
                : base("modPersone", typeof(Anagrafica.CPersonaCursor))
            {
                this.m_FindHandlers = new CKeyCollection<FindPersonaHandler>();
            }

            /// <summary>
            /// Restituisce la collezione degli handler di ricerca installati
            /// </summary>
            /// <returns></returns>
            public CCollection<FindPersonaHandler> GetInstalledFindHandlers()
            {
                lock (m_FindHandlersLock)
                    return new CCollection<FindPersonaHandler>(m_FindHandlers);
            }

            /// <summary>
            /// Restituisce l'handler di ricerca in base al nome
            /// </summary>
            /// <param name="command"></param>
            /// <returns></returns>
            public FindPersonaHandler GetFindHandler(string command)
            {
                if (string.IsNullOrEmpty(command))
                    throw new ArgumentNullException("command", "Command cannot be null");

                lock (m_FindHandlersLock)
                {
                    command = DMD.Strings.LCase(DMD.Strings.Trim(command));
                    return m_FindHandlers.GetItemByKey(command);
                }
            }

            /// <summary>
            /// Rimuove un handler
            /// </summary>
            /// <param name="h"></param>
            public void RemoveFindHandler(Anagrafica.FindPersonaHandler h)
            {
                if (h is null)
                    throw new ArgumentNullException("handler", "Handler cannot be null");
                lock (m_FindHandlersLock)
                    m_FindHandlers.Remove(h);
            }

            /// <summary>
            /// Installa un handler
            /// </summary>
            /// <param name="command"></param>
            /// <param name="h"></param>
            public void InstallFindHandler(string command, Anagrafica.FindPersonaHandler h)
            {
                if (string.IsNullOrEmpty(command))
                    throw new ArgumentNullException("command", "Cannot install null command");
                if (h is null)
                    throw new ArgumentNullException("handler", "Handler cannot be null");

                lock (m_FindHandlersLock)
                {
                    command = DMD.Strings.LCase(DMD.Strings.Trim(command));
                    m_FindHandlers.Add(command, h);
                    m_FindHandlers.Sort();
                }
            }

            ///// <summary>
            ///// Inizializza
            ///// </summary>
            //public override void Initialize()
            //{
            //    Databases.CDBTable tbl;
            //    Databases.CDBEntityField fld;
            //    tbl = this.Database.Tables.GetItemByKey("tbl_Persone");
            //    fld = tbl.Fields.Alter("Attributi", typeof(string), 0);
            //    fld = tbl.Fields.Alter("IMP_TipoRapporto", typeof(string), 255);
            //    tbl.Update();
            //    tbl = this.Database.Tables.GetItemByKey("tbl_Impiegati");
            //    fld = tbl.Fields.Alter("TipoRapporto", typeof(string), 255);
            //    tbl.Update();
            //    tbl = this.Database.Tables.GetItemByKey("Tiporapporto");
            //    fld = tbl.Fields.Alter("IdTipoRapporto", typeof(string), 255);
            //    tbl.Update();
            //    base.Initialize();
            //}



            //public override Anagrafica.CPersona GetItemById(int id)
            //{
            //    var conn = GetConnection();
            //    if (conn.IsRemote())
            //    {
            //        return base.GetItemById(id);
            //    }
            //    else
            //    {
            //        if (id == 0)
            //            return null;
            //        string dbSQL = "SELECT * FROM [tbl_Persone] WHERE [ID]=" + id;
            //        Anagrafica.CPersona ret = null;
            //        using (var dbRis = Databases.APPConn.ExecuteReader(dbSQL))
            //        {
            //            if (dbRis.Read())
            //            {
            //                ret = Instantiate((Anagrafica.TipoPersona)DMD.Integers.CInt(dbRis["TipoPersona"]));
            //                Databases.APPConn.Load(ret, dbRis);
            //            }
            //        }
            //        return ret;

            //    }
            //}

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
            /// Gestisce l'evento PersonaMoficied
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaModified(PersonaEventArgs e)
            {
                lock (this.cacheLock)
                {


                }
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
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

                    if (persona2 is CPersonaFisica)
                    {
                         

                    }
                    else
                    {
                        // Tabella tbl_Persone (Azienda)
                        using(var cursor = new CPersonaFisicaCursor())
                        {
                            cursor.IgnoreRights = true;
                            cursor.Impiego_IDAzienda.Value = mi.IDPersona2;
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_Persone";
                                rec.FieldName = "IMP_IDAzienda";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.ImpiegoPrincipale.Azienda = (CAzienda) mi.Persona1;
                                cursor.Item.Save();
                            }
                        }

                        using (var cursor = new CPersonaFisicaCursor())
                        {
                            cursor.IgnoreRights = true;
                            cursor.Impiego_IDEntePagante.Value = mi.IDPersona2;
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_Persone";
                                rec.FieldName = "IDEntePagante";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.ImpiegoPrincipale.EntePagante = (CAzienda)mi.Persona1;
                                cursor.Item.Save();
                            }
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


                    if (!(mi.Persona2 is CPersonaFisica))
                    {
                        // Tabella tbl_Persone (Azienda)
                        var items = mi.GetAffectedRecorsIDs("tbl_Persone", "IMP_IDAzienda");
                        //if (!string.IsNullOrEmpty(items))
                        //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_Persone] SET [IMP_IDAzienda]=" + DBUtils.GetID(persona1) + ", [IMP_NomeAzienda]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                        using (var cursor = new CPersonaCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                //todo
                            }
                        }
                        // Tabella tbl_Persone (EntePagante)
                        items = mi.GetAffectedRecorsIDs("tbl_Persone", "IMP_IDEntePagante");
                        //if (!string.IsNullOrEmpty(items))
                        //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_Persone] SET [IMP_IDEntePagante]=" + DBUtils.GetID(persona1) + ", [IMP_NomeEntePagante]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                        using (var cursor = new CPersonaCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                //todo
                            }
                        }

                    }

                }
            }


            /// <summary>
            /// Restituisce un oggetto CPersonaFisica inizializzato con i dati che corrispondono al codice fiscale
            /// </summary>
            /// <param name="cf"></param>
            /// <returns></returns>
            public Anagrafica.CPersonaFisica InvertCodiceFiscale(string cf)
            {
                var ret = new Anagrafica.CPersonaFisica();
                string mesi = "ABCDEHLMPRST";
                cf = Sistema.Formats.ParseCodiceFiscale(cf);
                if (string.IsNullOrEmpty(cf))
                    return null;
                ret.NatoA.NomeComune = Anagrafica.Luoghi.Comuni.GetNomeComuneByCatasto(DMD.Strings.Mid(cf, 12, 4));
                ret.CodiceFiscale = cf;
                int gg = DMD.Integers.ValueOf(DMD.Strings.Mid(cf, 10, 2));
                int mm = 1 + mesi.IndexOf(DMD.Strings.Mid(cf, 9, 1));
                int aa = 1900 + DMD.Integers.ValueOf(DMD.Strings.Mid(cf, 7, 2));
                if (gg > 40)
                {
                    ret.Sesso = "F";
                    gg -= 40;
                }
                else
                {
                    ret.Sesso = "M";
                }

#if (!DEBUG)
                try
                {
#endif
                    ret.DataNascita = DMD.DateUtils.MakeDate(aa, mm, gg);
#if (!DEBUG)
            } catch (Exception) { }
#endif
                return ret;
            }

            /// <summary>
            /// Data una stringa contenente il cognome ed il nome di una persona, tenta di separare i due componenti
            /// </summary>
            /// <param name="value"></param>
            /// <param name="cognome"></param>
            /// <param name="nome"></param>
            /// <remarks></remarks>
            public void SplitCognomeNome(string value, ref string cognome, ref string nome)
            {
                int p;
                value = DMD.Strings.Replace(DMD.Strings.Trim(value), "  ", " ");
                p = DMD.Strings.InStrRev(value, " ");
                if (p > 1)
                {
                    cognome = DMD.Strings.Left(value, p - 1);
                    nome = DMD.Strings.Mid(value, p + 1);
                }
                else
                {
                    cognome = value;
                    nome = DMD.Strings.vbNullString;
                }
            }

            ///// <summary>
            ///// Aggiunge le info su una persona
            ///// </summary>
            ///// <param name="col"></param>
            ///// <param name="dbRis"></param>
            //private void AddPersonInfo(
            //                CCollection<Anagrafica.CPersonaInfo> col, 
            //                IDataReader dbRis
            //                )
            //{
            //    var info = new Anagrafica.CPersonaInfo(); // Persone.Instantiate( dbRis("TipoPersona")  )
            //    info.IDPersona = DMD.Integers.CInt(dbRis["ID"]);
            //    info.NomePersona = DMD.Strings.JoinW(DMD.Strings.Trim(DMD.Strings.ToNameCase(Sistema.Formats.ToString(dbRis["Nome"]))), " ", DMD.Strings.UCase(Sistema.Formats.ToString(dbRis["Cognome"])));
            //    info.Notes = "";
            //    if (info.Persona is object)
            //    {
            //        {
            //            var withBlock = info.Persona;
            //            info.Deceduto = withBlock.Deceduto;
            //            info.IconURL = withBlock.IconURL;
            //            if (!string.IsNullOrEmpty(withBlock.NatoA.NomeComune))
            //                info.Notes = DMD.Strings.Combine(info.Notes, DMD.Strings.JoinW("Nato a: ", withBlock.NatoA.NomeComune), ", ");
            //            if (!Sistema.Types.IsNull(withBlock.DataNascita))
            //                info.Notes = DMD.Strings.Combine(info.Notes, DMD.Strings.JoinW("Nato il: ", Sistema.Formats.FormatUserDate(withBlock.DataNascita)), ", ");
            //            if (!string.IsNullOrEmpty(withBlock.CodiceFiscale))
            //                info.Notes = DMD.Strings.Combine(info.Notes, DMD.Strings.JoinW("C.F: ", Sistema.Formats.FormatCodiceFiscale(withBlock.CodiceFiscale)), ", ");
            //            if (!string.IsNullOrEmpty(withBlock.DomiciliatoA.ToString()))
            //                info.Notes = DMD.Strings.Combine(info.Notes, DMD.Strings.JoinW("Indirizzo: ", withBlock.DomiciliatoA.ToString()), ", ");
            //        }
            //    }

            //    col.Add(info);
            //}

            /// <summary>
            /// Aggiunge le info su una persona
            /// </summary>
            /// <param name="col"></param>
            /// <param name="p"></param>
            /// <returns></returns>
            private Anagrafica.CPersonaInfo AddPersonInfo(
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CPersona p
                            )
            {
                var info = new Anagrafica.CPersonaInfo(p); // Persone.Instantiate( dbRis("TipoPersona")  )
                col.Add(info);
                return info;
            }

            private void ApplyFilterToCursor(CRMFindParams filter, CPersonaCursor cursor)
            {
                if (filter.tipoPersona.HasValue) cursor.TipoPersona.Value = filter.tipoPersona.Value;
                if (filter.IDPuntoOperativo != 0) cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                if (filter.flags.HasValue)
                {
                    cursor.PFlags.Value = filter.flags.Value;
                    cursor.PFlags.Operator = OP.OP_ALLBITAND;
                }
                if (!string.IsNullOrEmpty(filter.DettaglioEsito)) cursor.DettaglioEsito.Value = filter.DettaglioEsito;

                cursor.IgnoreRights = filter.ignoreRights;

            }

            /// <summary>
            /// Cerca le persone in base all'azienda
            /// </summary>
            /// <param name="nomeAzienda"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByAzienda(
                            string nomeAzienda, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                nomeAzienda = DMD.Strings.Trim(nomeAzienda);
                
                if (string.IsNullOrEmpty(nomeAzienda))
                    return;
                
                var items = Sistema.IndexingService.Find(nomeAzienda, default); // filter.nMax)
                if (items.Count == 0)
                    return;

                var tmp = new List<int>();
                foreach (CResult res in items)
                    tmp.Add(res.OwnerID);
                tmp.Sort();
                int[] arr = tmp.ToArray();

                using (var cursor = new Anagrafica.CPersonaFisicaCursor())
                {
                    this.ApplyFilterToCursor(filter, cursor);
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    cursor.Impiego_IDAzienda.ValueIn(arr);
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base all'ente pagante
            /// </summary>
            /// <param name="nomeAzienda"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByEnte(string nomeAzienda, CCollection<Anagrafica.CPersonaInfo> col, Anagrafica.CRMFindParams filter, int[] findIn)
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                
                nomeAzienda = DMD.Strings.Trim(nomeAzienda);
                if (DMD.Strings.Len(nomeAzienda) < 3)
                    return;

                var items = Sistema.IndexingService.Find(nomeAzienda, default); // filter.nMax)
                if (items.Count == 0)
                    return;

                var tmp = new List<int>();
                foreach (CResult res in items)
                    tmp.Add(res.OwnerID);
                tmp.Sort();
                int[] arr = tmp.ToArray();
                

                using (var cursor = new Anagrafica.CPersonaFisicaCursor()) 
                {
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Impiego_IDEntePagante.ValueIn(arr);
                    
                    this.ApplyFilterToCursor(filter, cursor);

                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    
                    
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item); 
                        cnt += 1;
                    }
                }
                 
            }

            /// <summary>
            /// Cerca le persone in base alla categoria d'appartenenza
            /// </summary>
            /// <param name="categoriaAzienda"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByCategoria(string categoriaAzienda, CCollection<Anagrafica.CPersonaInfo> col, Anagrafica.CRMFindParams filter, int[] findIn)
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                categoriaAzienda = DMD.Strings.Trim(categoriaAzienda);
                if (string.IsNullOrEmpty(categoriaAzienda))
                    return;
                using (var cursor = new Anagrafica.CPersonaFisicaCursor()) {
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Impiego_CategoriaAzienda.Value = categoriaAzienda;
                    cursor.Impiego_CategoriaAzienda.Operator = OP.OP_EQ;
                    this.ApplyFilterToCursor(filter, cursor);
                    
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item); 
                        cnt += 1;
                    }
                }
                 
            }

            /// <summary>
            /// Cerca le persone in base alla tipologia azienda
            /// </summary>
            /// <param name="tipologiaAzienda"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByTipologia(
                            string tipologiaAzienda, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                tipologiaAzienda = DMD.Strings.Trim(tipologiaAzienda);
                if (string.IsNullOrEmpty(tipologiaAzienda))
                    return;

                using (var cursor = new Anagrafica.CPersonaFisicaCursor())
                {
                    this.ApplyFilterToCursor(filter, cursor);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Impiego_TipologiaAzienda.Value = tipologiaAzienda;
                    cursor.Impiego_TipologiaAzienda.Operator = OP.OP_EQ;
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
                 
            }

            /// <summary>
            /// Cerca le persone in base all'indirizzo
            /// </summary>
            /// <param name="indirizzo"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByIndirizzo(
                    string indirizzo, 
                    CCollection<Anagrafica.CPersonaInfo> col, 
                    Anagrafica.CRMFindParams filter, 
                    int[] findIn
                    )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                indirizzo = DMD.Strings.Trim(indirizzo);
                if (string.IsNullOrEmpty(indirizzo))
                    return;

                var address = new Anagrafica.CIndirizzo(indirizzo);
                if (string.IsNullOrEmpty(address.Via) && string.IsNullOrEmpty(address.Citta))
                    return;

                var list = new List<int>();
                using(var cursor = new CIndirizziCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (!string.IsNullOrEmpty(address.Via))
                    {
                        cursor.Via.Value = address.Via;
                        if (Strings.Len(address.Via) > 3) cursor.Via.Operator = OP.OP_LIKE;
                    }
                    if (!string.IsNullOrEmpty(address.Citta))
                    {
                        cursor.Citta.Value = address.Citta;
                        if (Strings.Len(address.Citta) > 3) cursor.Citta.Operator = OP.OP_LIKE;
                    }
                    if (!string.IsNullOrEmpty(address.Provincia))
                    {
                        cursor.Provincia.Value = address.Provincia;
                        if (Strings.Len(address.Provincia) > 3) cursor.Provincia.Operator = OP.OP_LIKE;
                    }
                    if (!string.IsNullOrEmpty(address.Civico))
                    {
                        cursor.Civico.Value = address.Civico;
                    }
                    if (!string.IsNullOrEmpty(address.CAP))
                    {
                        cursor.Civico.Value = address.CAP;
                    }

                    while (cursor.Read())
                    {
                        list.Add(cursor.Item.PersonaID);
                    }
                }

                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(findIn, arr);
                if (arr.Length == 0) return;

                
                using (var cursor = new CPersonaCursor())
                {
                    cursor.ID.ValueIn(arr);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);
                    int cnt = 0;
                    while (cursor.Read() & (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        var p = cursor.Item;
                        this.AddPersonInfo(col, p);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base al parametro
            /// </summary>
            /// <param name="param"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            private void Find_ByID(string param, CCollection<Anagrafica.CPersonaInfo> col, Anagrafica.CRMFindParams filter)
            {
                int id = DMD.Integers.ValueOf(param);
                var item = Anagrafica.Persone.GetItemById(id);
                if (item is object)
                {
                    AddPersonInfo(col, item);
                }
            }

            /// <summary>
            /// Restituisce tutte le persone nel comune specificato
            /// </summary>
            /// <param name="indirizzo"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByComune(
                        string indirizzo, 
                        CCollection<Anagrafica.CPersonaInfo> col, 
                        Anagrafica.CRMFindParams filter, 
                        int[] findIn
                        )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                
                string nomeComune = Anagrafica.Luoghi.GetComune(indirizzo);
                string nomeProvincia = Anagrafica.Luoghi.GetProvincia(indirizzo);
                
                if (string.IsNullOrEmpty(nomeComune))
                    return;

                var list = new List<int>();
                using(var cursor = new CIndirizziCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (!string.IsNullOrEmpty(nomeComune))
                        cursor.Citta.Value = nomeComune;
                    if (!string.IsNullOrEmpty(nomeProvincia))
                        cursor.Provincia.Value = nomeProvincia;
                    while (cursor.Read())
                    {
                        list.Add(cursor.Item.PersonaID);
                    }
                }
                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(arr, findIn);
                
                using(var cursor = new CPersonaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ID.ValueIn(arr);
                    this.ApplyFilterToCursor(filter, cursor);
                    int cnt = 0;
                    while (cursor.Read() & (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        var p = cursor.Item;
                        AddPersonInfo(col, p);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cera le persone in base alla provincia
            /// </summary>
            /// <param name="nomeProvincia"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByProvincia(
                                    string nomeProvincia, 
                                    CCollection<Anagrafica.CPersonaInfo> col, 
                                    Anagrafica.CRMFindParams filter, 
                                    int[] findIn
                                    )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                if (DMD.Arrays.Len(findIn) > 10000)
                    throw new OutOfMemoryException("Query troppo complessa");

                nomeProvincia = DMD.Strings.Trim(nomeProvincia);
                if (string.IsNullOrEmpty(nomeProvincia))
                    return;

                var list = new List<int>();
                using(var cursor = new CIndirizziCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (Strings.Len(nomeProvincia) > 3)
                    {
                        cursor.Provincia.Value = nomeProvincia;
                    }
                    else
                    {
                        cursor.Provincia.Value = nomeProvincia;
                        cursor.Provincia.Operator = OP.OP_LIKE;
                    }
                    while (cursor.Read())
                    {
                        list.Add(cursor.Item.PersonaID);
                    }
                }

                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(arr, findIn);
                
                using(var cursor = new CPersonaCursor ())
                {
                    this.ApplyFilterToCursor(filter, cursor);
                    cursor.ID.ValueIn(arr);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    int cnt = 0;
                    while (cursor.Read() & (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        var p = cursor.Item;
                        this.AddPersonInfo(col, p);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base al codice fiscale o alla partita iva
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByCFOrPIVA(
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                string cf = Sistema.Formats.ParseCodiceFiscale(value);
                string piva = Sistema.Formats.ParsePartitaIVA(value);
                
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    
                    int cnt = 0;
                    if (DMD.Strings.Len(cf) >= 6)
                    {
                        if (findIn is object)
                            cursor.ID.ValueIn(findIn);
                        this.ApplyFilterToCursor(filter, cursor);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.CodiceFiscale.Value = DMD.Strings.JoinW(cf, "%");
                        cursor.CodiceFiscale.Operator = OP.OP_LIKE;
                        while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                        {
                            AddPersonInfo(col, cursor.Item);                            
                            cnt += 1;
                        }

                        cursor.Reset1();
                    }

                    if (cnt == 0 && DMD.Strings.Len(piva) >= 11)
                    {
                        if (findIn is object)
                            cursor.ID.ValueIn(findIn);
                        this.ApplyFilterToCursor(filter, cursor);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.CodiceFiscale.Clear();
                        cursor.PartitaIVA.Value = piva;
                        cursor.PartitaIVA.Operator = OP.OP_LIKE;
                        while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                        {
                            AddPersonInfo(col, cursor.Item);
                            cnt += 1;
                        }
                    }
                }
                 
            }

            /// <summary>
            /// Cerca le persone in base alla partita iva
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByPIVA(
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                string piva = Sistema.Formats.ParsePartitaIVA(value);
                if (DMD.Strings.Len(piva) <= 6)
                    return;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);

                    this.ApplyFilterToCursor(filter, cursor);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.PartitaIVA.Value = DMD.Strings.JoinW(piva, "%");
                    cursor.PartitaIVA.Operator = OP.OP_LIKE;
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base al numero di telefono
            /// </summary>
            /// <param name="value"></param>
            /// <param name="filter"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersonaInfo> FindByTelefono(string value, Anagrafica.CRMFindParams filter)
            {
                var ret = new CCollection<Anagrafica.CPersonaInfo>();
                Find_ByTelefono(value, ret, filter, null);
                return ret;
            }

            /// <summary>
            /// Cerca le persone in base all'indirizzo email
            /// </summary>
            /// <param name="value"></param>
            /// <param name="filter"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersonaInfo> FindByEMail(string value, Anagrafica.CRMFindParams filter)
            {
                var ret = new CCollection<Anagrafica.CPersonaInfo>();
                Find_ByEMail(value, ret, filter, null);
                return ret;
            }

            /// <summary>
            /// Cerca le persone in base al numero di telefono
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByTelefono(
                        string value, 
                        CCollection<Anagrafica.CPersonaInfo> col, 
                        Anagrafica.CRMFindParams filter, 
                        int[] findIn
                        )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                string num = Sistema.Formats.ParsePhoneNumber(value);
                if (DMD.Strings.Len(num) < 3)
                    return;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);
                    cursor.Telefono.Value = DMD.Strings.JoinW(num, "%");
                    cursor.Telefono.Operator = OP.OP_LIKE;
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base all'indirizzo email
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByEMail(
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                string param = DMD.Strings.Replace(Sistema.Formats.ParseEMailAddress(value), "'", "''");
                if (DMD.Strings.Len(param) < 3)
                    return;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    cursor.eMail.Value = DMD.Strings.JoinW(param, "%");
                    cursor.eMail.Operator = OP.OP_LIKE;
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base all'indirizzo web
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByWebSite(
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                string param = Sistema.Formats.ParseWebAddress(value);
                if (DMD.Strings.Len(param) < 3)
                    return;
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    cursor.WebSite.Value = DMD.Strings.JoinW(param, "%");
                    cursor.WebSite.Operator = OP.OP_LIKE;
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base alla data di ricontatto
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByDataRicontatto(
                                string value, 
                                CCollection<Anagrafica.CPersonaInfo> col, 
                                Anagrafica.CRMFindParams filter, 
                                int[] findIn
                                )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");

                value = DMD.Strings.Replace(DMD.Strings.Trim(value), "  ", " ");

                var lstIn = new List<int>();
                var lstNotIn = new List<int>();

                using(var cursor = new CRicontattiCursor ())
                {
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoRicontatto.ValueIn(new StatoRicontatto[] { StatoRicontatto.PROGRAMMATO, StatoRicontatto.RIMANDATO });
                    switch (value ?? "")
                    {
                        case "nessuna":
                        case "non impostata":
                        case "nessuo":
                        case "null":
                            {
                                // Cerco tutte le anagrafiche senza data di ricontatto
                                while (cursor.Read())
                                {
                                    lstNotIn.Add(cursor.Item.IDPersona);
                                }

                                break;
                            }

                        case var @case when @case == "":
                            {
                                break;
                            }

                        default:
                            {
                                string[] nibbles;
                                DateTime? da = default;
                                DateTime? a = default;

                                if (DMD.Strings.StartsWidth(value, "tra"))
                                {
                                    nibbles = DMD.Strings.Split(DMD.Strings.Mid(value, 4), " e ");
                                    if (nibbles.Length == 2)
                                    {
                                        da = Sistema.Formats.ToDate(nibbles[0]);
                                        a = Sistema.Formats.ToDate(nibbles[1]);
                                    }
                                    else
                                    {
                                        throw new FormatException("Il formato deve essere del tipo 'tra 31/12/2000 e 01/01/2001'");
                                    }
                                }
                                else if (DMD.Strings.StartsWidth(value, "dopo il", true))
                                {
                                    da = Sistema.Formats.ToDate(DMD.Strings.Mid(value, 8));
                                }
                                else if (DMD.Strings.StartsWidth(value, "prima del", true))
                                {
                                    a = Sistema.Formats.ToDate(DMD.Strings.Mid(value, 10));
                                }
                                else
                                {
                                    var interval = DMD.DateUtils.PeriodoToDates(value, da, a);
                                    da = interval.Inizio;
                                    a = interval.Fine;
                                    if (da.HasValue && a.HasValue && da.Value == a.Value)
                                        a = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 24 * 3600 - 1, da.Value);
                                }

                                cursor.DataPrevista.Between(da, a);
                                 
                                while (cursor.Read())
                                {
                                    lstIn.Add(cursor.Item.IDPersona);
                                }

                                break;
                            }
                    }
                }

                lstIn.Sort();
                lstNotIn.Sort();

                var arrIn = lstIn.ToArray();
                var arrNotIn = lstNotIn.ToArray();

                if (findIn is object)
                    arrIn = DMD.Arrays.Join(findIn, arrIn);
                    
                 
                using (var cursor = new Anagrafica.CPersonaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);
                    
                    if (arrIn is object) cursor.WhereClauses = cursor.WhereClauses.And<int>("ID").In(arrIn);
                    if (arrNotIn is object) cursor.WhereClauses = cursor.WhereClauses.And<int>("ID").NotIn(arrNotIn);

                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        var item = cursor.Item;
                        AddPersonInfo(col, item);
                        cnt += 1;
                    }
                }
                
            }

            /// <summary>
            /// Carica i dati delle persone per i ricontatti
            /// </summary>
            /// <param name="ricontatti"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersona> SincronizzaPersone(
                            CCollection<Anagrafica.CRicontatto> ricontatti
                            )
            {
                var arr = new Dictionary<int, int>();
                foreach (Anagrafica.CRicontatto ric in ricontatti)
                {
                    if (
                           ric.IDPersona != 0
                        && !arr.ContainsKey(ric.IDPersona)
                        )
                        arr.Add(ric.IDPersona, ric.IDPersona);
                }

                var tmp = new CKeyCollection<Anagrafica.CPersona>();
                if (arr.Count > 0)
                {
                    using (var cursor = new Anagrafica.CPersonaCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.ID.ValueIn(new List<int>(arr.Values).ToArray());
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            tmp.Add("K" + DBUtils.GetID(cursor.Item), cursor.Item);
                        }
                    }

                    foreach (Anagrafica.CRicontatto ric in ricontatti)
                        ric.SetPersona(tmp.GetItemByKey("K" + ric.IDPersona));
                }

                return new CCollection<Anagrafica.CPersona>(tmp);
            }

            /// <summary>
            /// Cerca le persone contenute nella lista
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByLista(
                            string nomeLista, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter,
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                nomeLista = DMD.Strings.Trim(nomeLista);
                if (string.IsNullOrEmpty(nomeLista))
                    return;

                 
                var list = new List<int>();
                var ricontatti = new CCollection<Anagrafica.CRicontatto>();
                Anagrafica.CRicontatto ric;

                using (var cursor = new Anagrafica.ListaRicontattoItemCursor())
                {
                    if (DMD.Strings.Right(nomeLista, 1) != "%")
                        nomeLista = DMD.Strings.JoinW(nomeLista, "%");

                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);

                    cursor.NomeLista.Value = nomeLista;
                    cursor.NomeLista.Operator = OP.OP_LIKE;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = filter.ignoreRights;
                    cursor.PageSize = 1000;
                   
                    
                    while (cursor.Read()) // AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    {
                        ric = cursor.Item;
                        list.Add(ric.IDPersona);
                        ricontatti.Add(ric);                        
                    }
                }

                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(arr, findIn);

                using(var cursor = new CPersonaCursor())
                {
                    cursor.ID.ValueIn(arr);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);

                    int i = 0;
                    while (cursor.Read() && (filter.nMax == null || i < filter.nMax.Value))
                    {
                        var p = cursor.Item;
                        var info = AddPersonInfo(col, p);
                        
                        i += 1;
                    }
                }

                SincronizzaPersone(ricontatti);
                //foreach (var currentRic in ricontatti)
                //{
                //    ric = currentRic;
                //    if (ric.Persona is object)
                //    {
                //        if ((filter.tipoPersona.HasValue == false || filter.tipoPersona.Value == ric.Persona.TipoPersona) && (filter.flags.HasValue == false || Sistema.TestFlag(ric.Persona.Flags, filter.flags.Value)) && (string.IsNullOrEmpty(filter.DettaglioEsito) || (filter.DettaglioEsito ?? "") == (ric.Persona.DettaglioEsito ?? "")))
                //        {
                //            var info = AddPersonInfo(col, ric.Persona);
                //            cnt += 1;
                //            if (filter.nMax.HasValue && cnt >= filter.nMax.Value)
                //                break;
                //        }
                //    }
                //}
               
                 
            }

            /// <summary>
            /// Cerca le persone in base alla data di compleanno
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByCompleanno(
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");

                value = DMD.Strings.Replace(DMD.Strings.Trim(value), "  ", "");
                var intervallo = DMD.DateUtils.PeriodoToDates(value, default, default);
                var d1 = DMD.DateUtils.GetDatePart(intervallo.Inizio);
                var d2 = DMD.DateUtils.GetLastSecond(intervallo.Fine);
                int? m1 = DMD.DateUtils.Month(d1);
                int? g1 = DMD.DateUtils.Day(d2);
                int? m2 = DMD.DateUtils.Month(d1);
                int? g2 = DMD.DateUtils.Day(d2);

                using (var cursor = new CPersonaFisicaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    this.ApplyFilterToCursor(filter, cursor);
                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);

                    switch (DMD.Strings.LCase(value) ?? "")
                    {
                        case "domani":
                        case "oggi":
                        case "ieri":
                            {
                                cursor.WhereClauses.And(DBCursorField.Field<DateTime>("DataNascita").Month().EQ(m1))
                                                   .And(DBCursorField.Field<DateTime>("DataNascita").Day().EQ(d1));
                            }
                            break;
                        case "questo mese":
                        case "il mese scorso":
                        case "lo scorso mese":
                        case "il mese prossimo":
                        case "il prossimo mese":
                            {
                                cursor.WhereClauses.And(DBCursorField.Field<DateTime>("DataNascita").Month().EQ(m1));
                            }
                            break;
                        case "questa settimana":
                        case "la prossima settimana":
                        case "la settimana prossima":
                        case "la scorsa settimana":
                        case "la settimana scorsa":
                        case "tra":
                            {
                                if (m1 == m2)
                                {
                                    cursor.WhereClauses.And(DBCursorField.Field<DateTime>("DataNascita").Month().EQ(m1))
                                                   .And(DBCursorField.Field<DateTime>("DataNascita").Day().Between(d1, d2));

                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                            }
                            break; 
                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }   
                     
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base al tipo di relazione rispetto alle persone specificate
            /// </summary>
            /// <param name="tipoRelazione"></param>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByRelazione(
                        string tipoRelazione, 
                        string value, 
                        CCollection<Anagrafica.CPersonaInfo> col, 
                        Anagrafica.CRMFindParams filter,
                        int[] findIn
                        )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return;

                var list = new List<int>();
                
                using (var cursor = new Anagrafica.CRelazioneParentaleCursor())
                {
                    cursor.IgnoreRights = filter.ignoreRights;
                    cursor.NomeRelazione.Value = tipoRelazione;
                    cursor.NomePersona2.Value = DMD.Strings.JoinW("%", value, "%");
                    cursor.NomePersona2.Operator = OP.OP_LIKE;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        var rel = cursor.Item;
                        list.Add(rel.IDPersona1);
                    }
                }

                var inverse = Anagrafica.RelazioniParentali.GetInvertedRelations(tipoRelazione);
                foreach (string str in inverse)
                {
                    using (var cursor = new Anagrafica.CRelazioneParentaleCursor())
                    {
                        cursor.IgnoreRights = filter.ignoreRights;
                        cursor.NomeRelazione.Value = str;
                        cursor.NomePersona1.Value = DMD.Strings.JoinW("%", value, "%");
                        cursor.NomePersona1.Operator = OP.OP_LIKE;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        while (cursor.Read())
                        {
                            var rel = cursor.Item;
                            list.Add(rel.IDPersona2);
                        }
                    }
                }

                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(findIn, arr);

                using(var cursor = new CPersonaFisicaCursor())
                {
                    cursor.ID.ValueIn(findIn);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;


                    if (filter.IDPuntoOperativo != 0) cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.flags.HasValue )
                    {
                        cursor.PFlags.Value = filter.flags.Value;
                        cursor.PFlags.Operator = OP.OP_ALLBITAND;
                    }
                    if (!string.IsNullOrEmpty(filter.DettaglioEsito)) cursor.DettaglioEsito.Value = filter.DettaglioEsito;

                    int i = 0;
                    while (cursor.Read () && (filter.nMax == null || i < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        i += 1;
                    }

                }
                 
            }
            
            /// <summary>
            /// Cerca le persone in base al tipo di relazione inversa
            /// </summary>
            /// <param name="tipoRelazione"></param>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByRelazioneInversa(
                            string tipoRelazione, 
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter,
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return;

                var list = new List<int>();
                using (var cursor = new Anagrafica.CRelazioneParentaleCursor())
                {
                    cursor.IgnoreRights = filter.ignoreRights;
                    cursor.NomeRelazione.Value = tipoRelazione;
                    cursor.NomePersona1.Value = DMD.Strings.JoinW("%", value, "%");
                    cursor.NomePersona1.Operator = OP.OP_LIKE;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        var rel = cursor.Item;
                        list.Add(rel.IDPersona2);
                    }
                }

                var inverse = Anagrafica.RelazioniParentali.GetInvertedRelations(tipoRelazione);
                foreach (string str in inverse)
                {
                    using (var cursor = new Anagrafica.CRelazioneParentaleCursor())
                    {
                        cursor.IgnoreRights = filter.ignoreRights;
                        cursor.NomeRelazione.Value = str;
                        cursor.NomePersona2.Value = DMD.Strings.JoinW("%", value, "%");
                        cursor.NomePersona2.Operator = OP.OP_LIKE;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        while (cursor.Read())
                        {
                            var rel = cursor.Item;
                            list.Add(rel.IDPersona1);
                        }
                    }
                }

                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(findIn, arr);

                using (var cursor = new CPersonaFisicaCursor())
                {
                    cursor.ID.ValueIn(findIn);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;


                    if (filter.IDPuntoOperativo != 0) cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.flags.HasValue)
                    {
                        cursor.PFlags.Value = filter.flags.Value;
                        cursor.PFlags.Operator = OP.OP_ALLBITAND;
                    }
                    if (!string.IsNullOrEmpty(filter.DettaglioEsito)) cursor.DettaglioEsito.Value = filter.DettaglioEsito;

                    int i = 0;
                    while (cursor.Read() && (filter.nMax == null || i < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        i += 1;
                    }

                }
            }

            /// <summary>
            /// Cerca le persone in base al nome (usando l'indicizzatore)
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByNomeIntelli(
                            string value, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                //const int DIMSCAGLIONE = 5000;    // La ricerca nella tabella persone viene scaglionata

                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                
                var words = Sistema.IndexingService.SplitWords(value);

                var items = Sistema.IndexingService.Find(value, findIn, filter.nMax);
                if (items.Count == 0)
                    return;

                var list = new List<int>();
                foreach(var o in items)
                {
                    list.Add(o.OwnerID);
                }
                list.Sort();
                var arr = list.ToArray();
                arr = DMD.Arrays.Join(arr, findIn);

                using (var cursor = new CPersonaCursor())
                {
                    cursor.ID.ValueIn(arr);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.tipoPersona.HasValue) cursor.TipoPersona.Value = filter.tipoPersona.Value;
                    if (!string.IsNullOrEmpty(filter.DettaglioEsito)) cursor.DettaglioEsito.Value = filter.DettaglioEsito;
                    if (filter.flags.HasValue) 
                    {
                        cursor.PFlags.Value = filter.flags.Value;
                        cursor.PFlags.Operator = OP.OP_ALLBITAND;
                    }
                    if (filter.IDPuntoOperativo != 0) cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;

                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax == null || cnt < filter.nMax.Value ))
                    {
                        var o = cursor.Item;
                        if (this.VerificaNomePersona(words, o))
                            this.AddPersonInfo(col, o);
                    }
                             
                }

                
                 
            }

            /// <summary>
            /// Restituisce le parole indicizzate per la persona
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            private WordInfo[] GetWords(Anagrafica.CPersona o)
            {
                return Sistema.IndexingService.GetIndexableWords(o);
            }

            /// <summary>
            /// Clona l'array
            /// </summary>
            /// <param name="words"></param>
            /// <returns></returns>
            private WordInfo[] CloneWords(WordInfo[] words)
            {
                WordInfo[] ret;
                ret = new WordInfo[DMD.Arrays.UBound(words) + 1];
                for (int i = 0, loopTo = DMD.Arrays.UBound(words); i <= loopTo; i++)
                    ret[i] = (WordInfo)words[i].Clone();
                return ret;
            }


            private bool VerificaNomePersona(WordInfo[] wordsToFind, object o)
            {
                return true;

                // Dim words1() As Sistema.CIndexingService.WordInfo = Me.GetWords(o)
                // Dim words() As Sistema.CIndexingService.WordInfo = Me.CloneWords(wordsToFind)

                // For Each w As Sistema.CIndexingService.WordInfo In words
                // If w.IsLike Then
                // For Each w1 As Sistema.CIndexingService.WordInfo In words1
                // If DMD.Strings.Left(w1.Word, Len(w.Word)) = w.Word AndAlso w.Frequenza > 0 AndAlso w1.Frequenza > 0 Then
                // w.Frequenza -= 1
                // w1.Frequenza -= 1
                // Exit For
                // End If
                // Next
                // Else
                // For Each w1 As Sistema.CIndexingService.WordInfo In words1
                // If w.Word = w1.Word AndAlso w.Frequenza > 0 AndAlso w1.Frequenza > 0 Then
                // w.Frequenza -= 1
                // w1.Frequenza -= 1
                // End If
                // Next
                // End If
                // Next

                // For Each w As Sistema.CIndexingService.WordInfo In words
                // If w.Frequenza > 0 Then Return False
                // Next
                // Return True

                // ''Dim w, w1 As Sistema.CIndexingService.WordInfo
                // 'If (DMD.Arrays.Len(words1) < 0) Then Return False

                // ''Dim i As Integer = 0
                // ''Dim j As Integer
                // ''While (i <= UBound(words))
                // ''    w = words(i)
                // ''    If (w.IsLike) Then
                // ''    Else
                // ''        j = 0
                // ''        While (j <= UBound(words1))
                // ''            w1 = words1(j)
                // ''            If w1.Word = w.Word Then
                // ''                If (w1.Frequenza < w.Frequenza) Then
                // ''                    Return False
                // ''                Else
                // ''                    words = DMD.Arrays.RemoveAt(words, i)
                // ''                    words1 = DMD.Arrays.RemoveAt(words1, j)
                // ''                End If
                // ''            Else
                // ''                j += 1
                // ''            End If
                // ''        End While
                // ''    End If
                // ''End While

                // ''w1 = words1(UBound(words1))
                // ''Return DMD.Strings.Left(w1.Word, Len(w.Word)) = w.Word AndAlso w1.Frequenza >= w.Frequenza

                // 'Return True
            }
            /// <summary>
            /// Cerca le persone in base al nominativo (modalità standard)
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByNomeStandard(
                                        string value, 
                                        CCollection<Anagrafica.CPersonaInfo> col, 
                                        Anagrafica.CRMFindParams filter, 
                                        int[] findIn
                                        )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");

                if (col is null)
                    throw new ArgumentNullException("col");

                string name = DMD.Strings.Replace(DMD.Strings.Trim(value), "  ", " ");
                name = DMD.Strings.Replace(value, "'", "''");
                
                
                using (var cursor = new Anagrafica.CPersonaCursor())
                {
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);

                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = filter.ignoreRights;
                    
                    if (!string.IsNullOrEmpty(filter.DettaglioEsito)) cursor.DettaglioEsito.Value = filter.DettaglioEsito;
                    if (filter.tipoPersona.HasValue) cursor.TipoPersona.Value = filter.tipoPersona.Value;
                    if (filter.flags.HasValue)
                    {
                        cursor.PFlags.Value = filter.flags.Value;
                        cursor.PFlags.Operator = OP.OP_ALLBITAND;
                    }

                    if (DMD.Strings.Len(name) < 3)
                    {
                        var f1 = DBCursorField.Field<string>("Alias1").EQ(name);
                        var f2 = DBCursorField.Field<string>("Alias2").EQ(name);
                        var f = f1.Or(f2);

                        cursor.WhereClauses = cursor.WhereClauses.And(f);                                         
                    }
                    else
                    {
                        var f1 = DBCursorField.Field<string>("Alias1").IsLike(name);
                        var f2 = DBCursorField.Field<string>("Alias2").IsLike(name);
                        var f = f1.Or(f2);

                        cursor.WhereClauses = cursor.WhereClauses.And(f);
                    }

                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Cerca le persone in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByNome(
                                    string value, 
                                    CCollection<Anagrafica.CPersonaInfo> col, 
                                    Anagrafica.CRMFindParams filter, 
                                    int[] findIn
                                    )
            {
                value = DMD.Strings.Trim(value);
                value = DMD.Strings.OnlyCharsWS(value);
                int i = DMD.Strings.ContaRipetizioni(value, " ");
                if (!string.IsNullOrEmpty(value) && Sistema.IndexingService.Database is object && (filter.IntelliSearch || i <= MAXWORDSINTELLISEARCH))
                {
                    Find_ByNomeIntelli(value, col, filter, findIn);
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    Find_ByNomeStandard(value, col, filter, findIn);
                }
            }

            /// <summary>
            /// Cerca le persone in base al tipo rapporto
            /// </summary>
            /// <param name="tipoRapporto"></param>
            /// <param name="col"></param>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            private void Find_ByTipoRapporto(
                            string tipoRapporto, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            Anagrafica.CRMFindParams filter, 
                            int[] findIn
                            )
            {
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (col is null)
                    throw new ArgumentNullException("col");
                var tr = Anagrafica.TipiRapporto.GetItemByIdTipoRapporto(tipoRapporto);
                if (tr is object)
                    tipoRapporto = tr.IdTipoRapporto;
                if (string.IsNullOrEmpty(tipoRapporto))
                    return;
                using (var cursor = new Anagrafica.CPersonaFisicaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Impiego_TipoRapporto.Value = tipoRapporto;
                    cursor.Impiego_TipoRapporto.Operator = OP.OP_EQ;
                    cursor.IgnoreRights = filter.ignoreRights;
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    if (!string.IsNullOrEmpty(filter.DettaglioEsito))
                        cursor.DettaglioEsito.Value = filter.DettaglioEsito;
                    if (filter.tipoPersona.HasValue)
                        cursor.TipoPersona.Value = filter.tipoPersona.Value;
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.flags.HasValue)
                    {
                        cursor.PFlags.Value = filter.flags.Value;
                        cursor.PFlags.Operator = OP.OP_ALLBITAND;
                    }

                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);                         
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Comparatore tramite IDPersona
            /// </summary>
            private class ByIdComparer : IComparer
            {
                public int Compare(object x, object y)
                {
                    return ((Anagrafica.CPersonaInfo)x).IDPersona.CompareTo(((Anagrafica.CPersonaInfo)y).IDPersona);
                }
            }

            /// <summary>
            /// Cerca
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersonaInfo> Find(Anagrafica.CRMFindParams filter)
            {
                return Find(filter, null);
            }

            /// <summary>
            /// Cerca
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            /// <returns></returns>
            private CCollection<Anagrafica.CPersonaInfo> Find(Anagrafica.CRMFindParams filter, int[] findIn)
            {
                const string CMDID = "id:";
                const string CMDAZIENDA = "azienda:";
                const string CMDENTE = "ente:";
                const string CMDCATEGORIA = "categoria:";
                const string CMDTIPOLOGIA = "tipologia:";
                const string CMDTELEFONO = "telefono:";
                const string CMDINDIRIZZO = "indirizzo:";
                const string CMDNOME = "nome:";
                const string CMDPROVINCIA = "provincia:";
                const string CMDCOMUNE = "comune:";
                const string CMDEMAIL = "e-mail:";
                const string CDCF = "codice fiscale:";
                const string PIVA = "partita iva:";
                const string CDTR = "tipo rapporto:";
                const string CDCOMPLEANNO = "compleanno:";
                const string CMDDATARICONTATTO = "data ricontatto:";
                const string CMDLISTA = "lista:";
                const string CMDNUMERODOCUMENTO = "numero documento:";
                const string CMDPO = "punto operativo:";
                CCollection<Anagrafica.CPersonaInfo> retCol = null;
                string name, cf, num; // name1
                string parametriLista = filter.Text;
                parametriLista = DMD.Strings.Replace(DMD.Strings.Trim(parametriLista), "  ", " ");
                parametriLista = DMD.Strings.Replace(parametriLista, DMD.Strings.vbCr, DMD.Strings.vbLf);
                var @params = DMD.Strings.Split(parametriLista, DMD.Strings.vbLf);
                for (int i = 0, loopTo = DMD.Arrays.UBound(@params); i <= loopTo; i++)
                {
                    string param = @params[i];
                    var col = new CCollection<Anagrafica.CPersonaInfo>();
                    param = DMD.Strings.Trim(DMD.Strings.Replace(param, "  ", " "));
                    name = param;
                    cf = Sistema.Formats.ParseCodiceFiscale(name);
                    num = Sistema.Formats.ParsePhoneNumber(name);

                    // name = LCase(Replace(name, "'", "''"))
                    // name1 = DMD.Strings.OnlyChars(name)
                    // cf = Replace(cf, "'", "''")
                    // num = Replace(num, "'", "''")

                    col.Sorted = false;
                    switch (DMD.Strings.LCase(DMD.Strings.Trim(filter.Tipo)) ?? "")
                    {
                        case "id":
                            {
                                Find_ByID(filter.Text, col, filter);
                                break;
                            }

                        case "azienda":
                            {
                                Find_ByAzienda(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "ente":
                            {
                                Find_ByEnte(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "categoria":
                            {
                                Find_ByCategoria(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "tipologia":
                            {
                                Find_ByTipologia(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "telefono":
                            {
                                Find_ByTelefono(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "indirizzo":
                            {
                                Find_ByIndirizzo(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "nome":
                            {
                                Find_ByNome(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "provincia":
                            {
                                Find_ByProvincia(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "comune":
                            {
                                Find_ByComune(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "e-mail":
                            {
                                Find_ByEMail(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "codice fiscale":
                            {
                                Find_ByCFOrPIVA(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "partita iva":
                            {
                                Find_ByPIVA(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "tipo rapporto":
                            {
                                Find_ByTipoRapporto(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "compleanno":
                            {
                                Find_ByCompleanno(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "data ricontatto":
                            {
                                Find_ByDataRicontatto(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "lista":
                            {
                                Find_ByLista(filter.Text, col, filter, findIn);
                                break;
                            }

                        case "numero documento":
                            {
                                Find_ByNumeroDocumento(filter.Text, filter, col, findIn);
                                break;
                            }

                        case "punto operativo":
                            {
                                Find_ByPO(filter.Text, filter, col, findIn);
                                break;
                            }

                        default:
                            {
                                bool t = false;
                                var gestori = GetInstalledFindHandlers();
                                foreach (Anagrafica.FindPersonaHandler g in gestori)
                                {
                                    t = (DMD.Strings.LCase(DMD.Strings.Trim(g.GetHandledCommand())) ?? "") == (DMD.Strings.LCase(DMD.Strings.Trim(filter.Tipo)) ?? "");
                                    if (t)
                                    {
                                        g.Find(name, filter, col);
                                        break;
                                    }
                                }

                                if (!t)
                                {
                                    if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDAZIENDA)) ?? "") == CMDAZIENDA)
                                    {
                                        Find_ByAzienda(DMD.Strings.Mid(param, DMD.Strings.Len(CMDAZIENDA) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDENTE)) ?? "") == CMDENTE)
                                    {
                                        Find_ByEnte(DMD.Strings.Mid(param, DMD.Strings.Len(CMDENTE) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDTIPOLOGIA)) ?? "") == CMDTIPOLOGIA)
                                    {
                                        Find_ByTipologia(DMD.Strings.Mid(param, DMD.Strings.Len(CMDTIPOLOGIA) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDCATEGORIA)) ?? "") == CMDCATEGORIA)
                                    {
                                        Find_ByCategoria(DMD.Strings.Mid(param, DMD.Strings.Len(CMDCATEGORIA) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDID)) ?? "") == CMDID)
                                    {
                                        Find_ByID(DMD.Strings.Mid(param, DMD.Strings.Len(CMDID) + 1), col, filter);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDINDIRIZZO)) ?? "") == CMDINDIRIZZO)
                                    {
                                        Find_ByIndirizzo(DMD.Strings.Mid(param, DMD.Strings.Len(CMDINDIRIZZO) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDTELEFONO)) ?? "") == CMDTELEFONO)
                                    {
                                        Find_ByTelefono(DMD.Strings.Mid(param, DMD.Strings.Len(CMDTELEFONO) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDNOME)) ?? "") == CMDNOME)
                                    {
                                        Find_ByNome(DMD.Strings.Mid(param, DMD.Strings.Len(CMDNOME) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDPROVINCIA)) ?? "") == CMDPROVINCIA)
                                    {
                                        Find_ByProvincia(DMD.Strings.Mid(param, DMD.Strings.Len(CMDPROVINCIA) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDCOMUNE)) ?? "") == CMDCOMUNE)
                                    {
                                        Find_ByComune(DMD.Strings.Mid(param, DMD.Strings.Len(CMDCOMUNE) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDEMAIL)) ?? "") == CMDEMAIL)
                                    {
                                        Find_ByEMail(DMD.Strings.Mid(param, DMD.Strings.Len(CMDEMAIL) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDCF)) ?? "") == CDCF)
                                    {
                                        Find_ByCFOrPIVA(DMD.Strings.Mid(param, DMD.Strings.Len(CDCF) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(PIVA)) ?? "") == PIVA)
                                    {
                                        Find_ByPIVA(DMD.Strings.Mid(param, DMD.Strings.Len(PIVA) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDTR)) ?? "") == CDTR)
                                    {
                                        Find_ByTipoRapporto(DMD.Strings.Mid(param, DMD.Strings.Len(CDTR) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDCOMPLEANNO)) ?? "") == CDCOMPLEANNO)
                                    {
                                        Find_ByCompleanno(DMD.Strings.Mid(param, DMD.Strings.Len(CDCOMPLEANNO) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDDATARICONTATTO)) ?? "") == CMDDATARICONTATTO)
                                    {
                                        Find_ByDataRicontatto(DMD.Strings.Mid(param, DMD.Strings.Len(CMDDATARICONTATTO) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDLISTA)) ?? "") == CMDLISTA)
                                    {
                                        Find_ByLista(DMD.Strings.Mid(param, DMD.Strings.Len(CMDLISTA) + 1), col, filter, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDNUMERODOCUMENTO)) ?? "") == CMDNUMERODOCUMENTO)
                                    {
                                        Find_ByNumeroDocumento(DMD.Strings.Mid(param, DMD.Strings.Len(CMDNUMERODOCUMENTO) + 1), filter, col, findIn);
                                    }
                                    else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDPO)) ?? "") == CMDPO)
                                    {
                                        Find_ByPO(DMD.Strings.Mid(param, DMD.Strings.Len(CMDPO) + 1), filter, col, findIn);
                                    }
                                    else
                                    {
                                        var relazioni = Anagrafica.RelazioniParentali.GetSupportedNames();
                                        foreach (string rel in relazioni)
                                        {
                                            if ((DMD.Strings.LCase(DMD.Strings.Left(param, DMD.Strings.Len(rel) + 1)) ?? "") == (DMD.Strings.JoinW(DMD.Strings.LCase(rel), ":") ?? ""))
                                            {
                                                Find_ByRelazione(rel, DMD.Strings.Mid(param, DMD.Strings.Len(rel) + 2), col, filter, findIn);
                                                t = true;
                                            }
                                            else if ((DMD.Strings.LCase(DMD.Strings.Left(param, DMD.Strings.Len(rel) + 4)) ?? "") == (DMD.Strings.JoinW(DMD.Strings.LCase(rel), " di:") ?? ""))
                                            {
                                                Find_ByRelazioneInversa(rel, DMD.Strings.Mid(param, DMD.Strings.Len(rel) + 5), col, filter, findIn);
                                                t = true;
                                            }
                                        }

                                        if (!t)
                                        {
                                            if (IsEmail(name))
                                            {
                                                Find_ByEMail(name, col, filter, findIn);
                                            }
                                            else if (IsWebSite(name))
                                            {
                                                Find_ByWebSite(name, col, filter, findIn);
                                            }
                                            else if (IsCFOPIVA(name))
                                            {
                                                Find_ByCFOrPIVA(name, col, filter, findIn);
                                                if (DMD.Strings.Len(num) > 3)
                                                    Find_ByTelefono(num, col, filter, findIn);
                                            }
                                            else if (IsNumeroDocumento(name))
                                            {
                                                Find_ByNumeroDocumento(name, filter, col, findIn);
                                            }
                                            else if (IsPhoneNumber(name))
                                            {
                                                Find_ByTelefono(num, col, filter, findIn);
                                            }
                                            else
                                            {
                                                t = false;
                                                // Dim gestori As CCollection(Of FindPersonaHandler) = GetInstalledFindHandlers()
                                                foreach (Anagrafica.FindPersonaHandler g in gestori)
                                                {
#if (!DEBUG)
                                                    try
                                                    {
#endif
                                                        t = g.CanHandle(param, filter);
                                                        if (t)
                                                        {
                                                            g.Find(name, filter, col);
                                                            break;
                                                        }
#if (!DEBUG)
                                                } catch (Exception ex) {
                                                        Sistema.Events.NotifyUnhandledException(ex);
                                                    }
#endif
                                                }

                                                if (t == false)
                                                {
                                                    Find_ByNome(name, col, filter, findIn);
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            }
                    }

                    if (retCol is null)
                    {
                        retCol = col;
                    }
                    else
                    {
                        retCol = (CCollection<Anagrafica.CPersonaInfo>)retCol.IntersectWith(col);
                    }
                }

                // retCol.Sort()
                return retCol;
            }

            /// <summary>
            /// Restituisce true se la stringa rappresenta un indirizzo email
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private bool IsEmail(string value)
            {
                value = DMD.Strings.LCase(DMD.Strings.Replace(value, " ", ""));
                return Sistema.EMailer.IsValidAddress(value);
            }

            /// <summary>
            /// Restituisce true se la stringa rappresenta un numero telefonico
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private bool IsPhoneNumber(string value)
            {
                value = DMD.Strings.Trim(value);
                string ch = DMD.Strings.Left(value, 1);
                if (DMD.Strings.InStr("0123456789+", ch) <= 0)
                    return false;
                return DMD.Strings.Len(Sistema.Formats.ParsePhoneNumber(value)) > 3;
            }

            /// <summary>
            /// Restituisce true se la stringa rappresenta un indirizzo web
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private bool IsWebSite(string value)
            {
                value = DMD.Strings.LCase(DMD.Strings.Replace(value, " ", ""));
                if (DMD.Strings.Left(value, 4) == "www." || DMD.Strings.Left(value, 5) == "http:" || DMD.Strings.Left(value, 6) == "https:")
                {
                    return true;
                }
                else if (DMD.Strings.Left(value, 4) == "ftp." || DMD.Strings.Left(value, 4) == "ftp:" || DMD.Strings.Left(value, 7) == "ftps:")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Restituisce true se la stringa rappresenta un numero di documento d'identità
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool IsNumeroDocumento(string value)
            {
                value = DMD.Strings.LCase(DMD.Strings.Replace(value, " ", ""));
                if (DMD.Strings.Len(value) != 9)
                    return false;
                if (char.IsLetter(DMD.Chars.CChar(DMD.Strings.Mid(value, 1, 1))) && char.IsLetter(DMD.Chars.CChar(DMD.Strings.Mid(value, 2, 1))))
                {
                    for (int i = 3; i <= 9; i++)
                    {
                        if (!char.IsNumber(DMD.Chars.CChar(DMD.Strings.Mid(value, i, 1))))
                            return false;
                    }

                    return true;
                }
                else if (char.IsLetter(DMD.Chars.CChar(DMD.Strings.Mid(value, 8, 1))) && char.IsLetter(DMD.Chars.CChar(DMD.Strings.Mid(value, 9, 1))))
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        if (!char.IsNumber(DMD.Chars.CChar(DMD.Strings.Mid(value, i, 1))))
                            return false;
                    }

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Restituisce true se la stringa rappresenta un codice fiscale o una partita iva
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private bool IsCFOPIVA(string value)
            {
                string cf = Sistema.Formats.ParseCodiceFiscale(value);
                if (DMD.Strings.Len(cf) >= 16)
                    return true;
                string piva = Sistema.Formats.ParsePartitaIVA(value);
                if (DMD.Strings.Len(piva) >= 11)
                    return true;
                return false;
            }

            /// <summary>
            /// Restituisce una collezione di oggetto CPersonaInfo contenete le sole persone fisiche corrispondenti ai parametri di ricerca specificati
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Anagrafica.CPersonaInfo> FindPF(Anagrafica.CRMFindParams filter)
            {
                return FindPF(filter, null);
            }

            /// <summary>
            /// Cerca una persona fisica
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            /// <returns></returns>
            private CCollection<Anagrafica.CPersonaInfo> FindPF(Anagrafica.CRMFindParams filter, int[] findIn)
            {
                const string CMDAZIENDA = "azienda:";
                const string CMDENTE = "ente:";
                const string CMDTELEFONO = "telefono:";
                const string CMDINDIRIZZO = "indirizzo:";
                const string CMDNOME = "nome:";
                const string CMDPROVINCIA = "provincia:";
                const string CMDCOMUNE = "comune:";
                const string CMDEMAIL = "e-mail:";
                const string CDCF = "codice fiscale:";
                const string PIVA = "partita iva:";
                const string CDTR = "tipo rapporto:";
                const string CDCOMPLEANNO = "compleanno:";
                const string CMDNUMERODOCUMENTO = "numero documento:";
                const string CMDPO = "punto operativo:";
                var col = new CCollection<Anagrafica.CPersonaInfo>();
                string name, cf, num; // name1, 
                string param = DMD.Strings.Trim(DMD.Strings.Replace(filter.Text, "  ", " "));
                name = param;
                cf = Sistema.Formats.ParseCodiceFiscale(name);
                num = Sistema.Formats.ParsePhoneNumber(name);

                // name = LCase(Replace(name, "'", "''"))
                // name1 = DMD.Strings.OnlyChars(name)
                // cf = Replace(cf, "'", "''")
                // num = Replace(num, "'", "''")
                filter = (Anagrafica.CRMFindParams)filter.Clone();
                filter.tipoPersona = Anagrafica.TipoPersona.PERSONA_FISICA;
                col.Sorted = false;
                switch (DMD.Strings.LCase(DMD.Strings.Trim(filter.Tipo)) ?? "")
                {
                    case "id":
                        {
                            Find_ByID(filter.Text, col, filter);
                            break;
                        }

                    case "azienda":
                        {
                            Find_ByAzienda(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "ente":
                        {
                            Find_ByEnte(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "categoria":
                        {
                            Find_ByCategoria(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "tipologia":
                        {
                            Find_ByTipologia(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "telefono":
                        {
                            Find_ByTelefono(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "indirizzo":
                        {
                            Find_ByIndirizzo(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "nome":
                        {
                            Find_ByNome(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "provincia":
                        {
                            Find_ByProvincia(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "comune":
                        {
                            Find_ByComune(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "e-mail":
                        {
                            Find_ByEMail(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "codice fiscale":
                        {
                            Find_ByCFOrPIVA(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "partita iva":
                        {
                            Find_ByPIVA(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "tipo rapporto":
                        {
                            Find_ByTipoRapporto(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "compleanno":
                        {
                            Find_ByCompleanno(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "data ricontatto":
                        {
                            Find_ByDataRicontatto(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "lista":
                        {
                            Find_ByLista(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "numero documento":
                        {
                            Find_ByNumeroDocumento(filter.Text, filter, col, findIn);
                            break;
                        }

                    case "punto operativo":
                        {
                            Find_ByPO(filter.Text, filter, col, findIn);
                            break;
                        }

                    default:
                        {
                            if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDAZIENDA)) ?? "") == CMDAZIENDA)
                            {
                                Find_ByAzienda(DMD.Strings.Mid(param, DMD.Strings.Len(CMDAZIENDA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDENTE)) ?? "") == CMDENTE)
                            {
                                Find_ByEnte(DMD.Strings.Mid(param, DMD.Strings.Len(CMDAZIENDA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDINDIRIZZO)) ?? "") == CMDINDIRIZZO)
                            {
                                Find_ByIndirizzo(DMD.Strings.Mid(param, DMD.Strings.Len(CMDINDIRIZZO) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDTELEFONO)) ?? "") == CMDTELEFONO)
                            {
                                Find_ByTelefono(DMD.Strings.Mid(param, DMD.Strings.Len(CMDTELEFONO) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDNOME)) ?? "") == CMDNOME)
                            {
                                Find_ByNome(DMD.Strings.Mid(param, DMD.Strings.Len(CMDNOME) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDPROVINCIA)) ?? "") == CMDPROVINCIA)
                            {
                                Find_ByProvincia(DMD.Strings.Mid(param, DMD.Strings.Len(CMDPROVINCIA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDCOMUNE)) ?? "") == CMDCOMUNE)
                            {
                                Find_ByComune(DMD.Strings.Mid(param, DMD.Strings.Len(CMDCOMUNE) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDEMAIL)) ?? "") == CMDEMAIL)
                            {
                                Find_ByEMail(DMD.Strings.Mid(param, DMD.Strings.Len(CMDEMAIL) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDCF)) ?? "") == CDCF)
                            {
                                Find_ByCFOrPIVA(DMD.Strings.Mid(param, DMD.Strings.Len(CDCF) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(PIVA)) ?? "") == PIVA)
                            {
                                Find_ByPIVA(DMD.Strings.Mid(param, DMD.Strings.Len(PIVA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDTR)) ?? "") == CDTR)
                            {
                                Find_ByTipoRapporto(DMD.Strings.Mid(param, DMD.Strings.Len(CDTR) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDCOMPLEANNO)) ?? "") == CDCOMPLEANNO)
                            {
                                Find_ByCompleanno(DMD.Strings.Mid(param, DMD.Strings.Len(CDCOMPLEANNO) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDNUMERODOCUMENTO)) ?? "") == CMDNUMERODOCUMENTO)
                            {
                                Find_ByNumeroDocumento(DMD.Strings.Mid(param, DMD.Strings.Len(CMDNUMERODOCUMENTO) + 1), filter, col, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDPO)) ?? "") == CMDPO)
                            {
                                Find_ByPO(DMD.Strings.Mid(param, DMD.Strings.Len(CMDPO) + 1), filter, col, findIn);
                            }
                            // If (col.Count = 0) And (cf <> "") Then
                            // Find_ByCFOrPIVA(cf, col, TipoPersona.PERSONA_FISICA, nMax)
                            // End If
                            // If (col.Count = 0) And ((Left(name, 1) >= "a" And Left(name, 1) <= "z")) Then
                            // Find_ByNome(name, col, ignoreRights, TipoPersona.PERSONA_FISICA, nMax, intelliSearch)
                            // End If
                            // If (col.Count = 0) And (num <> "") Then
                            // Find_ByTelefono(num, col, ignoreRights, TipoPersona.PERSONA_FISICA, nMax)
                            // End If
                            // If (col.Count = 0) And (param <> "") Then
                            // Find_ByEMail(param, col, ignoreRights, TipoPersona.PERSONA_FISICA, nMax)
                            // End If
                            else if (IsEmail(name))
                            {
                                Find_ByEMail(name, col, filter, findIn);
                            }
                            else if (IsWebSite(name))
                            {
                                Find_ByWebSite(name, col, filter, findIn);
                            }
                            else if (IsCFOPIVA(name))
                            {
                                Find_ByCFOrPIVA(name, col, filter, findIn);
                                if (DMD.Strings.Len(num) > 3)
                                    Find_ByTelefono(num, col, filter, findIn);
                            }
                            else if (IsNumeroDocumento(name))
                            {
                                Find_ByNumeroDocumento(name, filter, col, findIn);
                            }
                            else if (IsPhoneNumber(name))
                            {
                                Find_ByTelefono(num, col, filter, findIn);
                            }
                            else
                            {
                                Find_ByNome(name, col, filter, findIn);
                            }

                            break;
                        }
                }
                // col.Sort()
                return col;
            }

            /// <summary>
            /// Restituisce una collezione di oggetto CPersonaInfo contenete le sole aziende corrispondenti ai parametri di ricerca specificati
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Anagrafica.CPersonaInfo> FindAZ(Anagrafica.CRMFindParams filter)
            {
                return FindAZ(filter, null);
            }

            /// <summary>
            /// Cerca un'azienda
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="findIn"></param>
            /// <returns></returns>
            private CCollection<Anagrafica.CPersonaInfo> FindAZ(Anagrafica.CRMFindParams filter, int[] findIn)
            {
                const string CMDAZIENDA = "azienda:";
                const string CMDENTE = "ente:";
                const string CMDTELEFONO = "telefono:";
                const string CMDINDIRIZZO = "indirizzo:";
                const string CMDNOME = "nome:";
                const string CMDPROVINCIA = "provincia:";
                const string CMDCOMUNE = "comune:";
                const string CMDEMAIL = "e-mail:";
                const string CDCF = "codice fiscale:";
                const string PIVA = "partita iva:";
                const string CDTR = "tipo rapporto:";
                const string CDCOMPLEANNO = "compleanno:";
                const string CMDNUMERODOCUMENTO = "numero documento:";
                const string CMDPO = "punto operativo:";
                var col = new CCollection<Anagrafica.CPersonaInfo>();
                string name, cf, num;
                string param = DMD.Strings.Trim(DMD.Strings.Replace(filter.Text, "  ", " "));
                name = param;
                cf = Sistema.Formats.ParseCodiceFiscale(name);
                num = Sistema.Formats.ParsePhoneNumber(name);
                filter = (Anagrafica.CRMFindParams)filter.Clone();
                filter.tipoPersona = Anagrafica.TipoPersona.PERSONA_GIURIDICA;
                // name = LCase(Replace(name, "'", "''"))
                // name1 = DMD.Strings.OnlyChars(name)
                // cf = Replace(cf, "'", "''")
                // num = Replace(num, "'", "''")

                col.Sorted = false;
                switch (DMD.Strings.LCase(DMD.Strings.Trim(filter.Tipo)) ?? "")
                {
                    case "id":
                        {
                            Find_ByID(filter.Text, col, filter);
                            break;
                        }

                    case "azienda":
                        {
                            Find_ByAzienda(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "ente":
                        {
                            Find_ByEnte(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "categoria":
                        {
                            Find_ByCategoria(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "tipologia":
                        {
                            Find_ByTipologia(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "telefono":
                        {
                            Find_ByTelefono(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "indirizzo":
                        {
                            Find_ByIndirizzo(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "nome":
                        {
                            Find_ByNome(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "provincia":
                        {
                            Find_ByProvincia(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "comune":
                        {
                            Find_ByComune(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "e-mail":
                        {
                            Find_ByEMail(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "codice fiscale":
                        {
                            Find_ByCFOrPIVA(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "partita iva":
                        {
                            Find_ByPIVA(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "tipo rapporto":
                        {
                            Find_ByTipoRapporto(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "compleanno":
                        {
                            Find_ByCompleanno(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "data ricontatto":
                        {
                            Find_ByDataRicontatto(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "lista":
                        {
                            Find_ByLista(filter.Text, col, filter, findIn);
                            break;
                        }

                    case "numero documento":
                        {
                            Find_ByNumeroDocumento(filter.Text, filter, col, findIn);
                            break;
                        }

                    case "punto operativo":
                        {
                            Find_ByPO(filter.Text, filter, col, findIn);
                            break;
                        }

                    default:
                        {
                            if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDAZIENDA)) ?? "") == CMDAZIENDA)
                            {
                                Find_ByNome(DMD.Strings.Mid(param, DMD.Strings.Len(CMDAZIENDA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDENTE)) ?? "") == CMDENTE)
                            {
                                Find_ByNome(DMD.Strings.Mid(param, DMD.Strings.Len(CMDENTE) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDINDIRIZZO)) ?? "") == CMDINDIRIZZO)
                            {
                                Find_ByIndirizzo(DMD.Strings.Mid(param, DMD.Strings.Len(CMDINDIRIZZO) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDTELEFONO)) ?? "") == CMDTELEFONO)
                            {
                                Find_ByTelefono(DMD.Strings.Mid(param, DMD.Strings.Len(CMDTELEFONO) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDNOME)) ?? "") == CMDNOME)
                            {
                                Find_ByNome(DMD.Strings.Mid(param, DMD.Strings.Len(CMDNOME) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDPROVINCIA)) ?? "") == CMDPROVINCIA)
                            {
                                Find_ByProvincia(DMD.Strings.Mid(param, DMD.Strings.Len(CMDPROVINCIA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDCOMUNE)) ?? "") == CMDCOMUNE)
                            {
                                Find_ByComune(DMD.Strings.Mid(param, DMD.Strings.Len(CMDCOMUNE) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDEMAIL)) ?? "") == CMDEMAIL)
                            {
                                Find_ByEMail(DMD.Strings.Mid(param, DMD.Strings.Len(CMDEMAIL) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDCF)) ?? "") == CDCF)
                            {
                                Find_ByCFOrPIVA(DMD.Strings.Mid(param, DMD.Strings.Len(CDCF) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(PIVA)) ?? "") == PIVA)
                            {
                                Find_ByPIVA(DMD.Strings.Mid(param, DMD.Strings.Len(PIVA) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDTR)) ?? "") == CDTR)
                            {
                                Find_ByTipoRapporto(DMD.Strings.Mid(param, DMD.Strings.Len(CDTR) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CDCOMPLEANNO)) ?? "") == CDCOMPLEANNO)
                            {
                                Find_ByCompleanno(DMD.Strings.Mid(param, DMD.Strings.Len(CDCOMPLEANNO) + 1), col, filter, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDNUMERODOCUMENTO)) ?? "") == CMDNUMERODOCUMENTO)
                            {
                                Find_ByNumeroDocumento(DMD.Strings.Mid(param, DMD.Strings.Len(CMDNUMERODOCUMENTO) + 1), filter, col, findIn);
                            }
                            else if ((DMD.Strings.Left(DMD.Strings.LCase(param), DMD.Strings.Len(CMDPO)) ?? "") == CMDPO)
                            {
                                Find_ByPO(DMD.Strings.Mid(param, DMD.Strings.Len(CMDPO) + 1), filter, col, findIn);
                            }
                            // If (col.Count = 0) And (cf <> "") Then
                            // Find_ByCFOrPIVA(cf, col, TipoPersona.PERSONA_GIURIDICA, nMax)
                            // End If
                            // If (col.Count = 0) And ((Left(name, 1) >= "a" And Left(name, 1) <= "z")) Then
                            // Find_ByNome(name, col, ignoreRights, TipoPersona.PERSONA_GIURIDICA, nMax, intelliSearch)
                            // End If
                            // If (col.Count = 0) And (num <> "") Then
                            // Find_ByTelefono(num, col, ignoreRights, TipoPersona.PERSONA_GIURIDICA, nMax)
                            // End If
                            // If (col.Count = 0) And (param <> "") Then
                            // Find_ByEMail(param, col, ignoreRights, TipoPersona.PERSONA_GIURIDICA, nMax)
                            // End If
                            else if (IsEmail(name))
                            {
                                Find_ByEMail(name, col, filter, findIn);
                            }
                            else if (IsWebSite(name))
                            {
                                Find_ByWebSite(name, col, filter, findIn);
                            }
                            else if (IsCFOPIVA(name))
                            {
                                Find_ByCFOrPIVA(name, col, filter, findIn);
                                if (DMD.Strings.Len(num) > 3)
                                    Find_ByTelefono(num, col, filter, findIn);
                            }
                            else if (IsNumeroDocumento(name))
                            {
                                Find_ByNumeroDocumento(name, filter, col, findIn);
                            }
                            else if (IsPhoneNumber(name))
                            {
                                Find_ByTelefono(num, col, filter, findIn);
                            }
                            else
                            {
                                Find_ByNome(name, col, filter, findIn);
                            }

                            break;
                        }
                }
                // col.Sort()
                return col;
            }



            /// <summary>
            /// Cerca la persona
            /// </summary>
            /// <param name="nominativo"></param>
            /// <param name="dataNascita"></param>
            /// <returns></returns>
            public Anagrafica.CPersonaFisica GetPersonaByParams(string nominativo, DateTime? dataNascita)
            {
                    nominativo = DMD.Strings.Trim(nominativo);
                    if (string.IsNullOrEmpty(nominativo) && dataNascita.HasValue == false)
                        return null;

                using (var cursor = new Anagrafica.CPersonaFisicaCursor())
                {  
                    if (!string.IsNullOrEmpty(nominativo))
                        cursor.Nominativo.Value = nominativo;
                    if (dataNascita.HasValue)
                        cursor.DataNascita.Value = dataNascita.Value;
                    if (cursor.Count() == 1)
                    {
                        return cursor.Item;
                    }
                    else
                    {
                        return null;
                    }
                }
                 
            }

            /// <summary>
            /// Istanzia un oggetto persona di tipo specifico
            /// </summary>
            /// <param name="tipoPersona"></param>
            /// <returns></returns>
            public Anagrafica.CPersona Instantiate(Anagrafica.TipoPersona tipoPersona)
            {
                switch (tipoPersona)
                {
                    case Anagrafica.TipoPersona.PERSONA_FISICA:
                        {
                            return new Anagrafica.CPersonaFisica();
                        }

                    default:
                        {
                            return new Anagrafica.CAzienda();
                        }
                }
            }

            /// <summary>
            /// Crea una persona fisica
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="cognome"></param>
            /// <returns></returns>
            public Anagrafica.CPersona CreatePersonaFisica(string nome, string cognome)
            {
                var ret = new Anagrafica.CPersonaFisica();
                ret.Nome = nome;
                ret.Cognome = cognome;
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.Save();
                return ret;
            }

            // Public Function CreateAzienda(ByVal ragioneSociale As String) As CAzienda
            // Dim ret As New CAzienda
            // ret.RagioneSociale = ragioneSociale
            // ret.Stato = ObjectStatus.OBJECT_VALID
            // APPConn.Save(ret)
            // Return ret
            // End Function

            /// <summary>
            /// Restituisce l'elenco delle persone che compiono gli anni
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Anagrafica.CPersonaFisica> GetCompleanniDi(DateTime data)
            {
                var ret = new CCollection<Anagrafica.CPersonaFisica>();
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataMorte.Value = default;
                    cursor.DataNascita.Value = default;
                    cursor.DataNascita.Operator = OP.OP_NE;
                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    int m = DMD.DateUtils.Month(data);
                    int d = DMD.DateUtils.Day(data);

                    var f =  (DBCursorField.Field<DateTime>("DataNascita").Month().EQ(m))
                           * (DBCursorField.Field<DateTime>("DataNascita").Day().EQ(d));
                    
                    cursor.WhereClauses *= f;

                    while (cursor.Read())
                    {
                        ret.Add((Anagrafica.CPersonaFisica)cursor.Item); 
                    }

                    return ret;
                }
                 
            }

            /// <summary>
            /// Restituisce la prima persona che corrisponde al codice fiscale
            /// </summary>
            /// <param name="cf"></param>
            /// <returns></returns>
            public Anagrafica.CPersona GetPersonaByCF(string cf)
            {
                cf = Sistema.Formats.ParseCodiceFiscale(cf);
                if (string.IsNullOrEmpty(cf))
                    return null;
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.CodiceFiscale.Value = cf;
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Nominativo.SortOrder = SortEnum.SORT_ASC;
                    return cursor.Item;
                }
                
            }

            /// <summary>
            /// Cerca tutte le persone che corrispondono al codice fiscale
            /// </summary>
            /// <param name="cf"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersona> FindPersoneByCF(string cf)
            {
                var ret = new CCollection<Anagrafica.CPersona>();
                cf = Sistema.Formats.ParseCodiceFiscale(cf);
                if (string.IsNullOrEmpty(cf))
                    return ret;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.CodiceFiscale.Value = Sistema.Formats.ParseCodiceFiscale(cf);
                    cursor.Nominativo.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                    return ret;
                }
                 
            }

            /// <summary>
            /// Restituisce la prima persona che corrisponde alla partita iva
            /// </summary>
            /// <param name="cf"></param>
            /// <returns></returns>
            public Anagrafica.CPersona FindPersonaByPIVA(string cf)
            {
                cf = Sistema.Formats.ParsePartitaIVA(cf);
                if (string.IsNullOrEmpty(cf))
                        return null;
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.PartitaIVA.Value = cf;
                    return cursor.Item;
                }
 
            }

            /// <summary>
            /// Riassegna le persone di una provinca ad un ufficio
            /// </summary>
            /// <param name="nomeProvincia"></param>
            /// <param name="po"></param>
            /// <param name="force"></param>
            /// <returns></returns>
            public int AssegnaResidentiPerProvincia
                                (
                                string nomeProvincia, 
                                CUfficio po, 
                                bool force = false
                                )
            {
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ResidenteA_Provincia.Value = DMD.Strings.Trim(nomeProvincia);
                    if (!force)
                    {
                        cursor.IDPuntoOperativo.Value = 0;
                        cursor.IDPuntoOperativo.IncludeNulls = true;
                    }

                    int ret = (int) cursor.Count();
                    while (cursor.Read())
                    {
                        cursor.Item.PuntoOperativo = po;
                        cursor.Item.Save();
                    }

                    return ret;
                }
                 
            }

            /// <summary>
            /// Riassegna le persone di un comune ad un ufficio
            /// </summary>
            /// <param name="nomeComune"></param>
            /// <param name="nomeProvincia"></param>
            /// <param name="po"></param>
            /// <param name="force"></param>
            /// <returns></returns>
            public int AssegnaResidentiPerComune(
                                string nomeComune, 
                                string nomeProvincia, 
                                CUfficio po, 
                                bool force = false
                                )
            {
                int ret = -1;
                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ResidenteA_Citta.Value = DMD.Strings.Trim(nomeComune);
                    cursor.ResidenteA_Provincia.Value = DMD.Strings.Trim(nomeProvincia);
                    if (!force)
                    {
                        cursor.IDPuntoOperativo.Value = 0;
                        cursor.IDPuntoOperativo.IncludeNulls = true;
                    }

                    ret = (int)cursor.Count();
                    while (cursor.Read())
                    {
                        cursor.Item.PuntoOperativo = po;
                        cursor.Item.Save();
                    }

                    return ret;
                }
                 
            }

            /// <summary>
            /// Restituisce la persona fisica in base all'ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public Anagrafica.CPersonaFisica GetPersonaById(int id)
            {
                if (id == 0)
                    return null;

                

                using (var cursor = new Anagrafica.CPersonaFisicaCursor())
                {
                    cursor.ID.Value = id;
                    return cursor.Item;
                }
                 
            }

            /// <summary>
            /// Cerca le persone in base al numero di documento d'identità
            /// </summary>
            /// <param name="numero"></param>
            /// <param name="filter"></param>
            /// <param name="col"></param>
            /// <param name="findIn"></param>
            private void Find_ByNumeroDocumento
                        (
                        string numero, 
                        Anagrafica.CRMFindParams filter, 
                        CCollection<Anagrafica.CPersonaInfo> col, 
                        int[] findIn
                        )
            {
                if (col is null)
                    throw new ArgumentNullException("col");
                if (filter is null)
                    throw new ArgumentNullException("filter");
                numero = DMD.Strings.Replace(numero, " ", "");
                if (string.IsNullOrEmpty(numero))
                    return;

                var list = new List<int>();
                using(var cursor = new CAttachmentsCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.OwnerType.ValueIn(new string[] { "CPersonaFisica", "CAzienda" });
                    cursor.Parametro.Value = numero;
                    while (cursor.Read())
                    {
                        list.Add(cursor.Item.IDOwner);
                    }
                }

                list.Sort();
                int[] arr = list.ToArray();
                arr = DMD.Arrays.Join(arr, findIn);

                if (arr.Length == 0)
                    return;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;                    
                    cursor.ID.ValueIn(arr);
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item);
                        cnt += 1;
                    }
                     
                }
                 
            }

            /// <summary>
            /// Cerca le persone in base al punto operativo di assegnazione
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="filter"></param>
            /// <param name="col"></param>
            /// <param name="findIn"></param>
            private void Find_ByPO(
                            string nome, 
                            Anagrafica.CRMFindParams filter, 
                            CCollection<Anagrafica.CPersonaInfo> col, 
                            int[] findIn
                            )
            {
                if (col is null)
                    throw new ArgumentNullException("col");
                if (filter is null)
                    throw new ArgumentNullException("filter");
                nome = DMD.Strings.LCase(DMD.Strings.Trim(nome));
                nome = DMD.Strings.Replace(nome, "  ", " ");
                if (string.IsNullOrEmpty(nome))
                    return;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (findIn is object)
                        cursor.ID.ValueIn(findIn);
                    cursor.NomePuntoOperativo.Value = nome;
                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        AddPersonInfo(col, cursor.Item); 
                        cnt += 1;
                    }
                     
                }
                 
            }

            ///// <summary>
            ///// Restituisce una stringa 
            ///// </summary>
            ///// <param name="findIn"></param>
            ///// <param name="sep"></param>
            ///// <returns></returns>
            //private string JoinID(int[] findIn, string sep)
            //{
            //    var ret = new System.Text.StringBuilder();
            //    int l = DMD.Arrays.Len(findIn);
            //    for (int i = 0, loopTo = l - 1; i <= loopTo; i++)
            //    {
            //        if (i > 0)
            //            ret.Append(",");
            //        ret.Append(DBUtils.DBNumber(findIn[i]));
            //    }

            //    return ret.ToString();
            //}

            /// <summary>
            /// Cerca le persone in base al recapito telefonico
            /// </summary>
            /// <param name="numero"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersona> FindPersoneByNumero(
                                string numero
                                )
            {
                var persone = new CCollection<Anagrafica.CPersona>();
               
                numero = Sistema.Formats.ParsePhoneNumber(numero);
                if (DMD.Strings.Len(numero) <= 3)
                    return persone;
                
                string[] numeri;
                if (DMD.Strings.Left(numero, 1) == "+")
                {
                    numero = "00" + DMD.Strings.Mid(numero, 2);
                }

                if (DMD.Strings.Left(numero, 4) == "0039" && DMD.Strings.Len(numero) > 5)
                {
                    numeri = new[] { numero, DMD.Strings.Mid(numero, 5) };
                }
                else
                {
                    numeri = new[] { numero };
                }

                var list = new List<int>();
                using (var cursor = new Anagrafica.CContattoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Tipo.ValueIn(new[] { "Cellulare", "Telefono", "Fax" });
                    cursor.Valore.ValueIn(numeri);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        list.Add(cursor.Item.PersonaID);
                    }
                }

                if (list.Count == 0) return persone;
                list.Sort();
                var arr = list.ToArray();


                using (var cursor = new Anagrafica.CPersonaCursor())
                {
                    cursor.ID.ValueIn(arr);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    //cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        persone.Add(cursor.Item);
                    }

                }

                return CheckIsSingleNumber(numero, persone);
            }

            /// <summary>
            /// Restituisce true 
            /// </summary>
            /// <param name="number"></param>
            /// <param name="persone"></param>
            /// <returns></returns>
            private CCollection<Anagrafica.CPersona> CheckIsSingleNumber(
                                string number, 
                                CCollection<Anagrafica.CPersona> persone
                                )
            {
                var validate = new CCollection<Anagrafica.CPersona>();
                int i = 0;
                while (i < persone.Count)
                {
                    var p = persone[i];
                    bool f = false;
                    foreach (Anagrafica.CContatto r in p.Recapiti)
                    {
                        if (r.Stato == ObjectStatus.OBJECT_VALID && ((r.Valore ?? "") == (number ?? "") || ("0039" + r.Valore ?? "") == (number ?? "")))
                        {
                            if (r.Validated.HasValue)
                            {
                                if (r.Validated.Value == true)
                                {
                                    validate.Add(p);
                                    break;
                                }
                                else
                                {
                                    persone.RemoveAt(i);
                                    f = true;
                                }
                            }
                        }
                    }

                    if (!f)
                        i += 1;
                }

                if (validate.Count > 0)
                {
                    return validate;
                }
                else
                {
                    return persone;
                }
            }
        }
    }

    public partial class Anagrafica
    {
        private static CPersoneClass m_Persone = null;

        /// <summary>
        /// Repository delle persone
        /// </summary>
        public static CPersoneClass Persone
        {
            get
            {
                if (m_Persone is null)
                    m_Persone = new CPersoneClass();
                return m_Persone;
            }
        }
    }
}