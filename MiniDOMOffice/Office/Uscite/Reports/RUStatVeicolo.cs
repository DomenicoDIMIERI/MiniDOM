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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Statistiche sulle uscite fatte usando uno specifico veicolo
        /// </summary>
        [Serializable]
        public class RUStatVeicolo 
            : IComparable, IComparable<RUStatVeicolo>
        {
            /// <summary>
            /// Veicolo
            /// </summary>
            public Veicolo Veicolo;

            /// <summary>
            /// Uscite fatte con il veicolo
            /// </summary>
            public CCollection<Uscita> Uscite;

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="veicolo"></param>
            public RUStatVeicolo(Veicolo veicolo)
            {
                Veicolo = veicolo;
                Uscite = new CCollection<Uscita>();
            }

            /// <summary>
            /// Nome del veicolo
            /// </summary>
            public string NomeVeicolo
            {
                get
                {
                    if (Veicolo is null)
                        return "";
                    return Veicolo.Nome;
                }
            }

            /// <summary>
            /// Distanza percorsa in tutte le commissioni
            /// </summary>
            public double? DistanzaPercorsa
            {
                get
                {
                    double? ret = default;
                    foreach (Uscita u in Uscite)
                    {
                        if (u.DistanzaPercorsa.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                ret = ret.Value + u.DistanzaPercorsa.Value;
                            }
                            else
                            {
                                ret = u.DistanzaPercorsa;
                            }
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Durata totale di utilizzo del veicolo
            /// </summary>
            public long? Durata
            {
                get
                {
                    long? ret = default;
                    foreach (Uscita u in Uscite)
                    {
                        if (u.Durata.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                ret = ret.Value + u.Durata.Value;
                            }
                            else
                            {
                                ret = u.Durata;
                            }
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Consumo di carburante totale
            /// </summary>
            public double? LitriCarburante
            {
                get
                {
                    double? ret = default;
                    foreach (Uscita u in Uscite)
                    {
                        if (u.LitriCarburante.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                ret = ret.Value + u.LitriCarburante.Value;
                            }
                            else
                            {
                                ret = u.LitriCarburante;
                            }
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Rimborso totale ottenuto per il veicolo
            /// </summary>
            public decimal? Rimborso
            {
                get
                {
                    long? ret = default;
                    foreach (Uscita u in Uscite)
                    {
                        if (u.Rimborso.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                ret = (long?)(ret.Value + u.Rimborso.Value);
                            }
                            else
                            {
                                ret = (long?)u.Rimborso;
                            }
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Compara due oggetti in base al nome del veicolo
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            int CompareTo(RUStatVeicolo withBlock)
            {
                return DMD.Strings.Compare(NomeVeicolo, withBlock.NomeVeicolo, true);
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((RUStatVeicolo)obj); }
             
        }
    }
}