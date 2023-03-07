using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;
using minidom.internals;
using DMD.FAX;
using DMD.FAX.Drivers;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Sistema di invio/ricezione fax
        /// </summary>
        public sealed partial class CFaxServiceClass
        {

            /// <summary>
            /// Evento generato quando viene ricevuto un fax
            /// </summary>
            public event FaxReceivedEventHandler FaxReceived;

            ///// <summary>
            ///// Evento generato quando viene modificata la configurazione di questo oggetto
            ///// </summary>
            ///// <remarks></remarks>
            //public event ConfigurationChangedEventHandler ConfigurationChanged;

            ///// <summary>
            ///// Evento generato quando viene modificata la configurazione di questo oggetto
            ///// </summary>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //public delegate void ConfigurationChangedEventHandler(EventArgs e);

            ///// <summary>
            ///// Evento generato quando si verifica un errore nell'invio di un fax
            ///// </summary>
            ///// <remarks></remarks>
            //public event FaxFailedEventHandler FaxFailed;

            ///// <summary>
            ///// Evento generato quando si verifica un errore nell'invio di un fax
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //public delegate void FaxFailedEventHandler(object sender, FaxJobEventArgs e);

            ///// <summary>
            ///// Evento generato quando un Fax viene inviato correttamente
            ///// </summary>
            ///// <remarks></remarks>
            //public event FaxDeliveredEventHandler FaxDelivered;

            ///// <summary>
            ///// Evento generato quando un Fax viene inviato correttamente
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //public delegate void FaxDeliveredEventHandler(object sender, FaxDeliverEventArgs e);

            ///// <summary>
            ///// Evento generato quando viene ricevuto un Fax
            ///// </summary>
            ///// <remarks></remarks>
            //public event FaxReceivedEventHandler FaxReceived;

            ///// <summary>
            ///// Evento generato quando viene ricevuto un Fax
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            ///// <remarks></remarks>
            //public delegate void FaxReceivedEventHandler(object sender, FaxReceivedEventArgs e);

            //private CKeyCollection<BaseFaxDriver> m_InstalledDrivers = new CKeyCollection<BaseFaxDriver>();
            //private BaseFaxDriver m_DefualtDriver;
            //private CFaxConfig m_Config;

            ///// <summary>
            ///// Costruttore
            ///// </summary>
            //public CFaxServiceClass()
            //{
            //    //DMDObject.IncreaseCounter(this);
            //    var nd = new NullFaxDriver();
            //    m_InstalledDrivers.Add(nd.GetUniqueID(), nd);
            //}

            ///// <summary>
            ///// Restituisce o imposta la configurazione del sistema di invio/ricezione dei fax
            ///// </summary>
            //public CFaxConfig Config
            //{
            //    get
            //    {
            //        if (m_Config is null)
            //        {
            //            string str = minidom.Sistema.ApplicationContext.Settings.GetValueString("Sistema.FaxService.Config", "");
            //            m_Config = DMD.XML.Utils.Deserialize<CFaxConfig>(str);
            //        }

            //        return m_Config;
            //    }
            //}

            ///// <summary>
            ///// Imposta la configurazione
            ///// </summary>
            ///// <param name="value"></param>
            //internal void SetConfig(CFaxConfig value)
            //{
            //    string str = DMD.XML.Utils.Serialize(value);
            //    minidom.Sistema.ApplicationContext.Settings.SetValueString("Sistema.FaxService.Config", str);
            //    m_Config = value;
            //    m_DefualtDriver = null;
            //    doConfigChanged(new EventArgs());
            //}

            //internal void doConfigChanged(EventArgs e)
            //{
            //    ConfigurationChanged?.Invoke(e);
            //}

            ///// <summary>
            ///// Restituisce la collezione dei driver installati
            ///// </summary>
            ///// <returns></returns>
            //public CKeyCollection<BaseFaxDriver> GetInstalledDrivers()
            //{
            //    lock (this)
            //        return new CKeyCollection<BaseFaxDriver>(m_InstalledDrivers);
            //}

            ///// <summary>
            ///// Aggiorna la configurazione del driver
            ///// </summary>
            ///// <param name="drv"></param>
            //internal void UpdateDriver(BaseFaxDriver drv)
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

            ///// <summary>
            ///// Restitusice vero se la stringa passata come argomento rappresenta un numero di fax valido
            ///// </summary>
            ///// <param name="number"></param>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //public bool IsValidNumber(string number)
            //{
            //    return !string.IsNullOrEmpty(Sistema.Formats.ParsePhoneNumber(number));
            //}

            ///// <summary>
            ///// Installa un driver
            ///// </summary>
            ///// <param name="driver"></param>
            //public void InstallDriver(BaseFaxDriver driver)
            //{
            //    lock (this)
            //    {
            //        if (m_InstalledDrivers.ContainsKey(driver.GetUniqueID()))
            //        {
            //            throw new ArgumentException("Il driver " + driver.GetUniqueID() + " è già installato");
            //        }

            //        m_InstalledDrivers.Add(driver.GetUniqueID(), driver);
            //    }
            //}

            ///// <summary>
            ///// Rimuove un driver
            ///// </summary>
            ///// <param name="driver"></param>
            //public void RemoveDriver(BaseFaxDriver driver)
            //{
            //    lock (this)
            //        m_InstalledDrivers.RemoveByKey(driver.GetUniqueID());
            //}


            ///// <summary>
            ///// Restituisce o imposta il driver predefinito utilizzato per l'invio degli SMS
            ///// </summary>
            ///// <value></value>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //public BaseFaxDriver DefaultDriver
            //{
            //    get
            //    {
            //        lock (this)
            //        {
            //            // If (m_DefualtDriver Is Nothing) Then
            //            // Dim dName As String = Trim([Module].Settings.GetValueString("DefaultFaxDriverName", ""))
            //            // If (dName <> "") Then m_DefualtDriver = m_InstalledDrivers.GetItemByKey(dName)
            //            // End If
            //            if (m_DefualtDriver is null && m_InstalledDrivers.Count > 0)
            //            {
            //                m_DefualtDriver = m_InstalledDrivers.GetItemByKey(Config.DefaultDriverName);
            //                if (m_DefualtDriver is null)
            //                    m_DefualtDriver = m_InstalledDrivers[0];
            //            }

            //            return m_DefualtDriver;
            //        }
            //    }
            //    // Set(value As BaseFaxDriver)
            //    // 'If (value IsNot Nothing) Then
            //    // '    My.Settings.FAXDefaultDriver = value.GetUniqueID
            //    // 'Else
            //    // '    [Module].Settings.SetValueString("DefaultFaxDriverName", "")
            //    // 'End If
            //    // m_DefualtDriver = value
            //    // End Set
            //}

            ///// <summary>
            ///// Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
            ///// </summary>
            ///// <param name="destNumber">[in] Numero di fax</param>
            ///// <param name="fileName">[in] Nome del file da inviare come fax</param>
            ///// <remarks></remarks>
            //public FaxJob Send(string destNumber, string fileName)
            //{
            //    return Send(DefaultDriver, destNumber, fileName);
            //}

            ///// <summary>
            ///// Invia un fax
            ///// </summary>
            ///// <param name="destNumber"></param>
            ///// <param name="fileName"></param>
            ///// <param name="options"></param>
            ///// <returns></returns>
            //public FaxJob Send(string destNumber, string fileName, FaxDriverOptions options)
            //{
            //    return Send(DefaultDriver, destNumber, fileName, options);
            //}

            ///// <summary>
            ///// Invia un fax
            ///// </summary>
            ///// <param name="driver"></param>
            ///// <param name="destNumber"></param>
            ///// <param name="fileName"></param>
            ///// <returns></returns>
            //public FaxJob Send(BaseFaxDriver driver, string destNumber, string fileName)
            //{
            //    return Send(driver, destNumber, fileName, (FaxDriverOptions)driver.GetDefaultOptions());
            //}

            /// <summary>
            /// Restituisce true se il numero di fax é valido
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public bool IsValidNumber(string address)
            {
                //TODO implementare il controllo del numero di fax
                throw new NotImplementedException();
            }

            ///// <summary>
            ///// Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
            ///// </summary>
            ///// <param name="destNumber">[in] Numero di fax</param>
            ///// <param name="fileName">[in] Nome del file da inviare come FAX</param>
            ///// <remarks></remarks>
            //public FaxJob Send(BaseFaxDriver driver, string destNumber, string fileName, FaxDriverOptions options)
            //{
            //    if (driver is null)
            //        throw new ArgumentNullException("driver");
            //    if (!System.IO.File.Exists(fileName))
            //        throw new System.IO.FileNotFoundException(fileName);
            //    if (options is null)
            //        throw new ArgumentNullException("options");
            //    var ret = NewJob();
            //    options.TargetNumber = destNumber;
            //    options.FileName = fileName;
            //    ret.SetDriver(driver);
            //    ret.SetOptions(options);
            //    Send(ret);
            //    return ret;
            //}

            ///// <summary>
            ///// Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
            ///// </summary>
            ///// <param name="job">[in] Fax da inviare</param>
            ///// <remarks></remarks>
            //public string Send(BaseFaxDriver driver, FaxJob job)
            //{
            //    if (driver is null)
            //        throw new ArgumentNullException("driver");
            //    job.SetDriver(driver);
            //    return Send(job);
            //}

            ///// <summary>
            ///// Invia un fax
            ///// </summary>
            ///// <param name="job"></param>
            ///// <returns></returns>
            //public string Send(FaxJob job)
            //{
            //    job.Send();
            //    return job.JobID;
            //}

            ///// <summary>
            ///// Genera l'evento FaxReceived
            ///// </summary>
            ///// <param name="e"></param>
            //internal void doFaxReceived(FaxReceivedEventArgs e)
            //{
            //    FaxReceived?.Invoke(null, e);
            //}

            ///// <summary>
            ///// Genera l'evento FaxDelivered
            ///// </summary>
            ///// <param name="e"></param>
            //internal void doFaxDelivered(FaxDeliverEventArgs e)
            //{
            //    FaxDelivered?.Invoke(null, e);
            //}


            ///// <summary>
            ///// Genera l'evento FaxFailed
            ///// </summary>
            ///// <param name="e"></param>
            //internal void doFaxFailed(FaxJobEventArgs e)
            //{
            //    FaxFailed?.Invoke(null, e);
            //}

            ///// <summary>
            ///// Restituisce il driver con id
            ///// </summary>
            ///// <param name="driverId"></param>
            ///// <returns></returns>
            //public BaseFaxDriver GetDriver(string driverId)
            //{
            //    lock (this)
            //        return m_InstalledDrivers.GetItemByKey(driverId);
            //}

            ///// <summary>
            ///// Crea un nuovo job
            ///// </summary>
            ///// <returns></returns>
            //public FaxJob NewJob()
            //{
            //    var job = new FaxJob();
            //    job.SetDriver(DefaultDriver);
            //    job.SetJobStatus(FaxJobStatus.PREPARING);
            //    job.SetOptions((FaxDriverOptions)DefaultDriver.GetDefaultOptions());
            //    return job;
            //}

            ///// <summary>
            ///// Restituisce la configurazione del driver
            ///// </summary>
            ///// <param name="drvName"></param>
            ///// <returns></returns>
            //public FaxDriverOptions GetDriverConfiguration(string drvName)
            //{
            //    var driver = GetDriver(drvName);
            //    return (FaxDriverOptions)driver.GetDefaultOptions();
            //}

            ///// <summary>
            ///// Imposta la configurazione del driver
            ///// </summary>
            ///// <param name="drvName"></param>
            ///// <param name="config"></param>
            //public void SetDriverConfiguration(string drvName, FaxDriverOptions config)
            //{
            //    var driver = GetDriver(drvName);
            //    driver.SetConfig(config);
            //}

            ////~CFaxServiceClass()
            ////{
            ////    //DMDObject.DecreaseCounter(this);
            ////}
        }
    }

    public partial class Sistema
    {
        private static CFaxServiceClass m_FaxService = null;

        /// <summary>
        /// Sistema di invio/ricezione fax
        /// </summary>
        public static CFaxServiceClass FaxService
        {
            get
            {
                if (m_FaxService is null)
                    m_FaxService = new CFaxServiceClass();
                return m_FaxService;
            }
        }
    }
}