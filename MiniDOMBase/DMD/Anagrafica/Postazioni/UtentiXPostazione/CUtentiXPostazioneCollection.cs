using System;
using DMD.XML;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Collections;


namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Utenti abilitati per ogni postazione di lavoro
        /// </summary>
        [Serializable]
        public class CUtentiXPostazioneCollection 
            : CCollection<CUtenteXPostazione>
        {
            
            [NonSerialized] private CPostazione m_Postazione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUtentiXPostazioneCollection()
            {
                m_Postazione = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="postazione"></param>
            public CUtentiXPostazioneCollection(CPostazione postazione) : this()
            {
                if (postazione is null)
                    throw new ArgumentNullException("postazione");
                SetPostazione(postazione);
            }

            /// <summary>
            /// Restituisce un riferimento alla postazione di lavoro
            /// </summary>
            public CPostazione Postazione
            {
                get
                {
                    return m_Postazione;
                }
            }

            /// <summary>
            /// Imposta la postazione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPostazione(CPostazione value)
            {
                m_Postazione = value;
                if (value is object)
                {
                    foreach (CUtenteXPostazione item in this)
                        item.SetPostazione(value);
                }
            }

            // Protected Friend Sub Load(ByVal value As CUfficio)
            // If (value Is Nothing) Then Throw New ArgumentNullException("Ufficio")
            // MyBase.Clear()
            // Me.m_Ufficio = value
            // If (GetID(value) = 0) Then Return

            // For i As Integer = 0 To Anagrafica.Uffici.UfficiConsentiti.Count - 1
            // Dim item As CUtenteXUfficio = Anagrafica.Uffici.UfficiConsentiti(i)
            // If (item.IDUfficio = GetID(value)) AndAlso (item.Utente IsNot Nothing) AndAlso (item.Utente.Stato = ObjectStatus.OBJECT_VALID) Then
            // MyBase.Add(item.Utente)
            // End If
            // Next
            // End Sub


        }
    }
}