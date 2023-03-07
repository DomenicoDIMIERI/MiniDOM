using System.Diagnostics;
using DMD;
using DMD.Databases.Collections;
using DMD.XML;

namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Handler per le spedizioni
        /// </summary>
        /// <remarks></remarks>
        public interface IHandlerTipoCampagna
        {
            
            bool SupportaConfermaRecapito();
            bool SupportaConfermaLettura();
            TipoCampagnaPubblicitaria GetHandledType();
            string GetNomeMezzoSpedizione();
            
            void Send(CRisultatoCampagna item);

            CCollection<CRisultatoCampagna> PrepareResults(CCampagnaPubblicitaria campagna, Anagrafica.CPersona item);

            bool IsExcluded(CRisultatoCampagna res);
            
            bool IsBanned(CRisultatoCampagna res);
            bool IsBlocked(CRisultatoCampagna res);

            CCollection<CRisultatoCampagna> GetListaInvio(CCampagnaPubblicitaria campagna);

            void ParseAddress(string str, ref string nome, ref string address);
            
            bool IsValidAddress(string address);


            /// <summary>
            /// Aggiorna lo stato del messaggio inviato
            /// </summary>
            /// <param name="res"></param>
            /// <remarks></remarks>
            void UpdateStatus(CRisultatoCampagna res);
             
        }
    }
}