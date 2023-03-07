Imports minidom
Imports minidom.Databases
Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Public Class Finanziaria


    <Serializable>
    Public Class CQSPDCPHSTATINFO
        Implements XML.IDMDXMLSerializable

        Public IDOperatore As Integer
        Public NomeOperatore As String
        Public Numero As Integer
        Public ML As Decimal
        Public Minimo As Double
        Public Massimo As Double
        Public Media As Double


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Numero" : Me.Numero = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Minimo" : Me.Minimo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Massimo" : Me.Massimo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Media" : Me.Media = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.NomeOperatore)
            writer.WriteAttribute("Numero", Me.Numero)
            writer.WriteAttribute("ML", Me.ML)
            writer.WriteAttribute("Minimo", Me.Minimo)
            writer.WriteAttribute("Massimo", Me.Massimo)
            writer.WriteAttribute("Media", Me.Media)
        End Sub
    End Class


End Class

