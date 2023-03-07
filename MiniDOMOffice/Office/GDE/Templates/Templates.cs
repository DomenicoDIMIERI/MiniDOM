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
        /// Repository di oggetti <see cref="DocumentTemplate"/>
        /// </summary>
        [Serializable]
        public class CGDETemplatesClass 
            : CModulesClass<DocumentTemplate>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGDETemplatesClass() 
                : base("modGDETemplates", typeof(minidom.Office.DocumentTemplateCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public DocumentTemplate GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                var items = LoadAll();
                foreach (minidom.Office.DocumentTemplate item in items)
                {
                    if (item.Stato == ObjectStatus.OBJECT_VALID && DMD.Strings.Compare(item.Name, name, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce la collezione dei template definiti per il contesto
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public CCollection<DocumentTemplate> GetItemsByContext(string context)
            {
                var ret = new CCollection<DocumentTemplate>();
                context = DMD.Strings.Trim(context);
                var items = LoadAll();
                foreach (var item in items)
                {
                    if (   
                           item.Stato == ObjectStatus.OBJECT_VALID 
                        && DMD.Strings.EQ(item.ContextType, context, true)
                        )
                        ret.Add(item);
                }

                return ret;
            }
        }
    }

    public partial class Office
    {
        private static CGDETemplatesClass m_Templates = null;

        /// <summary>
        /// Repository di oggetto <see cref="DocumentTemplate"/>
        /// </summary>
        public static CGDETemplatesClass Templates
        {
            get
            {
                if (m_Templates is null)
                    m_Templates = new CGDETemplatesClass();
                return m_Templates;
            }
        }
    }
}