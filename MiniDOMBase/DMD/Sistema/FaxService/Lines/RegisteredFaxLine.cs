using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;
using DMD.FAX;
using DMD.FAX.Drivers;
using DMD.Databases.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Linea fax registrata
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RegisteredFaxLine
            : minidom.Databases.DBObjectPO, IComparable, IComparable<RegisteredFaxLine>
        {

            private string m_Name;
            private string m_Tipologia;
            private string m_DialPrefix;
            private string m_Numero;
            private string m_EMailInvio;
            private string m_EMailRicezione;
            private string m_ServerName;
            private int m_ServerPort;
            private string m_Account;
            private string m_UserName;
            private string m_Password;
            [NonSerialized] private CCollection<CUfficio> m_PuntiOperativi;
            [NonSerialized] private BaseFaxDriver m_Owner;
            
            [NonSerialized] private CCollection<CGroup> m_NotificaAGruppi;
            [NonSerialized] private CCollection<CGroup> m_EscludiNotificaGruppi;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegisteredFaxLine()
            {
                //DMDObject.IncreaseCounter(this);
                m_Name = "";
                m_Tipologia = "";
                m_Numero = "";
                m_EMailInvio = "";
                m_EMailRicezione = "";
                m_ServerName = "";
                m_ServerPort = 0;
                m_Account = "";
                m_UserName = "";
                m_Password = "";
                m_Flags = (int) ( FaxModemFlags.ReceiveEnabled | FaxModemFlags.SendEnabled );
                m_PuntiOperativi = null;
                m_Owner = null;
                m_PuntiOperativi = null;
                m_NotificaAGruppi = null;
                m_EscludiNotificaGruppi = null;
                m_DialPrefix = "";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'ggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate (this.m_Name, this.m_Numero );
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return base.Equals(obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RegisteredFaxLine obj)
            {
                return base.Equals(obj)
                        && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                        && DMD.Strings.EQ(this.m_Tipologia, obj.m_Tipologia)
                        && DMD.Strings.EQ(this.m_DialPrefix, obj.m_DialPrefix)
                        && DMD.Strings.EQ(this.m_Numero, obj.m_Numero)
                        && DMD.Strings.EQ(this.m_EMailInvio, obj.m_EMailInvio)
                        && DMD.Strings.EQ(this.m_EMailRicezione, obj.m_EMailRicezione)
                        && DMD.Strings.EQ(this.m_ServerName, obj.m_ServerName)
                        && DMD.Integers.EQ(this.m_ServerPort, obj.m_ServerPort)
                        && DMD.Strings.EQ(this.m_Account, obj.m_Account)
                        && DMD.Strings.EQ(this.m_UserName, obj.m_UserName)
                        && DMD.Strings.EQ(this.m_Password, obj.m_Password)
                        ;
                        //[NonSerialized] private CCollection<CUfficio> m_PuntiOperativi;
                        //[NonSerialized] private BaseFaxDriver m_Owner;
                        //[NonSerialized] private CCollection<CGroup> m_NotificaAGruppi;
                        //[NonSerialized] private CCollection<CGroup> m_EscludiNotificaGruppi;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(RegisteredFaxLine obj)
            {
                int ret = DMD.Strings.Compare(this.m_Name, obj.m_Name, true);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((RegisteredFaxLine)obj);  }

            /// <summary>
            /// Restituisce o imposta un codice da anteporre al numero per eventuale accesso alle linee del centralino
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DialPrefix
            {
                get
                {
                    return m_DialPrefix;
                }

                set
                {
                    string oldValue = m_DialPrefix;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DialPrefix = value;
                    DoChanged("DialPrefix", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del server a cui si connette il modem
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ServerName
            {
                get
                {
                    return m_ServerName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ServerName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ServerName = value;
                    DoChanged("ServerName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la porta di ascolto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ServerPort
            {
                get
                {
                    return m_ServerPort;
                }

                set
                {
                    int oldValue = m_ServerPort;
                    if (oldValue == value)
                        return;
                    m_ServerPort = value;
                    DoChanged("ServerPort", value, oldValue);
                }
            }

             

            /// <summary>
            /// Restituisce il driver che gestisce il modem
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public BaseFaxDriver Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta il driver
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void SetOwner(BaseFaxDriver owner)
            {
                m_Owner = owner;
            }

            /// <summary>
            /// Restituisce o imposta il nome del modem
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la tipologia del modem (e-mail, analogico, ecc..)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Tipologia
            {
                get
                {
                    return m_Tipologia;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipologia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipologia = value;
                    DoChanged("Tipologia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero geografico utilizzato per la ricezione e l'invio di fax
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Numero
            {
                get
                {
                    return m_Numero;
                }

                set
                {
                    value = Formats.ParsePhoneNumber(value);
                    string oldValue = m_Numero;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Numero = value;
                    DoChanged("Numero", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo email a cui inviare i documenti da spedire come fax (per i servizi Fax Out)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string eMailInvio
            {
                get
                {
                    return m_EMailInvio;
                }

                set
                {
                    value = Formats.ParseEMailAddress(value);
                    string oldValue = m_EMailInvio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EMailInvio = value;
                    DoChanged("eMailInvio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo eMail di ascolto su cui vengono inviati i fax ricevuti (per i servizi fax in)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string eMailRicezione
            {
                get
                {
                    return m_EMailRicezione;
                }

                set
                {
                    string oldValue = m_EMailRicezione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EMailRicezione = value;
                    DoChanged("eMailRicezione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'account associato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Account
            {
                get
                {
                    return m_Account;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Account;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Account = value;
                    DoChanged("Account", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UserName
            {
                get
                {
                    return m_UserName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_UserName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UserName = value;
                    DoChanged("UserName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la password associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Password
            {
                get
                {
                    return m_Password;
                }

                set
                {
                    string oldValue = m_Password;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Password = value;
                    DoChanged("Password", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new FaxModemFlags Flags
            {
                get
                {
                    return (FaxModemFlags) base.Flags;
                }

                set
                {
                    base.Flags = (int)value;
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica il modem è abilitato all'invio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool SendEnabled
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, FaxModemFlags.SendEnabled);
                }

                set
                {
                    if (SendEnabled == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, (int)FaxModemFlags.SendEnabled, value);
                    DoChanged("SendEnabled", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il modem è abilitato alla ricezione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool ReceiveEnabled
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, (int)FaxModemFlags.ReceiveEnabled);
                }

                set
                {
                    if (ReceiveEnabled == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, (int)FaxModemFlags.ReceiveEnabled, value);
                    DoChanged("ReceiveEnabled", value, !value);
                }
            }

             
            /// <summary>
            /// Restituisce la collezione dei punti operativi per cui il fax è visibile
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CUfficio> PuntiOperativi
            {
                get
                {
                    lock (this)
                    {
                        if (m_PuntiOperativi is null)
                            m_PuntiOperativi = new CCollection<CUfficio>();
                        return m_PuntiOperativi;
                    }
                }
            }

            /// <summary>
            /// Restituisce la collezione dei gruppi a cui viene notificata la ricezione di un fax
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CGroup> NotificaAGruppi
            {
                get
                {
                    lock (this)
                    {
                        if (m_NotificaAGruppi is null)
                            m_NotificaAGruppi = new CCollection<CGroup>();
                        return m_NotificaAGruppi;
                    }
                }
            }

            /// <summary>
            /// Restituisce la collezione dei gruppi a cui viene impedita la notifica di ricezione di un fax (ha precedenza rispetto a NotificaAGruppi)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CGroup> EscludiNotificaAGruppi
            {
                get
                {
                    lock (this)
                    {
                        if (m_EscludiNotificaGruppi is null)
                            m_EscludiNotificaGruppi = new CCollection<CGroup>();
                        return m_EscludiNotificaGruppi;
                    }
                }
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
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tipologia":
                        {
                            m_Tipologia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Numero":
                        {
                            m_Numero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EMailInvio":
                        {
                            m_EMailInvio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EMailRicezione":
                        {
                            m_EMailRicezione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Account":
                        {
                            m_Account = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Password":
                        {
                            m_Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                  
                    case "PuntiOperativi":
                        {
                            m_PuntiOperativi = GetColUffici((int[])DMD.XML.Utils.Serializer.ToArray<int>(fieldValue));
                            break;
                        }

                    
                    case "NotificaAGruppi":
                        {
                            m_NotificaAGruppi = GetColGruppi((int[])DMD.XML.Utils.Serializer.ToArray<int>(fieldValue));
                            break;
                        }

                    case "EscludiNotificaGruppi":
                        {
                            m_EscludiNotificaGruppi = GetColGruppi((int[])DMD.XML.Utils.Serializer.ToArray<int>(fieldValue));
                            break;
                        }

                    case "ServerName":
                        {
                            m_ServerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ServerPort":
                        {
                            m_ServerPort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DialPrefix":
                        {
                            m_DialPrefix = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("Tipologia", m_Tipologia);
                writer.WriteAttribute("Numero", m_Numero);
                writer.WriteAttribute("EMailInvio", m_EMailInvio);
                writer.WriteAttribute("EMailRicezione", m_EMailRicezione);
                writer.WriteAttribute("Account", m_Account);
                writer.WriteAttribute("UserName", m_UserName);
                writer.WriteAttribute("Password", m_Password);
                writer.WriteAttribute("ServerName", m_ServerName);
                writer.WriteAttribute("ServerPort", m_ServerPort);
                writer.WriteAttribute("DialPrefix", m_DialPrefix);
                base.XMLSerialize(writer);
                writer.WriteTag("PuntiOperativi", GetArrUffici());
                writer.WriteTag("NotificaAGruppi", GetArrGruppi(NotificaAGruppi));
                writer.WriteTag("EscludiNotificaGruppi", GetArrGruppi(EscludiNotificaAGruppi));
            }

            /// <summary>
            /// Restituisce l'array degli id dei gruppi
            /// </summary>
            /// <param name="col"></param>
            /// <returns></returns>
            private int[] GetArrGruppi(CCollection<CGroup> col)
            {
                var ret = new List<int>();
                foreach (CGroup g in col)
                    ret.Add(DBUtils.GetID(g, 0));
                return (int[])ret.ToArray();
            }

            /// <summary>
            /// Restituisce l'array degli id degli uffici
            /// </summary>
            /// <returns></returns>
            private int[] GetArrUffici()
            {
                var ret = new List<int>();
                foreach (CUfficio po in PuntiOperativi)
                    ret.Add(DBUtils.GetID(po, 0));
                return (int[])ret.ToArray();
            }

            private CCollection<CUfficio> GetColUffici(int[] arr)
            {
                var ret = new CCollection<CUfficio>();
                for (int i = 0, loopTo = DMD.Arrays.Len(arr) - 1; i <= loopTo; i++)
                {
                    var u = Anagrafica.Uffici.GetItemById(arr[i]);
                    if (u is object)
                        ret.Add(u);
                }

                return ret;
            }

            private CCollection<CGroup> GetColGruppi(int[] arr)
            {
                var ret = new CCollection<CGroup>();
                for (int i = 0, loopTo = DMD.Arrays.Len(arr) - 1; i <= loopTo; i++)
                {
                    var g = Groups.GetItemById(arr[i]);
                    if (g is object)
                        ret.Add(g);
                }

                return ret;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_RegisteredFaxLines";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.FaxService.RegisteredLines;
            }

            /// <summary>
            /// Deserializzazione da db
            /// </summary>
            /// <param name="reader"></param>
            protected override void DBRead(DBReader reader)
            {
                this.m_Name = reader.Read("Name", this.m_Name);
                this.m_Tipologia = reader.Read("Tipologia", this.m_Tipologia);
                this.m_Numero = reader.Read("Numero", this.m_Numero);
                this.m_EMailInvio = reader.Read("EMailInvio", this.m_EMailInvio);
                this.m_EMailRicezione = reader.Read("EMailRicezione", this.m_EMailRicezione);
                this.m_Account = reader.Read("Account", this.m_Account);
                this.m_UserName = reader.Read("UserName", this.m_UserName);
                this.m_Password = reader.Read("Password", this.m_Password);
                this.m_ServerName = reader.Read("ServerName", this.m_ServerName);
                this.m_ServerPort = reader.Read("ServerPort", this.m_ServerPort);
                this.m_DialPrefix = reader.Read("DialPrefix", this.m_DialPrefix);
                string tmp = reader.Read("PuntiOperativi", "");
                if (!string.IsNullOrEmpty(tmp)) {
                    this.m_PuntiOperativi = new CCollection<CUfficio>();
                    this.m_PuntiOperativi.AddRange(DMD.XML.Utils.Deserialize<CCollection>(tmp));
                }

                tmp = reader.Read("NotificaAGruppi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.m_NotificaAGruppi = new CCollection<CGroup>();
                    this.m_NotificaAGruppi.AddRange(DMD.XML.Utils.Deserialize<CCollection>(tmp));
                }

                tmp = reader.Read("EscludiNotificaGruppi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.m_EscludiNotificaGruppi = new CCollection<CGroup>();
                    this.m_EscludiNotificaGruppi.AddRange(DMD.XML.Utils.Deserialize<CCollection>(tmp));
                }

                base.DBRead(reader);
            }

            /// <summary>
            /// Serializzazione da db
            /// </summary>
            /// <param name="writer"></param>
            protected override void DBWrite(DBWriter writer)
            {
                writer.Write("Name", m_Name);
                writer.Write("Tipologia", m_Tipologia);
                writer.Write("Numero", m_Numero);
                writer.Write("EMailInvio", m_EMailInvio);
                writer.Write("EMailRicezione", m_EMailRicezione);
                writer.Write("Account", m_Account);
                writer.Write("UserName", m_UserName);
                writer.Write("Password", m_Password);
                writer.Write("ServerName", m_ServerName);
                writer.Write("ServerPort", m_ServerPort);
                writer.Write("DialPrefix", m_DialPrefix);
                writer.Write("PuntiOperativi", DMD.XML.Utils.Serialize(this.PuntiOperativi));
                writer.Write("NotificaAGruppi", DMD.XML.Utils.Serialize(this.NotificaAGruppi));
                writer.Write("EscludiNotificaGruppi", DMD.XML.Utils.Serialize(this.EscludiNotificaAGruppi));
                base.DBWrite(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Name", typeof(string), 255);
                c = table.Fields.Ensure("Tipologia", typeof(string), 255);
                c = table.Fields.Ensure("Numero", typeof(string), 255);
                c = table.Fields.Ensure("EMailInvio", typeof(string), 255);
                c = table.Fields.Ensure("EMailRicezione", typeof(string), 255);
                c = table.Fields.Ensure("Account", typeof(string), 255);
                c = table.Fields.Ensure("UserName", typeof(string), 255);
                c = table.Fields.Ensure("Password", typeof(string), 255);
                c = table.Fields.Ensure("ServerName", typeof(string), 255);
                c = table.Fields.Ensure("ServerPort", typeof(int), 1);
                c = table.Fields.Ensure("DialPrefix", typeof(string), 255);
                c = table.Fields.Ensure("PuntiOperativi", typeof(string), 0);
                c = table.Fields.Ensure("NotificaAGruppi", typeof(string), 0);
                c = table.Fields.Ensure("EscludiNotificaGruppi", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxName", new string[] { "Name", "Tipologia" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumber", new string[] { "Numero", "DialPrefix", "Account" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParams1", new string[] { "ServerName", "ServerPort", "UserName" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParams2", new string[] { "EMailInvio", "EMailRicezione" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Password", typeof(string), 255);
                //c = table.Fields.Ensure("PuntiOperativi", typeof(string), 0);
                //c = table.Fields.Ensure("NotificaAGruppi", typeof(string), 0);
                //c = table.Fields.Ensure("EscludiNotificaGruppi", typeof(string), 0);
            }

        }
    }
}