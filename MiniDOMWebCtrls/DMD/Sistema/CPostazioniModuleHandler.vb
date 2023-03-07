Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils

Imports minidom.Anagrafica

Namespace Forms

    'Handler del module Utenti
    Public Class CPostazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPostazioniCursor
        End Function
    End Class


End Namespace