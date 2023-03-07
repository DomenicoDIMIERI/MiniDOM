Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms
     
    Public Class CQSPDPraticheXFonteHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()

        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPraticheCQSPDCursor
        End Function

    End Class


End Namespace