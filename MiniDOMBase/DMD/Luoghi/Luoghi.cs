using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="Luogo"/>
        /// </summary>
        [Serializable]
        public sealed class CLuoghiClass 
            : CModulesClass<Anagrafica.Luogo>
        {
            
            [NonSerialized] private CComuniClass m_Comuni = null;
            [NonSerialized] private CNazioniClass m_Nazioni = null;
            [NonSerialized] private CProvinceClass m_Province = null;
            [NonSerialized] private CRegioniClass m_Regioni = null;
            [NonSerialized] private CIndirizziClass m_Indirizzi = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CLuoghiClass() 
                : base("modLuoghi", typeof(Anagrafica.LuogoCursor<Anagrafica.Luogo>))
            {
            }

            

            /// <summary>
            /// Repository di oggetti <see cref="CIndirizzo"/>
            /// </summary>
            public CIndirizziClass Indirizzi
            {
                get
                {
                    if (m_Indirizzi is null)
                        m_Indirizzi = new CIndirizziClass();
                    return m_Indirizzi;
                }
            }
             

            /// <summary>
            /// Cerca tutti i luoghi che corrispondono alla descrizione
            /// </summary>
            /// <param name="value"></param>
            /// <param name="strict"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.Luogo> FindLuoghi(string value, bool strict = false)
            {
                var col = Anagrafica.Luoghi.Comuni.Find(value, strict);
                if (col.Count == 0)
                {
                    col = Anagrafica.Luoghi.Nazioni.Find(value, strict);
                }

                col.Comparer = new NomeComuniSorter(value);
                col.Sort();
                return col;
            }

            /// <summary>
            /// Formatta il comune
            /// </summary>
            /// <param name="nomeComune"></param>
            /// <param name="nomeProvincia"></param>
            /// <returns></returns>
            public string FormatNome(string nomeComune, string nomeProvincia)
            {
                string ret = DMD.Strings.Trim(nomeComune);
                nomeProvincia = DMD.Strings.Trim(nomeProvincia);
                if (!string.IsNullOrEmpty(nomeProvincia))
                    ret += " (" + nomeProvincia + ")";
                return ret;
            }

            /// <summary>
            /// Restituisce il codice catastale del luogo
            /// </summary>
            /// <param name="nomeLuogo">[in] Es. Salerno (SA), Italia, San Marino (EE)</param>
            /// <returns></returns>
            public string GetCodiceCatastale(string nomeLuogo)
            {
                var provincia = minidom.Anagrafica.Luoghi.GetProvincia(nomeLuogo);
                var comune = minidom.Anagrafica.Luoghi.GetComune(nomeLuogo);
                return this.GetCodiceCatastale(comune, provincia);
            }

            /// <summary>
            /// Restituisce il codice catastale del luogo
            /// </summary>
            /// <param name="comune"></param>
            /// <param name="provincia"></param>
            /// <returns></returns>
            public string GetCodiceCatastale(string comune, string provincia)
            {
                // Dim ret As String = vbNullString
                comune = DMD.Strings.Trim(comune);
                provincia = DMD.Strings.Trim(provincia);
                if (string.IsNullOrEmpty(comune))
                    throw new ArgumentNullException("comune");

                if (DMD.Strings.UCase(provincia) != "EE")
                {
                    // ret = Formats.ToString(APPConn.ExecuteScalar("SELECT [Codice_Catasto] FROM [tbl_Luoghi_Comuni] WHERE [Nome]='" & comune & "' AND [Provincia]='" & provincia & "' AND [Stato]=" & ObjectStatus.OBJECT_VALID))
                    foreach (var c in Anagrafica.Luoghi.Comuni.LoadAll())
                    {
                        if (
                                DMD.Strings.Compare(c.Nome, comune, true) == 0 
                            && (DMD.Strings.Compare(c.Provincia, provincia, true) == 0 || DMD.Strings.Compare(c.Sigla, provincia, true) == 0)
                            )
                            return c.CodiceCatasto;
                    }
                }

                foreach (var n in Anagrafica.Luoghi.Nazioni.LoadAll())
                {
                    if (DMD.Strings.Compare(n.Nome, comune, true) == 0)
                        return n.CodiceCatasto;
                }

                return "";
                // Return ret
            }

            /// <summary>
            /// Espande il nome della provincia a partire dalla sua sigla. Se il valore è già un nome viene restituito il valore stesso
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public string EspandiNomeProvincia(string value)
            {
                foreach (Anagrafica.CProvincia p in Anagrafica.Luoghi.Province.LoadAll())
                {
                    if (DMD.Strings.Compare(p.Sigla, value, true) == 0)
                        return p.Nome;
                }

                return "";
            }

            /// <summary>
            /// Estrae il nome del comune dalla stringa
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public string GetComune(string text)
            {
                int i, j = default;
                i = DMD.Strings.InStr(text, "(");
                if (i > 0)
                    j = DMD.Strings.InStr(i, text, ")");
                if (i > 0 & j > i)
                {
                    return DMD.Strings.Trim(DMD.Strings.Left(text, i - 1));
                }
                else
                {
                    return DMD.Strings.Trim(text);
                }
            }

            /// <summary>
            /// Estrae il nome della provincia dalla stringa
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public string GetProvincia(string text)
            {
                int i, j = default;
                i = DMD.Strings.InStr(text, "(");
                if (i > 0)
                    j = DMD.Strings.InStr(i, text, ")");
                if (i > 0 & j > i)
                {
                    return DMD.Strings.Trim(DMD.Strings.Mid(text, i + 1, j - i - 1));
                }
                else
                {
                    return DMD.Strings.vbNullString;
                }
            }

            /// <summary>
            /// Restituisce il luogo corrispondente al codice catastale
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public Anagrafica.Luogo GetItemByCodiceCatastale(string code)
            {
                Anagrafica.Luogo item = Comuni.GetItemByCodiceCatastale(code);
                if (item is null)
                    item = Nazioni.GetItemByCodiceCatastale(code);
                return item;
            }

            private string[] _toponimiSupportati = null;

            /// <summary>
            /// Restituisce i toponimi supportati
            /// </summary>
            /// <returns></returns>
            public string[] GetToponimiSupportati()
            {
                if (_toponimiSupportati is null)
                {
                    _toponimiSupportati = new[] { "Via", "Zona Industriale", "Parco", "Rione", "Largo", "L.go", "Viale", "V.le", "Corso", "C.so", "Vicolo", "V.lo", "Strada provinciale", "SP", "Strada Statale", "SS", "Piazza", "P.zza", "C/da", "C.da", "Cda", "Contrada", "Piazzale", "P.tta", "Piazzetta", "Str. vic.le", "Str. vicinale", "Loc.", "Località", "Vico", "Largo", "Vicolo", "Rampa", "Salita" };
                    DMD.Arrays.Sort(_toponimiSupportati, 0, _toponimiSupportati.Length, new CFunctionComparer(CompareToponimi));
                }

                return _toponimiSupportati;
            }

            /// <summary>
            /// Converte l'abbreviazione di un toponimo nella sua forma completa.
            /// Es. Pzz in Piazza
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string EspandiToponimo(string value)
            {
                value = DMD.Strings.Trim(value);
                switch (DMD.Strings.LCase(value) ?? "")
                {
                    // case "via":
                    // case "zona Industriale"
                    // case "parco"
                    // case "rione"
                    // case "largo"
                    case "l.go":
                        {
                            break;
                        }

                    case "lgo":
                        {
                            value = "Largo";
                            break;
                        }

                    case "v.le":
                        {
                            value = "Viale";
                            break;
                        }

                    case "c.so":
                        {
                            break;
                        }

                    case "cso":
                        {
                            value = "Corso";
                            break;
                        }

                    case "v.lo":
                        {
                            value = "Vicolo";
                            break;
                        }

                    case "sp":
                        {
                            value = "Strada provinciale";
                            break;
                        }

                    case "ss":
                        {
                            value = "Strada statale";
                            break;
                        }

                    case "p.zza":
                        {
                            value = "Piazza";
                            break;
                        }

                    case "c.da":
                        {
                            break;
                        }

                    case "cda":
                        {
                            break;
                        }

                    case "c/da":
                        {
                            break;
                        }

                    case "contr.":
                        {
                            value = "Contrada";
                            break;
                        }

                    case "p.le":
                        {
                            break;
                        }

                    case "ple":
                        {
                            value = "Piazzale";
                            break;
                        }

                    case "p.tta":
                        {
                            value = "Piazzetta";
                            break;
                        }

                    case "str. vic.le":
                        {
                            break;
                        }

                    case "str. vicinale":
                        {
                            value = "Strada vicinale";
                            break;
                        }

                    case "loc.":
                        {
                            break;
                        }

                    case "loc":
                        {
                            break;
                        }

                    case "localita'":
                        {
                            value = "Località";
                            break;
                        }
                        // case "vico":
                        // case "largo"
                        // case "vicolo"
                        // case "rampa"
                        // case "salita"
                }

                return value;
            }


            private int CompareToponimi(object a, object b)
            {
                int al = DMD.Strings.Len(DMD.Strings.CStr(a));
                int bl = DMD.Strings.Len(DMD.Strings.CStr(b));
                return bl - al;
            }

            /// <summary>
            /// Formata nome e provinca
            /// </summary>
            /// <param name="nomeCitta"></param>
            /// <param name="siglaProvincia"></param>
            /// <returns></returns>
            public string MergeComuneeProvincia(string nomeCitta, string siglaProvincia)
            {
                string ret = DMD.Strings.Trim(nomeCitta);
                siglaProvincia = DMD.Strings.Trim(siglaProvincia);
                if (!string.IsNullOrEmpty(siglaProvincia))
                    ret = ret + " (" + siglaProvincia + ")";
                return ret;
            }

           
            /// <summary>
            /// Repository di oggetti <see cref="CComune"/>
            /// </summary>
            public CComuniClass Comuni
            {
                get
                {
                    if (m_Comuni is null)
                        m_Comuni = new CComuniClass();
                    return m_Comuni;
                }
            }

            /// <summary>
            /// Repository di oggetti <see cref="CNazione"/>
            /// </summary>
            public CNazioniClass Nazioni
            {
                get
                {
                    if (m_Nazioni is null)
                        m_Nazioni = new CNazioniClass();
                    return m_Nazioni;
                }
            }

            [NonSerialized] private CContinentiClass m_Continenti = null;

            /// <summary>
            /// Repository di oggetti <see cref="CContinente"/>
            /// </summary>
            public CContinentiClass Continenti
            {
                get
                {
                    if (m_Continenti is null)
                        m_Continenti = new CContinentiClass();
                    return m_Continenti;
                }
            }

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CProvincia"/>
            /// </summary>
            public CProvinceClass Province
            {
                get
                {
                    if (m_Province is null)
                        m_Province = new CProvinceClass();
                    return m_Province;
                }
            }

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CRegione"/>
            /// </summary>
            public CRegioniClass Regioni
            {
                get
                {
                    if (m_Regioni is null)
                        m_Regioni = new CRegioniClass();
                    return m_Regioni;
                }
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            private class NomeComuniSorter : IComparer
            {
                private string m_Text;

                public NomeComuniSorter(string nome)
                {
                    //DMDObject.IncreaseCounter(this);
                    m_Text = nome;
                }

                private string GetText(Anagrafica.Luogo l)
                {
                    if (l is Anagrafica.CComune)
                    {
                        return ((Anagrafica.CComune)l).CittaEProvincia;
                    }
                    else
                    {
                        return ((Anagrafica.CNazione)l).Nome;
                    }
                }

                private int Compare(Anagrafica.Luogo x, Anagrafica.Luogo y)
                {
                    string str1 = GetText(x);
                    string str2 = GetText(y);
                    int i1 = DMD.Strings.InStr(str1, m_Text, true);
                    int i2 = DMD.Strings.InStr(str2, m_Text, true);
                    int ret = i1 - i2;
                    if (ret == 0)
                        ret = DMD.Strings.Compare(str1, str2, true);
                    return ret;
                }

                int IComparer.Compare(object x, object y)
                {
                    return Compare((Anagrafica.Luogo)x, (Anagrafica.Luogo)y);
                }

                
            }

            
        }
    }

    public partial class Anagrafica
    {
        private static CLuoghiClass m_Luoghi = null;

        /// <summary>
        /// Repository di oggetti <see cref="Luogo"/>
        /// </summary>
        public static CLuoghiClass Luoghi
        {
            get
            {
                if (m_Luoghi is null)
                    m_Luoghi = new CLuoghiClass();
                return m_Luoghi;
            }
        }
    }
}