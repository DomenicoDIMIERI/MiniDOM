Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms

    Public Class AssicurazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(Assicurazioni.Module, ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAssicurazioniCursor
        End Function



    End Class
End Namespace