using DMD.Databases;
using DMD.Databases.Collections;
using System;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione dei consensi espressi dalla persona
        /// </summary>
        [Serializable]
        public class ConsensoInformatoColleciton 
            : CCollection<ConsensoInformato>
        {
            
            [NonSerialized]  private CPersona m_Persona;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ConsensoInformatoColleciton()
            {
                m_Persona = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="persona"></param>
            public ConsensoInformatoColleciton(CPersona persona) : this()
            {
                Load(persona);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, ConsensoInformato value)
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
            protected override void OnSet(int index, ConsensoInformato oldValue, ConsensoInformato newValue)
            {
                if (m_Persona is object)
                    newValue.SetPersona(m_Persona);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica i consensi espressi dalla persona
            /// </summary>
            /// <param name="persona"></param>
            protected internal void Load(CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                
                Clear();

                if (DBUtils.GetID(persona, 0) == 0)
                    return;

                using (var cursor = new ConsensoInformatoCursor())
                {
                    cursor.IDPersona.Value = DBUtils.GetID(persona, 0);
                    cursor.IgnoreRights = true;
                    cursor.DataConsenso.SortOrder = SortEnum.SORT_DESC;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                     
            }
        }
    }
}