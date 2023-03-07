Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Anagrafica

    Public MustInherit Class LuogoCursor(Of T As Luogo)
        Inherits DBObjectCursorPO(Of T)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Anagrafica.Luoghi.Database
        End Function

    End Class

End Class