using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Office;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Azione da effettuare
    /// </summary>
    /// <remarks></remarks>
        public enum DocumentoXProdottoDisposition : int
        {
            /// <summary>
        /// Costituisce semplicemente una verifica
        /// </summary>
        /// <remarks></remarks>
            VERIFICA = 0,
            /// <summary>
        /// Carica a sistema
        /// </summary>
        /// <remarks></remarks>
            CARICA = 1,
            /// <summary>
        /// Stampa
        /// </summary>
        /// <remarks></remarks>
            STAMPA = 2,
            /// <summary>
        /// Firma
        /// </summary>
        /// <remarks></remarks>
            FIRMA = 3,
            /// <summary>
        /// Spedisci
        /// </summary>
        /// <remarks></remarks>
            SPEDISCI = 4,
            /// <summary>
        /// Archivia
        /// </summary>
        /// <remarks></remarks>
            ARCHIVIA = 5,
            /// <summary>
        /// Distruggi
        /// </summary>
        /// <remarks></remarks>
            DISTRUGGI = 6,

            /// <summary>
        /// Il vincolo indica che la pratica richiede l'approvazione di un supervisore
        /// </summary>
        /// <remarks></remarks>
            APPROVA = 7
        }

        /// <summary>
    /// Flags per i vincoli sui prodotti
    /// </summary>
    /// <remarks></remarks>
        [Flags]
        public enum VincoliProdottoFlags : int
        {
            /// <summary>
        /// Nessun flag
        /// </summary>
        /// <remarks></remarks>
            None = 0,

            /// <summary>
        /// Se vero indica che il vincolo è visibile nella scheda amministrazione
        /// </summary>
        /// <remarks></remarks>
            Visible = 1
        }

        /// <summary>
        /// Rappresenta un documento caricabile per un prodotto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CDocumentoXGruppoProdotti 
            : Databases.DBObject, IComparable
        {
            private string m_Nome;                                    // Nome del vincolo
            private string m_Descrizione;                             // Descrizione del vincolo
            private int m_Progressivo;                            // Ordine di verifica del vincolo
            private string m_Espressione;                             // Espressione che viene eseguita nel contesto client per veificare il vincolo
            private int m_IDGruppoProdotti;                       // ID del gruppo prodotti a cui si applica il vincolo
            [NonSerialized] private CGruppoProdotti m_GruppoProdotti;                 // Gruppo prodotti a cui si applica il vincolo
            private int m_IDDocumento;                            // ID del documento caricato
            [NonSerialized] private DocumentoCaricato m_Documento;                           // Documento caricato
            private DocumentoXProdottoDisposition m_Disposizione;     // Azione che deve essere eseguita affinchè il vincolo sia verificato
            private bool m_Richiesto;                              // Vero se il verificarsi del vincolo è obbligatorio
            private int m_IDStatoPratica;                         // ID dello stato pratica a cui si applica il vincolo
            [NonSerialized] private CStatoPratica m_StatoPratica;                     // Stato pratica a cui si applica il vincolo
            private VincoliProdottoFlags m_Flags;                     // Flags

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDocumentoXGruppoProdotti()
            {
                m_Nome = "";
                m_Descrizione = "";
                m_Progressivo = 0;
                m_Espressione = "";
                m_IDGruppoProdotti = 0;
                m_GruppoProdotti = null;
                m_IDDocumento = 0;
                m_Documento = null;
                m_Disposizione = DocumentoXProdottoDisposition.VERIFICA;
                m_Richiesto = false;
                m_IDStatoPratica = 0;
                m_StatoPratica = null;
                m_Flags = VincoliProdottoFlags.Visible;
            }

            public override CModulesClass GetModule()
            {
                return VincoliProdotto.Module;
            }

            /// <summary>
        /// Restituisce o imposta dei flags aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public VincoliProdottoFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del vincolo
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
        /// Restituisce o imposta la descrizione del vincolo
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Descrizione;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ordine di priorità del vincolo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Progressivo
            {
                get
                {
                    return m_Progressivo;
                }

                set
                {
                    int oldValue = m_Progressivo;
                    if (oldValue == value)
                        return;
                    m_Progressivo = value;
                    DoChanged("Progressivo", value, oldValue);
                }
            }

            protected internal void SetProgressivo(int value)
            {
                m_Progressivo = value;
            }

            /// <summary>
        /// Restituisce o imposta il codice che viene eseguito nel cliente per verificare il vincolo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Espressione
            {
                get
                {
                    return m_Espressione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Espressione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Espressione = value;
                    DoChanged("Espressione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'azione da compiere per il documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DocumentoXProdottoDisposition Disposizione
            {
                get
                {
                    return m_Disposizione;
                }

                set
                {
                    var oldValue = m_Disposizione;
                    if (oldValue == value)
                        return;
                    m_Disposizione = value;
                    DoChanged("Disposizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del prodotto associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDGruppoProdotti
            {
                get
                {
                    return DBUtils.GetID(m_GruppoProdotti, m_IDGruppoProdotti);
                }

                set
                {
                    int oldValue = IDGruppoProdotti;
                    if (oldValue == value)
                        return;
                    m_IDGruppoProdotti = value;
                    m_GruppoProdotti = null;
                    DoChanged("IDGruppoProdotti", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il gruppo prodotti associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CGruppoProdotti GruppoProdotti
            {
                get
                {
                    if (m_GruppoProdotti is null)
                        m_GruppoProdotti = GruppiProdotto.GetItemById(m_IDGruppoProdotti);
                    return m_GruppoProdotti;
                }

                set
                {
                    var oldValue = m_GruppoProdotti;
                    if (oldValue == value)
                        return;
                    m_GruppoProdotti = value;
                    m_IDGruppoProdotti = DBUtils.GetID(value);
                    DoChanged("GruppoProdotti", value, oldValue);
                }
            }

            protected internal virtual void SetGruppoProdotti(CGruppoProdotti value)
            {
                m_GruppoProdotti = value;
                m_IDGruppoProdotti = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID del documento associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDDocumento
            {
                get
                {
                    return DBUtils.GetID(m_Documento, m_IDDocumento);
                }

                set
                {
                    int oldValue = IDDocumento;
                    if (oldValue == value)
                        return;
                    m_IDDocumento = value;
                    m_Documento = null;
                    DoChanged("IDDocumento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il documento associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DocumentoCaricato Documento
            {
                get
                {
                    if (m_Documento is null)
                        m_Documento = GDE.GetItemById(m_IDDocumento);
                    return m_Documento;
                }

                set
                {
                    var oldValue = m_Documento;
                    if (oldValue == value)
                        return;
                    m_Documento = value;
                    m_IDDocumento = DBUtils.GetID(value);
                    this.DoChanged("Documento", value, oldValue);
                }
            }

            // Private Function GetStatoIndex(ByVal stato As StatoPraticaEnum) As Integer
            // Select Case stato
            // Case StatoPraticaEnum.STATO_CONTATTO : Return 0
            // Case StatoPraticaEnum.STATO_RICHIESTADELIBERA : Return 1
            // Case StatoPraticaEnum.STATO_DELIBERATA : Return 2
            // Case StatoPraticaEnum.STATO_LIQUIDATA : Return 3
            // Case StatoPraticaEnum.STATO_ARCHIVIATA : Return 4
            // Case StatoPraticaEnum.STATO_ANNULLATA : Return 5
            // Case Else : Throw New ArgumentOutOfRangeException("Stato non valido: " & [Enum].GetName(GetType(StatoPraticaEnum), stato))
            // End Select
            // End Function

            // Public Property Richiesto(ByVal stato As StatoPraticaEnum) As Boolean
            // Get
            // Return Me.m_RichiestoPerStati(Me.GetStatoIndex(stato))
            // End Get
            // Set(value As Boolean)
            // Me.m_RichiestoPerStati(Me.GetStatoIndex(stato)) = value
            // End Set
            // End Property

            // Private Function GetStrRichiesto() As String
            // Dim str As String = vbNullString
            // If (Me.Richiesto(StatoPraticaEnum.STATO_CONTATTO)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_CONTATTO, ",")
            // If (Me.Richiesto(StatoPraticaEnum.STATO_RICHIESTADELIBERA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_RICHIESTADELIBERA, ",")
            // If (Me.Richiesto(StatoPraticaEnum.STATO_DELIBERATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_DELIBERATA, ",")
            // If (Me.Richiesto(StatoPraticaEnum.STATO_LIQUIDATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_LIQUIDATA, ",")
            // If (Me.Richiesto(StatoPraticaEnum.STATO_ARCHIVIATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_ARCHIVIATA, ",")
            // If (Me.Richiesto(StatoPraticaEnum.STATO_ANNULLATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_ANNULLATA, ",")
            // Return str
            // End Function

            // Private Sub FromStrRichiesto(ByVal text As String)
            // For i As Integer = 0 To UBound(Me.m_RichiestoPerStati)
            // Me.m_RichiestoPerStati(i) = False
            // Next
            // text = Trim(text)
            // If (text <> "") Then
            // Dim str() As String = Split(text, ",")
            // For i As Integer = 0 To UBound(str)
            // Me.m_RichiestoPerStati(Me.GetStatoIndex(Formats.ToInteger(str(i)))) = True
            // Next
            // End If
            // End Sub

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se questa azione è necessaria
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Richiesto
            {
                get
                {
                    return m_Richiesto;
                }

                set
                {
                    if (m_Richiesto == value)
                        return;
                    m_Richiesto = value;
                    DoChanged("Richiesto", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dello stato a cui si applica la regola.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDStatoPratica
            {
                get
                {
                    return DBUtils.GetID(m_StatoPratica, m_IDStatoPratica);
                }

                set
                {
                    int oldValue = IDStatoPratica;
                    if (oldValue == value)
                        return;
                    m_IDStatoPratica = value;
                    m_StatoPratica = null;
                    DoChanged("IDStatoPratica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato pratica a cui si applica la regola (se NULL la regola si applica a tutti gli stati)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoPratica StatoPratica
            {
                get
                {
                    if (m_StatoPratica is null)
                        m_StatoPratica = StatiPratica.GetItemById(m_IDStatoPratica);
                    return m_StatoPratica;
                }

                set
                {
                    var oldValue = m_StatoPratica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoPratica = value;
                    m_IDStatoPratica = DBUtils.GetID(value);
                    DoChanged("StatoPratica", value, oldValue);
                }
            }

            public override string ToString()
            {
                return m_Nome;
            }

            public override string GetTableName()
            {
                return "tbl_DocumentiXGruppoProdotti";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDGruppoProdotti = reader.Read("GruppoProdotti", m_IDGruppoProdotti);
                m_IDDocumento = reader.Read("Documento", m_IDDocumento);
                m_Disposizione = reader.Read("Disposizione", m_Disposizione);
                m_Richiesto = reader.Read("Richiesto", m_Richiesto);
                m_IDStatoPratica = reader.Read("IDStatoPratica", m_IDStatoPratica);
                m_Progressivo = reader.Read("Progressivo", m_Progressivo);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_Nome = reader.Read("Nome", m_Nome);
                m_Espressione = reader.Read("Espressione", m_Espressione);
                m_Flags = reader.Read("Flags", m_Flags);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("GruppoProdotti", IDGruppoProdotti);
                writer.Write("Documento", IDDocumento);
                writer.Write("Disposizione", m_Disposizione);
                writer.Write("Richiesto", m_Richiesto);
                writer.Write("IDStatoPratica", IDStatoPratica);
                writer.Write("Progressivo", m_Progressivo);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Nome", m_Nome);
                writer.Write("Espressione", m_Espressione);
                writer.Write("Flags", m_Flags);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDGruppoProdotti", IDGruppoProdotti);
                writer.WriteAttribute("IDDocumento", IDDocumento);
                writer.WriteAttribute("Disposizione", (int?)m_Disposizione);
                writer.WriteAttribute("Richiesto", m_Richiesto);
                writer.WriteAttribute("IDStatoPratica", IDStatoPratica);
                writer.WriteAttribute("Progressivo", m_Progressivo);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("Espressione", m_Espressione);
                writer.WriteTag("Descrizione", m_Descrizione);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDGruppoProdotti":
                        {
                            m_IDGruppoProdotti = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDocumento":
                        {
                            m_IDDocumento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Disposizione":
                        {
                            m_Disposizione = (DocumentoXProdottoDisposition)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Richiesto":
                        {
                            m_Richiesto = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDStatoPratica":
                        {
                            m_IDStatoPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Progressivo":
                        {
                            m_Progressivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Espressione":
                        {
                            m_Espressione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (VincoliProdottoFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                VincoliProdotto.UpdateCached(this);
                if (GruppoProdotti is object)
                {
                    lock (GruppoProdotti)
                    {
                        var item = GruppoProdotti.Documenti.GetItemById(DBUtils.GetID(this));
                        int i = -1;
                        if (item is object)
                            i = GruppoProdotti.Documenti.IndexOf(item);
                        if (Stato == ObjectStatus.OBJECT_VALID)
                        {
                            if (i < 0)
                            {
                                GruppoProdotti.Documenti.Add(this);
                            }
                            else
                            {
                                GruppoProdotti.Documenti[i] = this;
                            }

                            GruppoProdotti.Documenti.Sort();
                        }
                        else if (i >= 0)
                            GruppoProdotti.Documenti.RemoveAt(i);
                    }
                }
            }

            public int CompareTo(CDocumentoXGruppoProdotti obj)
            {
                int ret = DMD.Arrays.Compare(m_Progressivo, obj.m_Progressivo);
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CDocumentoXGruppoProdotti)obj);
            }
        }
    }
}