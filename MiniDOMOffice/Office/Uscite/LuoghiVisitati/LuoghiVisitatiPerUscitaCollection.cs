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
        /// <see cref="Luogo"/> visitato durante una <see cref="Uscita"/>
        /// </summary>
        [Serializable]
        public class LuoghiVisitatiPerUscitaCollection
            : CCollection<LuogoVisitato>
        {
            [NonSerialized] private Uscita m_Uscita;

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuoghiVisitatiPerUscitaCollection()
            {
                m_Uscita = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="uscita"></param>
            public LuoghiVisitatiPerUscitaCollection(Uscita uscita)
            {
                if (uscita is null)
                    throw new ArgumentNullException("uscita");
                Load(uscita);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, LuogoVisitato value)
            {
                if (m_Uscita is object)
                    value.SetUscita(m_Uscita);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, LuogoVisitato oldValue, LuogoVisitato newValue)
            {
                if (m_Uscita is object)
                    newValue.SetUscita(m_Uscita);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica
            /// </summary>
            /// <param name="uscita"></param>
            protected internal void Load(Uscita uscita)
            {
                if (uscita is null)
                    throw new ArgumentNullException("uscita");
                
                this.Clear();
                this.m_Uscita = uscita;
                
                if (DBUtils.GetID(uscita, 0) == 0)
                    return;

                using (var cursor = new LuoghiVisitatiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.OraArrivo.SortOrder = SortEnum.SORT_ASC;
                    cursor.IDUscita.Value = DBUtils.GetID(uscita, 0);
                    cursor.Progressivo.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                     
            }
        }
    }
}