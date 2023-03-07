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
        /// Repository di oggetti <see cref="CComune"/>
        /// </summary>
        [Serializable]
        public sealed partial class CComuniClass 
            : CModulesClass<Anagrafica.CComune>
        {
            [NonSerialized] private CKeyCollection<Anagrafica.CComune> mCodiceCatastoMap = null;
            [NonSerialized] private CKeyCollection<Anagrafica.CComune> mNomeMap = null;
            [NonSerialized] private CIntervalliCAPRepository m_IntervalliCAP = null;
           
            /// <summary>
            /// Costruttore
            /// </summary>
            public CComuniClass() 
                : base("modComuni", typeof(Anagrafica.CComuniCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce true se la stringa rappresenta un codice fiscale valido
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool IsValidCAP(string value)
            {
                value = DMD.Strings.Trim(value);
                if (DMD.Strings.Len(value) < 4)
                    return false;
                for (int i = 1, loopTo = DMD.Strings.Len(value); i <= loopTo; i++)
                {
                    if (!char.IsDigit(DMD.Chars.CChar(DMD.Strings.Mid(value, i, 1))))
                        return false;
                }

                return true;
            }

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CIntervalloCAP"/>
            /// </summary>
            public CIntervalliCAPRepository IntervalliCAP
            {
                get
                {
                    if (this.m_IntervalliCAP is null) this.m_IntervalliCAP = new CIntervalliCAPRepository();
                    return this.m_IntervalliCAP;
                }
            }

            /// <summary>
            /// Restituisce l'elemento in base al codice catastale
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public Anagrafica.CComune GetItemByCodiceCatastale(string code)
            {
                code = DMD.Strings.UCase(DMD.Strings.Trim(code));
                if (string.IsNullOrEmpty(code))
                    return null;
                // Dim i As Integer
                lock (this)
                {
                    if (mCodiceCatastoMap is null)
                        RebuildKeys();
                    return mCodiceCatastoMap.GetItemByKey(code);
                }
            }

            /// <summary>
            /// Restituisce il nome del comune (città e provincia) corrispondente al codice catastale
            /// </summary>
            /// <param name="codice"></param>
            /// <returns></returns>
            public string GetNomeComuneByCatasto(string codice)
            {
                var item = GetItemByCodiceCatastale(codice);
                if (item is null)
                    return DMD.Strings.vbNullString;
                return item.CittaEProvincia;
            }

            /// <summary>
            /// Restituisce la collezione dei comuni corrispondenti al cap
            /// </summary>
            /// <param name="cap"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CComune> GetComuniByCAP(string cap)
            {
                var ret = new CCollection<Anagrafica.CComune>();
                CCollection<Anagrafica.CComune> items;
                cap = DMD.Strings.Trim(cap);
                if (string.IsNullOrEmpty(cap))
                    return ret;
                items = LoadAll();
                foreach (Anagrafica.CComune c in items)
                {
                    if (DMD.Strings.Compare(c.CAP, cap, true) == 0)
                        ret.Add(c);
                }

                if (ret.Count == 0 && DMD.Strings.IsNumeric(cap))
                {
                    int num = DMD.Integers.ValueOf(cap);
                    foreach (Anagrafica.CComune c in items)
                    {
                        int j = 0;
                        while (j < c.IntervalliCAP.Count)
                        {
                            var ci = c.IntervalliCAP[j];
                            if (ci.Da <= num && ci.A >= num)
                            {
                                if (ret.GetItemById(DBUtils.GetID(c)) is null)
                                    ret.Add(c);
                            }

                            j += 1;
                        }
                    }
                }

                return ret;
            }

            private bool GetItemByName_Compare(string a, string b, bool strict)
            {
                if (strict)
                    return DMD.Strings.Compare(a, b, true) == 0;
                a = DMD.Strings.OnlyCharsAndNumbers(a);
                b = DMD.Strings.OnlyCharsAndNumbers(b);
                return DMD.Strings.Compare(a, b, true) == 0;
            }

            /// <summary>
            /// Restituisce il comune in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="strict"></param>
            /// <returns></returns>
            public Anagrafica.CComune GetItemByName(string nome, bool strict = true)
            {
                nome = DMD.Strings.Trim(nome);
                string nomeC = Anagrafica.Luoghi.GetComune(nome);
                string nomeP = Anagrafica.Luoghi.GetProvincia(nome);
                if (string.IsNullOrEmpty(nomeC))
                    return null;
                foreach (Anagrafica.CComune c in LoadAll())
                {
                    if (GetItemByName_Compare(c.Nome, nomeC, strict) 
                        && 
                        (string.IsNullOrEmpty(nomeP)
                        || DMD.Strings.Compare(nomeP, c.Sigla, true) == 0 
                        || DMD.Strings.Compare(nomeP, c.Provincia, true) == 0))
                    {
                        return c;
                    }
                }
                 
                return null;
            }

            private void RebuildKeys()
            {
                lock (this)
                {
                    var items = LoadAll();
                    mCodiceCatastoMap = new CKeyCollection<Anagrafica.CComune>();
                    mNomeMap = new CKeyCollection<Anagrafica.CComune>();
                    for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                    {
                        var c = items[i];
                        if (!string.IsNullOrEmpty(c.CodiceCatasto))
                            mCodiceCatastoMap.Add(DMD.Strings.UCase(c.CodiceCatasto), c);
                        if (!string.IsNullOrEmpty(c.CittaEProvincia))
                            mNomeMap.Add(DMD.Strings.UCase(c.CittaEProvincia), c);
                    }
                }
            }


            /// <summary>
            /// Aggionra l'oggetto nella cache
            /// </summary>
            /// <param name="item"></param>
            public override void UpdateCached(Anagrafica.CComune item)
            {
                base.UpdateCached(item);
                InvalidateKeys();
            }

            private void InvalidateKeys()
            {
                lock (this)
                {
                    mCodiceCatastoMap = null;
                    mNomeMap = null;
                }
            }

            private bool Find_Compare(string a, string b, bool strict)
            {
                if (strict)
                    return DMD.Strings.InStr(a, b, true) > 0;
                a = DMD.Strings.OnlyCharsAndNumbers(a);
                b = DMD.Strings.OnlyCharsAndNumbers(b);
                return DMD.Strings.InStr(a, b, true) > 0;
            }

            /// <summary>
            /// Trova il comune
            /// </summary>
            /// <param name="value"></param>
            /// <param name="strict"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.Luogo> Find(string value, bool strict = false)
            {
                var col = new CCollection<Anagrafica.Luogo>();
                string citta = Anagrafica.Luoghi.GetComune(value);
                string provincia = Anagrafica.Luoghi.GetProvincia(value);
                foreach (Anagrafica.CComune c in LoadAll())
                {
                    if (
                        Find_Compare(c.Nome, citta, strict) 
                        && 
                        (
                                string.IsNullOrEmpty(provincia) 
                            || DMD.Strings.Compare(provincia, c.Provincia, true) == 0 
                            || DMD.Strings.Compare(provincia, c.Sigla, true) == 0)
                            )
                    {
                        col.Add(c);
                    }
                }

                return col;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CComuniClass m_Comuni = null;

        /// <summary>
        /// Repository di oggetti <see cref="CComune"/>
        /// </summary>
        public static CComuniClass Comuni
        {
            get
            {
                if (m_Comuni is null)
                    m_Comuni = new CComuniClass();
                return m_Comuni;
            }
        }
    }
}