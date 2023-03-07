using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        // .---------------------------------------------

        /// <summary>
    /// Classe che consente di accedere agli stati pratica
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public sealed class CStatiPraticaClass : CModulesClass<Finanziaria.CStatoPratica>
        {
            internal CStatiPraticaClass() : base("modCQSPDStatPrat", typeof(Finanziaria.CStatoPraticaCursor), -1)
            {
            }

            public string FormatMacroStato(Finanziaria.StatoPraticaEnum? stato)
            {
                return Finanziaria.Pratiche.FormatStatoPratica(stato);
            }


            /// <summary>
        /// Restituisce lo stato pratica iniziale (per una nuova pratica)
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CStatoPratica GetDefault()
            {
                return Finanziaria.Configuration.StatoPredefinito;
            }

            public Finanziaria.CStatoPratRulesCollection GetStatiSuccessivi(Finanziaria.CStatoPratica statoAttuale)
            {
                return statoAttuale.StatiSuccessivi;
            }

            public CCollection<Finanziaria.CStatoPratRule> GetStatiSuccessivi(Finanziaria.CPraticaCQSPD pratica)
            {
                var ret = new CCollection<Finanziaria.CStatoPratRule>();
                var ra = pratica.RichiestaApprovazione;
                if (ra is object && ra.StatoRichiesta != Finanziaria.StatoRichiestaApprovazione.APPROVATA)
                {
                    var stAnnullata = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ANNULLATA);
                    foreach (var rule in pratica.StatoAttuale.StatiSuccessivi)
                    {
                        if (rule.IDTarget == DBUtils.GetID(stAnnullata))
                            ret.Add(rule);
                    }
                }
                else if (pratica.StatoAttuale is object)
                {
                    if (pratica.StatoAttuale.StatiSuccessivi.Count > 0)
                        ret.AddRange(pratica.StatoAttuale.StatiSuccessivi.ToArray());
                }

                return ret;
            }

            /// <summary>
        /// Restituisce un oggetto CCollection contenente tutti gli stati attivi
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CStatoPratica> GetStatiAttivi()
            {
                var ret = new CCollection<Finanziaria.CStatoPratica>();
                foreach (Finanziaria.CStatoPratica item in LoadAll())
                {
                    if (item.Attivo)
                        ret.Add(item);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce il valore corrispondente al vecchio sistema secondo il campo OldStatus di compatibilità
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CStatoPratica GetItemByCompatibleID(Finanziaria.StatoPraticaEnum ms)
            {
                foreach (Finanziaria.CStatoPratica item in LoadAll())
                {
                    if (item.MacroStato.HasValue && item.MacroStato.Value == ms)
                        return item;
                }

                return null;
            }

            public Finanziaria.CStatoPratica GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (Finanziaria.CStatoPratica item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, name, true) == 0)
                        return item;
                }

                return null;
            }

            public CCollection<Finanziaria.CStatoPratica> GetSequenzaStandard()
            {
                var stato = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO);
                var items = new CCollection<Finanziaria.CStatoPratica>();
                while (stato is object)
                {
                    items.Add(stato);
                    stato = stato.DefaultTarget;
                }

                stato = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ANNULLATA);
                if (stato is object)
                    items.Add(stato);
                return items;
            }

            public Finanziaria.CStatoPratica StatoContrattoFirmato
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_FIRMATO);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_FIRMATO;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public Finanziaria.CStatoPratica StatoContrattoStampato
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_STAMPATO);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_STAMPATO;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public Finanziaria.CStatoPratica StatoRichiestaDelibera
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_RICHIESTADELIBERA);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_RICHIESTADELIBERA;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public Finanziaria.CStatoPratica StatoLiquidato
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_LIQUIDATA);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_LIQUIDATA;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public Finanziaria.CStatoPratica StatoPraticaCaricata
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_PRATICA_CARICATA);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_PRATICA_CARICATA;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public Finanziaria.CStatoPratica StatoAnnullato
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ANNULLATA);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_ANNULLATA;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public Finanziaria.CStatoPratica StatoArchiviato
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            /// <summary>
        /// Restituisce lo stato preventivo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CStatoPratica StatoPreventivo
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            /// <summary>
        /// Restituisce lo stato preventivo accettato
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CStatoPratica StatoPreventivoAccettato
            {
                get
                {
                    var ret = GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO);
                    if (ret is null)
                    {
                        ret = new Finanziaria.CStatoPratica();
                        ret.MacroStato = Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO;
                        ret.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Nome = FormatMacroStato(ret.MacroStato);
                        ret.Save();
                    }

                    return ret;
                }
            }

            public override void Initialize()
            {
                base.Initialize();
                var stati = LoadAll();
                foreach (Finanziaria.StatoPraticaEnum ms in Enum.GetValues(typeof(Finanziaria.StatoPraticaEnum)))
                {
                    Finanziaria.CStatoPratica s = null;
                    foreach (Finanziaria.CStatoPratica s1 in stati)
                    {
                        if (s1.MacroStato.HasValue && s1.MacroStato.Value == ms)
                        {
                            s = s1;
                            break;
                        }
                    }

                    if (s is null)
                    {
                        s = new Finanziaria.CStatoPratica();
                        s.MacroStato = ms;
                        s.Nome = FormatMacroStato(ms);
                        s.Descrizione = FormatMacroStato(ms);
                        s.Stato = ObjectStatus.OBJECT_VALID;
                        s.Save();
                    }
                }
            }
        }
    }

    public partial class Finanziaria
    {
        private static CStatiPraticaClass m_StatiPratica = null;

        public static CStatiPraticaClass StatiPratica
        {
            get
            {
                if (m_StatiPratica is null)
                    m_StatiPratica = new CStatiPraticaClass();
                return m_StatiPratica;
            }
        }
    }
}