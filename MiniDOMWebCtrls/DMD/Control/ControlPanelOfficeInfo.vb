Imports Microsoft.VisualBasic
Imports minidom
Imports minidom.Anagrafica
Imports minidom.WebSite
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.CustomerCalls


Namespace Forms
 

    Public Class ControlPanelOfficeInfo
        Implements minidom.XML.IDMDXMLSerializable

        Public Ufficio As CUfficio
        Public Utenti As CCollection(Of ControlPanelUserInfo)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal ufficio As CUfficio)
            Me.New
            Me.Ufficio = ufficio
            Me.Utenti = New CCollection(Of ControlPanelUserInfo)
        End Sub

        Public Sub Load(ByVal fromDate As Date)
            Me.Utenti = New CCollection(Of ControlPanelUserInfo)
            For Each u As CUser In Sistema.Users.LoadAll
                If (u.IsValid AndAlso u.CurrentLogin.IDUfficio = GetID(Me.Ufficio)) Then
                    Dim info As New ControlPanelUserInfo(u)
                    info.Load()
                    Me.Utenti.Add(info)
                End If
            Next


        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Ufficio" : Me.Ufficio = fieldValue
                Case "Utenti"
                    Me.Utenti = New CCollection(Of ControlPanelUserInfo)
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.Utenti.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is ControlPanelUserInfo) Then
                        Me.Utenti.Add(fieldValue)
                    End If
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("Ufficio", Me.Ufficio)
            writer.WriteTag("Utenti", Me.Utenti)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace