﻿Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Sistema

Partial Public Class Finanziaria

    Public Class CAssicurazioniCursor
        Inherits DBObjectCursor(Of CAssicurazione)


        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("descrizione")
        Private m_MeseScattoEta As New CCursorField(Of Integer)("mesescattoeta")
        Private m_MeseScattoAnzianita As New CCursorField(Of Integer)("mesescattoanzianita")

        Public Sub New()
        End Sub


        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property MeseScattoEta As CCursorField(Of Integer)
            Get
                Return Me.m_MeseScattoEta
            End Get
        End Property

        Public ReadOnly Property MeseScattoAnzianita As CCursorField(Of Integer)
            Get
                Return Me.m_MeseScattoAnzianita
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Finanziaria.Assicurazioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Assicurazioni"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAssicurazione
        End Function

    End Class

End Class