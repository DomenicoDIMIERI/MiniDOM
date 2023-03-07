using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {

        /// <summary>
    /// Modulo per la gestione delle richieste deroghe
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CRichiesteDerogheClass : CModulesClass<Finanziaria.CRichiestaDeroga>
        {
            public event InviataEventHandler Inviata;

            public delegate void InviataEventHandler(object sender, ItemEventArgs e);

            public event RivutaEventHandler Rivuta;

            public delegate void RivutaEventHandler(object sender, ItemEventArgs e);

            public CRichiesteDerogheClass() : base("modCQSPDRichDeroghe", typeof(Finanziaria.CRichiestaConteggioCursor), 0)
            {
            }

            internal void doOnInviata(ItemEventArgs e)
            {
                Inviata?.Invoke(this, e);
                Module.DispatchEvent(new Sistema.EventDescription("inviata", "Richiesta Deroga Inviata", e.Item));
            }

            internal void doOnRicevuta(ItemEventArgs e)
            {
                Rivuta?.Invoke(this, e);
                Module.DispatchEvent(new Sistema.EventDescription("ricevuta", "Richiesta Deroga Ricevuta", e.Item));
            }

            protected internal new void doItemCreated(ItemEventArgs e)
            {
                base.doItemCreated(e);
            }

            protected internal new void doItemDeleted(ItemEventArgs e)
            {
                base.doItemDeleted(e);
            }

            protected internal new void doItemModified(ItemEventArgs e)
            {
                base.doItemModified(e);
            }
        }
    }

    public partial class Finanziaria
    {
        private static CRichiesteDerogheClass m_RichiesteDeroghe = null;

        /// <summary>
    /// Modulo per la gestione delle richieste di deroghe alle offerte standard
    /// </summary>
    /// <returns></returns>
        public static CRichiesteDerogheClass RichiesteDeroghe
        {
            get
            {
                if (m_RichiesteDeroghe is null)
                    m_RichiesteDeroghe = new CRichiesteDerogheClass();
                return m_RichiesteDeroghe;
            }
        }
    }
}