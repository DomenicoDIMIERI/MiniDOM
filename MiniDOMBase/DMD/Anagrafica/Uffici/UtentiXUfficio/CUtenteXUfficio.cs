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
    public partial class Anagrafica
    {



        /// <summary>
        /// Relazione tra un utente ed un ufficio
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUtenteXUfficio
            : Databases.DBObjectBase
        {
            private int m_IDUtente; 
            [NonSerialized] private CUser m_Utente;
            private int m_IDUfficio;  
            [NonSerialized] private CUfficio m_Ufficio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUtenteXUfficio()
            {
                m_IDUtente = 0;
                m_Utente = null;
                m_IDUfficio = 0;
                m_Ufficio = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="idUfficio"></param>
            /// <param name="idUtente"></param>
            public CUtenteXUfficio(int idUfficio, int idUtente)
            {
                m_IDUfficio = idUfficio;
                m_IDUtente = idUtente;
                m_Utente = null;
                m_Ufficio = null;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Uffici.UfficiConsentiti;
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'utente
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
            public CUser Utente
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
                    if (oldValue == value)
                        return;
                    m_Utente = value;
                    m_IDUtente = DBUtils.GetID(value, 0);
                    DoChanged("Utente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'ufficio
            /// </summary>
            public int IDUfficio
            {
                get
                {
                    return DBUtils.GetID(m_Ufficio, m_IDUfficio);
                }

                set
                {
                    int oldValue = IDUfficio;
                    if (oldValue == value)
                        return;
                    m_IDUfficio = value;
                    m_Ufficio = null;
                    DoChanged("IDUfficio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ufficio
            /// </summary>
            public CUfficio Ufficio
            {
                get
                {
                    if (m_Ufficio is null)
                        m_Ufficio = Uffici.GetItemById(m_IDUfficio);
                    return m_Ufficio;
                }

                set
                {
                    var oldValue = Ufficio;
                    if (oldValue == value)
                        return;
                    m_Ufficio = value;
                    m_IDUfficio = DBUtils.GetID(value, 0);
                    DoChanged("Ufficio", value, oldValue);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_UtentiXUfficio";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDUtente = reader.Read("Utente", this.m_IDUtente);
                this.m_IDUfficio = reader.Read("Ufficio", this.m_IDUfficio);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Utente", IDUtente);
                writer.Write("Ufficio", IDUfficio);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Utente", typeof(int), 1);
                c = table.Fields.Ensure("Ufficio", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUtenteUfficio", new string[] { "Utente", "Ufficio" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("UtenteXUfficio[" , IDUtente , ", " , IDUfficio , "]");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDUfficio, this.m_IDUtente);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CUtenteXUfficio) && this.Equals((CUtenteXUfficio)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CUtenteXUfficio obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDUtente, obj.m_IDUtente)
                    && DMD.Integers.EQ(this.m_IDUfficio, obj.m_IDUfficio)
                    ;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_IDUtente", IDUtente);
                writer.WriteAttribute("m_IDUfficio", IDUfficio);
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
                    case "m_IDUtente":
                        {
                            m_IDUtente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_IDUfficio":
                        {
                            m_IDUfficio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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