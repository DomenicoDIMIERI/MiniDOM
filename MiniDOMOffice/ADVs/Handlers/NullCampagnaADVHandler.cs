
using DMD.Databases.Collections;

namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Gestore "fasullo" di una campagna adv
        /// </summary>
        public class NullCampagnaADVHandler 
            : HandlerTipoCampagna
        {

            /// <summary>
            /// Restituisce il tipo di campagna
            /// </summary>
            /// <returns></returns>
            public override TipoCampagnaPubblicitaria GetHandledType()
            {
                return TipoCampagnaPubblicitaria.NonImpostato;
            }

            /// <summary>
            /// Restituisce false
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBanned(CRisultatoCampagna res)
            {
                return false;
            }

            /// <summary>
            /// Restituisce false
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBlocked(CRisultatoCampagna res)
            {
                return false;
            }

            /// <summary>
            /// Restituisce il nome dell'handler
            /// </summary>
            /// <returns></returns>
            public override string GetNomeMezzoSpedizione()
            {
                return "";
            }

            /// <summary>
            /// Restituisce una collezione vuota
            /// </summary>
            /// <param name="campagna"></param>
            /// <param name="item"></param>
            /// <returns></returns>
            public override CCollection<CRisultatoCampagna> PrepareResults(CCampagnaPubblicitaria campagna, Anagrafica.CPersona item)
            {
                return new CCollection<CRisultatoCampagna>();
            }

            /// <summary>
            /// Non effettua alcuna operazione
            /// </summary>
            /// <param name="item"></param>
            public override void Send(CRisultatoCampagna item)
            {
            }

            /// <summary>
            /// Restituisce false
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaLettura()
            {
                return false;
            }

            /// <summary>
            /// Restituisce false
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaRecapito()
            {
                return false;
            }
            
            /// <summary>
            /// Aggiorna lo stato del messaggio
            /// </summary>
            /// <param name="res"></param>
            public override void UpdateStatus(CRisultatoCampagna res)
            {
            }

            /// <summary>
            /// Interpreta la stringa
            /// </summary>
            /// <param name="str"></param>
            /// <param name="nome"></param>
            /// <param name="address"></param>
            public override void ParseAddress(
                                    string str, 
                                    ref string nome, 
                                    ref string address
                                    )
            {
                address = str;
                nome = str;
            }

            /// <summary>
            /// Restituisce true
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public override bool IsValidAddress(string address)
            {
                return true;
            }
        }
    }
}