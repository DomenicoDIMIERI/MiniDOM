using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Collections.Generic;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle persone
        /// </summary>
        [Serializable]
        public class CPersonaCursor 
            : minidom.Databases.DBObjectCursorPO<CPersona>
        {
            private DBCursorStringField m_DettaglioEsito = new DBCursorStringField("DettaglioEsito");
            private DBCursorStringField m_DettaglioEsito1 = new DBCursorStringField("DettagllioEsito");
            private DBCursorField<TipoPersona> m_TipoPersona = new DBCursorField<TipoPersona>("TipoPersona");
            private DBCursorStringField m_Nominativo = new DBCursorStringField("Trim([Nome] & ' ' & [Cognome])");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Cognome = new DBCursorStringField("Cognome");
            private DBCursorStringField m_Alias1 = new DBCursorStringField("Alias1");
            private DBCursorStringField m_Alias2 = new DBCursorStringField("Alias2");
            private DBCursorStringField m_Professione = new DBCursorStringField("Professione");
            private DBCursorStringField m_Titolo = new DBCursorStringField("Titolo");
            private DBCursorStringField m_Sesso = new DBCursorStringField("Sesso");
            private DBCursorStringField m_FormaGiuridica = new DBCursorStringField("TipoAzienda");
            private DBCursorStringField m_CodiceFiscale = new DBCursorStringField("CodiceFiscale");
            private DBCursorStringField m_PartitaIVA = new DBCursorStringField("PartitaIVA");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorField<bool> m_Deceduto = new DBCursorField<bool>("Deceduto");
            private DBCursorStringField m_NomeFonte = new DBCursorStringField("NomeFonte");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorField<int> m_IDStatoAttuale = new DBCursorField<int>("IDStatoAttuale");
            private DBCursorField<DateTime> m_DataNascita = new DBCursorField<DateTime>("DataNascita");
            private DBCursorField<DateTime> m_DataMorte = new DBCursorField<DateTime>("DataMorte");
            private DBCursorStringField m_NatoA_Citta = new DBCursorStringField("NatoA_Citta");
            private DBCursorStringField m_NatoA_Provincia = new DBCursorStringField("NatoA_Provincia");
            private DBCursorStringField m_MortoA_Citta = new DBCursorStringField("MortoA_Citta");
            private DBCursorStringField m_MortoA_Provincia = new DBCursorStringField("MortoA_Provincia");
            private DBCursorStringField m_ResidenteA_Citta = new DBCursorStringField("ResidenteA_Citta");
            private DBCursorStringField m_ResidenteA_Provincia = new DBCursorStringField("ResidenteA_Provincia");
            private DBCursorStringField m_ResidenteA_Via = new DBCursorStringField("ResidenteA_Via");
            private DBCursorStringField m_ResidenteA_CAP = new DBCursorStringField("ResidenteA_CAP");
            private DBCursorStringField m_DomiciliatoA_Citta = new DBCursorStringField("DomiciliatoA_Citta");
            private DBCursorStringField m_DomiciliatoA_Provincia = new DBCursorStringField("DomiciliatoA_Provincia");
            private DBCursorStringField m_DomiciliatoA_Via = new DBCursorStringField("DomiciliatoA_Via");
            private DBCursorStringField m_DomiciliatoA_CAP = new DBCursorStringField("DomiciliatoA_CAP");
            private DBCursorStringField m_Telefono = new DBCursorStringField("Telefono");
            private DBCursorStringField m_eMail = new DBCursorStringField("e-Mail");
            private DBCursorStringField m_WebSite = new DBCursorStringField("WebSite");
            private DBCursorField<double> m_Eta = new DBCursorField<double>("Eta");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<PFlags> m_PFlags = new DBCursorField<PFlags>("PFlags");
            private DBCursorField<PFlags> m_NFlags = new DBCursorField<PFlags>("NFlags");
            private DBCursorField<int> m_IDReferente1 = new DBCursorField<int>("Referente1");
            private DBCursorField<int> m_IDReferente2 = new DBCursorField<int>("Referente2");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonaCursor()
            {
            }

            /// <summary>
            /// IDStatoAttuale
            /// </summary>
            public DBCursorField<int> IDStatoAttuale
            {
                get
                {
                    return m_IDStatoAttuale;
                }
            }

            /// <summary>
            /// IDReferente1
            /// </summary>
            public DBCursorField<int> IDReferente1
            {
                get
                {
                    return m_IDReferente1;
                }
            }

            /// <summary>
            /// IDReferente2
            /// </summary>
            public DBCursorField<int> IDReferente2
            {
                get
                {
                    return m_IDReferente2;
                }
            }

            /// <summary>
            /// PFlags
            /// </summary>
            public DBCursorField<PFlags> PFlags
            {
                get
                {
                    return m_PFlags;
                }
            }

            /// <summary>
            /// NFlags
            /// </summary>
            public DBCursorField<PFlags> NFlags
            {
                get
                {
                    return m_NFlags;
                }
            }

            /// <summary>
            /// Eta
            /// </summary>
            public DBCursorField<double> Eta
            {
                get
                {
                    return m_Eta;
                }
            }

            /// <summary>
            /// Deceduto
            /// </summary>
            public DBCursorField<bool> Deceduto
            {
                get
                {
                    return m_Deceduto;
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// DettaglioEsito
            /// </summary>
            public DBCursorStringField DettaglioEsito
            {
                get
                {
                    return m_DettaglioEsito;
                }
            }

            /// <summary>
            /// DettaglioEsito1
            /// </summary>
            public DBCursorStringField DettaglioEsito1
            {
                get
                {
                    return m_DettaglioEsito1;
                }
            }

            /// <summary>
            /// NomeFonte
            /// </summary>
            public DBCursorStringField NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }
            }

            /// <summary>
            /// IDFonte
            /// </summary>
            public DBCursorField<int> IDFonte
            {
                get
                {
                    return m_IDFonte;
                }
            }

            /// <summary>
            /// TipoFonte
            /// </summary>
            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }
            }

            /// <summary>
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// Nominativo
            /// </summary>
            public DBCursorStringField Nominativo
            {
                get
                {
                    return m_Nominativo;
                }
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Cognome
            /// </summary>
            public DBCursorStringField Cognome
            {
                get
                {
                    return m_Cognome;
                }
            }

            /// <summary>
            /// Alias1
            /// </summary>
            public DBCursorStringField Alias1
            {
                get
                {
                    return m_Alias1;
                }
            }

            /// <summary>
            /// Alias2
            /// </summary>
            public DBCursorStringField Alias2
            {
                get
                {
                    return m_Alias2;
                }
            }

            /// <summary>
            /// Professione
            /// </summary>
            public DBCursorStringField Professione
            {
                get
                {
                    return m_Professione;
                }
            }

            /// <summary>
            /// Titolo
            /// </summary>
            public DBCursorStringField Titolo
            {
                get
                {
                    return m_Titolo;
                }
            }

            /// <summary>
            /// Sesso
            /// </summary>
            public DBCursorStringField Sesso
            {
                get
                {
                    return m_Sesso;
                }
            }

            /// <summary>
            /// FormaGiuridica
            /// </summary>
            public DBCursorStringField FormaGiuridica
            {
                get
                {
                    return m_FormaGiuridica;
                }
            }

            /// <summary>
            /// DataNascita
            /// </summary>
            public DBCursorField<DateTime> DataNascita
            {
                get
                {
                    return m_DataNascita;
                }
            }

            /// <summary>
            /// DataMorte
            /// </summary>
            public DBCursorField<DateTime> DataMorte
            {
                get
                {
                    return m_DataMorte;
                }
            }

            /// <summary>
            /// NatoA_Citta
            /// </summary>
            public DBCursorStringField NatoA_Citta
            {
                get
                {
                    return m_NatoA_Citta;
                }
            }

            /// <summary>
            /// NatoA_Provincia
            /// </summary>
            public DBCursorStringField NatoA_Provincia
            {
                get
                {
                    return m_NatoA_Provincia;
                }
            }

            /// <summary>
            /// MortoA_Citta
            /// </summary>
            public DBCursorStringField MortoA_Citta
            {
                get
                {
                    return m_MortoA_Citta;
                }
            }

            /// <summary>
            /// MortoA_Provincia
            /// </summary>
            public DBCursorStringField MortoA_Provincia
            {
                get
                {
                    return m_MortoA_Provincia;
                }
            }

            /// <summary>
            /// ResidenteA_Citta
            /// </summary>
            public DBCursorStringField ResidenteA_Citta
            {
                get
                {
                    return m_ResidenteA_Citta;
                }
            }

            /// <summary>
            /// ResidenteA_Provincia
            /// </summary>
            public DBCursorStringField ResidenteA_Provincia
            {
                get
                {
                    return m_ResidenteA_Provincia;
                }
            }

            /// <summary>
            /// ResidenteA_Via
            /// </summary>
            public DBCursorStringField ResidenteA_Via
            {
                get
                {
                    return m_ResidenteA_Via;
                }
            }

            /// <summary>
            /// ResidenteA_CAP
            /// </summary>
            public DBCursorStringField ResidenteA_CAP
            {
                get
                {
                    return m_ResidenteA_CAP;
                }
            }

            /// <summary>
            /// DomiciliatoA_Citta
            /// </summary>
            public DBCursorStringField DomiciliatoA_Citta
            {
                get
                {
                    return m_DomiciliatoA_Citta;
                }
            }

            /// <summary>
            /// DomiciliatoA_Provincia
            /// </summary>
            public DBCursorStringField DomiciliatoA_Provincia
            {
                get
                {
                    return m_DomiciliatoA_Provincia;
                }
            }

            /// <summary>
            /// DomiciliatoA_Via
            /// </summary>
            public DBCursorStringField DomiciliatoA_Via
            {
                get
                {
                    return m_DomiciliatoA_Via;
                }
            }

            /// <summary>
            /// DomiciliatoA_CAP
            /// </summary>
            public DBCursorStringField DomiciliatoA_CAP
            {
                get
                {
                    return m_DomiciliatoA_CAP;
                }
            }

            /// <summary>
            /// CodiceFiscale
            /// </summary>
            public DBCursorStringField CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }
            }

            /// <summary>
            /// PartitaIVA
            /// </summary>
            public DBCursorStringField PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }
            }

            /// <summary>
            /// TipoPersona
            /// </summary>
            public DBCursorField<TipoPersona> TipoPersona
            {
                get
                {
                    return m_TipoPersona;
                }
            }

            /// <summary>
            /// Telefono
            /// </summary>
            public DBCursorStringField Telefono
            {
                get
                {
                    return m_Telefono;
                }
            }

            /// <summary>
            /// eMail
            /// </summary>
            public DBCursorStringField eMail
            {
                get
                {
                    return m_eMail;
                }
            }

            /// <summary>
            /// WebSite
            /// </summary>
            public DBCursorStringField WebSite
            {
                get
                {
                    return m_WebSite;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CPersona InstantiateNewT(DBReader dbRis)
            {
                if (dbRis is object)
                {
                    return Persone.Instantiate((TipoPersona)dbRis.Read("TipoPersona", 0));
                }
                else
                {
                    return Persone.Instantiate((TipoPersona)TipoPersona.Value);
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Persone";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Persone; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Restituisce i campi utilizzabili dal cursore per la clausola where
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldsCollection GetCursorFields()
            {
                var ret = base.GetCursorFields();
                ret.Remove(m_Nominativo);
                ret.RemoveByKey("ResidenteA_Citta");
                ret.RemoveByKey("ResidenteA_Provincia");
                ret.RemoveByKey("ResidenteA_Via");
                ret.RemoveByKey("ResidenteA_CAP");
                ret.RemoveByKey("DomiciliatoA_Citta");
                ret.RemoveByKey("DomiciliatoA_Provincia");
                ret.RemoveByKey("DomiciliatoA_Via");
                ret.RemoveByKey("DomiciliatoA_CAP");
                ret.RemoveByKey("Eta");
                ret.RemoveByKey("e-Mail");
                ret.RemoveByKey("Telefono");
                ret.RemoveByKey("WebSite");
                return ret;
            }

            private bool IsResidenzaSet()
            {
                return m_ResidenteA_Citta.IsSet() || m_ResidenteA_Provincia.IsSet() || m_ResidenteA_CAP.IsSet() || m_ResidenteA_Via.IsSet();
            }

            private bool IsDomicilioSet()
            {
                return m_DomiciliatoA_Citta.IsSet() || m_DomiciliatoA_Provincia.IsSet() || m_DomiciliatoA_CAP.IsSet() || m_DomiciliatoA_Via.IsSet();
            }

            /// <summary>
            /// Genera la clausola where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var ret = base.GetWherePart();

                int[] findIn = null;
                if (    
                        this.m_Nominativo.IsSet() 
                    && !string.IsNullOrEmpty(DMD.Strings.Trim(this.m_Nominativo.Value))
                    )
                {
                    var frs = Sistema.IndexingService.Find(m_Nominativo.Value, default);
                    var lst = new List<int>();
                    foreach (CResult o in frs)
                    {
                        lst.Add(o.OwnerID);
                    }
                    lst.Sort();
                    findIn = lst.ToArray();                   
                }

                if (this.IsResidenzaSet())
                {
                    var lst = new List<int>();
                    using (var cursor = new CIndirizziCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Nome.Value = "Residenza";
                        if (m_ResidenteA_CAP.IsSet())
                        {
                            cursor.CAP.Operator = this.m_ResidenteA_CAP.Operator;
                            cursor.CAP.IncludeNulls = this.m_ResidenteA_CAP.IncludeNulls;
                            cursor.CAP.Values = this.m_ResidenteA_CAP.Values;
                        }

                        if (m_ResidenteA_Citta.IsSet())
                        {
                            cursor.Citta.Operator = this.m_ResidenteA_Citta.Operator;
                            cursor.Citta.IncludeNulls = this.m_ResidenteA_Citta.IncludeNulls;
                            cursor.Citta.Values = this.m_ResidenteA_Citta.Values;
                        }

                        if (m_ResidenteA_Citta.IsSet())
                        {
                            cursor.Provincia.Operator = this.m_ResidenteA_Provincia.Operator;
                            cursor.Provincia.IncludeNulls = this.m_ResidenteA_Provincia.IncludeNulls;
                            cursor.Provincia.Values = this.m_ResidenteA_Provincia.Values;
                        }

                        if (m_ResidenteA_Via.IsSet())
                        {
                            // wherePart2 = DMD.Strings.Combine(wherePart2, Me.m_ResidenteA_Via.GetSQL("Via"), " AND ")
                            string via = m_ResidenteA_Via.Value;
                            var tmp = new CIndirizzo();
                            tmp.ToponimoViaECivico = via;
                            if (!string.IsNullOrEmpty(tmp.Toponimo)) cursor.Toponimo.Value = tmp.Toponimo;
                            if (!string.IsNullOrEmpty(tmp.Via)) cursor.Via.Value = tmp.Via;
                            if (!string.IsNullOrEmpty(tmp.Civico)) cursor.Civico.Value = tmp.Civico;
                        }

                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.PersonaID);
                        }
                    }

                    lst.Sort();

                    findIn = (findIn == null)? lst.ToArray() : DMD.Arrays.Join(findIn, lst.ToArray());
                }

                if (this.IsDomicilioSet())
                {
                    var lst = new List<int>();
                    using (var cursor = new CIndirizziCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Nome.Value = "Domicilio";
                        if (m_DomiciliatoA_CAP.IsSet())
                        {
                            cursor.CAP.Operator = this.m_DomiciliatoA_CAP.Operator;
                            cursor.CAP.IncludeNulls = this.m_DomiciliatoA_CAP.IncludeNulls;
                            cursor.CAP.Values = this.m_DomiciliatoA_CAP.Values;
                        }

                        if (m_DomiciliatoA_Citta.IsSet())
                        {
                            cursor.Citta.Operator = this.m_DomiciliatoA_Citta.Operator;
                            cursor.Citta.IncludeNulls = this.m_DomiciliatoA_Citta.IncludeNulls;
                            cursor.Citta.Values = this.m_DomiciliatoA_Citta.Values;
                        }

                        if (m_DomiciliatoA_Citta.IsSet())
                        {
                            cursor.Provincia.Operator = this.m_DomiciliatoA_Provincia.Operator;
                            cursor.Provincia.IncludeNulls = this.m_DomiciliatoA_Provincia.IncludeNulls;
                            cursor.Provincia.Values = this.m_DomiciliatoA_Provincia.Values;
                        }

                        if (m_DomiciliatoA_Via.IsSet())
                        {
                            // wherePart2 = DMD.Strings.Combine(wherePart2, Me.m_DomiciliatoA_Via.GetSQL("Via"), " AND ")
                            string via = m_DomiciliatoA_Via.Value;
                            var tmp = new CIndirizzo();
                            tmp.ToponimoViaECivico = via;
                            if (!string.IsNullOrEmpty(tmp.Toponimo)) cursor.Toponimo.Value = tmp.Toponimo;
                            if (!string.IsNullOrEmpty(tmp.Via)) cursor.Via.Value = tmp.Via;
                            if (!string.IsNullOrEmpty(tmp.Civico)) cursor.Civico.Value = tmp.Civico;
                        }

                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.PersonaID);
                        }
                    }

                    lst.Sort();

                    findIn = (findIn == null) ? lst.ToArray() : DMD.Arrays.Join(findIn, lst.ToArray());
                }

                if (this.m_Telefono.IsSet())
                {
                    var lst = new List<int>();
                    using(var cursor = new CContattoCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Tipo.ValueIn(new string[] { "telefono", "fax", "cellulare" });
                        cursor.Valore.Operator = this.m_Telefono.Operator;
                        cursor.Valore.IncludeNulls = this.m_Telefono.IncludeNulls;
                        cursor.Valore.Values = this.m_Telefono.Values;

                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.PersonaID);
                        }
                    }
                    lst.Sort();
                    findIn = (findIn == null) ? lst.ToArray() : DMD.Arrays.Join(findIn, lst.ToArray());
                }

                if (this.m_eMail.IsSet())
                {
                    var lst = new List<int>();
                    using (var cursor = new CContattoCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Tipo.ValueIn(new string[] { "e-mail", "pec" });
                        cursor.Valore.Operator = this.m_eMail.Operator;
                        cursor.Valore.IncludeNulls = this.m_eMail.IncludeNulls;
                        cursor.Valore.Values = this.m_eMail.Values;

                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.PersonaID);
                        }
                    }
                    lst.Sort();
                    findIn = (findIn == null) ? lst.ToArray() : DMD.Arrays.Join(findIn, lst.ToArray());
                }

                if (this.m_WebSite.IsSet())
                {
                    var lst = new List<int>();
                    using (var cursor = new CContattoCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Tipo.ValueIn(new string[] { "website" });
                        cursor.Valore.Operator = this.m_WebSite.Operator;
                        cursor.Valore.IncludeNulls = this.m_WebSite.IncludeNulls;
                        cursor.Valore.Values = this.m_WebSite.Values;

                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.PersonaID);
                        }
                    }
                    lst.Sort();
                    findIn = (findIn == null) ? lst.ToArray() : DMD.Arrays.Join(findIn, lst.ToArray());
                }

                if (findIn is object )
                {
                    if (findIn.Length == 0) ret = DBCursorFieldBase.False;
                    else
                    {
                        var id = new DBCursorField<int>("ID", findIn);
                        ret = ret.And(id);
                    }
                }

                if (m_Eta.IsSet())
                {
                    var d = DMD.DateUtils.Now();
                    var d1 = default(DateTime?);
                    var d2 = default(DateTime?);
                    if (m_Eta.Value.HasValue) d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, -m_Eta.Value.Value, d);
                    if (m_Eta.Value1.HasValue)
                    {
                        d2 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, -m_Eta.Value1.Value, d);
                        d2 = DMD.DateUtils.GetLastSecond(d2);
                    }

                    switch (m_Eta.Operator)
                    {
                        case OP.OP_BETWEEN:
                            {
                                var cEta1 = new DBCursorField<DateTime>("DataNascita", d1, OP.OP_GE);
                                var cEta2 = new DBCursorField<DateTime>("DataNascita", d2, OP.OP_LT);

                                ret = ret.And(cEta1);
                                ret = ret.And(cEta2);
                                break;
                            }

                        case OP.OP_EQ:
                        case OP.OP_LIKE:
                            {
                                d1 = DMD.DateUtils.GetDatePart(d1);
                                d2 = d1;
                                d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, -1, d1);
                                d2 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, 1d, d1);
                                d2 = DMD.DateUtils.GetLastSecond(d2);

                                var cEta1 = new DBCursorField<DateTime>("DataNascita", d1, OP.OP_GT);
                                var cEta2 = new DBCursorField<DateTime>("DataNascita", d2, OP.OP_LE);

                                ret = ret.And(cEta1);
                                ret = ret.And(cEta2);
                                break;
                            }
                        case OP.OP_GE:
                            {
                                d2 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, 1d, d1);
                                d2 = DMD.DateUtils.GetLastSecond(d2);

                                var cEta2 = new DBCursorField<DateTime>("DataNascita", d2, OP.OP_LT);
                                ret = ret.And(cEta2);

                                break;
                            }

                        case OP.OP_GT:
                            {
                                d1 = DMD.DateUtils.GetDatePart(d1);
                                d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, -1, d1);
                                d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Day, -1, d1);
                                d1 = DMD.DateUtils.GetLastSecond(d1);

                                var cEta1 = new DBCursorField<DateTime>("DataNascita", d1, OP.OP_LE);
                                ret = ret.And(cEta1);

                                break;
                            }

                        case OP.OP_LE:
                            {
                                d1 = DMD.DateUtils.GetDatePart(d1);

                                var cEta1 = new DBCursorField<DateTime>("DataNascita", d1, OP.OP_GE);
                                ret = ret.And(cEta1);

                                break;
                            }

                        case OP.OP_LT:
                            {
                                d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Day, 1d, DMD.DateUtils.GetDatePart(d1));

                                var cEta1 = new DBCursorField<DateTime>("DataNascita", d1, OP.OP_GE);
                                ret = ret.And(cEta1);

                                break;
                            }

                        case OP.OP_NE:
                        case OP.OP_NOTLIKE:
                            {
                                d1 = DMD.DateUtils.GetDatePart(d1);
                                d2 = d1;
                                d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, -1, d1);
                                d2 = DMD.DateUtils.DateAdd(DateTimeInterval.Year, 1d, d1);
                                d2 = DMD.DateUtils.GetLastSecond(d2);
                                //w = "([DataNascita]<=" + DBUtils.DBDate(d1) + " OR [DataNascita]>" + DBUtils.DBDate(d2) + ")";

                                var cEta1 = new DBCursorField<DateTime>("DataNascita", d1, OP.OP_LE);
                                var cEta2 = new DBCursorField<DateTime>("DataNascita", d2, OP.OP_GT);

                                ret = ret * (cEta1 | cEta2);

                                break;
                            }
                    }

                }


                return ret;
            }

         
            /// <summary>
            /// Restituisce la clausola where
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                DBCursorFieldBase wherePart = DBCursorFieldBase.True;

                if (!this.Module.UserCanDoAction("list"))
                {
                    wherePart = DBCursorFieldBase.False;

                    var user = minidom.Sistema.Users.CurrentUser;
                    if (this.Module.UserCanDoAction("list_office"))
                    {
                        var arrPO = new List<int>();
                        foreach (var ufficio in user.Uffici)
                        {
                            arrPO.Add(DBUtils.GetID(ufficio, 0));
                        }

                        var officePart = new DBCursorField<int>("IDPuntoOperativo", OP.OP_IN, true);
                        officePart.ValueIn(arrPO.ToArray());
                        wherePart = wherePart.Or(officePart);
                    }

                    if (this.Module.UserCanDoAction("list_own"))
                    {
                        var userPart = new DBCursorField<int>("CreatoDa", DBUtils.GetID(user, 0)); 
                        wherePart = wherePart.Or(userPart);

                        var referente1 = new DBCursorField<int>("IDReferente1", DBUtils.GetID(user, 0));
                        wherePart = wherePart.Or(referente1);

                        var referente2 = new DBCursorField<int>("IDReferente2", DBUtils.GetID(user, 0)); 
                        wherePart = wherePart.Or(referente2);

                    }


                }

                return wherePart;
            }
 

            /// <summary>
            /// Restituisce i campi che entrano nella clausola sort
            /// </summary>
            /// <returns></returns>
            public override List<DBCursorField> GetSortFields()
            {
                var ret = base.GetSortFields();
                ret.Remove(this.m_ResidenteA_Citta);
                ret.Remove(this.m_ResidenteA_Provincia);
                ret.Remove(this.m_ResidenteA_Via);
                ret.Remove(this.m_ResidenteA_CAP);
                ret.Remove(this.m_DomiciliatoA_Citta);
                ret.Remove(this.m_DomiciliatoA_Provincia);
                ret.Remove(this.m_DomiciliatoA_Via);
                ret.Remove(this.m_DomiciliatoA_CAP);
                ret.Remove(this.m_Eta);
                ret.Remove(this.m_eMail);
                ret.Remove(this.m_Telefono);
                ret.Remove(this.m_WebSite);
                return ret;
            }

            
        }
    }
}