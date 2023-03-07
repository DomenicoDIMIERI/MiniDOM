Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Office
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Web

Namespace Forms

    Public Class GDETemplatesHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DocumentTemplateCursor
        End Function



        'Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
        '    Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
        '    ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
        '    ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
        '    Return ret
        'End Function


    End Class


End Namespace