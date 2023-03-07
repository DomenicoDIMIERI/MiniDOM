Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers

    ''' <summary>
    ''' Informazioni su una chiamata in ingresso
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IncomingCallEventArgs
        Inherits System.EventArgs

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace