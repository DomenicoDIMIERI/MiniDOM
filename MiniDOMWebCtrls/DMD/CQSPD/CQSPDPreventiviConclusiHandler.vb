Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office

Namespace Forms

    Public Class CQSPDPreventiviConclusiHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPraticheCQSPDCursor
        End Function



    End Class


End Namespace