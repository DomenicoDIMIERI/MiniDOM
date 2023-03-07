Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Class Sistema

    Public Class CIntervalloData
        Implements XML.IDMDXMLSerializable, ISupportInitializeFrom

        Public Tipo As String = ""
        Public Inizio As Date? = Nothing
        Public Fine As Date? = Nothing

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal tipo As String, ByVal fromDate As Date?, ByVal toDate As Date?)
            Me.New
            Me.Tipo = Trim(tipo)
            Me.Inizio = fromDate
            Me.Fine = toDate
        End Sub

        Public Overridable Function IsSet() As Boolean
            Return (Types.IsNull(Me.Inizio) = False) Or (Types.IsNull(Me.Fine) = False) Or (Me.Tipo <> "")
        End Function

        Protected Overridable Sub XMLSerialize(ByVal writer As minidom.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("m_Tipo", Me.Tipo)
            writer.WriteAttribute("m_Inizio", Me.Inizio)
            writer.WriteAttribute("m_Fine", Me.Fine)
        End Sub

        Protected Overridable Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "m_Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Inizio" : Me.Inizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Fine" : Me.Fine = XML.Utils.Serializer.DeserializeDate(fieldValue)
            End Select
        End Sub


        Public Overridable Sub CopyFrom(value As Object) Implements ISupportInitializeFrom.InitializeFrom
            With DirectCast(value, CIntervalloData)
                Me.Fine = .Fine
                Me.Inizio = .Inizio
                Me.Tipo = .Tipo
            End With
        End Sub

        Public Overridable Sub Clear()
            Me.Fine = Nothing
            Me.Inizio = Nothing
            Me.Tipo = ""
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class