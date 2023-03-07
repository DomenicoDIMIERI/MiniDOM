using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public sealed class CQSPDValutazioniAziendaModule : CModulesClass<Finanziaria.CQSPDValutazioneAzienda>
        {
            internal CQSPDValutazioniAziendaModule() : base("modCQSPDValutazioniAzienda", typeof(Finanziaria.CQSPDValutazioneAziendaCursor), 0)
            {
            }

            protected override Sistema.CModule CreateModuleInfo()
            {
                var m = base.CreateModuleInfo();
                m.DisplayName = "Valutazioni Aziende";
                m.ClassHandler = "ValutazioniAziendeHandler";
                m.Save();
                return m;
            }

            public Finanziaria.CQSPDValutazioneAzienda GetUltimaValutazioneAzienda(Anagrafica.CAzienda a)
            {
                if (a is null)
                    throw new ArgumentNullException("a");
                using (var cursor = new Finanziaria.CQSPDValutazioneAziendaCursor())
                {
                    cursor.IDAzienda.Value = DBUtils.GetID(a);
                    cursor.DataRevisione.SortOrder = SortEnum.SORT_DESC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    return cursor.Item;
                }
            }
        }
    }

    public partial class Finanziaria
    {
        private static CQSPDValutazioniAziendaModule m_ValutazioniAzienda = null;

        public static CQSPDValutazioniAziendaModule ValutazioniAzienda
        {
            get
            {
                if (m_ValutazioniAzienda is null)
                    m_ValutazioniAzienda = new CQSPDValutazioniAziendaModule();
                return m_ValutazioniAzienda;
            }
        }
    }
}