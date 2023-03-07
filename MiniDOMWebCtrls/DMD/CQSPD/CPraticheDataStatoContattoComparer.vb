Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms


 
  

    '--------------------------------
    Public Class CPraticheDataStatoContattoComparer
        Implements IComparer

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal a As CPraticaCQSPD, ByVal b As CPraticaCQSPD) As Integer
            Dim d1 As DateTime? = Nothing
            Dim d2 As DateTime? = Nothing
            If (a.StatoPreventivo IsNot Nothing) Then d1 = a.StatoPreventivo.Data
            If (b.StatoPreventivo IsNot Nothing) Then d2 = b.StatoPreventivo.Data
            Return DateUtils.Compare(d1, d2)
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class


End Namespace