
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Interfaccia implementata dagli oggetti su cui è possibile effettuare dei controlli amministrativi
    /// </summary>
    /// <remarks></remarks>
        public interface IOggettoVerificabile
        {

            /// <summary>
        /// Restituisce o imposta l'ultima verifica amministrativa effettuata sull'oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            VerificaAmministrativa UltimaVerifica { get; set; }
        }
    }
}