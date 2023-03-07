Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms

    Public Class MPANNHSTATSITEM
        Implements IComparable, XML.IDMDXMLSerializable


        Public Count As Integer = 0
        Public ML As Decimal = 0
        Public Motivo As String = ""

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim other As MPANNHSTATSITEM = obj
            If (Me.ML > other.ML) Then
                Return 1
            ElseIf (Me.ML < other.ML) Then
                Return -1
            Else
                Return 0
            End If
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Count" : Me.Count = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Motivo" : Me.Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Count", Me.Count)
            writer.WriteAttribute("ML", Me.ML)
            writer.WriteAttribute("Motivo", Me.Motivo)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Public Class MotivazioniPraticheAnnullateModuleHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()

        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPraticheCQSPDCursor
        End Function

    End Class


End Namespace