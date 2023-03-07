Imports minidom.Finanziaria
Imports minidom.Databases

Namespace Forms

    Public Class TipiContrattoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTipoContrattoCursor
        End Function




    End Class



End Namespace