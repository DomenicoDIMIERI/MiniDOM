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
using System.Data;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Statistiche per operatore
        /// </summary>
        [Serializable]
        public class CRMStatisticheOperatore 
            : minidom.Databases.DBObjectBase, IComparable, IComparable<CRMStatisticheOperatore>, ICloneable
        {
            private int m_IDPuntoOperativo = 0;
            private string m_NomePuntoOperativo = "";
            private int m_IDOperatore = 0;
            private string m_NomeOperatore = "";
            private DateTime m_Data;
            private CKeyCollection<CStatisticheOperazione> m_Statistiche;  
            private bool m_Ricalcola = false;

           
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatisticheOperatore()
            {
                this.m_IDPuntoOperativo = 0;
                this.m_NomePuntoOperativo = "";
                this.m_IDOperatore = 0;
                this.m_NomeOperatore = "";
                //this.m_Data;
                this.m_Statistiche = new CKeyCollection<CStatisticheOperazione>();
                this.m_Ricalcola = false;
            }

             

          

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.Statistiche.StatisticheOperatore;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CRMStats";
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CRMStatisticheOperatore Clone()
            {
                return (CRMStatisticheOperatore) this._Clone();
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            protected override Databases.DBObjectBase _Clone()
            {
                var ret = new CRMStatisticheOperatore();
                ret.m_IDPuntoOperativo = this.m_IDPuntoOperativo;
                ret.m_NomePuntoOperativo = this.m_NomePuntoOperativo;
                ret.m_IDOperatore = this.m_IDOperatore;
                ret.m_NomeOperatore = this.m_NomeOperatore;
                ret.m_Data = this.m_Data;
                ret.m_Ricalcola = this.m_Ricalcola;
                foreach(var k in this.Statistiche.Keys)
                {
                    this.Statistiche.Add(k, this.Statistiche[k].Clone());
                }
                return ret;
            }

            /// <summary>
            /// Restituisce o imposta l'id del punto operativo
            /// </summary>
            public int IDPuntoOperativo
            {
                get
                {
                    return this.m_IDPuntoOperativo;
                }
                set
                {
                    var oldValue = this.IDPuntoOperativo;
                    if (oldValue == value) return;
                    this.m_IDPuntoOperativo = value;
                    this.DoChanged("IDPuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del punto operativo
            /// </summary>
            public string NomePuntoOperativo
            {
                get
                {
                    return this.m_NomePuntoOperativo;
                }
                set
                {
                    string oldValue = this.m_NomePuntoOperativo;
                    value = Strings.Trim(value);
                    if (Strings.EQ(value, oldValue)) return;
                    this.m_NomePuntoOperativo = value;
                    this.DoChanged("NomePuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'operatore
            /// </summary>
            public int IDOperatore
            {
                get
                {
                    return this.m_IDOperatore;
                }
                set
                {
                    var oldValue = this.IDOperatore;
                    if (oldValue == value) return;
                    this.m_IDOperatore = value;
                    this.DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore
            /// </summary>
            public string NomeOperatore
            {
                get
                {
                    return this.m_NomeOperatore;
                }
                set
                {
                    var oldValue = this.m_NomeOperatore;
                    value = Strings.Trim(value);
                    if (Strings.EQ(value, oldValue)) return;
                    this.m_NomeOperatore = value;
                    this.DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui sono calcolate le statistiche (solo dd/MM/yyyy)
            /// </summary>
            public DateTime Data
            {
                get
                {
                    return this.m_Data;
                }
                set
                {
                    var oldValue = this.m_Data;
                    value = DMD.DateUtils.GetDatePart(value);
                    if (DMD.DateUtils.EQ(value, oldValue)) return;
                    this.m_Data = value;
                    this.DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se le statistiche devono essere ricalcolate
            /// </summary>
            public bool Ricalcola
            {
                get
                {
                    return this.m_Ricalcola;
                }
                set
                {
                    if (this.m_Ricalcola == value) return;
                    this.m_Ricalcola = value;
                    this.DoChanged("Ricalcola", value, !value);
                }
            }

            /// <summary>
            /// Restituisce la collezione delle statistiche per tipo di oggetto
            /// </summary>
            public CKeyCollection<CStatisticheOperazione> Statistiche
            {
                get
                {
                    if (this.m_Statistiche is null)
                        this.m_Statistiche = new CKeyCollection<CStatisticheOperazione>();
                    return this.m_Statistiche;
                }
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || DBUtils.IsChanged(this.Statistiche);
            }

            /// <summary>
            /// Imposta il valore changed
            /// </summary>
            /// <param name="value"></param>
            public override void SetChanged(bool value = true)
            {
                base.SetChanged(value);
                if (!value) 
                    DBUtils.SetChanged(this.Statistiche, false);
            }


            /// <summary>
            /// Aggregatore
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public CRMStatisticheOperatore AggregaCon(CRMStatisticheOperatore item)
            {
                var ret = this.Clone();

                ret.Data = DMD.DateUtils.Max(this.Data, item.Data);
                Dictionary<string, string> common = new Dictionary<string, string>();
                foreach(var k in this.Statistiche.Keys)
                {
                    var altro = item.Statistiche.GetItemByKey(k);
                    if (altro is object)
                    {
                        ret.Statistiche[k].AggregaCon(altro);
                        common.Add(k, k);
                    }                    
                }
                foreach(var k in item.Statistiche.Keys)
                {
                    if (!common.ContainsKey(k))
                    {
                        var altro = item.Statistiche[k];
                        ret.Statistiche.Add(altro.Clone());
                    }
                }
                ret.Ricalcola = false;
                return ret;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDPuntoOperativo = reader.Read("IDPuntoOperativo", this.m_IDPuntoOperativo);
                this.m_NomePuntoOperativo = reader.Read("NomePuntoOperativo", this.m_NomePuntoOperativo);
                this.m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                this.m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                this.m_Data = reader.Read("Data", this.m_Data);
                this.m_Ricalcola = reader.Read("Ricalcola", this.m_Ricalcola);
                string tmp = reader.Read("Statistiche", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.m_Statistiche = new CKeyCollection<CStatisticheOperazione>();
                    CKeyCollection col = DMD.XML.Utils.Deserialize<CKeyCollection>(tmp);
                    foreach(var k in col.Keys )
                    {
                        this.Statistiche.Add(k, (CStatisticheOperazione)col[k]);
                    }
                }
                //InCallNum = reader.Read("InCallNum", InCallNum);
                //InCallMinLen = reader.Read("InCallMinLen",  InCallMinLen);
                //InCallMaxLen = reader.Read("InCallMaxLen",  InCallMaxLen);
                //InCallTotLen = reader.Read("InCallTotLen",  InCallTotLen);
                //InCallMinWait = reader.Read("InCallMinWait",  InCallMinWait);
                //InCallMaxWait = reader.Read("InCallMaxWait",  InCallMaxWait);
                //InCallTotWait = reader.Read("InCallTotWait",  InCallTotWait);
                //InCallCost = reader.Read("InCallCost",  InCallCost);
                //OutCallNum = reader.Read("OutCallNum",  OutCallNum);
                //OutCallMinLen = reader.Read("OutCallMinLen",  OutCallMinLen);
                //OutCallMaxLen = reader.Read("OutCallMaxLen",  OutCallMaxLen);
                //OutCallTotLen = reader.Read("OutCallTotLen",  OutCallTotLen);
                //OutCallMinWait = reader.Read("OutCallMinWait",  OutCallMinWait);
                //OutCallMaxWait = reader.Read("OutCallMaxWait",  OutCallMaxWait);
                //OutCallTotWait = reader.Read("OutCallTotWait",  OutCallTotWait);
                //OutCallCost = reader.Read("OutCallCost",  OutCallCost);
                //InDateNum = reader.Read("InDateNum",  InDateNum);
                //InDateMinLen = reader.Read("InDateMinLen",  InDateMinLen);
                //InDateMaxLen = reader.Read("InDateMaxLen",  InDateMaxLen);
                //InDateTotLen = reader.Read("InDateTotLen",  InDateTotLen);
                //InDateMinWait = reader.Read("InDateMinWait",  InDateMinWait);
                //InDateMaxWait = reader.Read("InDateMaxWait",  InDateMaxWait);
                //InDateTotWait = reader.Read("InDateTotWait",  InDateTotWait);
                //InDateCost = reader.Read("InDateCost",  InDateCost);
                //OutDateNum = reader.Read("OutDateNum",  OutDateNum);
                //OutDateMinLen = reader.Read("OutDateMinLen",  OutDateMinLen);
                //OutDateMaxLen = reader.Read("OutDateMaxLen",  OutDateMaxLen);
                //OutDateTotLen = reader.Read("OutDateTotLen",  OutDateTotLen);
                //OutDateMinWait = reader.Read("OutDateMinWait",  OutDateMinWait);
                //OutDateMaxWait = reader.Read("OutDateMaxWait",  OutDateMaxWait);
                //OutDateTotWait = reader.Read("OutDateTotWait",  OutDateTotWait);
                //OutDateCost = reader.Read("OutDateCost",  OutDateCost);
                //InSMSNum = reader.Read("InSMSNum",  InSMSNum);
                //InSMSMinLen = reader.Read("InSMSMinLen",  InSMSMinLen);
                //InSMSMaxLen = reader.Read("InSMSMaxLen",  InSMSMaxLen);
                //InSMSTotLen = reader.Read("InSMSTotLen",  InSMSTotLen);
                //InSMSCost = reader.Read("InSMSCost",  InSMSCost);
                //OutSMSNum = reader.Read("OutSMSNum",  OutSMSNum);
                //OutSMSMinLen = reader.Read("OutSMSMinLen",  OutSMSMinLen);
                //OutSMSMaxLen = reader.Read("OutSMSMaxLen",  OutSMSMaxLen);
                //OutSMSTotLen = reader.Read("OutSMSTotLen",  OutSMSTotLen);
                //OutSMSCost = reader.Read("OutSMSCost",  OutSMSCost);
                //InFAXNum = reader.Read("InFAXNum",  InFAXNum);
                //InFAXMinLen = reader.Read("InFAXMinLen",  InFAXMinLen);
                //InFAXMaxLen = reader.Read("InFAXMaxLen",  InFAXMaxLen);
                //InFAXTotLen = reader.Read("InFAXTotLen",  InFAXTotLen);
                //InFAXCost = reader.Read("INFAXCost",  InFAXCost);
                //OutFAXNum = reader.Read("OutFAXNum",  OutFAXNum);
                //OutFAXMinLen = reader.Read("OutFAXMinLen",  OutFAXMinLen);
                //OutFAXMaxLen = reader.Read("OutFAXMaxLen",  OutFAXMaxLen);
                //OutFAXTotLen = reader.Read("OutFAXTotLen",  OutFAXTotLen);
                //OutFAXCost = reader.Read("OutFAXCost",  OutFAXCost);
                //InEMAILNum = reader.Read("InEMAILNum",  InEMAILNum);
                //InEMAILMinLen = reader.Read("InEMAILMinLen",  InEMAILMinLen);
                //InEMAILMaxLen = reader.Read("InEMAILMaxLen",  InEMAILMaxLen);
                //InEMAILTotLen = reader.Read("InEMAILTotLen",  InEMAILTotLen);
                //OutEMAILNum = reader.Read("OutEMAILNum",  OutEMAILNum);
                //OutEMAILMinLen = reader.Read("OutEMAILMinLen",  OutEMAILMinLen);
                //OutEMAILMaxLen = reader.Read("OutEMAILMaxLen",  OutEMAILMaxLen);
                //OutEMAILTotLen = reader.Read("OutEMAILTotLen",  OutEMAILTotLen);
                //InTelegramNum = reader.Read("InTelegramNum",  InTelegramNum);
                //InTelegramMinLen = reader.Read("InTelegramMinLen",  InTelegramMinLen);
                //InTelegramMaxLen = reader.Read("InTelegramMaxLen",  InTelegramMaxLen);
                //InTelegramTotLen = reader.Read("InTelegramTotLen",  InTelegramTotLen);
                //InTelegramCost = reader.Read("InTelegramCost",  InTelegramCost);
                //OutTelegramNum = reader.Read("OutTelegramNum",  OutTelegramNum);
                //OutTelegramMinLen = reader.Read("OutTelegramMinLen",  OutTelegramMinLen);
                //OutTelegramMaxLen = reader.Read("OutTelegramMaxLen",  OutTelegramMaxLen);
                //OutTelegramTotLen = reader.Read("OutTelegramTotLen",  OutTelegramTotLen);
                //OutTelegramCost = reader.Read("OutTelegramCost",  OutTelegramCost);
                //NumAppuntamentiPrevisti = reader.Read("NumAppuntamentiPrevisti",  NumAppuntamentiPrevisti);
                //NumTelefonatePreviste = reader.Read("NumTelefonatePreviste",  NumTelefonatePreviste);

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPuntoOperativo", this.m_IDPuntoOperativo);
                writer.Write("NomePuntoOperativo", this.m_NomePuntoOperativo);
                writer.Write("IDOperatore", this.m_IDOperatore);
                writer.Write("NomeOperatore", this.m_NomeOperatore);
                writer.Write("Data", this.m_Data);
                writer.Write("Ricalcola", this.m_Ricalcola);
                writer.Write("Statistiche", DMD.XML.Utils.Serialize(this.Statistiche));

                //writer.Write("InCallNum", InCallNum);
                //writer.Write("InCallMinLen", InCallMinLen);
                //writer.Write("InCallMaxLen", InCallMaxLen);
                //writer.Write("InCallTotLen", InCallTotLen);
                //writer.Write("InCallMinWait", InCallMinWait);
                //writer.Write("InCallMaxWait", InCallMaxWait);
                //writer.Write("InCallTotWait", InCallTotWait);
                //writer.Write("InCallCost", InCallCost);
                //writer.Write("OutCallNum", OutCallNum);
                //writer.Write("OutCallMinLen", OutCallMinLen);
                //writer.Write("OutCallMaxLen", OutCallMaxLen);
                //writer.Write("OutCallTotLen", OutCallTotLen);
                //writer.Write("OutCallMinWait", OutCallMinWait);
                //writer.Write("OutCallMaxWait", OutCallMaxWait);
                //writer.Write("OutCallTotWait", OutCallTotWait);
                //writer.Write("OutCallCost", OutCallCost);
                //writer.Write("InDateNum", InDateNum);
                //writer.Write("InDateMinLen", InDateMinLen);
                //writer.Write("InDateMaxLen", InDateMaxLen);
                //writer.Write("InDateTotLen", InDateTotLen);
                //writer.Write("InDateMinWait", InDateMinWait);
                //writer.Write("InDateMaxWait", InDateMaxWait);
                //writer.Write("InDateTotWait", InDateTotWait);
                //writer.Write("InDateCost", InDateCost);
                //writer.Write("OutDateNum", OutDateNum);
                //writer.Write("OutDateMinLen", OutDateMinLen);
                //writer.Write("OutDateMaxLen", OutDateMaxLen);
                //writer.Write("OutDateTotLen", OutDateTotLen);
                //writer.Write("OutDateMinWait", OutDateMinWait);
                //writer.Write("OutDateMaxWait", OutDateMaxWait);
                //writer.Write("OutDateTotWait", OutDateTotWait);
                //writer.Write("OutDateCost", OutDateCost);
                //writer.Write("InSMSNum", InSMSNum);
                //writer.Write("InSMSMinLen", InSMSMinLen);
                //writer.Write("InSMSMaxLen", InSMSMaxLen);
                //writer.Write("InSMSTotLen", InSMSTotLen);
                //writer.Write("InSMSCost", InSMSCost);
                //writer.Write("OutSMSNum", OutSMSNum);
                //writer.Write("OutSMSMinLen", OutSMSMinLen);
                //writer.Write("OutSMSMaxLen", OutSMSMaxLen);
                //writer.Write("OutSMSTotLen", OutSMSTotLen);
                //writer.Write("OutSMSCost", OutSMSCost);
                //writer.Write("InFAXNum", InFAXNum);
                //writer.Write("InFAXMinLen", InFAXMinLen);
                //writer.Write("InFAXMaxLen", InFAXMaxLen);
                //writer.Write("InFAXTotLen", InFAXTotLen);
                //writer.Write("InFAXCost", InFAXCost);
                //writer.Write("OutFAXNum", OutFAXNum);
                //writer.Write("OutFAXMinLen", OutFAXMinLen);
                //writer.Write("OutFAXMaxLen", OutFAXMaxLen);
                //writer.Write("OutFAXTotLen", OutFAXTotLen);
                //writer.Write("OutFAXCost", OutFAXCost);
                //writer.Write("InEMAILNum", InEMAILNum);
                //writer.Write("InEMAILMinLen", InEMAILMinLen);
                //writer.Write("InEMAILMaxLen", InEMAILMaxLen);
                //writer.Write("InEMAILTotLen", InEMAILTotLen);
                //writer.Write("OutEMAILNum", OutEMAILNum);
                //writer.Write("OutEMAILMinLen", OutEMAILMinLen);
                //writer.Write("OutEMAILMaxLen", OutEMAILMaxLen);
                //writer.Write("OutEMAILTotLen", OutEMAILTotLen);
                //writer.Write("InTelegramNum", InTelegramNum);
                //writer.Write("InTelegramMinLen", InTelegramMinLen);
                //writer.Write("InTelegramMaxLen", InTelegramMaxLen);
                //writer.Write("InTelegramTotLen", InTelegramTotLen);
                //writer.Write("InTelegramCost", InTelegramCost);
                //writer.Write("OutTelegramNum", OutTelegramNum);
                //writer.Write("OutTelegramMinLen", OutTelegramMinLen);
                //writer.Write("OutTelegramMaxLen", OutTelegramMaxLen);
                //writer.Write("OutTelegramTotLen", OutTelegramTotLen);
                //writer.Write("OutTelegramCost", OutTelegramCost);
                //writer.Write("NumAppuntamentiPrevisti", NumAppuntamentiPrevisti);
                //writer.Write("NumTelefonatePreviste", NumTelefonatePreviste);

                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPuntoOperativo", typeof(int), 1);
                c = table.Fields.Ensure("NomePuntoOperativo", typeof(string), 255);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("Ricalcola", typeof(bool), 1);
                c = table.Fields.Ensure("Statistiche", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOperatore", new string[] { "IDPuntoOperativo", "IDOperatore", "Data" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxAltro", new string[] { "Ricalcola", "NomeOperatore", "NomePuntoOperativo" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Statistiche", typeof(string), 0);
            }

            /// <summary>
            /// Notifica un nuovo contatto nella giornata
            /// </summary>
            /// <param name="c"></param>
            public void NotifyNew(CContattoUtente c)
            {
                if (c is null)
                    throw new ArgumentNullException("c");
                if (
                       c.IDOperatore != this.IDOperatore
                    || c.IDPuntoOperativo != this.IDPuntoOperativo
                    || !DMD.DateUtils.EQ(DMD.DateUtils.GetDatePart(c.Data), this.Data)
                    )
                    return;

                var key = DMD.RunTime.vbTypeName(c) + "_" + (c.Ricevuta ? "IN" : "OUT");
                var item = this.Statistiche.GetItemByKey(key);
                if (item is null)
                {
                    item = new CStatisticheOperazione();
                    this.Statistiche.Add(key, item);
                }

                item.MinLen = DMD.Maths.Min(item.MinLen, c.Durata);
                item.MaxLen = DMD.Maths.Max(item.MaxLen, c.Durata);
                item.MinWait = DMD.Maths.Min(item.MinWait, c.Attesa);
                item.MaxWait = DMD.Maths.Max(item.MaxWait, c.Attesa);
                item.TotalLen += c.Durata;
                item.TotalWait += c.Attesa;
                item.TotalCost += c.Costo ?? 0;
                item.Numero += 1;

                this.SetChanged(true);
            }

            /// <summary>
            /// Ricalcola le statistiche sulla base dell'effettivo contenuto delle tabelle
            /// </summary>
            /// <remarks></remarks>
            public void Aggiorna()
            {
                var toDate = DMD.DateUtils.DateAdd(DateTimeInterval.Day, 1d, Data);
                using (var cursor = new CContattoUtenteCursor())
                {
                    this.Statistiche.Clear();
                    while (cursor.Read())
                    {
                        this.NotifyNew(cursor.Item);
                    }
                }
                this.m_Ricalcola = false;
                this.Save(true);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("NomePuntoOperativo", NomePuntoOperativo);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", NomeOperatore);
                writer.WriteAttribute("Data", Data);
                writer.WriteAttribute("Ricalcola", Ricalcola);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPuntoOperativo":
                        {
                            IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePuntoOperativo":
                        {
                            NomePuntoOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                            
                    case "Ricalcola":
                        this.Ricalcola = (bool) DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue);
                        break;                         

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce le statistiche 
            /// </summary>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            public static CRMStatisticheOperatore GetStats(
                                    Anagrafica.CUfficio ufficio, 
                                    Sistema.CUser operatore, 
                                    DateTime data
                                    )
            {
                data = (DateTime)DMD.DateUtils.GetDatePart(data);
                CRMStatisticheOperatore item;

                var db = minidom.CustomerCalls.Statistiche.StatisticheOperatore.Database;

                using (var dbRis = (IDataReader) 
                               db.SELECT().
                                  FROM("tbl_CRMStats").
                                  WHERE(
                                       DBCursorField.Field("IDPuntoOperativo").EQ(DBUtils.GetID(ufficio, 0))
                                    * DBCursorField.Field("IDOperatore").EQ(DBUtils.GetID(operatore, 0))
                                    * DBCursorField.Field("Data").EQ(data)
                                    ))
                {
                    if (dbRis.Read())
                    {
                        item = new CRMStatisticheOperatore();
                        DBUtils.Read(dbRis, item);
                    }
                    else
                    {
                        item = new CRMStatisticheOperatore();
                        item.IDPuntoOperativo = DBUtils.GetID(ufficio, 0);
                        if (ufficio is object)
                            item.NomePuntoOperativo = ufficio.Nome;
                        item.IDOperatore = DBUtils.GetID(operatore, 0);
                        item.NomeOperatore = operatore.Nominativo;
                        item.Data = data;
                        // item.Save()
                    }

                    return item;
                }
            }

            /// <summary>
            /// Confronta due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CRMStatisticheOperatore other)
            {
                int ret = DMD.DateUtils.Compare(this.m_Data, other.m_Data);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_NomeOperatore, other.m_NomeOperatore, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_NomePuntoOperativo, other.m_NomePuntoOperativo, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CRMStatisticheOperatore)obj);
            }
        }
    }
}