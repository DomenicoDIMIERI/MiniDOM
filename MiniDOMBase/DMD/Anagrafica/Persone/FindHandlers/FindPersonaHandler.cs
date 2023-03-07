using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Oggetto base da cui sono implementati i filtri installabili per la ricerca delle anagrafiche
        /// </summary>
        public abstract class FindPersonaHandler 
            : IComparable, IComparable<FindPersonaHandler>
        {

            /// <summary>
            /// Evento generato quando l'handler subisce una modifica
            /// </summary>
            public event EventHandler Changed;

             

            private int m_Priority;

            /// <summary>
            /// Costruttore
            /// </summary>
            public FindPersonaHandler()
            {
                ////DMDObject.IncreaseCounter(this);
                m_Priority = 0;
            }

            /// <summary>
            /// Restituisce o imposta la priorietà dei risultati
            /// </summary>
            public int Prioriy
            {
                get
                {
                    return m_Priority;
                }

                set
                {
                    if (m_Priority == value)
                        return;
                    m_Priority = value;
                    OnChanged(new EventArgs());
                }
            }

            /// <summary>
            /// Restituisce il nome del comando elaborato da questo gestore
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract string GetHandledCommand();

            /// <summary>
            /// Se vero indica al sistema che il gestore è in grado di elaborare il filtro
            /// </summary>
            /// <param name="param"></param>
            /// <param name="filter"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract bool CanHandle(string param, CRMFindParams filter);

            /// <summary>
            /// Elabora il filtro
            /// </summary>
            /// <param name="param"></param>
            /// <param name="filter"></param>
            /// <param name="ret"></param>
            /// <remarks></remarks>
            public abstract void Find(string param, CRMFindParams filter, CCollection<CPersonaInfo> ret);

            /// <summary>
            /// Genera l'evento Changed
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnChanged(EventArgs e)
            {
                Changed?.Invoke(this, e);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(FindPersonaHandler obj)
            {
                return DMD.Arrays.Compare(this.m_Priority, obj.m_Priority);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((FindPersonaHandler)obj);
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~FindPersonaHandler()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}