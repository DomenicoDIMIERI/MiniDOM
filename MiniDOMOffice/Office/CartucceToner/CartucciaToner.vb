Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Class Office

    <Flags>
    Public Enum CartucciaTonerFlags As Integer
        None = 0

        ''' <summary>
        ''' Il toner è effettivamente utilizzato in qualche stampante
        ''' </summary>
        InUso = 1

        ''' <summary>
        ''' Si tratta di un toner originale
        ''' </summary>
        Originale = 2

        ''' <summary>
        ''' Il toner non è più utilizzabile 
        ''' </summary>
        Dismesso = 256
    End Enum

    ''' <summary>
    ''' Rappresenta un consumabile
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CartucciaToner
        Inherits DBObjectPO

        Private m_IDArticolo As Integer
        <NonSerialized> Private m_Articolo As Articolo
        Private m_NomeArticolo As String
        Private m_CodiceArticolo As String
        Private m_Modello As String
        Private m_Descrizione As String
        Private m_IDPostazione As Integer
        <NonSerialized> Private m_Postazione As CPostazione
        Private m_NomePostazione As String
        Private m_DataAcquisto As Date?
        Private m_DataInstallazione As Date?
        Private m_DataEsaurimento As Date?
        Private m_DataRimozione As Date?
        Private m_StampeDisponibili As Integer?
        Private m_StampeEffettuate As Integer?
        Private m_Flags As CartucciaTonerFlags

        Public Sub New()
            Me.m_IDArticolo = 0
            Me.m_Articolo = Nothing
            Me.m_NomeArticolo = ""
            Me.m_CodiceArticolo = ""
            Me.m_Modello = ""
            Me.m_Descrizione = ""
            Me.m_IDPostazione = 0
            Me.m_Postazione = Nothing
            Me.m_NomePostazione = ""
            Me.m_DataAcquisto = Nothing
            Me.m_DataInstallazione = Nothing
            Me.m_DataEsaurimento = Nothing
            Me.m_DataRimozione = Nothing
            Me.m_StampeDisponibili = Nothing
            Me.m_StampeEffettuate = Nothing
            Me.m_Flags = CartucciaTonerFlags.None
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'articolo associato 
        ''' </summary>
        ''' <returns></returns>
        Public Property IDArticolo As Integer
            Get
                Return GetID(Me.m_Articolo, Me.m_IDArticolo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDArticolo
                If (oldValue = value) Then Return
                Me.m_IDArticolo = value
                Me.m_Articolo = Nothing
                Me.DoChanged("IDArticolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'articolo associato
        ''' </summary>
        ''' <returns></returns>
        Public Property Articolo As Articolo
            Get
                If (Me.m_Articolo Is Nothing) Then Me.m_Articolo = Office.Articoli.GetItemById(Me.m_IDArticolo)
                Return Me.m_Articolo
            End Get
            Set(value As Articolo)
                Dim oldValue As Articolo = Me.Articolo
                If (oldValue Is value) Then Return
                Me.m_Articolo = value
                Me.m_IDArticolo = GetID(value)
                Me.m_NomeArticolo = "" : If (value IsNot Nothing) Then Me.m_NomeArticolo = value.Nome
                Me.DoChanged("Articolo", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetArticolo(ByVal value As Articolo)
            Me.m_Articolo = value
            Me.m_IDArticolo = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome dell'articolo associato
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeArticolo As String
            Get
                Return Me.m_NomeArticolo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeArticolo
                If (oldValue = value) Then Return
                Me.m_NomeArticolo = value
                Me.DoChanged("NomeArticolo", value, oldValue)
            End Set
        End Property

        Public Property CodiceArticolo As String
            Get
                Return Me.m_CodiceArticolo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceArticolo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_CodiceArticolo = value
                Me.DoChanged("CodiceArticolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modello
        ''' </summary>
        ''' <returns></returns>
        Public Property Modello As String
            Get
                Return Me.m_Modello
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Modello
                If (oldValue = value) Then Return
                Me.m_Modello = value
                Me.DoChanged("Modello", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione
        ''' </summary>
        ''' <returns></returns>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Return
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della postazione
        ''' </summary>
        ''' <returns></returns>
        Public Property IDPostazione As Integer
            Get
                Return GetID(Me.m_Postazione, Me.m_IDPostazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPostazione
                If (oldValue = value) Then Return
                Me.m_IDPostazione = value
                Me.m_Postazione = Nothing
                Me.DoChanged("IDPostazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la postazione
        ''' </summary>
        ''' <returns></returns>
        Public Property Postazione As CPostazione
            Get
                If (Me.m_Postazione Is Nothing) Then Me.m_Postazione = Anagrafica.Postazioni.GetItemById(Me.m_IDPostazione)
                Return Me.m_Postazione
            End Get
            Set(value As CPostazione)
                Dim oldValue As CPostazione = Me.Postazione
                If (oldValue Is value) Then Return
                Me.m_Postazione = value
                Me.m_IDPostazione = GetID(value)
                Me.m_NomePostazione = "" : If (value IsNot Nothing) Then Me.m_NomePostazione = value.Nome
                Me.DoChanged("Postazione", value, oldValue)
            End Set
        End Property

        Public Property NomePostazione As String
            Get
                Return Me.m_NomePostazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePostazione
                If (oldValue = value) Then Return
                Me.m_NomePostazione = value
                Me.DoChanged("NomePostazione", value, oldValue)
            End Set
        End Property

        Public Property DataAcquisto As Date?
            Get
                Return Me.m_DataAcquisto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAcquisto
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataAcquisto = value
                Me.DoChanged("DataAcquisto", value, oldValue)
            End Set
        End Property

        Public Property DataInstallazione As Date?
            Get
                Return Me.m_DataInstallazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInstallazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInstallazione = value
                Me.DoChanged("DataInstallazione", value, oldValue)
            End Set
        End Property

        Public Property DataEsaurimento As Date?
            Get
                Return Me.m_DataEsaurimento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEsaurimento
                If (DateUtils.Compare(oldValue, value) = 0) Then Return
                Me.m_DataEsaurimento = value
                Me.DoChanged("DataEsaurimento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui il toner é stato rimosso dalla stampante
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRimozione As Date?
            Get
                Return Me.m_DataRimozione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRimozione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRimozione = value
                Me.DoChanged("DataRimozione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di stampe disponibili dichiarate
        ''' </summary>
        ''' <returns></returns>
        Public Property StampeDisponibili As Integer?
            Get
                Return Me.m_StampeDisponibili
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_StampeDisponibili
                If (oldValue = value) Then Return
                Me.m_StampeDisponibili = value
                Me.DoChanged("StampeDisponibili", value, oldValue)
            End Set
        End Property

        Public Property StampeEffettuate As Integer?
            Get
                Return Me.m_StampeEffettuate
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_StampeEffettuate
                If (oldValue = value) Then Return
                Me.m_StampeEffettuate = value
                Me.DoChanged("StampeEffettuate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As CartucciaTonerFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CartucciaTonerFlags)
                Dim oldValue As CartucciaTonerFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_CodiceArticolo
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.CartucceToners.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCartucceToner"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDArticolo = reader.Read("IDArticolo", Me.m_IDArticolo)
            Me.m_NomeArticolo = reader.Read("NomeArticolo", Me.m_NomeArticolo)
            Me.m_CodiceArticolo = reader.Read("CodiceArticolo", Me.m_CodiceArticolo)
            Me.m_Modello = reader.Read("Modello", Me.m_Modello)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDPostazione = reader.Read("IDPostazione", Me.m_IDPostazione)
            Me.m_NomePostazione = reader.Read("NomePostazione", Me.m_NomePostazione)
            Me.m_DataAcquisto = reader.Read("DataAcquisto", Me.m_DataAcquisto)
            Me.m_DataInstallazione = reader.Read("DataInstallazione", Me.m_DataInstallazione)
            Me.m_DataEsaurimento = reader.Read("DataEsaurimento", Me.m_DataEsaurimento)
            Me.m_DataRimozione = reader.Read("DataRimozione", Me.m_DataRimozione)
            Me.m_StampeDisponibili = reader.Read("StampeDisponibili", Me.m_StampeDisponibili)
            Me.m_StampeEffettuate = reader.Read("StampeEffettuate", Me.m_StampeEffettuate)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDArticolo", Me.IDArticolo)
            writer.Write("NomeArticolo", Me.m_NomeArticolo)
            writer.Write("CodiceArticolo", Me.m_CodiceArticolo)
            writer.Write("Modello", Me.m_Modello)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDPostazione", Me.IDPostazione)
            writer.Write("NomePostazione", Me.m_NomePostazione)
            writer.Write("DataAcquisto", Me.m_DataAcquisto)
            writer.Write("DataInstallazione", Me.m_DataInstallazione)
            writer.Write("DataEsaurimento", Me.m_DataEsaurimento)
            writer.Write("DataRimozione", Me.m_DataRimozione)
            writer.Write("StampeDisponibili", Me.m_StampeDisponibili)
            writer.Write("StampeEffettuate", Me.m_StampeEffettuate)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDArticolo", Me.IDArticolo)
            writer.WriteAttribute("NomeArticolo", Me.m_NomeArticolo)
            writer.WriteAttribute("CodiceArticolo", Me.m_CodiceArticolo)
            writer.WriteAttribute("Modello", Me.m_Modello)
            writer.WriteAttribute("IDPostazione", Me.IDPostazione)
            writer.WriteAttribute("NomePostazione", Me.m_NomePostazione)
            writer.WriteAttribute("DataAcquisto", Me.m_DataAcquisto)
            writer.WriteAttribute("DataInstallazione", Me.m_DataInstallazione)
            writer.WriteAttribute("DataEsaurimento", Me.m_DataEsaurimento)
            writer.WriteAttribute("DataRimozione", Me.m_DataRimozione)
            writer.WriteAttribute("StampeDisponibili", Me.m_StampeDisponibili)
            writer.WriteAttribute("StampeEffettuate", Me.m_StampeEffettuate)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDArticolo" : Me.m_IDArticolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeArticolo" : Me.m_NomeArticolo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceArticolo" : Me.m_CodiceArticolo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Modello" : Me.m_Modello = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPostazione" : Me.m_IDPostazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePostazione" : Me.m_NomePostazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAcquisto" : Me.m_DataAcquisto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInstallazione" : Me.m_DataInstallazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataEsaurimento" : Me.m_DataEsaurimento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataRimozione" : Me.m_DataRimozione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StampeDisponibili" : Me.m_StampeDisponibili = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StampeEffettuate" : Me.m_StampeEffettuate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

    End Class



End Class