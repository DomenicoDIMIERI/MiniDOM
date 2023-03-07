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
        /// Rappresenta il tipo di periodicità delle bollette generate dall'utenza
        /// </summary>
        /// <remarks></remarks>
        public enum PeriodicitaUtenza : int
        {
            /// <summary>
            /// Nessuna periodicità: le bollette non vengono generate in automatico
            /// </summary>
            /// <remarks></remarks>
            Nessuna = 0,

            /// <summary>
            /// Viene generata una bolletta ogni n - giorni
            /// </summary>
            /// <remarks></remarks>
            Giornaliera = 1,

            /// <summary>
            /// Viene generata una bolletta ogni n - settimane
            /// </summary>
            /// <remarks></remarks>
            Settimanale = 2,

            /// <summary>
            /// Viene generata una bolletta ogni n - mesi
            /// </summary>
            /// <remarks></remarks>
            Mensile = 3,

            /// <summary>
            /// Viene generata una bolletta ogni n - bimestri
            /// </summary>
            /// <remarks></remarks>
            Bimestrale = 4,

            /// <summary>
            /// Viene generata una bolletta ogni n - trimestri
            /// </summary>
            /// <remarks></remarks>
            Trimestrale = 5,

            /// <summary>
            /// Viene generata una bolletta ogni n - quadrimestri
            /// </summary>
            /// <remarks></remarks>
            Quadrimestrale = 6,

            /// <summary>
            /// Viene generata una bolletta ogni n - semstri
            /// </summary>
            /// <remarks></remarks>
            Semestrale = 7,

            /// <summary>
            /// Viene generata una bolletta ogni n - anni
            /// </summary>
            /// <remarks></remarks>
            Annuale = 8,

            /// <summary>
            /// Viene generata una bolletta il giorno n di ogni mese
            /// </summary>
            /// <remarks></remarks>
            IlGiornoXDelMese = 9,

            /// <summary>
            /// Viene generata una bolletta il giorno n di ogni anno
            /// </summary>
            /// <remarks></remarks>
            IlGiornoXDellAnno = 10
        }


        /// <summary>
        /// Flag validi per un'utenza
        /// </summary>
        [Flags]
        public enum UtenzaFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }


        /// <summary>
        /// Rappresenta una utenza
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Utenza 
            : Databases.DBObjectPO, Sistema.ISchedulable
        {
            private string m_Nome;
            private int m_IDFornitore;
            [NonSerialized] private Anagrafica.CPersona m_Fornitore;
            private string m_NomeFornitore;
            private int m_IDCliente;
            [NonSerialized] private Anagrafica.CPersona m_Cliente;
            private string m_NomeCliente;
            private string m_NumeroContratto;
            private string m_CodiceCliente;
            private string m_CodiceUtenza;
            private string m_Descrizione;
            private PeriodicitaUtenza m_TipoPeriodicita;
            private int m_IntervalloPeriodicita;
            private DateTime? m_DataSottoscrizione;
            private DateTime? m_DataInizioPeriodo;
            private DateTime? m_DataFinePeriodo;
            private string m_UnitaMisura;
            private decimal? m_CostoUnitario;
            private decimal? m_CostiFissi;
            private string m_NomeValuta;
            //private UtenzaFlags m_Flags;
            private object m_MetodoDiPagamento;
            private string m_TipoMetodoDiPagamento;
            private int m_IDMetodoDiPagamento;
            private string m_NomeMetodoDiPagamento;
            private string m_TipoUtenza;
            private string m_StimatoreBolletta;
            private MultipleScheduleCollection m_Programmazione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Utenza()
            {
                m_Nome = "";
                m_IDFornitore = 0;
                m_Fornitore = null;
                m_NomeFornitore = "";
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_NumeroContratto = "";
                m_CodiceCliente = "";
                m_CodiceUtenza = "";
                m_Descrizione = "";
                m_TipoPeriodicita = PeriodicitaUtenza.Mensile;
                m_IntervalloPeriodicita = 1;
                m_DataSottoscrizione = default;
                m_DataInizioPeriodo = default;
                m_DataFinePeriodo = default;
                m_UnitaMisura = "";
                m_CostoUnitario = default;
                m_CostiFissi = default;
                m_NomeValuta = "";
                m_Flags = (int) UtenzaFlags.None;
                m_MetodoDiPagamento = null;
                m_TipoMetodoDiPagamento = "";
                m_IDMetodoDiPagamento = 0;
                m_NomeMetodoDiPagamento = "";
                m_TipoUtenza = "";
                m_StimatoreBolletta = "";
                m_Programmazione = null;
            }

            /// <summary>
            /// Periodicità
            /// </summary>
            public MultipleScheduleCollection Programmazione
            {
                get
                {
                    lock (this)
                    {
                        if (m_Programmazione is null)
                            m_Programmazione = new Sistema.MultipleScheduleCollection(this);
                        return m_Programmazione;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome di una classe che viene utilizzata per stimare la prossima bolletta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string StimatoreBolletta
            {
                get
                {
                    return m_StimatoreBolletta;
                }

                set
                {
                    string oldValue = m_StimatoreBolletta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StimatoreBolletta = value;
                    DoChanged("StimatoreBolletta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    string oldValue = m_Nome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
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
            /// Restituisce o imposta il fornitore
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
                    var oldValue = Fornitore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fornitore = value;
                    m_IDFornitore = DBUtils.GetID(value, 0);
                    m_NomeFornitore = "";
                    if (value is object)
                        m_NomeFornitore = value.Nominativo;
                    DoChanged("Fornitore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del fornitore
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
            /// Restituisce o imposta l'ID del Cliente
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
            /// Restituisce o imposta il Cliente
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
                    var oldValue = Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value, 0);
                    m_NomeCliente = "";
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del Cliente
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
            /// Restituisce o imposta il numero del contratto registrato dal fornitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NumeroContratto
            {
                get
                {
                    return m_NumeroContratto;
                }

                set
                {
                    string oldValue = m_NumeroContratto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroContratto = value;
                    DoChanged("NumeroContratto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice del cliente (utilizzato dal fornitore)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceCliente
            {
                get
                {
                    return m_CodiceCliente;
                }

                set
                {
                    string oldValue = m_CodiceCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceCliente = value;
                    DoChanged("CodiceCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice dell'utenza (nel sistema del fornitore)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceUtenza
            {
                get
                {
                    return m_CodiceUtenza;
                }

                set
                {
                    string oldValue = m_CodiceUtenza;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceUtenza = value;
                    DoChanged("CodiceUtenza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione dell'utenza
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
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di periodicità delle bollette generate da questa utenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PeriodicitaUtenza TipoPeriodicita
            {
                get
                {
                    return m_TipoPeriodicita;
                }

                set
                {
                    var oldValue = m_TipoPeriodicita;
                    if (oldValue == value)
                        return;
                    m_TipoPeriodicita = value;
                    DoChanged("TipoPeriodicita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'intervallo di periodicità
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IntervalloPeriodicita
            {
                get
                {
                    return m_IntervalloPeriodicita;
                }

                set
                {
                    int oldValue = m_IntervalloPeriodicita;
                    if (oldValue == value)
                        return;
                    m_IntervalloPeriodicita = value;
                    DoChanged("IntervalloPeriodicita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di sottoscrizione del contratto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataSottoscrizione
            {
                get
                {
                    return m_DataSottoscrizione;
                }

                set
                {
                    var oldValue = m_DataSottoscrizione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataSottoscrizione = value;
                    DoChanged("DataSottoscrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio di fornitura del servizio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizioPeriodo
            {
                get
                {
                    return m_DataInizioPeriodo;
                }

                set
                {
                    var oldValue = m_DataInizioPeriodo;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioPeriodo = value;
                    DoChanged("DataInizioPeriodo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di invio della prima bolletta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataFinePeriodo
            {
                get
                {
                    return m_DataFinePeriodo;
                }

                set
                {
                    var oldValue = m_DataFinePeriodo;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFinePeriodo = value;
                    DoChanged("DataFinePeriodo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'unità di misura del servizio fatturato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UnitaMisura
            {
                get
                {
                    return m_UnitaMisura;
                }

                set
                {
                    string oldValue = m_UnitaMisura;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UnitaMisura = value;
                    DoChanged("UnitaMisura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il costo per unità del servizio erogato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? CostoUnitario
            {
                get
                {
                    return m_CostoUnitario;
                }

                set
                {
                    var oldValue = m_CostoUnitario;
                    if (oldValue == value == true)
                        return;
                    m_CostoUnitario = value;
                    DoChanged("CostoUnitario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i costi fissi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? CostiFissi
            {
                get
                {
                    return m_CostiFissi;
                }

                set
                {
                    var oldValue = m_CostiFissi;
                    if (oldValue == value == true)
                        return;
                    m_CostiFissi = value;
                    DoChanged("CostiFissi", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della valuta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeValuta
            {
                get
                {
                    return m_NomeValuta;
                }

                set
                {
                    string oldValue = m_NomeValuta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeValuta = value;
                    DoChanged("NomeValuta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di utenza (Es. Luce, Gas, Acqua)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoUtenza
            {
                get
                {
                    return m_TipoUtenza;
                }

                set
                {
                    string oldValue = m_TipoUtenza;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoUtenza = value;
                    DoChanged("TipoUtenza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new UtenzaFlags Flags
            {
                get
                {
                    return (UtenzaFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce o imposta il metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object MetodoDiPagamento
            {
                get
                {
                    if (m_MetodoDiPagamento is null)
                        m_MetodoDiPagamento = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_TipoMetodoDiPagamento, m_IDMetodoDiPagamento);
                    return m_MetodoDiPagamento;
                }

                set
                {
                    var oldValue = MetodoDiPagamento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_MetodoDiPagamento = value;
                    m_IDMetodoDiPagamento = DBUtils.GetID(value, 0);
                    m_TipoMetodoDiPagamento = "";
                    m_NomeMetodoDiPagamento = "";
                    if (value is object)
                    {
                        m_TipoMetodoDiPagamento = DMD.RunTime.vbTypeName(value);
                        m_NomeMetodoDiPagamento = ((Anagrafica.IMetodoDiPagamento)value).NomeMetodo;
                    }

                    DoChanged("MetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoMetodoDiPagamento
            {
                get
                {
                    return m_TipoMetodoDiPagamento;
                }

                set
                {
                    string oldValue = m_TipoMetodoDiPagamento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoMetodoDiPagamento = value;
                    m_MetodoDiPagamento = null;
                    DoChanged("TipoMetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDMetodoDiPagamento
            {
                get
                {
                    return DBUtils.GetID(m_MetodoDiPagamento, m_IDMetodoDiPagamento);
                }

                set
                {
                    int oldValue = IDMetodoDiPagamento;
                    if (oldValue == value)
                        return;
                    m_IDMetodoDiPagamento = value;
                    m_MetodoDiPagamento = null;
                    DoChanged("IDMetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del metodo di pagamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeMetodoDiPagamento
            {
                get
                {
                    return m_NomeMetodoDiPagamento;
                }

                set
                {
                    string oldValue = m_NomeMetodoDiPagamento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeMetodoDiPagamento = value;
                    DoChanged("NomeMetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Utenze;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeUtenze";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_IDFornitore = reader.Read("IDFornitore", m_IDFornitore);
                m_NomeFornitore = reader.Read("NomeFornitore", m_NomeFornitore);
                m_IDCliente = reader.Read("IDCliente", m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                m_NumeroContratto = reader.Read("NumeroContratto", m_NumeroContratto);
                m_CodiceCliente = reader.Read("CodiceCliente", m_CodiceCliente);
                m_CodiceUtenza = reader.Read("CodiceUtenza", m_CodiceUtenza);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_TipoPeriodicita = reader.Read("TipoPeriodicita", m_TipoPeriodicita);
                m_IntervalloPeriodicita = reader.Read("IntervalloPeriodicita", m_IntervalloPeriodicita);
                m_DataSottoscrizione = reader.Read("DataSottoscrizione", m_DataSottoscrizione);
                m_DataInizioPeriodo = reader.Read("DataInizioPeriodo", m_DataInizioPeriodo);
                m_DataFinePeriodo = reader.Read("DataFinePeriodo", m_DataFinePeriodo);
                m_UnitaMisura = reader.Read("UnitaMisura", m_UnitaMisura);
                m_CostoUnitario = reader.Read("CostoUnitario", m_CostoUnitario);
                m_CostiFissi = reader.Read("CostiFissi", m_CostiFissi);
                m_NomeValuta = reader.Read("NomeValuta", m_NomeValuta);
                m_TipoMetodoDiPagamento = reader.Read("TipoMetodoDiPagamento", m_TipoMetodoDiPagamento);
                m_IDMetodoDiPagamento = reader.Read("IDMetodotoDiPagamento", m_IDMetodoDiPagamento);
                m_NomeMetodoDiPagamento = reader.Read("NomeMetodoDiPagamento", m_NomeMetodoDiPagamento);
                m_TipoUtenza = reader.Read("TipoUtenza", m_TipoUtenza);
                m_StimatoreBolletta = reader.Read("StimatoreBolletta", m_StimatoreBolletta);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IDFornitore", IDFornitore);
                writer.Write("NomeFornitore", m_NomeFornitore);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("NumeroContratto", m_NumeroContratto);
                writer.Write("CodiceCliente", m_CodiceCliente);
                writer.Write("CodiceUtenza", m_CodiceUtenza);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("TipoPeriodicita", m_TipoPeriodicita);
                writer.Write("IntervalloPeriodicita", m_IntervalloPeriodicita);
                writer.Write("DataSottoscrizione", m_DataSottoscrizione);
                writer.Write("DataInizioPeriodo", m_DataInizioPeriodo);
                writer.Write("DataFinePeriodo", m_DataFinePeriodo);
                writer.Write("UnitaMisura", m_UnitaMisura);
                writer.Write("CostoUnitario", m_CostoUnitario);
                writer.Write("CostiFissi", m_CostiFissi);
                writer.Write("NomeValuta", m_NomeValuta);
                writer.Write("TipoMetodoDiPagamento", m_TipoMetodoDiPagamento);
                writer.Write("IDMetodotoDiPagamento", IDMetodoDiPagamento);
                writer.Write("NomeMetodoDiPagamento", m_NomeMetodoDiPagamento);
                writer.Write("TipoUtenza", m_TipoUtenza);
                writer.Write("StimatoreBolletta", m_StimatoreBolletta);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara il db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("IDFornitore", typeof(int), 1);
                c = table.Fields.Ensure("NomeFornitore", typeof(string), 255);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("NumeroContratto", typeof(string), 255);
                c = table.Fields.Ensure("CodiceCliente", typeof(string), 255);
                c = table.Fields.Ensure("CodiceUtenza", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("TipoPeriodicita", typeof(int), 1);
                c = table.Fields.Ensure("IntervalloPeriodicita", typeof(int), 1);
                c = table.Fields.Ensure("DataSottoscrizione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataInizioPeriodo", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFinePeriodo", typeof(DateTime), 1);
                c = table.Fields.Ensure("UnitaMisura", typeof(string), 255);
                c = table.Fields.Ensure("CostoUnitario", typeof(decimal), 1);
                c = table.Fields.Ensure("CostiFissi", typeof(decimal), 1);
                c = table.Fields.Ensure("NomeValuta", typeof(string), 255);
                c = table.Fields.Ensure("TipoMetodoDiPagamento", typeof(string), 255);
                c = table.Fields.Ensure("IDMetodotoDiPagamento", typeof(int), 1);
                c = table.Fields.Ensure("NomeMetodoDiPagamento", typeof(string), 255);
                c = table.Fields.Ensure("TipoUtenza", typeof(string), 255);
                c = table.Fields.Ensure("StimatoreBolletta", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContratto", new string[] { "IDFornitore", "NumeroContratto", "IDCliente" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxContratto", new string[] { "NomeFornitore", "NomeCliente", "CodiceCliente", "CodiceUtenza" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPeriodo", new string[] { "TipoPeriodicita", "IntervalloPeriodicita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataSottoscrizione", "DataInizioPeriodo", "DataFinePeriodo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCosti", new string[] { "UnitaMisura", "CostoUnitario", "CostiFissi", "NomeValuta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPagamenti", new string[] { "TipoMetodoDiPagamento", "IDMetodotoDiPagamento", "NomeMetodoDiPagamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTipoUtenza", new string[] { "TipoUtenza", "StimatoreBolletta" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDFornitore", IDFornitore);
                writer.WriteAttribute("NomeFornitore", m_NomeFornitore);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("NumeroContratto", m_NumeroContratto);
                writer.WriteAttribute("CodiceCliente", m_CodiceCliente);
                writer.WriteAttribute("CodiceUtenza", m_CodiceUtenza);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("TipoPeriodicita", (int?)m_TipoPeriodicita);
                writer.WriteAttribute("IntervalloPeriodicita", m_IntervalloPeriodicita);
                writer.WriteAttribute("DataSottoscrizione", m_DataSottoscrizione);
                writer.WriteAttribute("DataInizioPeriodo", m_DataInizioPeriodo);
                writer.WriteAttribute("DataFinePeriodo", m_DataFinePeriodo);
                writer.WriteAttribute("UnitaMisura", m_UnitaMisura);
                writer.WriteAttribute("CostoUnitario", m_CostoUnitario);
                writer.WriteAttribute("CostiFissi", m_CostiFissi);
                writer.WriteAttribute("NomeValuta", m_NomeValuta);
                writer.WriteAttribute("TipoMetodoDiPagamento", m_TipoMetodoDiPagamento);
                writer.WriteAttribute("IDMetodotoDiPagamento", IDMetodoDiPagamento);
                writer.WriteAttribute("NomeMetodoDiPagamento", m_NomeMetodoDiPagamento);
                writer.WriteAttribute("TipoUtenza", m_TipoUtenza);
                writer.WriteAttribute("StimatoreBolletta", m_StimatoreBolletta);
                base.XMLSerialize(writer);
                writer.WriteTag("Programmazione", Programmazione);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "NumeroContratto":
                        {
                            m_NumeroContratto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceCliente":
                        {
                            m_CodiceCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceUtenza":
                        {
                            m_CodiceUtenza = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoPeriodicita":
                        {
                            m_TipoPeriodicita = (PeriodicitaUtenza)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IntervalloPeriodicita":
                        {
                            m_IntervalloPeriodicita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataSottoscrizione":
                        {
                            m_DataSottoscrizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataInizioPeriodo":
                        {
                            m_DataInizioPeriodo = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFinePeriodo":
                        {
                            m_DataFinePeriodo = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "UnitaMisura":
                        {
                            m_UnitaMisura = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CostoUnitario":
                        {
                            m_CostoUnitario = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CostiFissi":
                        {
                            m_CostiFissi = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeValuta":
                        {
                            m_NomeValuta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 
                    case "TipoMetodoDiPagamento":
                        {
                            m_TipoMetodoDiPagamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDMetodoDiPagamento":
                        {
                            m_IDMetodoDiPagamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeMetodoDiPagamento":
                        {
                            m_NomeMetodoDiPagamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoUtenza":
                        {
                            m_TipoUtenza = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StimatoreBolletta":
                        {
                            m_StimatoreBolletta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Programmazione":
                        {
                            m_Programmazione = (Sistema.MultipleScheduleCollection)fieldValue;
                            m_Programmazione.SetOwner(this);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Utenza) && this.Equals((Utenza)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Utenza obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Integers.EQ(this.m_IDFornitore, obj.m_IDFornitore)
                    && DMD.Strings.EQ(this.m_NomeFornitore, obj.m_NomeFornitore)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Strings.EQ(this.m_NumeroContratto, obj.m_NumeroContratto)
                    && DMD.Strings.EQ(this.m_CodiceCliente, obj.m_CodiceCliente)
                    && DMD.Strings.EQ(this.m_CodiceUtenza, obj.m_CodiceUtenza)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Integers.EQ((int)this.m_TipoPeriodicita, (int)obj.m_TipoPeriodicita)
                    && DMD.Integers.EQ(this.m_IntervalloPeriodicita, obj.m_IntervalloPeriodicita)
                    && DMD.DateUtils.EQ(this.m_DataSottoscrizione, obj.m_DataSottoscrizione)
                    && DMD.DateUtils.EQ(this.m_DataInizioPeriodo, obj.m_DataInizioPeriodo)
                    && DMD.DateUtils.EQ(this.m_DataFinePeriodo, obj.m_DataFinePeriodo)
                    && DMD.Strings.EQ(this.m_UnitaMisura, obj.m_UnitaMisura)
                    && DMD.Decimals.EQ(this.m_CostoUnitario, obj.m_CostoUnitario)
                    && DMD.Decimals.EQ(this.m_CostiFissi, obj.m_CostiFissi)
                    && DMD.Strings.EQ(this.m_NomeValuta, obj.m_NomeValuta)
                    //private object m_MetodoDiPagamento;
                    && DMD.Strings.EQ(this.m_TipoMetodoDiPagamento, obj.m_TipoMetodoDiPagamento)
                    && DMD.Integers.EQ(this.m_IDMetodoDiPagamento, obj.m_IDMetodoDiPagamento)
                    && DMD.Strings.EQ(this.m_NomeMetodoDiPagamento, obj.m_NomeMetodoDiPagamento)
                    && DMD.Strings.EQ(this.m_TipoUtenza, obj.m_TipoUtenza)
                    && DMD.Strings.EQ(this.m_StimatoreBolletta, obj.m_StimatoreBolletta)
                    ;
            //private MultipleScheduleCollection m_Programmazione;

        }

        /// <summary>
        /// Invalida la programmazione
        /// </summary>
        protected void InvalidateProgrammazione()
            {
                lock (this)
                    m_Programmazione = null;
            }

            /// <summary>
            /// Notifica una nuova programmazione
            /// </summary>
            /// <param name="s"></param>
            protected void NotifySchedule(Sistema.CalendarSchedule s)
            {
                lock (this)
                {
                    if (m_Programmazione is null)
                        return;
                    var o = m_Programmazione.GetItemById(DBUtils.GetID(s, 0));
                    if (ReferenceEquals(o, s))
                    {
                        return;
                    }

                    if (o is object)
                    {
                        int i = m_Programmazione.IndexOf(o);
                        if (s.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            m_Programmazione[i] = s;
                        }
                        else
                        {
                            m_Programmazione.RemoveAt(i);
                        }
                    }
                    else if (s.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        m_Programmazione.Add(s);
                    }
                }
            }

            void ISchedulable.InvalidateProgrammazione()
            {
                this.InvalidateProgrammazione();
            }

            void ISchedulable.NotifySchedule(CalendarSchedule s)
            {
                this.NotifySchedule(s);
            }
        }
    }
}