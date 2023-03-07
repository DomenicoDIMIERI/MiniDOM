using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Anagrafica;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di indirizzi registrati per una persona
        /// </summary>
        [Serializable]
        public class CIndirizziCollection 
            : CKeyCollection<CIndirizzo>
        {
            [NonSerialized] private CPersona m_Persona;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIndirizziCollection()
            {
                m_Persona = null;
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CIndirizzo value)
            {
                if (m_Persona is object)
                    value.SetPersona(this.m_Persona);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CIndirizzo oldValue, CIndirizzo newValue)
            {
                if (m_Persona is object)
                    newValue.SetPersona(this.m_Persona);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Aggiunge
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public CIndirizzo Add(string nome)
            {
                var item = new CIndirizzo();
                item.Nome = nome;
                item.Stato = ObjectStatus.OBJECT_VALID;
                this.Add(nome, item);
                return item;
            }

            // Public Function GetItemByKey(ByVal value As String) As CIndirizzo
            // Dim i As Integer = Me.IndexOfKey(value)
            // If (i < 0) Then
            // Return Nothing
            // Else
            // Return MyBase.Item(i)
            // End If
            // End Function


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            protected internal void LoadPersona(CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");

                this.Clear();

                this.m_Persona = persona;

                using (var cursor = new CIndirizziCursor())
                {
                    cursor.PersonaID.Value = DBUtils.GetID(persona, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Nome.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        this.Add(cursor.Item.Nome, cursor.Item);
                    }
                }
                 
            }
        }
    }
}