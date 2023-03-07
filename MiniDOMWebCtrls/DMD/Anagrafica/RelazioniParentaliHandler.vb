Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils


Imports minidom.Web
Imports minidom.XML

Namespace Forms

    Public Class RelazioniParentaliHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRelazioneParentaleCursor
        End Function


        Public Function GetRelazioni(ByVal renderer As Object) As String
            Dim pID As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim items As CCollection(Of CRelazioneParentale) = Anagrafica.RelazioniParentali.GetRelazioni(pID)
            Return XML.Utils.Serializer.Serialize(items)
        End Function

    End Class



End Namespace