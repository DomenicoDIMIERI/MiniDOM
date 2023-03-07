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
        /// Stati di un dispositivo
        /// </summary>
        public enum StatoDispositivo : int
        {
            /// <summary>
            /// L'oggetto non presenta difetti
            /// </summary>
            /// <remarks></remarks>
            NUOVO = 0,

            /// <summary>
            /// L'oggetto presenta qualche segno di usura ma è ancora funzionale
            /// </summary>
            /// <remarks></remarks>
            USURATO = 1,

            /// <summary>
            /// L'oggetto è danneggiato
            /// </summary>
            /// <remarks></remarks>
            DANNEGGIATO = 2,

            /// <summary>
            /// L'oggetto è stato dismesso
            /// </summary>
            /// <remarks></remarks>
            DISMESSO = 3
        }

        /// <summary>
        /// Rappresenta un dispositivo assegnato ad un utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Dispositivo 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<Dispositivo>
        {
            [NonSerialized] private Sistema.CUser m_User;
            private int m_UserID;
            private string m_Nome;
            private string m_Tipo;
            private string m_Modello;
            private string m_Seriale;
            private string m_IconURL;
            private DateTime? m_DataAcquisto;
            private DateTime? m_DataDismissione;
            private StatoDispositivo m_StatoDispositivo;
            private string m_Classe;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Dispositivo()
            {
                m_User = null;
                m_UserID = 0;
                m_Nome = DMD.Strings.vbNullString;
                m_Tipo = DMD.Strings.vbNullString;
                m_Modello = DMD.Strings.vbNullString;
                m_Seriale = DMD.Strings.vbNullString;
                m_IconURL = DMD.Strings.vbNullString;
                m_DataAcquisto = default;
                m_DataDismissione = default;
                m_StatoDispositivo = StatoDispositivo.NUOVO;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Dispositivo other)
            {
                var ret = Strings.Compare(this.m_Nome, other.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((Dispositivo)obj); }

            /// <summary>
            /// Restituisce o imposta l'utente a cui è assegnato il dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value, 0);
                    DoChanged("User", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è assegnato il dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int UserID
            {
                get
                {
                    return DBUtils.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_UserID = value;
                    m_User = null;
                    DoChanged("UserID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del dispositivo
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
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modello del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Modello
            {
                get
                {
                    return m_Modello;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Modello;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Modello = value;
                    DoChanged("Modello", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di serie del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Seriale
            {
                get
                {
                    return m_Seriale;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Seriale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Seriale = value;
                    DoChanged("Seriale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso dell'icona associata al dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la classe del dispositivo
            /// </summary>
            public string Classe //TODO mappare in js?
            {
                get
                {
                    return this.m_Classe;
                }
                set
                {
                    var oldValue = this.m_Classe;
                    value = Strings.Trim(value);
                    if (Strings.EQ(value, oldValue))
                        return;
                    this.m_Classe = value;
                    this.DoChanged("Classe", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica lo stato del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoDispositivo StatoDispositivo
            {
                get
                {
                    return m_StatoDispositivo;
                }

                set
                {
                    var oldValue = m_StatoDispositivo;
                    if (oldValue == value)
                        return;
                    m_StatoDispositivo = value;
                    DoChanged("StatoDispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di acquisto del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }

                set
                {
                    var oldValue = m_DataAcquisto;
                    if (oldValue == value == true)
                        return;
                    m_DataAcquisto = value;
                    DoChanged("DataAcquisto", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta la data di dismissione del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }

                set
                {
                    var oldValue = m_DataDismissione;
                    if (oldValue == value == true)
                        return;
                    m_DataDismissione = value;
                    DoChanged("DataDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray( m_Nome ,  " (" , m_Tipo , ", " , m_Modello , ", " , m_Seriale , ")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Dispositivi;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeDevices";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_UserID = reader.Read("UserID", m_UserID);
                m_Nome = reader.Read("Nome", m_Nome);
                m_Tipo = reader.Read("Tipo", m_Tipo);
                m_Modello = reader.Read("Modello", m_Modello);
                m_Seriale = reader.Read("Seriale", m_Seriale);
                m_IconURL = reader.Read("IconURL", m_IconURL);
                m_DataAcquisto = reader.Read("DataAcquisto", m_DataAcquisto);
                m_DataDismissione = reader.Read("DataDismissione", m_DataDismissione);
                m_StatoDispositivo = reader.Read("StatoDispositivo", m_StatoDispositivo);
                this.m_Classe = reader.Read("Classe", this.m_Classe);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva sul db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("UserID", UserID);
                writer.Write("Nome", m_Nome);
                writer.Write("Tipo", m_Tipo);
                writer.Write("Modello", m_Modello);
                writer.Write("Seriale", m_Seriale);
                writer.Write("IconURL", m_IconURL);
                writer.Write("DataAcquisto", m_DataAcquisto);
                writer.Write("DataDismissione", m_DataDismissione);
                writer.Write("StatoDispositivo", m_StatoDispositivo);
                writer.Write("Classe", this.m_Classe);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("UserID", typeof(int), 1);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Modello", typeof(string), 255);
                c = table.Fields.Ensure("Seriale", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("DataAcquisto", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataDismissione", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoDispositivo", typeof(int), 1);
                c = table.Fields.Ensure("Classe", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUser", new string[] { "UserID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "StatoDispositivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxModello", new string[] { "Tipo", "Modello" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSeriale", new string[] { "Seriale", "Classe" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataAcquisto", "DataDismissione" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
                

            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Modello", m_Modello);
                writer.WriteAttribute("Seriale", m_Seriale);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("DataAcquisto", m_DataAcquisto);
                writer.WriteAttribute("DataDismissione", m_DataDismissione);
                writer.WriteAttribute("StatoDispositivo", (int?)m_StatoDispositivo);
                writer.WriteAttribute("Classe", this.m_Classe); //TODO mappare in js?
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
                    case "UserID":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    case "Classe": //TODO mappare in js?
                        {
                            m_Classe = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    case "Modello":
                        {
                            m_Modello = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Seriale":
                        {
                            m_Seriale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAcquisto":
                        {
                            m_DataAcquisto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataDismissione":
                        {
                            m_DataDismissione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoDispositivo":
                        {
                            m_StatoDispositivo = (StatoDispositivo)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Dispositivo) && this.Equals((Dispositivo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Dispositivo obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Modello, obj.m_Modello)
                    && DMD.Strings.EQ(this.m_Seriale, obj.m_Seriale)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.DateUtils.EQ(this.m_DataAcquisto, obj.m_DataAcquisto)
                    && DMD.DateUtils.EQ(this.m_DataDismissione, obj.m_DataDismissione)
                    && DMD.Integers.EQ((int)this.m_StatoDispositivo, (int)obj.m_StatoDispositivo)
                    && DMD.Strings.EQ(this.m_Classe, obj.m_Classe)
                    ;
            }
            
        }
    }
}