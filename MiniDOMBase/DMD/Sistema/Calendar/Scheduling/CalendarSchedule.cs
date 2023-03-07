using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Tipo di riptetitività dell'evento
        /// </summary>
        public enum ScheduleType : int
        {
            /// <summary>
            /// L'evento viene eseguito una sola volta all'istante pianificato
            /// </summary>
            /// <remarks></remarks>
            UNA_VOLTA = 0,

            /// <summary>
            /// L'evento viene eseguito ogni N giorni all'orario pianificato
            /// </summary>
            /// <remarks></remarks>
            OGNI_N_GIORNI = 1,

            /// <summary>
            /// L'evento viene eseguito ogni N settimane nel giorno e all'orario specificato
            /// </summary>
            /// <remarks></remarks>
            OGNI_N_SETTIMANE = 2,

            /// <summary>
            /// L'evento viene eseguito ogni mese nel giorno e all'orario specificato
            /// </summary>
            /// <remarks></remarks>
            OGNI_N_MESI = 3,

            /// <summary>
            /// L'evento viene eseguito ogni anno nel giorno e all'orario specificato
            /// </summary>
            /// <remarks></remarks>
            OGNI_N_ANNI = 4,

            /// <summary>
            /// L'evento viene eseguito ogni ultimo giorno del mese
            /// </summary>
            /// <remarks></remarks>
            ULTIMO_DEL_MESE = 5,

            /// <summary>
            /// L'evento viene eseguito ogni primo [lun, ..., dom] del mese
            /// </summary>
            /// <remarks></remarks>
            PRIMO_X_DEL_MESE = 6,

            /// <summary>
            /// L'evento viene eseguito ogni ultimo [lun, ..., dom] del mese
            /// </summary>
            /// <remarks></remarks>
            ULTIMO_X_DEL_MESE = 7,

            /// <summary>
            /// L'evento viene eseguito ogni primo [lun, ..., dom] dell'anno
            /// </summary>
            /// <remarks></remarks>
            PRIMO_X_DELL_ANNO = 8,

            /// <summary>
            /// L'evento viene eseguito ogni ultimo [lun, ..., dom] dell'anno
            /// </summary>
            /// <remarks></remarks>
            ULTIMO_X_DELL_ANNO = 9,

            /// <summary>
            /// L'evento viene eseguito ogni n minuti
            /// </summary>
            OGNI_N_MINUTI = 10,

            /// <summary>
            /// L'evento viene eseguito ogni n ore
            /// </summary>
            OGNI_N_ORE = 11
        }

        /// <summary>
        /// Classe che rappresenta una programmazione temporale
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CalendarSchedule : Databases.DBObject
        {
            private ScheduleType m_ScheduleType;
            private DateTime m_DataInizio;
            private DateTime? m_DataFine;
            private double m_Intervallo;
            private int m_Ripetizioni;
            private DateTime? m_UltimaEsecuzione;
            private int m_ConteggioEsecuzioni;
            [NonSerialized] private ISchedulable m_Owner;
            private string m_OwnerType;
            private int m_OwnerID;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CalendarSchedule()
            {
                m_ScheduleType = ScheduleType.UNA_VOLTA;
                m_DataInizio = default;
                m_DataFine = default;
                m_Intervallo = 1.0;
                m_Ripetizioni = 0; // Infinite
                m_Owner = null;
                m_OwnerType = "";
                m_OwnerID = 0;
                m_UltimaEsecuzione = default;
                m_ConteggioEsecuzioni = 0;
                
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="dataInizio"></param>
            /// <param name="intervallo"></param>
            /// <param name="ripetizioni"></param>
            public CalendarSchedule(ScheduleType tipo, DateTime dataInizio, double intervallo = 1.0, int ripetizioni = 0)
                : this()
            {
                m_ScheduleType = tipo;
                m_DataInizio = dataInizio;
                m_DataFine = default;
                m_Intervallo = intervallo;
                m_Ripetizioni = ripetizioni;
                m_Owner = null;
                m_OwnerType = "";
                m_OwnerID = 0;
                m_UltimaEsecuzione = default;
                m_ConteggioEsecuzioni = 0;
            }


             
            /// <summary>
            /// Restituisce o imposta un valore che indica il tipo di programmazione dell'evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public ScheduleType ScheduleType
            {
                get
                {
                    return m_ScheduleType;
                }

                set
                {
                    var oldValue = m_ScheduleType;
                    if (oldValue == value)
                        return;
                    m_ScheduleType = value;
                    DoChanged("ScheduleType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio dell'evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data finale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'intervallo tra gli eventi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Intervallo
            {
                get
                {
                    return m_Intervallo;
                }

                set
                {
                    if (value < 0.0001d)
                        throw new ArgumentOutOfRangeException("N deve essere positivo");
                    var oldValue = m_Intervallo;
                    if (oldValue == value)
                        return;
                    m_Intervallo = value;
                    DoChanged("Intervallo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di ripetizioni da effettuare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int NumeroRipetizioni
            {
                get
                {
                    return m_Ripetizioni;
                }

                set
                {
                    int oldValue = m_Ripetizioni;
                    if (oldValue == value)
                        return;
                    m_Ripetizioni = value;
                    DoChanged("NumeroRipetizioni", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto a cui appartiene la schedulazione
            /// </summary>
            public object Owner
            {
                get
                {
                    if (m_Owner is null)
                        m_Owner = (ISchedulable) mindom.Sistema.ApplicationContext.GetItemByTypeAndId(m_OwnerType, m_OwnerID);
                    return m_Owner;
                }

                set
                {
                    object oldValue = m_Owner;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Owner = (ISchedulable)value;
                    m_OwnerID = DBUtils.GetID(value, 0);
                    m_OwnerType = DMD.RunTime.vbTypeName(value);
                    DoChanged("Owner", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'oggetto a cui appartiene la schedulazione
            /// </summary>
            /// <param name="value"></param>
            internal void SetOwner(object value)
            {
                m_Owner = (ISchedulable)value;
                m_OwnerID = DBUtils.GetID(value, 0);
                m_OwnerType = DMD.RunTime.vbTypeName(value);
            }

            /// <summary>
            /// Restituisce o imposta il tipo di oggetto a cui appartiene la schedulazione
            /// </summary>
            public string OwnerType
            {
                get
                {
                    return m_OwnerType;
                }

                set
                {
                    string oldValue = m_OwnerType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_OwnerType = value;
                    DoChanged("OwnerType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'oggetto a cui appartiene la schedulazione
            /// </summary>
            public int OwnerID
            {
                get
                {
                    return DBUtils.GetID(m_Owner, m_OwnerID);
                }

                set
                {
                    int oldValue = OwnerID;
                    if (oldValue == value)
                        return;
                    m_OwnerID = value;
                    DoChanged("OwnerID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultima esecuzione
            /// </summary>
            public DateTime? UltimaEsecuzione
            {
                get
                {
                    return m_UltimaEsecuzione;
                }

                set
                {
                    var oldValue = m_UltimaEsecuzione;
                    if (oldValue == value == true)
                        return;
                    m_UltimaEsecuzione = value;
                    DoChanged("UltimaEsecuzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di esecuzioni
            /// </summary>
            public int ConteggioEsecuzioni
            {
                get
                {
                    return m_ConteggioEsecuzioni;
                }

                set
                {
                    int oldValue = m_ConteggioEsecuzioni;
                    if (oldValue == value)
                        return;
                    m_ConteggioEsecuzioni = value;
                    DoChanged("ConteggioEsecuzioni", value, oldValue);
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Calendar.ScheduledTasks; //.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CalendarSchedules";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                if (m_Ripetizioni > 0)
                {
                    ret.Append("Ripeti per ");
                    ret.Append(m_Ripetizioni);
                    ret.Append(" volte");
                }
                if (m_Intervallo > 1.0)
                {
                    ret.Append(" ogni");
                    ret.Append(m_Intervallo);
                }

                switch (ScheduleType)
                {
                    case ScheduleType.OGNI_N_ANNI:
                        {
                            ret.Append((m_Intervallo > 1f)? " anni" : " anno");
                            break;
                        }

                    case ScheduleType.OGNI_N_MESI:
                        {
                            ret.Append ((m_Intervallo > 1f)? " mesi": " mese");
                            break;
                        }

                    case ScheduleType.OGNI_N_GIORNI:
                        {
                            ret.Append((m_Intervallo > 1f)? " giorni": " giorno");
                            break;
                        }

                    case ScheduleType.OGNI_N_MINUTI:
                        {
                            ret.Append( (m_Intervallo > 1f)? " minuti" : " minuto");
                            break;
                        }

                    case ScheduleType.OGNI_N_ORE:
                        {
                            ret.Append( (m_Intervallo > 1f)? " ore" : " ora");
                            break;
                        }

                    case ScheduleType.OGNI_N_SETTIMANE:
                        {
                            ret.Append( (m_Intervallo > 1f)? " settimane" : " settimana");
                            break;
                        }

                    case ScheduleType.UNA_VOLTA:
                        {
                            ret.Clear();
                            ret.Append("Esegui una volta il ");
                            ret.Append(Formats.FormatUserDate(m_DataInizio));
                            ret.Append(" alle ");
                            ret.Append(Formats.FormatUserTime(m_DataInizio));
                            return ret.ToString();
                        }

                    default:
                        {
                            throw new NotImplementedException();                             
                        }
                }

                ret.Append(" a partire dal ");
                ret.Append(Formats.FormatUserDate(m_DataInizio));
                ret.Append(" alle ");
                ret.Append(Formats.FormatUserTime(m_DataInizio));

                if (m_DataFine.HasValue)
                {
                    ret.Append(" e non oltre il ");
                    ret.Append(Formats.FormatUserDate(m_DataFine));
                    ret.Append(" alle ");
                    ret.Append(Formats.FormatUserTime(m_DataFine));
                }

                return ret.ToString();
            }

            /// <summary>
            /// OnAfterSave
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                base.OnAfterSave(e);
                if (this.Owner is object)
                    m_Owner.NotifySchedule(this);                 
            }

            /// <summary>
            /// OnAfterSave
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterDelete(DMDEventArgs e)
            {
                base.OnAfterDelete(e);
                if (this.Owner is object)
                    m_Owner.NotifySchedule(this);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_OwnerType = reader.Read("OwnerType", m_OwnerType);
                m_OwnerID = reader.Read("OwnerID", m_OwnerID);
                m_ScheduleType = reader.Read("ScheduleType", m_ScheduleType);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                m_Intervallo = reader.Read("Intervallo", m_Intervallo);
                m_Ripetizioni = reader.Read("Ripetizioni", m_Ripetizioni);
                m_UltimaEsecuzione = reader.Read("UltimaEsecuzione", m_UltimaEsecuzione);
                m_ConteggioEsecuzioni = reader.Read("ConteggioEsecuzioni", m_ConteggioEsecuzioni);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("OwnerType", m_OwnerType);
                writer.Write("OwnerID", OwnerID);
                writer.Write("ScheduleType", m_ScheduleType);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Intervallo", m_Intervallo);
                writer.Write("Ripetizioni", m_Ripetizioni);
                writer.Write("UltimaEsecuzione", m_UltimaEsecuzione);
                writer.Write("ConteggioEsecuzioni", m_ConteggioEsecuzioni);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("OwnerType", typeof(string), 255);
                c = table.Fields.Ensure("OwnerID", typeof(int), 1);
                c = table.Fields.Ensure("ScheduleType", typeof(int), 1);
                c = table.Fields.Ensure("DataInizio", typeof(int), 1);
                c = table.Fields.Ensure("DataFine", typeof(int), 1);
                c = table.Fields.Ensure("Intervallo", typeof(double), 1);
                c = table.Fields.Ensure("Ripetizioni", typeof(int), 1);
                c = table.Fields.Ensure("UltimaEsecuzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("ConteggioEsecuzioni", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOwner", new string[] { "OwnerType", "OwnerID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSchedule", new string[] { "ScheduleType", "Ripetizioni", "Intervallo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEsecuzione", new string[] { "UltimaEsecuzione", "ConteggioEsecuzioni" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OwnerType", m_OwnerType);
                writer.WriteAttribute("OwnerID", OwnerID);
                writer.WriteAttribute("ScheduleType", (int?)m_ScheduleType);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Intervallo", m_Intervallo);
                writer.WriteAttribute("Ripetizioni", m_Ripetizioni);
                writer.WriteAttribute("UltimaEsecuzione", m_UltimaEsecuzione);
                writer.WriteAttribute("ConteggioEsecuzioni", m_ConteggioEsecuzioni);
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
                    case "OwnerType":
                        {
                            m_OwnerType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "OwnerID":
                        {
                            m_OwnerID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ScheduleType":
                        {
                            m_ScheduleType = (ScheduleType)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Intervallo":
                        {
                            m_Intervallo = (float)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Ripetizioni":
                        {
                            m_Ripetizioni = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UltimaEsecuzione":
                        {
                            m_UltimaEsecuzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ConteggioEsecuzioni":
                        {
                            m_ConteggioEsecuzioni = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        } 

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce la prossima data di esecuzione
            /// </summary>
            /// <param name="tipoPeriodo"></param>
            /// <param name="periodo"></param>
            /// <param name="dataIniziale"></param>
            /// <param name="ultimaEsecuzione"></param>
            /// <returns></returns>
            public DateTime GetNextDate(
                                DMD.DateTimeInterval tipoPeriodo, 
                                int periodo, 
                                DateTime dataIniziale, 
                                DateTime? ultimaEsecuzione
                                )
            {
                DateTime d;
                if (ultimaEsecuzione.HasValue)
                {
                    d = ultimaEsecuzione.Value;
                    // While (d < dataCorrente)
                    d = DMD.DateUtils.DateAdd(tipoPeriodo, periodo, d);
                }
                // End While
                else
                {
                    d = dataIniziale;
                    // While (d < dataCorrente)
                    // d = DateAdd(tipoPeriodo, periodo, d)
                    // End While
                }

                return d;
            }

            /// <summary>
            /// Calcola il numero di esecuzioni tra le date specificate
            /// </summary>
            /// <param name="tipoPeriodo"></param>
            /// <param name="periodo"></param>
            /// <param name="dallaData"></param>
            /// <param name="allaData"></param>
            /// <returns></returns>
            public int CalcRuns(
                        DateTimeInterval tipoPeriodo, 
                        int periodo, 
                        DateTime dallaData, 
                        DateTime allaData
                        )
            {
                var d = dallaData;
                int n = 0;
                while (d < allaData)
                {
                    d = DMD.DateUtils.DateAdd(tipoPeriodo, periodo, d);
                    n += 1;
                }

                return n;
            }

            /// <summary>
            /// Calcola la prossima esecuzione
            /// </summary>
            /// <returns></returns>
            public DateTime? CalcolaProssimaEsecuzione()
            {
                DateTime? ret = default;
                switch (ScheduleType)
                {
                    case ScheduleType.OGNI_N_MINUTI:
                        {
                            ret = GetNextDate(DateTimeInterval.Minute, (int)Intervallo, DataInizio, UltimaEsecuzione);
                            break;
                        }

                    case ScheduleType.OGNI_N_ORE:
                        {
                            ret = GetNextDate(DateTimeInterval.Hour, (int)Intervallo, DataInizio, UltimaEsecuzione);
                            break;
                        }

                    case ScheduleType.OGNI_N_ANNI:
                        {
                            ret = GetNextDate(DateTimeInterval.Year, (int)Intervallo, DataInizio, UltimaEsecuzione);
                            break;
                        }

                    case ScheduleType.OGNI_N_GIORNI:
                        {
                            ret = GetNextDate(DateTimeInterval.Day, (int)Intervallo, DataInizio, UltimaEsecuzione);
                            break;
                        }

                    case ScheduleType.OGNI_N_MESI:
                        {
                            ret = GetNextDate(DateTimeInterval.Month, (int)Intervallo, DataInizio, UltimaEsecuzione);
                            break;
                        }

                    case ScheduleType.OGNI_N_SETTIMANE:
                        {
                            ret = GetNextDate(DateTimeInterval.Weekday, (int)Intervallo, DataInizio, UltimaEsecuzione);
                            break;
                        }

                    case ScheduleType.UNA_VOLTA:
                        {
                            if (UltimaEsecuzione.HasValue)
                            {
                                ret = (DMD.DateUtils.Compare(DataInizio , UltimaEsecuzione.Value)<=0)? default : DataInizio;
                            }
                            else
                            {
                                ret = DataInizio;
                                // Case Calendar.ScheduleType.PRIMO_X_DEL_MESE
                                // d = Me.Programmazione.DataInizio.Value
                                // While (d < dallaData)
                                // d = DateAdd(DateTimeInterval.Year, Me.Programmazione.N, d)
                                // End While
                                // If Me.m_UltimaEsecuzione.HasValue Then
                                // While (Me.m_UltimaEsecuzione.Value > d)
                                // d = DateAdd(DateTimeInterval.Year, Me.Programmazione.N, d)
                                // End While
                                // End If
                                // Return d
                                // Case Calendar.ScheduleType.PRIMO_X_DELL_ANNO
                                // Case Calendar.ScheduleType.ULTIMO_DEL_MESE
                                // Case Calendar.ScheduleType.ULTIMO_X_DEL_MESE
                                // Case Calendar.ScheduleType.ULTIMO_X_DELL_ANNO
                            }

                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException("ScheduleType");
                            
                        }
                }

                if (m_DataFine.HasValue && ret.HasValue)
                {
                    if (ret.Value > m_DataFine == true)
                        ret = default;
                }

                if (m_Ripetizioni > 0 && ret.HasValue)
                {
                    if (m_ConteggioEsecuzioni >= m_Ripetizioni)
                        ret = default;
                }

                return ret;
            }

            // Public Function CalcolaNumeroEsecuzioni(ByVal dallaData As Date, ByVal allaData As Date) As Integer
            // Select Case Me.ScheduleType
            // Case Calendar.ScheduleType.OGNI_N_MINUTI : Return CalcRuns(DateTimeInterval.Minute, Me.Intervallo, dallaData, allaData)
            // Case Calendar.ScheduleType.OGNI_N_ORE : Return CalcRuns(DateTimeInterval.Hour, Me.Intervallo, dallaData, allaData)
            // Case Calendar.ScheduleType.OGNI_N_ANNI : Return CalcRuns(DateTimeInterval.Year, Me.Intervallo, dallaData, allaData)
            // Case Calendar.ScheduleType.OGNI_N_GIORNI : Return CalcRuns(DateTimeInterval.Day, Me.Intervallo, dallaData, allaData)
            // Case Calendar.ScheduleType.OGNI_N_MESI : Return CalcRuns(DateTimeInterval.Month, Me.Intervallo, dallaData, allaData)
            // Case Calendar.ScheduleType.OGNI_N_SETTIMANE : Return CalcRuns(DateTimeInterval.Weekday, Me.Intervallo, dallaData, allaData)
            // Case Else
            // Throw New ArgumentOutOfRangeException("ScheduleType")
            // End Select
            // End Function

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_OwnerType, this.m_OwnerID, this.m_DataInizio, this.m_ScheduleType);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CalendarSchedule) && this.Equals((CalendarSchedule)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CalendarSchedule obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ((int)this.m_ScheduleType, (int)obj.m_ScheduleType)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Doubles.EQ(this.m_Intervallo, obj.m_Intervallo)
                    && DMD.Integers.EQ(this.m_Ripetizioni, obj.m_Ripetizioni)
                    && DMD.DateUtils.EQ(this.m_UltimaEsecuzione, obj.m_UltimaEsecuzione)
                    && DMD.Integers.EQ(this.m_ConteggioEsecuzioni, obj.m_ConteggioEsecuzioni)
                    && DMD.Strings.EQ(this.m_OwnerType, obj.m_OwnerType)
                    && DMD.Integers.EQ(this.m_OwnerID, obj.m_OwnerID)
                    ;
            //private CKeyCollection m_Parameters;
            }

        }
    }
}