Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.Office
Imports minidom.WebSite
Imports minidom.Anagrafica

Imports minidom.Databases
Imports minidom.Net
Imports minidom.Web

Namespace Forms



    Public Class MailAppsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New MailApplicationCursor
        End Function



        'Public Function CreateApp(ByVal renderer As Object) As String
        '    Dim name As String = RPC.n2str(GetParameter(renderer, "nm", ""))
        '    Dim displayName As String = RPC.n2str(GetParameter(renderer, "dn", ""))
        '    Dim workingDir As String = RPC.n2str(GetParameter(renderer, "wd", ""))
        '    Dim app As MailApplication = MailApplications.Create(name, displayName, Sistema.ApplicationContext.MapPath("/App_Data/" & workingDir))
        '    Return XML.Utils.Serializer.Serialize(app)
        'End Function

    End Class



End Namespace