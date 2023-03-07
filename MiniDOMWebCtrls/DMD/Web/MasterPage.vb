Imports Microsoft.VisualBasic

Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public Sub New()
        DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMDObject.DecreaseCounter(Me)
    End Sub
End Class

