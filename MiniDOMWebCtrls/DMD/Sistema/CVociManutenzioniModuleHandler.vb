Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils

Imports minidom.Anagrafica

Namespace Forms

    Public Class CVociManutenzioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            'Return MyBase.GetItemById(id)
            Return Anagrafica.Manutenzioni.Voci.GetItemById(id)
        End Function


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New VociManutenzioneCursor
        End Function
    End Class


End Namespace