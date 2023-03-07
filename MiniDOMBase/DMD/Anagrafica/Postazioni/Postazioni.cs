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

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CPostazione"/>
        /// </summary>
        public class CPostazioniClass 
            : CModulesClass<Anagrafica.CPostazione>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CPostazioniClass() 
                : base("modPostazioni", typeof(Anagrafica.CPostazioniCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce la postazione in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPostazione GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Anagrafica.CPostazione p in LoadAll())
                {
                    if ((p.Nome ?? "") == (value ?? ""))
                        return p;
                }

                return null;
            }

            private CValoriRegistriClass m_ValoriRegistri = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="ValoreRegistroContatore"/>
            /// </summary>
            public CValoriRegistriClass ValoriRegistri
            {
                get
                {
                    if (m_ValoriRegistri is null)
                        m_ValoriRegistri = new CValoriRegistriClass();
                    return m_ValoriRegistri;
                }
            }
        }
    }

    public partial class Anagrafica
    {
        
        private static CPostazioniClass m_Postazioni = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CPostazione"/>
        /// </summary>
        public static CPostazioniClass Postazioni
        {
            get
            {
                if (m_Postazioni is null)
                    m_Postazioni = new CPostazioniClass();
                return m_Postazioni;
            }
        }
    }
}