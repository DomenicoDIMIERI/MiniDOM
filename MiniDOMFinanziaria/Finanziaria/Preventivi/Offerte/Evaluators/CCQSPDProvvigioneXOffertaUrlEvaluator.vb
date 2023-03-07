Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria


    Public Class CCQSPDProvvigioneXOffertaUrlEvaluator
        Inherits CCQSPDProvvigioneXOffertaEvaluator

        Public Overrides Function Evaluate(ByVal pxo As CCQSPDProvvigioneXOfferta, ByVal offerta As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Double
            Dim params As CKeyCollection = pxo.Parameters
            Dim url As String = Strings.Trim(params.GetItemByKey("url"))
            If (url = "") Then Throw New ArgumentNullException("ArgumentNullException: url")
            Dim tmp As String = RPC.InvokeMethod(url, "p", RPC.str2n(XML.Utils.Serializer.Serialize(offerta)), "pxo", RPC.str2n(XML.Utils.Serializer.Serialize(pxo)), "est", RPC.str2n(XML.Utils.Serializer.Serialize(estinzioni)))
            Return XML.Utils.Serializer.DeserializeDouble(tmp)
        End Function

    End Class

End Class
