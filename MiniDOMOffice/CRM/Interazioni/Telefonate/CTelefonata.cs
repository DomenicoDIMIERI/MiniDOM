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
        /// Telefonata
        /// </summary>
        [Serializable]
        public class CTelefonata 
            : CContattoUtente
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelefonata()
            {
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.Telefonate;
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public override string DescrizioneAttivita
            {
                get
                {
                    string ret;
                    ret = "Telefonata " + (Ricevuta? "ricevuta da" : "a") + " " + NomePersona;
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

            // Protected Overrides Sub XMLSerialize(ByVal writer As minidom.XML.XMLWriter)
            // MyBase.XMLSerialize(writer)
            // 'writer.WriteTag("Numero", Me.m_Numero)
            // End Sub

            // Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            // Select Case fieldName
            // Case "Numero" : m_Numero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            // Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            // End Select
            // End Sub

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Telefonate";
            }

             
            /// <summary>
            /// Tipo oggetto
            /// </summary>
            /// <returns></returns>
            public override string GetNomeTipoOggetto()
            {
                return "Telefonata";
            }

           
             
        }
    }
}