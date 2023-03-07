Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports System.Threading
Imports System.Web.UI.HtmlControls
Imports System.IO

Partial Class WebSite



    Public Class UploadCalcelledException
        Inherits System.Exception

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class