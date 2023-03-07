using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Rappresenta una collezione di estinzioni
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CEstinzioniXEstintoreCollection : CCollection<EstinzioneXEstintore>
        {
            [NonSerialized]
            private object m_Estintore;

            public CEstinzioniXEstintoreCollection()
            {
                m_Estintore = null;
            }

            public CEstinzioniXEstintoreCollection(object estintore)
            {
                m_Estintore = estintore;
                Load();
            }

            public IEstintore Estintore
            {
                get
                {
                    return (IEstintore)m_Estintore;
                }
            }

            public EstinzioneXEstintore Add(CEstinzione es)
            {
                var item = new EstinzioneXEstintore();
                item.Estintore = (IEstintore)m_Estintore;
                item.Stato = ObjectStatus.OBJECT_VALID;
                item.Save();
                Add(item);
                return item;
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Estintore is object)
                    ((EstinzioneXEstintore)newValue).SetEstintore(m_Estintore);
                base.OnSet(index, oldValue, newValue);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Estintore is object)
                    ((EstinzioneXEstintore)value).SetEstintore(m_Estintore);
                base.OnInsert(index, value);
            }

            public void Load()
            {
                if (m_Estintore is null)
                    throw new ArgumentNullException("Estintore");
                Clear();
                if (DBUtils.GetID((Databases.IDBObjectBase)m_Estintore) == 0)
                    return;
                using (var cursor = new EstinzioneXEstintoreCursor())
                {
                    cursor.IDEstintore.Value = DBUtils.GetID((Databases.IDBObjectBase)m_Estintore);
                    cursor.TipoEstintore.Value = DMD.RunTime.vbTypeName(m_Estintore);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }
                }
            }

            protected internal virtual void SetEstintore(object value)
            {
                m_Estintore = value;
            }

            public void PreparaEstinzini()
            {
                PreparaEstinzini((DateTime)((IEstintore)m_Estintore).DataDecorrenza);
            }

            public void PreparaEstinzini(DateTime decorrenza)
            {
                IEstintore estintore = (IEstintore)m_Estintore;
                var persona = estintore.Cliente;
                if (persona is null)
                    throw new ArgumentNullException("cliente");
                CCollection<CEstinzione> altriPrestiti = Estinzioni.GetEstinzioniByPersona(persona);
                var items = this;
                for (int i = 0, loopTo = altriPrestiti.Count - 1; i <= loopTo; i++)
                {
                    bool trovato = false;
                    var est = altriPrestiti[i];
                    EstinzioneXEstintore item = null;
                    if (est.Stato == ObjectStatus.OBJECT_VALID && est.IsInCorso(decorrenza))
                    {
                        for (int j = 0, loopTo1 = items.Count - 1; j <= loopTo1; j++)
                        {
                            item = items[j];
                            trovato = item.IDEstinzione == DBUtils.GetID(est);
                            if (trovato)
                                break;
                        }
                    }

                    if (!trovato)
                    {
                        int resid = Sistema.Formats.ToInteger(est.NumeroRateResidue);
                        if (est.Scadenza.HasValue == false && est.DataInizio.HasValue && Sistema.Formats.ToInteger(est.Durata) > 0)
                        {
                            est.Scadenza = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd("M", est.Durata.Value, est.DataInizio));
                        }

                        if (est.Scadenza.HasValue)
                            resid = (int)Maths.Max(0L, DMD.DateUtils.DateDiff("M", decorrenza, est.Scadenza.Value) + 1L);
                        if (Sistema.Formats.ToInteger(est.Durata) > 0)
                            resid = (int)Maths.Min(resid, est.Durata);
                        item = new EstinzioneXEstintore();
                        item.Selezionata = false;
                        item.Estinzione = est;
                        item.Estintore = (IEstintore)m_Estintore;
                        item.Parametro = null;
                        item.Correzione = 0m;
                        item.NumeroQuoteInsolute = 0;
                        item.NumeroQuoteResidue = resid;
                        item.Stato = ObjectStatus.OBJECT_VALID;
                        item.DataEstinzione = decorrenza;
                        // item.DataCaricamento = dataCaricamento
                        item.AggiornaValori();
                        items.Add(item);
                        item.Save(true);
                    }
                }
            }

            public void Aggiorna()
            {
                var estintore = Estintore;
                string nomeCess = "";
                // Dim d As Date
                COffertaCQS o = null;
                var dcar = default(DateTime);
                var dest = default(DateTime);
                DateTime ddec;
                ddec = dcar;
                if (estintore is object)
                {
                    dcar = Estintore.DataCaricamento;
                    if (estintore.DataDecorrenza.HasValue)
                        ddec = Estintore.DataDecorrenza.Value;
                    if (estintore is CPraticaCQSPD)
                    {
                        o = ((CPraticaCQSPD)estintore).OffertaCorrente;
                    }
                    else if (estintore is CQSPDConsulenza)
                    {
                        CQSPDConsulenza cons = (CQSPDConsulenza)estintore;
                        if (cons.OffertaCQS is object && cons.OffertaCQS.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            o = cons.OffertaCQS;
                        }
                        else if (cons.OffertaPD is object && cons.OffertaPD.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            o = cons.OffertaPD;
                        }
                    }

                    if (o is object)
                    {
                        nomeCess = o.NomeCessionario;
                        dcar = (DateTime)o.DataCaricamento;
                        if (o.DataDecorrenza.HasValue)
                        {
                            ddec = (DateTime)o.DataDecorrenza;
                        }
                    }
                }

                foreach (EstinzioneXEstintore item in this)
                {
                    int resid = item.NumeroQuoteResidue;
                    bool IsInterno = DMD.Strings.Compare(nomeCess, item.NomeCessionario, true) == 0;
                    if (!item.DataFine.HasValue && item.DataDecorrenza.HasValue && Sistema.Formats.ToInteger(item.Durata) > 0)
                    {
                        item.DataFine = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd("M", item.Durata.Value, item.DataDecorrenza.Value));
                    }

                    item.DataCaricamento = dcar;
                    item.DataEstinzione = dest;
                    item.AggiornaCalcolo();
                    item.Save();
                }
            }
        }
    }
}