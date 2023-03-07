using System.Diagnostics;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;

namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Handler per le spedizioni
        /// </summary>
        /// <remarks></remarks>
        public abstract class HandlerTipoCampagna
            : IHandlerTipoCampagna
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public HandlerTipoCampagna()
            {
                
            }

            /// <summary>
            /// Restituisce true se l'handler supporta la richiesta di conferma recapito
            /// </summary>
            /// <returns></returns>
            public abstract bool SupportaConfermaRecapito();

            /// <summary>
            /// Restituisce true se l'handler supporta la richiesta di conferma lettura
            /// </summary>
            /// <returns></returns>
            public abstract bool SupportaConfermaLettura();

            /// <summary>
            /// Restituisce il codice dell'handler
            /// </summary>
            /// <returns></returns>
            public abstract TipoCampagnaPubblicitaria GetHandledType();

            /// <summary>
            /// Restituisce una descrizione dell'handler
            /// </summary>
            /// <returns></returns>
            public abstract string GetNomeMezzoSpedizione();

            /// <summary>
            /// Invia il messaggio
            /// </summary>
            /// <param name="item"></param>
            public abstract void Send(CRisultatoCampagna item);

            /// <summary>
            /// Prepara una collezione di messaggio da inviare agli indirizzi della persona
            /// </summary>
            /// <param name="campagna"></param>
            /// <param name="item"></param>
            /// <returns></returns>
            public abstract CCollection<CRisultatoCampagna> PrepareResults(CCampagnaPubblicitaria campagna, Anagrafica.CPersona item);

            /// <summary>
            /// Restiuisce true se l'indirizzo é escluso 
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            protected virtual bool IsExcluded(CRisultatoCampagna res)
            {
                var items = Strings.Split(res.Campagna.ListaNO, DMD.Strings.vbNewLine);
                string nome, indirizzo;
                foreach (string value in items)
                {
                    nome = "";
                    indirizzo = "";
                    ParseAddress(value, ref nome, ref indirizzo);
                    if (DMD.Strings.Like(res.IndirizzoDestinatario, indirizzo))
                        return true;
                }

                return false;
            }

            /// <summary>
            /// Restituisce true se l'indirizzo è bannato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public abstract bool IsBanned(CRisultatoCampagna res);

            /// <summary>
            /// Restituisce true se l'indirizzo é bloccato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public abstract bool IsBlocked(CRisultatoCampagna res);

            /// <summary>
            /// Restituisce l'elenco degli indirizzi inclusi nella campagna
            /// </summary>
            /// <param name="campagna"></param>
            /// <returns></returns>
            public virtual CCollection<CRisultatoCampagna> GetListaInvio(CCampagnaPubblicitaria campagna)
            {
                // If (Me.UsaListaDinamica) Then
                CCollection<Anagrafica.CPersonaInfo> items;
                var ret = new CCollection<CRisultatoCampagna>();
                CCollection<CRisultatoCampagna> cc;
                CRisultatoCampagna res;
                string[] addresses;
                if (campagna.UsaListaDinamica)
                {
                    if (campagna.ParametriLista == "*")
                    {
                        var t = DMD.DateUtils.Now();

                        using (var cursor = new Anagrafica.CPersonaCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                var consenso = cursor.Item.GetFlag(Anagrafica.PFlags.CF_CONSENSOADV);
                                if (!consenso.HasValue || consenso.Value)
                                {
                                    cc = PrepareResults(campagna, cursor.Item);
                                    foreach (var currentRes in cc)
                                    {
                                        res = currentRes;
                                        res.Stato = ObjectStatus.OBJECT_VALID;
                                        res.Campagna = campagna;
                                        res.Destinatario = cursor.Item;
                                        res.TipoCampagna = campagna.TipoCampagna;
                                        res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                                        if (!IsExcluded(res))
                                            ret.Add(res);
                                    }
                                }
 
                            }

                        }

                        Debug.Print("Time: " + (DMD.DateUtils.Now() - t).TotalSeconds + " s");
                    }
                    else
                    {
                        var filter = new Anagrafica.CRMFindParams();
                        filter.Text = campagna.ParametriLista;
                        filter.ignoreRights = true;
                        filter.nMax = default;
                        items = Anagrafica.Persone.Find(filter);
                        foreach (Anagrafica.CPersonaInfo item in items)
                        {
                            bool? consenso = default;
                            if (item.Persona is object)
                                consenso = item.Persona.GetFlag(Anagrafica.PFlags.CF_CONSENSOADV);
                            if (!consenso.HasValue || consenso.Value)
                            {
                                cc = PrepareResults(campagna, item.Persona);
                                foreach (var currentRes1 in cc)
                                {
                                    res = currentRes1;
                                    res.Stato = ObjectStatus.OBJECT_VALID;
                                    res.Campagna = campagna;
                                    res.Destinatario = item.Persona;
                                    res.TipoCampagna = campagna.TipoCampagna;
                                    res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                                    if (!IsExcluded(res))
                                        ret.Add(res);
                                }
                            }
                        }
                    }
                }
                else
                {
                    addresses = Strings.Split(campagna.ParametriLista, ";");
                    foreach (var address in addresses)
                    {
                        res = new CRisultatoCampagna();
                        string argnome = res.NomeDestinatario;
                        string argaddress = res.IndirizzoDestinatario;
                        ParseAddress(address, ref argnome, ref argaddress);
                        res.NomeDestinatario = argnome;
                        res.IndirizzoDestinatario = argaddress;
                        bool localIsValidAddress() { 
                            string argaddress = res.IndirizzoDestinatario; 
                            var ret = IsValidAddress(argaddress); 
                            return ret; 
                        }

                        if (localIsValidAddress())
                        {
                            res.Stato = ObjectStatus.OBJECT_VALID;
                            res.Campagna = campagna;
                            res.TipoCampagna = campagna.TipoCampagna;
                            res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                            if (!IsExcluded(res))
                                ret.Add(res);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(campagna.ListaCC))
                {
                    addresses = Strings.Split(campagna.ListaCC, ";");
                    foreach (var address in addresses)
                    {
                        res = new CRisultatoCampagna();
                        string argnome1 = res.NomeDestinatario;
                        string argaddress1 = res.IndirizzoDestinatario;
                        ParseAddress(address, ref argnome1, ref argaddress1);
                        res.NomeDestinatario = argnome1;
                        res.IndirizzoDestinatario = argaddress1;
                        bool localIsValidAddress1() 
                        { 
                            string argaddress = res.IndirizzoDestinatario; 
                            var ret = IsValidAddress(argaddress); 
                            return ret; 
                        }

                        if (localIsValidAddress1())
                        {
                            res.Stato = ObjectStatus.OBJECT_VALID;
                            res.Campagna = campagna;
                            res.TipoCampagna = campagna.TipoCampagna;
                            res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                            if (!IsExcluded(res))
                                ret.Add(res);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(campagna.ListaCCN))
                {
                    addresses = Strings.Split(campagna.ListaCCN, ";");
                    foreach (var address in addresses)
                    {
                        res = new CRisultatoCampagna();
                        string argnome2 = res.NomeDestinatario;
                        string argaddress2 = res.IndirizzoDestinatario;
                        ParseAddress(address, ref argnome2, ref argaddress2);
                        res.NomeDestinatario = argnome2;
                        res.IndirizzoDestinatario = argaddress2;
                        bool localIsValidAddress2() 
                        { 
                            string argaddress = res.IndirizzoDestinatario; 
                            var ret = IsValidAddress(argaddress); 
                            return ret; 
                        }

                        if (localIsValidAddress2())
                        {
                            res.Stato = ObjectStatus.OBJECT_VALID;
                            res.Campagna = campagna;
                            res.TipoCampagna = campagna.TipoCampagna;
                            res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                            if (!IsExcluded(res))
                                ret.Add(res);
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Interpreta l'indirizzo
            /// </summary>
            /// <param name="str"></param>
            /// <param name="nome"></param>
            /// <param name="address"></param>
            public abstract void ParseAddress(
                                string str, 
                                ref string nome, 
                                ref string address
                                );

            /// <summary>
            /// Restituisce true se l'indirizzo è valido
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public abstract bool IsValidAddress(string address);

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return GetNomeMezzoSpedizione();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            /// <summary>
            /// Aggiorna lo stato del messaggio inviato
            /// </summary>
            /// <param name="res"></param>
            /// <remarks></remarks>
            public abstract void UpdateStatus(CRisultatoCampagna res);

            bool IHandlerTipoCampagna.IsExcluded(CRisultatoCampagna res)
            {
                return this.IsExcluded(res);
            }

             
        }
    }
}