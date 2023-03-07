Imports minidom
Imports minidom.Sistema
Imports minidom.WebSite

Imports minidom.Forms
Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Forms.Utils

Namespace Forms

    Public Class NazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CNazioniCursor
            ret.Nome.SortOrder = SortEnum.SORT_ASC
            Return ret
        End Function


    End Class


End Namespace