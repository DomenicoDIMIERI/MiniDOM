Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports System.Runtime.Serialization.Formatters.Binary
Imports minidom
Imports minidom.Databases

Public NotInheritable Class Sistema
    Private Shared m_Module As CModule

    'Public Const PWDMINLEN As Integer = 6     'Lunghezza minima di una password
    Public Const DEFAULT_PASSWORD_INTERVAL = 60 '60 days
    Public Const SESSIONTIMEOUT As Integer = 120

    Private Sub New()
    End Sub


    Private Shared m_ApplicationContext As IApplicationContext

    Public Shared ReadOnly Property ApplicationContext As IApplicationContext
        Get
            Return m_ApplicationContext
        End Get
    End Property

    Public Shared Sub SetApplicationContext(ByVal value As IApplicationContext)
        m_ApplicationContext = value
    End Sub



    ''' <summary>
    ''' Inizializza la libreria
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Initialize()
        If Not Groups.KnownGroups.Administrators.Members.Contains(Users.KnownUsers.GlobalAdmin) Then
            Groups.KnownGroups.Administrators.Members.Add(Users.KnownUsers.GlobalAdmin)
        End If
        If Not Groups.KnownGroups.Guests.Members.Contains(Users.KnownUsers.GuestUser) Then
            Groups.KnownGroups.Guests.Members.Add(Users.KnownUsers.GuestUser)
        End If

        Sistema.Types.Initialize

    End Sub


#Region "Settings"

    Public NotInheritable Class Settings



        Private Shared m_PWDPATTERN As String = "^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
        Private Shared m_PWDMINLEN As Integer = 8

        Private Sub New()

        End Sub

        Public Shared Property PWDMINLEN As Integer
            Get
                Return m_PWDMINLEN
            End Get
            Set(value As Integer)
                If (value = m_PWDMINLEN) Then Exit Property
                m_PWDMINLEN = value
            End Set
        End Property

        Public Shared Property PWDPATTERN As String
            Get
                Return m_PWDPATTERN
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                If (m_PWDPATTERN = value) Then Return
                m_PWDPATTERN = value
            End Set
        End Property

    End Class

#End Region

    Public Shared Function IIF(Of T)(ByVal cond As Boolean, ByVal trueVal As T, ByVal falseVal As T) As T
        If (cond) Then
            Return trueVal
        Else
            Return falseVal
        End If
    End Function

    Public Shared ReadOnly Property [Module] As CModule
        Get
            If (m_Module Is Nothing) Then m_Module = Sistema.Modules.GetItemByName("modSistema")
            Return m_Module
        End Get
    End Property


    Public Shared Function TestFlag(ByVal value As Integer, ByVal fieldValue As Integer) As Boolean
        Return (value And fieldValue) = fieldValue
    End Function

    Public Shared Function SetFlag(ByVal value As Integer, ByVal fieldValue As Integer, ByVal cond As Boolean) As Integer
        If (cond = False) Then
            Return value And Not fieldValue
        Else
            Return value Or fieldValue
        End If
    End Function

    Public Shared Function TestFlag(Of T As IConvertible)(ByVal value As T, ByVal fieldValue As T) As Boolean
        Return TestFlag(Convert.ToInt32(value), Convert.ToInt32(fieldValue))
    End Function

    Public Shared Function SetFlag(Of T As IConvertible)(ByVal value As T, ByVal fieldValue As T, ByVal cond As Boolean) As T
        Dim obj As Object = SetFlag(Convert.ToInt32(value), Convert.ToInt32(fieldValue), cond)
        Return CType(obj, T)
    End Function

    Public Shared Function vbTypeName(ByVal obj As Object) As String
        'If (obj Is Nothing) Then Return vbNullString
        Return TypeName(obj)
    End Function

    Private Shared m_Formatter As New BinaryFormatter

    Public Shared Sub BinarySerialize(ByVal obj As Object, ByVal stream As System.IO.Stream)
        stream.WriteTimeout = 5000
        m_Formatter.Serialize(stream, obj)
    End Sub

    Public Shared Function BinaryDeserialize(ByVal stream As System.IO.Stream) As Object
        stream.ReadTimeout = 5000
        Dim formatter As New BinaryFormatter()
        Return m_Formatter.Deserialize(stream)
    End Function

End Class

