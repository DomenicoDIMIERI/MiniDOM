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
using static minidom.Finanziaria;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Collezione ordinata per data degli stati di lavorazione di una pratica
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CStatiLavorazionePraticaCollection
            : CCollection<CStatoLavorazionePratica>
        {
            [NonSerialized] private CPraticaCQSPD m_Pratica;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CStatiLavorazionePraticaCollection()
            {
                m_Pratica = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="pratica"></param>
            public CStatiLavorazionePraticaCollection(CPraticaCQSPD pratica)
                : this()
            {
                Load(pratica);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CStatoLavorazionePratica value)
            {
                if (m_Pratica is object)
                    value.SetPratica(m_Pratica);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CStatoLavorazionePratica oldValue, CStatoLavorazionePratica newValue)
            {
                if (m_Pratica is object)
                    newValue.SetPratica(m_Pratica);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dalla collection
            /// </summary>
            /// <param name="pratica"></param>
            protected internal void Load(CPraticaCQSPD pratica)
            {
                if (pratica is null)
                    throw new ArgumentException("pratica");
                
                this.Clear();
                
                this.m_Pratica = pratica;
                if (DBUtils.GetID(pratica, 0) == 0)
                    return;

                using (var cursor = new CStatiLavorazionePraticaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Data.SortOrder = SortEnum.SORT_ASC;
                    cursor.ID.SortOrder = SortEnum.SORT_ASC;
                    cursor.IDPratica.Value = DBUtils.GetID(pratica);
                    cursor.IgnoreRights = true;
                    cursor.ID.SortPriority = 99;
                    cursor.Data.SortPriority = 100;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                     

                Sort();
            }

            /// <summary>
            /// Restituisce l'elemento in base al macostrato
            /// </summary>
            /// <param name="ms"></param>
            /// <returns></returns>
            public CStatoLavorazionePratica GetItemByMacroStato(StatoPraticaEnum ms)
            {
                foreach(var c in this) 
                {
                    if (c.MacroStato.HasValue && c.MacroStato.Value == ms)
                        return c;
                } 
                return null;
            }

            /// <summary>
            /// Imposta l'oggetto a cui appartiene la collection
            /// </summary>
            /// <param name="p"></param>
            protected internal void SetPratica(CPraticaCQSPD p)
            {
                this.m_Pratica = p;
                foreach (var c in this)
                {
                    c.SetPratica(p);
                }
            }
        }
    }
}