using System;
using System.Collections;
using System.Collections.Generic;
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
        /// flag di un task
        /// </summary>
        [Flags]
        public enum StatoTaskLavorazioneFlags : int
        {
            /// <summary>
            /// Nessuno
            /// </summary>
            None = 0,

            /// <summary>
            /// Attivo
            /// </summary>
            Attivo = 1,

            /// <summary>
            /// Stato iniziale
            /// </summary>
            Iniziale = 2,

            /// <summary>
            /// Stato finale
            /// </summary>
            Finale = 4,

            /// <summary>
            /// Stato nascosto
            /// </summary>
            Nascosto = 8,

            /// <summary>
            /// Stato raggiungibile solo da utenti privilegiati
            /// </summary>
            Privilegiato = 16,

            /// <summary>
            /// Lo stato appartiene ad un processo di lavorazione
            /// </summary>
            InLavorazione = 32,

            /// <summary>
            /// Lo stato richiede il ricontatto
            /// </summary>
            RichiedeRicontatto = 64,

            /// <summary>
            /// Lo stato indica che il processo di lavorazione é completato
            /// </summary>
            Completato = 128,

            /// <summary>
            /// Lo stato indica che il processo di lavorazione é annullato
            /// </summary>
            Annullato = 256
        }

        /// <summary>
        /// Macrostati
        /// </summary>
        public enum MacroStatoLavorazione : int
        {
            /// <summary>
            /// Non iniziato
            /// </summary>
            Inattivo = 0,

            /// <summary>
            /// Attivo
            /// </summary>
            Attivo = 10,

            /// <summary>
            /// Da contattare
            /// </summary>
            DaContattare = 20,

            /// <summary>
            /// In contatto
            /// </summary>
            InContatto = 30,

            /// <summary>
            /// In trattativa
            /// </summary>
            InTrattativa = 40,

            /// <summary>
            /// non interessato
            /// </summary>
            NonInteressato = 50,

            /// <summary>
            /// In lavorazione
            /// </summary>
            InLavorazione = 60,

            /// <summary>
            /// Rifiutato dal cliente
            /// </summary>
            Rifiutato = 70,

            /// <summary>
            /// Bocciato dall'agenzia
            /// </summary>
            Bocciato = 80,

            /// <summary>
            /// Non fattibile
            /// </summary>
            NonFattibile = 90,

            /// <summary>
            /// Annullato
            /// </summary>
            Annullato = 100
        }

        /// <summary>
        /// Rappresenta uno stato di lavorazione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class StatoTaskLavorazione
            : Databases.DBObject
        {
            private string m_Nome;
            private string m_Categoria;
            private string m_Descrizione;
            private string m_Descrizione2;
            private MacroStatoLavorazione m_MacroStato;
            private int m_IDStatoSuccessivoPredefinito;
            private StatoTaskLavorazione m_StatoSuccessivoPredefinito;
            private RegoleTaskLavorazionePerStato m_Regole;
            private TipoPersona m_SiApplicaA;
            private string m_NomeHandler;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public StatoTaskLavorazione()
            {
                m_Nome = "";
                m_Descrizione = "";
                m_Descrizione2 = "";
                m_Categoria = "";
                m_MacroStato = MacroStatoLavorazione.Inattivo;
                m_Flags = (int)StatoTaskLavorazioneFlags.Attivo;
                m_IDStatoSuccessivoPredefinito = 0;
                m_StatoSuccessivoPredefinito = null;
                m_Regole = null;
                m_SiApplicaA = TipoPersona.PERSONA_FISICA;
                m_NomeHandler = "";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome, this.m_SiApplicaA);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is StatoTaskLavorazione) && this.Equals((StatoTaskLavorazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(StatoTaskLavorazione obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_Descrizione2, obj.m_Descrizione2)
                    && DMD.Integers.EQ((int)this.m_MacroStato, (int)obj.m_MacroStato)
                    && DMD.Integers.EQ(this.m_IDStatoSuccessivoPredefinito, obj.m_IDStatoSuccessivoPredefinito)
                    && DMD.Integers.EQ((int)this.m_SiApplicaA, (int)obj.m_SiApplicaA)
                    && DMD.Strings.EQ(this.m_NomeHandler, obj.m_NomeHandler)
                    ;
            //private StatoTaskLavorazione m_StatoSuccessivoPredefinito;
            //private RegoleTaskLavorazionePerStato m_Regole;
            }
             

            /// <summary>
            /// NomeHandler
            /// </summary>
            public string NomeHandler
            {
                get
                {
                    return m_NomeHandler;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeHandler;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeHandler = value;
                    DoChanged("NomeHandler", value, oldValue);
                }
            }

            /// <summary>
            /// SiApplicaA
            /// </summary>
            public TipoPersona SiApplicaA
            {
                get
                {
                    return m_SiApplicaA;
                }

                set
                {
                    var oldValue = m_SiApplicaA;
                    if (oldValue == value)
                        return;
                    m_SiApplicaA = value;
                    DoChanged("SiApplicaA", value, oldValue);
                }
            }

            /// <summary>
            /// MacroStato
            /// </summary>
            public MacroStatoLavorazione MacroStato
            {
                get
                {
                    return m_MacroStato;
                }

                set
                {
                    var oldValue = m_MacroStato;
                    if (oldValue == value)
                        return;
                    m_MacroStato = value;
                    DoChanged("MacroStato", value, oldValue);
                }
            }

            // Private regoleLock As New Object

            /// <summary>
            /// Regole
            /// </summary>
            public RegoleTaskLavorazionePerStato Regole
            {
                get
                {
                    // SyncLock Me.regoleLock
                    if (m_Regole is null)
                        m_Regole = new RegoleTaskLavorazionePerStato(this);
                    return m_Regole;
                    // End SyncLock
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
            /// Restituisce o imposta la categoria dello stato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    string oldValue = m_Categoria;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
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
            /// Descrizione2
            /// </summary>
            public string Descrizione2
            {
                get
                {
                    return m_Descrizione2;
                }

                set
                {
                    string oldValue = m_Descrizione2;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione2 = value;
                    DoChanged("Descrizione2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un itero utilizzabile per i flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new StatoTaskLavorazioneFlags Flags
            {
                get
                {
                    return (StatoTaskLavorazioneFlags) this.m_Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
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
                    return DMD.RunTime.TestFlag(this.Flags, StatoTaskLavorazioneFlags.Attivo);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, StatoTaskLavorazioneFlags.Attivo, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se lo stato può essere usato come stato di partenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Iniziale
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, StatoTaskLavorazioneFlags.Iniziale);
                }

                set
                {
                    if (Iniziale == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, StatoTaskLavorazioneFlags.Iniziale, value);
                    DoChanged("Iniziale", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se lo stato può essere usato come stato finale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Finale
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, StatoTaskLavorazioneFlags.Finale);
                }

                set
                {
                    if (Finale == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, StatoTaskLavorazioneFlags.Finale, value);
                    DoChanged("Finale", value, !value);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID dello stato di lavorazione successivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDStatoSuccessivoPredefinito
            {
                get
                {
                    return DBUtils.GetID(m_StatoSuccessivoPredefinito, m_IDStatoSuccessivoPredefinito);
                }

                set
                {
                    int oldValue = IDStatoSuccessivoPredefinito;
                    if (oldValue == value)
                        return;
                    m_IDStatoSuccessivoPredefinito = value;
                    m_StatoSuccessivoPredefinito = null;
                    DoChanged("IDStatoSuccessivoPredefinito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato successivo predefinito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoTaskLavorazione StatoSuccessivoPredefinito
            {
                get
                {
                    if (m_StatoSuccessivoPredefinito is null)
                        m_StatoSuccessivoPredefinito = StatiTasksLavorazione.GetItemById(m_IDStatoSuccessivoPredefinito);
                    return m_StatoSuccessivoPredefinito;
                }

                set
                {
                    var oldValue = StatoSuccessivoPredefinito;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoSuccessivoPredefinito = value;
                    m_IDStatoSuccessivoPredefinito = DBUtils.GetID(value, 0);
                    DoChanged("StatoSuccessivoPredefinito", value, oldValue);
                }
            }

            /// <summary>
            /// Respository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.StatiTasksLavorazione; //.Module;
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
                return "tbl_TaskLavorazioneStati";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_Descrizione2 = reader.Read("Descrizione2", m_Descrizione2);
                m_IDStatoSuccessivoPredefinito = reader.Read("IDStatoSuccessivo", m_IDStatoSuccessivoPredefinito);
                m_MacroStato = reader.Read("MacroStato", m_MacroStato);
                m_Categoria = reader.Read("Categoria", m_Categoria);
                m_SiApplicaA = reader.Read("SiApplicaA", m_SiApplicaA);
                m_NomeHandler = reader.Read("NomeHandler", m_NomeHandler);
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
                writer.Write("Descrizione2", m_Descrizione2);
                writer.Write("IDStatoSuccessivo", IDStatoSuccessivoPredefinito);
                writer.Write("MacroStato", m_MacroStato);
                writer.Write("Categoria", m_Categoria);
                writer.Write("SiApplicaA", m_SiApplicaA);
                writer.Write("NomeHandler", m_NomeHandler);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("Descrizione2", typeof(string), 0);
                c = table.Fields.Ensure("IDStatoSuccessivo", typeof(int), 1);
                c = table.Fields.Ensure("IDStMacroStatoatoSuccessivo", typeof(int), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("SiApplicaA", typeof(int), 1);
                c = table.Fields.Ensure("NomeHandler", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "NomeHandler" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescr", new string[] { "Descrizione", "Descrizione2" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStati", new string[] { "IDStatoSuccessivo", "IDStMacroStatoatoSuccessivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCateg", new string[] { "SiApplicaA", "Categoria" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Descrizione2", m_Descrizione2);
                writer.WriteAttribute("IDStatoSuccessivo", IDStatoSuccessivoPredefinito);
                writer.WriteAttribute("MacroStato", (int?)m_MacroStato);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("SiApplicaA", (int?)m_SiApplicaA);
                writer.WriteAttribute("NomeHandler", m_NomeHandler);
                base.XMLSerialize(writer);
            }

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

                    case "Descrizione2":
                        {
                            m_Descrizione2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                         

                    case "IDStatoSuccessivo":
                        {
                            m_IDStatoSuccessivoPredefinito = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MacroStato":
                        {
                            m_MacroStato = (MacroStatoLavorazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SiApplicaA":
                        {
                            m_SiApplicaA = (TipoPersona)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeHandler":
                        {
                            m_NomeHandler = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Notifica a questo oggetto che una regola che lo utilizza é stata modificata
            /// </summary>
            /// <param name="regola"></param>
            protected internal void NotifyRegolaChanged(RegolaTaskLavorazione regola)
            {
                // SyncLock Me.regoleLock
                if (m_Regole is null)
                    return;
                var r = m_Regole.GetItemById(DBUtils.GetID(regola, 0));
                if (regola.IDStatoSorgente == DBUtils.GetID(this, 0) && regola.Stato == ObjectStatus.OBJECT_VALID)
                {
                    if (r is null)
                    {
                        m_Regole.Add(regola);
                    }
                    else
                    {
                        m_Regole[m_Regole.IndexOf(r)] = regola;
                    }
                }
                else if (r is object)
                {
                    m_Regole.Remove(r);
                }
                // End SyncLock
            }
        }
    }
}