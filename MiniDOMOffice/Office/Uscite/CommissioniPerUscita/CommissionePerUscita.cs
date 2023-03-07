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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {


        /// <summary>
        /// Descrive lo svolgimento di una <see cref="Commissione"/> durante un'<see cref="Uscita"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CommissionePerUscita
            : minidom.Databases.DBObject
        {
            [NonSerialized] private CUser m_Operatore;                    // Operatore che ha svolto la commissione
            private int m_IDOperatore;                // ID dell'operatore che ha svolto la commissione
            private string m_NomeOperatore;               // Nome dell'operatore che ha svolto la commissione
            private DateTime? m_OraInizio;        // Data ed ora di uscita (per svolgere la commissione)
            private DateTime? m_OraFine;       // Data ed ora di rientro
            private double? m_DistanzaPercorsa;   // Distanza percorsa
            private int m_IDCommissione;
            [NonSerialized] private Commissione m_Commissione;
            private int m_IDUscita;
            [NonSerialized] private Uscita m_Uscita;
            private string m_DescrizioneEsito;
            private CCollection<LuogoDaVisitare> m_Luoghi;
            private StatoCommissione m_StatoCommissione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CommissionePerUscita()
            {
                m_Operatore = null;
                m_IDOperatore = 0;
                m_NomeOperatore = DMD.Strings.vbNullString;
                m_IDCommissione = 0;
                m_Commissione = null;
                m_IDUscita = 0;
                m_Uscita = null;
                m_DescrizioneEsito = DMD.Strings.vbNullString;
                m_OraInizio = default;
                m_OraFine = default;
                m_DistanzaPercorsa = default;
                m_Luoghi = new CCollection<LuogoDaVisitare>(); // = vbNullString
                m_StatoCommissione = StatoCommissione.NonIniziata;
            }

            // Public Property Luogo As String
            // Get
            // Return Me.m_Luogo
            // End Get
            // Set(value As String)
            // value = Strings.Trim(value)
            // Dim oldValue As String = Me.m_Luogo
            // If (oldValue = value) Then Exit Property
            // Me.m_Luogo = value
            // Me.DoChanged("Luogo", value, oldValue)
            // End Set
            // End Property

            public CCollection<LuogoDaVisitare> Luoghi
            {
                get
                {
                    return m_Luoghi;
                }
            }

            /// <summary>
            /// Stato della commissione dopo questa operazione
            /// </summary>
            public StatoCommissione StatoCommissione
            {
                get
                {
                    return m_StatoCommissione;
                }

                set
                {
                    var oldValue = m_StatoCommissione;
                    if (oldValue == value)
                        return;
                    m_StatoCommissione = value;
                    DoChanged("StatoCommissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha effettuato la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = m_Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha effettuato la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha effettuate la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// ID della commissione
            /// </summary>
            public int IDCommissione
            {
                get
                {
                    return DBUtils.GetID(m_Commissione, m_IDCommissione);
                }

                set
                {
                    int oldValue = IDCommissione;
                    if (oldValue == value)
                        return;
                    m_IDCommissione = value;
                    m_Commissione = null;
                    DoChanged("IDCommissione", value, oldValue);
                }
            }

            /// <summary>
            /// Commissione
            /// </summary>
            public Commissione Commissione
            {
                get
                {
                    if (m_Commissione is null)
                        m_Commissione = Commissioni.GetItemById(m_IDCommissione);
                    return m_Commissione;
                }

                set
                {
                    var oldValue = m_Commissione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Commissione = value;
                    m_IDCommissione = DBUtils.GetID(value, 0);
                    DoChanged("Commissione", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la commissione
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetCommissione(Commissione value)
            {
                m_Commissione = value;
                m_IDCommissione = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// ID dell'uscita
            /// </summary>
            public int IDUscita
            {
                get
                {
                    return DBUtils.GetID(m_Uscita, m_IDUscita);
                }

                set
                {
                    int oldValue = IDUscita;
                    if (oldValue == value)
                        return;
                    m_IDUscita = value;
                    m_Uscita = null;
                    DoChanged("IDUscita", value, oldValue);
                }
            }

            /// <summary>
            /// Uscita
            /// </summary>
            public Uscita Uscita
            {
                get
                {
                    if (m_Uscita is null)
                        m_Uscita = Uscite.GetItemById(m_IDUscita);
                    return m_Uscita;
                }

                set
                {
                    var oldValue = m_Uscita;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Uscita = value;
                    m_IDUscita = DBUtils.GetID(value, 0);
                    DoChanged("Uscita", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'uscita
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUscita(Uscita value)
            {
                m_Uscita = value;
                m_IDUscita = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Descrizione dell'esito della commissione in seguito a questa operazione
            /// </summary>
            public string DescrizioneEsito
            {
                get
                {
                    return m_DescrizioneEsito;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DescrizioneEsito;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneEsito = value;
                    DoChanged("DescrizioneEsito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di uscita per la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? OraInizio
            {
                get
                {
                    return m_OraInizio;
                }

                set
                {
                    var oldValue = m_OraInizio;
                    if (oldValue == value == true)
                        return;
                    m_OraInizio = value;
                    DoChanged("OraInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la durata in secondi (differenza tra ora ingresso ed ora uscita)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int? Durata
            {
                get
                {
                    if (m_OraInizio.HasValue && m_OraFine.HasValue)
                    {
                        return (int?)Maths.Abs(DMD.DateUtils.DateDiff("s", m_OraFine.Value, m_OraInizio.Value));
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            /// <summary>
            /// Restituisce o imposta la data di rientro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? OraFine
            {
                get
                {
                    return m_OraFine;
                }

                set
                {
                    var oldValue = m_OraFine;
                    if (oldValue == value == true)
                        return;
                    m_OraFine = value;
                    DoChanged("OraFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la distanza percorsa
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? DistanzaPercorsa
            {
                get
                {
                    return m_DistanzaPercorsa;
                }

                set
                {
                    var oldValue = m_DistanzaPercorsa;
                    if (oldValue == value == true)
                        return;
                    m_DistanzaPercorsa = value;
                    DoChanged("DistanzaPercorsa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Commissione { ", this.IDCommissione , " x ", this.IDUscita, "}");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDUscita);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CommissionePerUscita) && this.Equals((CommissionePerUscita)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CommissionePerUscita obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.DateUtils.EQ(this.m_OraInizio, obj.m_OraInizio)
                    && DMD.DateUtils.EQ(this.m_OraFine, obj.m_OraFine)
                    && DMD.Doubles.EQ(this.m_DistanzaPercorsa, obj.m_DistanzaPercorsa)
                    && DMD.Integers.EQ(this.m_IDCommissione, obj.m_IDCommissione)
                    && DMD.Integers.EQ(this.m_IDUscita, obj.m_IDUscita)
                    && DMD.Strings.EQ(this.m_DescrizioneEsito, obj.m_DescrizioneEsito)
                    && this.m_Luoghi.Equals(obj.m_Luoghi)
                    && DMD.RunTime.EQ(this.m_StatoCommissione, obj.m_StatoCommissione)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Uscite.CommissioniPerUscite;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeCommissXUscite";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDOperatore = reader.Read("IDOperatore",  m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore",  m_NomeOperatore);
                m_IDUscita = reader.Read("IDUscita",  m_IDUscita);
                m_IDCommissione = reader.Read("IDCommissione",  m_IDCommissione);
                m_DescrizioneEsito = reader.Read("DescrizioneEsito",  m_DescrizioneEsito);
                m_OraInizio = reader.Read("OraInizio",  m_OraInizio);
                m_OraFine = reader.Read("OraFine",  m_OraFine);
                m_DistanzaPercorsa = reader.Read("DistanzaPercorsa",  m_DistanzaPercorsa);
                // Me.m_Luogo = reader.Read("Luogo", Me.m_Luogo)
                string tmp = reader.Read("Luogo", "");
                if (!string.IsNullOrEmpty(tmp))
                {  
                    m_Luoghi = new CCollection<LuogoDaVisitare>();
                    m_Luoghi.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(luogo));
                }
                m_StatoCommissione = reader.Read("StatoCommissione",  m_StatoCommissione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("IDUscita", IDUscita);
                writer.Write("IDCommissione", IDCommissione);
                writer.Write("DescrizioneEsito", m_DescrizioneEsito);
                writer.Write("OraInizio", m_OraInizio);
                writer.Write("OraFine", m_OraFine);
                writer.Write("DistanzaPercorsa", m_DistanzaPercorsa);
                writer.Write("StatoCommissione", m_StatoCommissione);
                writer.Write("Luogo", DMD.XML.Utils.Serializer.Serialize(Luoghi));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema del db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("IDUscita", typeof(int), 1);
                c = table.Fields.Ensure("IDCommissione", typeof(int), 1);
                c = table.Fields.Ensure("DescrizioneEsito", typeof(string), 255);
                c = table.Fields.Ensure("OraInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("DistanzaPercorsa", typeof(double), 1);
                c = table.Fields.Ensure("StatoCommissione", typeof(int), 1);
                c = table.Fields.Ensure("Luogo", typeof(string), 0);
                 
            }

            /// <summary>
            /// Prepara i vincoli del db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "IDUscita", "IDCommissione", "OraInizio", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxEsito", new string[] { "OraFine", "DescrizioneEsito", "StatoCommissione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri", new string[] { "DistanzaPercorsa", "NomeOperatore" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Luogo", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("IDUscita", IDUscita);
                writer.WriteAttribute("DescrizioneEsito", m_DescrizioneEsito);
                writer.WriteAttribute("OraInizio", m_OraInizio);
                writer.WriteAttribute("OraFine", m_OraFine);
                writer.WriteAttribute("DistanzaPercorsa", m_DistanzaPercorsa);
                writer.WriteAttribute("StatoCommissione", (int?)m_StatoCommissione);
                base.XMLSerialize(writer);
                writer.WriteTag("Luoghi", Luoghi);
                if (!writer.GetSetting("uscitaserialization", false))
                    writer.WriteTag("Uscita", Uscita);
                writer.WriteTag("IDCommissione", IDCommissione);
                if (!writer.GetSetting("commissioneserialization", false))
                    writer.WriteTag("Commissione", Commissione);
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
                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDUscita":
                        {
                            m_IDUscita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Uscita":
                        {
                            m_Uscita = (Uscita)fieldValue;
                            break;
                        }

                    case "IDCommissione":
                        {
                            m_IDCommissione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Commissione":
                        {
                            m_Commissione = (Commissione)fieldValue;
                            break;
                        }

                    case "DescrizioneEsito":
                        {
                            m_DescrizioneEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "OraInizio":
                        {
                            m_OraInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraFine":
                        {
                            m_OraFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DistanzaPercorsa":
                        {
                            m_DistanzaPercorsa = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Luoghi":
                        {
                            m_Luoghi.Clear();
                            m_Luoghi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "StatoCommissione":
                        {
                            m_StatoCommissione = (StatoCommissione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }


        }
    }
}