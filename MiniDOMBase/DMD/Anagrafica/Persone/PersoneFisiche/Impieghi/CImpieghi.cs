using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Collezione di impieghi di una persona
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CImpieghi 
            : CCollection<CImpiegato>
        {
            [NonSerialized] private CPersonaFisica m_Persona;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CImpieghi()
            {
                m_Persona = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="persona"></param>
            public CImpieghi(CPersonaFisica persona) : this()
            {
                Initialize(persona);
            }

            /// <summary>
            /// Restituisce un riferimento alla persona 
            /// </summary>
            public CPersonaFisica Persona
            {
                get
                {
                    return m_Persona;
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CImpiegato value)
            {
                if (m_Persona is object)
                    value.SetPersona(m_Persona);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CImpiegato oldValue, CImpiegato newValue)
            {
                if (m_Persona is object)
                    newValue.SetPersona(m_Persona);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Imposta la persona a cui appartiene la collezione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(CPersonaFisica value)
            {
                m_Persona = value;
                foreach (CImpiegato impiego in this)
                    impiego.Persona = value;
            }

            /// <summary>
            /// Aggiunge un nuovo impiego vuoto
            /// </summary>
            /// <returns></returns>
            public CImpiegato Add()
            {
                var item = new CImpiegato();
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                return item;
            }

            /// <summary>
            /// Aggiunge un nuovo impiego
            /// </summary>
            /// <param name="azienda"></param>
            /// <param name="ufficio"></param>
            /// <param name="posizione"></param>
            /// <returns></returns>
            public CImpiegato AddImpiego(CAzienda azienda, string ufficio, string posizione)
            {
                var item = new CImpiegato();
                if (azienda is null)
                    throw new ArgumentNullException("azienda");
                item.Azienda = azienda;
                item.Ufficio = ufficio;
                item.Posizione = posizione;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                return item;
            }

            /// <summary>
            /// Carica gli impieghi
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            protected internal bool Initialize(CPersonaFisica persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                Clear();
                m_Persona = persona;
                if (DBUtils.GetID(persona, 0) == 0)
                    return true;
                using (var cursor = new CImpiegatiCursor())
                { 
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.PersonaID.Value = DBUtils.GetID(persona, 0);
                    cursor.ID.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                     
                
                return true;
            }
        }
    }
}