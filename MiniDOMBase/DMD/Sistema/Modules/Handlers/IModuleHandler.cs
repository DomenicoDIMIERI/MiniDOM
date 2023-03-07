using System;
using DMD;
using DMD.Databases.Collections;
using DMD.XML;

namespace minidom
{

    /// <summary>
    /// Flag validi per gli handler dei moduli
    /// </summary>
    [Flags]
    public enum ModuleSupportFlags : int
    {

        /// <summary>
        /// Nessun flag
        /// </summary>
        SNone = 0,

        /// <summary>
        /// Supporta le annotazioni
        /// </summary>
        SAnnotations = 1,

        /// <summary>
        /// Supporta la creazione di nuovi elementi
        /// </summary>
        SCreate = 2,

        /// <summary>
        /// Supporta la modifica di elementi esistenti
        /// </summary>
        SEdit = 4,

        /// <summary>
        /// Supporta l'eliminazione degli elementi
        /// </summary>
        SDelete = 8,

        /// <summary>
        /// Supporta la duplicazione degli elementi
        /// </summary>
        SDuplicate = 16,

        /// <summary>
        /// Supporta l'importazione
        /// </summary>
        SImport = 32,

        /// <summary>
        /// Supporta l'espertazione
        /// </summary>
        SExport = 64,

        /// <summary>
        /// Supporta la stampa
        /// </summary>
        SPrint = 256
    }

    /// <summary>
    /// Interfaccia implementata dagli handler delle azioni di un modulo
    /// </summary>
    public interface IModuleHandler
    {
        /// <summary>
        /// Restituisce il modulo gestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        Sistema.CModule Module { get; }

        /// <summary>
        /// Imposta il modulo gestito
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
        void SetModule(Sistema.CModule value);

        /// <summary>
        /// Esegue l'azione specificata
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="renderer"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        string ExecuteAction(object renderer, string actionName);

        // ''' <summary>
        // ''' Esegue l'azione specificata
        // ''' </summary>
        // ''' <param name="actionName"></param>
        // ''' <returns></returns>
        // ''' <remarks></remarks>
        // Function ExecuteAction1(ByVal renderer As Object, ByVal actionName As String) As MethodResults


        /// <summary>
        /// Crea un cursore per l'accesso agli oggetti gestiti dal modulo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        Databases.DBObjectCursorBase CreateCursor();

        /// <summary>
        /// Restituisce vero se l'utente corrente puo eseguire il comando list sul modulo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        bool CanList();

        /// <summary>
        /// Restituisce true se la risorsa può essere creata
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CanCreate(object item);

        /// <summary>
        /// Restituisce true se la risorse può essere modificata
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CanEdit(object item);

        /// <summary>
        /// Restituisce true se la risorsa può essere eliminata
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CanDelete(object item);

        /// <summary>
        /// Restituisce vero se l'utente corrente può eseguire la configurazione del modulo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        bool CanConfigure();

        /// <summary>
        /// Restituisce la collezione delle colonne esportabili
        /// </summary>
        /// <returns></returns>
        CCollection<ExportableColumnInfo> GetExportableColumnList();
    }
}