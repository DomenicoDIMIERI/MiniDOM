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
        /// Rappresenta la definizione di percorso
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class LuogoDaVisitare 
            : minidom.Databases.DBObject, IComparable, IComparable<LuogoDaVisitare>
        {
            private string m_Etichetta;
            private int m_IDPercorso;
            [NonSerialized] private PercorsoDefinito m_Percorso;
            private Anagrafica.CIndirizzo m_Indirizzo;
            private int m_IDPersona;
            [NonSerialized] private Anagrafica.CPersona m_Persona;
            private string m_NomePersona;
            private int m_Progressivo;
            private PriorityEnum m_Priorita;

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuogoDaVisitare()
            {
                m_Etichetta = "";
                m_IDPercorso = 0;
                m_Percorso = null;
                m_Indirizzo = new Anagrafica.CIndirizzo();
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_Progressivo = 0;
                m_Priorita = PriorityEnum.PRIORITY_NORMAL;
            }



            /// <summary>
            /// Restituisce o imposta un numero che indica l'ordine da seguire in un elenco di luoghi da visitare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Progressivo
            {
                get
                {
                    return m_Progressivo;
                }

                set
                {
                    int oldValue = m_Progressivo;
                    if (oldValue == value)
                        return;
                    m_Progressivo = value;
                    DoChanged("Progressivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la priorità da dare a questo luogo (in caso non si possano effettuare tutti)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PriorityEnum Priorita
            {
                get
                {
                    return m_Priorita;
                }

                set
                {
                    var oldValue = m_Priorita;
                    if (oldValue == value)
                        return;
                    m_Priorita = value;
                    DoChanged("Priorita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una etichetta che identifica il luogo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Etichetta
            {
                get
                {
                    return m_Etichetta;
                }

                set
                {
                    string oldValue = m_Etichetta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Etichetta = value;
                    DoChanged("Etichetta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del percorso a cui appartiene questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPercorso
            {
                get
                {
                    return DBUtils.GetID(m_Percorso, m_IDPercorso);
                }

                set
                {
                    int oldValue = IDPercorso;
                    if (oldValue == value)
                        return;
                    m_IDPercorso = value;
                    m_Percorso = null;
                    DoChanged("IDPercorso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso a cui appartiene questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PercorsoDefinito Percorso
            {
                get
                {
                    if (m_Percorso is null)
                        m_Percorso = minidom.Office.PercorsiDefiniti.GetItemById(m_IDPercorso);
                    return m_Percorso;
                }

                set
                {
                    var oldValue = m_Percorso;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Percorso = value;
                    m_IDPercorso = DBUtils.GetID(value, 0);
                    DoChanged("Percorso", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il percorso
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetPercorso(PercorsoDefinito value)
            {
                m_Percorso = value;
                m_IDPercorso = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce l'indirizzo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona o dell'azienda da visitare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta la persona o l'azienda da visitare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = minidom.Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona o dell'azienda da visitare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    string oldValue = m_NomePersona;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

       
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.PercorsiDefiniti.LuoghiDaVisitare;
            }

            /// <summary>
            /// Dscriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeDefPercorsiL";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || m_Indirizzo.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_Indirizzo.SetChanged(false);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Progressivo = reader.Read("Progressivo", m_Progressivo);
                m_Priorita = reader.Read("Priorita", m_Priorita);
                m_Etichetta = reader.Read("Etichetta", m_Etichetta);
                m_IDPercorso = reader.Read("IDPercorso", m_IDPercorso);
                m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", m_Indirizzo.CAP);
                m_Indirizzo.ToponimoEVia = reader.Read("Indirizzo_Via", m_Indirizzo.ToponimoEVia);
                m_Indirizzo.Civico = reader.Read("Indirizzo_Civico", m_Indirizzo.Civico);
                m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", m_Indirizzo.Citta);
                m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", m_Indirizzo.Provincia);
                m_Indirizzo.Latitude = reader.Read("Lat", m_Indirizzo.Latitude);
                m_Indirizzo.Longitude = reader.Read("Lng", m_Indirizzo.Longitude);
                m_Indirizzo.Altitude = reader.Read("Alt", m_Indirizzo.Altitude);
                m_Indirizzo.SetChanged(false);
                m_IDPersona = reader.Read("IDPersona", m_IDPersona);
                m_NomePersona = reader.Read("NomePersona", m_NomePersona);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Progressivo", m_Progressivo);
                writer.Write("Priorita", m_Priorita);
                writer.Write("Etichetta", m_Etichetta);
                writer.Write("IDPercorso", IDPercorso);
                writer.Write("Indirizzo_CAP", m_Indirizzo.CAP);
                writer.Write("Indirizzo_Via", m_Indirizzo.ToponimoEVia);
                writer.Write("Indirizzo_Civico", m_Indirizzo.Civico);
                writer.Write("Indirizzo_Citta", m_Indirizzo.Citta);
                writer.Write("Indirizzo_Provincia", m_Indirizzo.Provincia);
                writer.Write("Lat", m_Indirizzo.Latitude);
                writer.Write("Lng", m_Indirizzo.Longitude);
                writer.Write("Alt", m_Indirizzo.Altitude);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Progressivo", typeof(int), 1);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
                c = table.Fields.Ensure("Etichetta", typeof(string), 255);
                c = table.Fields.Ensure("IDPercorso", typeof(int), 1);
                c = table.Fields.Ensure("Indirizzo_CAP", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Via", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Civico", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Citta", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Lat", typeof(double), 1);
                c = table.Fields.Ensure("Lng", typeof(double), 1);
                c = table.Fields.Ensure("Alt", typeof(double), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPercorsor", new string[] { "Progressivo", "IDPercorso" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo1", new string[] { "Indirizzo_CAP", "Indirizzo_Provincia", "Indirizzo_Citta", "Indirizzo_Via", "Indirizzo_Civico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo2", new string[] { "Lat", "Lng", "Alt" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPriorita", new string[] { "Priorita", "Etichetta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None);

                 

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Progressivo", m_Progressivo);
                writer.WriteAttribute("Priorita", (int?)m_Priorita);
                writer.WriteAttribute("Etichetta", m_Etichetta);
                writer.WriteAttribute("IDPercorso", IDPercorso);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                base.XMLSerialize(writer);
                writer.WriteTag("Indirizzo", m_Indirizzo);
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
                    case "Progressivo":
                        {
                            m_Progressivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Priorita":
                        {
                            m_Priorita = (PriorityEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Etichetta":
                        {
                            m_Etichetta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPercorso":
                        {
                            this.m_IDPercorso = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "IDPersona":
                        {
                            this.m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return m_Etichetta;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Etichetta);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((LuogoDaVisitare)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual int CompareTo(LuogoDaVisitare obj)
            {
                int ret = m_Progressivo - obj.m_Progressivo;
                if (ret == 0) ret = DMD.Strings.Compare(m_Etichetta, obj.m_Etichetta, true);
                return ret;
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is LuogoDaVisitare) && this.Equals((LuogoDaVisitare)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(LuogoDaVisitare obj)
            {
                return base.Equals(obj)
                    &&  DMD.Strings.EQ(this.m_Etichetta, obj.m_Etichetta)
                    && DMD.Integers.EQ((int)this.m_IDPercorso, (int)obj.m_IDPercorso)
                    && this.m_Indirizzo.Equals(obj.m_Indirizzo)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona, true)
                    && DMD.Integers.EQ(this.m_Progressivo, obj.m_Progressivo)
                    && DMD.Integers.EQ( (int)this.m_Priorita, (int)obj.m_Priorita)
                    ;
            }
        }
    }
}