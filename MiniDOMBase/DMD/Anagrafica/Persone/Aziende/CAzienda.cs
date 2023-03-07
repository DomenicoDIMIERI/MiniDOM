using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta un'azienda o una persona giuridica generica
        /// </summary>
        [Serializable]
        public class CAzienda 
            : CPersona
        {
            private string m_RagioneSociale;
            private string m_FormaGiuridica;
            private string m_Categoria;
            private decimal? m_CapitaleSociale;
            private decimal? m_Fatturato;
            private int? m_NumeroDipendenti;
            private string m_Tipologia;
            [NonSerialized] private CImpiegati m_Impiegati;
            private int m_IDEntePagante;
            private string m_NomeEntePagante;
            [NonSerialized] private CAzienda m_EntePagante;
            private int m_IDReferente;
            [NonSerialized] private CImpiegato m_Referente;
            private int m_ValutazioneGARF;
            private string m_TipoRapporto;
            [NonSerialized] private CAziendaUffici m_Uffici;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAzienda()
            {
                m_RagioneSociale = "";
                m_FormaGiuridica = "";
                m_Categoria = "";
                m_CapitaleSociale = default;
                m_Fatturato = default;
                m_NumeroDipendenti = default;
                m_Tipologia = "";
                m_Impiegati = null;
                m_IDEntePagante = 0;
                m_NomeEntePagante = "";
                m_EntePagante = null;
                ResidenteA.Nome = "Sede legale";
                DomiciliatoA.Nome = "Sede operativa";
                m_ValutazioneGARF = 1;
                m_TipoRapporto = "";
                m_Uffici = null;
            }

            /// <summary>
            /// Restituisce l'elenco degli uffici associati all'azienda
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CAziendaUffici Uffici
            {
                get
                {
                    lock (this)
                    {
                        if (m_Uffici is null)
                            m_Uffici = new CAziendaUffici(this);
                        if (m_Uffici.Count == 0)
                        {
                            var u = new CUfficio();
                            u.Nome = "Sede Legale";
                            u.Azienda = this;
                            u.Stato = ObjectStatus.OBJECT_VALID;
                            u.Indirizzo.InitializeFrom(ResidenteA);
                            m_Uffici.Add(u);
                            u.Save();
                            foreach (CContatto c in Recapiti)
                            {
                                u.Recapiti.Add(c);
                                c.Save(true);
                            }

                            if (!string.IsNullOrEmpty(DomiciliatoA.ToString()))
                            {
                                u = new CUfficio();
                                u.Nome = "Sede Operativa";
                                u.Azienda = this;
                                u.Stato = ObjectStatus.OBJECT_VALID;
                                u.Indirizzo.InitializeFrom(DomiciliatoA);
                                m_Uffici.Add(u);
                                u.Save(true);
                            }
                        }

                        return m_Uffici;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la sede legale
            /// </summary>
            public CUfficio SedeLegale
            {
                get
                {
                    foreach (CUfficio u in Uffici)
                    {
                        if (   u.Stato == ObjectStatus.OBJECT_VALID 
                            && DMD.RunTime.TestFlag(u.Flags, UfficioFlags.SedeLegale)
                            )
                            return u;
                    }

                    foreach (CUfficio u in Uffici)
                    {
                        if (u.Stato == ObjectStatus.OBJECT_VALID)
                            return u;
                    }

                    return null;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Aziende; //.Module;
            }

            /// <summary>
            /// Restituisce le parole chiave per l'indicizzatore
            /// </summary>
            /// <returns></returns>
            protected override string[] GetKeyWords()
            {
                var ret = new ArrayList();
                ret.AddRange(base.GetKeyWords());
                foreach (CUfficio u in Uffici)
                {
                    if (u.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        ret.Add(u.Nome);
                        // Dim str As String = Replace(Me.Alias1, vbCrLf, " ")
                        // str = Replace(str, vbCr, " ")
                        // str = Replace(str, vbLf, " ")
                        // str = Replace(str, "  ", " ")
                    }
                }

                return (string[])ret.ToArray(typeof(string));
            }

            /// <summary>
            /// Unisce le persone
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="autoDelete"></param>
            public override void MergeWith(CPersona obj, bool autoDelete = true)
            {
                if (obj is null)
                    throw new ArgumentNullException("persona");
                // If (Me Is obj) Then Return

                if (DBUtils.GetID(this, 0) != DBUtils.GetID(obj, 0))
                {
                    CAzienda persona = (CAzienda)obj;
                    var u = new CUfficio();
                    u.Nome = persona.Nominativo;
                    u.Indirizzo.MergeWith(persona.ResidenteA);
                    u.Stato = ObjectStatus.OBJECT_VALID;
                    Uffici.Add(u);
                    u.Save();
                    foreach (CContatto r in persona.Recapiti)
                    {
                        u.Recapiti.Add(r);
                        r.Save(true);
                    }

                    persona.Recapiti.Clear();
                    foreach (CImpiegato imp in persona.Impiegati)
                    {
                        imp.Azienda = this;
                        imp.Sede = u;
                        imp.Save();
                    }

                    persona.Impiegati.Clear();
                    if (string.IsNullOrEmpty(m_RagioneSociale))
                        m_RagioneSociale = persona.RagioneSociale;
                    if (string.IsNullOrEmpty(m_FormaGiuridica))
                        m_FormaGiuridica = persona.FormaGiuridica;
                    if (string.IsNullOrEmpty(m_Categoria))
                        m_Categoria = persona.Categoria;
                    if (m_CapitaleSociale.HasValue == false || m_CapitaleSociale.Value <= 0m)
                        m_CapitaleSociale = persona.CapitaleSociale;
                    if (m_Fatturato.HasValue == false || m_Fatturato.Value <= 0m)
                        m_Fatturato = persona.Fatturato;
                    if (m_NumeroDipendenti.HasValue == false || m_NumeroDipendenti.Value <= 0)
                        m_NumeroDipendenti = persona.NumeroDipendenti;
                    if (string.IsNullOrEmpty(m_Tipologia))
                        m_Tipologia = persona.Tipologia;
                    if (EntePagante is null)
                        EntePagante = persona.EntePagante;
                    if (string.IsNullOrEmpty(m_NomeEntePagante))
                        m_NomeEntePagante = persona.m_NomeEntePagante;
                    if (IDReferente == 0)
                        m_IDReferente = persona.IDReferente;
                    if (m_ValutazioneGARF < persona.m_ValutazioneGARF)
                        m_ValutazioneGARF = persona.m_ValutazioneGARF;
                    if (string.IsNullOrEmpty(m_TipoRapporto))
                        m_TipoRapporto = persona.m_TipoRapporto;
                    if (EntePagante is null)
                        EntePagante = persona.EntePagante;
                    // If (Me.Referente Is Nothing) Then Me.Referente = .Referente
                    if (string.IsNullOrEmpty(m_TipoRapporto))
                        m_TipoRapporto = persona.m_TipoRapporto;
                    base.MergeWith(persona, autoDelete);
                    m_Impiegati = null;
                    m_Uffici = null;
                }
                else
                {
                    base.MergeWith(obj, autoDelete);
                }
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            protected override Databases.DBObjectBase _Clone()
            {
                var ret = (CAzienda)base._Clone();
                ret.m_Uffici = null;
                ret.m_Impiegati = null;
                return ret;
            }

            /// <summary>
            /// Restituisce una copia esatta dell'oggetto
            /// </summary>
            /// <returns></returns>
            public CAzienda Clone()
            {
                return (CAzienda) this._Clone();
            }

            /// <summary>
            /// Restituisce o imposta il tipo rapporto predefinito per l'azienda
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoRapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoRapporto = value;
                    DoChanged("TipoRapporto", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta la valutazione GARF dell'azienda
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ValutazioneGARF
            {
                get
                {
                    return m_ValutazioneGARF;
                }

                set
                {
                    int oldValue = m_ValutazioneGARF;
                    if (oldValue == value)
                        return;
                    m_ValutazioneGARF = value;
                    DoChanged("ValutazioneGARF", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'impiegato a cui fare riferimento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDReferente
            {
                get
                {
                    return DBUtils.GetID(m_Referente, m_IDReferente);
                }

                set
                {
                    int oldValue = IDReferente;
                    if (oldValue == value)
                        return;
                    m_IDReferente = value;
                    m_Referente = null;
                    DoChanged("IDReferente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce  o imposta l'impiegato a cui fare riferimento.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CImpiegato Referente
            {
                get
                {
                    if (m_Referente is null)
                        m_Referente = Impiegati.GetItemById(m_IDReferente);
                    return m_Referente;
                }

                set
                {
                    int oldValue = IDReferente;
                    if (oldValue == DBUtils.GetID(value, 0))
                        return;
                    if (Impiegati.GetItemById(DBUtils.GetID(value, 0)) is null)
                    {
                        throw new ArgumentOutOfRangeException("Il referente non è presente nell'elenco degli impiegati");
                    }

                    m_Referente = value;
                    m_IDReferente = DBUtils.GetID(value, 0);
                    DoChanged("Referente", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce una collezione di tutte le relazioni lavorative tra l'azienda e le persone fisiche assunte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CImpiegati Impiegati
            {
                get
                {
                    if (m_Impiegati is null)
                        m_Impiegati = new CImpiegati(this);
                    return m_Impiegati;
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di azienda
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FormaGiuridica
            {
                get
                {
                    return m_FormaGiuridica;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_FormaGiuridica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FormaGiuridica = value;
                    DoChanged("FormaGiuridica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria azienda
            /// </summary>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la tipologia dell'azienda
            /// </summary>
            public string Tipologia
            {
                get
                {
                    return m_Tipologia;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipologia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipologia = value;
                    DoChanged("Tipologia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il capitale sociale nominale
            /// </summary>
            public decimal? CapitaleSociale
            {
                get
                {
                    return m_CapitaleSociale;
                }

                set
                {
                    var oldValue = m_CapitaleSociale;
                    if (oldValue == value == true)
                        return;
                    m_CapitaleSociale = value;
                    DoChanged("CapitaleSociale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il fatturato annuo dell'ultima rilevazione
            /// </summary>
            public decimal? Fatturato
            {
                get
                {
                    return m_Fatturato;
                }

                set
                {
                    var oldValue = m_Fatturato;
                    if (oldValue == value == true)
                        return;
                    m_Fatturato = value;
                    DoChanged("Fatturato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di dipendenti
            /// </summary>
            public int? NumeroDipendenti
            {
                get
                {
                    return m_NumeroDipendenti;
                }

                set
                {
                    var oldValue = m_NumeroDipendenti;
                    if (oldValue == value == true)
                        return;
                    m_NumeroDipendenti = value;
                    DoChanged("NumeroDipendenti", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o importa l'id dell'azienda principale
            /// </summary>
            public int IDEntePagante
            {
                get
                {
                    return DBUtils.GetID(m_EntePagante, m_IDEntePagante);
                }

                set
                {
                    int oldValue = IDEntePagante;
                    if (oldValue == value)
                        return;
                    m_IDEntePagante = value;
                    m_EntePagante = null;
                    DoChanged("IDEntePagante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azienda principale
            /// </summary>
            public CAzienda EntePagante
            {
                get
                {
                    if (m_EntePagante is null)
                        m_EntePagante = Aziende.GetItemById(m_IDEntePagante);
                    return m_EntePagante;
                }

                set
                {
                    var oldValue = EntePagante;
                    if (oldValue == value)
                        return;
                    m_EntePagante = value;
                    m_IDEntePagante = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeEntePagante = value.Nominativo;
                    DoChanged("EntePagante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda principale
            /// </summary>
            public string NomeEntePagante
            {
                get
                {
                    return m_NomeEntePagante;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeEntePagante;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeEntePagante = value;
                    DoChanged("NomeEntePagante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il tipo della persona
            /// </summary>
            public override TipoPersona TipoPersona
            {
                get
                {
                    return TipoPersona.PERSONA_GIURIDICA;
                }
            }

            /// <summary>
            /// Restituisce o imposta la ragione sociale dell'azienda
            /// </summary>
            public string RagioneSociale
            {
                get
                {
                    return m_RagioneSociale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_RagioneSociale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RagioneSociale = value;
                    DoChanged("RagioneSociale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome del discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Persone";
            }

            /// <summary>
            /// Restituisce true se l'azienda ha subito modifiche
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (!ret && m_Impiegati is object) ret = DBUtils.IsChanged(m_Impiegati);
                if (!ret && m_Uffici is object) ret = DBUtils.IsChanged(this.m_Uffici);
                return ret;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>
            /// Restituisce true se le aziende sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(CPersona obj)
            {
                return (obj is CPersona) && this.Equals((CPersona)obj);
            }

            /// <summary>
            /// Restituisce true se le aziende sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CAzienda obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_RagioneSociale, obj.m_RagioneSociale)
                     && DMD.Strings.EQ(this.m_FormaGiuridica, obj.m_FormaGiuridica)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Decimals.EQ(this.m_CapitaleSociale, obj.m_CapitaleSociale)
                    && DMD.Decimals.EQ(this.m_Fatturato, obj.m_Fatturato)
                    && DMD.Integers.EQ(this.m_NumeroDipendenti, obj.m_NumeroDipendenti)
                    && DMD.Strings.EQ(this.m_Tipologia, obj.m_Tipologia)
                    && DMD.Integers.EQ(this.IDEntePagante, obj.IDEntePagante)
                    && DMD.Strings.EQ(this.m_NomeEntePagante, obj.m_NomeEntePagante)
                    && DMD.Integers.EQ(this.IDReferente, obj.IDReferente)
                    && DMD.Integers.EQ(this.m_ValutazioneGARF, obj.m_ValutazioneGARF)
                    && DMD.Strings.EQ(this.m_TipoRapporto, obj.m_TipoRapporto)
                    ;
                //TODO  CImpiegati m_Impiegati m_EntePagante m_Referente m_Uffici;
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                // If (Me.m_Uffici IsNot Nothing) Then
                // Dim u As CUfficio = Me.SedeLegale
                // If (u IsNot Nothing AndAlso Not Me.ResidenteA.Equals(u.Indirizzo)) Then
                // Me.ResidenteA.CopyFrom(u.Indirizzo)
                // Me.ResidenteA.SetChanged(True)
                // End If
                // End If
                base.Save(force);
                if (m_Uffici is object) m_Uffici.Save(this.GetConnection(), force);
                if (m_Impiegati is object) m_Impiegati.Save(this.GetConnection(), force);
            }

            //protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            //{
            //    bool ret = base.SaveToDatabase(dbConn, force);
            //    if (m_Impiegati is object)
            //        m_Impiegati.Save(force);
            //    return ret;
            //}

            /// <summary>
            /// OnAfterSave
            /// Controlla se l'azienda è l'azienda principale
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                base.OnAfterSave(e);
                if (minidom.Anagrafica.Aziende.IDAziendaPrincipale == ID)
                {
                    Aziende.SetAziendaPrincipale(this);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_RagioneSociale = reader.Read("Cognome", this.m_RagioneSociale);
                m_FormaGiuridica = reader.Read("TipoAzienda", this.m_FormaGiuridica);
                m_CapitaleSociale = reader.Read("CapitaleSociale", this.m_CapitaleSociale);
                m_Fatturato = reader.Read("Fatturato", this.m_Fatturato);
                m_NumeroDipendenti = reader.Read("NumeroDipendenti", this.m_NumeroDipendenti);
                m_Categoria = reader.Read("Categoria", this.m_Categoria);
                m_Tipologia = reader.Read("Tipologia", this.m_Tipologia);
                m_IDEntePagante = reader.Read("IDEntePagante", this.m_IDEntePagante);
                m_NomeEntePagante = reader.Read("NomeEntePagante", this.m_NomeEntePagante);
                m_IDReferente = reader.Read("IDImpiego", this.m_IDReferente);
                m_ValutazioneGARF = reader.Read("GARF", this.m_ValutazioneGARF);
                m_TipoRapporto = reader.Read("TipoRapporto", this.m_TipoRapporto);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salve nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Cognome", m_RagioneSociale);
                writer.Write("TipoAzienda", m_FormaGiuridica);
                writer.Write("CapitaleSociale", m_CapitaleSociale);
                writer.Write("Fatturato", m_Fatturato);
                writer.Write("NumeroDipendenti", m_NumeroDipendenti);
                writer.Write("Categoria", m_Categoria);
                writer.Write("Tipologia", m_Tipologia);
                writer.Write("IDEntePagante", m_IDEntePagante);
                writer.Write("NomeEntePagante", m_NomeEntePagante);
                writer.Write("IDImpiego", IDReferente);
                writer.Write("GARF", m_ValutazioneGARF);
                writer.Write("TipoRapporto", m_TipoRapporto);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                DBField c;
                c = table.Fields.Ensure("Cognome", typeof(string), 255);
                c = table.Fields.Ensure("TipoAzienda", typeof(string), 255);
                c = table.Fields.Ensure("CapitaleSociale", typeof(decimal), 1);
                c = table.Fields.Ensure("Fatturato", typeof(decimal), 1);
                c = table.Fields.Ensure("NumeroDipendenti", typeof(int), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Tipologia", typeof(string), 255);
                c = table.Fields.Ensure("IDEntePagante", typeof(int), 1);
                c = table.Fields.Ensure("NomeEntePagante", typeof(string), 255);
                c = table.Fields.Ensure("IDImpiego", typeof(int), 1);
                c = table.Fields.Ensure("GARF", typeof(int), 1);
                c = table.Fields.Ensure("TipoRapporto", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCognome", new string[] { "Cognome" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTipoAzienda", new string[] { "Tipologia", "TipoAzienda", "Categoria", "TipoRapporto", "StatoCivile" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInfoAzienda", new string[] { "CapitaleSociale", "Fatturato", "NumeroDipendenti", "GARF" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInfoEnte", new string[] { "IDEntePagante", "NomeEntePagante", "NumeroDipendenti" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("IDImpiego", typeof(int), 1);
                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("RagioneSociale", m_RagioneSociale);
                writer.WriteAttribute("FormaGiuridica", m_FormaGiuridica);
                writer.WriteAttribute("CapitaleSociale", m_CapitaleSociale);
                writer.WriteAttribute("Fatturato", m_Fatturato);
                writer.WriteAttribute("NumeroDipendenti", m_NumeroDipendenti);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("Tipologia", m_Tipologia);
                writer.WriteAttribute("IDEntePagante", IDEntePagante);
                writer.WriteAttribute("NomeEntePagante", m_NomeEntePagante);
                writer.WriteAttribute("IDReferente", IDReferente);
                writer.WriteAttribute("GARF", m_ValutazioneGARF);
                writer.WriteAttribute("TipoRapporto", m_TipoRapporto);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "RagioneSociale":
                        {
                            m_RagioneSociale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FormaGiuridica":
                        {
                            m_FormaGiuridica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CapitaleSociale":
                        {
                            m_CapitaleSociale = (decimal?)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Fatturato":
                        {
                            m_Fatturato = (decimal?)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NumeroDipendenti":
                        {
                            m_NumeroDipendenti = (int?)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Tipologia":
                        {
                            m_Tipologia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDEntePagante":
                        {
                            m_IDEntePagante = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDReferente":
                        {
                            m_IDReferente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeEntePagante":
                        {
                            m_NomeEntePagante = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "GARF":
                        {
                            m_ValutazioneGARF = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoRapporto":
                        {
                            m_TipoRapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Nominativo
            /// </summary>
            public override string Nominativo
            {
                get
                {
                    return m_RagioneSociale;
                }
            }

            //protected override bool DropFromDatabase(DBConnection dbConn, bool force)
            //{
            //    if (DBUtils.GetID(Aziende.AziendaPrincipale) == ID && !force)
            //        throw new InvalidOperationException("Non è possibile eliminare l'azienda principale");
            //    return base.DropFromDatabase(dbConn, force);
            //}

            internal void InternalAddUfficio(CUfficio value)
            {
                lock (this)
                {
                    if (m_Uffici is object)
                    {
                        value = m_Uffici.GetItemById(DBUtils.GetID(value, 0));
                        if (value is object)
                            m_Uffici.Add(value);
                        m_Uffici.Sort();
                    }
                }
            }

            internal void InternalRemoveUfficio(CUfficio value)
            {
                lock (this)
                {
                    if (m_Uffici is object)
                    {
                        value = m_Uffici.GetItemById(DBUtils.GetID(value, 0));
                        if (value is object)
                            m_Uffici.Remove(value);
                    }
                }
            }

            internal void InternalUpdateUfficio(CUfficio value)
            {
                lock (this)
                {
                    if (m_Uffici is object)
                    {
                        var oldValue = m_Uffici.GetItemById(DBUtils.GetID(value, 0));
                        if (oldValue is object)
                            m_Uffici.Remove(oldValue);
                        if (value.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            m_Uffici.Add(value);
                            Uffici.Sort();
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fondazione
            /// </summary>
            public DateTime? DataFondazione
            {
                get
                {
                    return DataNascita;
                }

                set
                {
                    DataNascita = value;
                }
            }

            /// <summary>
            /// Restitusice o imposta la data di chiusura 
            /// </summary>
            public DateTime? DataChiusura
            {
                get
                {
                    return DataMorte;
                }

                set
                {
                    DataMorte = value;
                }
            }

            /// <summary>
            /// Inizializza dall'azienda
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            protected override CPersona InitializeFrom(CPersona value)
            {
                return this.InitializeFrom((CAzienda)value);
            }

            /// <summary>
            /// Inizializza dall'azienda
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public CAzienda InitializeFrom(CAzienda value)
            {
                base.InitializeFrom(value);
                this.m_RagioneSociale = value.m_RagioneSociale;
                this.m_FormaGiuridica = value.m_FormaGiuridica;
                this.m_Categoria = value.m_Categoria;
                this.m_CapitaleSociale = value.m_CapitaleSociale;
                this.m_Fatturato = value.m_Fatturato;
                this.m_NumeroDipendenti = value.m_NumeroDipendenti;
                this.m_Tipologia = value.m_Tipologia;
                this.m_IDEntePagante = value.m_IDEntePagante;
                this.m_NomeEntePagante = value.m_NomeEntePagante;
                this.m_EntePagante = value.m_EntePagante;
                this.m_IDReferente = value.m_IDReferente;
                this.m_Referente = value.m_Referente;
                this.m_ValutazioneGARF = value.m_ValutazioneGARF;
                this.m_TipoRapporto = value.m_TipoRapporto;
                //[NonSerialized] private CAziendaUffici m_Uffici;
                return this;
            }
        }
    }
}