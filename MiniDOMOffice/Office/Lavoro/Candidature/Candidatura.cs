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
        /// Rappresenta una candidatura di una persona ad un posto di lavoro
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Candidatura 
            : minidom.Databases.DBObjectPO
        {
            private DateTime? m_DataCandidatura;
            private int m_IDCandidato;
            [NonSerialized] private Anagrafica.CPersona m_Candidato;
            private string m_NomeCandidato;
            private int m_IDCurriculum;
            [NonSerialized] private Curriculum m_Curriculum;
            private int m_IDOfferta;
            [NonSerialized] private OffertaDiLavoro m_Offerta;
            private string m_NomeOfferta;
            private int m_IDCanale;
            [NonSerialized] private Anagrafica.CCanale m_Canale;
            private string m_NomeCanale;
            private string m_TipoFonte;
            private int m_IDFonte;
            [NonSerialized] private IFonte m_Fonte;
            private string m_NomeFonte;
            private DateTime? m_DataNascita;
            private Anagrafica.CIndirizzo m_NatoA;
            private Anagrafica.CIndirizzo m_ResidenteA;
            private string[] m_Telefono;
            private string m_eMail;
            private int? m_Valutazione;
            private int m_ValutatoDaID;
            [NonSerialized] private CUser m_ValutatoDa;
            private string m_ValutatoDaNome;
            private DateTime? m_ValutatoIl;
            private string m_MotivoValutazione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Candidatura()
            {
                m_DataCandidatura = default;
                m_IDCandidato = 0;
                m_Candidato = null;
                m_NomeCandidato = "";
                m_IDCurriculum = 0;
                m_Curriculum = null;
                m_IDOfferta = 0;
                m_NomeOfferta = "";
                m_Offerta = null;
                m_IDCanale = 0;
                m_Canale = null;
                m_NomeCanale = "";
                m_TipoFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = "";
                m_DataNascita = default;
                m_NatoA = new Anagrafica.CIndirizzo();
                m_NatoA.Nome = "NatoA";
                m_ResidenteA = new Anagrafica.CIndirizzo();
                m_ResidenteA.Nome = "ResidenteA";
                m_Telefono = new string[2];
                m_Telefono[0] = "";
                m_Telefono[1] = "";
                m_eMail = "";
                m_Valutazione = default;
                m_ValutatoDaID = 0;
                m_ValutatoDa = null;
                m_ValutatoDaNome = "";
                m_ValutatoIl = default;
                m_MotivoValutazione = "";
            }

            /// <summary>
            /// Motivo valutazione
            /// </summary>
            public string MotivoValutazione
            {
                get
                {
                    return m_MotivoValutazione;
                }

                set
                {
                    string oldValue = m_MotivoValutazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoValutazione = value;
                    DoChanged("MotivoValutazione", value, oldValue);
                }
            }

            /// <summary>
            /// Esito valutazione
            /// </summary>
            public int? Valutazione
            {
                get
                {
                    return m_Valutazione;
                }

                set
                {
                    var oldValue = m_Valutazione;
                    if (oldValue == value == true)
                        return;
                    m_Valutazione = value;
                    DoChanged("Valutazione", value, oldValue);
                }
            }

            /// <summary>
            /// Valutado da
            /// </summary>
            public int ValutatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_ValutatoDa, m_ValutatoDaID);
                }

                set
                {
                    int oldValue = ValutatoDaID;
                    if (oldValue == value)
                        return;
                    m_ValutatoDaID = value;
                    m_ValutatoDa = null;
                    DoChanged("ValutatoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Valutato da
            /// </summary>
            public Sistema.CUser ValutatoDa
            {
                get
                {
                    if (m_ValutatoDa is null)
                        m_ValutatoDa = Sistema.Users.GetItemById(m_ValutatoDaID);
                    return m_ValutatoDa;
                }

                set
                {
                    var oldValue = m_ValutatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ValutatoDa = value;
                    m_ValutatoDaID = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_ValutatoDaNome = value.Nominativo;
                    DoChanged("ValutatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Nome valutato da
            /// </summary>
            public string ValutatoDaNome
            {
                get
                {
                    return m_ValutatoDaNome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ValutatoDaNome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ValutatoDaNome = value;
                    DoChanged("ValutatoDaNome", value, oldValue);
                }
            }

            /// <summary>
            /// Data di valutazione
            /// </summary>
            public DateTime? ValutatoIl
            {
                get
                {
                    return m_ValutatoIl;
                }

                set
                {
                    var oldValue = m_ValutatoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_ValutatoIl = value;
                    DoChanged("ValutatoIl", value, oldValue);
                }
            }

            /// <summary>
            /// Nato A
            /// </summary>
            public Anagrafica.CIndirizzo NatoA
            {
                get
                {
                    return m_NatoA;
                }
            }

            /// <summary>
            /// Residente A
            /// </summary>
            public Anagrafica.CIndirizzo ResidenteA
            {
                get
                {
                    return m_ResidenteA;
                }
            }

            /// <summary>
            /// Data di nascita
            /// </summary>
            public DateTime? DataNascita
            {
                get
                {
                    return m_DataNascita;
                }

                set
                {
                    var oldValue = m_DataNascita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataNascita = value;
                    DoChanged("DataNascita", value, oldValue);
                }
            }

            /// <summary>
            /// Telefono
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public string get_Telefono(string index)
            {
                return m_Telefono[DMD.Integers.ValueOf(index)];
            }

            /// <summary>
            /// Telefono
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            public void set_Telefono(string index, string value)
            {
                value = Sistema.Formats.ParsePhoneNumber(value);
                string oldValue = m_Telefono[DMD.Integers.ValueOf(index)];
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_Telefono[DMD.Integers.ValueOf(index)] = value;
                DoChanged("Telefono(" + index + ")", value, oldValue);
            }

            /// <summary>
            /// email
            /// </summary>
            public string eMail
            {
                get
                {
                    return m_eMail;
                }

                set
                {
                    string oldValue = m_eMail;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_eMail = value;
                    DoChanged("eMail", value, oldValue);
                }
            }

            /// <summary>
            /// Età
            /// </summary>
            /// <returns></returns>
            public int? Eta()
            {
                return Eta(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Età alla data specificata
            /// </summary>
            /// <param name="at"></param>
            /// <returns></returns>
            public int? Eta(DateTime at)
            {
                if (m_DataNascita.HasValue == false)
                    return default;
                return (int?)DMD.DateUtils.CalcolaEta(m_DataNascita, at);
            }

            /// <summary>
            /// Canale sorgente
            /// </summary>
            public int IDCanale
            {
                get
                {
                    return DBUtils.GetID(m_Canale, m_IDCanale);
                }

                set
                {
                    int oldValue = IDCanale;
                    if (oldValue == value)
                        return;
                    m_IDCanale = value;
                    m_Canale = null;
                    DoChanged("IDCanale", value, oldValue);
                }
            }

            /// <summary>
            /// Sorgente
            /// </summary>
            public Anagrafica.CCanale Canale
            {
                get
                {
                    if (m_Canale is null)
                        m_Canale = Anagrafica.Canali.GetItemById(m_IDCanale);
                    return m_Canale;
                }

                set
                {
                    var oldValue = m_Canale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Canale = value;
                    m_IDCanale = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeCanale = value.Nome;
                    DoChanged("Canale", value, oldValue);
                }
            }

            /// <summary>
            /// Nome della fonte
            /// </summary>
            public string NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCanale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCanale = value;
                    DoChanged("NomeCanale", value, oldValue);
                }
            }

            /// <summary>
            /// Tipo fonte
            /// </summary>
            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    m_Fonte = null;
                    m_IDFonte = 0;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

            /// <summary>
            /// ID fonte
            /// </summary>
            public int IDFonte
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Fonte, m_IDFonte);
                }

                set
                {
                    int oldValue = IDFonte;
                    if (oldValue == value)
                        return;
                    m_IDFonte = value;
                    m_Fonte = null;
                    DoChanged("IDFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Fonte
            /// </summary>
            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null)
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_TipoFonte, m_TipoFonte, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    var oldValue = m_Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeFonte = value.Nome;
                    DoChanged("Fonte", value, oldValue);
                }
            }

            /// <summary>
            /// Nome fonte
            /// </summary>
            public string NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFonte = value;
                    DoChanged("NomeFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Data di canditatura
            /// </summary>
            public DateTime? DataCandidatura
            {
                get
                {
                    return m_DataCandidatura;
                }

                set
                {
                    var oldValue = m_DataCandidatura;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataCandidatura = value;
                    DoChanged("DataCandidatura", value, oldValue);
                }
            }

            /// <summary>
            /// ID del candidato
            /// </summary>
            public int IDCandidato
            {
                get
                {
                    return DBUtils.GetID(m_Candidato, m_IDCandidato);
                }

                set
                {
                    int oldValue = IDCandidato;
                    if (oldValue == value)
                        return;
                    m_IDCandidato = value;
                    m_Candidato = null;
                    DoChanged("IDCandidato", value, oldValue);
                }
            }

            /// <summary>
            /// Candidato
            /// </summary>
            public Anagrafica.CPersona Candidato
            {
                get
                {
                    if (m_Candidato is null)
                        m_Candidato = Anagrafica.Persone.GetItemById(m_IDCandidato);
                    return m_Candidato;
                }

                set
                {
                    var oldValue = m_Candidato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Candidato = value;
                    m_IDCandidato = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_NomeCandidato = value.Nominativo;
                        m_DataNascita = value.DataNascita;
                        m_NatoA.CopyFrom(value.NatoA);
                        m_ResidenteA.CopyFrom(value.ResidenteA);
                        m_Telefono[0] = value.Telefono;
                        m_Telefono[1] = value.Cellulare;
                        m_eMail = value.eMail;
                    }

                    DoChanged("Candidato", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del candidato
            /// </summary>
            public string NomeCandidato
            {
                get
                {
                    return m_NomeCandidato;
                }

                set
                {
                    string oldValue = m_NomeCandidato;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCandidato = value;
                    DoChanged("NomeCandidato", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'offerta
            /// </summary>
            public int IDOfferta
            {
                get
                {
                    return DBUtils.GetID(m_Offerta, m_IDOfferta);
                }

                set
                {
                    int oldValue = IDOfferta;
                    if (oldValue == value)
                        return;
                    m_IDOfferta = value;
                    m_Offerta = null;
                    DoChanged("IDOfferta", value, oldValue);
                }
            }

            /// <summary>
            /// Offerta
            /// </summary>
            public OffertaDiLavoro Offerta
            {
                get
                {
                    if (m_Offerta is null)
                        m_Offerta = minidom.Office.OfferteDiLavoro.GetItemById(m_IDOfferta);
                    return m_Offerta;
                }

                set
                {
                    var oldValue = m_Offerta;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Offerta = value;
                    m_IDOfferta = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOfferta = value.NomeOfferta;
                    DoChanged("Offerta", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'offerta
            /// </summary>
            public string NomeOfferta
            {
                get
                {
                    return m_NomeOfferta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeOfferta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOfferta = value;
                    DoChanged("NomeOfferta", value, oldValue);
                }
            }

            /// <summary>
            /// Curriculum
            /// </summary>
            public int IDCurriculum
            {
                get
                {
                    return DBUtils.GetID(m_Curriculum, m_IDCurriculum);
                }

                set
                {
                    int oldValue = IDCurriculum;
                    if (oldValue == value)
                        return;
                    m_IDCurriculum = value;
                    m_Curriculum = null;
                    DoChanged("IDCurriculum", value, oldValue);
                }
            }

            /// <summary>
            /// Curriculum
            /// </summary>
            public Curriculum Curriculum
            {
                get
                {
                    if (m_Curriculum is null)
                        m_Curriculum = minidom.Office.Curricula.GetItemById(m_IDCurriculum);
                    return m_Curriculum;
                }

                set
                {
                    var oldValue = m_Curriculum;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Curriculum = value;
                    m_IDCurriculum = DBUtils.GetID(value, 0);
                    DoChanged("Curriculum", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(
                            "Candidatura di " , m_NomeCandidato , " del " , Sistema.Formats.FormatUserDateTime(m_DataCandidatura) , " per " , m_NomeOfferta
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDOfferta);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Candidatura) && this.Equals((Candidatura)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Candidatura obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataCandidatura, obj.m_DataCandidatura)
                    && DMD.Integers.EQ(this.m_IDCandidato, obj.m_IDCandidato)
                    && DMD.Strings.EQ(this.m_NomeCandidato, obj.m_NomeCandidato)
                    && DMD.Integers.EQ(this.m_IDCurriculum, obj.m_IDCurriculum)
                    && DMD.Integers.EQ(this.m_IDOfferta, obj.m_IDOfferta)
                    && DMD.Strings.EQ(this.m_NomeOfferta, obj.m_NomeOfferta)
                    && DMD.Integers.EQ(this.m_IDCanale, obj.m_IDCanale)
                    && DMD.Strings.EQ(this.m_NomeCanale, obj.m_NomeCanale)
                    && DMD.Strings.EQ(this.m_TipoFonte, obj.m_TipoFonte)
                    && DMD.Integers.EQ(this.m_IDFonte, obj.m_IDFonte)
                    && DMD.Strings.EQ(this.m_NomeFonte, obj.m_NomeFonte)
                    && DMD.DateUtils.EQ(this.m_DataNascita, obj.m_DataNascita)
                    && this.m_NatoA.Equals(obj.m_NatoA)
                    && this.m_ResidenteA.Equals(obj.m_ResidenteA)
                    && DMD.Arrays.EQ(this.m_Telefono, obj.m_Telefono)
                    && DMD.Strings.EQ(this.m_eMail, obj.m_eMail)
                    && DMD.Integers.EQ(this.m_Valutazione, obj.m_Valutazione)
                    && DMD.Integers.EQ(this.m_ValutatoDaID, obj.m_ValutatoDaID)
                    && DMD.Strings.EQ(this.m_ValutatoDaNome, obj.m_ValutatoDaNome)
                    && DMD.DateUtils.EQ(this.m_ValutatoIl, obj.m_ValutatoIl)
                    && DMD.Strings.EQ(this.m_MotivoValutazione, obj.m_MotivoValutazione)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Candidature;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeCandidature";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || m_NatoA.IsChanged() || m_ResidenteA.IsChanged();
            }

            /// <summary>
            /// Salova l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_NatoA.SetChanged(false);
                m_ResidenteA.SetChanged(false);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_DataCandidatura = reader.Read("DataCandidatura",  m_DataCandidatura);
                m_IDCandidato = reader.Read("IDCandidato",  m_IDCandidato);
                m_NomeCandidato = reader.Read("NomeCandidato",  m_NomeCandidato);
                m_IDCurriculum = reader.Read("IDCurriculum",  m_IDCurriculum);
                m_IDOfferta = reader.Read("IDOfferta",  m_IDOfferta);
                m_NomeOfferta = reader.Read("NomeOfferta",  m_NomeOfferta);
                m_IDCanale = reader.Read("IDCanale",  m_IDCanale);
                m_NomeCanale = reader.Read("NomeCanale",  m_NomeCanale);
                m_TipoFonte = reader.Read("TipoFonte",  m_TipoFonte);
                m_IDFonte = reader.Read("IDFonte",  m_IDFonte);
                m_NomeFonte = reader.Read("NomeFonte",  m_NomeFonte);
                m_DataNascita = reader.Read("DataNascita",  m_DataNascita);
                m_NatoA.Citta = reader.Read("NatoA_Comune", m_NatoA.Citta);
                m_NatoA.Provincia = reader.Read("NatoA_Provincia", m_NatoA.Provincia);
                m_NatoA.SetChanged(false);
                m_ResidenteA.Citta = reader.Read("ResidenteA_Citta", m_ResidenteA.Citta);
                m_ResidenteA.Provincia = reader.Read("ResidenteA_Provincia", m_ResidenteA.Provincia);
                m_ResidenteA.CAP = reader.Read("ResidenteA_CAP", m_ResidenteA.CAP);
                m_ResidenteA.ToponimoEVia = reader.Read("ResidenteA_Via", m_ResidenteA.ToponimoEVia);
                m_ResidenteA.Civico = reader.Read("ResidenteA_Civico", m_ResidenteA.Civico);
                m_ResidenteA.SetChanged(false);
                m_Telefono[0] = reader.Read("TelefonoN1",  m_Telefono[0]);
                m_Telefono[1] = reader.Read("TelefonoN2",  m_Telefono[1]);
                m_eMail = reader.Read("eMail1",  m_eMail);
                m_Valutazione = reader.Read("Valutazione",  m_Valutazione);
                m_ValutatoDaID = reader.Read("ValutatoDaID",  m_ValutatoDaID);
                m_ValutatoDaNome = reader.Read("ValutatoDaNome",  m_ValutatoDaNome);
                m_ValutatoIl = reader.Read("ValutatoIl",  m_ValutatoIl);
                m_MotivoValutazione = reader.Read("MotivoValutazione",  m_MotivoValutazione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataCandidatura", m_DataCandidatura);
                writer.Write("IDCandidato", IDCandidato);
                writer.Write("NomeCandidato", m_NomeCandidato);
                writer.Write("IDCurriculum", IDCurriculum);
                writer.Write("IDOfferta", IDOfferta);
                writer.Write("NomeOfferta", m_NomeOfferta);
                writer.Write("IDCanale", IDCanale);
                writer.Write("NomeCanale", m_NomeCanale);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("NomeFonte", m_NomeFonte);
                writer.Write("DataNascita", m_DataNascita);
                writer.Write("NatoA_Comune", m_NatoA.Citta);
                writer.Write("NatoA_Provincia", m_NatoA.Provincia);
                writer.Write("ResidenteA_Citta", m_ResidenteA.Citta);
                writer.Write("ResidenteA_Provincia", m_ResidenteA.Provincia);
                writer.Write("ResidenteA_CAP", m_ResidenteA.CAP);
                writer.Write("ResidenteA_Via", m_ResidenteA.ToponimoEVia);
                writer.Write("ResidenteA_Civico", m_ResidenteA.Civico);
                writer.Write("TelefonoN1", m_Telefono[0]);
                writer.Write("TelefonoN2", m_Telefono[1]);
                writer.Write("eMail1", m_eMail);
                writer.Write("Valutazione", m_Valutazione);
                writer.Write("ValutatoDaID", ValutatoDaID);
                writer.Write("ValutatoDaNome", m_ValutatoDaNome);
                writer.Write("ValutatoIl", m_ValutatoIl);
                writer.Write("MotivoValutazione", m_MotivoValutazione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataCandidatura", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDCandidato", typeof(int), 1);
                c = table.Fields.Ensure("NomeCandidato", typeof(string), 255);
                c = table.Fields.Ensure("IDCurriculum", typeof(int), 1);
                c = table.Fields.Ensure("IDOfferta", typeof(int), 1);
                c = table.Fields.Ensure("NomeOfferta", typeof(string), 255);
                c = table.Fields.Ensure("IDCanale", typeof(int), 1);
                c = table.Fields.Ensure("NomeCanale", typeof(string), 255);
                c = table.Fields.Ensure("TipoFonte", typeof(string), 255);
                c = table.Fields.Ensure("IDFonte", typeof(int), 1);
                c = table.Fields.Ensure("NomeFonte", typeof(string), 255);
                c = table.Fields.Ensure("DataNascita", typeof(DateTime), 1);
                c = table.Fields.Ensure("NatoA_Comune", typeof(string), 255);
                c = table.Fields.Ensure("NatoA_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("ResidenteA_Citta", typeof(string), 255);
                c = table.Fields.Ensure("ResidenteA_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("ResidenteA_CAP", typeof(string), 255);
                c = table.Fields.Ensure("ResidenteA_Via", typeof(string), 255);
                c = table.Fields.Ensure("ResidenteA_Civico", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoN1", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoN2", typeof(string), 255);
                c = table.Fields.Ensure("eMail1", typeof(string), 255);
                c = table.Fields.Ensure("Valutazione", typeof(int), 1);
                c = table.Fields.Ensure("ValutatoDaID", typeof(int), 1);
                c = table.Fields.Ensure("ValutatoDaNome", typeof(string), 255);
                c = table.Fields.Ensure("ValutatoIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("MotivoValutazione", typeof(string), 0);
                 

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxData", new string[] { "DataCandidatura", "IDCandidato", "IDOfferta", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxNomi", new string[] { "NomeCandidato", "NomeOfferta", "NomeCanale", "NomeFonte" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFonte", new string[] { "IDFonte", "TipoFonte", "IDCanale" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri", new string[] { "DataNascita", "IDCurriculum" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNatoA", new string[] { "NatoA_Comune", "NatoA_Provincia" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxResidenteA", new string[] { "ResidenteA_Citta", "ResidenteA_Provincia", "ResidenteA_CAP", "ResidenteA_Via", "ResidenteA_Civico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTelefoni", new string[] { "TelefonoN1", "TelefonoN2", "eMail1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValutazione", new string[] { "Valutazione", "MotivoValutazione"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValutatoDa", new string[] { "ValutatoDaID", "ValutatoIl", "ValutatoDaNome" }, DBFieldConstraintFlags.None);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataCandidatura", m_DataCandidatura);
                writer.WriteAttribute("IDCandidato", IDCandidato);
                writer.WriteAttribute("NomeCandidato", m_NomeCandidato);
                writer.WriteAttribute("IDCurriculum", IDCurriculum);
                writer.WriteAttribute("IDOfferta", IDOfferta);
                writer.WriteAttribute("NomeOfferta", m_NomeOfferta);
                writer.WriteAttribute("IDCanale", IDCanale);
                writer.WriteAttribute("NomeCanale", m_NomeCanale);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("NomeFonte", m_NomeFonte);
                writer.WriteAttribute("DataNascita", m_DataNascita);
                writer.WriteAttribute("eMail1", m_eMail);
                writer.WriteAttribute("Valutazione", m_Valutazione);
                writer.WriteAttribute("ValutatoDaID", ValutatoDaID);
                writer.WriteAttribute("ValutatoDaNome", m_ValutatoDaNome);
                writer.WriteAttribute("ValutatoIl", m_ValutatoIl);
                base.XMLSerialize(writer);
                writer.WriteTag("NatoA", m_NatoA);
                writer.WriteTag("ResidenteA", m_ResidenteA);
                writer.WriteTag("Telefono", m_Telefono);
                writer.WriteTag("MotivoValutazione", m_MotivoValutazione);
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
                    case "DataCandidatura":
                        {
                            m_DataCandidatura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDCandidato":
                        {
                            m_IDCandidato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCandidato":
                        {
                            m_NomeCandidato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCurriculum":
                        {
                            m_IDCurriculum = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOfferta":
                        {
                            m_IDOfferta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOfferta":
                        {
                            m_NomeOfferta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCanale":
                        {
                            m_IDCanale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCanale":
                        {
                            m_NomeCanale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            m_TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            m_IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeFonte":
                        {
                            m_NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataNascita":
                        {
                            m_DataNascita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "eMail1":
                        {
                            m_eMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valutazione":
                        {
                            m_Valutazione = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValutatoDaID":
                        {
                            m_ValutatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValutatoDaNome":
                        {
                            m_ValutatoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValutatoIl":
                        {
                            m_ValutatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Telefono":
                        {
                            m_Telefono = (string[])DMD.Arrays.Convert<string>(fieldValue);
                            break;
                        }

                    case "NatoA":
                        {
                            m_NatoA = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "ResidenteA":
                        {
                            m_ResidenteA = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "MotivoValutazione":
                        {
                            m_MotivoValutazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}