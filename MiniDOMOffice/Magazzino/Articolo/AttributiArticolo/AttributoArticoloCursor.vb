﻿Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Office

    <Serializable> _
    Public Class AttributoArticoloCursor
        Inherits DBObjectCursor(Of AttributoArticolo)

        Private m_IDArticolo As New CCursorField(Of Integer)("IDArticolo")
        Private m_NomeAttributo As New CCursorFieldObj(Of String)("NomeAttributo")
        Private m_ValoreAttributoFormattato As New CCursorFieldObj(Of String)("ValoreAttributo")
        Private m_TipoAttributo As New CCursorField(Of System.TypeCode)("TipoAttributo")
        Private m_UnitaDiMisura As New CCursorFieldObj(Of String)("UnitaDiMisura")

        Public Sub New()
        End Sub


        Public ReadOnly Property IDArticolo As CCursorField(Of Integer)
            Get
                Return Me.m_IDArticolo
            End Get
        End Property

        Public ReadOnly Property NomeAttributo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAttributo
            End Get
        End Property

        Public ReadOnly Property ValoreAttributoFormattato As CCursorFieldObj(Of String)
            Get
                Return Me.m_ValoreAttributoFormattato
            End Get
        End Property

        Public ReadOnly Property TipoAttributo As CCursorField(Of System.TypeCode)
            Get
                Return Me.m_TipoAttributo
            End Get
        End Property

        Public ReadOnly Property UnitaDiMisura As CCursorFieldObj(Of String)
            Get
                Return Me.m_UnitaDiMisura
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoliAttributi"
        End Function


    End Class

End Class


