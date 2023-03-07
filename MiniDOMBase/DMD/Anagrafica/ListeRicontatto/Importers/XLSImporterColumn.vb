Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports System
Imports minidom.Net.Mail
Imports minidom.Anagrafica
Imports minidom.XML

Partial Public Class Anagrafica

    <Serializable>
    Public Class XLSImporterColumn
        Implements XML.IDMDXMLSerializable

        Public SourceName As String
        Public SourceDataType As System.TypeCode
        Public SuggestedTargetField As String
        Public TargetField As String
        Public DoImport As Boolean

        Public Sub New()
            Me.SourceName = ""
            Me.SourceDataType = TypeCode.Empty
            Me.SuggestedTargetField = ""
            Me.TargetField = ""
            Me.DoImport = True
        End Sub


        Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("SourceName", Me.SourceName)
            writer.WriteAttribute("SourceDataType", Me.SourceDataType)
            writer.WriteAttribute("SuggestedTargetField", Me.SuggestedTargetField)
            writer.WriteAttribute("TargetField", Me.TargetField)
            writer.WriteAttribute("DoImport", Me.DoImport)
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "SourceName" : Me.SourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceDataType" : Me.SourceDataType = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SuggestedTargetField" : Me.SuggestedTargetField = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TargetField" : Me.TargetField = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DoImport" : Me.DoImport = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
            End Select
        End Sub
    End Class

End Class
