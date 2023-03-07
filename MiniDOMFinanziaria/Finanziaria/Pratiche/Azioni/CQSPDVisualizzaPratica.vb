Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Sistema

Partial Public Class Finanziaria

    Public Class CQSPDVisualizzaPratica
        Inherits AzioneEseguibile


        Public Overrides ReadOnly Property Description As String
            Get
                Return "Visualizza la pratica"
            End Get
        End Property

        Protected Overrides Function ExecuteInternal(notifica As Notifica, parameters As String) As String
            Return ""
        End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "CQSPDVISPRAT"
            End Get
        End Property

        Public Overrides Function Render(notifica As Notifica, context As Object) As Object
            Return Nothing
        End Function

    End Class

End Class