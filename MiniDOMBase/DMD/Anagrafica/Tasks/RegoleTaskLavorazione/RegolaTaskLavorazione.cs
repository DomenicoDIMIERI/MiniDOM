using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;


namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flag di una regola 
        /// </summary>
        [Flags]
        public enum RegolaTaskLavorazioneFlags : int
        {
            /// <summary>
            /// Nessuno
            /// </summary>
            None = 0,

            /// <summary>
            /// Regola attiva
            /// </summary>
            Attivo = 1,

            /// <summary>
            /// Regola nascota
            /// </summary>
            Nascosto = 2,

            /// <summary>
            /// Regola applicabile solo da utenti privilegiati
            /// </summary>
            Privilegiato = 4
        }

        /// <summary>
        /// Rappresenta una possibile regola applicabile ad un task di lavorazione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RegolaTaskLavorazione 
            : Databases.DBObject, IComparable, IComparable<RegolaTaskLavorazione>
        {
            
            private string m_Nome;                                // Nome della regola
            private string m_Descrizione;                         // Descrizione della regola
            private int m_IDStatoSorgente;                    // ID dello stato sorgente
            [NonSerialized] private StatoTaskLavorazione m_StatoSorgente;         // stato destinazione
            private int m_IDStatoDestinazione;                // ID dello stato destinazione
            [NonSerialized] private StatoTaskLavorazione m_StatoDestinazione;     // Stato destinazione
            private string m_NomeHandler;                         // handler da chiamare per l'esecuzione della regola
            private string m_ContextType;                         // Contesto in cui é applicabile la regola
            private int m_Ordine;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegolaTaskLavorazione()
            {
                this.m_Nome = "";
                this.m_Descrizione = "";
                this.m_IDStatoSorgente = 0;
                this.m_StatoSorgente = null;
                this.m_IDStatoDestinazione = 0;
                this.m_StatoDestinazione = null;
                this.m_Flags = (int)RegolaTaskLavorazioneFlags.Attivo;
                this.m_NomeHandler = "";
                this.m_ContextType = "";
                this.m_Ordine = 0; 
            }
             

            /// <summary>
            /// Restituisce o imposta il contesto in cui si applica la regola (es. CTelefonata, CVisita, ec...)
            /// </summary>
            /// <returns></returns>
            public string ContextType
            {
                get
                {
                    return m_ContextType;
                }

                set
                {
                    string oldValue = m_ContextType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContextType = value;
                    DoChanged("ContextType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dello stato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    string oldValue = m_Nome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome della classe che implementa l'interfaccia ITaskHandler il cui metodo Handle verrà lanciato all'esecuzione dell'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeHandler
            {
                get
                {
                    return m_NomeHandler;
                }

                set
                {
                    string oldValue = m_NomeHandler;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeHandler = value;
                    DoChanged("NomeHandler", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione dello stato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un itero utilizzabile per i flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new RegolaTaskLavorazioneFlags Flags
            {
                get
                {
                    return (RegolaTaskLavorazioneFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    this.m_Flags = (int)value;
                    this.DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica la precedenza di visualizzazione
            /// </summary>
            public int Ordine
            {
                get
                {
                    return m_Ordine;
                }

                set
                {
                    int oldValue = m_Ordine;
                    if (oldValue == value)
                        return;
                    m_Ordine = value;
                    DoChanged("Ordine", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'ordine
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetOrdine(int value)
            {
                m_Ordine = value;
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se lo stato è attivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, RegolaTaskLavorazioneFlags.Attivo);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, RegolaTaskLavorazioneFlags.Attivo, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dello stato sorgente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDStatoSorgente
            {
                get
                {
                    return DBUtils.GetID(m_StatoSorgente, m_IDStatoSorgente);
                }

                set
                {
                    int oldValue = IDStatoSorgente;
                    if (oldValue == value)
                        return;
                    var oldStato = StatoSorgente;
                    m_IDStatoSorgente = value;
                    m_StatoSorgente = null;
                    DoChanged("IDStatoSorgente", value, oldValue);
                    if (oldStato is object)
                        oldStato.NotifyRegolaChanged(this);
                }
            }

            /// <summary>
            /// Restituisce o impsota lo stato origine
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoTaskLavorazione StatoSorgente
            {
                get
                {
                    if (m_StatoSorgente is null)
                        m_StatoSorgente = StatiTasksLavorazione.GetItemById(m_IDStatoSorgente);
                    return m_StatoSorgente;
                }

                set
                {
                    var oldValue = StatoSorgente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoSorgente = value;
                    m_IDStatoSorgente = DBUtils.GetID(value, 0);
                    DoChanged("StatoSorgente", value, oldValue);
                    if (oldValue is object)
                        oldValue.NotifyRegolaChanged(this);
                }
            }

            /// <summary>
            /// Imposta lo stato sorgente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetStatoSorgente(StatoTaskLavorazione value)
            {
                m_StatoSorgente = value;
                m_IDStatoSorgente = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dello stato destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDStatoDestinazione
            {
                get
                {
                    return DBUtils.GetID(m_StatoDestinazione, m_IDStatoDestinazione);
                }

                set
                {
                    int oldValue = IDStatoDestinazione;
                    if (oldValue == value)
                        return;
                    m_IDStatoDestinazione = value;
                    m_StatoDestinazione = null;
                    DoChanged("IDStatoDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoTaskLavorazione StatoDestinazione
            {
                get
                {
                    if (m_StatoDestinazione is null)
                        m_StatoDestinazione = StatiTasksLavorazione.GetItemById(m_IDStatoDestinazione);
                    return m_StatoDestinazione;
                }

                set
                {
                    var oldValue = StatoDestinazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoDestinazione = value;
                    m_IDStatoDestinazione = DBUtils.GetID(value, 0);
                    DoChanged("StatoDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.RegoleTasksLavorazione; // Anagrafica.RegoleTasksLavorazione.Module
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return TasksDiLavorazione.Database;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_TaskLavorazioneRegole";
            }


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this. m_Nome);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_IDStatoSorgente = reader.Read("IDStatoSorgente", this.m_IDStatoSorgente);
                m_IDStatoDestinazione = reader.Read("IDStatoDestinazione", this.m_IDStatoDestinazione);
                m_NomeHandler = reader.Read("NomeHandler", this.m_NomeHandler);
                m_ContextType = reader.Read("ContextType", this.m_ContextType);
                m_Ordine = reader.Read("Ordine", this.m_Ordine);
                 
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDStatoSorgente", IDStatoSorgente);
                writer.Write("IDStatoDestinazione", IDStatoDestinazione);
                writer.Write("NomeHandler", m_NomeHandler);
                writer.Write("ContextType", m_ContextType);
                writer.Write("Ordine", m_Ordine);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("IDStatoSorgente", typeof(int), 1);
                c = table.Fields.Ensure("IDStatoDestinazione", typeof(int), 1);
                c = table.Fields.Ensure("NomeHandler", typeof(string), 255);
                c = table.Fields.Ensure("ContextType", typeof(string), 255);
                c = table.Fields.Ensure("Ordine", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Flags" , "NomeHandler" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFromTo", new string[] { "IDStatoSorgente", "IDStatoDestinazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContext", new string[] { "ContextType", "Ordine" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Descrizione", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("IDStatoSorgente", IDStatoSorgente);
                writer.WriteAttribute("IDStatoDestinazione", IDStatoDestinazione);
                writer.WriteAttribute("NomeHandler", m_NomeHandler);
                writer.WriteAttribute("ContextType", m_ContextType);
                writer.WriteAttribute("Ordine", m_Ordine);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDStatoSorgente":
                        {
                            m_IDStatoSorgente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDStatoDestinazione":
                        {
                            m_IDStatoDestinazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeHandler":
                        {
                            m_NomeHandler = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContextType":
                        {
                            m_ContextType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Ordine":
                        {
                            m_Ordine = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void OnAfterSave(DMDEventArgs e)
            {
                base.OnAfterSave(e);
                if (this.StatoSorgente is object )
                    StatoSorgente.NotifyRegolaChanged(this);
            }

           
            /// <summary>
            /// Esegue la regola
            /// </summary>
            /// <param name="taskSorgente"></param>
            /// <param name="parametri"></param>
            /// <returns></returns>
            public object Execute(TaskLavorazione taskSorgente, string parametri)
            {
                ITaskHandler handler = (ITaskHandler)Sistema.ApplicationContext.CreateInstance(NomeHandler);
                return handler.Handle(taskSorgente, this, parametri);
            }

            /// <summary>
            /// Crea un nuovo task
            /// </summary>
            /// <param name="cliente"></param>
            /// <param name="parametri"></param>
            /// <returns></returns>
            public TaskLavorazione NewTask(CPersona cliente, string parametri)
            {
                var taskSorgente = new TaskLavorazione();
                taskSorgente.AssegnatoDa = Sistema.Users.CurrentUser;
                taskSorgente.AssegnatoA = Sistema.Users.CurrentUser;
                taskSorgente.Categoria = StatoSorgente.Categoria;
                taskSorgente.StatoAttuale = StatoSorgente;
                taskSorgente.DataInizioEsecuzione = DMD.DateUtils.Now();
                taskSorgente.Cliente = cliente;
                taskSorgente.PuntoOperativo = cliente.PuntoOperativo;
                taskSorgente.Stato = ObjectStatus.OBJECT_VALID;
                taskSorgente.Save();
                ITaskHandler handler = (ITaskHandler)Sistema.ApplicationContext.CreateInstance(NomeHandler);
                return handler.Handle(taskSorgente, this, parametri);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((RegolaTaskLavorazione)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(RegolaTaskLavorazione obj)
            {
                int ret = DMD.Arrays.Compare(m_Ordine, obj.m_Ordine);
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Descrizione, obj.m_Descrizione, true);
                return ret;
            }

            /// <summary>
            /// Restitusice una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray(
                            "Rule: ", this.Nome, 
                            " (", this.IDStatoSorgente, " to ", this.IDStatoDestinazione, ")"
                            );
            }

            /// <summary>
            /// Calcola il codice hash
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome, this.IDStatoSorgente, this.IDStatoDestinazione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is RegolaTaskLavorazione) && this.Equals((RegolaTaskLavorazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RegolaTaskLavorazione obj)
            {
                return base.Equals(obj)
                            && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                            && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                            && DMD.Integers.EQ(this.m_IDStatoSorgente, obj.m_IDStatoSorgente)
                            && DMD.Integers.EQ(this.m_IDStatoDestinazione, obj.m_IDStatoDestinazione)
                            && DMD.Strings.EQ(this.m_NomeHandler, obj.m_NomeHandler)
                            && DMD.Strings.EQ(this.m_ContextType, obj.m_ContextType)
                            && DMD.Integers.EQ(this.m_Flags, obj.m_Flags)
                            && DMD.Integers.EQ(this.m_Ordine, obj.m_Ordine)
                            ;
                     //private CKeyCollection m_Parameters;

            }
        }
    }
}