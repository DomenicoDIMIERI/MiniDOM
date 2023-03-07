Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Class Sistema

    Public Class CCalendarItemsComparer
        Implements IComparer

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim a As ICalendarActivity = x
            Dim b As ICalendarActivity = y
            If a.DataInizio < b.DataInizio Then Return -1
            If a.DataInizio > b.DataInizio Then Return 1
            Return 0
        End Function

    End Class

End Class