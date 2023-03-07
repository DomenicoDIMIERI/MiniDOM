using System;
using System.Collections.Generic;
using System.Data;
using DMD;
using DMD.Databases;
using static minidom.Anagrafica;
using static minidom.Sistema;

namespace minidom
{

    /// <summary>
    /// Evento generato quando due oggetti CPersona vengono unifificati attraverso il metodo Merge
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    /// <remarks></remarks>
    public delegate void PersonaMergedEventHandler(object sender, MergePersonaEventArgs e);
     
    /// <summary>
    /// Evento generato quando viene annullata l'unione di due anagrafiche
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    /// <remarks></remarks>
    public delegate void PersonaUnMergedEventHandler(object sender, MergePersonaEventArgs e);

    /// <summary>
    /// Evento generato quando viene chiamato il metodo Transfer su un oggetto CPersona per trasferire la competenza ad un ufficio diverso
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    /// <remarks></remarks>
    public delegate void PersonaTransferredEventHandler(object sender, TransferPersonaEventArgs e);
     
    /// <summary>
    /// Modulo Anagrafica
    /// </summary>
    public sealed partial class Anagrafica
    {
        private static Sistema.CModule m_Module;


        /// <summary>
        /// Evento generato quando una persona o un'azienda viene creata
        /// </summary>
        /// <remarks></remarks>
        public static event ItemEventHandler<CPersona> PersonaCreated;

        /// <summary>
        /// Evento generato quando una persona o un'azienda viene eliminata
        /// </summary>
        /// <remarks></remarks>
        public static event ItemEventHandler<CPersona> PersonaDeleted;

        /// <summary>
        /// Evento generato quando una persona o un'azienda viene modificata
        /// </summary>
        /// <remarks></remarks>
        public static event ItemEventHandler<CPersona> PersonaModified;

        /// <summary>
        /// Evento generato quando due oggetti CPersona vengono unifificati attraverso il metodo Merge
        /// </summary>
        /// <remarks></remarks>
        public static event PersonaMergedEventHandler PersonaMerged;

        
        /// <summary>
        /// Evento generato quando viene annullata l'unione di due anagrafiche
        /// </summary>
        /// <remarks></remarks>
        public static event PersonaUnMergedEventHandler PersonaUnMerged;

        
        /// <summary>
        /// Evento generato quando viene chiamato il metodo Transfer su un oggetto CPersona per trasferire la competenza ad un ufficio diverso
        /// </summary>
        /// <remarks></remarks>
        public static event PersonaTransferredEventHandler PersonaTransferred;

     
        /// <summary>
        /// Lock sul modulo Anagrafica
        /// </summary>
        public static readonly object @lock = new object();

        /// <summary>
        /// Costruttore
        /// </summary>
        private Anagrafica()
        {
        }

        
        

        ///// <summary>
        ///// Inizializza i providers
        ///// </summary>
        //public static void Intialize()
        //{
        //    Sistema.Types.RegisteredTypeProviders.Add("CFonte", Fonti.GetItemById);
        //    Sistema.Types.RegisteredTypeProviders.Add("CCanale", Canali.GetItemById);
        //    Sistema.Types.RegisteredTypeProviders.Add("CAzienda", Aziende.GetItemById);
        //    Sistema.Types.RegisteredTypeProviders.Add("CPersonaFisica", Persone.GetItemById);
        //}

        /// <summary>
        /// Descrittore del modulo anagrafica
        /// </summary>
        public static CModule Module
        {
            get
            {
                if (m_Module is null)
                    m_Module = minidom.Sistema.Modules.GetItemByName("modAnagrafica");
                return m_Module;
            }
        }

        
        /// <summary>
        /// Genera l'evento PersonaTransferred
        /// </summary>
        /// <param name="e"></param>
        internal static void OnPersonaTransferred(TransferPersonaEventArgs e)
        {
            PersonaTransferred?.Invoke(null, e);
        }

        
        /// <summary>
        /// Genera l'evento PersonaUnMerged
        /// </summary>
        /// <param name="e"></param>
        internal static void OnPersonaUnMerged(MergePersonaEventArgs e)
        {
            PersonaUnMerged?.Invoke(null, e);
        }

        /// <summary>
        /// Genera l'evento PersonaMerged
        /// </summary>
        /// <param name="e"></param>
        internal static void OnPersonaMerged(MergePersonaEventArgs e)
        {
            PersonaMerged?.Invoke(null, e);
        }

        /// <summary>
        /// Genera l'evento PersonaCreated
        /// </summary>
        /// <param name="e"></param>
        internal static void OnPersonaCreated(ItemEventArgs<CPersona> e)
        {
            PersonaCreated?.Invoke(null, e);
        }

        /// <summary>
        /// Genera l'evento PersonaDeleted
        /// </summary>
        /// <param name="e"></param>
        internal static void OnPersonaDeleted(ItemEventArgs<CPersona> e)
        {
            PersonaDeleted?.Invoke(null, e);
        }

        /// <summary>
        /// Genera l'evento PersonaChanged
        /// </summary>
        /// <param name="e"></param>
        internal static void OnPersonaModified(ItemEventArgs<CPersona> e)
        {
            PersonaModified?.Invoke(null, e);
        }

    }
}