Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals


    '.---------------------------------------------

    ''' <summary>
    ''' Classe che consente di accedere agli stati pratica
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public NotInheritable Class CStatiPraticaClass
        Inherits CModulesClass(Of CStatoPratica)

        Friend Sub New()
            MyBase.New("modCQSPDStatPrat", GetType(CStatoPraticaCursor), -1)
        End Sub

        Public Function FormatMacroStato(ByVal stato As StatoPraticaEnum?) As String
            Return minidom.Finanziaria.Pratiche.FormatStatoPratica(stato)
        End Function


        ''' <summary>
        ''' Restituisce lo stato pratica iniziale (per una nuova pratica)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDefault() As CStatoPratica
            Return minidom.Finanziaria.Configuration.StatoPredefinito
        End Function

        Public Function GetStatiSuccessivi(ByVal statoAttuale As CStatoPratica) As CStatoPratRulesCollection
            Return statoAttuale.StatiSuccessivi
        End Function

        Public Function GetStatiSuccessivi(ByVal pratica As CPraticaCQSPD) As CCollection(Of CStatoPratRule)
            Dim ret As New CCollection(Of CStatoPratRule)
            Dim ra As CRichiestaApprovazione = pratica.RichiestaApprovazione
            If (ra IsNot Nothing AndAlso ra.StatoRichiesta <> StatoRichiestaApprovazione.APPROVATA) Then
                Dim stAnnullata As CStatoPratica = Finanziaria.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
                Dim rule As CStatoPratRule
                For Each rule In pratica.StatoAttuale.StatiSuccessivi
                    If (rule.IDTarget = GetID(stAnnullata)) Then ret.Add(rule)
                Next
            ElseIf (pratica.StatoAttuale IsNot Nothing) Then
                If (pratica.StatoAttuale.StatiSuccessivi.Count > 0) Then ret.AddRange(pratica.StatoAttuale.StatiSuccessivi.ToArray)
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente tutti gli stati attivi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStatiAttivi() As CCollection(Of CStatoPratica)
            Dim ret As New CCollection(Of CStatoPratica)
            For Each item As CStatoPratica In Me.LoadAll()
                If (item.Attivo) Then ret.Add(item)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il valore corrispondente al vecchio sistema secondo il campo OldStatus di compatibilità
        ''' </summary>
        ''' <param name="ms"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByCompatibleID(ByVal ms As StatoPraticaEnum) As CStatoPratica
            For Each item As CStatoPratica In Me.LoadAll
                If (item.MacroStato.HasValue AndAlso item.MacroStato.Value = ms) Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByName(ByVal name As String) As CStatoPratica
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            For Each item As CStatoPratica In Me.LoadAll
                If (Strings.Compare(item.Nome, name) = 0) Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetSequenzaStandard() As CCollection(Of CStatoPratica)
            Dim stato As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO)
            Dim items As New CCollection(Of CStatoPratica)
            While stato IsNot Nothing
                items.Add(stato)
                stato = stato.DefaultTarget
            End While
            stato = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
            If (stato IsNot Nothing) Then items.Add(stato)
            Return items
        End Function

        Public ReadOnly Property StatoContrattoFirmato As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTRATTO_FIRMATO)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_CONTRATTO_FIRMATO
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property StatoContrattoStampato As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTRATTO_STAMPATO)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_CONTRATTO_STAMPATO
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property StatoRichiestaDelibera As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_RICHIESTADELIBERA)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_RICHIESTADELIBERA
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property StatoLiquidato As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_LIQUIDATA
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property StatoPraticaCaricata As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_PRATICA_CARICATA)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_PRATICA_CARICATA
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property StatoAnnullato As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_ANNULLATA
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public ReadOnly Property StatoArchiviato As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_ARCHIVIATA
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce lo stato preventivo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoPreventivo As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_PREVENTIVO
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce lo stato preventivo accettato
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatoPreventivoAccettato As CStatoPratica
            Get
                Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO)
                If (ret Is Nothing) Then
                    ret = New CStatoPratica
                    ret.MacroStato = StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Nome = FormatMacroStato(ret.MacroStato)
                    ret.Save()
                End If
                Return ret
            End Get
        End Property

        Public Overrides Sub Initialize()
            MyBase.Initialize()
            Dim stati As CCollection(Of CStatoPratica) = Me.LoadAll()
            For Each ms As StatoPraticaEnum In [Enum].GetValues(GetType(StatoPraticaEnum))
                Dim s As CStatoPratica = Nothing
                For Each s1 As CStatoPratica In stati
                    If (s1.MacroStato.HasValue AndAlso s1.MacroStato.Value = ms) Then
                        s = s1
                        Exit For
                    End If
                Next
                If (s Is Nothing) Then
                    s = New CStatoPratica()
                    s.MacroStato = ms
                    s.Nome = FormatMacroStato(ms)
                    s.Descrizione = FormatMacroStato(ms)
                    s.Stato = ObjectStatus.OBJECT_VALID
                    s.Save()
                End If
            Next
        End Sub

    End Class
End Namespace

Partial Public Class Finanziaria




    Private Shared m_StatiPratica As CStatiPraticaClass = Nothing

    Public Shared ReadOnly Property StatiPratica As CStatiPraticaClass
        Get
            If (m_StatiPratica Is Nothing) Then m_StatiPratica = New CStatiPraticaClass
            Return m_StatiPratica
        End Get
    End Property

End Class
