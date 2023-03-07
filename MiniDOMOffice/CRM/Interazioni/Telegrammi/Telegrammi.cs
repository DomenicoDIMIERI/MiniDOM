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
        /// Repository di oggetti di tipo <see cref="CTelegramma"/>
        /// </summary>
        [Serializable]
        public sealed class CTelegrammiClass 
            : CContattiRepository<CTelegramma>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelegrammiClass() 
                : base("modContattiTelegrammi", typeof(CTelegrammiCursor), 0)
            {
            }
 

            //public CStatisticheOperazione GetStatsEffettuate(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutTelegramNum]) As [Num] , Sum([OutTelegramTotLen]) As [TotLen], Min([OutTelegramMinLen]) As [MinLen], Max([OutTelegramMaxLen]) As [MaxLen], SUM([OutTelegramCost]) As [TotCost] FROM [tbl_CRMStats] ";
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
            //            ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
            //            ret.TotalWait = 0d;
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
            //    string dbSQL = "SELECT Sum([OutTelegramNum]) As [Num] , Sum([OutTelegramTotLen]) As [TotLen], Min([OutTelegramMinLen]) As [MinLen], Max([OutTelegramMaxLen]) As [MaxLen] FROM [tbl_CRMStats] ";
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
            //            ret.TotalWait = 0d;
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
            //    string dbSQL = "SELECT Sum([InTelegramNum]) As [Num], Sum([InTelegramTotLen]) As [TotLen], Min([InTelegramMinLen]) As [MinLen], Max([InTelegramMaxLen]) As [MaxLen], Max([InTelegramCost]) As [TotCost] FROM [tbl_CRMStats] ";
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
            //            ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
            //            ret.TotalWait = 0d; // Formats.ToDouble(dbRis("Attesa"))
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
            //    string dbSQL = "SELECT Sum([InTelegramNum]) As [Num], Sum([InTelegramTotLen]) As [TotLen], Min([InTelegramMinLen]) As [MinLen], Max([InTelegramMaxLen]) As [MaxLen] FROM [tbl_CRMStats] ";
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
            //            ret.TotalWait = 0d; // Formats.ToDouble(dbRis("Attesa"))
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

            //public CStatisticheOperazione GetOutTelegrammiStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([OutTelegramNum]) As [Num], Sum([OutTelegramTotLen]) As [TotLen], Min([OutTelegramMinLen]) As [MinLen], Max([OutTelegramMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
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
            //        dbRis = CRM.TelDB.ExecuteReader(dbSQL);
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = 0d;
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

            //public CStatisticheOperazione GetInTelegrammiStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    string dbSQL = "SELECT Sum([InTelegramNum]) As [Num], Sum([InTelegramTotLen]) As [TotLen], Min([InTelegramMinLen]) As [MinLen], Max([InTelegramMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
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
            //        dbRis = CRM.TelDB.ExecuteReader(dbSQL);
            //        if (dbRis.Read())
            //        {
            //            ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //            ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //            ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //            ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //            ret.TotalWait = 0d;
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

            //public CTelegramma GetUltimaTelegrammaInCorso(int pID)
            //{
            //    if (pID == 0)
            //        return null;
            //    lock (m_inAttesaLock)
            //    {
            //        foreach (CTelegramma v in InAttesa)
            //        {
            //            if (v.IDPersona == pID)
            //                return v;
            //        }
            //    }

            //    return null;
            //}

            //private CCollection<CTelegramma> InnerInAttesa()
            //{
            //    if (m_InAttesa is null)
            //    {
            //        m_InAttesa = new CCollection<CTelegramma>();
            //        var cursor = new CTelegrammiCursor();
            //        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //        cursor.IDAzienda.Value = DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale);
            //        cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //        cursor.StatoConversazione.Value = StatoConversazione.CONCLUSO;
            //        cursor.StatoConversazione.Operator = OP.OP_NE;
            //        cursor.IgnoreRights = true;
            //        while (!cursor.EOF())
            //        {
            //            m_InAttesa.Add(cursor.Item);
            //            cursor.MoveNext();
            //        }

            //        cursor.Dispose();
            //    }

            //    return m_InAttesa;
            //}

            //public CCollection<CTelegramma> InAttesa
            //{
            //    get
            //    {
            //        lock (m_inAttesaLock)
            //            return new CCollection<CTelegramma>(InnerInAttesa());
            //    }
            //}

            //public void SetInAttesa(CTelegramma item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InnerInAttesa().GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InnerInAttesa().Remove(oldItem);
            //        InnerInAttesa().Add(item);
            //    }
            //}

            //public void SetFineAttesa(CTelegramma item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InnerInAttesa().GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InnerInAttesa().Remove(oldItem);
            //    }
            //}

            //public CCollection<CTelegramma> GetTelegrammiRicevute(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? dataInizio, DateTime? dataFine)
            //{
            //    var ret = new CCollection<CTelegramma>();
            //    var cursor = new CTelegrammiCursor();
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
            //        ret.Add(cursor.Item);
            //        cursor.MoveNext();
            //    }

            //    if (cursor is object)
            //    {
            //        cursor.Dispose();
            //        cursor = null;
            //    }

            //    return ret;
            //}

            //public CTelegramma GetUltimoTelegramma(Anagrafica.CPersona p)
            //{
            //    var cursor = new CTelegrammiCursor();
            //    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //    cursor.IDPersona.Value = DBUtils.GetID(p);
            //    cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //    cursor.IgnoreRights = true;
            //    var ret = cursor.Item;
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
     
        private static CTelegrammiClass m_Telegrammi = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CTelegramma"/>
        /// </summary>
        public static CTelegrammiClass Telegrammi
        {
            get
            {
                if (m_Telegrammi is null)
                    m_Telegrammi = new CTelegrammiClass();
                return m_Telegrammi;
            }
        }
    }
}