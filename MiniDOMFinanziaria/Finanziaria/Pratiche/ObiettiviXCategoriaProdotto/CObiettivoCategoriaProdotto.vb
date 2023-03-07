Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica


Partial Public Class Finanziaria



    ''' <summary>
    ''' Rappresenta la specifica di un obiettivo per un ufficio
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CObiettivoCategoriaProdotto
        Inherits DBObjectPO
        Implements IComparable

        Private Const FLAG_ATTIVO As Integer = 1

        Private m_Nome As String
        Private m_TipoObiettivo As TipoObiettivo
        Private m_PeriodicitaObiettivo As PeriodicitaObiettivo

        Private m_IDCategoria As Integer
        Private m_Categoria As CCategoriaProdotto
        Private m_NomeGruppo As String

        Private m_Percentuale As Double


        Private m_DataInizio As Date?
        Private m_DataFine As Date?

        Private m_Flags As Integer
        Private m_Descrizione As String

        Private m_Attributi As CKeyCollection


        Public Sub New()
            Me.m_Nome = ""
            Me.m_IDCategoria = 0
            Me.m_Categoria = Nothing
            Me.m_TipoObiettivo = TipoObiettivo.Liquidato
            Me.m_PeriodicitaObiettivo = PeriodicitaObiettivo.Mensile
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Descrizione = ""
            Me.m_Attributi = Nothing
            Me.m_NomeGruppo = ""
            Me.m_Percentuale = 0
            Me.m_Flags = FLAG_ATTIVO
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del gruppo di categorie prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeGruppo As String
            Get
                Return Me.m_NomeGruppo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeGruppo
                If (oldValue = value) Then Return
                Me.m_NomeGruppo = value
                Me.DoChanged("NomeGruppo", value, oldValue)
            End Set
        End Property

        Public Property IDCategoria As Integer
            Get
                Return GetID(Me.m_Categoria, Me.m_IDCategoria)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCategoria
                If (oldValue = value) Then Return
                Me.m_IDCategoria = value
                Me.m_Categoria = Nothing
                Me.DoChanged("IDCategoria", value, oldValue)
            End Set
        End Property

        Public Property Categoria As CCategoriaProdotto
            Get
                If (Me.m_Categoria Is Nothing) Then Me.m_Categoria = Finanziaria.CategorieProdotto.GetItemById(Me.m_IDCategoria)
                Return Me.m_Categoria
            End Get
            Set(value As CCategoriaProdotto)
                Dim oldValue As CCategoriaProdotto = Me.Categoria
                If (oldValue Is value) Then Return
                Me.m_Categoria = value
                Me.m_IDCategoria = GetID(value)
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale 
        ''' </summary>
        ''' <returns></returns>
        Public Property Percentuale As Double
            Get
                Return Me.m_Percentuale
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Percentuale
                If (oldValue = value) Then Return
                Me.m_Percentuale = value
                Me.DoChanged("Percentuale", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property



        ''' <summary>
        ''' Restituisce o imposta il nome dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoObiettivo As TipoObiettivo
            Get
                Return Me.m_TipoObiettivo
            End Get
            Set(value As TipoObiettivo)
                Dim oldValue As TipoObiettivo = Me.m_TipoObiettivo
                If (oldValue = value) Then Exit Property
                Me.m_TipoObiettivo = value
                Me.DoChanged("TipoObiettivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la periodicità dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PeriodicitaObiettivo As PeriodicitaObiettivo
            Get
                Return Me.m_PeriodicitaObiettivo
            End Get
            Set(value As PeriodicitaObiettivo)
                Dim oldValue As PeriodicitaObiettivo = Me.m_PeriodicitaObiettivo
                If (oldValue = value) Then Exit Property
                Me.m_PeriodicitaObiettivo = value
                Me.DoChanged("PeriodicitaObiettivo", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Date?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Date?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'obiettivo è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, FLAG_ATTIVO)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, FLAG_ATTIVO, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero se l'obiettivo è attivo e valido alla data di oggi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid() As Boolean
            Return Me.IsValid(DateUtils.Now)
        End Function

        ''' <summary>
        ''' Restituisce vero se l'obiettivo è attivo e valido alla data indicata
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid(ByVal d As Date) As Boolean
            Return Me.Attivo AndAlso DateUtils.CheckBetween(d, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Finanziaria.ObiettiviXCategoria.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDObiettiviXCat"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Finanziaria.ObiettiviXCategoria.UpdateCached(Me)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_TipoObiettivo = reader.Read("TipoObiettivo", Me.m_TipoObiettivo)
            Me.m_PeriodicitaObiettivo = reader.Read("PeriodicitaObiettivo", Me.m_PeriodicitaObiettivo)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDCategoria = reader.Read("IDCategoria", Me.m_IDCategoria)
            Me.m_Percentuale = reader.Read("Percentuale", Me.m_Percentuale)
            Me.m_NomeGruppo = reader.Read("NomeGruppo", Me.m_NomeGruppo)
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("TipoObiettivo", Me.m_TipoObiettivo)
            writer.Write("PeriodicitaObiettivo", Me.m_PeriodicitaObiettivo)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDCategoria", Me.IDCategoria)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("Percentuale", Me.m_Percentuale)
            writer.Write("NomeGruppo", Me.m_NomeGruppo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("TipoObiettivo", Me.m_TipoObiettivo)
            writer.WriteAttribute("PeriodicitaObiettivo", Me.m_PeriodicitaObiettivo)
            'writer.WriteAttribute("ValoreObiettivo", Me.m_ValoreObiettivo)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDCategoria", Me.IDCategoria)
            writer.WriteAttribute("Percentuale", Me.m_Percentuale)
            writer.WriteAttribute("NomeGruppo", Me.m_NomeGruppo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoObiettivo" : Me.m_TipoObiettivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PeriodicitaObiettivo" : Me.m_PeriodicitaObiettivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case "IDCategoria" : Me.m_IDCategoria = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Percentuale" : Me.m_Percentuale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NomeGruppo" : Me.m_NomeGruppo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal obj As CObiettivoCategoriaProdotto) As Integer
            Dim a1 As Integer = IIf(Me.Attivo, 0, 1)
            Dim a2 As Integer = IIf(obj.Attivo, 0, 1)
            Dim ret As Integer = a1 - a1
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataInizio, obj.m_DataInizio)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataFine, obj.m_DataFine)
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, obj.m_Nome)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function
    End Class




End Class
