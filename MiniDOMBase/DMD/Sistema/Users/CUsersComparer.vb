Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    Public Class CUsersComparer
        Implements IComparer

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim a As CUser = x
            Dim b As CUser = y
            Return Strings.Compare(a.Nominativo, b.Nominativo, CompareMethod.Text)
        End Function

    End Class


End Class
