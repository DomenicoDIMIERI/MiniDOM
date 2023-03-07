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
using static minidom.Contabilita;

namespace minidom
{
    public partial class Contabilita
    {

        /// <summary>
        /// Stato del documento contabile
        /// </summary>
        public enum StatoDocumentoContabile : int
        {
            /// <summary>
            /// Registrato (da emettere)
            /// </summary>
            Registrato = 0,

            /// <summary>
            /// Documento emesso
            /// </summary>
            Emesso = 1,

            /// <summary>
            /// Documento evase
            /// </summary>
            Evaso = 2
        }

        /// <summary>
        /// Rappresenta un documento contabile
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class DocumentoContabile 
            : minidom.Databases.DBObjectPO
        {
            private string m_TipoDocumento;
            private string m_NumeroDocumento;
            private DateTime? m_DataRegistrazione;
            private DateTime? m_DataEmissione;
            private DateTime? m_DataEvasione;
            [NonSerialized] private StatoDocumentoContabile m_StatoDocumento;
            private int m_IDCliente;
            [NonSerialized] private Anagrafica.CPersona m_Cliente;
            private string m_NomeCliente;
            private Anagrafica.CIndirizzo m_IndirizzoCliente;
            private string m_CodiceFiscaleCliente;
            private string m_PartitaIVACliente;
            private int m_IDFornitore;
            [NonSerialized] private Anagrafica.CPersona m_Fornitore;
            private string m_NomeFornitore;
            private Anagrafica.CIndirizzo m_IndirizzoFornitore;
            private string m_CodiceFiscaleFornitore;
            private string m_PartitaIVAFornitore;
            private Anagrafica.CIndirizzo m_IndirizzoSpedizione;

            // Private m_RiferimentoADocumenti As RiferimentoADocumentiCollection
            private string m_Descrizione;
            private decimal? m_TotaleImponibile;
            private decimal? m_TotaleIvato;
            private VociDiPagamentoPerDocumento m_VociPagamento;
            [NonSerialized] private object m_Source;
            private string m_SourceType;
            private int m_SourceID;
            private string m_SourceParams;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentoContabile()
            {
                m_TipoDocumento = "";
                m_NumeroDocumento = "";
                m_DataRegistrazione = default;
                m_DataEmissione = default;
                m_DataEvasione = default;
                m_StatoDocumento = StatoDocumentoContabile.Registrato;
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_IndirizzoCliente = new Anagrafica.CIndirizzo();
                m_CodiceFiscaleCliente = "";
                m_PartitaIVACliente = "";
                m_IDFornitore = 0;
                m_Fornitore = null;
                m_NomeFornitore = "";
                m_IndirizzoFornitore = new Anagrafica.CIndirizzo();
                m_CodiceFiscaleFornitore = "";
                m_PartitaIVAFornitore = "";
                m_Descrizione = "";
                m_IndirizzoSpedizione = new Anagrafica.CIndirizzo();
                m_TotaleImponibile = default;
                m_TotaleIvato = default;
                m_VociPagamento = null;
                m_Source = null;
                m_SourceType = "";
                m_SourceID = 0;
                m_SourceParams = "";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.TipoDocumento, "[", this.m_NumeroDocumento , "] del ", this.m_DataEmissione , " (", this.m_NomeCliente , ")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NumeroDocumento);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is DocumentoContabile) && this.Equals((DocumentoContabile)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(DocumentoContabile obj)
            {
                return base.Equals(obj)
                        && DMD.Strings.EQ(this.m_TipoDocumento, obj.m_TipoDocumento)
                        && DMD.Strings.EQ(this.m_NumeroDocumento, obj.m_NumeroDocumento)
                        && DMD.DateUtils.EQ(this.m_DataRegistrazione, obj.m_DataRegistrazione)
                        && DMD.DateUtils.EQ(this.m_DataEmissione, obj.m_DataEmissione)
                        && DMD.DateUtils.EQ(this.m_DataEvasione, obj.m_DataEvasione)
                        && DMD.Integers.EQ((int)this.m_StatoDocumento, (int)obj.m_StatoDocumento)
                        && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                        && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                        && this.m_IndirizzoCliente.Equals(obj.m_IndirizzoCliente)
                        && DMD.Strings.EQ(this.m_CodiceFiscaleCliente, obj.m_CodiceFiscaleCliente)
                        && DMD.Strings.EQ(this.m_PartitaIVACliente, obj.m_PartitaIVACliente)
                        && DMD.Integers.EQ(this.m_IDFornitore, obj.m_IDFornitore)
                        && DMD.Strings.EQ(this.m_NomeFornitore, obj.m_NomeFornitore)
                        && this.m_IndirizzoFornitore.Equals(obj.m_IndirizzoFornitore)
                        && DMD.Strings.EQ(this.m_CodiceFiscaleFornitore, obj.m_CodiceFiscaleFornitore)
                        && DMD.Strings.EQ(this.m_PartitaIVAFornitore, obj.m_PartitaIVAFornitore)
                        && this.m_IndirizzoSpedizione.Equals(obj.m_IndirizzoSpedizione)
                        && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                        && DMD.Decimals.EQ(this.m_TotaleImponibile, obj.m_TotaleImponibile)
                        && DMD.Decimals.EQ(this.m_TotaleIvato, obj.m_TotaleIvato)
                        && DMD.Strings.EQ(this.m_SourceType, obj.m_SourceType)
                        && DMD.Integers.EQ(this.m_SourceID, obj.m_SourceID)
                        && DMD.Strings.EQ(this.m_SourceParams, obj.m_SourceParams)
                        // m_VociPagamento;
                        ;
            }

            /// <summary>
            /// Restituisce o imposta il documento o l'oggetto che ha causato la movimentazione di denaro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object Source
            {
                get
                {
                    if (m_Source is null)
                        m_Source = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_SourceType, m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_SourceType = "";
                    if (value is object)
                        m_SourceType = DMD.RunTime.vbTypeName(value);
                    m_SourceID = DBUtils.GetID(value, 0);
                    DoChanged("Source", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la sorgente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetSource(object value)
            {
                m_Source = value;
                m_SourceID = DBUtils.GetID(value, 0);
                m_SourceType = "";
                if (value is object)
                    m_SourceType = DMD.RunTime.vbTypeName(value);
            }

            /// <summary>
        /// Restituisec o imposta il tipo del documento o l'oggetto che ha causato la movimentazione di denaro
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string SourceType
            {
                get
                {
                    return m_SourceType;
                }

                set
                {
                    string oldValue = m_SourceType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceType = value;
                    m_Source = null;
                    DoChanged("SourceType", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del documento o dell'oggetto che ha causato la movimentazione di denaro
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei parametri aggiuntivi per il documento o l'oggetto che ha causato la movimentazione di denaro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceParams
            {
                get
                {
                    return m_SourceParams;
                }

                set
                {
                    string oldValue = m_SourceParams;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceParams = value;
                    DoChanged("SourceParams", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta i parametri sorgente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetSourceParams(string value)
            {
                m_SourceParams = value;
            }

            /// <summary>
            /// Restituisce o imposta il tipo del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoDocumento
            {
                get
                {
                    return m_TipoDocumento;
                }

                set
                {
                    string oldValue = m_TipoDocumento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoDocumento = value;
                    DoChanged("TipoDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NumeroDocumento
            {
                get
                {
                    return m_NumeroDocumento;
                }

                set
                {
                    string oldValue = m_NumeroDocumento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroDocumento = value;
                    DoChanged("NumeroDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di registrazione del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataRegistrazione
            {
                get
                {
                    return m_DataRegistrazione;
                }

                set
                {
                    var oldValue = m_DataRegistrazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRegistrazione = value;
                    DoChanged("DataRegistrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di emissione del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataEmissione
            {
                get
                {
                    return m_DataEmissione;
                }

                set
                {
                    var oldValue = m_DataEmissione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEmissione = value;
                    DoChanged("DataEmissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di evasione del documento (es. per gli ordini)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataEvasione
            {
                get
                {
                    return m_DataEvasione;
                }

                set
                {
                    var oldValue = m_DataEvasione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEvasione = value;
                    DoChanged("DataEvasione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoDocumentoContabile StatoDocumento
            {
                get
                {
                    return m_StatoDocumento;
                }

                set
                {
                    var oldValue = m_StatoDocumento;
                    if (oldValue == value)
                        return;
                    m_StatoDocumento = value;
                    DoChanged("StatoDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_NomeCliente = value.Nominativo;
                    }
                    else
                    {
                        m_NomeCliente = "";
                    }

                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    string oldValue = m_NomeCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'indirizzo del cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoCliente
            {
                get
                {
                    return m_IndirizzoCliente;
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice fiscale del cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceFiscaleCliente
            {
                get
                {
                    return m_CodiceFiscaleCliente;
                }

                set
                {
                    string oldValue = m_CodiceFiscaleCliente;
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscaleCliente = value;
                    DoChanged("CodiceFiscaleCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la partita IVA del cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string PartitaIVACliente
            {
                get
                {
                    return m_PartitaIVACliente;
                }

                set
                {
                    string oldValue = m_PartitaIVACliente;
                    value = Sistema.Formats.ParsePartitaIVA(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PartitaIVACliente = value;
                    DoChanged("PartitaIVACliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDFornitore
            {
                get
                {
                    return DBUtils.GetID(m_Fornitore, m_IDFornitore);
                }

                set
                {
                    int oldValue = IDFornitore;
                    if (oldValue == value)
                        return;
                    m_IDFornitore = value;
                    m_Fornitore = null;
                    DoChanged("IDFornitore", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il Fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Fornitore
            {
                get
                {
                    if (m_Fornitore is null)
                        m_Fornitore = Anagrafica.Persone.GetItemById(m_IDFornitore);
                    return m_Fornitore;
                }

                set
                {
                    var oldValue = m_Fornitore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fornitore = value;
                    m_IDFornitore = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_NomeFornitore = value.Nominativo;
                    }
                    else
                    {
                        m_NomeFornitore = "";
                    }

                    DoChanged("Fornitore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del Fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeFornitore
            {
                get
                {
                    return m_NomeFornitore;
                }

                set
                {
                    string oldValue = m_NomeFornitore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFornitore = value;
                    DoChanged("NomeFornitore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'indirizzo del Fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoFornitore
            {
                get
                {
                    return m_IndirizzoFornitore;
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice fiscale del Fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceFiscaleFornitore
            {
                get
                {
                    return m_CodiceFiscaleFornitore;
                }

                set
                {
                    string oldValue = m_CodiceFiscaleFornitore;
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscaleFornitore = value;
                    DoChanged("CodiceFiscaleFornitore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la partita IVA del Fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string PartitaIVAFornitore
            {
                get
                {
                    return m_PartitaIVAFornitore;
                }

                set
                {
                    string oldValue = m_PartitaIVAFornitore;
                    value = Sistema.Formats.ParsePartitaIVA(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PartitaIVAFornitore = value;
                    DoChanged("PartitaIVAFornitore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'indirizzo di spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoSpedizione
            {
                get
                {
                    return m_IndirizzoSpedizione;
                }
            }

            /// <summary>
            /// Restituisce o imposta il campo descrizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

          

            /// <summary>
            /// Restituisce o imposta il totale imponibile del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? TotaleImponibile
            {
                get
                {
                    return m_TotaleImponibile;
                }

                set
                {
                    var oldValue = m_TotaleImponibile;
                    if (oldValue == value == true)
                        return;
                    m_TotaleImponibile = value;
                    DoChanged("TotaleImponibile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il totale ivato del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? TotaleIvato
            {
                get
                {
                    return m_TotaleIvato;
                }

                set
                {
                    var oldValue = m_TotaleIvato;
                    if (oldValue == value == true)
                        return;
                    m_TotaleIvato = value;
                    DoChanged("TotaleIvato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elenco delle movimentazioni di denaro causate da questo documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public VociDiPagamentoPerDocumento VociDiPagamento
            {
                get
                {
                    if (m_VociPagamento is null)
                        m_VociPagamento = new VociDiPagamentoPerDocumento(this);
                    return m_VociPagamento;
                }
            }

         
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Contabilita.DocumentiContabili;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeDocumentiContabili";
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (this.m_VociPagamento is object)
                    m_VociPagamento.Save();
            }

            /// <summary>
            /// After Save
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                m_IndirizzoCliente.SetChanged(false);
                m_IndirizzoFornitore.SetChanged(false);
                m_IndirizzoSpedizione.SetChanged(false);
                if (m_VociPagamento is object)
                    m_VociPagamento.SetChanged(false);
                base.OnAfterSave(e);
            }

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return    base.IsChanged() 
                       || m_IndirizzoCliente.IsChanged() 
                       || m_IndirizzoFornitore.IsChanged() 
                       || m_IndirizzoSpedizione.IsChanged() 
                       || (m_VociPagamento is object && m_VociPagamento.IsChanged());
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_TipoDocumento = reader.Read("TipoDocumento", m_TipoDocumento);
                m_NumeroDocumento = reader.Read("NumeroDocumento", m_NumeroDocumento);
                m_DataRegistrazione = reader.Read("DataRegistrazione", m_DataRegistrazione);
                m_DataEmissione = reader.Read("DataEmissione", m_DataEmissione);
                m_DataEvasione = reader.Read("DataEvasione", m_DataEvasione);
                m_StatoDocumento = reader.Read("StatoDocumento", m_StatoDocumento);
                m_TotaleImponibile = reader.Read("TotaleImponibile", m_TotaleImponibile);
                m_TotaleIvato = reader.Read("TotaleIvato", m_TotaleIvato);
                m_IDCliente = reader.Read("IDCliente", m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                {
                    var withBlock = m_IndirizzoCliente;
                    withBlock.Nome = reader.Read("INDCLT_LABEL", withBlock.Nome);
                    withBlock.ToponimoViaECivico = reader.Read("INDCLT_VIA", withBlock.ToponimoViaECivico);
                    withBlock.CAP = reader.Read("INDCLT_CAP", withBlock.CAP);
                    withBlock.Citta = reader.Read("INDCLT_CITTA", withBlock.Citta);
                    withBlock.Provincia = reader.Read("INDCLT_PROV", withBlock.Provincia);
                    withBlock.SetChanged(false);
                }

                m_CodiceFiscaleCliente = reader.Read("CodiceFiscaleCliente", m_CodiceFiscaleCliente);
                m_PartitaIVACliente = reader.Read("PartitaIVACliente", m_PartitaIVACliente);
                m_IDFornitore = reader.Read("IDFornitore", m_IDFornitore);
                m_NomeFornitore = reader.Read("NomeFornitore", m_NomeFornitore);
                {
                    var withBlock1 = m_IndirizzoFornitore;
                    withBlock1.Nome = reader.Read("INDFNT_LABEL", withBlock1.Nome);
                    withBlock1.ToponimoViaECivico = reader.Read("INDFNT_VIA", withBlock1.ToponimoViaECivico);
                    withBlock1.CAP = reader.Read("INDFNT_CAP", withBlock1.CAP);
                    withBlock1.Citta = reader.Read("INDFNT_CITTA", withBlock1.Citta);
                    withBlock1.Provincia = reader.Read("INDFNT_PROV", withBlock1.Provincia);
                    withBlock1.SetChanged(false);
                }

                m_CodiceFiscaleFornitore = reader.Read("CodiceFiscaleFornitore", m_CodiceFiscaleFornitore);
                m_PartitaIVAFornitore = reader.Read("PartitaIVAFornitore", m_PartitaIVAFornitore);
                {
                    var withBlock2 = m_IndirizzoSpedizione;
                    withBlock2.Nome = reader.Read("INDSPD_LABEL", withBlock2.Nome);
                    withBlock2.ToponimoViaECivico = reader.Read("INDSPD_VIA", withBlock2.ToponimoViaECivico);
                    withBlock2.CAP = reader.Read("INDSPD_CAP", withBlock2.CAP);
                    withBlock2.Citta = reader.Read("INDSPD_CITTA", withBlock2.Citta);
                    withBlock2.Provincia = reader.Read("INDSPD_PROV", withBlock2.Provincia);
                    withBlock2.SetChanged(false);
                }

                // Private m_RiferimentoADocumenti As RiferimentoADocumentiCollection
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_SourceType = reader.Read("SourceType", m_SourceType);
                m_SourceID = reader.Read("SourceID", m_SourceID);
                m_SourceParams = reader.Read("SourceParams", m_SourceParams);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("TipoDocumento", m_TipoDocumento);
                writer.Write("NumeroDocumento", m_NumeroDocumento);
                writer.Write("DataRegistrazione", m_DataRegistrazione);
                writer.Write("DataEmissione", m_DataEmissione);
                writer.Write("DataEvasione", m_DataEvasione);
                writer.Write("StatoDocumento", m_StatoDocumento);
                writer.Write("TotaleImponibile", m_TotaleImponibile);
                writer.Write("TotaleIvato", m_TotaleIvato);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                {
                    var withBlock = m_IndirizzoCliente;
                    writer.Write("INDCLT_LABEL", withBlock.Nome);
                    writer.Write("INDCLT_VIA", withBlock.ToponimoViaECivico);
                    writer.Write("INDCLT_CAP", withBlock.CAP);
                    writer.Write("INDCLT_CITTA", withBlock.Citta);
                    writer.Write("INDCLT_PROV", withBlock.Provincia);
                }

                writer.Write("CodiceFiscaleCliente", m_CodiceFiscaleCliente);
                writer.Write("PartitaIVACliente", m_PartitaIVACliente);
                writer.Write("IDFornitore", IDFornitore);
                writer.Write("NomeFornitore", m_NomeFornitore);
                {
                    var withBlock1 = m_IndirizzoFornitore;
                    writer.Write("INDFNT_LABEL", withBlock1.Nome);
                    writer.Write("INDFNT_VIA", withBlock1.ToponimoViaECivico);
                    writer.Write("INDFNT_CAP", withBlock1.CAP);
                    writer.Write("INDFNT_CITTA", withBlock1.Citta);
                    writer.Write("INDFNT_PROV", withBlock1.Provincia);
                }

                writer.Write("CodiceFiscaleFornitore", m_CodiceFiscaleFornitore);
                writer.Write("PartitaIVAFornitore", m_PartitaIVAFornitore);
                {
                    var withBlock2 = m_IndirizzoSpedizione;
                    writer.Write("INDSPD_LABEL", withBlock2.Nome);
                    writer.Write("INDSPD_VIA", withBlock2.ToponimoViaECivico);
                    writer.Write("INDSPD_CAP", withBlock2.CAP);
                    writer.Write("INDSPD_CITTA", withBlock2.Citta);
                    writer.Write("INDSPD_PROV", withBlock2.Provincia);
                }

                // Private m_RiferimentoADocumenti As RiferimentoADocumentiCollection
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("SourceType", m_SourceType);
                writer.Write("SourceID", SourceID);
                writer.Write("SourceParams", m_SourceParams);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("TipoDocumento", typeof(string), 255);
                c = table.Fields.Ensure("NumeroDocumento", typeof(string), 255);
                c = table.Fields.Ensure("DataRegistrazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataEmissione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataEvasione", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoDocumento", typeof(int), 1);
                c = table.Fields.Ensure("TotaleImponibile", typeof(decimal), 1);
                c = table.Fields.Ensure("TotaleIvato", typeof(decimal), 1);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("INDCLT_LABEL", typeof(string), 255);
                c = table.Fields.Ensure("INDCLT_VIA", typeof(string), 255);
                c = table.Fields.Ensure("INDCLT_CAP", typeof(string), 255);
                c = table.Fields.Ensure("INDCLT_CITTA", typeof(string), 255);
                c = table.Fields.Ensure("INDCLT_PROV", typeof(string), 255);
                c = table.Fields.Ensure("CodiceFiscaleCliente", typeof(string), 255);
                c = table.Fields.Ensure("PartitaIVACliente", typeof(string), 255);
                c = table.Fields.Ensure("IDFornitore", typeof(int), 1);
                c = table.Fields.Ensure("NomeFornitore", typeof(string), 255);
                c = table.Fields.Ensure("INDFNT_LABEL", typeof(string), 255);
                c = table.Fields.Ensure("INDFNT_VIA", typeof(string), 255);
                c = table.Fields.Ensure("INDFNT_CAP", typeof(string), 255);
                c = table.Fields.Ensure("INDFNT_CITTA", typeof(string), 255);
                c = table.Fields.Ensure("INDFNT_PROV", typeof(string), 255);
                c = table.Fields.Ensure("CodiceFiscaleFornitore", typeof(string), 255);
                c = table.Fields.Ensure("PartitaIVAFornitore", typeof(string), 255);
                c = table.Fields.Ensure("INDSPD_LABEL", typeof(string), 255);
                c = table.Fields.Ensure("INDSPD_VIA", typeof(string), 255);
                c = table.Fields.Ensure("INDSPD_CAP", typeof(string), 255);
                c = table.Fields.Ensure("INDSPD_CITTA", typeof(string), 255);
                c = table.Fields.Ensure("INDSPD_PROV", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("SourceType", typeof(string), 255);
                c = table.Fields.Ensure("SourceID", typeof(int), 1);
                c = table.Fields.Ensure("SourceParams", typeof(string), 255);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxTipoDoc", new string[] { "TipoDocumento", "NumeroDocumento", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDate", new string[] { "StatoDocumento", "DataRegistrazione", "DataEmissione", "DataEvasione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "CodiceFiscaleCliente", "PartitaIVACliente", "NomeCliente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatistiche", new string[] { "TotaleImponibile", "TotaleIvato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndCliente", new string[] { "INDCLT_CAP", "INDCLT_PROV", "INDCLT_CITTA", "INDCLT_VIA", "INDCLT_LABEL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFornitore", new string[] { "IDFornitore", "CodiceFiscaleFornitore", "PartitaIVAFornitore", "NomeFornitore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndFornitore", new string[] { "INDFNT_CAP", "INDFNT_PROV", "INDFNT_CITTA", "INDFNT_VIA", "INDFNT_LABEL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndSpedizione", new string[] { "INDSPD_CAP", "INDSPD_PROV", "INDSPD_CITTA", "INDSPD_VIA", "INDSPD_LABEL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceType", "SourceID", "SourceParams" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("TipoDocumento", m_TipoDocumento);
                writer.WriteAttribute("NumeroDocumento", m_NumeroDocumento);
                writer.WriteAttribute("DataRegistrazione", m_DataRegistrazione);
                writer.WriteAttribute("DataEmissione", m_DataEmissione);
                writer.WriteAttribute("DataEvasione", m_DataEvasione);
                writer.WriteAttribute("StatoDocumento", (int?)m_StatoDocumento);
                writer.WriteAttribute("TotaleImponibile", m_TotaleImponibile);
                writer.WriteAttribute("TotaleIvato", m_TotaleIvato);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("CodiceFiscaleCliente", m_CodiceFiscaleCliente);
                writer.WriteAttribute("PartitaIVACliente", m_PartitaIVACliente);
                writer.WriteAttribute("IDFornitore", IDFornitore);
                writer.WriteAttribute("NomeFornitore", m_NomeFornitore);
                writer.WriteAttribute("CodiceFiscaleFornitore", m_CodiceFiscaleFornitore);
                writer.WriteAttribute("PartitaIVAFornitore", m_PartitaIVAFornitore);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("SourceType", m_SourceType);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("SourceParams", m_SourceParams);
                base.XMLSerialize(writer);
                writer.WriteTag("INDCLT", IndirizzoCliente);
                writer.WriteTag("INDFNT", IndirizzoFornitore);
                writer.WriteTag("INDSPD", IndirizzoSpedizione);
                writer.WriteTag("VociDiPagamento", VociDiPagamento);
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
                    case "TipoDocumento":
                        {
                            m_TipoDocumento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroDocumento":
                        {
                            m_NumeroDocumento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRegistrazione":
                        {
                            m_DataRegistrazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataEmissione":
                        {
                            m_DataEmissione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataEvasione":
                        {
                            m_DataEvasione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoDocumento":
                        {
                            m_StatoDocumento = (StatoDocumentoContabile)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TotaleImponibile":
                        {
                            m_TotaleImponibile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TotaleIvato":
                        {
                            m_TotaleIvato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceFiscaleCliente":
                        {
                            m_CodiceFiscaleCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PartitaIVACliente":
                        {
                            m_PartitaIVACliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFornitore":
                        {
                            m_IDFornitore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeFornitore":
                        {
                            m_NomeFornitore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceFiscaleFornitore":
                        {
                            m_CodiceFiscaleFornitore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PartitaIVAFornitore":
                        {
                            m_PartitaIVAFornitore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 

                    case "INDCLT":
                        {
                            m_IndirizzoCliente = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "INDFNT":
                        {
                            m_IndirizzoFornitore = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "INDSPD":
                        {
                            m_IndirizzoSpedizione = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }
 
                    case "VociDiPagamento":
                        {
                            m_VociPagamento = (VociDiPagamentoPerDocumento)fieldValue;
                            m_VociPagamento.SetDocumento(this);
                            break;
                        }

                    case "SourceType":
                        {
                            m_SourceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SourceParams":
                        {
                            m_SourceParams = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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