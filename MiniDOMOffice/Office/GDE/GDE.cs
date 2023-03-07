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
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="DocumentoCaricato"/>
        /// </summary>
        [Serializable]
        public sealed class CGDECLass 
            : CModulesClass<DocumentoCaricato>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CGDECLass() 
                : base("modGDE", typeof(DocumentiCaricatiCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce il documento in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public minidom.Office.DocumentoCaricato GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (minidom.Office.DocumentoCaricato d in LoadAll())
                {
                    if (DMD.Strings.Compare(d.Nome, name, true) == 0)
                        return d;
                }

                return null;
            }
        }
    }

    public partial class Office
    {
        private static CGDECLass m_GDE = null;

        /// <summary>
        /// Repository di <see cref="DocumentoCaricato"/>
        /// </summary>
        public static CGDECLass GDE
        {
            get
            {
                if (m_GDE is null)
                    m_GDE = new CGDECLass();
                return m_GDE;
            }
        }
    }
}