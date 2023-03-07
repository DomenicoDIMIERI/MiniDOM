using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class COfferteClass : CModulesClass<Finanziaria.COffertaCQS>
        {
            internal COfferteClass() : base("modOfferteCQS", typeof(Finanziaria.CCQSPDOfferteCursor))
            {
            }

            public CCollection<Finanziaria.COffertaCQS> GetOfferteByPersona(Anagrafica.CPersona persona)
            {
                return GetOfferteByPersona(DBUtils.GetID(persona));
            }

            public CCollection<Finanziaria.COffertaCQS> GetOfferteByPersona(int idPersona)
            {
                var cursor = new Finanziaria.CCQSPDOfferteCursor();
                var ret = new CCollection<Finanziaria.COffertaCQS>();
                if (idPersona != 0)
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = idPersona;
                    cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }

                return ret;
            }

            public CCollection<Finanziaria.COffertaCQS> GetOfferteByPratica(Finanziaria.CPraticaCQSPD pratica)
            {
                return GetOfferteByPratica(DBUtils.GetID(pratica));
            }

            public CCollection<Finanziaria.COffertaCQS> GetOfferteByPratica(int idPratica)
            {
                var cursor = new Finanziaria.CCQSPDOfferteCursor();
                var ret = new CCollection<Finanziaria.COffertaCQS>();
                if (idPratica != 0)
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPratica.Value = idPratica;
                    cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }

                return ret;
            }

            public string FormatStatoOfferta(Finanziaria.StatoOfferta value)
            {
                var values = new Finanziaria.StatoOfferta[] { Finanziaria.StatoOfferta.NON_ASSOCIATO, Finanziaria.StatoOfferta.PROPOSTA, Finanziaria.StatoOfferta.RIFIUTATA_CLIENTE, Finanziaria.StatoOfferta.ACCETTATA_CLIENTE, Finanziaria.StatoOfferta.RICHIESTA_APPROVAZIONE, Finanziaria.StatoOfferta.APPROVATA, Finanziaria.StatoOfferta.RIFIUTATA };
                var names = new string[] { "Temporanea", "Proposta", "Rifiutata dal cliente", "Accettada dal cliente", "Richiesta Approvazione", "Approvata", "Rifiutata" };
                int i = DMD.Arrays.IndexOf(values, value);
                return names[i];
            }
        }
    }

    public partial class Finanziaria
    {
        private static COfferteClass m_Offerte = null;

        public static COfferteClass Offerte
        {
            get
            {
                if (m_Offerte is null)
                    m_Offerte = new COfferteClass();
                return m_Offerte;
            }
        }
    }
}