using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="StatoTaskLavorazione"/>
        /// </summary>
        public sealed class CStatiTasksDiLavorazioneClass 
            : CModulesClass<Anagrafica.StatoTaskLavorazione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CStatiTasksDiLavorazioneClass() 
                : base("modAnaStatoTaskLavorazione", typeof(Anagrafica.StatoTaskLavorazioneCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce il task attivo in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.StatoTaskLavorazione GetItemByName(string name)
            {
                var items = LoadAll();
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (Anagrafica.StatoTaskLavorazione stato in items)
                {
                    if (DMD.Strings.Compare(stato.Nome, name, true) == 0)
                        return stato;
                }

                return null;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CStatiTasksDiLavorazioneClass m_StatiTasksLavorazione = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="StatoTaskLavorazione"/>
        /// </summary>
        public static CStatiTasksDiLavorazioneClass StatiTasksLavorazione
        {
            get
            {
                if (m_StatiTasksLavorazione is null)
                    m_StatiTasksLavorazione = new CStatiTasksDiLavorazioneClass();
                return m_StatiTasksLavorazione;
            }
        }
    }
}