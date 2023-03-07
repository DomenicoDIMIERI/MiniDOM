using System;
using DMD.Databases.Collections;
using DMD.SIP;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class CTelephoneServiceClass
        {

        //    /// <summary>
        ///// Evento generato quando viene modificata la configurazione di questo oggetto
        ///// </summary>
        ///// <param name="e"></param>
        ///// <remarks></remarks>
        //    public event ConfigurationChangedEventHandler ConfigurationChanged;

        //    public delegate void ConfigurationChangedEventHandler(EventArgs e);

        //    /// <summary>
        ///// Evento generato quando un Telephone inviato cambia stato
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ///// <remarks></remarks>
        //    public event TelephoneStatusChangedEventHandler TelephoneStatusChanged;

        //    public delegate void TelephoneStatusChangedEventHandler(object sender, TelephoneStatusEventArgs e);


        //    /// <summary>
        ///// Evento generato quando un Telephone inviato cambia stato
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ///// <remarks></remarks>
        //    public event TelephoneDeliverEventHandler TelephoneDeliver;

        //    public delegate void TelephoneDeliverEventHandler(object sender, TelephoneDeliverEventArgs e);

        //    /// <summary>
        ///// Evento generato quando viene ricevuto un Telephone
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ///// <remarks></remarks>
        //    public event TelephoneReceivedEventHandler TelephoneReceived;

        //    public delegate void TelephoneReceivedEventHandler(object sender, TelephoneReceivedEventArgs e);

        //    /// <summary>
        ///// Evento generato per segnalare un evento generico del driver
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ///// <remarks></remarks>
        //    public event TelephoneEventEventHandler TelephoneEvent;

        //    public delegate void TelephoneEventEventHandler(object sender, TelephoneEventArgs e);

        //    private CKeyCollection<BasicTelephoneDriver> m_InstalledDrivers = new CKeyCollection<BasicTelephoneDriver>();
        //    private BasicTelephoneDriver m_DefualtDriver;
        //    private CTelephoneConfig m_Config;

        //    internal CTelephoneServiceClass()
        //    {
        //        //DMDObject.IncreaseCounter(this);
        //        var nd = new Drivers.NullTelephoneDriver();
        //        m_InstalledDrivers.Add(nd.GetUniqueID(), nd);
        //    }

        //    public Sistema.CTelephoneConfig Config
        //    {
        //        get
        //        {
        //            if (m_Config is null)
        //            {
        //                m_Config = new Sistema.CTelephoneConfig();
        //                m_Config.Load();
        //            }

        //            return m_Config;
        //        }
        //    }

        //    protected internal void SetConfig(Sistema.CTelephoneConfig value)
        //    {
        //        m_Config = value;
        //        m_DefualtDriver = null;
        //        doConfigChanged(new EventArgs());
        //    }

        //    internal void doConfigChanged(EventArgs e)
        //    {
        //        ConfigurationChanged?.Invoke(e);
        //    }

        //    public CKeyCollection<BasicTelephoneDriver> GetInstalledDrivers()
        //    {
        //        return new CKeyCollection<BasicTelephoneDriver>(m_InstalledDrivers);
        //    }

        //    public void InstallDriver(BasicTelephoneDriver driver)
        //    {
        //        if (m_InstalledDrivers.ContainsKey(driver.GetUniqueID()))
        //        {
        //            throw new ArgumentException("Il driver " + driver.GetUniqueID() + " è già installato");
        //        }

        //        m_InstalledDrivers.Add(driver.GetUniqueID(), driver);
        //    }

        //    public void RemoveDriver(BasicTelephoneDriver driver)
        //    {
        //        m_InstalledDrivers.RemoveByKey(driver.GetUniqueID());
        //    }

        //    /// <summary>
        ///// Restituisce o imposta il driver predefinito utilizzato per l'invio degli Telephone
        ///// </summary>
        ///// <value></value>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //    public BasicTelephoneDriver DefaultDriver
        //    {
        //        get
        //        {
        //            // 'If (m_DefualtDriver Is Nothing) Then
        //            // '    Dim dName As String = Trim([Module].Settings.GetValueString("DefaultTelephoneDriverName", ""))
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
        //        // Set(value As BasicTelephoneDriver)
        //        // 'If (value IsNot Nothing) Then
        //        // '    [Module].Settings.SetValueString("DefaultTelephoneDriverName", value.GetUniqueID)
        //        // 'Else
        //        // '    [Module].Settings.SetValueString("DefaultTelephoneDriverName", "")
        //        // 'End If
        //        // m_DefualtDriver = value
        //        // End Set
        //    }

        //    /// <summary>
        ///// Invia un Telephone utilizzando il driver predefinito
        ///// </summary>
        ///// <param name="targetNumber"></param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //    public string Send(string targetNumber, string message)
        //    {
        //        return Send(DefaultDriver, targetNumber, message, (TelephoneLineSettings)DefaultDriver.GetDefaultOptions());
        //    }

        //    /// <summary>
        //    /// Invia un Telephone utilizzando il driver predefinito
        //    /// </summary>
        //    /// <param name="targetNumber"></param>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    /// <remarks></remarks>
        //    public string Send(string targetNumber, string message, TelephoneLineSettings options)
        //    {
        //        return Send(DefaultDriver, targetNumber, message, options);
        //    }

        //    /// <summary>
        //    /// Invia un Telephone utilizzando il driver predefinito
        //    /// </summary>
        //    /// <param name="targetNumber"></param>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    /// <remarks></remarks>
        //    public string Send(BasicTelephoneDriver driver, string targetNumber, string message)
        //    {
        //        if (driver is null)
        //            throw new ArgumentNullException("driver");
        //        return Send(driver, targetNumber, message, (TelephoneLineSettings)driver.GetDefaultOptions());
        //    }

        //    /// <summary>
        //    /// Invia un Telephone utilizzando il driver predefinito
        //    /// </summary>
        //    /// <param name="targetNumber"></param>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    /// <remarks></remarks>
        //    public string Send(BasicTelephoneDriver driver, string targetNumber, string message, TelephoneLineSettings options)
        //    {
        //        if (driver is null)
        //            throw new ArgumentNullException("driver");
        //        if (options is null)
        //            throw new ArgumentNullException("options");
        //        return driver.Dial(targetNumber, message, options);
        //    }

        //    public BasicTelephoneDriver GetDriver(string driverId)
        //    {
        //        lock (this)
        //            return m_InstalledDrivers.GetItemByKey(driverId);
        //    }

        //    public SIPSession GetStatus(string messageID)
        //    {
        //        return GetStatus(DefaultDriver, messageID);
        //    }

        //    public SIPSession GetStatus(BasicTelephoneDriver driver, string messageID)
        //    {
        //        if (driver is null)
        //            throw new ArgumentNullException("driver");
        //        return driver.GetStatus(messageID);
        //    }

        //    public bool IsValidNumber(string value)
        //    {
        //        return IsValidNumber(DefaultDriver, value);
        //    }

        //    public bool IsValidNumber(BasicTelephoneDriver driver, string value)
        //    {
        //        if (driver is null)
        //            throw new ArgumentNullException("driver");
        //        return driver.IsValidNumber(value);
        //    }

        //    internal void doIncomingCall(TelephoneReceivedEventArgs e)
        //    {
        //        TelephoneReceived?.Invoke(null, e);
        //    }

        //    internal void doTelephoneStatusChanged(TelephoneStatusEventArgs e)
        //    {
        //        TelephoneStatusChanged?.Invoke(null, e);
        //    }

        //    internal void doTelephoneEvent(TelephoneEventArgs e)
        //    {
        //        TelephoneEvent?.Invoke(null, e);
        //    }

        //    internal void doNotifyCall(TelephoneDialEventArgs e)
        //    {
        //        TelephoneDeliver?.Invoke(null, e);
        //    }

            //public TelephoneLineSettings GetDriverConfiguration(string drvName)
            //{
            //    var driver = GetDriver(drvName);
            //    return (TelephoneLineSettings)driver.GetDefaultOptions();
            //}

            //public void SetDriverConfiguration(string drvName, TelephoneLineSettings config)
            //{
            //    var driver = GetDriver(drvName);
            //    driver.SetDefaultOptions(config);
            //}

            //protected internal void UpdateDriver(BasicTelephoneDriver drv)
            //{
            //    lock (this)
            //    {
            //        int i = m_InstalledDrivers.IndexOfKey(drv.GetUniqueID());
            //        if (i >= 0)
            //        {
            //            bool isConnected = m_InstalledDrivers[i].IsConnected();
            //            if (isConnected)
            //                m_InstalledDrivers[i].Disconnect();
            //            m_InstalledDrivers[i] = drv;
            //            if (isConnected)
            //                m_InstalledDrivers[i].Connect();
            //        }
            //        else
            //        {
            //            m_InstalledDrivers.Add(drv.GetUniqueID(), drv);
            //            drv.Connect();
            //        }
            //    }
            //}

         

        //    /// <summary>
        ///// Inizializza le tabelle
        ///// </summary>
        //    public void Initialize()
        //    {
        //        var db = Databases.APPConn;
        //        if (!db.TableExists("tbl_TelephoneConfig"))
        //        {
        //            string dbSQL;
        //            dbSQL = "CREATE TABLE [tbl_TelephoneConfig]" + "(" + "[ID] Counter Primary Key," + "[DefaultDriverName] Text(255)," + "[DefaultSenderName] Text(255)," + "[DefaultSenderNumber] Text(255)," + "[Flags] Int," + "[CreatoDa] Int," + "[CreatoIl] Date," + "[ModificatoDa] Int," + "[ModificatoIl] Date," + "[Stato] Int" + ")";
        //            Databases.DBUtils.CreateTable(db, dbSQL);
        //            Databases.DBUtils.CreateIndex(db, "idxDefaultDriverName", "tbl_TelephoneConfig", new[] { "DefaultDriverName" });
        //        }
        //    }
        }
    }

    public partial class Sistema
    {
        private static CTelephoneServiceClass m_TelephoneService = null;

        public static CTelephoneServiceClass TelephoneService
        {
            get
            {
                if (m_TelephoneService is null)
                {
                    m_TelephoneService = new CTelephoneServiceClass();
                    //m_TelephoneService.Initialize();
                }

                return m_TelephoneService;
            }
        }
    }
}