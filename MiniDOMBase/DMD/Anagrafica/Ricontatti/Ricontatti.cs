using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Repository degli appuntamenti
        /// </summary>
        [Serializable]
        public sealed class CRicontattiClass 
            : CModulesClass<Anagrafica.CRicontatto>
        {
 
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRicontattiClass()
                : base("modRicontatti", typeof(Anagrafica.CRicontattiCursor))
            {
            }

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
            /// Gestisce l'evento PersonaMerged
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

                    // Tabella tbl_Ricontatti 
                    using(var cursor = new CRicontattiCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        cursor.IDPersona.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Ricontatti";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            rec.FieldName = "IDPersona";
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.Persona = mi.Persona1;
                            cursor.Item.Save();
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
                    var items = mi.GetAffectedRecorsIDs("tbl_Ricontatti", "IDPersona");
                    using (var cursor = new CRicontattiCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Persona = mi.Persona2;
                            cursor.Item.Save();
                        }
                    }
                }
            }



            /// <summary>
            /// Gestisce l'evento PersonaChanged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaModified(PersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var p = e.Persona;
                    // If (p.PuntoOperativo IsNot Nothing) Then
                    using(var cursor = new CRicontattiCursor())
                    {
                        cursor.IDPersona.Value = DBUtils.GetID(p, 0);
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.StatoRicontatto.ValueIn(new StatoRicontatto[] { StatoRicontatto.PROGRAMMATO, StatoRicontatto.RIMANDATO });
                        while (cursor.Read())
                        {
                            cursor.Item.PuntoOperativo = p.PuntoOperativo;
                            cursor.Item.Save();
                        }

                    }
                }
            }


            /// <summary>
            /// Formatta lo stato del ricontatto
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string FormatStatoRicontatto(Anagrafica.StatoRicontatto value)
            {
                string ret;
                switch (value)
                {
                    case Anagrafica.StatoRicontatto.NONPROGRAMMATO:
                        {
                            ret = "NON PROGRAMMATO";
                            break;
                        }

                    case Anagrafica.StatoRicontatto.PROGRAMMATO:
                        {
                            ret = "PROGRAMMATO";
                            break;
                        }

                    case Anagrafica.StatoRicontatto.EFFETTUATO:
                        {
                            ret = "EFFETTUATO";
                            break;
                        }

                    case Anagrafica.StatoRicontatto.RIMANDATO:
                        {
                            ret = "RIMANDATO";
                            break;
                        }

                    case Anagrafica.StatoRicontatto.ANNULLATO:
                        {
                            ret = "ANNULLATO";
                            break;
                        }

                    default:
                        {
                            ret = "";
                            break;
                        }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce il prossimo ricontatto per la persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetProssimoRicontatto(Anagrafica.CPersona persona) // , Optional ByVal nomeLista As String = vbNullString
            {
                return GetProssimoRicontatto(DBUtils.GetID(persona, 0)); // nomeLista
            }

            /// <summary>
            /// Restituisce il prossimo ricontatto a partire da adesso
            /// </summary>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CRicontatto GetProssimoRicontatto(int idPersona) // , Optional ByVal nomeLista As String = vbNullString
            {
                if (idPersona == 0)
                    return null;
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPersona.Value = idPersona;
                    cursor.DataPrevista.Value = DMD.DateUtils.Now();
                    cursor.DataPrevista.Operator = OP.OP_GE;
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC;
                    cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce l'ultimo ricontatto
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetUltimoRicontatto(Anagrafica.CPersona persona) // , Optional ByVal nomeLista As String = vbNullString
            {
                return GetUltimoRicontatto(DBUtils.GetID(persona, 0)); // , nomeLista
            }

            /// <summary>
            /// Restituisce l'ultimo ricontatto
            /// </summary>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetUltimoRicontatto(int idPersona) // , Optional ByVal nomeLista As String = vbNullString
            {
                if (idPersona == 0)
                    return null;
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPersona.Value = idPersona;
                    cursor.DataPrevista.Value = DMD.DateUtils.Now();
                    cursor.DataPrevista.Operator = OP.OP_LT;
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC;
                    cursor.StatoRicontatto.Value = StatoRicontatto.EFFETTUATO;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce il ricontatto programmato dalla sorgente
            /// </summary>
            /// <param name="source"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetRicontattoBySource(object source)
            {
                return GetRicontattoBySource(
                            DMD.RunTime.vbTypeName(source), 
                            DBUtils.GetID((Databases.IDBObjectBase)source).ToString()
                            );
            }

            /// <summary>
            /// Restituisce il ricontatto programmato dalla sorgente
            /// </summary>
            /// <param name="sourceName"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetRicontattoBySource(string sourceName, string param)
            {
                sourceName = DMD.Strings.Trim(sourceName);
                param = DMD.Strings.Trim(param);
                
                if (string.IsNullOrEmpty(sourceName) && string.IsNullOrEmpty(param))
                    return null;

                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.SourceName.Value = sourceName;
                    cursor.SourceParam.Value = param;
                    cursor.ID.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Programma un ricontatto
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="data"></param>
            /// <param name="motivo"></param>
            /// <param name="sourceName"></param>
            /// <param name="sourceParam"></param>
            /// <param name="nomeLista"></param>
            /// <param name="puntoOperativo"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto ProgrammaRicontatto(
                                Anagrafica.CPersona persona, 
                                DateTime data, 
                                string motivo, 
                                string sourceName, 
                                string sourceParam, 
                                string nomeLista, 
                                CUfficio puntoOperativo, 
                                Sistema.CUser operatore
                                )
            {
                if (persona is null)
                    return null;
                Anagrafica.CRicontatto item;
                nomeLista = DMD.Strings.Trim(nomeLista);

                if (string.IsNullOrEmpty(nomeLista))
                {
                    item = new Anagrafica.CRicontatto();
                }
                else
                {
                    item = new Anagrafica.ListaRicontattoItem();
                    (item as ListaRicontattoItem).NomeLista = nomeLista;
                }

                item.DataPrevista = data;
                // item.PuntoOperativo = persona.PuntoOperativo
                item.Note = motivo;
                item.Persona = persona;
                item.SourceName = sourceName;
                item.SourceParam = sourceParam;
                item.Stato = ObjectStatus.OBJECT_VALID;
                item.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO;
                item.GiornataIntera = false;
                item.PuntoOperativo = puntoOperativo;
                item.AssegnatoA = operatore;
                // item.Produttore = Anagrafica.Aziende.AziendaPrincipale
                item.Save();
                return item;
            }

            /// <summary>
            /// Annulla il ricontatto
            /// </summary>
            /// <param name="src"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto AnnullaRicontattoBySource(object src)
            {
                return AnnullaRicontattoBySource(DMD.RunTime.vbTypeName(src), DBUtils.GetID(src, 0).ToString());
            }

            /// <summary>
            /// Annulla il ricontatto
            /// </summary>
            /// <param name="source"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto AnnullaRicontattoBySource(string source, string param)
            {
                var ric = GetRicontattoBySource(source, param);
                if (ric is object)
                {
                    ric.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                    ric.Save();
                }

                return ric;
            }

            /// <summary>
            /// Conta i ricontatti previsti per il filtro
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public int ContaRicontattiPrevistiPerData(Anagrafica.CRMFilter filter)
            {
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.DataFine.HasValue)
                        cursor.DataRicontatto.Value = filter.DataFine;
                    cursor.DataRicontatto.Operator = OP.OP_LE;
                    cursor.StatoRicontatto.Value = Anagrafica.StatoRicontatto.PROGRAMMATO;
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.IDOperatore != 0)
                        cursor.IDOperatore.Value = filter.IDOperatore;
                    return (int)cursor.Count();
                }
            }

            /// <summary>
            /// Restituisce le persone attive secondo i parametri del filtro e del cursore
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="cursor"></param>
            /// <returns></returns>
            public CCollection<Sistema.CActivePerson> GetActivePersons(
                            Anagrafica.CRMFilter filter, 
                            Anagrafica.CPersonaCursor cursor
                            )
            {
                var lst = new List<int>();
                var ricontatti = new Dictionary<string, CRicontatto>();

                using (var riccursor = (!string.IsNullOrEmpty(filter.NomeLista))? (CRicontattiCursor)( new ListaRicontattoItemCursor()) : new CRicontattiCursor())
                {
                    riccursor.IgnoreRights = true;
                    if (!string.IsNullOrEmpty(filter.NomeLista)) 
                        ((ListaRicontattoItemCursor)riccursor).NomeLista.Value = filter.NomeLista;
                    riccursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    riccursor.StatoRicontatto.ValueIn(new StatoRicontatto[] { StatoRicontatto.PROGRAMMATO, StatoRicontatto.RIMANDATO });
                    if (filter.IDOperatore != 0)
                        riccursor.IDAssegnatoA.ValueIn(new int?[] { null, 0, filter.IDOperatore });
                    if (filter.IDPuntoOperativo != 0)
                        riccursor.IDPuntoOperativo.ValueIn(new int?[] { null, 0, filter.IDPuntoOperativo });
                    if (!string.IsNullOrEmpty(filter.Motivo))
                    {
                        riccursor.Note.Value = filter.Motivo;
                        riccursor.Note.Operator = OP.OP_LIKE;
                    }
                    if (filter.DataInizio.HasValue || filter.DataFine.HasValue)
                        riccursor.DataPrevista.Between(filter.DataInizio, filter.DataFine);

                    if (filter.Categorie.Count > 0)
                        riccursor.Categoria.ValueIn(filter.Categorie.ToArray());
                    
                    if (filter.TipiAppuntamento.Count > 0)
                        riccursor.TipoAppuntamento.ValueIn(filter.TipiAppuntamento.ToArray());
  
                    while (riccursor.Read())  
                    {
                        var ric = riccursor.Item;
                        ricontatti["K" + ric.IDPersona] = ric;
                        lst.Add(ric.IDPersona);
                    }

                }
                var ret = new CCollection<Sistema.CActivePerson>();
                if (lst.Count == 0)
                    return ret;

                
                //using(var cursor  = (filter.TipiRapporto.Count>0) ? (CPersonaCursor) new CPersonaFisicaCursor() : (CPersonaCursor)new CAziendeCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (!string.IsNullOrEmpty(filter.ResidenteA))
                    {
                        cursor.ResidenteA_Citta.Value = Anagrafica.Luoghi.GetComune(filter.ResidenteA);
                        cursor.ResidenteA_Provincia.Value = Anagrafica.Luoghi.GetProvincia(filter.ResidenteA);
                    }
                    if (DMD.RunTime.TestFlag(filter.Flags, Anagrafica.CRMFilterFlags.SOLOAZIENDE))
                        cursor.TipoPersona.Value = TipoPersona.PERSONA_GIURIDICA;
                    

                    if (filter.TipiRapporto.Count > 0)
                    {
                        ((CPersonaFisicaCursor)cursor).Impiego_TipoRapporto.ValueIn(filter.TipiRapporto.ToArray());
                    }

                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        var persona = cursor.Item;
                        var r = ricontatti["K" + DBUtils.GetID(persona, 0)];
                        var p = new Sistema.CActivePerson();
                        p.Data = r.DataPrevista;
                        p.Notes = r.Note;
                        p.Ricontatto = r;
                        p.GiornataIntera = r.GiornataIntera;
                        p.Promemoria = r.Promemoria;
                        p.Categoria = r.Categoria;
                        p.PersonID = DBUtils.GetID(persona, 0);
                        p.Person = persona;
                        p.IconURL = persona.IconURL;
                        
                        ret.Add(p);
                         

                        cnt += 1;
                    }
                }

                ret.Comparer = filter;
                ret.Sort();
                 
                return ret;
            }

            /// <summary>
            /// Conta gli appuntamenti previsit
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="cursor"></param>
            /// <returns></returns>
            public int ContaPreviste(Anagrafica.CRMFilter filter, Anagrafica.CPersonaCursor cursor)
            {
                return GetActivePersons(filter, cursor).Count;
            }

            /// <summary>
            /// Conta gli appuntamenti assegnati all'operatore per la data specificata
            /// </summary>
            /// <param name="op"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            public int ContaRicontattiPerData(Sistema.CUser op, DateTime data)
            {
                if (op is null)
                    throw new ArgumentNullException("op");
                return ContaRicontattiPerData(DBUtils.GetID(op, 0), data);
            }

            /// <summary>
            /// Conta gli appuntamenti assegnati all'operatore per la data specificata
            /// </summary>
            /// <param name="oID"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            public int ContaRicontattiPerData(int oID, DateTime data)
            {
                if (oID == 0)
                    return 0;
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    DateTime di = (DateTime)DMD.DateUtils.GetDatePart(data);
                    var df = DMD.DateUtils.DateAdd(DMD.DateTimeInterval.Second, 3600 * 24 - 1, di);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDAssegnatoA.Value = oID;
                    cursor.DataPrevista.Value = di;
                    cursor.DataPrevista.Value1 = df;
                    cursor.DataPrevista.Operator = OP.OP_BETWEEN;
                    cursor.IgnoreRights = true;
                    return (int)cursor.Count();
                }
                    
            }

            /// <summary>
            /// Restituisce il primo ricontatto programmato per la persona
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetRicontatto(Anagrafica.CPersona p) // , ByVal nomeLista As String
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                return GetRicontatto(DBUtils.GetID(p, 0));
            }

            /// <summary>
            /// Restituisce il primo ricontatto programmato per la persona
            /// </summary>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetRicontatto(int idPersona) // , ByVal nomeLista As String
            {
                Anagrafica.CRicontatto ret = null;
                using(var cursor = new Anagrafica.CRicontattiCursor())
                {
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    if (idPersona == 0)
                        return null;
                    
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoRicontatto.Value = Anagrafica.StatoRicontatto.PROGRAMMATO;
                    cursor.IDPersona.Value = idPersona;
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC;
                    // cursor.WhereClauses.Add(DMD.Strings.JoinW("([NFlags] And ", CStr(RicontattoFlags.Reserved), ") = 0"))
                    // cursor.Flags.Value = RicontattoFlags.Reserved
                    // cursor.Flags.Operator = 

                    ret = cursor.Item;
                    while (!cursor.EOF())
                    {
                        if (cursor.Item.IDAssegnatoA == DBUtils.GetID(Sistema.Users.CurrentUser, 0))
                        {
                            ret = cursor.Item;
                        }

                        cursor.MoveNext();
                    }
                }
                 
                return ret;
            }

            /// <summary>
            /// Restituisce gli appuntamenti programmati per la persona
            /// </summary>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CRicontatto> GetRicontatti(int idPersona) // , ByVal nomeLista As String
            {
                var ret = new CCollection<Anagrafica.CRicontatto>();
                if (idPersona == 0)
                    return ret;

                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoRicontatto.Value = Anagrafica.StatoRicontatto.PROGRAMMATO;
                    cursor.IDPersona.Value = idPersona;
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC;
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                }

                return ret;
            }

            /// <summary>
            /// Restituisce gli appuntamenti programmati per la persona e visibili all'operatore
            /// </summary>
            /// <param name="operatore"></param>
            /// <param name="p"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetRicontattoByOperatore(Sistema.CUser operatore, Anagrafica.CPersona p) // , ByVal nomeLista As String
            {
                if (operatore is null)
                    throw new ArgumentNullException("operatore");
                if (p is null)
                    throw new ArgumentNullException("p");
                if (DBUtils.GetID(p, 0) == 0)
                    return null;
                return GetRicontattoByOperatore(DBUtils.GetID(operatore, 0), DBUtils.GetID(p, 0)); // , nomeLista
            }

            /// <summary>
            /// Restituisce gli appuntamenti programmati per la persona e visibili all'operatore
            /// </summary>
            /// <param name="idoperatore"></param>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            public Anagrafica.CRicontatto GetRicontattoByOperatore(int idoperatore, int idPersona) // , ByVal nomeLista As String
            {
                if (idPersona == 0)
                    return null;
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoRicontatto.Value = Anagrafica.StatoRicontatto.PROGRAMMATO;
                    cursor.IDPersona.Value = idPersona;
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC;
                    cursor.IDAssegnatoA.Value = idoperatore;
                    return cursor.Item;
                }
            }

            //internal void doRicontattoCreated(Anagrafica.RicontattoEventArgs e)
            //{
            //    RicontattoCreated?.Invoke(this, e);
            //    Module.DispatchEvent(new Sistema.EventDescription("ricontatto_created", "Creato ricontatto per " + e.Ricontatto.NomePersona, e));
            //}

            //internal void doRicontattoDeleted(Anagrafica.RicontattoEventArgs e)
            //{
            //    RicontattoDeleted?.Invoke(this, e);
            //    Module.DispatchEvent(new Sistema.EventDescription("ricontatto_deleted", "Eliminato ricontatto per " + e.Ricontatto.NomePersona, e));
            //}

            //internal void doRicontattoModified(Anagrafica.RicontattoEventArgs e)
            //{
            //    RicontattoModified?.Invoke(this, e);
            //    Module.DispatchEvent(new Sistema.EventDescription("ricontatto_modified", "Modificato ricontatto per " + e.Ricontatto.NomePersona, e));
            //}

            /// <summary>
            /// Restituisce i ricontatti modificati
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CRicontatto> GetRicontattiModificati(Anagrafica.CRMFilter filter)
            {
                var ret = new CCollection<Anagrafica.CRicontatto>();
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.IDOperatore != 0)
                        cursor.IDOperatore.Value = filter.IDOperatore;
                    if (filter.DataInizio.HasValue)
                    {
                        if (filter.DataFine.HasValue)
                        {
                            cursor.DataPrevista.Between(filter.DataInizio, filter.DataFine);
                        }
                        else
                        {
                            cursor.DataPrevista.Value = filter.DataInizio;
                            cursor.DataPrevista.Operator = OP.OP_GE;
                        }
                    }
                    else if (filter.DataFine.HasValue)
                    {
                        cursor.DataPrevista.Value = filter.DataFine;
                        cursor.DataPrevista.Operator = OP.OP_LE;
                    }

                    if (!string.IsNullOrEmpty(filter.Motivo))
                    {
                        cursor.Note.Value = DMD.Strings.JoinW("%", filter.Motivo, "%");
                        cursor.Note.Operator = OP.OP_LIKE;
                    }
                    // filter.Flags = 0; //SystemUtils
                    cursor.ModificatoIl.Value = DMD.DateUtils.DateAdd("s", -60, filter.fromDate.Value);
                    cursor.ModificatoIl.Operator = OP.OP_GT;
                    if (filter.nMax.HasValue)
                        cursor.PageSize = filter.nMax.Value;
                    int i = 0;
                    while (!cursor.EOF() && (filter.nMax.HasValue == false || i < filter.nMax.Value))
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                }


                return ret;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CRicontattiClass m_Ricontatti = null;

        /// <summary>
        /// Repository dei ricontatti
        /// </summary>
        public static CRicontattiClass Ricontatti
        {
            get
            {
                if (m_Ricontatti is null)
                    m_Ricontatti = new CRicontattiClass();
                return m_Ricontatti;
            }
        }
    }
}