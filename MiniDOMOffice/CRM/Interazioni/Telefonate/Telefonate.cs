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
        /// Repository di oggetti di tipo <see cref="CTelefonata"/>
        /// </summary>
        [Serializable]
        public sealed class CTelefonateClass
            : CContattiRepository<CustomerCalls.CTelefonata>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelefonateClass() 
                : base("modContattiTelefonate", typeof(CTelefonateCursor), 0)
            {
            }

            //public CustomerCalls.CRMStatsAggregation GetStats(Anagrafica.CRMFilter filter)
            //{
            //    var ret = new CustomerCalls.CRMStatsAggregation();
                
            //    filter = DMD.RunTime.Clone(filter);
            //    filter.MostraAppuntamenti = false;
            //    filter.MostraTelefonate = true;

            //    // ret.Previste = Ricontatti.ContaPreviste(filter)
            //    filter = DMD.RunTime.Clone(filter);
            //    ret.Effettuate = CustomerCalls.Telefonate.GetStatsEffettuate(filter);
            //    ret.Ricevute = CustomerCalls.Telefonate.GetStatsRicevute(filter);
            //    return ret;
            //}

            //public CustomerCalls.CStatisticheOperazione GetStatsEffettuate(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen], SUM([OutCallCost]) AS [TotCost] FROM [tbl_CRMStats]  ";
            //    string wherePart = "";
            //    if (filter.IDPuntoOperativo != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    if (filter.IDOperatore != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    if (filter.DataInizio.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    if (filter.DataFine.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    if (!string.IsNullOrEmpty(wherePart))
            //        dbSQL += " WHERE " + wherePart;
            //    var ret = new CustomerCalls.CStatisticheOperazione();
            //    using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
            //    {
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //            ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
            //            // ret.MinWait = Formats.ToDouble(dbRis("MinWait"))
            //            // ret.MaxWait = Formats.ToDouble(dbRis("MaxWait"))
            //        }

            //        return ret;
            //    }
            //}

            //public int ContaEffettuate(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    return GetStatsEffettuate(filter, ignoreRights).Numero;
            //}

            //public CustomerCalls.CStatisticheOperazione GetStatsRicevute(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen], SUM([InCallCost]) AS [TotCost] FROM [tbl_CRMStats] ";
            //    string wherePart = "";
            //    if (filter.IDPuntoOperativo != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    if (filter.IDOperatore != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    if (filter.DataInizio.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    if (filter.DataFine.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    if (!string.IsNullOrEmpty(wherePart))
            //        dbSQL += " WHERE " + wherePart;

            //    var ret = new CustomerCalls.CStatisticheOperazione();

            //    using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
            //    {
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //            ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
            //        }

            //        return ret;
            //    }
            //}

            //public int ContaRicevute(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] ";
            //    string wherePart = "";
            //    if (filter.IDPuntoOperativo != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    if (filter.IDOperatore != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    if (filter.DataInizio.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    if (filter.DataFine.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    if (!string.IsNullOrEmpty(wherePart))
            //        dbSQL += " WHERE " + wherePart;
                
            //    var ret = new CustomerCalls.CStatisticheOperazione();
            //    using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
            //    { 
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //        }

            //        return ret.Numero;
            //    }
                 
            //}

            //public CustomerCalls.CStatisticheOperazione ContaTelefonateEffettuatePerData(int idUfficio, int idOperatore, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore]=" + DBUtils.DBNumber(idOperatore);
            //    if (inizio.HasValue)
            //        dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    if (fine.HasValue)
            //        dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
                
            //    var ret = new CustomerCalls.CStatisticheOperazione();
            //    using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL))
            //    { 
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //        }

            //        return ret;
            //    }
                
            //}

            //public CustomerCalls.CStatisticheOperazione GetOutCallsStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
            //    for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
            //    {
            //        if (i > 0)
            //            dbSQL += ",";
            //        dbSQL += DBUtils.DBNumber(operatori[i]);
            //    }

            //    dbSQL += ")";
            //    if (inizio.HasValue)
            //        dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    if (fine.HasValue)
            //        dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
                
            //    var ret = new CustomerCalls.CStatisticheOperazione();
            //    using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
            //    { 
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //        }

            //        return ret;
            //    }
                 
            //}

            //public CustomerCalls.CStatisticheOperazione ContaTelefonateRicevutePerData(int idUfficio, int idOperatore, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    // If (idOperatore <> 0) Then
            //    // Return Me.ContaTelefonateRicevutePerData(idUfficio, {idOperatore}, inizio, fine, ignoreRights)
            //    // Else
            //    // Return Me.ContaTelefonateRicevutePerData(idUfficio, {}, inizio, fine, ignoreRights)
            //    // End If
            //    string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore]=" + DBUtils.DBNumber(idOperatore);
            //    if (inizio.HasValue)
            //        dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    if (fine.HasValue)
            //        dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
                
            //    var ret = new CustomerCalls.CStatisticheOperazione();

            //    using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL))
            //    { 
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //        }

            //        return ret;
            //    }
                 
            //}

            //public CustomerCalls.CStatisticheOperazione GetInCallStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
            //    for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
            //    {
            //        if (i > 0)
            //            dbSQL += ",";
            //        dbSQL += DBUtils.DBNumber(operatori[i]);
            //    }

            //    dbSQL += ")";
            //    if (inizio.HasValue)
            //        dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    if (fine.HasValue)
            //        dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));

            //    var ret = new CustomerCalls.CStatisticheOperazione();
            //    using (var dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader(dbSQL)) 
            //    { 
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = Sistema.Formats.ToDouble(dbRis["Attesa"]);
            //        }
            //        return ret;
            //    }
 
            //}

            //public int ContaPersoneContattate(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    DateTime? d1, d2;
            //    // Dim ret As Integer
            //    d1 = DMD.DateUtils.GetDatePart(inizio);
            //    if (Sistema.Types.IsNull(fine))
            //    {
            //        d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d1);
            //    }
            //    else
            //    {
            //        d2 = DMD.DateUtils.GetDatePart(fine);
            //        d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d2);
            //    }
            //    // Dim wherePart As String = "[tbl_Persone].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Ricevuta]=" & DBUtils.DBBool(False)
            //    string wherePart = "";
            //    wherePart = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
            //    if (d1.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]>=" + DBUtils.DBDate(d1.Value), " AND ");
            //    if (d2.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]<=" + DBUtils.DBDate(d2.Value), " AND ");
            //    if (idUfficio != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDPuntoOperativo]=" + idUfficio, " AND ");
            //    if (DMD.Arrays.Len(operatori) > 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDOperatore] In (" + DBUtils.MakeArrayStr(operatori) + ")", " AND ");
            //    return Sistema.Formats.ToInteger(CustomerCalls.CRM.TelDB.ExecuteScalar("SELECT Count(*) FROM (SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " + wherePart + " GROUP BY [IDPersona])"));
            //}

            //public int[] GetIDPersoneContattate(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    DateTime? d1, d2;
            //    // Dim ret As Integer
            //    d1 = DMD.DateUtils.GetDatePart(inizio);
            //    if (Sistema.Types.IsNull(fine))
            //    {
            //        d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d1);
            //    }
            //    else
            //    {
            //        d2 = DMD.DateUtils.GetDatePart(fine);
            //        d2 = DMD.DateUtils.DateAdd("s", 24 * 3600 - 1, d2);
            //    }

            //    string wherePart = ""; // "[tbl_Persone].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Ricevuta]=" & DBUtils.DBBool(False)
            //    wherePart = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
            //    if (d1.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]>=" + DBUtils.DBDate(d1.Value), " AND ");
            //    if (d2.HasValue)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[Data]<=" + DBUtils.DBDate(d2.Value), " AND ");
            //    if (idUfficio != 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDPuntoOperativo]=" + idUfficio, " AND ");
            //    if (DMD.Arrays.Len(operatori) > 0)
            //        wherePart = DMD.Strings.Combine(wherePart, "[tbl_Telefonate].[IDOperatore] In (" + DBUtils.MakeArrayStr(operatori) + ")", " AND ");

            //    int[] ret = null;
            //    var cntSQL = DMD.Strings.JoinW("SELECT Count(*) FROM (SELECT [IDPersona] FROM [tbl_Telefonate] WHERE ", wherePart, " GROUP BY [IDPersona])");
            //    int cnt = DMD.Integers.CInt(CustomerCalls.CRM.TelDB.ExecuteScalar(cntSQL));
            //    if (cnt > 0)
            //    {
            //        ret = new int[cnt];
            //        using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " + wherePart + " GROUP BY [IDPersona]"))
            //        {
            //            int i = 0;
            //            while (!dbRis.Read())
            //                ret[i] = DMD.Integers.CInt(dbRis["IDPersona"]);
            //        }
            //    }

            //    return ret;
            //}

            //private string FormatImpiego(Anagrafica.CImpiegato impiego)
            //{
            //    string ret = "";
            //    if (!string.IsNullOrEmpty(impiego.Posizione))
            //        ret = impiego.Posizione;
            //    if (!string.IsNullOrEmpty(impiego.TipoRapporto))
            //        ret = DMD.Strings.Combine(ret, "(" + impiego.TipoRapporto + ")", " ");
            //    if (!string.IsNullOrEmpty(impiego.NomeAzienda))
            //        ret = DMD.Strings.Combine(ret, impiego.NomeAzienda, " presso ");
            //    if (impiego.DataAssunzione.HasValue)
            //        ret = DMD.Strings.Combine(ret, Sistema.Formats.FormatUserDate(impiego.DataAssunzione), " dal ");
            //    return ret;
            //}

            //public CustomerCalls.CTelefonata GetUltimaTelefonataInCorso(int pID)
            //{
            //    if (pID == 0)
            //        return null;
            //    lock (m_inAttesaLock)
            //    {
            //        foreach (CustomerCalls.CTelefonata v in InAttesa)
            //        {
            //            if (v.IDPersona == pID)
            //                return v;
            //        }
            //    }

            //    return null;
            //}

            //public int GetIDPersoneContattate(int idUfficio, object p2, DateTime? inizio, DateTime? fine, bool ir)
            //{
            //    throw new NotImplementedException();
            //}

            //protected CCollection<CustomerCalls.CTelefonata> InternalList()
            //{
            //    if (m_InAttesa is null)
            //    {
            //        m_InAttesa = new CCollection<CustomerCalls.CTelefonata>();
            //        using (var cursor = new CustomerCalls.CTelefonateCursor())
            //        {
            //            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //            cursor.IDAzienda.Value = DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale);
            //            cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //            cursor.StatoConversazione.Value = CustomerCalls.StatoConversazione.CONCLUSO;
            //            cursor.StatoConversazione.Operator = OP.OP_NE;
            //            cursor.IgnoreRights = true;
            //            while (!cursor.EOF())
            //            {
            //                m_InAttesa.Add(cursor.Item);
            //                cursor.MoveNext();
            //            }
            //        }
            //    }

            //    return m_InAttesa;
            //}

            //public CCollection<CustomerCalls.CTelefonata> InAttesa
            //{
            //    get
            //    {
            //        lock (m_inAttesaLock)
            //            return new CCollection<CustomerCalls.CTelefonata>(InternalList());
            //    }
            //}

            //public void SetInAttesa(CustomerCalls.CTelefonata item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InternalList().GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InternalList().Remove(oldItem);
            //        InternalList().Add(item);
            //    }
            //}

            //public void SetFineAttesa(CustomerCalls.CTelefonata item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InternalList().GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InternalList().Remove(oldItem);
            //    }
            //}

            //protected internal new void doItemCreated(ItemEventArgs e)
            //{
            //    base.doItemCreated(e);
            //}

            //protected internal new void doItemDeleted(ItemEventArgs e)
            //{
            //    base.doItemDeleted(e);
            //}

            //protected internal new void doItemModified(ItemEventArgs e)
            //{
            //    base.doItemModified(e);
            //}

            private class ByDateComparer 
                : IComparer, IComparer<CRMStatistichePersona>
            {
                public int Compare(CRMStatistichePersona s1, CRMStatistichePersona s2)
                {
                    return DMD.DateUtils.Compare(s1.DataUltimoContatto, s2.DataUltimoContatto);
                }

                int IComparer.Compare(object x, object y)
                {
                    return this.Compare((CRMStatistichePersona)x, (CRMStatistichePersona)y);
                }
            }

            /// <summary>
            /// Restituisce le persone che si suggerisce di contattare
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public CCollection<CActivePerson> GetSuggeriti(CRMFilter filter)
            {
                var personeID = DMD.Arrays.Empty<int>();
                
                if (filter is null)
                       throw new ArgumentNullException("filter");
                
                var ret = new CCollection<CActivePerson>();
                var tmp = new Dictionary<int, CRMStatistichePersona>();
                
                CPersonaFisica persona;
                CRMStatistichePersona stats;
                
                var currentLogin = minidom.Sistema.Users.CurrentUser.CurrentLogin;

                using (var cursor = new CRMStatistichePersona())
                {
                    // If (filter.IDOperatore <> 0) Then cursor.id
                    // If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                    cursor.IDPuntoOperativo.Value = DBUtils.GetID(currentLogin.Ufficio);
                    cursor.DataUltimoContattoOk.Value = DMD.DateUtils.DateAdd(DateTimeInterval.Month, -8, DMD.DateUtils.ToDay());
                    cursor.DataUltimoContattoOk.Operator = OP.OP_LT;
                    cursor.DataUltimoContattoOk.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;

                    // cursor.Flags.Value = (PersonFlags.PersonaFisica Or PersonFlags.Ricontattabile) And Not PersonFlags.Deceduto

                    while (cursor.Read())
                    {
                        stats = cursor.Item;
                        if (stats.IDPersona != 0)
                        {
                            tmp.Add(stats.IDPersona, stats);
                            personeID = DMD.Arrays.Append(personeID, stats.IDPersona);
                        }
                    }

                }

                if (tmp.Count > 0)
                {
                    using (var pcursor = new Anagrafica.CPersonaFisicaCursor()) {
                        pcursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        pcursor.Deceduto.Value = false;
                        pcursor.ID.ValueIn(personeID);
                        while (pcursor.Read())
                        {
                            persona = pcursor.Item;
                            stats = tmp[DBUtils.GetID(persona, 0)];
                            stats.Persona = persona;
                        }
                    }
                }

                var lst = new List<CRMStatistichePersona>();
                lst.Sort(new ByDateComparer());
                foreach (var currentStats in lst)
                {
                    stats = currentStats;
                    if (filter.nMax.HasValue && ret.Count > filter.nMax.Value)
                        break;

                    persona = (Anagrafica.CPersonaFisica)stats.Persona;
                    bool include = !persona.Deceduto;
                    if (include)
                    {
                        if (filter.TipiRapporto.Count > 0)
                        {
                            include = false;
                            foreach (string tr in filter.TipiRapporto)
                            {
                                if ((persona.ImpiegoPrincipale.TipoRapporto ?? "") == (tr ?? ""))
                                {
                                    include = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (include)
                    {
                        var a = new Sistema.CActivePerson();
                        a.Person = persona;
                        a.GiornataIntera = true;
                        a.IconURL = persona.IconURL;
                        a.MoreInfo.Add("DettaglioEsito", persona.DettaglioEsito);
                        a.MoreInfo.Add("DataUltimoContatto", stats.DataUltimoContatto);
                        ret.Add(a);
                    }
                }

                return ret;
                 
            }
        }
    }

    public partial class CustomerCalls
    {
        private static CTelefonateClass m_Telefonate = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CTelefonata"/>
        /// </summary>
        public static CTelefonateClass Telefonate
        {
            get
            {
                if (m_Telefonate is null)
                    m_Telefonate = new CTelefonateClass();
                return m_Telefonate;
            }
        }
    }
}