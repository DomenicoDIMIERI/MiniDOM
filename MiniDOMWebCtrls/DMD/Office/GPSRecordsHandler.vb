Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Office
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms
 
    Public Class GPSRecordsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New GPSRecordCursor
        End Function

    End Class


End Namespace