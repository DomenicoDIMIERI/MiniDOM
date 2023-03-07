Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Office
Imports minidom.Internals
Imports minidom.XML

Partial Class Office

    <Serializable>
    Public Class MailAddressCursor
        Inherits DBObjectCursorBase(Of MailAddress)

        Private m_MessageID As New CCursorField(Of Integer)("MessageID")
        Private m_FieldName As New CCursorFieldObj(Of String)("FieldName")
        Private m_Address As New CCursorFieldObj(Of String)("Address")
        Private m_DisplayName As New CCursorFieldObj(Of String)("DisplayName")

        Public Sub New()
        End Sub

        Public ReadOnly Property MessageID As CCursorField(Of Integer)
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property FieldName As CCursorFieldObj(Of String)
            Get
                Return Me.m_FieldName
            End Get
        End Property

        Public ReadOnly Property Address As CCursorFieldObj(Of String)
            Get
                Return Me.m_Address
            End Get
        End Property

        Public ReadOnly Property DisplayName As CCursorFieldObj(Of String)
            Get
                Return Me.m_DisplayName
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return minidom.Office.Mails.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MailAddresses"
        End Function


    End Class

End Class
