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
        /// rappresenta le telefonate fatte ad una persona
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTelefonatePersona 
            : CCollection<CTelefonata>
        {
            
            [NonSerialized] private Anagrafica.CPersona m_Persona;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelefonatePersona()
            {
                m_Persona = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="persona"></param>
            public CTelefonatePersona(CPersona persona) : this()
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");

                Load(persona);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CTelefonata value)
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
            protected override void OnSet(int index, CTelefonata oldValue, CTelefonata newValue)
            {
                if (this.m_Persona is object)
                    newValue.SetPersona(this.m_Persona);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="persona"></param>
            protected internal void Load(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("Persona");
                
                this.Clear();

                this.m_Persona = persona;

                if (DBUtils.GetID(persona, 0) == 0)
                    return;

                using (var cursor = new CContattoUtenteCursor())
                {
                    cursor.IDPersona.Value = DBUtils.GetID(persona, 0);
                    cursor.Data.SortOrder = SortEnum.SORT_DESC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ClassName.Value = "CTelefonata";
                    cursor.ClassName.Operator = OP.OP_LIKE;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add((CTelefonata)cursor.Item);
                    }
                }
                     
            }
        }
    }
}