Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria


    Public MustInherit Class CCQSPDProvvigioneXOffertaEvaluator

        Public MustOverride Function Evaluate(ByVal pxo As CCQSPDProvvigioneXOfferta, ByVal offerta As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Double

    End Class

End Class
