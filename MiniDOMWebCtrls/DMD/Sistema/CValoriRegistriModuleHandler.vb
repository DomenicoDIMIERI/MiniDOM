Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils

Imports minidom.Anagrafica

Namespace Forms

    'Handler del module Utenti
    Public Class CValoriRegistriModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            'Return MyBase.GetItemById(id)
            Return Anagrafica.Postazioni.ValoriRegistri.GetItemById(id)
        End Function


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ValoreRegistroCursor
        End Function
    End Class


End Namespace