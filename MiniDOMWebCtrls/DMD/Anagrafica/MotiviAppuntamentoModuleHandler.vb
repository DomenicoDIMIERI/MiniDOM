Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils



Namespace Forms


    Public Class MotiviAppuntamentoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New MotiviAppuntamentoCursor
        End Function


    End Class


End Namespace