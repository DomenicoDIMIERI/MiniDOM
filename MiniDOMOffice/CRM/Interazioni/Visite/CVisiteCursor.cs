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
        /// Cursore sulla tabella delle visite
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CVisiteCursor 
            : CContattoUtenteCursor
        {

            // Private m_Indirizzo_Via As New DBCursorFieldObj(Of String)("Indirizzo_Via")
            // Private m_Indirizzo_Civico As New DBCursorFieldObj(Of String)("Indirizzo_Civico")
            // Private m_Indirizzo_CAP As New DBCursorFieldObj(Of String)("Indirizzo_CAP")
            // Private m_Indirizzo_Citta As New DBCursorFieldObj(Of String)("Indirizzo_Citta")
            // Private m_Indirizzo_Provincia As New DBCursorFieldObj(Of String)("Indirizzo_Provincia")
            // Private m_Indirizzo_Nome As New DBCursorFieldObj(Of String)("Indirizzo_Nome")

            /// <summary>
            /// Costruttore
            /// </summary>
            public CVisiteCursor()
            {
                base.ClassName.Value = "CVisita";
            }

            /// <summary>
            /// Nascondo
            /// </summary>
            private new object ClassName
            {
                get
                {
                    return null;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            public new CVisita Item
            {
                get
                {
                    return (CVisita)base.Item;
                }
                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge una riga
            /// </summary>
            /// <returns></returns>
            public new CVisita Add()
            {
                return (CVisita)base.Add();
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CContattoUtente InstantiateNewT(DBReader dbRis)
            {
                return new CVisita();
            }

            // Public ReadOnly Property Indirizzo_Via As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo_Via
            // End Get
            // End Property

            // Public ReadOnly Property Indirizzo_Civico As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo_Civico
            // End Get
            // End Property

            // Public ReadOnly Property Indirizzo_CAP As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo_CAP
            // End Get
            // End Property

            // Public ReadOnly Property Indirizzo_Citta As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo_Citta
            // End Get
            // End Property

            // Public ReadOnly Property Indirizzo_Provincia As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo_Provincia
            // End Get
            // End Property

            // Public ReadOnly Property Indirizzo_Nome As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo_Nome
            // End Get
            // End Property




        }
    }
}