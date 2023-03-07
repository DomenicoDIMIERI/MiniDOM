Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Collezione dei moduli definiti per il gruppo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CModuleXGroupCollection
        Inherits CCollection(Of CModuleXGroup)

        <NonSerialized> _
        Private m_Group As CGroup

        Public Sub New()
            Me.m_Group = Nothing
        End Sub

        Public Sub New(ByVal Group As CGroup)
            Me.New()
            Me.Load(Group)
        End Sub

        Friend Sub Load(ByVal Group As CGroup)
            If (Group Is Nothing) Then Throw New ArgumentNullException("Group")
            Me.Clear()
            Me.m_Group = Group
            Dim cursor As New CModuleXGroupCursor
            cursor.GroupID.Value = GetID(Group)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Sub

        Public Function GetItemByModule(ByVal [module] As CModule) As CModuleXGroup
            For Each item As CModuleXGroup In Me
                If (item.[Module] Is [module]) Then Return item
            Next
            Return Nothing
        End Function

        Public Sub SetAllowNegate(ByVal [module] As CModule, ByVal allow As Boolean)
            Dim item As CModuleXGroup = Me.GetItemByModule([module])
            If (item IsNot Nothing) Then
                item.Allow = allow
                item.Save()
            Else
                item = New CModuleXGroup
                item.Module = [module]
                item.Group = Me.m_Group
                item.Allow = allow
                item.Save()
                Me.Add(item)
            End If
        End Sub

        Public Sub GetAllowNegate(ByVal [module] As CModule, ByRef a As Boolean)
            Dim item As CModuleXGroup = Me.GetItemByModule([module])
            If (item IsNot Nothing) Then
                a = a Or item.Allow
            End If
        End Sub


    End Class


End Class