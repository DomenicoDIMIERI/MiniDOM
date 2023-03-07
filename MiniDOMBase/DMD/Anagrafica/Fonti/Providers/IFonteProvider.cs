
namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Interfaccia implementata dagli oggetto che fungono da generatori di fonti
        /// </summary>
        /// <remarks></remarks>
        public interface IFonteProvider
        {

            /// <summary>
            /// Restituisce un array contenente i nomi delle fonti gestiti da questo oggetto
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            string[] GetSupportedNames();

            /// <summary>
            /// Restituisce un array delle fonti corrispondenti al nome specificato
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            IFonte[] GetItemsAsArray(string tipo, bool onlyValid = true);

                /// <summary>
            /// Restituisce la fonte in base all'ID
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            IFonte GetItemById(string tipo, int id);

            /// <summary>
            /// Restituisce la fonte in base al nome
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="nome"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            IFonte GetItemByName(string tipo, string nome);
        }
    }
}