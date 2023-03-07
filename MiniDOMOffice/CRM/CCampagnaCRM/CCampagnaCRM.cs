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
    public partial class CustomerCalls
    {

        /// <summary>
        /// Tipo di campagna crm
        /// </summary>
        public enum TipoCampagnaCRM : int
        {
            /// <summary>
            /// Normale
            /// </summary>
            Normale = 0,

            /// <summary>
            /// Chiamata automatica
            /// </summary>
            Autochiamata = 1,

            /// <summary>
            /// Chiamate suggerite
            /// </summary>
            Predictive = 2
        }

        /// <summary>
        /// Flag per una campagna crm
        /// </summary>
        [Flags]
        public enum CampagnaCRMFlag : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Disattiva
            /// </summary>
            Disattiva = 1,

            /// <summary>
            /// Richiede approvazione
            /// </summary>
            RichiediApprovazionePause = 2
        }


        /// <summary>
        /// Campagna CRM 
        /// </summary>
        [Serializable]
        public class CCampagnaCRM 
            : Databases.DBObjectPO
        {
            private string m_Nome;
            private DateTime? m_Inizio;
            private DateTime? m_Fine;
            private TipoCampagnaCRM m_TipoAssegnazione;
            private string m_TipoCampagna;
            private CCollection<CCampagnaXGroupAllowNegate> m_Gruppi;
            private CCollection<CCampagnaXUserAllowNegate> m_Utenti;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagnaCRM()
            {
                m_Nome = "";
                m_Inizio = default;
                m_Fine = default;
                m_TipoCampagna = "";
                m_TipoAssegnazione = TipoCampagnaCRM.Normale;
                m_Flags = (int)CampagnaCRMFlag.None;
                m_Gruppi = null;
                m_Utenti = null;
            }

            /// <summary>
            /// Restituisce o imposta il nome della campagna
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta la data inizio
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta la data di fine
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta la tipologia di assegnazione dei contatti
            /// </summary>
            /// <returns></returns>
            public TipoCampagnaCRM TipoAssegnazione
            {
                get
                {
                    return m_TipoAssegnazione;
                }

                set
                {
                    var oldValue = m_TipoAssegnazione;
                    if (oldValue == value)
                        return;
                    m_TipoAssegnazione = value;
                    DoChanged("TipoAssegnazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la tipologia della campagna
            /// </summary>
            /// <returns></returns>
            public string TipoCampagna
            {
                get
                {
                    return m_TipoCampagna;
                }

                set
                {
                    string oldValue = m_TipoCampagna;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoCampagna = value;
                    DoChanged("TipoCampagna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flag aggiuntivi
            /// </summary>
            /// <returns></returns>
            public new CampagnaCRMFlag Flags
            {
                get
                {
                    return (CampagnaCRMFlag)base.Flags;
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
            /// Restituisce o imposta un valore booleano che indica se la campagna é attiva
            /// </summary>
            /// <returns></returns>
            public bool Attiva
            {
                get
                {
                    return !DMD.RunTime.TestFlag(this.Flags, CampagnaCRMFlag.Disattiva);
                }

                set
                {
                    if (Attiva == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, CampagnaCRMFlag.Disattiva, !value);
                    DoChanged("Attiva", value, !value);
                }
            }

            

            /// <summary>
            /// Restituisce l'elenco dei gruppi abilitati o inibiliti alla campagna
            /// </summary>
            /// <returns></returns>
            public CCollection<CCampagnaXGroupAllowNegate> Gruppi
            {
                get
                {
                    if (m_Gruppi is null)
                        m_Gruppi = new CCollection<CCampagnaXGroupAllowNegate>();
                    return m_Gruppi;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elenco degli utenti abilitati o inibili alla campagna
            /// </summary>
            /// <returns></returns>
            public CCollection<CCampagnaXUserAllowNegate> Utenti
            {
                get
                {
                    if (m_Utenti is null)
                        m_Utenti = new CCollection<CCampagnaXUserAllowNegate>();
                    return m_Utenti;
                }
            }

            /// <summary>
            /// Restituisce un oggetto che rappresenta l'abilitazione della campagna rispetto al gruppo
            /// </summary>
            /// <param name="group"></param>
            /// <returns></returns>
            public CCampagnaXGroupAllowNegate GetGroupAllowNegate(Sistema.CGroup group)
            {
                if (group is null)
                    throw new ArgumentNullException("group");
                foreach (CCampagnaXGroupAllowNegate item in Gruppi)
                {
                    if (item.GroupID == DBUtils.GetID(group, 0))
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Imposta le autorizzazioni per il gruppo
            /// </summary>
            /// <param name="group"></param>
            /// <param name="allow"></param>
            /// <returns></returns>
            public CCampagnaXGroupAllowNegate SetGroupAllowNegate(Sistema.CGroup group, bool allow)
            {
                if (group is null)
                    throw new ArgumentNullException("group");
                var item = GetGroupAllowNegate(group);
                if (item is null)
                {
                    item = new CCampagnaXGroupAllowNegate();
                    item.Item = this;
                    item.Group = group;
                    Gruppi.Add(item);
                }

                item.Allow = allow;
                SetChanged(DMD.Booleans.ValueOf("Grouppi"));
                return item;
            }

            /// <summary>
            /// Restituisce l'autorizzazione per l'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CCampagnaXUserAllowNegate GetUserAllowNegate(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                foreach (CCampagnaXUserAllowNegate item in Utenti)
                {
                    if (item.UserID == DBUtils.GetID(user, 0))
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Imposta l'autorizzazione per l'utente
            /// </summary>
            /// <param name="User"></param>
            /// <param name="allow"></param>
            /// <returns></returns>
            public CCampagnaXUserAllowNegate SetUserAllowNegate(Sistema.CUser User, bool allow)
            {
                if (User is null)
                    throw new ArgumentNullException("User");
                var item = GetUserAllowNegate(User);
                if (item is null)
                {
                    item = new CCampagnaXUserAllowNegate();
                    item.Item = this;
                    item.User = User;
                    Utenti.Add(item);
                }

                item.Allow = allow;
                SetChanged(DMD.Booleans.ValueOf("Utenti"));
                return item;
            }

            /// <summary>
            /// Restituisce true se il gruppo é autorizzato
            /// </summary>
            /// <param name="grp"></param>
            /// <returns></returns>
            public bool IsGroupAllowed(Sistema.CGroup grp)
            {
                var item = GetGroupAllowNegate(grp);
                if (item is null)
                    return false;
                return item.Allow && !item.Negate;
            }

            /// <summary>
            /// Restituisce true se l'utente é autorizzato (compreso tutti i gruppi)
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public bool IsUserAllowed(Sistema.CUser user)
            {
                bool allow = true;
                var u = GetUserAllowNegate(user);
                if (u is object)
                    return u.Allow && !u.Negate;

                foreach (Sistema.CGroup grp in user.Groups)
                {
                    if (grp.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        var g = GetGroupAllowNegate(grp);
                        if (g is object)
                        {
                            allow = allow & g.Allow && !g.Negate;
                        }
                    }
                }

                return allow;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.CampagneCRM;
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
            /// Restituisce tru se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CCampagnaCRM) && this.Equals((CCampagnaCRM)obj);
            }

            /// <summary>
            /// Restituisce tru se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CCampagnaCRM obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.DateUtils.EQ(this.m_Inizio, obj.m_Inizio)
                    && DMD.DateUtils.EQ(this.m_Fine, obj.m_Fine)
                    && DMD.Integers.EQ((int)this.m_TipoAssegnazione, (int)obj.m_TipoAssegnazione)
                    && DMD.Strings.EQ(this.m_TipoCampagna, obj.m_TipoCampagna)
                    ;
            //private CCollection<CCampagnaXGroupAllowNegate> m_Gruppi;
            //private CCollection<CCampagnaXUserAllowNegate> m_Utenti;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CampagnaCRM";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_Inizio = reader.Read("Inizio", m_Inizio);
                m_Fine = reader.Read("Fine", m_Fine);
                m_TipoAssegnazione = reader.Read("TipoAssegnazione", m_TipoAssegnazione);
                m_TipoCampagna = reader.Read("TipoCampagna", m_TipoCampagna);
                string tmp = reader.Read("Gruppi", ""); 
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Gruppi = new CCollection<CCampagnaXGroupAllowNegate>();
                    m_Gruppi.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                }
                tmp = reader.Read("Utenti", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Utenti = new CCollection<CCampagnaXUserAllowNegate>();
                    m_Utenti.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
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
                writer.Write("Inizio", m_Inizio);
                writer.Write("Fine", m_Fine);
                writer.Write("TipoAssegnazione", m_TipoAssegnazione);
                writer.Write("TipoCampagna", m_TipoCampagna);
                writer.Write("Gruppi", DMD.XML.Utils.Serializer.Serialize(Gruppi));
                writer.Write("Utenti", DMD.XML.Utils.Serializer.Serialize(Utenti));
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
                c = table.Fields.Ensure("Inizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("Fine", typeof(DateTime), 1);
                c = table.Fields.Ensure("TipoAssegnazione", typeof(int), 1);
                c = table.Fields.Ensure("TipoCampagna", typeof(string), 255);
                c = table.Fields.Ensure("Gruppi", typeof(string), 0);
                c = table.Fields.Ensure("Utenti", typeof(string), 0);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDate", new string[] { "Inizio", "Fine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTipologia", new string[] { "TipoCampagna", "TipoAssegnazione" }, DBFieldConstraintFlags.None);
                 
                //c = table.Fields.Ensure("Gruppi", typeof(string), 0);
                //c = table.Fields.Ensure("Utenti", typeof(string), 0);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Inizio", m_Inizio);
                writer.WriteAttribute("Fine", m_Fine);
                writer.WriteAttribute("TipoAssegnazione", (int?)m_TipoAssegnazione);
                writer.WriteAttribute("TipoCampagna", m_TipoCampagna);
                base.XMLSerialize(writer);
                writer.WriteTag("Gruppi", Gruppi);
                writer.WriteTag("Utenti", Utenti);
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

                    case "TipoAssegnazione":
                        {
                            m_TipoAssegnazione = (TipoCampagnaCRM)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCampagna":
                        {
                            m_TipoCampagna = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
  
                    case "Gruppi":
                        {
                            m_Gruppi = new CCollection<CCampagnaXGroupAllowNegate>();
                            m_Gruppi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Utenti":
                        {
                            m_Utenti = new CCollection<CCampagnaXUserAllowNegate>();
                            m_Utenti.AddRange((IEnumerable)fieldValue);
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
                return m_Nome;
            }
             
        }
    }
}