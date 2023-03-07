using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;


namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Regole di derivazione 
        /// </summary>
        [Serializable]
        public sealed class RegoleTaskLavorazionePerStato 
            : CCollection<RegolaTaskLavorazione>
        {
            
            [NonSerialized] private StatoTaskLavorazione m_Stato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegoleTaskLavorazionePerStato()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="stato"></param>
            public RegoleTaskLavorazionePerStato(StatoTaskLavorazione stato) : this()
            {
                if (stato is null)
                    throw new ArgumentNullException("stato");
                Load(stato);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, RegolaTaskLavorazione value)
            {
                if (m_Stato is object)
                {
                    var withBlock = value;
                    withBlock.SetStatoSorgente(m_Stato);
                    withBlock.SetOrdine(index);
                }

                base.OnInsert(index, value);
            }

            protected override void OnSetComplete(int index, RegolaTaskLavorazione oldValue, RegolaTaskLavorazione newValue)
            {
                if (m_Stato is object)
                {
                    var withBlock = newValue;
                    withBlock.SetStatoSorgente(m_Stato);
                    withBlock.SetOrdine(index);
                }

                base.OnSetComplete(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica le regole
            /// </summary>
            /// <param name="stato"></param>
            internal void Load(StatoTaskLavorazione stato)
            {
                Clear();
                m_Stato = stato;
                if (DBUtils.GetID(stato, 0) == 0)
                    return;
                 
                lock (RegoleTasksLavorazione)
                {
                    foreach (RegolaTaskLavorazione regola in RegoleTasksLavorazione.LoadAll())
                    {
                        if (regola.IDStatoSorgente == DBUtils.GetID(stato, 0))
                        {
                            Add(regola);
                        }
                    }
                }

                lock (this)
                    Sort();
            }
        }
    }
}