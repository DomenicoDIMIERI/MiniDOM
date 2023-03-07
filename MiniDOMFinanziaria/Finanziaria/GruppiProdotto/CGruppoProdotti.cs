using System;
using System.Data;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Rappresenta un gruppo di prodotti
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CGruppoProdotti : Databases.DBObject, ICloneable
        {
            private string m_NomeCessionario;
            private int m_CessionarioID;
            [NonSerialized]
            private CCQSPDCessionarioClass m_Cessionario;
            private string m_TipoContrattoID;
            // Private m_TipoContratto As CQSPDTipoContratto
            private string m_TipoRapportoID;
            // Private m_TipoRapporto As TipoRapporto
            private decimal m_costoPrestitalia;
            private int m_TipoCostoPrestitalia;
            private decimal m_CostoAmministrazione;
            private int m_TipoCostoAmministrazione;
            private TipoCalcoloTEG m_TipoCalcoloTEG;
            private int m_Scomposizione;
            // Private m_IDProduttore As Integer
            // Private m_Produttore As  pro
            private double m_ImpostaSostitutiva;
            private double m_ProvvigioneMassima;
            private bool m_InBando;
            // Private m_Assicurazioni
            // Private m_Convenzioni
            private decimal m_CostoPerRata;
            private double m_Rappel;
            private decimal m_Costo;
            // Private m_CriteriAssuntiviID
            // Private m_CriteriAssuntivi
            // Private m_DocumentiPerPratica As CDocumentoPerPraticaGrpCollection
            private string m_Descrizione;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private bool m_Visible;
            [NonSerialized]
            private CStatoPratica m_StatoIniziale;
            private int m_IDStatoIniziale;
            [NonSerialized]
            private CDocumentiXGruppoProdottiCollection m_Documenti;
            private double[] m_SpeseCoefficiente;
            private double[] m_Interessi;
            private double[] m_Durata;
            private decimal[] m_SpeseFisse;
            private decimal[] m_Rivalsa;
            [NonSerialized]
            private object m_Lock = new object();
            [NonSerialized]
            private CCQSPDTipoProvvigioneCollection m_Provvigioni;

            public CGruppoProdotti()
            {
                m_Descrizione = "Nuovo gruppo prodotti";
                m_NomeCessionario = "";
                m_CessionarioID = 0;
                m_Cessionario = null;
                m_TipoContrattoID = "C";
                // m_TipoContratto = Nothing
                m_TipoRapportoID = "";
                // m_TipoRapporto = Nothing
                m_costoPrestitalia = 0m;
                m_TipoCostoPrestitalia = 0;
                m_CostoAmministrazione = 0m;
                m_TipoCostoAmministrazione = 0;
                m_TipoCalcoloTEG = 0;
                m_Scomposizione = 0;
                // m_IDProduttore = 0
                // m_Produttore = Nothing
                m_Durata = new double[11];
                m_ImpostaSostitutiva = 0d;
                m_ProvvigioneMassima = 0d;
                m_InBando = false;
                // m_Documenti = Nothing
                m_SpeseFisse = new decimal[11];
                m_SpeseCoefficiente = new double[11];
                m_CostoPerRata = 0m;
                m_Interessi = new double[11];
                m_Rappel = 0d;
                m_Costo = 0m;
                // m_CriteriAssuntivi = Nothing
                m_Rivalsa = new decimal[11];
                // m_DocumentiPerPratica = Nothing
                m_Descrizione = "";
                m_Visible = true;
                m_StatoIniziale = null;
                m_IDStatoIniziale = 0;
                m_Documenti = null;
                m_Provvigioni = null;
            }

            /// <summary>
        /// Restituisce la collezione delle provvigioni definite per il gruppo prodotti
        /// </summary>
        /// <returns></returns>
            public CCQSPDTipoProvvigioneCollection Provvigioni
            {
                get
                {
                    if (m_Provvigioni is null)
                        m_Provvigioni = new CCQSPDTipoProvvigioneCollection(this);
                    return m_Provvigioni;
                }
            }

            internal void InvalidateTipiProvvigione()
            {
                m_Provvigioni = null;
            }

            public CDocumentiXGruppoProdottiCollection Documenti
            {
                get
                {
                    lock (m_Lock)
                    {
                        if (m_Documenti is null)
                            m_Documenti = new CDocumentiXGruppoProdottiCollection(this);
                        return m_Documenti;
                    }
                }
            }

            public CStatoPratica StatoIniziale
            {
                get
                {
                    if (m_StatoIniziale is null)
                        m_StatoIniziale = StatiPratica.GetItemById(m_IDStatoIniziale);
                    return m_StatoIniziale;
                }

                set
                {
                    var oldValue = m_StatoIniziale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoIniziale = value;
                    m_IDStatoIniziale = DBUtils.GetID(value);
                    DoChanged("StatoIniziale", value, oldValue);
                }
            }

            public int IDStatoIniziale
            {
                get
                {
                    return DBUtils.GetID(m_StatoIniziale, m_IDStatoIniziale);
                }

                set
                {
                    int oldValue = IDStatoIniziale;
                    if (oldValue == value)
                        return;
                    m_IDStatoIniziale = value;
                    m_StatoIniziale = null;
                    DoChanged("IDStatoIniziale", value, oldValue);
                }
            }

            public override CModulesClass GetModule()
            {
                return GruppiProdotto.Module;
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il gruppo prodotti è visibile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Visible
            {
                get
                {
                    return m_Visible;
                }

                set
                {
                    if (m_Visible == value)
                        return;
                    m_Visible = value;
                    DoChanged("Visible", value, !value);
                }
            }

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

            public string NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            public int CessionarioID
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_CessionarioID);
                }

                set
                {
                    int oldValue = CessionarioID;
                    if (oldValue == value)
                        return;
                    m_CessionarioID = value;
                    m_Cessionario = null;
                    DoChanged("CessionarioID", value, oldValue);
                }
            }

            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_CessionarioID);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (oldValue == value)
                        return;
                    m_Cessionario = value;
                    m_CessionarioID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            public string TipoContrattoID
            {
                get
                {
                    return m_TipoContrattoID;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_TipoContrattoID;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContrattoID = value;
                    DoChanged("TipoContrattoID", value, oldValue);
                }
            }

            public string TipoRapportoID
            {
                get
                {
                    return m_TipoRapportoID;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_TipoRapportoID;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoRapportoID = value;
                    DoChanged("TipoRapportoID", value, oldValue);
                }
            }

            public decimal get_Rivalsa(int durata)
            {
                return m_Rivalsa[(int)Maths.Floor(durata / 12d)];
            }

            public void set_Rivalsa(int durata, decimal value)
            {
                decimal oldValue = m_Rivalsa[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_Rivalsa[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("Rivalsa", value, oldValue);
            }

            public double get_Coefficiente(int durata)
            {
                return m_Durata[(int)Maths.Floor(durata / 12d)];
            }

            public void set_Coefficiente(int durata, double value)
            {
                double oldValue = m_Durata[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_Durata[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("Durata", value, oldValue);
            }

            public double get_Interessi(int durata)
            {
                return m_Interessi[(int)Maths.Floor(durata / 12d)];
            }

            public void set_Interessi(int durata, double value)
            {
                double oldValue = m_Interessi[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_Interessi[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("Interessi", value, oldValue);
            }

            public double get_Commissioni(int durata)
            {
                return get_Coefficiente(durata) - get_Interessi(durata);
            }

            public void set_Commissioni(int durata, double value)
            {
                set_Coefficiente(durata, get_Interessi(durata) + value);
            }

            public decimal CostoPerRata
            {
                get
                {
                    return m_CostoPerRata;
                }

                set
                {
                    decimal oldValue = m_CostoPerRata;
                    if (oldValue == value)
                        return;
                    m_CostoPerRata = value;
                    DoChanged("CostoPerRata", value, oldValue);
                }
            }

            public decimal get_SpeseFisse(int durata)
            {
                return m_SpeseFisse[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpeseFisse(int durata, decimal value)
            {
                decimal oldValue = m_SpeseFisse[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_SpeseFisse[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("SpeseFisse", value, oldValue);
            }

            public double get_SpeseCoefficiente(int durata)
            {
                return m_SpeseCoefficiente[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpeseCoefficiente(int durata, double value)
            {
                double oldValue = m_SpeseCoefficiente[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_SpeseCoefficiente[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("SpeseCoefficiente", value, oldValue);
            }

            public double ProvvigioneMassima
            {
                get
                {
                    return m_ProvvigioneMassima;
                }

                set
                {
                    double oldValue = m_ProvvigioneMassima;
                    if (oldValue == value)
                        return;
                    m_ProvvigioneMassima = value;
                    DoChanged("ProvvigioneMassima", value, oldValue);
                }
            }

            public decimal CostoPrestitalia
            {
                get
                {
                    return m_costoPrestitalia;
                }

                set
                {
                    decimal oldValue = m_costoPrestitalia;
                    if (oldValue == value)
                        return;
                    m_costoPrestitalia = value;
                    DoChanged("CostoPrestitalia", value, oldValue);
                }
            }

            public decimal CostoAmministrazione
            {
                get
                {
                    return m_CostoAmministrazione;
                }

                set
                {
                    decimal oldValue = m_CostoAmministrazione;
                    if (oldValue == value)
                        return;
                    m_CostoAmministrazione = value;
                    DoChanged("CostoAmministrazione", value, oldValue);
                }
            }

            public double ImpostaSostitutiva
            {
                get
                {
                    return m_ImpostaSostitutiva;
                }

                set
                {
                    double oldValue = m_ImpostaSostitutiva;
                    if (oldValue == value)
                        return;
                    m_ImpostaSostitutiva = value;
                    DoChanged("ImpostaSostitutiva", value, oldValue);
                }
            }

            public decimal Costo
            {
                get
                {
                    return m_Costo;
                }

                set
                {
                    decimal oldValue = m_Costo;
                    if (oldValue == value)
                        return;
                    m_Costo = value;
                    DoChanged("Costo", value, oldValue);
                }
            }

            public double Rappel
            {
                get
                {
                    return m_Rappel;
                }

                set
                {
                    double oldValue = m_Rappel;
                    if (oldValue == value)
                        return;
                    m_Rappel = value;
                    DoChanged("Rappel", value, oldValue);
                }
            }

            public TipoCalcoloTEG TipoCalcoloTeg
            {
                get
                {
                    return m_TipoCalcoloTEG;
                }

                set
                {
                    var oldValue = m_TipoCalcoloTEG;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloTEG = value;
                    DoChanged("TipoCalcoloTEG", value, oldValue);
                }
            }

            public double GetMaxTEG(COffertaCQS calculator)
            {
                IDataReader dbRis;
                string dbSQL;
                double ret;
                decimal montanteLordo;
                int durata;
                montanteLordo = calculator.MontanteLordo;
                durata = calculator.Durata;
                ret = 0d;
                switch (TipoCalcoloTeg)
                {
                    case TipoCalcoloTEG.TEG_COSTANTE:
                        {
                            dbSQL = "SELECT * FROM [tbl_TEG_Costante] WHERE [Tipo]='G' And [Prodotto] = " + ID + ";";
                            dbRis = Database.ExecuteReader(dbSQL);
                            if (dbRis.Read())
                            {
                                ret = Sistema.Formats.ToDouble(dbRis["teg" + calculator.Durata]);
                            }

                            dbRis.Dispose();
                            dbRis = null;
                            break;
                        }

                    case TipoCalcoloTEG.TEG_FDIML:
                        {
                            dbSQL = "SELECT * FROM [tbl_TEG_ML] WHERE [Tipo]='G' And [FinoAML] = (SELECT Min([FinoAML]) FROM [tbl_TEG_ML] WHERE [Tipo]='G' And [FinoAML]>" + DBUtils.DBNumber(calculator.MontanteLordo) + " And [Prodotto]=" + ID + ") And [Prodotto]=" + ID + ";";
                            dbRis = Database.ExecuteReader(dbSQL);
                            if (dbRis.Read())
                            {
                                ret = Sistema.Formats.ToDouble(dbRis["teg" + calculator.Durata]);
                            }

                            dbRis.Dispose();
                            dbRis = null;
                            break;
                        }

                    case TipoCalcoloTEG.TEG_FDIETA:
                        {
                            dbSQL = "SELECT * FROM [tbl_TEG_Eta] WHERE [Tipo]='G' And [Eta] = (SELECT Min([Eta]) FROM [tbl_TEG_Eta] WHERE [Tipo]='G' And [Eta]>" + calculator.Eta + " And [Prodotto]=" + ID + ") And [Prodotto]=" + ID + ";";
                            dbRis = Database.ExecuteReader(dbSQL);
                            if (dbRis.Read())
                            {
                                ret = Sistema.Formats.ToDouble(dbRis["teg" + calculator.Durata]);
                            }

                            dbRis.Dispose();
                            dbRis = null;
                            break;
                        }

                    default:
                        {
                            // Response.Write("Parametro TipoCalcoloTeg non valido in CFsbPrevGruppo_Prodotti")
                            // Response.End
                            ret = -1;
                            break;
                        }
                }

                return ret;
            }

            // Property Get CriteriAssuntivi
            // If (m_CriteriAssuntivi Is Nothing) And (m_CriteriAssuntiviID <> 0) Then
            // Set m_CriteriAssuntivi = GestioneDocumentale.GetDocumentoByID(m_CriteriAssuntiviID)
            // End If
            // Set CriteriAssuntivi = m_CriteriAssuntivi
            // End Property
            // Property Set CriteriAssuntivi(value)
            // Set m_CriteriAssuntivi = value
            // m_CriteriAssuntiviID = GetObjectID(value)
            // End Property

            // Property Get CriteriAssuntiviID
            // CriteriAssuntiviID = m_CriteriAssuntiviID
            // End Property
            // Property Let CriteriAssuntiviID(value)
            // m_CriteriAssuntiviID = Users.ParseInteger(value)
            // Set m_CriteriAssuntivi = Nothing
            // End Property

            public CCQSPDProdotto[] GetArrayProdottiValidi()
            {
                var cursor = new CProdottiCursor();
                var ret = new CCollection<CCQSPDProdotto>();
                cursor.GruppoProdottiID.Value = DBUtils.GetID(this);
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
                cursor.OnlyValid = true;
                while (!cursor.EOF())
                {
                    ret.Add(cursor.Item);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret.ToArray();
            }

            // Public ReadOnly Property DocumentiPerPratica As CDocumentoPerPraticaGrpCollection
            // Get
            // If Me.m_DocumentiPerPratica Is Nothing Then
            // Me.m_DocumentiPerPratica = New CDocumentoPerPraticaGrpCollection
            // Me.m_DocumentiPerPratica.Load(Me)
            // End If
            // Return Me.m_DocumentiPerPratica
            // End Get
            // End Property

            public override string GetTableName()
            {
                return "tbl_GruppiProdotti";
            }

            // Protected Overrides Function SaveToDatabase(dbConn As CDBConnection) As Boolean
            // Dim ret As Boolean = MyBase.SaveToDatabase(dbConn)
            // 'If Not (Me.m_DocumentiPerPratica Is Nothing) Then dbConn.Save(Me.m_DocumentiPerPratica)
            // Return ret
            // End Function

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_CessionarioID = reader.Read("Cessionario",  m_CessionarioID);
                m_NomeCessionario = reader.Read("NomeCessionario",  m_NomeCessionario);
                m_TipoContrattoID = reader.Read("TipoContratto",  m_TipoContrattoID);
                m_TipoRapportoID = reader.Read("TipoRapporto",  m_TipoRapportoID);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_costoPrestitalia = reader.Read("CostoPrestitalia",  m_costoPrestitalia);
                m_CostoAmministrazione = reader.Read("CostoAmministrazione",  m_CostoAmministrazione);
                m_TipoCalcoloTEG = reader.Read("TipoCalcoloTeg",  m_TipoCalcoloTEG);
                m_CostoPerRata = reader.Read("CostoPerRata",  m_CostoPerRata);
                m_Rappel = reader.Read("Rappel",  m_Rappel);
                m_Costo = reader.Read("Costo",  m_Costo);
                // m_CriteriAssuntiviID = Formats.ToInteger(dbRis("CriteriAssuntivi"))
                for (int i = 2; i <= 10; i++)
                {
                    m_Durata[i] = reader.Read("durata" + i * 12, m_Durata[i]);
                    m_SpeseFisse[i] = reader.Read("SpeseFisse" + i * 12, m_SpeseFisse[i]);
                    m_SpeseCoefficiente[i] = reader.Read("SpeseCoefficiente" + i * 12, m_SpeseCoefficiente[i]);
                    m_Interessi[i] = reader.Read("Interessi" + i * 12, m_Interessi[i]);
                    m_Rivalsa[i] = reader.Read("rivalsa" + i * 12, m_Rivalsa[i]);
                }

                m_ProvvigioneMassima = reader.Read("provvigioneMassima", m_ProvvigioneMassima);
                m_ImpostaSostitutiva = reader.Read("ImpostaSostitutiva", m_ImpostaSostitutiva);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                m_Visible = reader.Read("Visible", m_Visible);
                m_IDStatoIniziale = reader.Read("IDStatoIniziale", m_IDStatoIniziale);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                int i;
                writer.Write("Cessionario", DBUtils.GetID(m_Cessionario, m_CessionarioID));
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("TipoContratto", m_TipoContrattoID);
                writer.Write("TipoRapporto", m_TipoRapportoID);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("costoPrestitalia", m_costoPrestitalia);
                writer.Write("CostoAmministrazione", m_CostoAmministrazione);
                writer.Write("TipoCalcoloTeg", m_TipoCalcoloTEG);
                writer.Write("CostoPerRata", m_CostoPerRata);
                writer.Write("Rappel", m_Rappel);
                writer.Write("Costo", m_Costo);
                // dbRis("CriteriAssuntivi") = m_CriteriAssuntiviID
                for (i = 2; i <= 10; i++)
                {
                    writer.Write("durata" + i * 12, m_Durata[i]);
                    writer.Write("SpeseFisse" + i * 12, m_SpeseFisse[i]);
                    writer.Write("SpeseCoefficiente" + i * 12, m_SpeseCoefficiente[i]);
                    writer.Write("Interessi" + i * 12, m_Interessi[i]);
                    writer.Write("Rivalsa" + i * 12, m_Rivalsa[i]);
                }

                writer.Write("ProvvigioneMassima", m_ProvvigioneMassima);
                writer.Write("ImpostaSostitutiva", m_ImpostaSostitutiva);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Visible", m_Visible);
                writer.Write("IDStatoIniziale", IDStatoIniziale);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Descrizione;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("CessionarioID", CessionarioID);
                writer.WriteAttribute("TipoContrattoID", m_TipoContrattoID);
                writer.WriteAttribute("TipoRapportoID", m_TipoRapportoID);
                writer.WriteAttribute("costoPrestitalia", m_costoPrestitalia);
                writer.WriteAttribute("TipoCostoPrestitalia", m_TipoCostoPrestitalia);
                writer.WriteAttribute("CostoAmministrazione", m_CostoAmministrazione);
                writer.WriteAttribute("TipoCostoAmministrazione", m_TipoCostoAmministrazione);
                writer.WriteAttribute("TipoCalcoloTEG", (int?)m_TipoCalcoloTEG);
                writer.WriteAttribute("Scomposizione", m_Scomposizione);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Visible", m_Visible);
                writer.WriteAttribute("IDStatoIniziale", IDStatoIniziale);
                writer.WriteAttribute("ImpostaSostitutiva", m_ImpostaSostitutiva);
                writer.WriteAttribute("ProvvigioneMassima", m_ProvvigioneMassima);
                writer.WriteAttribute("InBando", m_InBando);
                writer.WriteAttribute("CostoPerRata", m_CostoPerRata);
                writer.WriteAttribute("Rappel", m_Rappel);
                writer.WriteAttribute("Costo", m_Costo);
                base.XMLSerialize(writer);
                writer.WriteTag("Provvigioni", Provvigioni);
                writer.WriteTag("Durata", m_Durata);
                writer.WriteTag("SpeseFisse", m_SpeseFisse);
                writer.WriteTag("SpeseCoefficiente", m_SpeseCoefficiente);
                writer.WriteTag("Interessi", m_Interessi);
                writer.WriteTag("Rivalsa", m_Rivalsa);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CessionarioID":
                        {
                            m_CessionarioID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoContrattoID":
                        {
                            m_TipoContrattoID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoRapportoID":
                        {
                            m_TipoRapportoID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "costoPrestitalia":
                        {
                            m_costoPrestitalia = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TipoCostoPrestitalia":
                        {
                            m_TipoCostoPrestitalia = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CostoAmministrazione":
                        {
                            m_CostoAmministrazione = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TipoCostoAmministrazione":
                        {
                            m_TipoCostoAmministrazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCalcoloTEG":
                        {
                            m_TipoCalcoloTEG = (TipoCalcoloTEG)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Scomposizione":
                        {
                            m_Scomposizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = (double[])DMD.Arrays.Convert<double>(fieldValue);
                            break;
                        }

                    case "ImpostaSostitutiva":
                        {
                            m_ImpostaSostitutiva = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvigioneMassima":
                        {
                            m_ProvvigioneMassima = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "InBando":
                        {
                            m_InBando = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "SpeseFisse":
                        {
                            m_SpeseFisse = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "SpeseCoefficiente":
                        {
                            m_SpeseCoefficiente = (double[])DMD.Arrays.Convert<double>(fieldValue);
                            break;
                        }

                    case "CostoPerRata":
                        {
                            m_CostoPerRata = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Interessi":
                        {
                            m_Interessi = (double[])DMD.Arrays.Convert<double>(fieldValue);
                            break;
                        }

                    case "Rappel":
                        {
                            m_Rappel = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Costo":
                        {
                            m_Costo = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rivalsa":
                        {
                            m_Rivalsa = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Visible":
                        {
                            m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDStatoIniziale":
                        {
                            m_IDStatoIniziale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Provvigioni":
                        {
                            m_Provvigioni = (CCQSPDTipoProvvigioneCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            m_Provvigioni.SetGruppoProdotti(this);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override bool IsChanged()
            {
                return base.IsChanged() || m_Documenti is object && m_Documenti.IsChanged();
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret && m_Documenti is object)
                    m_Documenti.Save(force);
                if (ret && m_Provvigioni is object)
                    m_Provvigioni.Save(force);
                return ret;
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                GruppiProdotto.UpdateCached(this);
            }

            public override void InitializeFrom(object obj)
            {
                CGruppoProdotti value = (CGruppoProdotti)obj;
                base.InitializeFrom(value);
                m_Durata = DMD.Arrays.Clone(m_Durata);
                m_SpeseFisse = DMD.Arrays.Clone(m_SpeseFisse);
                m_SpeseCoefficiente = DMD.Arrays.Clone(m_SpeseCoefficiente);
                m_Interessi = DMD.Arrays.Clone(m_Interessi);
                m_Rivalsa = DMD.Arrays.Clone(m_Rivalsa);
                m_Documenti = new CDocumentiXGruppoProdottiCollection();
                m_Documenti.SetOwner(this);
                foreach (CDocumentoXGruppoProdotti doc1 in value.Documenti)
                {
                    var doc = new CDocumentoXGruppoProdotti();
                    doc.InitializeFrom(doc1);
                    m_Documenti.Add(doc);
                }

                m_Provvigioni = new CCQSPDTipoProvvigioneCollection();
                m_Provvigioni.SetGruppoProdotti(this);
                foreach (CCQSPDTipoProvvigione provv1 in value.Provvigioni)
                {
                    var provv = new CCQSPDTipoProvvigione();
                    provv.InitializeFrom(provv1);
                    m_Provvigioni.Add(provv);
                }
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}