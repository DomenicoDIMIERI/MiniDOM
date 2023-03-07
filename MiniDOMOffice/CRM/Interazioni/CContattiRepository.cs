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


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CContattoUtente"/>
        /// </summary>
        [Serializable]
        public abstract class CContattiRepository 
            : CModulesClass<CContattoUtente>
        {
            /// <summary>
            /// Oggetto per la sincronizzazione della lista interna dei contatti in attesa
            /// </summary>
            protected readonly object inAttesaLock = new object();

            /// <summary>
            /// Lista dei contatti in attesa
            /// </summary>
            private Dictionary<int, CContattoUtente> m_InAttesa;

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            /// <param name="cursorType"></param>
            /// <param name="cacheSize"></param>
            public CContattiRepository(string moduleName, System.Type cursorType, int cacheSize) 
                : base(moduleName, cursorType, cacheSize)
            {
                m_InAttesa = null;
            }

            /// <summary>
            /// Restituisce le statistiche
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public virtual CRMStatsAggregation GetStats(CRMFilter filter)
            {
                var ret = new CustomerCalls.CRMStatsAggregation();
                
                filter = DMD.RunTime.Clone(filter);
                filter.MostraAppuntamenti = false;
                filter.MostraTelefonate = true;

                // ret.Previste = Ricontatti.ContaPreviste(filter)
                filter = DMD.RunTime.Clone(filter);
                ret.Effettuate = this.GetStatsEffettuate(filter);
                ret.Ricevute = this.GetStatsRicevute(filter);
                return ret;
            }

            /// <summary>
            /// Restituisce le statistiche sui contatti in uscita
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual CStatisticheOperazione GetStatsEffettuate(CRMFilter filter, bool ignoreRights = true)
            {
                //TODO GetStatsEffettuate
                throw new NotImplementedException();

                //string dbSQL = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen], SUM([OutCallCost]) AS [TotCost] FROM [tbl_CRMStats]  ";
                //string wherePart = "";
                //if (filter.IDPuntoOperativo != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
                //if (filter.IDOperatore != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
                //if (filter.DataInizio.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
                //if (filter.DataFine.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
                //if (!string.IsNullOrEmpty(wherePart))
                //    dbSQL += " WHERE " + wherePart;
                //var ret = new CStatisticheOperazione();
                //using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
                //{
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //        ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
                //        // ret.MinWait = Formats.ToDouble(dbRis("MinWait"))
                //        // ret.MaxWait = Formats.ToDouble(dbRis("MaxWait"))
                //    }

                //    return ret;
                //}
            }

            /// <summary>
            /// Conta i contatti in uscita
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual int ContaEffettuate(CRMFilter filter, bool ignoreRights = true)
            {
                return GetStatsEffettuate(filter, ignoreRights).Numero;
            }

            /// <summary>
            /// Restituisce le statistiche sui contatti in ingresso
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual CStatisticheOperazione GetStatsRicevute(CRMFilter filter, bool ignoreRights = true)
            {
                //TODO GetStatsRicevute
                throw new NotImplementedException();

                //string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen], SUM([InCallCost]) AS [TotCost] FROM [tbl_CRMStats] ";
                //string wherePart = "";
                //if (filter.IDPuntoOperativo != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
                //if (filter.IDOperatore != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
                //if (filter.DataInizio.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
                //if (filter.DataFine.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
                //if (!string.IsNullOrEmpty(wherePart))
                //    dbSQL += " WHERE " + wherePart;

                //var ret = new CStatisticheOperazione();

                //using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
                //{
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //        ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
                //    }

                //    return ret;
                //}
            }

            /// <summary>
            /// Conta i contatti in ingresso
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual int ContaRicevute(CRMFilter filter, bool ignoreRights = true)
            {
                return this.GetStatsRicevute(filter, ignoreRights).Numero;
                //string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] ";
                //string wherePart = "";
                //if (filter.IDPuntoOperativo != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
                //if (filter.IDOperatore != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
                //if (filter.DataInizio.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
                //if (filter.DataFine.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
                //if (!string.IsNullOrEmpty(wherePart))
                //    dbSQL += " WHERE " + wherePart;
                
                //var ret = new CStatisticheOperazione();
                //using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
                //{ 
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //    }

                //    return ret.Numero;
                //}
                 
            }

            /// <summary>
            /// Restituisce le statistiche dei contatti in uscita
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="idOperatore"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual CStatisticheOperazione GetEffettuateDaOperatore(
                                    int idUfficio, 
                                    int idOperatore, 
                                    DateTime? inizio, 
                                    DateTime? fine, 
                                    bool ignoreRights = true
                                    )
            {
                //TODO ContaEffettuatePerData
                throw new NotImplementedException();

                //string dbSQL = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore]=" + DBUtils.DBNumber(idOperatore);
                //if (inizio.HasValue)
                //    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
                //if (fine.HasValue)
                //    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));

                //var ret = new CStatisticheOperazione();
                //using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL))
                //{ 
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //    }

                //    return ret;
                //}

            }

            /// <summary>
            /// Restituisce le statistiche per il gruppo di operatori
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="operatori"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual CStatisticheOperazione GetEffettuateDaOperatori(
                                            int idUfficio, 
                                            int[] operatori, 
                                            DateTime? inizio, 
                                            DateTime? fine, 
                                            bool ignoreRights = true
                                            )
            {
                //TODO GetEffettuateDaOperatori
                throw new NotImplementedException();

                //string dbSQL = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
                //for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
                //{
                //    if (i > 0)
                //        dbSQL += ",";
                //    dbSQL += DBUtils.DBNumber(operatori[i]);
                //}

                //dbSQL += ")";
                //if (inizio.HasValue)
                //    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
                //if (fine.HasValue)
                //    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));

                //var ret = new CStatisticheOperazione();
                //using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
                //{ 
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //    }

                //    return ret;
                //}

            }

            /// <summary>
            /// Restituisce le statistiche per operatore
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="idOperatore"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual CStatisticheOperazione GetRicevuteDaOperatore(
                                            int idUfficio, 
                                            int idOperatore, 
                                            DateTime? inizio, 
                                            DateTime? fine, 
                                            bool ignoreRights = true
                                            )
            {
                //TODO GetRicevuteDaOperatore
                throw new NotImplementedException();

                //// If (idOperatore <> 0) Then
                //// Return Me.ContaTelefonateRicevutePerData(idUfficio, {idOperatore}, inizio, fine, ignoreRights)
                //// Else
                //// Return Me.ContaTelefonateRicevutePerData(idUfficio, {}, inizio, fine, ignoreRights)
                //// End If
                //string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore]=" + DBUtils.DBNumber(idOperatore);
                //if (inizio.HasValue)
                //    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
                //if (fine.HasValue)
                //    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));

                //var ret = new CStatisticheOperazione();

                //using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
                //{ 
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //    }

                //    return ret;
                //}

            }

            /// <summary>
            /// Restituisce le statistiche per il gruppo di operatori
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="operatori"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual CStatisticheOperazione GetRicevuteDaOperatori(
                                                    int idUfficio, 
                                                    int[] operatori, 
                                                    DateTime? inizio, 
                                                    DateTime? fine, 
                                                    bool ignoreRights = true
                                                    )
            {
                //TODO GetRicevuteDaOperatori
                throw new NotImplementedException();

                //string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
                //for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
                //{
                //    if (i > 0)
                //        dbSQL += ",";
                //    dbSQL += DBUtils.DBNumber(operatori[i]);
                //}

                //dbSQL += ")";
                //if (inizio.HasValue)
                //    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
                //if (fine.HasValue)
                //    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));

                //var ret = new CStatisticheOperazione();
                //using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL)) 
                //{ 
                //    if (dbRis.Read())
                //    {
                //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
                //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
                //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
                //        ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
                //    }
                //    return ret;
                //}

            }

            /// <summary>
            /// Restituisce il numero di persone diverse contatta dagli operatori nel periodo selezionato
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="operatori"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual int ContaPersoneContattate(
                                            int idUfficio, 
                                            int[] operatori, 
                                            DateTime? inizio, 
                                            DateTime? fine, 
                                            bool ignoreRights = true
                                            )
            {
                return this.GetIDPersoneContattate(idUfficio, operatori, inizio, fine, ignoreRights).Length;
                //DateTime? d1, d2;
                //// Dim ret As Integer
                //d1 = DMD.DateUtils.GetDatePart(inizio);
                //if (Sistema.Types.IsNull(fine))
                //{
                //    d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d1);
                //}
                //else
                //{
                //    d2 = DMD.DateUtils.GetDatePart(fine);
                //    d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d2);
                //}
                //// Dim wherePart As String = "[tbl_Persone].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Ricevuta]=" & DBUtils.DBBool(False)
                //string wherePart = "";
                //wherePart = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                //if (d1.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]>=" + DBUtils.DBDate(d1.Value), " AND ");
                //if (d2.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]<=" + DBUtils.DBDate(d2.Value), " AND ");
                //if (idUfficio != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDPuntoOperativo]=" + idUfficio, " AND ");
                //if (DMD.Arrays.Len(operatori) > 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDOperatore] In (" + DBUtils.MakeArrayStr(operatori) + ")", " AND ");
                //return Sistema.Formats.ToInteger(CustomerCalls.CRM.TelDB.ExecuteScalar("SELECT Count(*) FROM (SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " + wherePart + " GROUP BY [IDPersona])"));
            }

            /// <summary>
            /// Restituisce un array contenente gli id di tutte le persone contattate dall'operatore nel periodo indicato
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="idOperatore"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual int[] GetIDPersoneContattate(
                                            int idUfficio, 
                                            int idOperatore, 
                                            DateTime? inizio, 
                                            DateTime? fine, 
                                            bool ignoreRights = true
                                            )
            {
                //TODO GetIDPersoneContattate
                return this.GetIDPersoneContattate(idUfficio, new int[] { idOperatore }, inizio, fine, ignoreRights);
            }

            /// <summary>
            /// Restituisce un array contenente gli id di tutte le persone contattate dagli operatori nel periodo indicato
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="operatori"></param>
            /// <param name="inizio"></param>
            /// <param name="fine"></param>
            /// <param name="ignoreRights"></param>
            /// <returns></returns>
            public virtual int[] GetIDPersoneContattate(
                                            int idUfficio, 
                                            int[] operatori, 
                                            DateTime? inizio, 
                                            DateTime? fine, 
                                            bool ignoreRights = true
                                            )
            {
                //TODO GetIDPersoneContattate
                throw new NotImplementedException();
                //DateTime? d1, d2;
                //// Dim ret As Integer
                //d1 = DMD.DateUtils.GetDatePart(inizio);
                //if (Sistema.Types.IsNull(fine))
                //{
                //    d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d1);
                //}
                //else
                //{
                //    d2 = DMD.DateUtils.GetDatePart(fine);
                //    d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d2);
                //}

                //string wherePart = ""; // "[tbl_Persone].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Ricevuta]=" & DBUtils.DBBool(False)
                //wherePart = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                //if (d1.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]>=" + DBUtils.DBDate(d1.Value), " AND ");
                //if (d2.HasValue)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]<=" + DBUtils.DBDate(d2.Value), " AND ");
                //if (idUfficio != 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDPuntoOperativo]=" + idUfficio, " AND ");
                //if (DMD.Arrays.Len(operatori) > 0)
                //    wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDOperatore] In (" + DBUtils.MakeArrayStr(operatori) + ")", " AND ");

                //int[] ret = null;
                //var cntSQL = DMD.Strings.JoinW("SELECT Count(*) FROM (SELECT [IDPersona] FROM [tbl_Telefonate] WHERE ", wherePart, " GROUP BY [IDPersona])");
                //int cnt = DMD.Integers.CInt(CustomerCalls.CRM.TelDB.ExecuteScalar(cntSQL));
                //if (cnt > 0)
                //{
                //    ret = new int[cnt];
                //    using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " + wherePart + " GROUP BY [IDPersona]"))
                //    {
                //        int i = 0;
                //        while (!dbRis.Read())
                //            ret[i] = DMD.Integers.CInt(dbRis["IDPersona"]);
                //    }
                //}

                //return ret;
            }

             
            /// <summary>
            /// Restituisce l'ultimo contatto da o verso la persona
            /// </summary>
            /// <param name="pID"></param>
            /// <returns></returns>
            public CContattoUtente GetLastRunning(int pID)
            {
                if (pID == 0)
                    return null;
                lock (inAttesaLock)
                {
                    foreach (var v in InAttesa)
                    {
                        if (v.IDPersona == pID)
                            return v;
                    }
                }

                return null;
            }

           
            /// <summary>
            /// Lista interna
            /// </summary>
            protected Dictionary<int, CContattoUtente> InnerList
            {
                get
                {
                    if (m_InAttesa is null)
                    {
                        m_InAttesa = new Dictionary<int, CContattoUtente>();
                        using (var cursor = (CContattoUtenteCursor) this.CreateCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDAzienda.Value = DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale);
                            cursor.Data.SortOrder = SortEnum.SORT_DESC;
                            cursor.StatoConversazione.Value = CustomerCalls.StatoConversazione.CONCLUSO;
                            cursor.StatoConversazione.Operator = OP.OP_NE;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                var item = cursor.Item;
                                m_InAttesa.Add(DBUtils.GetID(item, 0), item);
                            }
                        }
                    }

                    return m_InAttesa;
                }
            }

            /// <summary>
            /// Restituisce la collezione dei contatti in attesa
            /// </summary>
            public List<CContattoUtente> InAttesa
            {
                get
                {
                    lock (inAttesaLock)
                        return new List<CContattoUtente>(InnerList.Values);
                }
            }

            /// <summary>
            /// Aggiunge il contatto in attesa
            /// </summary>
            /// <param name="item"></param>
            public virtual void SetInAttesa(CContattoUtente item)
            {
                lock (inAttesaLock)
                {
                    int id = DBUtils.GetID(item, 0);
                    if (InnerList.ContainsKey(id))
                        InnerList.Remove(id);
                    InnerList.Add(id, item);
                }
            }

            /// <summary>
            /// Rimuove il contatto in attesa
            /// </summary>
            /// <param name="item"></param>
            public virtual void SetFineAttesa(CContattoUtente item)
            {
                lock (inAttesaLock)
                {
                    int id = DBUtils.GetID(item, 0);
                    if (InnerList.ContainsKey(id))
                        InnerList.Remove(id);
                }
            }

              
        }
    }
     
}