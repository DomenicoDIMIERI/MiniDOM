Imports minidom.Databases
Imports minidom.Finanziaria
Imports minidom.Anagrafica
Imports minidom.Sistema
Imports minidom.XML

Namespace Forms

    Public Class CProfiliModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CProfiliCursor
        End Function


    End Class

End Namespace