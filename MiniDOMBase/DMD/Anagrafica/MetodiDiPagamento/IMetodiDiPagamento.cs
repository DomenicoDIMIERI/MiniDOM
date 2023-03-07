
namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Interfaccia implementata dagli oggetti che consentono una movimentazione
        /// di denaro per pagare beni o servizi
        /// </summary>
        public interface IMetodoDiPagamento
        {


            /// <summary>
            /// Restituisce il nome del metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string NomeMetodo { get; }

            /// <summary>
            /// Restituisce il conto corrente associato al metodo di pagamento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            ContoCorrente ContoCorrente { get; set; }
        }
    }
}