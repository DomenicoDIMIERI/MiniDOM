using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Rappresenta una annotazione speciale utilizzabile dai collaboratori
        /// </summary>
        [Serializable]
        public class NotaCollaboratore
            : Databases.DBObject, IComparable, IComparable<NotaCollaboratore>
        {
            private int m_IDPersona;
            [NonSerialized] private Anagrafica.CPersonaFisica m_Persona;
            private int m_IDCollaboratore;
            [NonSerialized] private CCollaboratore m_Collaboratore;
            private int m_IDClienteXCollaboratore;
            [NonSerialized] private ClienteXCollaboratore m_ClienteXCollaboratore;
            private DateTime m_Data;
            private string m_Tipo;
            private string m_Indirizzo;
            private int m_Esito;
            private string m_Scopo;
            private string m_Nota;

            /// <summary>
            /// Costruttore
            /// </summary>
            public NotaCollaboratore()
            {
                m_IDPersona = 0;
                m_Persona = null;
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
                m_IDClienteXCollaboratore = 0;
                m_ClienteXCollaboratore = null;
                m_Data = DMD.DateUtils.Now();
                m_Tipo = "";
                m_Indirizzo = "";
                m_Esito = 0;
                m_Scopo = "";
                m_Nota = "";
            }

            /// <summary>
            /// Restituisce o imposta il tipo della nota
            /// </summary>
            /// <returns></returns>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona fisica nel database principale associata al cliente gestito dal collaboratore
            /// </summary>
            /// <returns></returns>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona fisica nel database principale associata al cliente gestito dal collaboratore
            /// </summary>
            /// <returns></returns>
            public Anagrafica.CPersonaFisica Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value);
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il cliente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(Anagrafica.CPersonaFisica value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value);
            }

            /// <summary>
            /// Restituisce o imposta l'ID del collaboratore che ha in gestione il cliente
            /// </summary>
            /// <returns></returns>
            public int IDCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_Collaboratore, m_IDCollaboratore);
                }

                set
                {
                    int oldValue = IDCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDCollaboratore = value;
                    m_Collaboratore = null;
                    DoChanged("IDCollaboratore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il collaboratore che ha in gestione il cliente
            /// </summary>
            /// <returns></returns>
            public CCollaboratore Collaboratore
            {
                get
                {
                    if (m_Collaboratore is null)
                        m_Collaboratore = Collaboratori.GetItemById(m_IDCollaboratore);
                    return m_Collaboratore;
                }

                set
                {
                    var oldValue = Collaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Collaboratore = value;
                    m_IDCollaboratore = DBUtils.GetID(value);
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il collaboratore
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetCollaboratore(CCollaboratore value)
            {
                m_Collaboratore = value;
                m_IDCollaboratore = DBUtils.GetID(value);
            }

            /// <summary>
            /// Restituisce o imposta l'ID del ClienteXCollaboratore che ha in gestione il cliente
            /// </summary>
            /// <returns></returns>
            public int IDClienteXCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_ClienteXCollaboratore, m_IDClienteXCollaboratore);
                }

                set
                {
                    int oldValue = IDClienteXCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDClienteXCollaboratore = value;
                    m_ClienteXCollaboratore = null;
                    DoChanged("IDClienteXCollaboratore", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la relazione cliente x collaboratore
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetClienteXCollaboratore(ClienteXCollaboratore value)
            {
                m_ClienteXCollaboratore = value;
                m_IDClienteXCollaboratore = DBUtils.GetID(value);
            }

            /// <summary>
            /// Restituisce o imposta il ClienteXCollaboratore che ha in gestione il cliente
            /// </summary>
            /// <returns></returns>
            public ClienteXCollaboratore ClienteXCollaboratore
            {
                get
                {
                    if (m_ClienteXCollaboratore is null)
                        m_ClienteXCollaboratore = Collaboratori.ClientiXCollaboratori.GetItemById(m_IDClienteXCollaboratore);
                    return m_ClienteXCollaboratore;
                }

                set
                {
                    var oldValue = m_ClienteXCollaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ClienteXCollaboratore = value;
                    m_IDClienteXCollaboratore = DBUtils.GetID(value);
                    DoChanged("ClienteXCollaboratore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data
            /// </summary>
            /// <returns></returns>
            public DateTime? Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    DateTime? oldValue = m_Data;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Data = (DateTime)value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo del ricontatto fissato dal collaboratore
            /// </summary>
            /// <returns></returns>
            public string Scopo
            {
                get
                {
                    return m_Scopo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Scopo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Scopo = value;
                    DoChanged("Scopo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags aggiuntivi
            /// </summary>
            /// <returns></returns>
            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei Esito aggiuntivi
            /// </summary>
            /// <returns></returns>
            public int Esito
            {
                get
                {
                    return m_Esito;
                }

                set
                {
                    int oldValue = m_Esito;
                    if (oldValue == value)
                        return;
                    m_Esito = value;
                    DoChanged("Esito", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta l'indirizzo
            /// </summary>
            public string Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }

                set
                {
                    string oldValue = m_Indirizzo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Indirizzo = value;
                    DoChanged("Indirizzo", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il motivo dell'assegnazione
            /// </summary>
            /// <returns></returns>
            public string Nota
            {
                get
                {
                    return m_Nota;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nota;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nota = value;
                    DoChanged("Nota", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Collaboratori.Note;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CQSPDNoteCliXCollab";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                m_IDClienteXCollaboratore = reader.Read("IDCliXCollab", this.m_IDClienteXCollaboratore);
                m_Data = reader.Read("Data", this.m_Data);
                m_Tipo = reader.Read("Tipo", this.m_Tipo);
                m_Indirizzo = reader.Read("Indirizzo", this.m_Indirizzo);
                m_Esito = reader.Read("Esito", this.m_Esito);
                m_Scopo = reader.Read("Scopo", this.m_Scopo);
                m_Nota = reader.Read("Nota", this.m_Nota);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPersona", this.IDPersona);
                writer.Write("IDCollaboratore", this.IDCollaboratore);
                writer.Write("IDCliXCollab", this.IDClienteXCollaboratore);
                writer.Write("Data", this.m_Data);
                writer.Write("Tipo", this.m_Tipo);
                writer.Write("Indirizzo", this.m_Indirizzo);
                writer.Write("Esito", this.m_Esito);
                writer.Write("Scopo", this.m_Scopo);
                writer.Write("Nota", this.m_Nota);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("IDCollaboratore", typeof(int), 1);
                c = table.Fields.Ensure("IDCliXCollab", typeof(int), 1);
                c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo", typeof(string), 255);
                c = table.Fields.Ensure("Esito", typeof(int), 1);
                c = table.Fields.Ensure("Scopo", typeof(string), 255);
                c = table.Fields.Ensure("Nota", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "IDCliXCollab" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxCollab", new string[] { "IDCollaboratore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxData", new string[] { "Data", "Tipo", "Scopo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAddress", new string[] { "Indirizzo", "Esito" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNota", new string[] { "Nota" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteAttribute("IDCliXCollab", IDClienteXCollaboratore);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Indirizzo", m_Indirizzo);
                writer.WriteAttribute("Esito", m_Esito);
                writer.WriteAttribute("Scopo", m_Scopo);
                base.XMLSerialize(writer);
                writer.WriteTag("Nota", m_Nota);
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
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCliXCollab":
                        {
                            m_IDClienteXCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                   

                    case "Esito":
                        {
                            m_Esito = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Scopo":
                        {
                            m_Scopo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                   
                    case "Nota":
                        {
                            m_Nota = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(NotaCollaboratore obj)
            {
                return DMD.DateUtils.Compare(Data, obj.Data);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((NotaCollaboratore)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return
                    DMD.Strings.ConcatArray(
                            DMD.DateUtils.FormatDateYYYYMMDD(this.m_Data), " - ",
                            this.m_Nota
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_Data, this.m_IDClienteXCollaboratore, this.m_Nota);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is NotaCollaboratore) && this.Equals((NotaCollaboratore)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(NotaCollaboratore obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Integers.EQ(this.m_IDCollaboratore, obj.m_IDCollaboratore)
                    && DMD.Integers.EQ(this.m_IDClienteXCollaboratore, obj.m_IDClienteXCollaboratore)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Indirizzo, obj.m_Indirizzo)
                    && DMD.Integers.EQ(this.m_Esito, obj.m_Esito)
                    && DMD.Strings.EQ(this.m_Scopo, obj.m_Scopo)
                    && DMD.Strings.EQ(this.m_Nota, obj.m_Nota)
                    ;
            }
        }
    }
}