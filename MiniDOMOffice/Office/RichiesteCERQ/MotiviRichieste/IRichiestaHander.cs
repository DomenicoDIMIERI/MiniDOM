
namespace minidom
{
    public partial class Office
    {


        /// <summary>
        /// Cursore sulla tabella dei motivi delle richieste
        /// </summary>
        /// <remarks></remarks>
        public interface IRichiestaHandler
        {

            /// <summary>
            /// Convalida la richiesta
            /// </summary>
            /// <param name="richiesta"></param>
            /// <param name="motivo"></param>
            /// <param name="toStato"></param>
            void validate(RichiestaCERQ richiesta, MotivoRichiesta motivo, StatoRichiestaCERQ toStato);
            
            /// <summary>
            /// Esegue la richiesta
            /// </summary>
            /// <param name="richiesta"></param>
            /// <param name="motivo"></param>
            /// <param name="toStato"></param>
            void execute(RichiestaCERQ richiesta, MotivoRichiesta motivo, StatoRichiestaCERQ toStato);
        }
    }
}