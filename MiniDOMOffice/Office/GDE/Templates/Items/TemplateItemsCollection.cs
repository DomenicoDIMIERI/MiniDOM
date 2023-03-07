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
        /// Collezione di elementi generati da un template
        /// </summary>
        [Serializable]
        public class TemplateItemsCollection
            : CCollection<TemplateItem>
        {
           
            [NonSerialized] private DocumentTemplate m_Document;

            /// <summary>
            /// Costruttore
            /// </summary>
            public TemplateItemsCollection()
            {
                m_Document = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="doc"></param>
            public TemplateItemsCollection(DocumentTemplate doc)
                : this()
            {
                if (doc is null)
                    throw new ArgumentNullException("document");
                m_Document = doc;
            }

            /// <summary>
            /// Imposta il template
            /// </summary>
            /// <param name="doc"></param>
            protected internal virtual void SetDocument(DocumentTemplate doc)
            {
                m_Document = doc;
            }

            /// <summary>
            /// Aggiunge un elemento nel template
            /// </summary>
            /// <param name="type"></param>
            /// <param name="text"></param>
            /// <param name="bounds"></param>
            /// <returns></returns>
            public TemplateItem Add(TemplateItemTypes type, string text, CRectangle bounds)
            {
                var ret = new TemplateItem(type, text, bounds);
                Add(ret);
                return ret;
            }

            /// <summary>
            /// Aggiunge un nuovo elemento al template
            /// </summary>
            /// <param name="type"></param>
            /// <param name="text"></param>
            /// <param name="bounds"></param>
            /// <param name="color"></param>
            /// <returns></returns>
            public TemplateItem Add(TemplateItemTypes type, string text, CRectangle bounds, string color)
            {
                var ret = new TemplateItem(type, text, bounds, color);
                Add(ret);
                return ret;
            }
        }
    }
}