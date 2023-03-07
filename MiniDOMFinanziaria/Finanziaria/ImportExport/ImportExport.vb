Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals
    
    Public NotInheritable Class CImportExportClass
        Inherits CModulesClass(Of CImportExport)


        Friend Sub New()
            MyBase.New("modCQSPDImportExport", GetType(CImportExportCursor))
        End Sub

        Public Function GetItemByKey(ByVal src As CImportExportSource, ByVal sharedKey As String) As CImportExport
            If (src Is Nothing) Then Throw New ArgumentNullException("src")
            If (sharedKey = "") Then Return Nothing
            Dim cursor As New CImportExportCursor

            cursor.IgnoreRights = True
            cursor.SourceID.Value = GetID(src)
            cursor.SharedKey.Value = sharedKey
            cursor.ID.SortOrder = SortEnum.SORT_DESC
            Dim ret As CImportExport = cursor.Item
            cursor.Dispose()

            Return ret
        End Function


    End Class

End Namespace


Partial Public Class Finanziaria



    Private Shared m_ImportExport As CImportExportClass = Nothing

    Public Shared ReadOnly Property ImportExport As CImportExportClass
        Get
            If (m_ImportExport Is Nothing) Then m_ImportExport = New CImportExportClass
            Return m_ImportExport
        End Get
    End Property

End Class