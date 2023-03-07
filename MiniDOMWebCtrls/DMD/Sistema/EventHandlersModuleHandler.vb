Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils


Imports minidom.Anagrafica
Imports minidom.Web

Namespace Forms

    Public Class EventHandlersModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)

        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New RegisteredEventHandlerCursor
        End Function



        Public Overrides Function GetInternalItemById(id As Integer) As Object
            For Each item As RegisteredEventHandler In RegisteredEventHandlers.LoadAll
                If (GetID(item) = id) Then Return item
            Next
            Return Nothing
        End Function

    End Class


End Namespace