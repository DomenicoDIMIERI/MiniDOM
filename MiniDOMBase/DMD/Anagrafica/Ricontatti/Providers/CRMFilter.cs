using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flag per il filtro sui ricontatti
        /// </summary>
        [Flags]
        public enum CRMFilterFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            NONE = 0,
            // APPUNTAMENTI = 1
            // TELEFONATE = 2

            /// <summary>
            /// Restituisce solo le azienda
            /// </summary>
            SOLOAZIENDE = 4,

            /// <summary>
            /// Restituisce solo i clienti attivi
            /// </summary>
            SOLOINCORSO = 8
        }

        /// <summary>
        /// Tipi di ordinamento dei risultati
        /// </summary>
        public enum CRMFilterSortFlags : int
        {
            /// <summary>
            /// Mostra prima i più importanti
            /// </summary>
            MOSTIMPORTANT = 0,

            /// <summary>
            /// Mostra prima i più recenti
            /// </summary>
            MOSTRECENT = 1,

            /// <summary>
            /// Mostra prima i meno recenti
            /// </summary>
            LEASTRECENT = 2,

            /// <summary>
            /// Ordina per nome del cliente
            /// </summary>
            NAME = 3
        }

        /// <summary>
        /// Filtro di ricerca
        /// </summary>
        public class CRMFilter 
            : IDMDXMLSerializable, ICloneable, IComparer , IComparer<CActivePerson>
        {

            /// <summary>
            /// Tipi di appuntamento da restituire
            /// </summary>
            public CCollection<string> TipiAppuntamento = new CCollection<string>();

            /// <summary>
            /// Categorie di appuntamento da restituire
            /// </summary>
            public CCollection<string> Categorie = new CCollection<string>();

            /// <summary>
            /// Tipi rapporto da restituire
            /// </summary>
            public CCollection<string> TipiRapporto = new CCollection<string>();

            /// <summary>
            /// Flags
            /// </summary>
            public CRMFilterFlags Flags = CRMFilterFlags.NONE; // CRMFilterFlags.APPUNTAMENTI Or CRMFilterFlags.TELEFONATE

            /// <summary>
            /// Motivo del ricontatto
            /// </summary>
            public string Motivo = "";

            /// <summary>
            /// Nome della lista
            /// </summary>
            public string NomeLista = "";

            /// <summary>
            /// Id del punto operativo di assegnazione
            /// </summary>
            public int IDPuntoOperativo = 0;

            /// <summary>
            /// Tipo periodo (es oggi, domani, tra)
            /// </summary>
            public string Periodo = "";

            /// <summary>
            /// Data inizio
            /// </summary>
            public DateTime? DataInizio = default;

            /// <summary>
            /// Data fine
            /// </summary>
            public DateTime? DataFine = default;

            /// <summary>
            /// Di operatore di assegnazione
            /// </summary>
            public int IDOperatore = 0;

            /// <summary>
            /// Ordinamento dei risultati
            /// </summary>
            public CRMFilterSortFlags SortOrder = CRMFilterSortFlags.MOSTIMPORTANT;

            /// <summary>
            /// Numero massimo dei risultati da restituire
            /// </summary>
            public int? nMax = default;

            /// <summary>
            /// Inizia a restituire dalla data
            /// </summary>
            public DateTime? fromDate = default;

            /// <summary>
            /// Indirizzo di residenza
            /// </summary>
            public string ResidenteA = "";

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMFilter()
            {
                ////DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Mostra solo le persone giuridiche
            /// </summary>
            public bool MostraSoloAziende
            {
                get
                {
                    return DMD.RunTime.TestFlag(Flags, CRMFilterFlags.SOLOAZIENDE);
                }

                set
                {
                    Flags = DMD.RunTime.SetFlag(Flags, CRMFilterFlags.SOLOAZIENDE, value);
                }
            }

            /// <summary>
            /// Mostra le telefonate
            /// </summary>
            public bool MostraTelefonate
            {
                get
                {
                    return TipiAppuntamento.Count == 0 || DMD.Booleans.ValueOf(TipiAppuntamento.Contains("Telefonata"));
                }

                set
                {
                    if (value)
                    {
                        if (!TipiAppuntamento.Contains("Telefonata"))
                            TipiAppuntamento.Add("Telefonata");
                    }
                    else if (DMD.Booleans.ValueOf(TipiAppuntamento.Contains("Telefonata")))
                        TipiAppuntamento.Remove("Telefonata");
                }
            }

            /// <summary>
            /// Mostra gli appuntamenti
            /// </summary>
            public bool MostraAppuntamenti
            {
                get
                {
                    return TipiAppuntamento.Count == 0 || DMD.Booleans.ValueOf(TipiAppuntamento.Contains("Appuntamento"));
                }

                set
                {
                    if (value)
                    {
                        if (!TipiAppuntamento.Contains("Appuntamento"))
                            TipiAppuntamento.Add("Appuntamento");
                    }
                    else if (DMD.Booleans.ValueOf(TipiAppuntamento.Contains("Appuntamento")))
                        TipiAppuntamento.Remove("Appuntamento");
                }
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Motivo, this.NomeLista, this.Periodo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(object obj)
            {
                return (obj is CRMFilter) && this.Equals((CRMFilter)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRMFilter obj )
            {
                return
                       CollectionUtils.EQAnyOrder(this.TipiAppuntamento, obj.TipiAppuntamento)
                    && CollectionUtils.EQAnyOrder(this.Categorie, obj.Categorie)
                    && CollectionUtils.EQAnyOrder(this.TipiRapporto, obj.TipiRapporto)
                    && DMD.Integers.EQ((int)this.Flags, (int)obj.Flags)
                    && DMD.Strings.EQ(this.Motivo, obj.Motivo)
                    && DMD.Strings.EQ(this.NomeLista, obj.NomeLista)
                    && DMD.Integers.EQ(this.IDPuntoOperativo, obj.IDPuntoOperativo)
                    && DMD.Strings.EQ(this.Periodo, obj.Periodo)
                    && DMD.DateUtils.EQ(this.DataInizio, obj.DataInizio)
                    && DMD.DateUtils.EQ(this.DataFine, obj.DataFine)
                    && DMD.Integers.EQ(this.IDOperatore, obj.IDOperatore)
                    && DMD.Integers.EQ((int)this.SortOrder, (int)obj.SortOrder)
                    && DMD.Integers.EQ(this.nMax, obj.nMax)
                    && DMD.DateUtils.EQ(this.fromDate, obj.fromDate)
                    && DMD.Strings.EQ(this.ResidenteA, obj.ResidenteA)
             ;

        }

        /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la ricerca deve restituire o meno i promemoria
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MostraPromemoria
            {
                get
                {
                    return TipiAppuntamento.Count == 0 || DMD.Booleans.ValueOf(TipiAppuntamento.Contains("Promemoria"));
                }

                set
                {
                    if (value)
                    {
                        if (!TipiAppuntamento.Contains("Promemoria"))
                            TipiAppuntamento.Add("Promemoria");
                    }
                    else if (DMD.Booleans.ValueOf(TipiAppuntamento.Contains("Promemoria")))
                        TipiAppuntamento.Remove("Promemoria");
                }
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "TipiAppuntamento":
                        {
                            TipiAppuntamento.Clear();
                            TipiAppuntamento.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Categorie":
                        {
                            Categorie.Clear();
                            Categorie.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "TipiRapporto":
                        {
                            TipiRapporto.Clear();
                            TipiRapporto.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            Flags = (CRMFilterFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Motivo":
                        {
                            Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeLista":
                        {
                            NomeLista = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativo":
                        {
                            IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Periodo":
                        {
                            Periodo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SortOrder":
                        {
                            SortOrder = (CRMFilterSortFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "nMax":
                        {
                            nMax = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "fromDate":
                        {
                            fromDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ResidenteA":
                        {
                            ResidenteA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Flags", (int?)Flags);
                writer.WriteAttribute("Motivo", Motivo);
                writer.WriteAttribute("NomeLista", NomeLista);
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("Periodo", Periodo);
                writer.WriteAttribute("DataInizio", DataInizio);
                writer.WriteAttribute("DataFine", DataFine);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("SortOrder", (int?)SortOrder);
                writer.WriteAttribute("nMax", nMax);
                writer.WriteAttribute("fromDate", fromDate);
                writer.WriteAttribute("ResidenteA", ResidenteA);
                writer.WriteTag("TipiAppuntamento", TipiAppuntamento);
                writer.WriteTag("Categorie", Categorie);
                writer.WriteTag("TipiRapporto", TipiRapporto);
            }

            /// <summary>
            /// Verifica se il ricontatto verifica i requisiti del filtro
            /// </summary>
            /// <param name="ric"></param>
            /// <returns></returns>
            public bool check(CRicontatto ric)
            {
                var p = ric.Persona;


                // (ric.NomeLista = Me.NomeLista) AndAlso
                bool ret = (ric.IDPuntoOperativo == IDPuntoOperativo || ric.IDAssegnatoA == IDOperatore) && DMD.DateUtils.CheckBetween(ric.DataPrevista, DataInizio, DataFine) && (string.IsNullOrEmpty(Motivo) || DMD.Strings.InStr(ric.Note, Motivo) > 0);
                if (ret && TipiAppuntamento.Count > 0)
                {
                    ret = false;
                    foreach (string value in TipiAppuntamento)
                    {
                        if ((ric.TipoAppuntamento ?? "") == (value ?? ""))
                        {
                            ret = true;
                            break;
                        }
                    }
                }

                if (ret && TipiRapporto.Count > 0)
                {
                    p = ric.Persona;
                    string tr = "";
                    if (p is CPersonaFisica && ((CPersonaFisica)p).ImpiegoPrincipale is object)
                    {
                        tr = ((CPersonaFisica)p).ImpiegoPrincipale.TipoRapporto;
                    }

                    foreach (string value in TipiRapporto)
                    {
                        if ((value ?? "") == (tr ?? ""))
                        {
                            ret = true;
                            break;
                        }
                    }
                }

                if (ret && Categorie.Count > 0)
                {
                    ret = false;
                    foreach (string value in Categorie)
                    {
                        if ((ric.Categoria ?? "") == (value ?? ""))
                        {
                            ret = true;
                            break;
                        }
                    }
                }

                if (ret && !string.IsNullOrEmpty(ResidenteA))
                {
                    p = ric.Persona;
                    ret = ret && (p.ResidenteA.NomeComune ?? "") == (ResidenteA ?? "");
                }

                return ret;
            }

            /// <summary>
            /// Clona il filtro
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                var ret = new CRMFilter();
                ret.TipiAppuntamento.AddRange(TipiAppuntamento);
                ret.Categorie.AddRange(Categorie);
                ret.TipiRapporto.AddRange(TipiRapporto);
                ret.Flags = Flags;
                ret.Motivo = Motivo;
                ret.NomeLista = NomeLista;
                ret.IDPuntoOperativo = IDPuntoOperativo;
                ret.DataInizio = DataInizio;
                ret.DataFine = DataFine;
                ret.IDOperatore = IDOperatore;
                ret.Periodo = Periodo;
                ret.SortOrder = SortOrder;
                ret.nMax = nMax;
                ret.fromDate = fromDate;
                ret.ResidenteA = ResidenteA;
                return ret;
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CRMFilter()
            {
                // //DMDObject.DecreaseCounter(this);
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((Sistema.CActivePerson)x, (Sistema.CActivePerson)y);
            }

            /// <summary>
            /// Compara due ricontatti sulla base delle impostazioni del filtro 
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public int Compare(Sistema.CActivePerson a, Sistema.CActivePerson b)
            {
                int ret = 0;
                switch (SortOrder)
                {
                    case CRMFilterSortFlags.LEASTRECENT:
                        {
                            ret = DMD.DateUtils.Compare(a.Data, b.Data);
                            if (ret == 0)
                                ret = DMD.Strings.Compare(a.Nominativo, b.Nominativo, true);
                            break;
                        }

                    case CRMFilterSortFlags.MOSTIMPORTANT:
                        {
                            int p1 = 0;
                            int p2 = 0;
                            if (a.Ricontatto is object)
                                p1 = a.Ricontatto.Priorita;
                            if (b.Ricontatto is object)
                                p2 = b.Ricontatto.Priorita;
                            ret = p1.CompareTo(p2);
                            break;
                        }

                    case CRMFilterSortFlags.MOSTRECENT:
                        {
                            ret = -DMD.DateUtils.Compare(a.Data, b.Data);
                            if (ret == 0)
                                ret = DMD.Strings.Compare(a.Nominativo, b.Nominativo, true);
                            break;
                        }

                    case CRMFilterSortFlags.NAME:
                        {
                            ret = DMD.Strings.Compare(a.Nominativo, b.Nominativo, true);
                            break;
                        }

                    default:
                        {
                            ret = -DMD.DateUtils.Compare(a.Data, b.Data);
                            if (ret == 0)
                                ret = DMD.Strings.Compare(a.Nominativo, b.Nominativo, true);
                            break;
                        }
                }

                return ret;
            }
        }
    }
}