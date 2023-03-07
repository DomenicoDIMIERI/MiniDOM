using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;
using DMD.SMS;
 
namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Gestore degli sms
        /// </summary>
        public sealed class CSMSServiceClass
        {
            /// <summary>
            /// Evento generato quando viene ricevuto un sms
            /// </summary>
            public event SMSReceivedEventHandler SMSReceived;

            //    /// <summary>
            ///// Evento generato quando viene modificata la configurazione di questo oggetto
            ///// </summary>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //    public event ConfigurationChangedEventHandler ConfigurationChanged;

            //    public delegate void ConfigurationChangedEventHandler(EventArgs e);

            //    /// <summary>
            ///// Evento generato quando un SMS inviato cambia stato
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //    public event SMSStatusChangedEventHandler SMSStatusChanged;

            //    public delegate void SMSStatusChangedEventHandler(object sender, SMSStatusEventArgs e);


            //    /// <summary>
            ///// Evento generato quando un SMS inviato cambia stato
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //    public event SMSDeliverEventHandler SMSDeliver;

            //    public delegate void SMSDeliverEventHandler(object sender, SMSDeliverEventArgs e);

            //    /// <summary>
            ///// Evento generato quando viene ricevuto un SMS
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //    public event SMSReceivedEventHandler SMSReceived;

            
            //    /// <summary>
            ///// Evento generato per segnalare un evento generico del driver
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //    public event SMSEventEventHandler SMSEvent;

            //    public delegate void SMSEventEventHandler(object sender, SMSEventArgs e);

            //    private CKeyCollection<BasicSMSDriver> m_InstalledDrivers = new CKeyCollection<BasicSMSDriver>();
            //    private BasicSMSDriver m_DefualtDriver;
            //    private Sistema.CSMSConfig m_Config;

            //    internal CSMSServiceClass()
            //    {
            //        //DMDObject.IncreaseCounter(this);
            //        var nd = new NullSMSDriver();
            //        m_InstalledDrivers.Add(nd.GetUniqueID(), nd);
            //    }

            //    public Sistema.CSMSConfig Config
            //    {
            //        get
            //        {
            //            if (m_Config is null)
            //            {
            //                m_Config = new Sistema.CSMSConfig();
            //                m_Config.Load();
            //            }

            //            return m_Config;
            //        }
            //    }

            //    protected internal void SetConfig(Sistema.CSMSConfig value)
            //    {
            //        m_Config = value;
            //        m_DefualtDriver = null;
            //        doConfigChanged(new EventArgs());
            //    }

            //    internal void doConfigChanged(EventArgs e)
            //    {
            //        ConfigurationChanged?.Invoke(e);
            //    }

            //    public CKeyCollection<BasicSMSDriver> GetInstalledDrivers()
            //    {
            //        return new CKeyCollection<BasicSMSDriver>(m_InstalledDrivers);
            //    }

            //    public void InstallDriver(BasicSMSDriver driver)
            //    {
            //        if (m_InstalledDrivers.ContainsKey(driver.GetUniqueID()))
            //        {
            //            throw new ArgumentException("Il driver " + driver.GetUniqueID() + " è già installato");
            //        }

            //        m_InstalledDrivers.Add(driver.GetUniqueID(), driver);
            //    }

            //    public void RemoveDriver(BasicSMSDriver driver)
            //    {
            //        m_InstalledDrivers.RemoveByKey(driver.GetUniqueID());
            //    }

            //    /// <summary>
            ///// Restituisce o imposta il driver predefinito utilizzato per l'invio degli SMS
            ///// </summary>
            ///// <value></value>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //    public BasicSMSDriver DefaultDriver
            //    {
            //        get
            //        {
            //            // 'If (m_DefualtDriver Is Nothing) Then
            //            // '    Dim dName As String = Trim([Module].Settings.GetValueString("DefaultSMSDriverName", ""))
            //            // '    If (dName <> "") Then m_DefualtDriver = m_InstalledDrivers.GetItemByKey(dName)
            //            // 'End If
            //            // If (m_DefualtDriver Is Nothing AndAlso m_InstalledDrivers.Count > 0) Then
            //            // m_DefualtDriver = m_InstalledDrivers(0)
            //            // End If
            //            // Return m_DefualtDriver
            //            if (m_DefualtDriver is null && m_InstalledDrivers.Count > 0)
            //            {
            //                m_DefualtDriver = m_InstalledDrivers.GetItemByKey(Config.DefaultDriverName);
            //                if (m_DefualtDriver is null)
            //                    m_DefualtDriver = m_InstalledDrivers[0];
            //            }

            //            return m_DefualtDriver;
            //        }
            //        // Set(value As BasicSMSDriver)
            //        // 'If (value IsNot Nothing) Then
            //        // '    [Module].Settings.SetValueString("DefaultSMSDriverName", value.GetUniqueID)
            //        // 'Else
            //        // '    [Module].Settings.SetValueString("DefaultSMSDriverName", "")
            //        // 'End If
            //        // m_DefualtDriver = value
            //        // End Set
            //    }

            //    /// <summary>
            ///// Invia un SMS utilizzando il driver predefinito
            ///// </summary>
            ///// <param name="targetNumber"></param>
            ///// <param name="message"></param>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //    public string Send(string targetNumber, string message)
            //    {
            //        return Send(DefaultDriver, targetNumber, message, (SMSLineParameters)DefaultDriver.GetDefaultOptions());
            //    }

            //    /// <summary>
            ///// Invia un SMS utilizzando il driver predefinito
            ///// </summary>
            ///// <param name="targetNumber"></param>
            ///// <param name="message"></param>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //    public string Send(string targetNumber, string message, SMSLineParameters options)
            //    {
            //        return Send(DefaultDriver, targetNumber, message, options);
            //    }

            //    /// <summary>
            ///// Invia un SMS utilizzando il driver predefinito
            ///// </summary>
            ///// <param name="targetNumber"></param>
            ///// <param name="message"></param>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //    public string Send(BasicSMSDriver driver, string targetNumber, string message)
            //    {
            //        if (driver is null)
            //            throw new ArgumentNullException("driver");
            //        return Send(driver, targetNumber, message, (SMSLineParameters)driver.GetDefaultOptions());
            //    }

            //    /// <summary>
            ///// Invia un SMS utilizzando il driver predefinito
            ///// </summary>
            ///// <param name="targetNumber"></param>
            ///// <param name="message"></param>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //    public string Send(BasicSMSDriver driver, string targetNumber, string message, SMSLineParameters options)
            //    {
            //        if (driver is null)
            //            throw new ArgumentNullException("driver");
            //        if (options is null)
            //            throw new ArgumentNullException("options");
            //        return driver.Send(targetNumber, message, options);
            //    }

            //    public BasicSMSDriver GetDriver(string driverId)
            //    {
            //        lock (this)
            //            return m_InstalledDrivers.GetItemByKey(driverId);
            //    }

            //    public DriverMessageStatus GetStatus(string messageID)
            //    {
            //        return GetStatus(DefaultDriver, messageID);
            //    }

            //    public DriverMessageStatus GetStatus(BasicSMSDriver driver, string messageID)
            //    {
            //        if (driver is null)
            //            throw new ArgumentNullException("driver");
            //        return driver.GetStatus(messageID);
            //    }

            //    public bool IsValidNumber(string value)
            //    {
            //        return IsValidNumber(DefaultDriver, value);
            //    }

            //    public bool IsValidNumber(BasicSMSDriver driver, string value)
            //    {
            //        if (driver is null)
            //            throw new ArgumentNullException("driver");
            //        return driver.IsValidNumber(value);
            //    }

            //    internal void doSMSReceived(SMSReceivedEventArgs e)
            //    {
            //        SMSReceived?.Invoke(null, e);
            //    }

            //    internal void doSMSStatusChanged(SMSStatusEventArgs e)
            //    {
            //        SMSStatusChanged?.Invoke(null, e);
            //    }

            //    internal void doSMSEvent(SMSEventArgs e)
            //    {
            //        SMSEvent?.Invoke(null, e);
            //    }

            //    internal void doSMSDelivered(SMSDeliverEventArgs e)
            //    {
            //        SMSDeliver?.Invoke(null, e);
            //    }

            //    public SMSLineParameters GetDriverConfiguration(string drvName)
            //    {
            //        var driver = GetDriver(drvName);
            //        return (SMSLineParameters)driver.GetDefaultOptions();
            //    }

            //    public void SetDriverConfiguration(string drvName, SMSLineParameters config)
            //    {
            //        var driver = GetDriver(drvName);
            //        driver.SetDefaultOptions(config);
            //    }

            //    protected internal void UpdateDriver(BasicSMSDriver drv)
            //    {
            //        lock (this)
            //        {
            //            int i = m_InstalledDrivers.IndexOfKey(drv.GetUniqueID());
            //            if (i >= 0)
            //            {
            //                bool isConnected = m_InstalledDrivers[i].IsConnected();
            //                if (isConnected)
            //                    m_InstalledDrivers[i].Disconnect();
            //                m_InstalledDrivers[i] = drv;
            //                if (isConnected)
            //                    m_InstalledDrivers[i].Connect();
            //            }
            //            else
            //            {
            //                m_InstalledDrivers.Add(drv.GetUniqueID(), drv);
            //                drv.Connect();
            //            }
            //        }

            /// <summary>
            /// Restituisce true se il numero é valido per l'invio di un sms
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public bool IsValidNumber(string address)
            {
                //TODO Verifica del numero per gli sms
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce il messaggio in base all'id
            /// </summary>
            /// <param name="messageID"></param>
            /// <returns></returns>
            public DriverMessageStatus GetMessageById(string messageID)
            {
                //TODO implementare funzionalità SMS
                throw new NotImplementedException();
            }
        }



    }

    public partial class Sistema
    {
        private static CSMSServiceClass m_SMSService = null;

        /// <summary>
        /// Gestore degli sms
        /// </summary>
        public static CSMSServiceClass SMSService
        {
            get
            {
                if (m_SMSService is null)
                    m_SMSService = new CSMSServiceClass();
                return m_SMSService;
            }
        }
    }

}