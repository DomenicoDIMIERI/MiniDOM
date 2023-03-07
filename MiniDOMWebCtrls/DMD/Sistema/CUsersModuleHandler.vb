Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils

Imports minidom.Anagrafica
Imports minidom.XML

Namespace Forms

    'Handler del module Utenti
    Public Class CUsersModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            'Return MyBase.GetItemById(id)
            Return Sistema.Users.GetItemById(id)
        End Function



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CUserCursor
            cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
            Return cursor
        End Function



    End Class


End Namespace