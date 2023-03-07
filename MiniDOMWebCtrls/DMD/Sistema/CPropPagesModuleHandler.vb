Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils


Namespace Forms




    '--------------------------------------------------------
    Public Class CPropPagesModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New CRegisteredPropertyPageCursor
        End Function

        'Public Overrides Function GetEditor() As Object
        '    Return Nothing
        'End Function

        'Public Overrides Function delete(ByVal renderer As Object) As String
        '    Dim item As CRegisteredPropertyPage
        '    Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", "0"))
        '    Dim ret As String
        '    item = Sistema.PropertyPages.GetItemById(itemID)
        '    ret = MyBase.delete(renderer)
        '    'Sistema.PropertyPages.CachedItems.Remove(item)
        '    Return ret
        'End Function

        'Public Overrides ReadOnly Property SupportsCreate As Boolean
        '    Get
        '        Return True
        '    End Get
        'End Property

        'Public Overrides ReadOnly Property SupportsEdit As Boolean
        '    Get
        '        Return True
        '    End Get
        'End Property

        'Public Overrides ReadOnly Property SupportsDelete As Boolean
        '    Get
        '        Return True
        '    End Get
        'End Property

        Public Overrides Function GetInternalItemById(ByVal id As Integer) As Object
            Return Sistema.PropertyPages.GetItemById(id)
        End Function


    End Class


End Namespace