using System;
using DMD.XML;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flags definiti per una postazione di lavoro
        /// </summary>
        [Flags]
        public enum FlagsPostazioneLavoro : int
        {

            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }

        /// <summary>
        /// Rappresenta una postazione di lavoro
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CPostazione 
            : Databases.DBObjectPO, IComparable, IComparable<CPostazione>
        {
            private string m_Nome;
            private int m_IDUtentePrincipale;
            [NonSerialized] private Sistema.CUser m_UtentePrincipale;
            private string m_NomeUtentePrincipale;
            private string m_NomeReparto;
            private string m_Categoria;
            private string m_SottoCategoria;
            private string m_InternoTelefonico;
            private CUtentiXPostazioneCollection m_Utenti;
            private string m_SistemaOperativo;
            private string m_Note;
            private CCollection<RegistroContatore> m_RegistriContatori;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPostazione()
            {
                m_Nome = "";
                m_Flags = (int)FlagsPostazioneLavoro.None;
                m_IDUtentePrincipale = 0;
                m_UtentePrincipale = null;
                m_NomeUtentePrincipale = "";
                m_NomeReparto = "";
                m_InternoTelefonico = "";
                m_Utenti = null;
                m_SistemaOperativo = "";
                 
                m_Note = "";
                m_RegistriContatori = null;
                m_Categoria = "";
                m_SottoCategoria = "";
            }

            /// <summary>
            /// Restituisce o imposta la categoria della postazione (PC, stampante, ecc..)
            /// </summary>
            /// <returns></returns>
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
            /// SottoCategoria
            /// </summary>
            public string SottoCategoria
            {
                get
                {
                    return m_SottoCategoria;
                }

                set
                {
                    string oldValue = m_SottoCategoria;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SottoCategoria = value;
                    DoChanged("SottoCategoria", value, oldValue);
                }
            }

            /// <summary>
            /// RegistriContatori
            /// </summary>
            public CCollection<RegistroContatore> RegistriContatori
            {
                get
                {
                    if (m_RegistriContatori is null)
                        m_RegistriContatori = new CCollection<RegistroContatore>();
                    return m_RegistriContatori;
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della stazione di lavoro
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags
            /// </summary>
            /// <returns></returns>
            public new FlagsPostazioneLavoro Flags
            {
                get
                {
                    return (FlagsPostazioneLavoro) base.Flags;
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
            /// Restituisce o imposta l'ID dell'utente che normalmente utilizza la stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public int IDUtentePrincipale
            {
                get
                {
                    return DBUtils.GetID(m_UtentePrincipale, m_IDUtentePrincipale);
                }

                set
                {
                    int oldValue = IDUtentePrincipale;
                    if (oldValue == value)
                        return;
                    m_UtentePrincipale = null;
                    m_IDUtentePrincipale = value;
                    DoChanged("IDUtentePrincipale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che normalmente utilizza la stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public Sistema.CUser UtentePrincipale
            {
                get
                {
                    if (m_UtentePrincipale is null)
                        m_UtentePrincipale = Sistema.Users.GetItemById(m_IDUtentePrincipale);
                    return m_UtentePrincipale;
                }

                set
                {
                    var oldValue = UtentePrincipale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_UtentePrincipale = value;
                    m_IDUtentePrincipale = DBUtils.GetID(value, 0);
                    m_NomeUtentePrincipale = "";
                    if (value is object)
                        m_NomeUtentePrincipale = value.Nominativo;
                    DoChanged("UtentePrincipale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che normalmente utilizza la stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public string NomeUtentePrincipale
            {
                get
                {
                    return m_NomeUtentePrincipale;
                }

                set
                {
                    string oldValue = m_NomeUtentePrincipale;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUtentePrincipale = value;
                    DoChanged("NomeUtentePrincipale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del reparto in cui si trova la stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public string NomeReparto
            {
                get
                {
                    return m_NomeReparto;
                }

                set
                {
                    string oldValue = m_NomeReparto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeReparto = value;
                    DoChanged("NomeReparto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero telefonico interno associato alla stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public string InternoTelefonico
            {
                get
                {
                    return m_InternoTelefonico;
                }

                set
                {
                    string oldValue = m_InternoTelefonico;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_InternoTelefonico = value;
                    DoChanged("InternoTelefonico", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'elenco degli utenti definiti sulla stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public CUtentiXPostazioneCollection Utenti
            {
                get
                {
                    if (m_Utenti is null)
                        m_Utenti = new CUtentiXPostazioneCollection(this);
                    return m_Utenti;
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del sistema operativo installato sulla stazione di lavoro
            /// </summary>
            /// <returns></returns>
            public string SistemaOperativo
            {
                get
                {
                    return m_SistemaOperativo;
                }

                set
                {
                    string oldValue = m_SistemaOperativo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SistemaOperativo = value;
                    DoChanged("SistemaOperativo", value, oldValue);
                }
            }

           

            /// <summary>
            /// Restituisce o imposta la descrizione della stazione
            /// </summary>
            /// <returns></returns>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Postazioni; //.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PostazioniLavoro";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this. m_Nome);
                m_IDUtentePrincipale = reader.Read("IDUtentePrincipale", this.m_IDUtentePrincipale);
                m_NomeUtentePrincipale = reader.Read("NomeUtentePrincipale", this.m_NomeUtentePrincipale);
                m_NomeReparto = reader.Read("NomeReparto", this.m_NomeReparto);
                m_InternoTelefonico = reader.Read("InternoTelefonico", this.m_InternoTelefonico);
                m_SistemaOperativo = reader.Read("SistemaOperativo", this.m_SistemaOperativo);
                m_Note = reader.Read("Note", this.m_Note);
                m_Categoria = reader.Read("Categoria", this.m_Categoria);
                m_SottoCategoria = reader.Read("SottoCategoria", this.m_SottoCategoria);
                string tmp = reader.Read("Utenti", "");
                if (!string.IsNullOrEmpty(tmp))
                    m_Utenti = (CUtentiXPostazioneCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                
                tmp = reader.Read("Registri", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    var obj = DMD.XML.Utils.Serializer.Deserialize(tmp);
                    if (obj is object)
                    {
                        m_RegistriContatori = new CCollection<RegistroContatore>();
                        m_RegistriContatori.AddRange((IEnumerable)obj);
                    }
                }
                 

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
                writer.Write("IDUtentePrincipale", IDUtentePrincipale);
                writer.Write("NomeUtentePrincipale", m_NomeUtentePrincipale);
                writer.Write("NomeReparto", m_NomeReparto);
                writer.Write("InternoTelefonico", m_InternoTelefonico);
                writer.Write("SistemaOperativo", m_SistemaOperativo);
                writer.Write("Note", m_Note);
                writer.Write("Utenti", DMD.XML.Utils.Serializer.Serialize(Utenti));
                writer.Write("Registri", DMD.XML.Utils.Serializer.Serialize(RegistriContatori));
                writer.Write("Categoria", m_Categoria);
                writer.Write("SottoCategoria", m_SottoCategoria);
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
                c = table.Fields.Ensure("Flags", typeof(int), 1);
                c = table.Fields.Ensure("IDUtentePrincipale", typeof(int), 1);
                c = table.Fields.Ensure("NomeUtentePrincipale", typeof(string), 255);
                c = table.Fields.Ensure("NomeReparto", typeof(string), 255);
                c = table.Fields.Ensure("InternoTelefonico", typeof(string), 255);
                c = table.Fields.Ensure("SistemaOperativo", typeof(string), 255);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("Utenti", typeof(string), 0);
                c = table.Fields.Ensure("Registri", typeof(string), 0);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("SottoCategoria", typeof(string), 255);
                 
            }


            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "SistemaOperativo", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUtente", new string[] { "IDUtentePrincipale", "NomeUtentePrincipale" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "SottoCategoria" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxReparto", new string[] { "NomeReparto", "InternoTelefonico" }, DBFieldConstraintFlags.None);
 
                //c = table.Fields.Ensure("Note", typeof(string), 0);
                //c = table.Fields.Ensure("Utenti", typeof(string), 0);
                //c = table.Fields.Ensure("Registri", typeof(string), 0);
                 

            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDUtentePrincipale", IDUtentePrincipale);
                writer.WriteAttribute("NomeUtentePrincipale", m_NomeUtentePrincipale);
                writer.WriteAttribute("NomeReparto", m_NomeReparto);
                writer.WriteAttribute("InternoTelefonico", m_InternoTelefonico);
                writer.WriteAttribute("SistemaOperativo", m_SistemaOperativo);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("SottoCategoria", m_SottoCategoria);
                base.XMLSerialize(writer);
                writer.WriteTag("RegistriContatori", RegistriContatori);
                writer.WriteTag("Utenti", Utenti);
                writer.WriteTag("Note", m_Note);
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
                         
                    case "IDUtentePrincipale":
                        {
                            m_IDUtentePrincipale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUtentePrincipale":
                        {
                            m_NomeUtentePrincipale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeReparto":
                        {
                            m_NomeReparto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "InternoTelefonico":
                        {
                            m_InternoTelefonico = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SistemaOperativo":
                        {
                            m_SistemaOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RegistriContatori":
                        {
                            m_RegistriContatori = new CCollection<RegistroContatore>();
                            m_RegistriContatori.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SottoCategoria":
                        {
                            m_SottoCategoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Utenti":
                        {
                            m_Utenti = (CUtentiXPostazioneCollection)fieldValue;
                            m_Utenti.SetPostazione(this);
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
            /// Compara due oggetti per nome
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CPostazione other)
            {
                return DMD.Strings.Compare(m_Nome, other.m_Nome, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CPostazione)obj);
            }


            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Calcola il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CPostazione) && this.Equals((CPostazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CPostazione obj)
            {
                return base.Equals(obj);
            }


            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    // Me.m_RegistriContatori = Nothing
            //}
        }
    }
}