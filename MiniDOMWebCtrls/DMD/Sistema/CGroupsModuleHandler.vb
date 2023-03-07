Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils

Imports minidom.Anagrafica
Imports minidom.XML

Namespace Forms


    Public Class CGroupsModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CGroupCursor
            cursor.GroupName.SortOrder = SortEnum.SORT_ASC
            Return cursor
        End Function


    End Class




End Namespace