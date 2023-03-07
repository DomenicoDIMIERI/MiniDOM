﻿Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports System
Imports minidom.Net.Mail
Imports minidom.Anagrafica
Imports minidom.XML

Partial Public Class Anagrafica

    <Serializable>
    Public Class XLSImporterResult
        Implements XML.IDMDXMLSerializable

        Public RowStatus As String

        Public Sub New()
            Me.RowStatus = ""
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("RowStatus", Me.RowStatus)
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "RowStatus" : Me.RowStatus = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub
    End Class


End Class
