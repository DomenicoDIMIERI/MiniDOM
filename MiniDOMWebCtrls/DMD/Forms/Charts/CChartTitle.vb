Imports minidom
Imports minidom.Sistema
Imports minidom.Forms


Namespace Forms

 

    Public Class CChartTitle
        Inherits CChartElement

        Public Sub New()
            Me.BackColor = Drawing.Color.White
            Me.Position = ChartElementPosition.Top
        End Sub

        Public Sub New(ByVal owner As CChart)
            MyBase.New(owner)
            Me.BackColor = Drawing.Color.White
            Me.Position = ChartElementPosition.Top
        End Sub

    End Class


End Namespace