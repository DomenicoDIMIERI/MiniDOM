using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Rappresenta un Fax inviato o ricevuto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class FaxDocument 
            : CContattoUtente
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public FaxDocument()
            {
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.FAX;
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public override string DescrizioneAttivita
            {
                get
                {
                    string ret;
                    ret = "FAX " + (Ricevuta?  "ricevuto da" : "inviato a") + " " + NomePersona;
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
            /// Nome tipo oggetto
            /// </summary>
            /// <returns></returns>
            public override string GetNomeTipoOggetto()
            {
                return "FAX";
            }

            /// <summary>
            /// Numero
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
            /// Numero
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