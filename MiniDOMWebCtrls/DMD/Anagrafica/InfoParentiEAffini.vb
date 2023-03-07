Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils




Namespace Forms

    Public Class InfoParentiEAffini
        Implements minidom.XML.IDMDXMLSerializable

        Public Relazione As CRelazioneParentale
        Public Contatto As CContatto
        Public Note As String
        Public IconURL1 As String
        Public IconURL2 As String

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Relazione" : Me.Relazione = fieldValue
                Case "Contatto" : Me.Contatto = fieldValue
                Case "Note" : Me.Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL1" : Me.IconURL1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL2" : Me.IconURL2 = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IconURL1", Me.IconURL1)
            writer.WriteAttribute("IconURL2", Me.IconURL2)
            writer.WriteTag("Relazione", Me.Relazione)
            writer.WriteTag("Contatto", Me.Contatto)
            writer.WriteTag("Note", Me.Note)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Namespace