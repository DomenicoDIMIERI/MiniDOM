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
        /// Repository di oggetti di tipo <see cref="CVisita"/>
        /// </summary>
        [Serializable]
        public sealed class CVisiteClass
             : CContattiRepository<CVisita>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CVisiteClass() 
                : base("modContattiVisite", typeof(CVisiteCursor), 0)
            {
            }
             
            //public CStatisticheOperazione GetStatsEffettuate(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutDateNum]) As [Num] , Sum ([OutDateTotWait]) As [Attesa], Sum([OutDateTotLen]) As [TotLen], Min([OutDateMinLen]) As [MinLen], Max([OutDateMaxLen]) As [MaxLen], SUM([OutDateCost]) AS [TotCost] FROM [tbl_CRMStats] ";
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
            //    IDataReader dbRis = null;
            //    var ret = new CStatisticheOperazione();
            //    try
            //    {
            //        dbRis = CRM.StatsDB.ExecuteReader(dbSQL);
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
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        if (dbRis is object)
            //            dbRis.Dispose();
            //    }
            //}

            //public int ContaEffettuate(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutDateNum]) As [Num] , Sum ([OutDateTotWait]) As [Attesa], Sum([OutDateTotLen]) As [TotLen], Min([OutDateMinLen]) As [MinLen], Max([OutDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] ";
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
            //    IDataReader dbRis = null;
            //    var ret = new CStatisticheOperazione();
            //    try
            //    {
            //        dbRis = CRM.StatsDB.ExecuteReader(dbSQL);
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
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        if (dbRis is object)
            //            dbRis.Dispose();
            //    }
            //}

            //public CStatisticheOperazione GetStatsRicevute(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InDateNum]) As [Num], Sum ([InDateTotWait]) As [Attesa], Sum([InDateTotLen]) As [TotLen], Min([InDateMinLen]) As [MinLen], Max([InDateMaxLen]) As [MaxLen], SUM([InDateCost]) As [TotCost] FROM [tbl_CRMStats] ";
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
            //    IDataReader dbRis = null;
            //    var ret = new CStatisticheOperazione();
            //    try
            //    {
            //        dbRis = CRM.StatsDB.ExecuteReader(dbSQL);
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
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        if (dbRis is object)
            //            dbRis.Dispose();
            //    }
            //}

            //public int ContaRicevute(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InDateNum]) As [Num], Sum ([InDateTotWait]) As [Attesa], Sum([InDateTotLen]) As [TotLen], Min([InDateMinLen]) As [MinLen], Max([InDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] ";
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
            //    IDataReader dbRis = null;
            //    var ret = new CStatisticheOperazione();
            //    try
            //    {
            //        dbRis = CRM.StatsDB.ExecuteReader(dbSQL);
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
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        if (dbRis is object)
            //            dbRis.Dispose();
            //    }
            //}

            //public CRMStatsAggregation GetStats(Anagrafica.CRMFilter filter)
            //{
            //    var ret = new CRMStatsAggregation();
            //    filter = (Anagrafica.CRMFilter)Sistema.Types.Clone(filter);
            //    filter.MostraTelefonate = false;
            //    filter.MostraAppuntamenti = true;

            //    // ret.Previste = Ricontatti.ContaPreviste(filter)
            //    // filter.DataInizio = Calendar.ToDay
            //    // filter.DataFine = Calendar.DateAdd(DateTimeInterval.Second, 3600 * 24 - 1, filter.DataInizio.Value)

            //    ret.Effettuate = GetStatsEffettuate(filter);
            //    ret.Ricevute = GetStatsRicevute(filter);
            //    return ret;
            //}

            //public CStatisticheOperazione GetOutVisitsStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutDateNum]) As [Num], Sum([OutDateTotWait]) As [Attesa], Sum([OutDateTotLen]) As [TotLen], Min([OutDateMinLen]) As [MinLen], Max([OutDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
            //    for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
            //    {
            //        if (i > 0)
            //            dbSQL += ", ";
            //        dbSQL += DBUtils.DBNumber(operatori[i]);
            //    }

            //    dbSQL += ")";
            //    if (inizio.HasValue)
            //        dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    if (fine.HasValue)
            //        dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
            //    IDataReader dbRis = null;
            //    var ret = new CStatisticheOperazione();
            //    try
            //    {
            //        dbRis = CRM.StatsDB.ExecuteReader(dbSQL);
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
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        if (dbRis is object)
            //            dbRis.Dispose();
            //    }
            //}

            //public CStatisticheOperazione GetInVisitsStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InDateNum]) As [Num], Sum([InDateTotWait]) As [Attesa], Sum([InDateTotLen]) As [TotLen], Min([InDateMinLen]) As [MinLen], Max([InDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
            //    for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
            //    {
            //        if (i > 0)
            //            dbSQL += ", ";
            //        dbSQL += DBUtils.DBNumber(operatori[i]);
            //    }

            //    dbSQL += ")";
            //    if (inizio.HasValue)
            //        dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    if (fine.HasValue)
            //        dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
            //    IDataReader dbRis = null;
            //    var ret = new CStatisticheOperazione();
            //    try
            //    {
            //        dbRis = CRM.StatsDB.ExecuteReader(dbSQL);
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
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        if (dbRis is object)
            //            dbRis.Dispose();
            //    }
            //}

            //public CVisita GetUltimaVisitaInCorso(int pID)
            //{
            //    if (pID == 0)
            //        return null;
            //    lock (m_inAttesaLock)
            //    {
            //        foreach (CVisita v in InAttesa)
            //        {
            //            if (v.IDPersona == pID)
            //                return v;
            //        }
            //    }

            //    return null;
            //}

            //protected CCollection<CVisita> InternalList()
            //{
            //    if (m_InAttesa is null)
            //    {
            //        m_InAttesa = new CCollection<CVisita>();
            //        var cursor = new CVisiteCursor();
            //        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //        cursor.IDAzienda.Value = DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale);
            //        cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //        cursor.StatoConversazione.Value = StatoConversazione.CONCLUSO;
            //        cursor.StatoConversazione.Operator = OP.OP_NE;
            //        cursor.IgnoreRights = true;
            //        while (!cursor.EOF())
            //        {
            //            m_InAttesa.Add((CVisita)cursor.Item);
            //            cursor.MoveNext();
            //        }

            //        cursor.Dispose();
            //    }

            //    return m_InAttesa;
            //}

            //public CCollection<CVisita> InAttesa
            //{
            //    get
            //    {
            //        lock (m_inAttesaLock)
            //            return new CCollection<CVisita>(InternalList());
            //    }
            //}

            //public void SetInAttesa(CVisita item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InternalList().GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InternalList().Remove(oldItem);
            //        InternalList().Add(item);
            //    }
            //}

            //public void SetFineAttesa(CVisita item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InternalList().GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InternalList().Remove(oldItem);
            //    }
            //}

            //public CCollection<CVisita> GetVisiteRicevute(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? dataInizio, DateTime? dataFine)
            //{
            //    var ret = new CCollection<CVisita>();
            //    var cursor = new CVisiteCursor();
            //    if (ufficio is object)
            //        cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio);
            //    if (operatore is object)
            //        cursor.IDOperatore.Value = DBUtils.GetID(operatore);
            //    if (dataInizio.HasValue)
            //    {
            //        if (dataFine.HasValue)
            //        {
            //            cursor.Data.Between(dataInizio.Value, dataFine.Value);
            //        }
            //        else
            //        {
            //            cursor.Data.Value = dataInizio.Value;
            //            cursor.Data.Operator = OP.OP_GE;
            //        }
            //    }
            //    else if (dataFine.HasValue)
            //    {
            //        cursor.Data.Value = dataFine.Value;
            //        cursor.Data.Operator = OP.OP_LE;
            //    }

            //    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //    cursor.Ricevuta.Value = true;
            //    while (!cursor.EOF())
            //    {
            //        ret.Add((CVisita)cursor.Item);
            //        cursor.MoveNext();
            //    }

            //    if (cursor is object)
            //    {
            //        cursor.Dispose();
            //        cursor = null;
            //    }

            //    return ret;
            //}

            //public CVisita GetUltimaVisita(Anagrafica.CPersona p)
            //{
            //    var cursor = new CVisiteCursor();
            //    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //    cursor.IDPersona.Value = DBUtils.GetID(p);
            //    cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //    cursor.IgnoreRights = true;
            //    CVisita ret = (CVisita)cursor.Item;
            //    if (cursor is object)
            //    {
            //        cursor.Dispose();
            //        cursor = null;
            //    }

            //    return ret;
            //}
        }

    }

    public partial class CustomerCalls
    {
     
        private static CVisiteClass m_Visite = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CVisita"/>
        /// </summary>
        public static CVisiteClass Visite
        {
            get
            {
                if (m_Visite is null)
                    m_Visite = new CVisiteClass();
                return m_Visite;
            }
        }
    }
}