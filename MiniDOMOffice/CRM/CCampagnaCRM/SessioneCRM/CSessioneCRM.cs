using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Sessione CRM
        /// </summary>
        [Serializable]
        public class CSessioneCRM 
            : minidom.Databases.DBObjectPO
        {
            private int m_IDCampagnaCRM;
            [NonSerialized] private CCampagnaCRM m_CampagnaCRM;
            private int m_IDUtente;
            [NonSerialized] private Sistema.CUser m_Utente;
            private string m_NomeUtente;
            private DateTime? m_Inizio;
            private DateTime? m_Fine;
            private int m_NumeroTelefonateRisposte;
            private int m_NumeroTelefonateNonRisposte;
            private double m_MinutiConversazione;
            private double m_MinutiAttesa;
            private int m_NumeroAppuntamentiFissati;
            private string m_DMDpage;
            private int m_IDSupervisore;
            [NonSerialized] private Sistema.CUser m_Supervisore;
            private string m_NomeSupervisore;
            private DateTime? m_LastUpdated;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSessioneCRM()
            {
                m_IDCampagnaCRM = 0;
                m_CampagnaCRM = null;
                m_IDUtente = 0;
                m_Utente = null;
                m_NomeUtente = "";
                m_Inizio = default;
                m_Fine = default;
                m_NumeroTelefonateRisposte = 0;
                m_NumeroTelefonateNonRisposte = 0;
                m_MinutiConversazione = 0;
                m_MinutiAttesa = 0;
                m_NumeroAppuntamentiFissati = 0;
                m_Flags = 0;
                m_DMDpage = "";
                m_IDSupervisore = 0;
                m_Supervisore = null;
                m_NomeSupervisore = "";
                m_LastUpdated = default;
            }

            /// <summary>
            /// Data e ora dell'ultimo aggiornamento ricevuto
            /// </summary>
            public DateTime? LastUpdated
            {
                get
                {
                    return m_LastUpdated;
                }

                set
                {
                    var oldValue = m_LastUpdated;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_LastUpdated = value;
                    DoChanged("LastUpdated", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del resposabile di supervisione
            /// </summary>
            public int IDSupervisore
            {
                get
                {
                    return DBUtils.GetID(m_Supervisore, m_IDSupervisore);
                }

                set
                {
                    int oldValue = IDSupervisore;
                    if (oldValue == value)
                        return;
                    m_IDSupervisore = value;
                    m_Supervisore = null;
                    DoChanged("IDSupervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il supervisore
            /// </summary>
            public Sistema.CUser Supervisore
            {
                get
                {
                    if (m_Supervisore is null)
                        m_Supervisore = Sistema.Users.GetItemById(m_IDSupervisore);
                    return m_Supervisore;
                }

                set
                {
                    var oldValue = Supervisore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDSupervisore = DBUtils.GetID(value, 0);
                    m_Supervisore = value;
                    m_NomeSupervisore = (value is object)? value.Nominativo : string.Empty;
                    DoChanged("Supervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del supervisore
            /// </summary>
            public string NomeSupervisore
            {
                get
                {
                    return m_NomeSupervisore;
                }

                set
                {
                    string oldValue = m_NomeSupervisore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSupervisore = value;
                    DoChanged("NomeSupervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della pagina in cui é stata attivata la sessione
            /// </summary>
            /// <returns></returns>
            public string dmdpage
            {
                get
                {
                    return m_DMDpage;
                }

                set
                {
                    string oldValue = m_DMDpage;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DMDpage = value;
                    DoChanged("dmdpage", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della campagna crm in corso
            /// </summary>
            public int IDCampagnaCRM
            {
                get
                {
                    return DBUtils.GetID(m_CampagnaCRM, m_IDCampagnaCRM);
                }

                set
                {
                    int oldValue = IDCampagnaCRM;
                    if (oldValue == value)
                        return;
                    m_IDCampagnaCRM = value;
                    m_CampagnaCRM = null;
                    DoChanged("IDCampagnaCRM", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la campgna in corso per la sessione crm
            /// </summary>
            public CCampagnaCRM CampagnaCRM
            {
                get
                {
                    if (m_CampagnaCRM is null)
                        m_CampagnaCRM = minidom.CustomerCalls.CampagneCRM.GetItemById(m_IDCampagnaCRM);
                    return m_CampagnaCRM;
                }

                set
                {
                    var oldValue = CampagnaCRM;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CampagnaCRM = value;
                    m_IDCampagnaCRM = DBUtils.GetID(value, 0);
                    DoChanged("CampagnaCRM", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la campagna
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetCampagnaCRM(CCampagnaCRM value)
            {
                m_CampagnaCRM = value;
                m_IDCampagnaCRM = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'id utente 
            /// </summary>
            public int IDUtente
            {
                get
                {
                    return DBUtils.GetID(m_Utente, m_IDUtente);
                }

                set
                {
                    int oldValue = IDUtente;
                    if (oldValue == value)
                        return;
                    m_IDUtente = value;
                    m_Utente = null;
                    DoChanged("IDUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente
            /// </summary>
            public Sistema.CUser Utente
            {
                get
                {
                    if (m_Utente is null)
                        m_Utente = Sistema.Users.GetItemById(m_IDUtente);
                    return m_Utente;
                }

                set
                {
                    var oldValue = Utente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Utente = value;
                    m_IDUtente = DBUtils.GetID(value, 0);
                    m_NomeUtente = "";
                    if (value is object)
                        m_NomeUtente = value.Nominativo;
                    DoChanged("Utente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome utente
            /// </summary>
            public string NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }

                set
                {
                    string oldValue = m_NomeUtente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUtente = value;
                    DoChanged("NomeUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUtente(Sistema.CUser value)
            {
                m_Utente = value;
                m_IDUtente = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio della sessione
            /// </summary>
            public DateTime? Inizio
            {
                get
                {
                    return m_Inizio;
                }

                set
                {
                    var oldValue = m_Inizio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Inizio = value;
                    DoChanged("Inizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data finale della sessione
            /// </summary>
            public DateTime? Fine
            {
                get
                {
                    return m_Fine;
                }

                set
                {
                    var oldValue = m_Fine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Fine = value;
                    DoChanged("Fine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di telefonate a cui l'operatore ha risposto durante la sessione
            /// </summary>
            public int NumeroTelefonateRisposte
            {
                get
                {
                    return m_NumeroTelefonateRisposte;
                }

                set
                {
                    int oldValue = m_NumeroTelefonateRisposte;
                    if (oldValue == value)
                        return;
                    m_NumeroTelefonateRisposte = value;
                    DoChanged("NumeroTelefonateRisposte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di telefonate a cui non si é risposto durante la sessione
            /// </summary>
            public int NumeroTelefonateNonRisposte
            {
                get
                {
                    return m_NumeroTelefonateNonRisposte;
                }
                set
                {
                    int oldValue = m_NumeroTelefonateNonRisposte;
                    if (oldValue == value)
                        return;
                    m_NumeroTelefonateRisposte = value;
                    DoChanged("NumeroTelefonateNonRisposte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i minuti di conversazione
            /// </summary>
            public double MinutiConversazione
            {
                get
                {
                    return m_MinutiConversazione;
                }

                set
                {
                    var oldValue = m_MinutiConversazione;
                    if (oldValue == value)
                        return;
                    m_MinutiConversazione = value;
                    DoChanged("MinutiConversazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i minuti di attesa
            /// </summary>
            public double MinutiAttesa
            {
                get
                {
                    return m_MinutiAttesa;
                }

                set
                {
                    var oldValue = m_MinutiAttesa;
                    if (oldValue == value)
                        return;
                    m_MinutiAttesa = value;
                    DoChanged("MinutiAttesa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i numeri di appuntamenti fissati durante la sessione
            /// </summary>
            public int NumeroAppuntamentiFissati
            {
                get
                {
                    return m_NumeroAppuntamentiFissati;
                }

                set
                {
                    var oldValue = m_NumeroAppuntamentiFissati;
                    if (oldValue == value)
                        return;
                    m_NumeroAppuntamentiFissati = value;
                    DoChanged("NumeroAppuntamentiFissati", value, oldValue);
                }
            }
              
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.SessioniCRM;
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>

            public override string GetTableName()
            {
                return "tbl_SessioneCRM";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCampagnaCRM = reader.Read("IDCampagnaCRM", m_IDCampagnaCRM);
                m_IDUtente = reader.Read("IDUtente", m_IDUtente);
                m_NomeUtente = reader.Read("NomeUtente", m_NomeUtente);
                m_Inizio = reader.Read("Inizio", m_Inizio);
                m_Fine = reader.Read("Fine", m_Fine);
                m_NumeroTelefonateRisposte = reader.Read("NumeroTelefonateRisposte", m_NumeroTelefonateRisposte);
                m_NumeroTelefonateNonRisposte = reader.Read("NumeroTelefonateNonRisposte", m_NumeroTelefonateNonRisposte);
                m_MinutiConversazione = reader.Read("MinutiConversazione", m_MinutiConversazione);
                m_MinutiAttesa = reader.Read("MinutiAttesa", m_MinutiAttesa);
                m_NumeroAppuntamentiFissati = reader.Read("NumeroAppuntamentiFissati", m_NumeroAppuntamentiFissati);
                m_DMDpage = reader.Read("dmdpage", m_DMDpage);
                m_IDSupervisore = reader.Read("IDSupervisore", m_IDSupervisore);
                m_NomeSupervisore = reader.Read("NomeSupervisore", m_NomeSupervisore);
                m_LastUpdated = reader.Read("LastUpdated", m_LastUpdated);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCampagnaCRM", IDCampagnaCRM);
                writer.Write("IDUtente", IDUtente);
                writer.Write("NomeUtente", m_NomeUtente);
                writer.Write("Inizio", m_Inizio);
                writer.Write("Fine", m_Fine);
                writer.Write("NumeroTelefonateRisposte", m_NumeroTelefonateRisposte);
                writer.Write("NumeroTelefonateNonRisposte", m_NumeroTelefonateNonRisposte);
                writer.Write("MinutiConversazione", m_MinutiConversazione);
                writer.Write("MinutiAttesa", m_MinutiAttesa);
                writer.Write("NumeroAppuntamentiFissati", m_NumeroAppuntamentiFissati);
                writer.Write("dmdpage", m_DMDpage);
                writer.Write("IDSupervisore", m_IDSupervisore);
                writer.Write("NomeSupervisore", m_NomeSupervisore);
                writer.Write("LastUpdated", m_LastUpdated);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDCampagnaCRM", typeof(int), 1);
                c = table.Fields.Ensure("IDUtente", typeof(int), 1);
                c = table.Fields.Ensure("NomeUtente", typeof(string), 255);
                c = table.Fields.Ensure("Inizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("Fine", typeof(DateTime), 1);
                c = table.Fields.Ensure("NumeroTelefonateRisposte", typeof(int), 1);
                c = table.Fields.Ensure("NumeroTelefonateNonRisposte", typeof(int), 1);
                c = table.Fields.Ensure("MinutiConversazione", typeof(double));
                c = table.Fields.Ensure("MinutiAttesa", typeof(double));
                c = table.Fields.Ensure("NumeroAppuntamentiFissati", typeof(int), 1);
                c = table.Fields.Ensure("dmdpage", typeof(string), 255);
                c = table.Fields.Ensure("IDSupervisore", typeof(int), 1);
                c = table.Fields.Ensure("NomeSupervisore", typeof(string), 255);
                c = table.Fields.Ensure("LastUpdated", typeof(DateTime), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCampagna", new string[] { "IDCampagnaCRM", "IDUtente", "Inizio" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxUtente", new string[] { "IDSupervisore", "NomeUtente", "NomeSupervisore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "LastUpdated", "Fine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumeri1", new string[] { "NumeroTelefonateRisposte", "NumeroTelefonateNonRisposte", "MinutiConversazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumeri2", new string[] { "MinutiAttesa", "NumeroAppuntamentiFissati"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxdmdpage", new string[] { "dmdpage" }, DBFieldConstraintFlags.None);

                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCampagnaCRM", IDCampagnaCRM);
                writer.WriteAttribute("IDUtente", IDUtente);
                writer.WriteAttribute("NomeUtente", m_NomeUtente);
                writer.WriteAttribute("Inizio", m_Inizio);
                writer.WriteAttribute("Fine", m_Fine);
                writer.WriteAttribute("NumeroTelefonateRisposte", m_NumeroTelefonateRisposte);
                writer.WriteAttribute("NumeroTelefonateNonRisposte", m_NumeroTelefonateNonRisposte);
                writer.WriteAttribute("MinutiConversazione", m_MinutiConversazione);
                writer.WriteAttribute("MinutiAttesa", m_MinutiAttesa);
                writer.WriteAttribute("NumeroAppuntamentiFissati", m_NumeroAppuntamentiFissati);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("dmdpage", m_DMDpage);
                writer.WriteAttribute("IDSupervisore", m_IDSupervisore);
                writer.WriteAttribute("NomeSupervisore", m_NomeSupervisore);
                writer.WriteAttribute("LastUpdated", m_LastUpdated);
                base.XMLSerialize(writer);
                writer.WriteTag("Attributi", Attributi);
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
                    case "IDCampagnaCRM":
                        {
                            m_IDCampagnaCRM = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUtente":
                        {
                            m_IDUtente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUtente":
                        {
                            m_NomeUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Inizio":
                        {
                            m_Inizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Fine":
                        {
                            m_Fine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "LastUpdated":
                        {
                            m_LastUpdated = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NumeroTelefonateRisposte":
                        {
                            m_NumeroTelefonateRisposte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroTelefonateNonRisposte":
                        {
                            m_NumeroTelefonateNonRisposte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MinutiConversazione":
                        {
                            m_MinutiConversazione = (double)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MinutiAttesa":
                        {
                            m_MinutiAttesa = (double)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroAppuntamentiFissati":
                        {
                            m_NumeroAppuntamentiFissati = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }                   

                    case "dmdpage":
                        {
                            m_DMDpage = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder(512);
                ret.Append(m_NomeUtente);
                ret.Append(" [");
                ret.Append(Sistema.Formats.FormatUserDateTime(m_Inizio));
                ret.Append(" - ");
                ret.Append(Sistema.Formats.FormatUserDateTime(m_Fine));
                ret.Append("]");
                return ret.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDUtente);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CSessioneCRM) && this.Equals((CSessioneCRM)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CSessioneCRM obj)
            {
                return base.Equals(obj)
                        && DMD.Integers.EQ(this.m_IDCampagnaCRM, obj.m_IDCampagnaCRM)
                        && DMD.Integers.EQ(this.m_IDUtente, obj.m_IDUtente)
                        && DMD.Strings.EQ(this.m_NomeUtente, obj.m_NomeUtente)
                        && DMD.DateUtils.EQ(this.m_Inizio, obj.m_Inizio)
                        && DMD.DateUtils.EQ(this.m_Fine, obj.m_Fine)
                        && DMD.Integers.EQ(this.m_NumeroTelefonateRisposte, obj.m_NumeroTelefonateRisposte)
                        && DMD.Integers.EQ(this.m_NumeroTelefonateNonRisposte, obj.m_NumeroTelefonateNonRisposte)
                        && DMD.Doubles.EQ(this.m_MinutiConversazione, obj.m_MinutiConversazione)
                        && DMD.Doubles.EQ(this.m_MinutiAttesa, obj.m_MinutiAttesa)
                        && DMD.Integers.EQ(this.m_NumeroAppuntamentiFissati, obj.m_NumeroAppuntamentiFissati)
                        && DMD.Strings.EQ(this.m_DMDpage, obj.m_DMDpage)
                        && DMD.Integers.EQ(this.m_IDSupervisore, obj.m_IDSupervisore)
                        && DMD.Strings.EQ(this.m_NomeSupervisore, obj.m_NomeSupervisore)
                        && DMD.DateUtils.EQ(this.m_LastUpdated, obj.m_LastUpdated)
                        ;
            }
        }
    }
}