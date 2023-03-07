using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flag relativi ai conti corrente
        /// </summary>
        [Flags]
        public enum ContoCorrenteFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }

        /// <summary>
        /// Stati di un conto corrente
        /// </summary>
        public enum StatoContoCorrente : int
        {
            /// <summary>
            /// Stato sconosciuto
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Conto corrente valido e attivo
            /// </summary>
            Attivo = 1,

            /// <summary>
            /// Conto sospeso
            /// </summary>
            Sospeso = 2,

            /// <summary>
            /// Conto chiuso
            /// </summary>
            Chiuso = 3
            // Sconosciuto = 255
        }

        /// <summary>
    /// Rappresenta un conto corrente
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class ContoCorrente 
            : Databases.DBObject
        {
            private string m_Nome;
            private string m_Numero;
            private string m_IBAN;
            private string m_SWIFT;
            private int m_IDBanca;
            private string m_NomeBanca;
            [NonSerialized] private CBanca m_Banca;
            private DateTime? m_DataApertura;
            private DateTime? m_DataChiusura;
            private decimal? m_Saldo;
            private decimal? m_SaldoDisponibile;
            private StatoContoCorrente m_StatoContoCorrente;
            [NonSerialized] private CIntestatariContoCorrente m_Intestatari;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public ContoCorrente()
            {
                this.m_Nome = "";
                this.m_Numero = "";
                this.m_IBAN = "";
                this.m_SWIFT = "";
                this.m_IDBanca = 0;
                this.m_NomeBanca = "";
                this.m_Banca = null;
                this.m_DataApertura = default;
                this.m_DataChiusura = default;
                this.m_Saldo = default;
                this.m_SaldoDisponibile = default;
                this.m_StatoContoCorrente = StatoContoCorrente.Attivo;
                this.m_Flags = (int) ContoCorrenteFlags.None;
                this.m_Intestatari = null;                 
            }

            
            /// <summary>
            /// Restituisce o imposta il nome del conto corrente
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero del conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Numero
            {
                get
                {
                    return m_Numero;
                }

                set
                {
                    value = ContiCorrente.ParseNumero(value);
                    string oldValue = m_Numero;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Numero = value;
                    DoChanged("Numero", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'IBAN
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IBAN
            {
                get
                {
                    return m_IBAN;
                }

                set
                {
                    value = ContiCorrente.ParseIBAN(value);
                    string oldValue = m_IBAN;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IBAN = value;
                    DoChanged("IBAN", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice SWIFT
            /// </summary>
            public string SWIFT
            {
                get
                {
                    return m_SWIFT;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SWIFT;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SWIFT = value;
                    DoChanged("SWIFT", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della banca
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDBanca
            {
                get
                {
                    return DBUtils.GetID(this.m_Banca, this.m_IDBanca);
                }

                set
                {
                    int oldValue = this.IDBanca;
                    if (oldValue == value)
                        return;
                    m_IDBanca = value;
                    m_Banca = null;
                    DoChanged("IDBanca", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la banca presso cui è stato aperto il conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CBanca Banca
            {
                get
                {
                    if (m_Banca is null)
                        m_Banca = minidom.Anagrafica.Banche.GetItemById(m_IDBanca);
                    return m_Banca;
                }

                set
                {
                    var oldValue = Banca;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Banca = value;
                    m_IDBanca = DBUtils.GetID(value, this.m_IDBanca);
                    m_NomeBanca = "";
                    if (value is object)
                        m_NomeBanca = value.Descrizione;
                    DoChanged("Banca", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la banca
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetBanca(CBanca value)
            {
                m_Banca = value;
                m_IDBanca = DBUtils.GetID(value, this.m_IDBanca);
            }

            /// <summary>
            /// Restituisce o imposta il nome della banca presso cui è stato aperto il conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeBanca
            {
                get
                {
                    return m_NomeBanca;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeBanca;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeBanca = value;
                    DoChanged("NomeBanca", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di apertura del conto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataApertura
            {
                get
                {
                    return m_DataApertura;
                }

                set
                {
                    var oldValue = m_DataApertura;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataApertura = value;
                    DoChanged("DataApertura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di chiusura del conto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }

                set
                {
                    var oldValue = m_DataChiusura;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataChiusura = value;
                    DoChanged("DataChiusura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il saldo finale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? Saldo
            {
                get
                {
                    return m_Saldo;
                }

                set
                {
                    var oldValue = m_Saldo;
                    if (oldValue == value == true)
                        return;
                    m_Saldo = value;
                    DoChanged("Saldo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il saldo disponibile
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? SaldoDisponibile
            {
                get
                {
                    return m_SaldoDisponibile;
                }

                set
                {
                    var oldValue = m_SaldoDisponibile;
                    if (oldValue == value == true)
                        return;
                    m_SaldoDisponibile = value;
                    DoChanged("SaldoDisponibile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato del conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoContoCorrente StatoContoCorrente
            {
                get
                {
                    return m_StatoContoCorrente;
                }

                set
                {
                    var oldValue = m_StatoContoCorrente;
                    if (oldValue == value)
                        return;
                    m_StatoContoCorrente = value;
                    DoChanged("StatoContoCorrente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flag aggiuntivi
            /// </summary>
            public new ContoCorrenteFlags Flags
            {
                get
                {
                    return (ContoCorrenteFlags)m_Flags;
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
            /// Restituisce l'elenco degli intestatari del conto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIntestatariContoCorrente Intestatari
            {
                get
                {
                    if (m_Intestatari is null)
                        m_Intestatari = new CIntestatariContoCorrente(this);
                    return m_Intestatari;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ContiCorrente; //.Module;
            }

            /// <summary>
            /// Discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ContiCorrenti";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_Numero = reader.Read("Numero", m_Numero);
                m_IBAN = reader.Read("IBAN", m_IBAN);
                m_SWIFT = reader.Read("SWIFT", m_SWIFT);
                m_IDBanca = reader.Read("IDBanca", m_IDBanca);
                m_NomeBanca = reader.Read("NomeBanca", m_NomeBanca);
                m_DataApertura = reader.Read("DataApertura", m_DataApertura);
                m_DataChiusura = reader.Read("DataChiusura", m_DataChiusura);
                m_Saldo = reader.Read("Saldo", m_Saldo);
                m_SaldoDisponibile = reader.Read("SaldoDisponibile", m_SaldoDisponibile);
                m_StatoContoCorrente = reader.Read("StatoContoCorrente", m_StatoContoCorrente);
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
                writer.Write("Numero", m_Numero);
                writer.Write("IBAN", m_IBAN);
                writer.Write("SWIFT", m_SWIFT);
                writer.Write("IDBanca", IDBanca);
                writer.Write("NomeBanca", m_NomeBanca);
                writer.Write("DataApertura", m_DataApertura);
                writer.Write("DataChiusura", m_DataChiusura);
                writer.Write("Saldo", m_Saldo);
                writer.Write("SaldoDisponibile", m_SaldoDisponibile);
                writer.Write("StatoContoCorrente", m_StatoContoCorrente);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Numero", typeof(string), 255);
                c = table.Fields.Ensure("IBAN", typeof(string), 255);
                c = table.Fields.Ensure("SWIFT", typeof(string), 255);
                c = table.Fields.Ensure("IDBanca", typeof(int), 1);
                c = table.Fields.Ensure("NomeBanca", typeof(string), 255);
                c = table.Fields.Ensure("DataApertura", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataChiusura", typeof(DateTime), 1);
                c = table.Fields.Ensure("Saldo", typeof(Decimal), 1);
                c = table.Fields.Ensure("SaldoDisponibile", typeof(Decimal), 1);
                c = table.Fields.Ensure("StatoContoCorrente", typeof(int), 1);
                c = table.Fields.Ensure("Flags", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "StatoContoCorrente", "Nome" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataApertura", "DataChiusura" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCodici", new string[] { "Numero", "IBAN", "SWIFT" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxBanca", new string[] { "IDBanca", "NomeBanca", "Flags" }, DBFieldConstraintFlags.None);
                //c = table.Constraints.Ensure("idxSaldi", new string[] { "Saldo", "SaldoDisponibile" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Numero", m_Numero);
                writer.WriteAttribute("IBAN", m_IBAN);
                writer.WriteAttribute("SWIFT", m_SWIFT);
                writer.WriteAttribute("IDBanca", IDBanca);
                writer.WriteAttribute("NomeBanca", m_NomeBanca);
                writer.WriteAttribute("DataApertura", m_DataApertura);
                writer.WriteAttribute("DataChiusura", m_DataChiusura);
                writer.WriteAttribute("Saldo", m_Saldo);
                writer.WriteAttribute("SaldoDisponibile", m_SaldoDisponibile);
                writer.WriteAttribute("StatoContoCorrente", (int?)m_StatoContoCorrente);
                base.XMLSerialize(writer);
                writer.WriteTag("Intestatari", this.Intestatari);
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

                    case "Numero":
                        {
                            m_Numero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IBAN":
                        {
                            m_IBAN = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SWIFT":
                        {
                            m_SWIFT = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDBanca":
                        {
                            m_IDBanca = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeBanca":
                        {
                            m_NomeBanca = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataApertura":
                        {
                            m_DataApertura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataChiusura":
                        {
                            m_DataChiusura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Saldo":
                        {
                            m_Saldo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SaldoDisponibile":
                        {
                            m_SaldoDisponibile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "StatoContoCorrente":
                        {
                            m_StatoContoCorrente = (StatoContoCorrente)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Intestatari":
                        {
                            m_Intestatari = (CIntestatariContoCorrente)fieldValue;
                            m_Intestatari.SetContoCorrente(this);
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
                return DMD.Strings.ConcatArray("{ ", this.m_Nome, " : ", this.m_NomeBanca , ", ", this.m_IBAN , ", ", this.m_DataApertura, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome, this.m_NomeBanca, this.m_IBAN, this.m_DataApertura);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is ContoCorrente) && this.Equals((ContoCorrente)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ContoCorrente obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_Numero, obj.m_Numero)
                     && DMD.Strings.EQ(this.m_IBAN, obj.m_IBAN)
                    && DMD.Strings.EQ(this.m_SWIFT, obj.m_SWIFT)
                    && DMD.Integers.EQ(this.IDBanca, obj.IDBanca)
                    && DMD.Strings.EQ(this.m_NomeBanca, obj.m_NomeBanca)
                    && DMD.DateUtils.EQ(this.m_DataApertura, obj.m_DataApertura)
                    && DMD.DateUtils.EQ(this.m_DataChiusura, obj.m_DataChiusura)
                    && DMD.Decimals.EQ(this.m_Saldo, obj.m_Saldo)
                    && DMD.Decimals.EQ(this.m_SaldoDisponibile, obj.m_SaldoDisponibile)
                    && DMD.Integers.EQ((int)this.m_StatoContoCorrente, (int)obj.m_StatoContoCorrente)
                    ;
            //[NonSerialized] private CIntestatariContoCorrente m_Intestatari;
            //private CKeyCollection m_Parameters;

        }
        }
    }
}