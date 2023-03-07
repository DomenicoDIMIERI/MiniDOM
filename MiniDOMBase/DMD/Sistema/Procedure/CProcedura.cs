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

namespace minidom
{
    public partial class Sistema
    {
        /// <summary>
        /// Flag validi per una procedura
        /// </summary>
        [Flags]
        public enum ProceduraFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Procedura disabilitata
            /// </summary>
            Disabilitata = 1
        }


        /// <summary>
        /// Procedura di sistema
        /// </summary>
        [Serializable]
        public class CProcedura 
            : minidom.Databases.DBObject, ISchedulable
        {
            private string m_Tipo;
            private string m_Nome;
            private MultipleScheduleCollection m_Programmazione;   // Parametri di programmazione della campagna
            private PriorityEnum m_Priority;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CProcedura()
            {
                m_Tipo = "";
                m_Nome = "";
                m_Flags = (int) ProceduraFlags.None;
                m_Programmazione = null;
                m_Priority = PriorityEnum.PRIORITY_NORMAL;
                // Me.m_UltimaEsecuzione = Nothing
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
                return HashCalculator.Calculate(this.m_Nome, this.m_Tipo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CProcedura) && this.Equals((CProcedura)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CProcedura obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Integers.EQ((int)this.m_Priority, (int)obj.m_Priority)
                    ;
                    //MultipleScheduleCollection m_Programmazione;   // Parametri di programmazione della campagna
            }

            /// <summary>
            /// Restituisce o imposta la priorietà assegnata alla procedura
            /// </summary>
            public PriorityEnum Priority
            {
                get
                {
                    return m_Priority;
                }

                set
                {
                    var oldValue = m_Priority;
                    if (oldValue == value)
                        return;
                    m_Priority = value;
                    DoChanged("Priority", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della procedura
            /// </summary>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    string oldValue = m_Tipo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della procedura
            /// </summary>
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
            /// Restituisce o imposta dei flag aggiuntivi
            /// </summary>
            public new ProceduraFlags Flags
            {
                get
                {
                    return (ProceduraFlags)base.Flags;
                }

                set
                {
                    base.Flags = (int)value;
                }
            }

            /// <summary>
            /// Restituisce la programmazione dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MultipleScheduleCollection Programmazione
            {
                get
                {
                    lock (this)
                    {
                        if (m_Programmazione is null)
                            m_Programmazione = new MultipleScheduleCollection(this);
                        return m_Programmazione;
                    }
                }
            }

            /// <summary>
            /// Invalida la programmazione
            /// </summary>
            internal void InvalidateProgrammazione()
            {
                lock (this)
                    m_Programmazione = null;
            }
             
            /// <summary>
            /// Esegue la procedura
            /// </summary>
            public void Run()
            {
                IProceduraHandler handler = (IProceduraHandler)minidom.Sistema.ApplicationContext.CreateInstance(this.Tipo);
                handler.Run(this);
            }

            /// <summary>
            /// Rrepository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Procedure; //.Module;
            }


            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    m_Programmazione = null;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CalendarProcs";
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (m_Programmazione is object)
                    m_Programmazione.Save(this.GetConnection(), force);
            }


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Tipo = reader.Read("Tipo", this.m_Tipo);
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Priority = reader.Read("Priorita", this.m_Priority);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Tipo", m_Tipo);
                writer.Write("Nome", m_Nome);
                writer.Write("Priorita", m_Priority);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Tipo", typeof(int), 1);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxTipo", new string[] { "Tipo", "Nome", "Priorita" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Priorita", (int?)m_Priority);
                base.XMLSerialize(writer);
                writer.WriteTag("Programmazione", Programmazione);
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
                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                     
                    case "Programmazione":
                        {
                            m_Programmazione = (MultipleScheduleCollection)fieldValue;
                            m_Programmazione.SetOwner(this);
                            break;
                        }

                    case "Priorita":
                        {
                            m_Priority = (PriorityEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Restituisce la data della prossima esecuzione della procedura
            /// </summary>
            public DateTime? ProssimaEsecuzione
            {
                get
                {
                    DateTime? ret = default;
                    foreach (CalendarSchedule s in Programmazione)
                    {
                        var p = s.CalcolaProssimaEsecuzione();
                        if (p.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                if (p.Value < ret.Value)
                                    ret = p;
                            }
                            else
                            {
                                ret = p;
                            }
                        }
                    }

                    return ret;
                }
            }

            void ISchedulable.InvalidateProgrammazione() { this.InvalidateProgrammazione();  }
            void ISchedulable.NotifySchedule(CalendarSchedule s) { this.NotifySchedule(s); }
            MultipleScheduleCollection ISchedulable.Programmazione { get { return this.Programmazione; } }

            /// <summary>
            /// Restituisce la data dell'ultima esecuzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? UltimaEsecuzione
            {
                get
                {
                    DateTime? ret = default;
                    foreach (CalendarSchedule s in Programmazione)
                    {
                        var p = s.UltimaEsecuzione;
                        if (p.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                if (ret.Value < p.Value)
                                    ret = p;
                            }
                            else
                            {
                                ret = p;
                            }
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Resetta i parametri
            /// </summary>
            public void ResettaParametri()
            {
                this.Parameters.Clear();
                IProceduraHandler handler = (IProceduraHandler)minidom.Sistema.ApplicationContext.CreateInstance(Tipo);
                handler.InitializeParameters(this);
            }

            /// <summary>
            /// Notifica la modifica di una programmazione
            /// </summary>
            /// <param name="s"></param>
            protected internal void NotifySchedule(CalendarSchedule s)
            {
                lock (this)
                {
                    if (m_Programmazione is null)
                        return;
                    var o = m_Programmazione.GetItemById(DBUtils.GetID(s));
                    if (ReferenceEquals(o, s))
                    {
                        return;
                    }

                    if (o is object)
                    {
                        int i = m_Programmazione.IndexOf(o);
                        if (s.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            m_Programmazione[i] = s;
                        }
                        else
                        {
                            m_Programmazione.RemoveAt(i);
                        }
                    }
                    else if (s.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        m_Programmazione.Add(s);
                    }
                }
            }
        }
    }
}