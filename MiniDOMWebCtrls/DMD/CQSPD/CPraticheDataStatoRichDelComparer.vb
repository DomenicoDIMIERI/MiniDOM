Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms

 
    Public Class CPraticheDataStatoRichDelComparer
        Implements IComparer

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal a As CPraticaCQSPD, ByVal b As CPraticaCQSPD) As Integer
            If a.StatoRichiestaDelibera.Data < b.StatoRichiestaDelibera.Data Then Return -1
            If a.StatoRichiestaDelibera.Data > b.StatoRichiestaDelibera.Data Then Return 1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class


End Namespace