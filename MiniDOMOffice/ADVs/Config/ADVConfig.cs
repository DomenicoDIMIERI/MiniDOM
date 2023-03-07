using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Configurazione delle campagne pubblicitarie
        /// </summary>
        [Serializable]
        public sealed class ADVConfig 
            : DMD.XML.DMDBaseXMLObject 
        {
            private ADVBannedAddressCollection m_BannedEMailAddresses = new ADVBannedAddressCollection();
            private ADVBannedAddressCollection m_BannedFaxAddresses = new ADVBannedAddressCollection();
            private ADVBannedAddressCollection m_BannedSMSAddresses = new ADVBannedAddressCollection();
            private ADVBannedAddressCollection m_BannedWebAddresses = new ADVBannedAddressCollection();

            /// <summary>
            /// Costruttore
            /// </summary>
            public ADVConfig()
            {
            }

            /// <summary>
            /// Indirizzi email bannati
            /// </summary>
            public ADVBannedAddressCollection BannedEMailAddresses
            {
                get
                {
                    return m_BannedEMailAddresses;
                }
            }

            /// <summary>
            /// Indirizzi fax bannati
            /// </summary>
            public ADVBannedAddressCollection BannedFaxAddresses
            {
                get
                {
                    return m_BannedFaxAddresses;
                }
            }

            /// <summary>
            /// Indirizzi sms bannati
            /// </summary>
            public ADVBannedAddressCollection BannedSMSAddresses
            {
                get
                {
                    return m_BannedSMSAddresses;
                }
            }

            /// <summary>
            /// Indirizzi web bannati
            /// </summary>
            public ADVBannedAddressCollection BannedWebAddresses
            {
                get
                {
                    return m_BannedWebAddresses;
                }
            }

            //protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            //{
            //    bool ret = base.SaveToDatabase(dbConn, force);
            //    SetConfig(this);
            //    return ret;
            //}

            //public override CModulesClass GetModule()
            //{
            //    return null;
            //}

            //internal void Load()
            //{
            //    string dbSQL = "SELECT * FROM [tbl_ADVConfig] ORDER BY [ID] ASC";
            //    var dbRis = Database.ExecuteReader(dbSQL);
            //    if (dbRis.Read())
            //    {
            //        Database.Load(this, dbRis);
            //    }

            //    dbRis.Dispose();
            //}

            //protected override bool SaveToRecordset(DBWriter writer)
            //{
            //    m_BannedEMailAddresses.Write(writer, "BannedEMailAddresses");
            //    m_BannedFaxAddresses.Write(writer, "BannedFaxAddresses");
            //    m_BannedSMSAddresses.Write(writer, "BannedSMSAddresses");
            //    m_BannedWebAddresses.Write(writer, "BannedWebAddresses");
            //    return base.SaveToRecordset(writer);
            //}

            //protected override bool LoadFromRecordset(DBReader reader)
            //{
            //    m_BannedEMailAddresses.Read(reader, "BannedEMailAddresses");
            //    m_BannedFaxAddresses.Read(reader, "BannedFaxAddresses");
            //    m_BannedSMSAddresses.Read(reader, "BannedSMSAddresses");
            //    m_BannedWebAddresses.Read(reader, "BannedWebAddresses");
            //    return base.LoadFromRecordset(reader);
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_ADVConfig";
            //}

            //protected override Databases.CDBConnection GetConnection()
            //{
            //    return Database;
            //}

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("BannedEMailAddresses", m_BannedEMailAddresses);
                writer.WriteTag("BannedFaxAddresses", m_BannedFaxAddresses);
                writer.WriteTag("BannedSMSAddresses", m_BannedSMSAddresses);
                writer.WriteTag("BannedWebAddresses", m_BannedWebAddresses);
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
                    case "BannedEMailAddresses":
                        {
                            m_BannedEMailAddresses = (ADVBannedAddressCollection)fieldValue;
                            break;
                        }

                    case "BannedFaxAddresses":
                        {
                            m_BannedFaxAddresses = (ADVBannedAddressCollection)fieldValue;
                            break;
                        }

                    case "BannedSMSAddresses":
                        {
                            m_BannedSMSAddresses = (ADVBannedAddressCollection)fieldValue;
                            break;
                        }

                    case "BannedWebAddresses":
                        {
                            m_BannedWebAddresses = (ADVBannedAddressCollection)fieldValue;
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

        /// <summary>
        /// Evento generato quando viene modificata la configurazione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public static event ConfigurationChangedEventHandler ConfigurationChanged;

        public delegate void ConfigurationChangedEventHandler(object sender, EventArgs e);

        private static ADVConfig m_Config;

        /// <summary>
        /// Oggetto che contiene alcuni parametri relativi alla configurazione del modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADVConfig Configuration
        {
            get
            {
                if (m_Config is null)
                {
                    var str = minidom.Sistema.ApplicationContext.Settings.GetValueString("ADV.Config", "");
                    m_Config = DMD.XML.Utils.Deserialize<ADVConfig>(str);
                }

                return m_Config;
            }
        }

        public static void SetConfig(ADVConfig value)
        {
            var str = DMD.XML.Utils.Serialize(value);
            minidom.Sistema.ApplicationContext.Settings.SetValueString("ADV.Config", str);
            m_Config = value;
            ConfigurationChanged?.Invoke(null, new EventArgs());
        }
    }
}