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
        /// Flag definiti su un device
        /// </summary>
        [Flags]
        public enum RegisteredDeviceCaps : int
        {
            /// <summary>
            /// Nessun fla
            /// </summary>
            None = 0,

            /// <summary>
            /// Se vero indica che il tipo di dispositivo supporta le funzioni di localizzazione GPS
            /// </summary>
            CanGPS = 1,

            /// <summary>
            /// Se vero indica che il dispositivo può stampare dei documenti cartacei
            /// </summary>
            CanPrint = 2,

            /// <summary>
            /// Se vero indica che il dispositivo può effettuare scansioni
            /// </summary>
            CanScan = 4,

            /// <summary>
            /// Se vero indica che il dispositivo supporta il riconoscimento tramite impronte digitali
            /// </summary>
            CanFingerprint = 8,

            /// <summary>
            /// Se vero indica che il dispositivo può effettuare riconoscimenti tramite metodi diversi dall'impronta digitale
            /// </summary>
            CanRecognizeOther = 16,

            /// <summary>
            /// Se vero indica che il dispositivo è utilizzabile come destkop
            /// </summary>
            IsDesktop = 32,

            /// <summary>
            /// Se vero indica che il dispositivo è utilizzabile come telefono per le conversazioni
            /// </summary>
            IsPhone = 64,

            /// <summary>
            /// Se vero indica che il dispositivo supporta l'invio o la ricezione di SMS
            /// </summary>
            IsSMS = 128,

            /// <summary>
            /// Sorgente audio
            /// </summary>
            IsAudioSource = 256,

            /// <summary>
            /// Player audio
            /// </summary>
            IsAudioPlayer = 512,

            /// <summary>
            /// La periferica é utilizzabile come sorgente streaming video (es. webcam)
            /// </summary>
            IsVideoSource = 1024,

            /// <summary>
            /// La periferica é utilizzabile per mostrare video (es. display)
            /// </summary>
            IsVideoPlayer = 2048


        }


        /// <summary>
        /// Pagina di proprietà registrata per un oggetto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CRegisteredDevice 
            : minidom.Databases.DBObject, IComparable, IComparable<CRegisteredDevice>, ISettingsOwner
        {
            private string m_Nome;
            private string m_Tipo;
            private string m_Modello;
            private string m_ClassName;
            private string m_Address;
            private string m_DeviceName;
            private string m_DriverName;
            private CSettings m_Settings;
            private RegisteredDeviceCaps m_Capabilities;
            //[NonSerialized] private PropPageUserAllowNegateCollection m_UsersAuth;
            //[NonSerialized] private PropPageGroupAllowNegateCollection m_GroupAuth;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredDevice()
            {
                this.m_ClassName = "";
                this.m_DriverName = "";
                this.m_Nome = "";
                this.m_Tipo = "";
                this.m_Modello = "";
                this.m_Settings = null;
                this.m_Capabilities = RegisteredDeviceCaps.None;
            }

            /// <summary>
            /// Restituiuisce o imposta il nome della periferica
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
            /// Restituisce o imposta il tipo della periferica
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
            /// Restituisce o imposta il modello della periferica
            /// </summary>
            public string Modello
            {
                get
                {
                    return m_Modello;
                }

                set
                {
                    string oldValue = m_Modello;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Modello = value;
                    DoChanged("Modello", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una collezione di flag che descrive il tipo di periferica
            /// </summary>
            public RegisteredDeviceCaps Capabilities
            {
                get
                {
                    return this.m_Capabilities;
                }
                set
                {
                    var oldValue = this.m_Capabilities;
                    if (this.m_Capabilities == value) return;
                    this.m_Capabilities = value;
                    this.DoChanged("Capabilities", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la configurazione della periferica
            /// </summary>
            public CSettings Settings
            {
                get
                {
                    if (this.m_Settings is null)
                        this.m_Settings = new CSettings(this);
                    return this.m_Settings;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo di installazione del device
            /// </summary>
            public string Address
            {
                get
                {
                    return this.m_Address;
                }
                set
                {
                    value = Strings.Trim(value);
                    var oldValue = this.m_Address;
                    if (DMD.Strings.EQ(value, oldValue)) return;
                    this.m_Address = value;
                    this.DoChanged("Address", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del device
            /// </summary>
            public string DeviceName
            {
                get
                {
                    return this.m_DeviceName;
                }
                set
                {
                    value = Strings.Trim(value);
                    var oldValue = this.m_DeviceName;
                    if (Strings.EQ(value, oldValue)) return;
                    this.m_DeviceName = value;
                    this.DoChanged("DeviceName", value, oldValue);
                } 
            }


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.RegisteredDevices; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta il nome della classe per cui é registrata la pagina di proprietà
            /// </summary>
            public string ClassName
            {
                get
                {
                    return m_ClassName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ClassName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ClassName = value;
                    DoChanged("ClassName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del driver che gestisce la periferica
            /// </summary>
            public string DriverName
            {
                get
                {
                    return m_DriverName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DriverName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DriverName = value;
                    DoChanged("DriverName", value, oldValue);
                }
            }

            CSettings ISettingsOwner.Settings
            {
                get
                {
                    return this.Settings;
                }
            }


            /// <summary>
            /// Restituisce i parametri di configurazione
            /// </summary>
            /// <returns></returns>
            public CSettings GetSettings 
            {
                get
                {
                    if (this.m_Settings is null) this.m_Settings = new CSettings(this);
                    return this.m_Settings;
                }
            }

            /// <summary>
            /// Imposta gli attributi
            /// </summary>
            /// <param name="value"></param>
            public void SetSettings(CSettings value)
            {
                if (object.ReferenceEquals(this.m_Settings, value))
                    return;
                this.m_Settings = value;
                if(value is object)
                    this.m_Settings.SetOwner(this);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_RegisteredDevices";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_ClassName = reader.Read("ClassName", this.m_ClassName);
                this.m_DriverName = reader.Read("DriverName", this.m_DriverName);
                this.m_DriverName = reader.Read("Address", this.m_Address);
                this.m_DriverName = reader.Read("DeviceName", this.m_DeviceName);
                this.m_Capabilities = reader.Read("Capabilities", this.m_Capabilities);
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Tipo = reader.Read("Tipo", this.m_Tipo);
                this.m_Modello = reader.Read("Modello", this.m_Modello);
                string tmp = reader.Read("Settings", "");
                this.m_Settings = DMD.XML.Utils.Deserialize<CSettings>(tmp);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ClassName", this.m_ClassName);
                writer.Write("DriverName", this.m_DriverName);
                writer.Write("DeviceName", this.m_DeviceName);
                writer.Write("Address", this.m_Address);
                writer.Write("Capabilities", this.m_Capabilities);
                writer.Write("Settings", DMD.XML.Utils.Serialize(this.m_Settings));
                writer.Write("Nome", this.m_Nome);
                writer.Write("Tipo", this.m_Tipo);
                writer.Write("Modello", this.m_Modello);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("ClassName", typeof(string), 255);
                c = table.Fields.Ensure("DriverName", typeof(string), 255);
                c = table.Fields.Ensure("DeviceName", typeof(string), 255);
                c = table.Fields.Ensure("Address", typeof(string), 255);
                c = table.Fields.Ensure("Settings", typeof(string), 0);
                c = table.Fields.Ensure("Capabilities", typeof(int), 1);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Modello", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxClassName", new string[] {  "ClassName", "DeviceName" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAddress", new string[] {  "Address", "DriverName", "Capabilities" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Tipo", "Modello" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Priority", typeof(int), 1);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ClassName", this.m_ClassName);
                writer.WriteAttribute("DriverName", this.m_DriverName);
                writer.WriteAttribute("DeviceName", this.m_DeviceName);
                writer.WriteAttribute("Address", this.m_Address);
                writer.WriteAttribute("Capabilities", this.m_Capabilities);
                writer.WriteAttribute("Nome", this.m_Nome);
                writer.WriteAttribute("Tipo", this.m_Tipo);
                writer.WriteAttribute("Modello", this.m_Modello);
                base.XMLSerialize(writer);
                writer.WriteTag("Settings", this.m_Settings);
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
                    case "Nome": m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Tipo": m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Modello": m_Modello = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "ClassName": this.m_ClassName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "DriverName": this.m_DriverName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "DeviceName": this.m_DeviceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Address": this.m_Address = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Capabilities": this.m_Capabilities = (RegisteredDeviceCaps)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "Settings": 
                        this.m_Settings = (CSettings)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        this.m_Settings.SetOwner(this);
                        break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_ClassName , "/" , this.m_DriverName );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_ClassName, this.m_Address);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CRegisteredDevice) && this.Equals((CRegisteredDevice)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRegisteredDevice obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_ClassName, obj.m_ClassName)
                    && DMD.Strings.EQ(this.m_Address, obj.m_Address)
                    && DMD.Strings.EQ(this.m_DeviceName, obj.m_DeviceName)
                    && DMD.Strings.EQ(this.m_DriverName, obj.m_DriverName)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Modello, obj.m_Modello)
                    ;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public int CompareTo(CRegisteredDevice value)
            {
                int ret = DMD.Strings.Compare(this.ClassName, value.ClassName, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.DeviceName, value.DeviceName, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.Address, value.Address, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CRegisteredDevice)obj);
            }

            void ISettingsOwner.NotifySettingsChanged(SettingsChangedEventArgs e)
            {
                this.SetChanged(true);
            }

            CSetting ISettingsOwner.CreateSetting(object args)
            {
                return new CSetting();
            }
        }
    }
}