using System;



namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Classe che consente di calcolare il codice fiscale di una persona fisica
        /// </summary>
        /// <remarks></remarks>
        public sealed class CFCalculator
        {
            private const string m_consonanti = "bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ";
            private const string Mesi = "ABCDEHLMPRST";
            private const string arrPari = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private const string arrDispariCh = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private const string arrDispariVal = "1|0|5|7|9|13|15|17|19|21|1|0|5|7|9|13|15|17|19|21|2|4|18|20|11|3|6|8|12|14|16|10|22|25|24|23";
            private string m_Nome;
            private string m_Cognome;
            private string m_NatoAComune;
            private string m_NatoAProvincia;
            private string m_CodiceCatasto;
            private DateTime m_NatoIl;
            private string m_Sesso;
            private int m_ErrorCode;
            private string m_ErrorDescription;
            private string m_CodiceFiscale;
            private bool m_Calculated;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFCalculator()
            {
                ////DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Codice di errore dell'ultimo calcolo
            /// </summary>
            public int ErrorCode
            {
                get
                {
                    return m_ErrorCode;
                }
            }

            /// <summary>
            /// Descrizione dell'errore verificatosi nell'ultimo calcolo
            /// </summary>
            public string ErrorDescription
            {
                get
                {
                    return m_ErrorDescription;
                }
            }

            /// <summary>
            /// Nome della persona di cui calcolare il codice fiscale
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    m_Nome = DMD.Strings.Trim(value);
                    Invalidate();
                }
            }

            /// <summary>
            /// Cognome della persona di cui calcolare il codice fiscale
            /// </summary>
            public string Cognome
            {
                get
                {
                    return m_Cognome;
                }

                set
                {
                    m_Cognome = DMD.Strings.Trim(value);
                    Invalidate();
                }
            }

            /// <summary>
            /// Comune di nascita della persona di cui calcolare il codice fiscale
            /// </summary>
            public string NatoAComune
            {
                get
                {
                    return m_NatoAComune;
                }

                set
                {
                    m_NatoAComune = DMD.Strings.Trim(value);
                    Invalidate();
                }
            }

            /// <summary>
            /// Provincia di nascita della persona di cui calcolare il codice fiscale
            /// </summary>
            public string NatoAProvincia
            {
                get
                {
                    return m_NatoAProvincia;
                }

                set
                {
                    m_NatoAProvincia = DMD.Strings.Trim(value);
                    Invalidate();
                }
            }

            /// <summary>
            /// Codice catastale del comune o stato estero di nascita della persona di cui calcolare il codice fiscale
            /// </summary>
            public string CodiceCatasto
            {
                get
                {
                    return m_CodiceCatasto;
                }

                set
                {
                    m_CodiceCatasto = DMD.Strings.Trim(value);
                    Invalidate();
                }
            }

            /// <summary>
            /// Data di nascita della persona di cui calcolare il codice fiscale
            /// </summary>
            public DateTime NatoIl
            {
                get
                {
                    return m_NatoIl;
                }

                set
                {
                    m_NatoIl = value;
                    Invalidate();
                }
            }

            /// <summary>
            /// Sesso (M o F) della persona di cui calcolare il codice fiscale
            /// </summary>
            public string Sesso
            {
                get
                {
                    return m_Sesso;
                }

                set
                {
                    m_Sesso = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(value), 1));
                    Invalidate();
                }
            }

            /// <summary>
            /// Codice fiscale calcolato o da invertire
            /// </summary>
            public string CodiceFiscale
            {
                get
                {
                    Validate();
                    return m_CodiceFiscale;
                }

                set
                {
                    m_CodiceFiscale = value;
                }
            }

            /// <summary>
            /// Effettua il calcolo
            /// </summary>
            public void Validate()
            {
                if (m_Calculated == false)
                    Calcola();
            }

            /// <summary>
            /// Forza il ricalcolo
            /// </summary>
            public void Invalidate()
            {
                m_Calculated = false;
            }

            /// <summary>
            /// Se è stato inserito il codice fiscale inverte
            /// </summary>
            /// <remarks></remarks>
            public void Inverti()
            {
                const string mesi = "ABCDEHLMPRST";
                string cf = Sistema.Formats.ParseCodiceFiscale(m_CodiceFiscale);
                if (DMD.Strings.Len(cf) != 16)
                    return;
                m_CodiceCatasto = DMD.Strings.Mid(cf, 12, 4);
                var luogo = Luoghi.GetItemByCodiceCatastale(m_CodiceCatasto);
                if (luogo is CComune)
                {
                    {
                        var withBlock = (CComune)luogo;
                        m_NatoAComune = withBlock.Nome;
                        m_NatoAProvincia = withBlock.Provincia;
                    }
                }
                else if (luogo is CNazione)
                {
                    {
                        var withBlock1 = (CNazione)luogo;
                        m_NatoAComune = withBlock1.Nome;
                    }
                }

                int gg = DMD.Integers.ValueOf(DMD.Strings.Mid(cf, 10, 2));
                int mm = 1 + mesi.IndexOf(DMD.Strings.Mid(cf, 9, 1));
                int aa = 1900 + DMD.Integers.ValueOf(DMD.Strings.Mid(cf, 7, 2));
                if (gg > 40)
                {
                    m_Sesso = "F";
                    gg -= 40;
                }
                else
                {
                    m_Sesso = "M";
                }

                m_NatoIl = DMD.DateUtils.MakeDate(aa, mm, gg);
            }

            /// <summary>
            /// Calcola il codice fiscale
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Calcola()
            {
                string CodiceCatastale; // Codice Catastale del comune
                string CodiceCognome;
                string CodiceNome;
                string CodiceDataNascita;
                var ret = new System.Text.StringBuilder();
                m_Calculated = true;
                m_ErrorCode = 255;
                m_ErrorDescription = "Errore generico";
                if (string.IsNullOrEmpty(Nome) | string.IsNullOrEmpty(Cognome))
                {
                    m_ErrorCode = 1;
                    m_ErrorDescription = "Nome o Cognome non valido";
                    return DMD.Strings.vbNullString;
                }

                if (NatoIl == default)
                {
                    m_ErrorCode = 2;
                    m_ErrorDescription = "Data di nascita non valida";
                    return DMD.Strings.vbNullString;
                }

                if (!string.IsNullOrEmpty(m_CodiceCatasto))
                {
                    CodiceCatastale = m_CodiceCatasto;
                }
                else
                {
                    CodiceCatastale = Luoghi.GetCodiceCatastale(NatoAComune, NatoAProvincia);
                }

                if (string.IsNullOrEmpty(CodiceCatastale))
                {
                    m_ErrorCode = 2;
                    m_ErrorDescription = DMD.Strings.JoinW("Luogo di nascita non valido: ", NatoAComune, " (", NatoAProvincia, ")");
                    return DMD.Strings.vbNullString;
                }

                CodiceCognome = CalcolaCognome(DMD.Strings.OnlyChars(Cognome));
                CodiceNome = CalcolaNome(DMD.Strings.OnlyChars(Nome));
                CodiceDataNascita = CalcolaNascita(DMD.DateUtils.Day(NatoIl), DMD.DateUtils.Month(NatoIl), DMD.DateUtils.Year(NatoIl), Sesso);
                ret.Append(CodiceCognome);
                ret.Append(" ");
                ret.Append(CodiceNome);
                ret.Append(" ");
                ret.Append(CodiceDataNascita);
                ret.Append(" ");
                ret.Append(CodiceCatastale);
                ret.Append(" ");
                ret.Append(CalcolaK(CodiceCognome + CodiceNome + CodiceDataNascita + CodiceCatastale));
                m_ErrorCode = 0;
                m_ErrorDescription = "";
                m_CodiceFiscale = ret.ToString();
                return m_CodiceFiscale;
            }

            private string CalcolaCognome(string Cognome)
            {
                string code = GetConsonanti(Cognome);
                if (DMD.Strings.Len(code) >= 3)
                {
                    code = DMD.Strings.Left(code, 3);
                }
                else
                {
                    code = code + DMD.Strings.Left(GetVocali(Cognome), 3 - DMD.Strings.Len(code));
                    while (DMD.Strings.Len(code) < 3)
                        code = DMD.Strings.JoinW(code, "X");
                }

                return code;
            }

            private string CalcolaNome(string Nome)
            {
                string code;
                string cons;
                cons = GetConsonanti(Nome);
                if (DMD.Strings.Len(cons) > 3)
                {
                    // code = Mid(cons.substring(0, 1) + cons.substring(2, 3) + cons.substring(3, 4);
                    code = DMD.Strings.Mid(cons, 1, 1) + DMD.Strings.Mid(cons, 3, 1) + DMD.Strings.Mid(cons, 4, 1);
                }
                else if (DMD.Strings.Len(cons) == 3)
                {
                    code = cons;
                }
                else
                {
                    code = cons + DMD.Strings.Left(GetVocali(Nome), 3 - DMD.Strings.Len(cons));
                    while (DMD.Strings.Len(code) < 3)
                        code = DMD.Strings.JoinW(code, "X");
                }

                return code;
            }

            private string GetConsonanti(string Stringa)
            {
                string cons;
                int i;
                string ch;
                cons = "";
                var loopTo = DMD.Strings.Len(Stringa);
                for (i = 1; i <= loopTo; i++)
                {
                    ch = DMD.Strings.Mid(Stringa, i, 1);
                    if (DMD.Strings.InStr(m_consonanti, ch) > 0)
                    {
                        cons = cons + ch;
                    }
                }

                return DMD.Strings.UCase(cons);
            }

            private string GetVocali(string Stringa)
            {
                string voc;
                int i;
                string ch;
                voc = "";
                var loopTo = DMD.Strings.Len(Stringa);
                for (i = 1; i <= loopTo; i++)
                {
                    ch = DMD.Strings.Mid(Stringa, i, 1);
                    if (DMD.Strings.InStr(m_consonanti, ch) < 1 & ch != " ")
                    {
                        voc = voc + ch;
                    }
                }

                return DMD.Strings.UCase(voc);
            }

            private string CalcolaMese(int mese)
            {
                return DMD.Strings.Mid(Mesi, mese, 1);
            }

            private string CalcolaNascita(int Giorno, int Mese, int Anno, string Sesso)
            {
                var code = new System.Text.StringBuilder();
                Sesso = DMD.Strings.UCase(DMD.Strings.Trim(Sesso));
                if (DMD.Strings.Left(Sesso, 1) == "F")
                {
                    Giorno = Giorno + 40;
                }

                code.Append(DMD.Strings.Right("0000" + Anno, 2));
                code.Append(CalcolaMese(Mese));
                code.Append(DMD.Strings.Right("00" + Giorno, 2));
                return code.ToString();
            }

            private string CalcolaK(string Stringa)
            {
                int somma = 0;
                int k;
                string[] arrDispari;
                int i;
                int j;
                string ch;
                Stringa = DMD.Strings.UCase(Stringa);
                arrDispari = DMD.Strings.Split(arrDispariVal, "|");
                somma = 0;
                var loopTo = DMD.Strings.Len(Stringa);
                for (i = 1; i <= loopTo; i += 2)
                {
                    ch = DMD.Strings.Mid(Stringa, i, 1);
                    j = DMD.Strings.InStr(arrDispariCh, ch);
                    somma = somma + DMD.Integers.ValueOf(arrDispari[j - 1]);
                }

                var loopTo1 = DMD.Strings.Len(Stringa);
                for (i = 2; i <= loopTo1; i += 2)
                {
                    ch = DMD.Strings.Mid(Stringa, i, 1);
                    if (!DMD.Strings.IsNumeric(ch))
                    {
                        somma = somma + DMD.Strings.InStr(arrPari, ch) - 1;
                    }
                    else
                    {
                        somma = somma + DMD.Integers.ValueOf(ch);
                    }
                }

                k = somma % 26;
                return DMD.Strings.Mid(arrPari, k + 1, 1);
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CFCalculator()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}