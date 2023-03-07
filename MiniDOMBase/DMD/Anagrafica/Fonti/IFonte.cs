
namespace minidom
{

    /// <summary>
    /// Interfaccia implementata dagli oggetti utilizzabili come fonti di un contatto, di una anagrafica, di una pratica ecc
    /// </summary>
    /// <remarks></remarks>
    public interface IFonte
    {

        /// <summary>
        /// Nome della fonte
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        string Nome { get; }

        /// <summary>
        /// Percorso dell'icona associata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        string IconURL { get; }
    }

    
}