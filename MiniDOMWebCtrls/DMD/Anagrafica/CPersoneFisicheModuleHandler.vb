Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.XML

Namespace Forms


    Public Class CPersoneFisicheModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CPersonaCursor
            ret.Nominativo.SortOrder = SortEnum.SORT_ASC
            ret.TipoPersona.Value = TipoPersona.PERSONA_FISICA
            Return ret
        End Function


    End Class

End Namespace