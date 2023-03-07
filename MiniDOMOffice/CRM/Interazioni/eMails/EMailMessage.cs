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
using static minidom.Office;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Email inviata o ricevuta
        /// </summary>
        [Serializable]
        public class CEMailMessage
            : CContattoUtente
        {


            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEMailMessage()
            {
                
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.EMailMessages;
            }

            /// <summary>
            /// Descrizione del'attività
            /// </summary>
            public override string DescrizioneAttivita
            {
                get
                {
                    string ret;
                    ret = "eMail " + ((Ricevuta)? "ricevuta da" : "a") + " " + NomePersona;
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

            ///// <summary>
            ///// Discriminator
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_Telefonate";
            //}
             
            /// <summary>
            /// Nome del contatto
            /// </summary>
            /// <returns></returns>
            public override string GetNomeTipoOggetto()
            {
                return "e-Mail";
            }

            

            

            /// <summary>
            /// Restituisce o imposta l'indirizzo del mittente
            /// </summary>
            /// <returns></returns>
            public string From
            {
                get
                {
                    return this.Parameters.GetItemByKey("email_from") as string ?? "";
                }

                set
                {
                    string oldValue = From;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    this.Parameters.SetItemByKey("email_from", value);
                    DoChanged("From", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elenco degli indirizzi dei destinatari separato dal ;
            /// </summary>
            /// <returns></returns>
            public string To
            {
                get
                {
                    return this.Parameters.GetItemByKey("email_to") as string ?? "";
                }

                set
                {
                    string oldValue = To;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    this.Parameters.SetItemByKey("email_to", value);
                    DoChanged("To", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elenco degli indirizzi dei destinatari in copia carbone separato dal ;
            /// </summary>
            /// <returns></returns>
            public string CC
            {
                get
                {
                    return this.Parameters.GetItemByKey("email_cc") as string ?? "";
                }

                set
                {
                    string oldValue = CC;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    this.Parameters.SetItemByKey("email_cc", value);
                    DoChanged("CC", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elenco degli indirizzi dei destinatari in copia carbone nascota separato dal ;
            /// </summary>
            /// <returns></returns>
            public string CCn
            {
                get
                {
                    return this.Parameters.GetItemByKey("email_ccn") as string ?? "";
                }

                set
                {
                    string oldValue = CCn;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    this.Parameters.SetItemByKey("email_ccn", value);
                    DoChanged("CCn", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto della mail
            /// </summary>
            /// <returns></returns>
            public string Subject
            {
                get
                {
                    return this.Parameters.GetItemByKey("email_subject") as string ?? "";
                }

                set
                {
                    string oldValue = Subject;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    this.Parameters.SetItemByKey("email_subject", value);
                    DoChanged("Subject", value, oldValue);
                }
            }

            
             
        }
    }
}