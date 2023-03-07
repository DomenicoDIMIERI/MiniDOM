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
    public partial class Office
    {

        /// <summary>
        /// Collezione di luoghi da visitare per un percorso
        /// </summary>
        [Serializable]
        public class LuoghiDefinitiPerPercorso
            : CCollection<LuogoDaVisitare>
        {

            [NonSerialized] private PercorsoDefinito m_Percorso;

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuoghiDefinitiPerPercorso()
            {
                m_Percorso = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="percorso"></param>
            public LuoghiDefinitiPerPercorso(PercorsoDefinito percorso) 
                : this()
            {
                Load(percorso);
            }

            /// <summary>
            /// Restituisce un riferimento al percorso
            /// </summary>
            public PercorsoDefinito Percorso
            {
                get
                {
                    return m_Percorso;
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, LuogoDaVisitare value)
            {
                if (m_Percorso is object)
                    value.SetPercorso(m_Percorso);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, LuogoDaVisitare oldValue, LuogoDaVisitare newValue)
            {
                if (m_Percorso is object)
                    newValue.SetPercorso(m_Percorso);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="percorso"></param>
            protected internal virtual void Load(PercorsoDefinito percorso)
            {
                if (percorso is null)
                    throw new ArgumentNullException("percorso");
                Clear();
                
                m_Percorso = percorso;

                if (DBUtils.GetID(percorso, 0) == 0)
                    return;

                using (var cursor = new LuoghiDaVisitareCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.IDPercorso.Value = DBUtils.GetID(percorso, 0);
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                     
                Sort();
                 
            }

            /// <summary>
            /// Imposta il percorso
            /// </summary>
            /// <param name="p"></param>
            protected internal void SetPercorso(PercorsoDefinito p)
            {
                m_Percorso = p;
                foreach (LuogoDaVisitare Item in this)
                    Item.SetPercorso(p);
            }
        }
    }
}