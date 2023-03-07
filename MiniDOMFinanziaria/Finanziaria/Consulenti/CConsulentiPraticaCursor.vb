﻿Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica




Partial Public Class Finanziaria


    Public Class CConsulentiPraticaCursor
        Inherits DBObjectCursorPO(Of CConsulentePratica)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_IDUser As New CCursorField(Of Integer)("IDUser")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Public ReadOnly Property IDUser As CCursorField(Of Integer)
            Get
                Return Me.m_IDUser
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property
 

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return minidom.Finanziaria.Consulenti.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConsulenti"
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("OnlyValid", Me.m_OnlyValid)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class