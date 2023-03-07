using System;

namespace minidom
{

    /// <summary>
    /// Sistema di gestione delle campagne pubblicitarie automatizzate
    /// </summary>
    public sealed partial class ADV
    {
         
        private ADV()
        {
        }

        /// <summary>
        /// Inizalizza il sistema di invio automatico dei messaggi pubblicitari
        /// </summary>
        public static void Initialize()
        {
            var h = Campagne.GetHandler(TipoCampagnaPubblicitaria.eMail);
            Campagne.Initialize();
        }

        /// <summary>
        /// Ferma il sistema di invio automatico dei messaggi pubblicitari
        /// </summary>
        public static void Terminate()
        {
            Campagne.Terminate();
        }

        
    }
}