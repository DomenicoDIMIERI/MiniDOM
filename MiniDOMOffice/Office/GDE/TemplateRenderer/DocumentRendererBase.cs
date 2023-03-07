using System;
using System.Drawing;
using DMD;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Renderer dei documenti generabili
        /// </summary>
        public abstract class DocumentRendererBase
        {
            /// <summary>
            /// Documento sorgente
            /// </summary>
            private DocumentoCaricato m_Document;

            /// <summary>
            /// Template del documento
            /// </summary>
            private DocumentTemplate m_Template;

            /// <summary>
            /// Contesto in cui viene generato il documento
            /// </summary>
            private object m_Context;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentRendererBase()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="document">[in] Tipo di documento da generare</param>
            /// <param name="template">[in] Template da usare</param>
            /// <param name="context">[in] Contesto in cui generare il documento</param>
            public DocumentRendererBase(DocumentoCaricato document, DocumentTemplate template, object context) : this()
            {
                m_Document = document;
                m_Template = template;
                m_Context = context;
            }

            /// <summary>
            /// Tipo del documento da generare
            /// </summary>
            public DocumentoCaricato Document
            {
                get
                {
                    return m_Document;
                }
            }

            /// <summary>
            /// Template da usare per generare il documento
            /// </summary>
            public DocumentTemplate DocumentTemplate
            {
                get
                {
                    return m_Template;
                }
            }

            /// <summary>
            /// Contesto in cui generare il documento
            /// </summary>
            public object Context
            {
                get
                {
                    return m_Context;
                }
            }

            /// <summary>
            /// Genera il documento
            /// </summary>
            public abstract void Render();

            /// <summary>
            /// Genera il documento e lo salva nel file
            /// </summary>
            /// <param name="fileName"></param>
            public abstract void SaveToFile(string fileName);

            /// <summary>
            /// Carica il documento generato come un'immagine
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public virtual Image LoadImage(string path)
            {
                path = Strings.Trim(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(path);
                return new Bitmap(path);
            }

            /// <summary>
            /// Restituisce il valore del campo
            /// </summary>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            public virtual object GetDataField(string fieldName)
            {
                //TODO migliorare GetDataField
                fieldName = Strings.Trim(fieldName);
                if (string.IsNullOrEmpty(fieldName))
                    throw new ArgumentNullException(fieldName);
                var t = Context.GetType();
                var m = t.GetProperty(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                return m.GetValue(Context, new object[] { });
            }

            /// <summary>
            /// Valuta l'espressione
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public virtual object EvaluateExpression(string expression)
            {
                //TODO EvaluateExpression
                throw new NotImplementedException();
            }
        }
    }
}