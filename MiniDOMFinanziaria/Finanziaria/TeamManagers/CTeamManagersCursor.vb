Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    Public Class CTeamManagersCursor
        Inherits DBObjectCursorPO(Of CTeamManager)

        Private m_Nominativo As New CCursorFieldObj(Of String)("Nominativo")
        Private m_IDListinoPredefinito As New CCursorField(Of Integer)("ListinoPredefinito")
        Private m_IDReferente As New CCursorField(Of Integer)("Referente")
        Private m_IDUtente As New CCursorField(Of Integer)("Utente")
        Private m_IDPersona As New CCursorField(Of Integer)("Persona")
        Private m_Rapporto As New CCursorFieldObj(Of String)("Rapporto")
        Private m_DataInizioRapporto As New CCursorField(Of Date)("DataInizioRapporto")
        Private m_DataFineRapporto As New CCursorField(Of Date)("DataFineRapporto")
        Private m_SetPremiPersonalizzato As New CCursorField(Of Boolean)("SetPremiPersonalizzato")
        Private m_IDSetPremiSpecificato As New CCursorField(Of Integer)("SetPremi")
        Private m_StatoTeamManager As New CCursorField(Of StatoTeamManager)("StatoTeamManager")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

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

        Public ReadOnly Property Nominativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Public ReadOnly Property IDListinoPredefinito As CCursorField(Of Integer)
            Get
                Return Me.m_IDListinoPredefinito
            End Get
        End Property

        Public ReadOnly Property IDReferente As CCursorField(Of Integer)
            Get
                Return Me.m_IDReferente
            End Get
        End Property

        Public ReadOnly Property IDUtente As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtente
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property Rapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_Rapporto
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioRapporto
            End Get
        End Property

        Public ReadOnly Property StatoTeamManager As CCursorField(Of StatoTeamManager)
            Get
                Return Me.m_StatoTeamManager
            End Get
        End Property

        Public ReadOnly Property SetPremiPersonalizzato As CCursorField(Of Boolean)
            Get
                Return Me.m_SetPremiPersonalizzato
            End Get
        End Property

        Public ReadOnly Property IDSetPremiSpecificato As CCursorField(Of Integer)
            Get
                Return Me.m_IDSetPremiSpecificato
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFineRapporto
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTeamManager
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Finanziaria.TeamManagers.Module
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "([StatoTeamManager] = " & Finanziaria.StatoTeamManager.STATO_ATTIVO & ")", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataInizioRapporto] Is Null) Or ([DataInizioRapporto]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFineRapporto] Is Null) Or ([DataFineRapporto]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class
