Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Sistema

Namespace Internals

    <Serializable>
    Public NotInheritable Class CModulesModeClass
        Inherits CModulesClass(Of CModule)

        Public ReadOnly actionsLock As New Object

        Public Event ModuleCreated(ByVal e As ModuleEventArgs)
        Public Event ModuleDeleted(ByVal e As ModuleEventArgs)


        <NonSerialized> Private m_DefinedActions As CDefinedActions

        Friend Sub New()
            MyBase.New("modModuli", GetType(CModulesCursor), -1)
        End Sub



        Friend Sub OnModuleCreated(ByVal e As ModuleEventArgs)
            RaiseEvent ModuleCreated(e)
        End Sub

        Friend Sub OnModuleDeleted(ByVal e As ModuleEventArgs)
            RaiseEvent ModuleDeleted(e)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione delle azioni definite su tutti i moduli
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefinedActions As CDefinedActions
            Get
                SyncLock actionsLock
                    If Me.m_DefinedActions Is Nothing Then
                        Me.m_DefinedActions = New CDefinedActions
                        Me.m_DefinedActions.Load()
                    End If
                    Return Me.m_DefinedActions
                End SyncLock
            End Get
        End Property

        Public Overrides Sub Initialize()
            MyBase.Initialize()
            For Each m As CModule In Me.LoadAll
                m.InitializeStandardActions()
            Next
        End Sub

        ''' <summary>
        ''' Restituisce i moduli visibili all'utente
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUserModules(ByVal user As CUser) As CCollection(Of CModule)
            Dim ret As New CCollection(Of CModule)
            For Each m As CModule In Me.LoadAll
                If (m.Stato = ObjectStatus.OBJECT_VALID AndAlso m.Visible AndAlso m.ParentID = 0 AndAlso m.IsVisibleToUser(user)) Then
                    ret.Add(m)
                End If
            Next
            ret.Sort()
            Return ret
        End Function


        Public Function GetItemByName(ByVal value As String) As CModule
            value = Trim(value)
            If (value = "") Then Return Nothing
            For Each m As CModule In Me.LoadAll
                If Strings.Compare(m.ModuleName, value) = 0 Then Return m
            Next
            Return Nothing
        End Function

    End Class
End Namespace

Partial Public Class Sistema

    Private Shared m_Modules As CModulesModeClass = Nothing

    Public Shared ReadOnly Property Modules As CModulesModeClass
        Get
            If (m_Modules Is Nothing) Then m_Modules = New CModulesModeClass
            Return m_Modules
        End Get
    End Property



End Class