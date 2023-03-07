Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Collezione dei moduli definiti per l'utente
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CModuleXUserCollection
        Inherits CCollection(Of CModuleXUser)

        Private m_User As CUser

        Public Sub New()
            Me.m_User = Nothing
        End Sub

        Public Sub New(ByVal User As CUser)
            Me.New()
            Me.Load(User)
        End Sub

        Friend Sub Load(ByVal User As CUser)
            If (User Is Nothing) Then Throw New ArgumentNullException("User")
            Me.Clear()
            Me.m_User = User
            Dim cursor As New CModuleXUserCursor
            cursor.UserID.Value = GetID(User)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Sub

        Public Function GetItemByModule(ByVal [module] As CModule) As CModuleXUser
            For Each item As CModuleXUser In Me
                If (item.[Module] Is [module]) Then Return item
            Next
            Return Nothing
        End Function

        Public Sub SetAllowNegate(ByVal [module] As CModule, ByVal allow As Boolean)
            Dim item As CModuleXUser = Me.GetItemByModule([module])
            If (item IsNot Nothing) Then
                item.Allow = allow
                item.Save()
            Else
                item = New CModuleXUser
                item.Module = [module]
                item.User = Me.m_User
                item.Allow = allow
                item.Save()
                Me.Add(item)
            End If
        End Sub

        Public Sub GetAllowNegate(ByVal [module] As CModule, ByRef a As Boolean)
            Dim item As CModuleXUser = Me.GetItemByModule([module])
            If (item IsNot Nothing) Then
                a = a Or item.Allow
            End If
        End Sub


    End Class


End Class