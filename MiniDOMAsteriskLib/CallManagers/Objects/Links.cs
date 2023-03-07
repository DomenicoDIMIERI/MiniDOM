using System.Collections;
using System.Collections.Generic;
using DMD.XML;

namespace minidom.CallManagers
{
    public class Links 
        : IEnumerable<Link> // (Of Link)
    {
        private List<Link> col = new List<Link>();
        private AsteriskCallManager m_Owner = null;

        /// <summary>
        /// Costruttore
        /// </summary>
        public Links()
        {
            DMDObject.IncreaseCounter(this);
        }

       
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="owner"></param>
        public Links(AsteriskCallManager owner) : this()
        {
            this.m_Owner = owner;
        }

        /// <summary>
        /// Restituisce un riferimento al manager
        /// </summary>
        public AsteriskCallManager Owner
        {
            get
            {
                return this.m_Owner;
            }
        }

        /// <summary>
        /// Restituisce il numero di elementi nella lista
        /// </summary>
        public int Count
        {
            get
            {
                return this.col.Count;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al canale con indice base 0
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Link this[int index]
        {
            get
            {
                return this.col[index];
            }
        }

        // Protected Overrides Sub OnInsert(index As Integer, value As Object)
        // If (Me.m_Owner IsNot Nothing) Then DirectCast(value, Link).SetOwner(Me.m_Owner)
        // MyBase.OnInsert(index, value)
        // End Sub

        /// <summary>
        /// Aggiorna lo stato del canale sulla base dell'evento del manager
        /// </summary>
        /// <param name="e"></param>
        protected internal void Update(AsteriskEvent e)
        {
            lock (this.m_Owner)
            {
                // Dim link As Link
                if (e is Events.Link)
                {
                    Events.Link e1 = (Events.Link)e;
                }
                // link = New Link
                // link.Channel1 = Me.Owner.Channels(e1.Channel1)
                // link.Channel2 = Me.Owner.Channels(e1.Channel2)
                // Me.Add(link)
                // Me.Owner.NotifyLink(link)
                else if (e is Events.Unlink)
                {
                    Events.Unlink e1 = (Events.Unlink)e;
                    int j = -1;
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        if (
                                this[i].Channel1.Channel == e1.Channel1  
                            &&  this[i].Channel2.Channel == e1.Channel2
                            )
                        {
                            j = i;
                            break;
                        }
                    }

                    if (j >= 0)
                       this.col.RemoveAt(j);
                }
            }
        }

        /// <summary>
        /// Restituisce un oggetto IEnumerator sulla lista
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Link> GetEnumerator()
        {
            return this.col.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator(); 
        }

        /// <summary>
        /// Distruttore
        /// </summary>
        ~Links()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}