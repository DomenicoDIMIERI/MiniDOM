Imports System
Imports System.Text
Imports System.IO

Namespace Security

    <Serializable> _
    Public Class InvalidAlgorithmParameterException
        Inherits System.Exception

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
            MyBase.New(message, innerException)
            DMDObject.IncreaseCounter(Me)
        End Sub

        Sub New(ex As System.Exception)
            MyBase.New(ex.Message, ex)
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace