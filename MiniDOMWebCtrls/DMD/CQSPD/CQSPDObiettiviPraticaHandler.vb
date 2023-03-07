Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms
  
    Public Class CQSPDObiettiviPraticaHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CObiettivoPraticaCursor
            Return cursor
        End Function

         
        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Finanziaria.Obiettivi.GetItemById(id)
        End Function



    End Class




End Namespace