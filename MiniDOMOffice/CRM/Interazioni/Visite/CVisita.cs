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
        /// Rappresenta una visita effettuata o ricevuta
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CVisita 
            : CContattoUtente
        {
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CVisita()
            {
            }

            /// <summary>
            /// Nome del tipo di oggetti
            /// </summary>
            /// <returns></returns>
            public override string GetNomeTipoOggetto()
            {
                return "Visita";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.Visite;
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public override string DescrizioneAttivita
            {
                get
                {
                    string ret;
                    ret = "Visita " + (Ricevuta? "ricevuta da" : "effettuata a ") + " " + NomePersona;
                    ret += ", presso: ";
                    switch (Luogo.Nome ?? "")
                    {
                        case "Residenza":
                        case "Domicilio":
                        case "Sede di impiego":
                            {
                                ret += " " + Luogo.Nome;
                                break;
                            }

                        default:
                            {
                                if (Strings.Left(Luogo.Nome, Strings.Len("Ufficio di ")) == "Ufficio di ")
                                {
                                    ret += " " + Luogo.Nome;
                                }
                                else
                                {
                                    ret += Luogo.ToString();
                                }

                                break;
                            }
                    }

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
                    // Dim azione As CAzioneProposta
                    // ret = MyBase.AzioniProposte
                    // If (Me.Ricontattare) And (Me.DataRicontatto <= Now()) Then
                    // azione = New CAzioneRicontatto
                    // Call azione.Initialize(Me)
                    // ret.Add(azione)
                    // Else
                    // azione = New CAzioneChiama
                    // Call azione.Initialize(Me)
                    // ret.Add(azione)
                    // End If
                    // azione = New CAzioneCreaPratica
                    // Call azione.Initialize(Me)
                    // ret.Add(azione)
                    // AzioniProposte = ret
                }
            }
             

            //public override string GetTableName()
            //{
            //    return "tbl_Telefonate";
            //}

             
              
        }
    }
}