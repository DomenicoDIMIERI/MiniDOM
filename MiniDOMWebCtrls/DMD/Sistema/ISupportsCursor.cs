
namespace minidom.Forms
{

    /// <summary>
    /// Interfaccia implementata dai controlli che utilizzano un cursore
    /// </summary>
    /// <remarks></remarks>
    public interface ISupportsCursor
    {

        /// <summary>
        /// Restituisce o imposta il cursore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        Databases.DBObjectCursorBase Cursor { get; set; }
    }
}