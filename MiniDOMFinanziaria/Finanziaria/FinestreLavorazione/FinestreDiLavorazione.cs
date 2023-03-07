using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Finanziaria;
using static minidom.CustomerCalls;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="FinestraLavorazione"/>
        /// </summary>
        [Serializable]
        public class FinestreDiLavorazioneClass
            : CModulesClass<Finanziaria.FinestraLavorazione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public FinestreDiLavorazioneClass() 
                : base("modCQSPDWinLavorazione", typeof(Finanziaria.FinestraLavorazioneCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce la prossima finestra di lavorazione per il cliente specificato
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.FinestraLavorazione GetProssimaFinestra(Anagrafica.CPersonaFisica persona)
            {
                Finanziaria.FinestraLavorazione lw;
                if (persona is null)
                    throw new ArgumentNullException("persona");
                if (DBUtils.GetID(persona, 0) == 0)
                    return null;
                lw = GetFinestraCorrente(persona);
                if (lw is object)
                    return lw; // Non ci possono essere due finestra attive
                if (persona.Deceduto)
                    return null; // Non si può più lavorare un deceduto
                if (lw is null)
                {
                    lw = GetUltimaFinestraLavorata(persona);
                }

                var w = new Finanziaria.FinestraLavorazione();
                w.Cliente = persona;
                w.Stato = ObjectStatus.OBJECT_VALID;
                w.DataInizioLavorabilita = (DateTime)w.DataProssimaFinestra;
                w.SetFlag(Finanziaria.FinestraLavorazioneFlags.Rinnovo, w.AltriPrestiti.Count > 0);
                w.DataUltimoAggiornamento = DMD.DateUtils.Now();
                w.SetFlag(Finanziaria.FinestraLavorazioneFlags.Disponibile_CQS, HaCQS(persona, w.DataInizioLavorabilita));
                w.SetFlag(Finanziaria.FinestraLavorazioneFlags.Disponibile_PD, HaPD(persona, w.DataInizioLavorabilita));
                if (persona.ImpiegoPrincipale.StipendioNetto.HasValue)
                {
                    // If w.GetFlag(FinestraLavorazioneFlags.Disponibile_CQS) AndAlso w.GetFlag(FinestraLavorazioneFlags.Disponibile_PD) Then
                    // w.QuotaCedibile = 2 * persona.ImpiegoPrincipale.StipendioNetto.Value / 5
                    // Else
                    // w.QuotaCedibile = persona.ImpiegoPrincipale.StipendioNetto.Value / 5
                    // End If
                }

                w.Save();
                return w;
            }

            private DateTime? GetMinDate(CCollection<CContattoUtente> contatti)
            {
                DateTime? ret = default;
                foreach (var c in contatti)
                    ret = DMD.DateUtils.Min(ret, c.Data);
                return ret;
            }

            /// <summary>
            /// Ricostruisce le finestre
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public CCollection<Finanziaria.FinestraLavorazione> Ricostruisci(Anagrafica.CPersonaFisica p)
            {
                var finestre = minidom.Finanziaria.FinestreDiLavorazione.GetFinestreByPersona(p);
                finestre.Sort();
                var pratiche = minidom.Finanziaria.Pratiche.GetPraticheByPersona(p);
                pratiche.Sort();
                var studif = minidom.Finanziaria.Consulenze.GetConsulenzeByPersona(p);
                studif.Sort();
                var richiestef = minidom.Finanziaria.RichiesteFinanziamento.GetRichiesteByPersona(p);
                richiestef.Sort();
                var contatti = minidom.CustomerCalls.CRM.GetContattiByPersona(p);
                contatti.Sort();
                CCollection<Finanziaria.CEstinzione> prestiti = minidom.Finanziaria.Estinzioni.GetEstinzioniByPersona(p);
                Finanziaria.FinestraLavorazione w = null;
                if (finestre.Count == 0)
                {
                    w = new Finanziaria.FinestraLavorazione();
                    w.Cliente = p;
                    w.Stato = ObjectStatus.OBJECT_VALID;
                    finestre.Add(w);
                }

                int i = finestre.Count;
                var data = GetMinDate(contatti);
                if (data is null)
                    data = p.CreatoIl;
                data = DMD.DateUtils.GetDatePart(data);
                while (i > 0)
                {
                    i = 1;
                    w = finestre[i];
                    // w.AltriPrestiti

                }

                return finestre;
            }

            /// <summary>
            /// Restituisce la prossima finestra di lavorazione
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public Finanziaria.FinestraLavorazione GetProssimaFinestra(int id)
            {
                if (id == 0)
                    return null;
                Anagrafica.CPersonaFisica persona = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(id);
                return GetProssimaFinestra(persona);
            }

            /// <summary>
            /// Restituisce la finestra di lavorazione corrente
            /// </summary>
            /// <param name="pid"></param>
            /// <returns></returns>
            public Finanziaria.FinestraLavorazione GetFinestraCorrente(int pid)
            {
                if (pid == 0)
                    return null;

                FinestraLavorazione ret = null;

                using (var cursor = new Finanziaria.FinestraLavorazioneCursor())
                {
                    cursor.StatoFinestra.Value = Finanziaria.StatoFinestraLavorazione.Aperta;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = pid;
                    cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    ret = cursor.Item;
                }

                if (ret is null)
                {
                    using (var cursor = new Finanziaria.FinestraLavorazioneCursor())
                    {
                        cursor.StatoFinestra.Value = Finanziaria.StatoFinestraLavorazione.NonAperta;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDCliente.Value = pid;
                        cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_DESC;
                        cursor.IgnoreRights = true;
                        ret = cursor.Item;
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce la finestra di lavorazione corrente
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public Finanziaria.FinestraLavorazione GetFinestraCorrente(Anagrafica.CPersonaFisica persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                if (DBUtils.GetID(persona, 0) == 0)
                    return null;
                var ret = GetFinestraCorrente(DBUtils.GetID(persona, 0));
                if (ret is object)
                    ret.SetCliente(persona);
                return ret;
            }

            /// <summary>
            /// Aggiorna la finestra di lavorazione
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="w"></param>
            public void AggiornaFinestraLavorazione(
                                        Anagrafica.CPersonaFisica persona, 
                                        Finanziaria.FinestraLavorazione w
                                        )
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                if (w is null)
                    throw new ArgumentNullException("w");
                w.Cliente = persona;
                w.NomeCliente = persona.Nominativo;
                w.IconaCliente = persona.IconURL;
                w.Stato = ObjectStatus.OBJECT_VALID;
                w.DataInizioLavorabilita = (DateTime)CalcolaDataLavorabilita(persona);
                w.DataUltimoAggiornamento = DMD.DateUtils.Now();
                w.SetFlag(Finanziaria.FinestraLavorazioneFlags.Disponibile_CQS, HaCQS(persona, w.DataInizioLavorabilita));
                w.SetFlag(Finanziaria.FinestraLavorazioneFlags.Disponibile_PD, HaPD(persona, w.DataInizioLavorabilita));
                w.Save();
            }

            /// <summary>
            /// Restituisce la data di inizio lavorabilità per la persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? CalcolaDataLavorabilita(Anagrafica.CPersonaFisica persona)
            {
                DateTime? ret = default;
                var ultima = GetUltimaFinestraLavorata(persona);
                if (ultima is object)
                    ret = ultima.DataFineLavorazione;
                var prestiti = Finanziaria.Estinzioni.GetPrestitiAttivi(persona);
                DateTime? minPrestito = default;
                foreach (Finanziaria.CEstinzione p in prestiti)
                    minPrestito = DMD.DateUtils.Min(minPrestito, p.DataRinnovo);
                ret = DMD.DateUtils.Max(minPrestito, ret);
                if (ret.HasValue == false)
                    ret = DMD.DateUtils.ToDay();
                return ret;
            }

            /// <summary>
            /// Restituisce true se la persona può stipulare una CQS
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="al"></param>
            /// <returns></returns>
            public bool HaCQS(Anagrafica.CPersonaFisica persona, DateTime? al)
            {
                string tr = persona.ImpiegoPrincipale.TipoRapporto;
                return true;
                // If (tr = "") Then Return True
                // If (al.HasValue = False) Then al = Calendar.Now
                // For Each p As CCQSPDProdotto In Finanziaria.Prodotti.LoadAll
                // If p.Stato = ObjectStatus.OBJECT_VALID AndAlso p.IdTipoRapporto = tr AndAlso p.IdTipoContratto = "C" AndAlso p.IsValid(al) Then
                // Return True
                // End If
                // Next
                // Return False
            }

            /// <summary>
            /// Restituisce true se la persona può stipulare una PD
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="al"></param>
            /// <returns></returns>
            public bool HaPD(Anagrafica.CPersonaFisica persona, DateTime? al)
            {
                string tr = persona.ImpiegoPrincipale.TipoRapporto;
                return string.IsNullOrEmpty(tr) || tr != "H";
            }

            /// <summary>
            /// Restituisce la collezione delle finestre di lavorazione della persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public CCollection<Finanziaria.FinestraLavorazione> GetFinestreByPersona(Anagrafica.CPersona persona)
            {
                
                if (persona is null)
                    throw new ArgumentNullException("persona");
                var ret = new CCollection<Finanziaria.FinestraLavorazione>();
                if (DBUtils.GetID(persona, 0) == 0)
                    return ret;

                using (var cursor = new Finanziaria.FinestraLavorazioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    // cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_ASC
                    cursor.IDCliente.Value = DBUtils.GetID(persona);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        var fl = cursor.Item;
                        fl.SetCliente(persona);
                        ret.Add(fl);
                    }
                }

                ret.Sort();
                return ret;
                 
            }

            /// <summary>
            /// Restituisce la finestra di lavorazione attiva alla data specificata.
            /// La funzione non è utilizzabile per le date future (cerca solo tra le finestre già salvate)
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="allaData"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.FinestraLavorazione GetFinestra(Anagrafica.CPersona persona, DateTime allaData)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                if (DBUtils.GetID(persona, 0) == 0)
                    return null;

                using (var cursor = new Finanziaria.FinestraLavorazioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_ASC;
                    cursor.IDCliente.Value = DBUtils.GetID(persona);
                    cursor.IgnoreRights = true;
                    cursor.WhereClauses *=
                                      DBCursorField.Field("DataInizioLavorabilita").EQ(allaData) 
                                    * DBCursorField.Field("DataFineLavorazione").GE(allaData);
                    var fl = cursor.Item;
                    if (fl is object)
                        fl.SetCliente(persona);
                    return fl;
                }
            }

            /// <summary>
            /// Restituisce l'ultima finestra in stato chiuso
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.FinestraLavorazione GetUltimaFinestraLavorata(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                if (DBUtils.GetID(persona, 0) == 0)
                    return null;

                using (var cursor = new Finanziaria.FinestraLavorazioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = DBUtils.GetID(persona);
                    cursor.StatoFinestra.Value = Finanziaria.StatoFinestraLavorazione.Chiusa;
                    cursor.IgnoreRights = true;
                    cursor.DataFineLavorazione.SortOrder = SortEnum.SORT_DESC;
                    var fl = cursor.Item;
                    if (fl is object)
                        fl.SetCliente(persona);
                    return fl;
                }
            }

            
        }
    }

    public partial class Finanziaria
    {
        private static FinestreDiLavorazioneClass m_FinestreDiLavorazione = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="FinestraLavorazione"/>
        /// </summary>
        public static FinestreDiLavorazioneClass FinestreDiLavorazione
        {
            get
            {
                if (m_FinestreDiLavorazione is null)
                    m_FinestreDiLavorazione = new FinestreDiLavorazioneClass();
                return m_FinestreDiLavorazione;
            }
        }
    }
}