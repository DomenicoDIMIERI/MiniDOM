
namespace minidom
{
    public partial class Finanziaria
    {
        public interface IOggettoApprovabile
        {
            /// <summary>
        /// Evento generato quando vine formulata un'offerta che richiede l'approvazione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            event RequireApprovationEventHandler RequireApprovation;

            delegate void RequireApprovationEventHandler(object sender, ItemEventArgs e);


            /// <summary>
        /// Evento generato quando l'offerta corrente viene approvata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            event ApprovataEventHandler Approvata;

            delegate void ApprovataEventHandler(object sender, ItemEventArgs e);

            /// <summary>
        /// Evento generato quando l'offerta viene rifiutata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            event RifiutataEventHandler Rifiutata;

            delegate void RifiutataEventHandler(object sender, ItemEventArgs e);

            /// <summary>
        /// Evento generato quando l'offerta viene rifiutata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            event PresaInCaricoEventHandler PresaInCarico;

            delegate void PresaInCaricoEventHandler(object sender, ItemEventArgs e);

            /// <summary>
        /// Restitusice o imposta l'ID della richiesta di approvazione corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            int IDRichiestaApprovazione { get; set; }

            /// <summary>
        /// Restituisce la richiesta ai approvazione corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            CRichiestaApprovazione RichiestaApprovazione { get; set; }

            /// <summary>
        /// Genera una richiesta di approvazione
        /// </summary>
        /// <param name="motivo"></param>
        /// <param name="dettaglio"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            CRichiestaApprovazione RichiediApprovazione(string motivo, string dettaglio, string parametri);

            /// <summary>
        /// Approva la richiesta corrente
        /// </summary>
        /// <param name="motivo"></param>
        /// <param name="dettaglio"></param>
        /// <remarks></remarks>
            CRichiestaApprovazione Approva(string motivo, string dettaglio);


            /// <summary>
        /// Nega la richiesta corrente
        /// </summary>
        /// <param name="motivo"></param>
        /// <param name="dettaglio"></param>
        /// <remarks></remarks>
            CRichiestaApprovazione Nega(string motivo, string dettaglio);

            /// <summary>
        /// Prende in carico la richiesta
        /// </summary>
        /// <remarks></remarks>
            CRichiestaApprovazione PrendiInCarico();
        }
    }
}