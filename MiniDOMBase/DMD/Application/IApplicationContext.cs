using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Interfaccia implementata dall'applicazione e necessaria per interfacciarsi alla libreria
        /// </summary>
        /// <remarks></remarks>
        public interface IApplicationContext 
            : ISettingsOwner
        {

            // ''' <summary>
            // ''' Restituisce o imposta la collezione delle impostazioni di sistema
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // ReadOnly Property Settings As CSettings

            /// <summary>
            /// Restituisce l'oggetto di tipo specifico con id specifico.
            /// L'oggetto viene cercato nel repository che gestisce il tipo
            /// </summary>
            /// <param name="objectType"></param>
            /// <param name="objectId"></param>
            /// <returns></returns>
            object GetItemByTypeAndId(string objectType, int objectId);


            string RemoteIP { get; }
            int RemotePort { get; }

            /// <summary>
            /// Restituisce la cartella di avvio del programma
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string StartupFloder { get; }

            /// <summary>
            /// Restituisce la cartella temporanea predefinita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string TmporaryFolder { get; }

            /// <summary>
            /// Restituisce la cartella in cui l'utente corrente può salvare i dati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string UserDataFolder { get; }

            /// <summary>
            /// Restituisce la cartella dati di sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string SystemDataFolder { get; }

            // ''' <summary>
            // ''' Restituisce il percorso predefinito dell'utente corrente
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // ReadOnly Property UserFolder As String


            /// <summary>
            /// Restituisce l'ID dell'applicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string InstanceID { get; }

            /// <summary>
            /// Restituisce la descrizione dell'applicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string Description { get; }

            /// <summary>
            /// Restituisce l'utente corrente (nella sessione)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            CUser CurrentUser { get; set; }

            /// <summary>
            /// Restituisce la sessione corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            object CurrentSession { get; set; }

            /// <summary>
            /// Restituisce l'assembly principale
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            System.Reflection.Assembly GetEntryAssembly();

            /// <summary>
            /// Restituisce un parametro letto dall'applicazione
            /// </summary>
            /// <param name="paramName"></param>
            /// <param name="defValue"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            string GetParameter(string paramName, string defValue = DMD.Strings.vbNullString);

            /// <summary>
            /// Restituisce un parametro letto dall'applicazione
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="paramName"></param>
            /// <param name="defValue"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            T GetParameter<T>(string paramName, object defValue = null);

            /// <summary>
            /// Effettua il mapping del percorso
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            string MapPath(string path);

            /// <summary>
            /// Effettua l'unmapping del percorso
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            string UnMapPath(string path);

            /// <summary>
            /// Restituisce vero se l'utente è connesso
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            bool IsUserLogged(CUser user);

            /// <summary>
            /// Restituisce vero se l'applicazione è in modalità di debug
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            bool IsDebug();

            /// <summary>
            /// Restituisce o imposta l'azienda di lavoro principale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            int IDAziendaPrincipale { get; set; }


            string SupportEMail { get; }

            /// <summary>
            /// Restituisec la url del percorso di lavoro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string BaseURL { get; }

            /// <summary>
            /// Restituisce una descrizione breve dell'applicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string Title { get; }

            /// <summary>
            /// Ferma l'applicazione e rilascia tutte le risorse
            /// </summary>
            /// <remarks></remarks>
            void Stop();

            /// <summary>
            /// Inizializza l'applicazione ed acquisice le risorse
            /// </summary>
            /// <remarks></remarks>
            void Start();

            /// <summary>
            /// Restituisce l'ufficio di lavoro corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            CUfficio CurrentUfficio { get; set; }

            /// <summary>
            /// Restituisce o imposta la connessione al db principale
            /// </summary>
            DBConnection APPConn { get; set; }

            /// <summary>
            /// Restituisce o imposta la connessione al db dei log
            /// </summary>
            DBConnection LOGConn { get; set; }

            /// <summary>
            /// Restituisce o imposta l'oggetto LoginHistory della sessione corrente
            /// </summary>
            CLoginHistory CurrentLogin { get; set; }

            /// <summary>
            /// Restituisce una proprietà dell'applicazione in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            object GetProperty(string name);

            /// <summary>
            /// Restituisce vero se l'applicazione è stata messa in stato manutenzione
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            bool IsMaintenance();

            /// <summary>
            /// Mette l'applicazione in stato manutenzione
            /// </summary>
            /// <remarks></remarks>
            void EnterMaintenance();

            /// <summary>
            /// Esce dallo stato manutenzione
            /// </summary>
            /// <remarks></remarks>
            void QuitMaintenance();

            /// <summary>
            /// Notifica un messaggio di log
            /// </summary>
            /// <param name="message"></param>
            /// <param name="severity"></param>
            void Log(string message, DMD.WarningSeverity severity = DMD.WarningSeverity.MESSAGE);

            /// <summary>
            /// Crea un oggetto del tipo specificato
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            object CreateInstance(System.Type t);

            /// <summary>
            /// Crea un oggetto del tipo specificato
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            object CreateInstance(string typeName);
        }
    }
}