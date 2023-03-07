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
        /// Repository di oggetti di tipo <see cref="SMSMessage"/>
        /// </summary>
        public sealed class CSMSClass
          : CContattiRepository<SMSMessage>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CSMSClass() 
                : base("modContattiSMS", typeof(SMSMessageCursor), 0)
            {
            }
             
            ///// <summary>
            ///// Restituisce le statistiche sugli SMS inviati
            ///// </summary>
            ///// <param name="filter"></param>
            ///// <param name="ignoreRights"></param>
            ///// <returns></returns>
            //public CStatisticheOperazione GetStatsSMSInviati(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    //TODO GetStatsSMSInviati
            //    throw new NotImplementedException();

            //    //string dbSQL = "SELECT Sum([OutSMSNum]) As [Num] , Sum([OutSMSTotLen]) As [TotLen], Min([OutSMSMinLen]) As [MinLen], Max([OutSMSMaxLen]) As [MaxLen], SUM([OutSMSCost]) As [TotCost] FROM [tbl_CRMStats]  ";
            //    //string wherePart = "";
            //    //if (filter.IDPuntoOperativo != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    //if (filter.IDOperatore != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    //if (filter.DataInizio.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    //if (filter.DataFine.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    //if (!string.IsNullOrEmpty(wherePart))
            //    //    dbSQL += " WHERE " + wherePart;

            //    //var ret = new CStatisticheOperazione();
            //    //using (var dbRis = CRM.StatsDB.ExecuteReader(dbSQL))
            //    //{  
            //    //    if (dbRis.Read())
            //    //    {
            //    //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //    //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //    //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //    //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //    //        ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
            //    //    }

            //    //    return ret;
            //    //}
                     
            //}

            ///// <summary>
            ///// Conta gli SMS inviati
            ///// </summary>
            ///// <param name="filter"></param>
            ///// <param name="ignoreRights"></param>
            ///// <returns></returns>
            //public int ContaSMSInviati(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    //TODO ContaSMSInviati
            //    throw new NotImplementedException();

            //    //string dbSQL = "SELECT Sum([OutSMSNum]) As [Num] , Sum([OutSMSTotLen]) As [TotLen], Min([OutSMSMinLen]) As [MinLen], Max([OutSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats]  ";
            //    //string wherePart = "";
            //    //if (filter.IDPuntoOperativo != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    //if (filter.IDOperatore != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    //if (filter.DataInizio.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    //if (filter.DataFine.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    //if (!string.IsNullOrEmpty(wherePart))
            //    //    dbSQL += " WHERE " + wherePart;
            //    //var ret = new CStatisticheOperazione();
            //    //using (var dbRis = CRM.StatsDB.ExecuteReader(dbSQL))
            //    //{  
            //    //    if (dbRis.Read())
            //    //    {
            //    //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //    //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //    //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //    //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //    //    }

            //    //    return ret.Numero;
            //    //}
                
            //}

            ///// <summary>
            ///// Restituisce le statistiche sugli SMS ricevuti
            ///// </summary>
            ///// <param name="filter"></param>
            ///// <param name="ignoreRights"></param>
            ///// <returns></returns>
            //public CStatisticheOperazione GetStatsSMSRicevuti(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    //TODO GetStatsSMSRicevuti
            //    throw new NotImplementedException();

            //    //string dbSQL = "SELECT Sum([InSMSNum]) As [Num], Sum([InSMSTotLen]) As [TotLen], Min([InSMSMinLen]) As [MinLen], Max([InSMSMaxLen]) As [MaxLen], SUM([InSMSCost]) As [TotCost] FROM [tbl_CRMStats]  ";
            //    //string wherePart = "";
            //    //if (filter.IDPuntoOperativo != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    //if (filter.IDOperatore != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    //if (filter.DataInizio.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    //if (filter.DataFine.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    //if (!string.IsNullOrEmpty(wherePart))
            //    //    dbSQL += " WHERE " + wherePart;
            //    //var ret = new CStatisticheOperazione();
            //    //using (var dbRis = CRM.StatsDB.ExecuteReader(dbSQL))
            //    //{  
            //    //    if (dbRis.Read())
            //    //    {
            //    //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //    //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //    //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //    //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //    //        ret.TotalCost = (decimal)Sistema.Formats.ToDouble(dbRis["TotCost"]);
            //    //    }

            //    //    return ret;
            //    //}
                
            //}

            ///// <summary>
            ///// Conta gli SMS ricevuti
            ///// </summary>
            ///// <param name="filter"></param>
            ///// <param name="ignoreRights"></param>
            ///// <returns></returns>
            //public int ContaSMSRicevuti(Anagrafica.CRMFilter filter, bool ignoreRights = true)
            //{
            //    //TODO ContaSMSRicevuti
            //    throw new NotImplementedException();

            //    //string dbSQL = "SELECT Sum([InSMSNum]) As [Num], Sum([InSMSTotLen]) As [TotLen], Min([InSMSMinLen]) As [MinLen], Max([InSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats]  ";
            //    //string wherePart = "";
            //    //if (filter.IDPuntoOperativo != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDPuntoOperativo] = " + DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ");
            //    //if (filter.IDOperatore != 0)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore] = " + DBUtils.DBNumber(filter.IDOperatore), " AND ");
            //    //if (filter.DataInizio.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ");
            //    //if (filter.DataFine.HasValue)
            //    //    wherePart = DMD.Strings.Combine(wherePart, "[Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(filter.DataFine.Value)), " AND ");
            //    //if (!string.IsNullOrEmpty(wherePart))
            //    //    dbSQL += " WHERE " + wherePart;
            //    //var ret = new CStatisticheOperazione();
            //    //using (var dbRis = CRM.StatsDB.ExecuteReader(dbSQL))
            //    //{  
            //    //    if (dbRis.Read())
            //    //    {
            //    //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //    //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //    //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //    //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //    //    }

            //    //    return ret.Numero;
            //    //}
                 
            //}

            ///// <summary>
            ///// Restituisce le statistiche
            ///// </summary>
            ///// <param name="filter"></param>
            ///// <returns></returns>
            //public CRMStatsAggregation GetStats(Anagrafica.CRMFilter filter)
            //{
            //    var ret = new CRMStatsAggregation();
            //    filter = DMD.RunTime.Clone(filter);
            //    filter.MostraTelefonate = false;
            //    filter.MostraAppuntamenti = true;

            //    // ret.Previste = Ricontatti.ContaPreviste(filter)
            //    // filter.DataInizio = Calendar.ToDay
            //    // filter.DataFine = Calendar.DateAdd(DateTimeInterval.Second, 3600 * 24 - 1, filter.DataInizio.Value)

            //    ret.Effettuate = GetStatsSMSInviati(filter);
            //    ret.Ricevute = GetStatsSMSRicevuti(filter);
            //    return ret;
            //}

            ///// <summary>
            ///// Restituisce le statistiche sugli sms inviati
            ///// </summary>
            ///// <param name="idUfficio"></param>
            ///// <param name="operatori"></param>
            ///// <param name="inizio"></param>
            ///// <param name="fine"></param>
            ///// <param name="ignoreRights"></param>
            ///// <returns></returns>
            //public CStatisticheOperazione GetOutSMSStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    //TODO GetOutSMSStats
            //    throw new NotImplementedException();

            //    //string dbSQL = "SELECT Sum([OutSMSNum]) As [Num], Sum([OutSMSTotLen]) As [TotLen], Min([OutSMSMinLen]) As [MinLen], Max([OutSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
            //    //for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
            //    //{
            //    //    if (i > 0)
            //    //        dbSQL += ", ";
            //    //    dbSQL += DBUtils.DBNumber(operatori[i]);
            //    //}

            //    //dbSQL += ")";
            //    //if (inizio.HasValue)
            //    //    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    //if (fine.HasValue)
            //    //    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
                
            //    //var ret = new CStatisticheOperazione();
            //    //using (var dbRis = CRM.TelDB.ExecuteReader(dbSQL))
            //    //{  
            //    //    if (dbRis.Read())
            //    //    {
            //    //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //    //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //    //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //    //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //    //    }

            //    //    return ret;
            //    //}
                 
            //}

            ///// <summary>
            ///// Restituisce le statistiche sugli sms ricevuti
            ///// </summary>
            ///// <param name="idUfficio"></param>
            ///// <param name="operatori"></param>
            ///// <param name="inizio"></param>
            ///// <param name="fine"></param>
            ///// <param name="ignoreRights"></param>
            ///// <returns></returns>
            //public CStatisticheOperazione GetInSMSStats(int idUfficio, int[] operatori, DateTime? inizio, DateTime? fine, bool ignoreRights = true)
            //{
            //    //TODO GetInSMSStats
            //    throw new NotImplementedException(); 
            //    //string dbSQL = "SELECT Sum([InSMSNum]) As [Num], Sum([InSMSTotLen]) As [TotLen], Min([InSMSMinLen]) As [MinLen], Max([InSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In (";
            //    //for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
            //    //{
            //    //    if (i > 0)
            //    //        dbSQL += ", ";
            //    //    dbSQL += DBUtils.DBNumber(operatori[i]);
            //    //}

            //    //dbSQL += ")";
            //    //if (inizio.HasValue)
            //    //    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(inizio.Value));
            //    //if (fine.HasValue)
            //    //    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DMD.DateUtils.GetDatePart(fine.Value));
                
            //    //var ret = new CStatisticheOperazione();
            //    //using (var dbRis = CRM.TelDB.ExecuteReader(dbSQL))
            //    //{  
            //    //    if (dbRis.Read())
            //    //    {
            //    //        ret.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
            //    //        ret.MinLen = Sistema.Formats.ToDouble(dbRis["MinLen"]);
            //    //        ret.MaxLen = Sistema.Formats.ToDouble(dbRis["MaxLen"]);
            //    //        ret.TotalLen = Sistema.Formats.ToDouble(dbRis["TotLen"]);
            //    //    }

            //    //    return ret;
            //    //}
                 
            //}

            ///// <summary>
            ///// Restituisce l'ultimo sms inviato
            ///// </summary>
            ///// <param name="pID"></param>
            ///// <returns></returns>
            //public SMSMessage GetUltimoSMSInCorso(int pID)
            //{
            //    if (pID == 0)
            //        return null;
            //    lock (m_inAttesaLock)
            //    {
            //        foreach (SMSMessage v in InAttesa)
            //        {
            //            if (v.IDPersona == pID)
            //                return v;
            //        }
            //    }

            //    return null;
            //}

            //private Dictionary<int , SMSMessage> InnerList
            //{
            //    get
            //    {
            //        if (m_InAttesa is null)
            //        {
            //            m_InAttesa = new Dictionary<int, SMSMessage>();
            //            using (var cursor = new SMSMessageCursor())
            //            {
            //                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //                cursor.IDAzienda.Value = DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale);
            //                cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //                cursor.StatoConversazione.Value = StatoConversazione.CONCLUSO;
            //                cursor.StatoConversazione.Operator = OP.OP_NE;
            //                cursor.IgnoreRights = true;
            //                while (cursor.Read())
            //                {
            //                    var item = cursor.Item;
            //                    m_InAttesa.Add(DBUtils.GetID(item), item);
            //                }

            //            }
            //        }

            //        return m_InAttesa;
            //    }
            //}

            ///// <summary>
            ///// Restituisce la collezione degli SMS in uscita
            ///// </summary>
            //public List<SMSMessage> InAttesa
            //{
            //    get
            //    {
            //        lock (m_inAttesaLock)
            //        {
            //            return new List(this.InnerList.Values);
            //        }
            //    }
            //}

            ///// <summary>
            ///// Aggiunge l'SMS in attesa
            ///// </summary>
            ///// <param name="item"></param>
            //public void SetInAttesa(SMSMessage item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InAttesa.GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InAttesa.Remove(oldItem);
            //        m_InAttesa.Add(item);
            //    }
            //}

            ///// <summary>
            ///// Rimuove l'SMS in attesa
            ///// </summary>
            ///// <param name="item"></param>
            //public void SetFineAttesa(SMSMessage item)
            //{
            //    lock (m_inAttesaLock)
            //    {
            //        var oldItem = InAttesa.GetItemById(DBUtils.GetID(item));
            //        if (oldItem is object)
            //            InAttesa.Remove(oldItem);
            //    }
            //}

            //public CCollection<SMSMessage> GetSMSRicevute(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? dataInizio, DateTime? dataFine)
            //{
            //    var ret = new CCollection<SMSMessage>();
            //    using (var cursor = new SMSMessageCursor())
            //    {
            //        if (ufficio is object)
            //            cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio, 0);
            //        if (operatore is object)
            //            cursor.IDOperatore.Value = DBUtils.GetID(operatore);
            //        if (dataInizio.HasValue)
            //        {
            //            if (dataFine.HasValue)
            //            {
            //                cursor.Data.Between(dataInizio.Value, dataFine.Value);
            //            }
            //            else
            //            {
            //                cursor.Data.Value = dataInizio.Value;
            //                cursor.Data.Operator = OP.OP_GE;
            //            }
            //        }
            //        else if (dataFine.HasValue)
            //        {
            //            cursor.Data.Value = dataFine.Value;
            //            cursor.Data.Operator = OP.OP_LE;
            //        }

            //        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //        cursor.Ricevuta.Value = true;
            //        while (cursor.Read())
            //        {
            //            ret.Add(cursor.Item);
            //        }

            //    }

            //    return ret;
            //}

            ///// <summary>
            ///// Restituisce l'ultimo SMS
            ///// </summary>
            ///// <param name="p"></param>
            ///// <returns></returns>
            //public SMSMessage GetUltimoSMS(Anagrafica.CPersona p)
            //{
            //    using (var cursor = new SMSMessageCursor())
            //    {
            //        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
            //        cursor.IDPersona.Value = DBUtils.GetID(p);
            //        cursor.Data.SortOrder = SortEnum.SORT_DESC;
            //        cursor.IgnoreRights = true;
            //        return cursor.Item;
            //    }
            //}
        }


    }

    public partial class CustomerCalls
    {
      
        private static CSMSClass m_SMS = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="SMSMessage"/>
        /// </summary>
        public static CSMSClass SMS
        {
            get
            {
                if (m_SMS is null)
                    m_SMS = new CSMSClass();
                return m_SMS;
            }
        }
    }
}