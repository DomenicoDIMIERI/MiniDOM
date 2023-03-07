Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    Public Class CUserComparer
        Implements IComparer

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal user1 As CUser, ByVal user2 As CUser) As Integer
            Return user1.CompareTo(user2)
        End Function

        Private Function Compare1(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(a, b)
        End Function

    End Class

End Class
