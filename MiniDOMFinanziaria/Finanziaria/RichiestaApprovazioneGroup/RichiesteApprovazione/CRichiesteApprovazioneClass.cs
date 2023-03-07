using System;
using DMD;

namespace minidom
{
    namespace CQSPDInternals
    {
        public class CRichiesteApprovazioneClass : CModulesClass<Finanziaria.CRichiestaApprovazione>
        {
            public CRichiesteApprovazioneClass() : base("modCQSPDRichiesteApprovazione", typeof(Finanziaria.CRichiestaApprovazioneCursor), 0)
            {
            }

            /// <summary>
        /// Restituisce tutte le richieste di approvazione generate per l'oggetto specificate oridinate per Data della richiesta crescente
        /// </summary>
        /// <param name="oggetto"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CRichiestaApprovazione> GetRichiesteByOggetto(object oggetto)
            {
                if (oggetto is null)
                    throw new ArgumentNullException("oggetto");
                var ret = new CCollection<Finanziaria.CRichiestaApprovazione>();
                var cursor = new Finanziaria.CRichiestaApprovazioneCursor();
                // cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.TipoOggettoApprovabile.Value = DMD.RunTime.vbTypeName(oggetto);
                cursor.IDOggettoApprovabile.Value = DBUtils.GetID((Databases.IDBObjectBase)oggetto);
                cursor.DataRichiestaApprovazione.SortOrder = SortEnum.SORT_ASC;
                while (!cursor.EOF())
                {
                    ret.Add(cursor.Item);
                    cursor.MoveNext();
                }

                cursor.Dispose();
                return ret;
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
        private static CQSPDInternals.CRichiesteApprovazioneClass m_RichiesteApprovazione;

        public static CQSPDInternals.CRichiesteApprovazioneClass RichiesteApprovazione
        {
            get
            {
                if (m_RichiesteApprovazione is null)
                    m_RichiesteApprovazione = new CQSPDInternals.CRichiesteApprovazioneClass();
                return m_RichiesteApprovazione;
            }
        }
    }
}