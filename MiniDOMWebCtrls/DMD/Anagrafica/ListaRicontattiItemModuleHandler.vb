Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.CustomerCalls
Imports minidom.XML

Namespace Forms

    Public Class ListaRicontattiItemModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SDelete)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ListaRicontattoItemCursor
        End Function



    End Class



End Namespace