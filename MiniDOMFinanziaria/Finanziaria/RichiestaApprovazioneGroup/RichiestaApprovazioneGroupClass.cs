using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public class RichiestaApprovazioneGroupClass : CModulesClass<Finanziaria.RichiestaApprovazioneGroup>
        {
            public RichiestaApprovazioneGroupClass() : base("modCQSPDRichApprGrp", typeof(Finanziaria.RichiestaApprovazioneGroupCursor), 0)
            {
            }

            public Finanziaria.RichiestaApprovazioneGroup GetRichiestaByPersona(Anagrafica.CPersonaFisica p)
            {
                Finanziaria.RichiestaApprovazioneGroupCursor cursor = null;
                Finanziaria.RichiestaApprovazioneGroup ret = null;
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (p is null)
                    throw new ArgumentNullException("p");
                cursor = new Finanziaria.RichiestaApprovazioneGroupCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
                cursor.IDCliente.Value = DBUtils.GetID(p);
                cursor.DataEsito.Value = default;
                cursor.DataEsito.IncludeNulls = true;
                ret = cursor.Item;
                if (ret is object)
                    ret.SetCliente(p);

                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                return ret;
            }

            internal void doOnRichiedi(ItemEventArgs<Finanziaria.RichiestaApprovazioneGroup> e)
            {
                Module.DispatchEvent(new Sistema.EventDescription("richiedi", "Richiesta Approvazione", e.Item));
            }

            internal void doOnApprova(ItemEventArgs<Finanziaria.RichiestaApprovazioneGroup> e)
            {
                Module.DispatchEvent(new Sistema.EventDescription("approva", "Approvazione Concessa", e.Item));
            }

            internal void doOnRifiuta(ItemEventArgs<Finanziaria.RichiestaApprovazioneGroup> e)
            {
                Module.DispatchEvent(new Sistema.EventDescription("rifiuta", "Approvazione Negata", e.Item));
            }
        }
    }

    public partial class Finanziaria
    {
        private static RichiestaApprovazioneGroupClass m_RichiesteApprovazioneGroups = null;

        public static RichiestaApprovazioneGroupClass RichiesteApprovazioneGroups
        {
            get
            {
                if (m_RichiesteApprovazioneGroups is null)
                    m_RichiesteApprovazioneGroups = new RichiestaApprovazioneGroupClass();
                return m_RichiesteApprovazioneGroups;
            }
        }
    }
}