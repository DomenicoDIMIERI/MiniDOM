Imports minidom
Imports minidom.Sistema
Imports minidom.Databases



Partial Public Class Anagrafica

    ''' <summary>
    ''' Flags per un Motivo Contatto
    ''' </summary>
    <Flags>
    Public Enum MotivoContattoFlags As Integer
        None = 0

        ''' <summary>
        ''' Attivo
        ''' </summary>
        Attivo = 1


        ''' <summary>
        ''' Valido per le persone fisiche
        ''' </summary>
        PersoneFisiche = 2

        ''' <summary>
        ''' Valido per le persone giuridiche
        ''' </summary>
        PersoneGiuridiche = 4

        ''' <summary>
        ''' Lo scopo é valido per i contatti in uscita
        ''' </summary>
        InUscita = 8

        ''' <summary>
        ''' Lo scopo é valido per i contatti in ingresso
        ''' </summary>
        InEntrata = 16
    End Enum

    ''' <summary>
    ''' Rappresenta un elemento nel menu di scelta a tendina per lo scopo dei contatti (telefonate, visite, ecc..)
    ''' </summary>
    <Serializable>
    Public Class MotivoContatto
        Inherits DBObject

        Private m_Nome As String
        Private m_Descrizione As String
        Private m_Flags As MotivoContattoFlags
        Private m_TipoContatto As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_Flags = MotivoContattoFlags.Attivo
            Me.m_TipoContatto = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del proprietario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContatto As String
            Get
                Return Me.m_TipoContatto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoContatto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContatto = value
                Me.DoChanged("TipoContatto", value, oldValue)
            End Set
        End Property

        Public Property Flags As MotivoContattoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As MotivoContattoFlags)
                Dim oldValue As MotivoContattoFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.MotiviContatto.Module
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_CRMMotiviContatto"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_TipoContatto = reader.Read("TipoContatto", Me.m_TipoContatto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("TipoContatto", Me.m_TipoContatto)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("TipoContatto", Me.m_TipoContatto)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldNome As String, fieldValue As Object)
            Select Case fieldNome
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContatto" : Me.m_TipoContatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldNome, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.MotiviContatto.UpdateCached(Me)
        End Sub

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function
    End Class


End Class