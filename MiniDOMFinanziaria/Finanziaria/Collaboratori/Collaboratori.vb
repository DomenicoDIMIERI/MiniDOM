﻿Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals

    <Serializable>
    Public NotInheritable Class CCollaboratoriClass
        Inherits CModulesClass(Of CCollaboratore)

        <NonSerialized> Private m_ClientiXCollaboratore As ClientiXCollaboratoriClass = Nothing


        Friend Sub New()
            MyBase.New("modCQSPDCollaboratori", GetType(CCollaboratoriCursor), -1)
        End Sub

        Public Function FormatStatoProduttore(ByVal value As StatoProduttore) As String
            Select Case value
                Case Finanziaria.StatoProduttore.STATO_ATTIVO : Return "Attivo"
                Case Finanziaria.StatoProduttore.STATO_DISABILITATO : Return "Disabilitato"
                Case Finanziaria.StatoProduttore.STATO_ELIMINATO : Return "Eliminato"
                Case Finanziaria.StatoProduttore.STATO_INATTIVAZIONE : Return "In Attivazione"
                Case Finanziaria.StatoProduttore.STATO_SOSPESO : Return "Sospeso"
                    'Case Finanziaria.StatoProduttore.STATO_INVALID : Return "Non Valido"
                Case Else : Return "Sconosciuto"
            End Select
        End Function

        Public Function ParseStatoProduttore(ByVal value As String) As StatoProduttore
            Select Case LCase(Trim(value))
                Case "attivo" : Return StatoProduttore.STATO_ATTIVO
                Case "disabilitato" : Return StatoProduttore.STATO_DISABILITATO
                Case "eliminato" : Return StatoProduttore.STATO_ELIMINATO
                Case "in attivazione" : Return StatoProduttore.STATO_INATTIVAZIONE
                Case "sospeso" : Return StatoProduttore.STATO_SOSPESO
                    'Case "non valido" : Return StatoProduttore.STATO_INVALID
                Case Else : Return 0
            End Select
        End Function

        'Public  Function GetItemByCodiceFiscale(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    cursor.CodiceFiscale.Value = Formats.ParseCodiceFiscale(valore)
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        'Public  Function GetItemByPartitaIVA(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    'cursor.PartitaIVA.Value = Formats.ParsePartitaIVA(valore)
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        'Public  Function GetItemByEMail(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    cursor.eMail.Value = valore
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        'Public Function GetItemByUIF(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    For   ret In Me.CachedItems
        '        If ret.NumeroIsci Then
        '    Next
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    cursor.NumeroIscrizioneUIF.Value = valore
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        Public Function GetItemByRUI(ByVal valore As String) As CCollaboratore
            valore = Trim(valore)
            If (valore = "") Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(item.NumeroIscrizioneRUI, valore) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByISVAP(ByVal valore As String) As CCollaboratore
            valore = Trim(valore)
            If (valore = "") Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If Strings.Compare(item.NumeroIscrizioneISVAP, valore) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByPersona(ByVal personID As Integer) As CCollaboratore
            If (personID = 0) Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso item.IDPersona = personID Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByUser(ByVal userId As Integer) As CCollaboratore
            If (userId = 0) Then Return Nothing
            Return Me.GetItemByUser(Sistema.Users.GetItemById(userId))
        End Function

        Public Function GetItemByUser(ByVal user As CUser) As CCollaboratore
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            For Each item As CCollaboratore In Me.LoadAll
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso item.UserID = GetID(user) Then
                    Return item
                End If
            Next
            Return Nothing
        End Function


        Public Function GetItemByName(ByVal value As String) As CCollaboratore
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(item.NomePersona, value) = 0 Then Return item
            Next
            Return Nothing
        End Function

        'Public Function CalcolaPremio(ByVal value As Decimal) As Decimal
        '    Dim dbRis As System.Data.IDataReader
        '    Dim dbSQL As String
        '    Dim ret, somma, termine, finoA As Decimal
        '    Dim perc As Double
        '    dbSQL = "SELECT * FROM [tbl_CollaboratoriPremi] ORDER BY [FinoA] ASC"
        '    dbRis = Finanziaria.Database.ExecuteReader(dbSQL)
        '    somma = value
        '    ret = 0
        '    While dbRis.Read And (somma > 0)
        '        finoA = Formats.ToValuta(dbRis("FinoA"))
        '        perc = Formats.ToDouble(dbRis("Percentuale"))
        '        If "" & finoA = "" Then finoA = 0
        '        If "" & perc = "" Then perc = 0
        '        termine = IIf(somma <= finoA, somma, finoA)
        '        somma = somma - termine
        '        termine = termine * perc / 100
        '        ret = ret + termine
        '    End While
        '    dbRis.Dispose()
        '    dbRis = Nothing
        '    Return ret
        'End Function


        Public ReadOnly Property ClientiXCollaboratori As ClientiXCollaboratoriClass
            Get
                If (m_ClientiXCollaboratore Is Nothing) Then m_ClientiXCollaboratore = New ClientiXCollaboratoriClass
                Return m_ClientiXCollaboratore
            End Get
        End Property

        Friend Sub InvalidateTrattative()
            For Each c As CacheItem In Me.CachedItems
                DirectCast(c.Item, CCollaboratore).InvalidateTrattative
            Next
        End Sub
    End Class

End Namespace


Partial Class Finanziaria

    Private Shared m_Collaboratori As CCollaboratoriClass = Nothing

    Public Shared ReadOnly Property Collaboratori As CCollaboratoriClass
        Get
            If (m_Collaboratori Is Nothing) Then m_Collaboratori = New CCollaboratoriClass
            Return m_Collaboratori
        End Get
    End Property

End Class