using DMD;
using DMD.Databases.Collections;
using minidom.repositories;
using System;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Rappresenta un messaggio SMS
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class SMSMessage 
            : CContattoUtente
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public SMSMessage()
            {
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.SMS;
            }

            /// <summary>
            /// Descrizione attività
            /// </summary>
            public override string DescrizioneAttivita
            {
                get
                {
                    string ret;
                    ret = "SMS " + (Ricevuta? "ricevuto da" : "inviato a") + " " + NomePersona;
                    if (!string.IsNullOrEmpty(NumeroOIndirizzo))
                        ret += ", tel: " + Sistema.Formats.FormatPhoneNumber(NumeroOIndirizzo);
                    return ret;
                }
            }

            /// <summary>
            /// Azioni proposte
            /// </summary>
            public override CCollection<CAzioneProposta> AzioniProposte
            {
                get
                {
                    return new CCollection<CAzioneProposta>();
                }
            }

             
            /// <summary>
            /// Tipo oggetto
            /// </summary>
            /// <returns></returns>
            public override string GetNomeTipoOggetto()
            {
                return "SMS";
            }

            /// <summary>
            /// Numero della controparte
            /// </summary>
            public string Numero
            {
                get
                {
                    return NumeroOIndirizzo;
                }

                set
                {
                    NumeroOIndirizzo = value;
                }
            }

            /// <summary>
            /// Numero della controparte
            /// </summary>
            public override string NumeroOIndirizzo
            {
                get
                {
                    return base.NumeroOIndirizzo;
                }

                set
                {
                    base.NumeroOIndirizzo = Sistema.Formats.ParsePhoneNumber(value);
                }
            }

            
        }
    }
}