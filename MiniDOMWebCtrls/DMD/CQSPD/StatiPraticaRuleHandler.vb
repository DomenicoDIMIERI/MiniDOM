Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Web

Namespace Forms

    Public Class StatiPraticaRuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CStatoPratRuleCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Finanziaria.StatiPratRules.GetItemById(id)
        End Function

    End Class


End Namespace