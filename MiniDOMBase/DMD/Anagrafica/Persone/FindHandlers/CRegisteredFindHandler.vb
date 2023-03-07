Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.XML

Partial Public Class Anagrafica


    <Serializable>
    Public Class CRegisteredFindHandler
        Inherits DBObject
        Implements IComparable, IComparable(Of CRegisteredFindHandler)


        Private m_HandlerClass As String
        Private m_EditorClass As String
        Private m_Context As String
        Private m_Priority As Integer

        Public Sub New()
            Me.m_HandlerClass = ""
            Me.m_EditorClass = ""
            Me.m_Context = ""
            Me.m_Priority = 0
        End Sub

        Public Property HandlerClass As String
            Get
                Return Me.m_HandlerClass
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_HandlerClass
                If (value = oldValue) Then Return
                Me.m_HandlerClass = value
                Me.DoChanged("HandlerClass", value, oldValue)
            End Set
        End Property

        Public Property EditorClass As String
            Get
                Return Me.m_EditorClass
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_EditorClass
                If (value = oldValue) Then Return
                Me.m_EditorClass = value
                Me.DoChanged("EditorClass", value, oldValue)
            End Set
        End Property

        Public Property Context As String
            Get
                Return Me.m_Context
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Context
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Context = value
                Me.DoChanged("Context", value, oldValue)
            End Set
        End Property

        Public Property Priority As Integer
            Get
                Return Me.m_Priority
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Priority
                If (oldValue = value) Then Return
                Me.m_Priority = value
                Me.DoChanged("Priority", value, oldValue)
            End Set
        End Property

        Public Function CompareTo(ByVal other As CRegisteredFindHandler) As Integer Implements IComparable(Of CRegisteredFindHandler).CompareTo
            Dim ret As Integer = 0
            If (ret = 0) Then ret = Me.m_Priority.CompareTo(other.m_Priority)
            If (ret = 0) Then ret = Me.m_HandlerClass.CompareTo(other.m_HandlerClass)
            Return ret
        End Function

        Private Function CompareTo(ByVal other As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(other, CRegisteredFindHandler))
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder()
            sb.Append(Me.m_HandlerClass)
            Return sb.ToString()
        End Function

        Public NotOverridable Overrides Function Equals(obj As Object) As Boolean
            Return (TypeOf (obj) Is CRegisteredFindHandler) AndAlso Me.Equals(DirectCast(obj, CRegisteredFindHandler))
        End Function

        Public Overridable Overloads Function Equals(ByVal obj As CRegisteredFindHandler) As Boolean
            Return Me.m_Context = obj.m_Context AndAlso
                   Me.m_HandlerClass = obj.m_HandlerClass AndAlso
                   Me.m_Priority = obj.m_Priority
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.m_HandlerClass.GetHashCode()
        End Function


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMRegFindHandler"
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Anagrafica.Persone.GetConnection()
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("HandlerClass", Me.m_HandlerClass)
            writer.WriteAttribute("EditorClass", Me.m_EditorClass)
            writer.WriteAttribute("Context", Me.m_Context)
            writer.WriteAttribute("Priority", Me.m_Priority)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "HandlerClass" : Me.m_HandlerClass = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EditorClass" : Me.m_EditorClass = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Context" : Me.m_Context = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Priority" : Me.m_Priority = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function DropFromDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Return MyBase.DropFromDatabase(dbConn, force)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_HandlerClass = reader.Read("HandlerClass", Me.m_HandlerClass)
            Me.m_EditorClass = reader.Read("EditorClass", Me.m_HandlerClass)
            Me.m_Context = reader.Read("Context", Me.m_Context)
            Me.m_Priority = reader.Read("Priority", Me.m_Priority)
            Return MyBase.LoadFromRecordset(reader)
        End Function


        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("HandlerClass", Me.m_HandlerClass)
            writer.Write("EditorClass", Me.m_EditorClass)
            writer.Write("Context", Me.m_Context)
            writer.Write("Priority", Me.m_Priority)
            Return MyBase.SaveToRecordset(writer)
        End Function
    End Class




End Class