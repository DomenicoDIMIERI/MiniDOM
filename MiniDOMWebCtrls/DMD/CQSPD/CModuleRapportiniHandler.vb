Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Imports minidom.Web
Imports minidom.XML

Namespace Forms



    '----------------------
    Public Class CModulePraticheCQSPDHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPraticheCQSPDCursor
        End Function



    End Class


End Namespace