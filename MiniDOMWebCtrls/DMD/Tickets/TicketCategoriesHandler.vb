
Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Office
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Web
Imports minidom.XML
Imports minidom.diallib

Namespace Forms

    Public Class TicketCategoriesHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTicketCategoryCursor()
        End Function


    End Class

    Public Class ChiamateRegistrateHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New Office.ChiamataRegistrataCursor()
        End Function


    End Class


    Public Class DialTPConfigHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DMDSIPConfigCursor()
        End Function


    End Class



End Namespace
