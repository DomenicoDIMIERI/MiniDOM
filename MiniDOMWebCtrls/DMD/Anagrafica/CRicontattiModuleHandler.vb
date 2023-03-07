Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.CustomerCalls
Imports minidom.XML

Namespace Forms

    Public Class CRicontattiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SDelete)

            'AddHandler CustomerCalls.CRM.NuovoContatto, AddressOf Me.handleNuovaTelefonata
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRicontattiCursor
        End Function


    End Class



End Namespace