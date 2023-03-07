Imports Microsoft.VisualBasic
Imports minidom
Imports minidom.WebSite
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.CustomerCalls

Namespace Forms


    Public Class ControlPanelUserInfo
        Implements minidom.XML.IDMDXMLSerializable

        Public User As CUser
        Public LastLogin As CLoginHistory
        'Public Attivita As CContattoUtente
        'Public DescrizioneAttivita As String

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New
            Me.User = user
        End Sub

        Public Sub Load()
            Me.LastLogin = Me.User.CurrentLogin
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "User" : Me.User = fieldValue
                Case "LastLogin"
                    Me.LastLogin = Nothing
                    If (TypeOf (fieldValue) Is CLoginHistory) Then Me.LastLogin = fieldValue
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("User", Me.User)
            writer.WriteTag("LastLogin", Me.LastLogin)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace