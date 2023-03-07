using DMD;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Rappresenta un'azione di una pagina o di un controllo all'interno di una pagina web
        /// </summary>
        /// <remarks></remarks>
        public class CAzioneProposta 
            : DMD.XML.DMDBaseXMLObject
        {
            private string m_Descrizione;
            private string m_Command;
            private bool m_IsScript;
            private CContattoUtente m_Contatto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAzioneProposta()
            {
                m_Descrizione = "";
                m_Command = "";
                m_IsScript = false;
                m_Contatto = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="c"></param>
            public CAzioneProposta(CContattoUtente c) : this()
            {
                Initialize(c);
            }
 

            /// <summary>
            /// Restituisce il contatto
            /// </summary>
            public CContattoUtente Contatto
            {
                get
                {
                    return m_Contatto;
                }

                set
                {
                    m_Contatto = value;
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione dell'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    m_Descrizione = value;
                }
            }

            /// <summary>
            /// Restituisce il comando
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public string GetCommand()
            {
                return m_Command;
            }

            /// <summary>
            /// Imposta il comando
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetCommand(string value)
            {
                m_Command = Strings.Trim(value);
            }

            /// <summary>
            /// Restitusice vero se l'azione è uno script
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsScript()
            {
                return m_IsScript;
            }

            /// <summary>
            /// Imposta IsScript
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetIsScript(bool value)
            {
                m_IsScript = value;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Descrizione;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Descrizione);
            }

            
            
            /// <summary>
            /// Inizializza
            /// </summary>
            /// <param name="c"></param>
            protected internal virtual void Initialize(CContattoUtente c)
            {
                m_Contatto = c;
            }
        }
    }
}