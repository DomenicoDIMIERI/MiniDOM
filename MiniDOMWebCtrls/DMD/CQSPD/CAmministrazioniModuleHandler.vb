Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms
  

    Public Class CAmministrazioniModuleHandler
        Inherits CPersoneGiuridicheModuleHandler

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CPersonaCursor
            ret.TipoPersona.Value = TipoPersona.PERSONA_GIURIDICA
            Return ret
        End Function




    End Class


End Namespace