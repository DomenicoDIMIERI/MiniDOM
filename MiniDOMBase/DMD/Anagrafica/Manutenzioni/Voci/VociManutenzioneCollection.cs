using DMD.Databases;
using DMD.Databases.Collections;
using System;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Voci di manutenzione
        /// </summary>
        [Serializable]
        public class VociManutenzioneCollection 
            : CCollection<VoceManutenzione>
        {
            [NonSerialized] private CManutenzione m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public VociManutenzioneCollection()
            {
                m_Owner = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public VociManutenzioneCollection(CManutenzione owner) : this()
            {
                Load(owner);
            }

            /// <summary>
            /// Possessore
            /// </summary>
            public CManutenzione Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta il possessore
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetOwner(CManutenzione value)
            {
                m_Owner = value;
                if (value is object)
                {
                    foreach (VoceManutenzione v in this)
                        v.SetManutenzione(value);
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, VoceManutenzione oldValue, VoceManutenzione newValue)
            {
                if (m_Owner is object)
                    newValue.SetManutenzione(m_Owner);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, VoceManutenzione value)
            {
                if (m_Owner is object)
                    value.SetManutenzione(m_Owner);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// Carica le voci
            /// </summary>
            /// <param name="value"></param>
            protected internal void Load(CManutenzione value)
            {
                if (value is null)
                    throw new ArgumentNullException("owner");
                
                this.Clear();

                if (DBUtils.GetID(value, 0) == 0)
                    return;

                using (var cursor = new VociManutenzioneCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDManutenzione.Value = DBUtils.GetID(value, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        this.Add(cursor.Item);
                    }
                }
                 
            }
        }
    }
}