Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    ''' <summary>
    ''' Cursore sulla tabella delle pratiche
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CPraticheCQSPDCursor
        Inherits DBObjectCursorPO(Of CPraticaCQSPD)

        Private m_RinnovaDa As String = vbNullString
        Private m_Nominativo As New CCursorFieldObj(Of String)("Nominativo")
        Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")
        Private m_IDOperatoreTrasferita As New CCursorField(Of Integer)("TrasferitoDa")
        Private m_DaVedere As New CCursorField(Of Boolean)("(([tbl_Pratiche].[Flags] AND " & PraticaFlags.DAVEDERE & ")=" & PraticaFlags.DAVEDERE & ")")
        Private m_Trasferita As New CCursorField(Of Boolean)("(([tbl_Pratiche].[Flags] AND " & PraticaFlags.TRASFERITA & ")=" & PraticaFlags.TRASFERITA & ")")
        'Private m_MotivoScontoDettaglio As New CCursorFieldObj(Of String)("MotivoScontoDettaglio")
        Private m_ScontoAutorizzatoIl As New CCursorField(Of Date)("ScontoAutorizzatoIl")
        Private m_ScontoNomeMotivo As New CCursorFieldObj(Of String)("ScontoNomeMotivo")
        Private m_NomeAmministrazione As New CCursorFieldObj(Of String)("Ente")
        Private m_IDAmministrazione As New CCursorField(Of Integer)("IDAmministrazione")
        Private m_IDEntePagante As New CCursorField(Of Integer)("IDEntePagante")
        Private m_IDScontoAutorizzatoDa As New CCursorField(Of Integer)("IDScontoAutorizzatoDa")
        Private m_Spread As New CCursorField(Of Double)("(IIF([MontanteLordo]>0, 100*(([UpFront] + [Running])/[MontanteLordo]), 0))")
        Private m_Rappel As New CCursorField(Of Double)("(IIF([MontanteLordo]>0, 100*([Rappel]/[MontanteLordo]), 0))")
        Private m_PremioDaCessionario As New CCursorField(Of Double)("PremioDaCessionario")
        Private m_ValoreRappel As New CCursorField(Of Double)("Rappel")
        Private m_MontanteLordo As New CCursorField(Of Decimal)("MontanteLordo")
        Private m_DataDecorrenza As New CCursorField(Of Date)("DataDecorrenza")
        Private m_Flags As New CCursorField(Of PraticaFlags)("[tbl_Pratiche].Flags")
        Private m_IDCorrezione As New CCursorField(Of Integer)("IDCorrezione")
        Private m_IDStatoAttuale As New CCursorField(Of Integer)("IDStatoAttuale")
        Private m_IDConsulenza As New CCursorField(Of Integer)("IDConsulenza")
        Private m_IDRichiestaDiFinanziamento As New CCursorField(Of Integer)("IDRichiestaFinanziamento")
        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_CognomeCliente As New CCursorFieldObj(Of String)("CognomeCliente")
        Private m_IDCliente As New CCursorField(Of Integer)("Cliente")
        Private m_NatoAComune As New CCursorFieldObj(Of String)("NatoAComune")
        Private m_NatoAProvincia As New CCursorFieldObj(Of String)("NatoAProvincia")
        Private m_NatoIl As New CCursorField(Of Date)("NatoIl")
        Private m_ResidenteAComune As New CCursorFieldObj(Of String)("ResidenteAComune")
        Private m_ResidenteAProvincia As New CCursorFieldObj(Of String)("ResidenteAProvincia")
        Private m_ResidenteACAP As New CCursorFieldObj(Of String)("ResidenteACAP")
        Private m_ResidenteAVia As New CCursorFieldObj(Of String)("ResidenteAVia")
        Private m_CodiceFiscale As New CCursorFieldObj(Of String)("CodiceFiscale")
        Private m_PartitaIVA As New CCursorFieldObj(Of String)("PartitaIVA")
        Private m_CommercialeID As New CCursorField(Of Integer)("Commerciale")
        Private m_NomeProdotto As New CCursorFieldObj(Of String)("CQS_PD")
        Private m_ProdottoID As New CCursorField(Of Integer)("Prodotto")
        Private m_TipoFonteContatto As New CCursorFieldObj(Of String)("TipoFonteContatto")
        Private m_NomeFonte As New CCursorFieldObj(Of String)("FonteContatto")
        Private m_NomeProfilo As New CCursorFieldObj(Of String)("NomeProfilo")
        Private m_IDProfilo As New CCursorField(Of Integer)("Profilo")
        Private m_NettoRicavo As New CCursorField(Of Decimal)("NettoRicavo")
        Private m_IDCessionario As New CCursorField(Of Integer)("Cessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_StatoPratica As New CCursorField(Of StatoPraticaEnum)("StatoPratica")
        Private m_NumeroRate As New CCursorField(Of Integer)("NumeroRate")
        Private m_IDCanale As New CCursorField(Of Integer)("IDCanale")
        Private m_NomeCanale As New CCursorFieldObj(Of String)("NomeCanale")
        Private m_IDCanale1 As New CCursorField(Of Integer)("IDCanale1")
        Private m_NomeCanale1 As New CCursorFieldObj(Of String)("NomeCanale1")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_IDRichiestaApprovazione As New CCursorField(Of Integer)("IDRichiestaApprovazione")
        Private m_Rata As New CCursorField(Of Decimal)("[MontanteLordo]/IIF([NumeroRate]>0,[NumeroRate],1)")
        Private m_StatoRichiestaApprovazione As New CCursorField(Of StatoRichiestaApprovazione)("StatoRichiestaApprovazione")
        Private m_NumeroEsterno As New CCursorFieldObj(Of String)("StatRichD_Params")
        Private m_IDScontoRichiestoDa As New CCursorField(Of Integer)("IDScontoRichiestoDa")

        Private m_TipoFonteCliente As New CCursorFieldObj(Of String)("TipoFonteCliente")
        Private m_IDFonteCliente As New CCursorField(Of Integer)("IDFonteCliente")
        Private m_IDFinestraLavorazione As New CCursorField(Of Integer)("IDFinestraLavorazione")
        Private m_IDTabellaFinanziaria As New CCursorField(Of Integer)("IDTabellaFinanziaria")
        Private m_IDTabellaVita As New CCursorField(Of Integer)("IDTabellaVita")
        Private m_IDTabellaImpiego As New CCursorField(Of Integer)("IDTabellaImpiego")
        Private m_IDTabellaCredito As New CCursorField(Of Integer)("IDTabellaCredito")

        Private m_IDUltimaVerifica As New CCursorField(Of Integer)("IDUltimaVerifica")

        Private m_DataValuta As New CCursorField(Of Date)("DataValuta")
        Private m_DataStampaSecci As New CCursorField(Of Date)("DataStampaSecci")

        Private m_CapitaleFinanziato As New CCursorField(Of Decimal)("CapitaleFinanziato")

        Private m_CategoriaProdotto As New CCursorFieldObj(Of String)("CategoriaProdotto")

        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")

        Private m_PassaggiDiStato As CCollection(Of CInfoStato)



        Public Sub New()


        End Sub

        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Public ReadOnly Property CategoriaProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_CategoriaProdotto
            End Get
        End Property

        Public ReadOnly Property PremioDaCessionario As CCursorField(Of Double)
            Get
                Return Me.m_PremioDaCessionario
            End Get
        End Property

        Public ReadOnly Property DataValuta As CCursorField(Of Date)
            Get
                Return Me.m_DataValuta
            End Get
        End Property

        Public ReadOnly Property DataStampaSecci As CCursorField(Of Date)
            Get
                Return Me.m_DataStampaSecci
            End Get
        End Property


        Public ReadOnly Property IDUltimaVerifica As CCursorField(Of Integer)
            Get
                Return Me.m_IDUltimaVerifica
            End Get
        End Property

        Public ReadOnly Property IDTabellaFinanziaria As CCursorField(Of Integer)
            Get
                Return Me.m_IDTabellaFinanziaria
            End Get
        End Property

        Public ReadOnly Property IDTabellaVita As CCursorField(Of Integer)
            Get
                Return Me.m_IDTabellaVita
            End Get
        End Property

        Public ReadOnly Property IDTabellaImpiego As CCursorField(Of Integer)
            Get
                Return Me.m_IDTabellaImpiego
            End Get
        End Property

        Public ReadOnly Property IDTabellaCredito As CCursorField(Of Integer)
            Get
                Return Me.m_IDTabellaCredito
            End Get
        End Property

        Public ReadOnly Property DaVedere As CCursorField(Of Boolean)
            Get
                Return Me.m_DaVedere
            End Get
        End Property

        Public ReadOnly Property Trasferita As CCursorField(Of Boolean)
            Get
                Return Me.m_Trasferita
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazione
            End Get
        End Property


        Public ReadOnly Property Rappel As CCursorField(Of Double)
            Get
                Return Me.m_Rappel
            End Get
        End Property

        Public ReadOnly Property ValoreRappel As CCursorField(Of Double)
            Get
                Return Me.m_ValoreRappel
            End Get
        End Property

        Public ReadOnly Property TipoFonteCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonteCliente
            End Get
        End Property

        Public ReadOnly Property IDFonteCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonteCliente
            End Get
        End Property


        Public ReadOnly Property IDScontoRichiestoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDScontoRichiestoDa
            End Get
        End Property

        Public ReadOnly Property NumeroEsterno As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroEsterno
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property IDRichiestaApprovazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaApprovazione
            End Get
        End Property

        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property TipoContesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
            End Get
        End Property

        Public ReadOnly Property IDCanale As CCursorField(Of Integer)
            Get
                Return Me.m_IDCanale
            End Get
        End Property

        Public ReadOnly Property NomeCanale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCanale
            End Get
        End Property

        Public ReadOnly Property IDCanale1 As CCursorField(Of Integer)
            Get
                Return Me.m_IDCanale1
            End Get
        End Property

        Public ReadOnly Property NomeCanale1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCanale1
            End Get
        End Property

        Public ReadOnly Property IDProfilo As CCursorField(Of Integer)
            Get
                Return Me.m_IDProfilo
            End Get
        End Property

        Public ReadOnly Property IDConsulenza As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulenza
            End Get
        End Property

        Public ReadOnly Property IDRichiestaDiFinanziamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaDiFinanziamento
            End Get
        End Property

        Public ReadOnly Property IDCorrezione As CCursorField(Of Integer)
            Get
                Return Me.m_IDCorrezione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of PraticaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property MontanteLordo As CCursorField(Of Decimal)
            Get
                Return Me.m_MontanteLordo
            End Get
        End Property

        Public ReadOnly Property DataDecorrenza As CCursorField(Of Date)
            Get
                Return Me.m_DataDecorrenza
            End Get
        End Property

        Public Property RinnovaDa As String
            Get
                Return Me.m_RinnovaDa
            End Get
            Set(value As String)
                Me.m_RinnovaDa = Trim(value)
            End Set
        End Property

        Public ReadOnly Property Spread As CCursorField(Of Double)
            Get
                Return Me.m_Spread
            End Get
        End Property

        'Public ReadOnly Property IDOperatoreContatto As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreContatto
        '    End Get
        'End Property

        ''Private m_NomeOperatoreContatto As New CCursorFieldObj(Of String)("NomeOperatoreContatto")
        'Public ReadOnly Property IDOperatoreRichiestaDelibera As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreRichiestaDelibera
        '    End Get
        'End Property

        'Public ReadOnly Property IDOperatoreDeliberata As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreDeliberata
        '    End Get
        'End Property

        'Public ReadOnly Property IDOperatoreProntaPerLiquidazione As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreProntaPerLiquidazione
        '    End Get
        'End Property

        'Public ReadOnly Property IDOperatoreLiquidata As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreLiquidata
        '    End Get
        'End Property

        'Public ReadOnly Property IDOperatoreArchiviata As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreArchiviata
        '    End Get
        'End Property

        Public ReadOnly Property IDOperatoreTrasferita As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreTrasferita
            End Get
        End Property

        'Public ReadOnly Property IDOperatoreAnnullata As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDOperatoreAnnullata
        '    End Get
        'End Property

        Public ReadOnly Property NomeAmministrazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAmministrazione
            End Get
        End Property

        Public ReadOnly Property IDAmministrazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDAmministrazione
            End Get
        End Property

        'Public ReadOnly Property NomeEntePagante As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_NomeEntePagante
        '    End Get
        'End Property

        Public ReadOnly Property IDEntePagante As CCursorField(Of Integer)
            Get
                Return Me.m_IDEntePagante
            End Get
        End Property

        Public ReadOnly Property NumeroRate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroRate
            End Get
        End Property

        'Public ReadOnly Property Running As CCursorField(Of Double)
        '    Get
        '        Return Me.m_Running
        '    End Get
        'End Property

        Public ReadOnly Property Nominativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property CognomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_CognomeCliente
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NatoAComune As CCursorFieldObj(Of String)
            Get
                Return Me.m_NatoAComune
            End Get
        End Property

        Public ReadOnly Property NatoAProvincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_NatoAProvincia
            End Get
        End Property

        Public ReadOnly Property NatoIl As CCursorField(Of Date)
            Get
                Return Me.m_NatoIl
            End Get
        End Property

        Public ReadOnly Property ResidenteAComune As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteAComune
            End Get
        End Property

        Public ReadOnly Property ResidenteAProvincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteAProvincia
            End Get
        End Property

        Public ReadOnly Property ResidenteACAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteACAP
            End Get
        End Property

        Public ReadOnly Property ResidenteAVia As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteAVia
            End Get
        End Property


        Public ReadOnly Property CodiceFiscale As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceFiscale
            End Get
        End Property

        Public ReadOnly Property PartitaIVA As CCursorFieldObj(Of String)
            Get
                Return Me.m_PartitaIVA
            End Get
        End Property

        'Public ReadOnly Property NomeConsulente As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_NomeConsulente
        '    End Get
        'End Property

        Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulente
            End Get
        End Property

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonteContatto
            End Get
        End Property

        Public ReadOnly Property NomeFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFonte
            End Get
        End Property

        Public ReadOnly Property IDFonte As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonte
            End Get
        End Property

        'Public ReadOnly Property ProvvMax As CCursorField(Of Double)
        '    Get
        '        Return Me.m_ProvvMax
        '    End Get
        'End Property

        'Public ReadOnly Property MotivoSconto As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_MotivoSconto
        '    End Get
        'End Property

        'Public ReadOnly Property MotivoScontoDettaglio As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_MotivoScontoDettaglio
        '    End Get
        'End Property

        Public ReadOnly Property IDScontoAutorizzatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDScontoAutorizzatoDa
            End Get
        End Property

        Public ReadOnly Property ScontoAutorizzatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ScontoAutorizzatoIl
            End Get
        End Property

        Public ReadOnly Property ScontoAutorizzatoNote As CCursorFieldObj(Of String)
            Get
                Return Me.m_ScontoNomeMotivo
            End Get
        End Property


        '------------------------------------------------------
        ' PRODOTTO
        '------------------------------------------------------	
        Public ReadOnly Property NomeProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProdotto
            End Get
        End Property

        Public ReadOnly Property IDProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_ProdottoID
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        'Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_NomeOperatore
        '    End Get
        'End Property

        Public ReadOnly Property StatoPratica As CCursorField(Of StatoPraticaEnum)
            Get
                Return Me.m_StatoPratica
            End Get
        End Property

        Public ReadOnly Property IDStatoAttuale As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoAttuale
            End Get
        End Property

        Public ReadOnly Property NomeProfilo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProfilo
            End Get
        End Property

        Public ReadOnly Property IDCommerciale As CCursorField(Of Integer)
            Get
                Return Me.m_CommercialeID
            End Get
        End Property

        'Public ReadOnly Property NomeCommerciale As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_NomeCommerciale
        '    End Get
        'End Property

        'Public ReadOnly Property NomeProduttore As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_NomeProduttore
        '    End Get
        'End Property

        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
            End Get
        End Property

        Public ReadOnly Property NettoRicavo As CCursorField(Of Decimal)
            Get
                Return Me.m_NettoRicavo
            End Get
        End Property




        Public ReadOnly Property StatoRichiestaApprovazione As CCursorField(Of StatoRichiestaApprovazione)
            Get
                Return Me.m_StatoRichiestaApprovazione
            End Get
        End Property


#Region "Passaggi di stato"

        Public ReadOnly Property PassaggiDiStato As CCollection(Of CInfoStato)
            Get
                If (Me.m_PassaggiDiStato Is Nothing) Then
                    Me.m_PassaggiDiStato = New CCollection(Of CInfoStato)
                    Dim stati As StatoPraticaEnum() = {
                        StatoPraticaEnum.STATO_PREVENTIVO,
                        StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO,
                        StatoPraticaEnum.STATO_CONTRATTO_STAMPATO,
                        StatoPraticaEnum.STATO_CONTRATTO_FIRMATO,
                        StatoPraticaEnum.STATO_PRATICA_CARICATA,
                        StatoPraticaEnum.STATO_RICHIESTADELIBERA,
                        StatoPraticaEnum.STATO_DELIBERATA,
                        StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE,
                        StatoPraticaEnum.STATO_LIQUIDATA,
                        StatoPraticaEnum.STATO_ARCHIVIATA,
                        StatoPraticaEnum.STATO_ESTINTAANTICIPATAMENTE,
                        StatoPraticaEnum.STATO_ANNULLATA
                        }
                    For Each stato As StatoPraticaEnum In stati
                        Dim info As New CInfoStato(stato)
                        Me.m_PassaggiDiStato.Add(info)
                    Next
                    Me.m_PassaggiDiStato.Add(New CInfoStato)
                End If
                Return Me.m_PassaggiDiStato
            End Get
        End Property

        Protected Function GetStato(ByVal value As StatoPraticaEnum?) As CInfoStato
            For Each o As CInfoStato In Me.PassaggiDiStato
                If (value.HasValue) Then
                    If (o.MacroStato.HasValue AndAlso o.MacroStato.Value = value) Then Return o
                Else
                    If (o.MacroStato.HasValue = False) Then Return o
                End If
            Next
            Return Nothing
        End Function

        Public ReadOnly Property StatoPreventivo As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_PREVENTIVO)
            End Get
        End Property

        Public ReadOnly Property StatoPreventivoAccettato As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO)
            End Get
        End Property

        Public ReadOnly Property StatoContrattoStampato As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_CONTRATTO_STAMPATO)
            End Get
        End Property

        Public ReadOnly Property StatoContrattoFirmato As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_CONTRATTO_FIRMATO)
            End Get
        End Property

        Public ReadOnly Property StatoPraticaCaricata As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_PRATICA_CARICATA)
            End Get
        End Property

        Public ReadOnly Property StatoRichiestaDelibera As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_RICHIESTADELIBERA)
            End Get
        End Property

        Public ReadOnly Property StatoDeliberata As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_DELIBERATA)
            End Get
        End Property

        Public ReadOnly Property StatoProntaPerLiquidazione As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE)
            End Get
        End Property

        Public ReadOnly Property StatoLiquidata As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_LIQUIDATA)
            End Get
        End Property

        Public ReadOnly Property StatoArchiviata As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_ARCHIVIATA)
            End Get
        End Property

        Public ReadOnly Property StatoEstintaAnticipatamente As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_ESTINTAANTICIPATAMENTE)
            End Get
        End Property

        Public ReadOnly Property StatoAnnullata As CInfoStato
            Get
                Return Me.GetStato(StatoPraticaEnum.STATO_ANNULLATA)
            End Get
        End Property

        Public ReadOnly Property StatoGenerico As CInfoStato
            Get
                Return Me.GetStato(Nothing)
            End Get
        End Property


#End Region


        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_IDCessionario
            End Get
        End Property


        Protected Overrides Sub OnInitialize(item As Object)
            MyBase.OnInitialize(item)
        End Sub

        '-----------------------------------------------------------

        Protected Overrides Function GetModule() As CModule
            Return minidom.Finanziaria.Pratiche.Module
        End Function

        Public Overrides Function GetSQL() As String
            Dim ret As String
            Dim wherePart As String = Me.GetWherePart
            Dim items As CCollection(Of CInfoStato) = Me.PassaggiDiStato
            Dim isSet As Boolean = False

            For Each s As CInfoStato In items
                If (s.IsSet) Then
                    isSet = True
                    Exit For
                End If
            Next

            If (isSet) Then
                ret = ""
                Dim cnt As Integer = 0
                For i As Integer = 0 To items.Count - 1
                    Dim item As CInfoStato = items(i)
                    Dim tmp As String = ""
                    If (item.IsSet) Then
                        tmp &= "(SELECT [IDPratica] FROM [tbl_PraticheSTL] WHERE [tbl_PraticheSTL].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND " & item.GetSQL() & " GROUP BY [IDPratica]) AS [T" & i & "] "
                        If (cnt = 0) Then
                            ret = "SELECT [tbl_Pratiche].* FROM [tbl_Pratiche] INNER JOIN " & tmp
                            ret &= " ON [tbl_Pratiche].[ID]=[T" & i & "].[IDPratica]"
                        Else
                            ret = "SELECT [A" & i & "].* FROM (" & ret & ") As [A" & i & "] INNER JOIN " & tmp
                            ret &= " ON [A" & i & "].[ID]=[T" & i & "].[IDPratica]"
                        End If
                        cnt += 1
                    End If
                Next

                'If (Me.m_IDScontoAutorizzatoDa.IsSet) Then
                '    ret = "SELECT * FROM (SELECT [B].*, [tbl_PraticheInfo].[IDScontoAutorizzatoDa] FROM (" & ret & ") AS [B] INNER JOIN [tbl_PraticheInfo] ON [B].[ID] = [tbl_PraticheInfo].[IDPratica])"
                '    wherePart = Strings.Combine(wherePart, "[IDScontoAutorizzatoDa]=" & DBUtils.DBNumber(Me.m_IDScontoAutorizzatoDa.Value), " AND ")
                'End If
                If (wherePart <> "") Then ret = ret & " WHERE " & wherePart
            Else
                'If (Me.m_IDScontoAutorizzatoDa.IsSet) Then
                '    ret = "SELECT * FROM (SELECT [tbl_Pratiche].*, [tbl_PraticheInfo].[IDScontoAutorizzatoDa] FROM [tbl_Pratiche] INNER JOIN [tbl_PraticheInfo] ON [tbl_Pratiche].[ID] = [tbl_PraticheInfo].[IDPratica]) WHERE "
                '    wherePart = Strings.Combine(wherePart, "[IDScontoAutorizzatoDa]=" & DBUtils.DBNumber(Me.m_IDScontoAutorizzatoDa.Value), " AND ")
                '    ret &= wherePart
                'Else
                ret = MyBase.GetSQL()
                'End If
            End If

            If (Me.m_StatoRichiestaApprovazione.IsSet OrElse
                Me.m_IDScontoRichiestoDa.IsSet OrElse
                Me.m_IDScontoAutorizzatoDa.IsSet OrElse
                Me.m_ScontoAutorizzatoIl.IsSet OrElse
                Me.m_ScontoNomeMotivo.IsSet) Then
                Dim where1 As String = ""
                where1 = " [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [TipoOggettoApprovabile]='CPraticaCQSPD' "
                If (Me.m_StatoRichiestaApprovazione.IsSet) Then where1 = Strings.Combine(where1, Me.m_StatoRichiestaApprovazione.GetSQL("StatoRichiesta"), " AND ")
                If (Me.m_IDScontoAutorizzatoDa.IsSet) Then where1 = Strings.Combine(where1, Me.m_IDScontoAutorizzatoDa.GetSQL("IDConfermataDa"), " AND ")
                If (Me.m_ScontoAutorizzatoIl.IsSet) Then where1 = Strings.Combine(where1, Me.m_ScontoAutorizzatoIl.GetSQL("DataConferma"), " AND ")
                If (Me.m_ScontoNomeMotivo.IsSet) Then where1 = Strings.Combine(where1, Me.m_ScontoNomeMotivo.GetSQL("NomeMotivoRichiesta"), " AND ")
                If (Me.m_IDScontoRichiestoDa.IsSet) Then where1 = Strings.Combine(where1, Me.m_IDScontoRichiestoDa.GetSQL("IDUtenteRichiestaApprovazione"), " AND ")
                ret = "SELECT [TRA1].* FROM (" & ret & ") AS [TRA1] INNER JOIN (SELECT * FROM [tbl_CQSPDRichiesteApprovazione] WHERE " & where1 & ") AS [TRA2] ON [TRA1].[ID]=[TRA2].[IDOggettoApprovabile]"
            End If



            Return ret
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart
            'If Me.StatoContatto.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoContatto.GetCampoSQL, " AND ")
            'If Me.StatoRichiestaDelibera.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoRichiestaDelibera.GetCampoSQL, " AND ")
            'If Me.StatoDelibera.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoDelibera.GetCampoSQL, " AND ")
            'If Me.StatoProntaPerLiquidazione.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoProntaPerLiquidazione.GetCampoSQL, " AND ")
            'If Me.StatoLiquidazione.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoLiquidazione.GetCampoSQL, " AND ")
            'If Me.StatoArchiviazione.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoArchiviazione.GetCampoSQL, " AND ")
            'If Me.StatoAnnullamento.IsSet Then wherePart = Strings.Combine(wherePart, Me.StatoAnnullamento.GetCampoSQL, " AND ")

            'With Me.DataTrasferimento
            '    If .IsSet Then wherePart = Strings.Combine(wherePart, GetCampoSQL("DataTrasferimento", .Tipo, .Inizio, .Fine), " AND ")
            'End With
            If Not Me.IgnoreRights Then
                If Not Me.Module.UserCanDoAction("seeliqui") Then
                    wherePart = Strings.Combine(wherePart, "([StatoPratica] Is Null Or [StatoPratica]<" & StatoPraticaEnum.STATO_LIQUIDATA & ")", " AND ")
                End If
                If Not Me.Module.UserCanDoAction("seearch") Then
                    wherePart = Strings.Combine(wherePart, "([StatoPratica] Is Null Or [StatoPratica]<" & StatoPraticaEnum.STATO_ARCHIVIATA & ")", " AND ")
                End If
            End If
            If Me.m_RinnovaDa <> vbNullString Then
                wherePart = Strings.Combine(wherePart, "([ID] In (SELECT [IDPratica] FROM [tbl_Estinzioni] WHERE [NomeIstituto]=" & DBUtils.DBString(Me.m_RinnovaDa) & " And [Stato]=" & ObjectStatus.OBJECT_VALID & ") GROUP BY [IDPratica])", " AND ")
            End If
            If (Me.m_Nominativo.IsSet) Then
                Me.m_Nominativo.Value = Strings.Replace(Me.m_Nominativo.Value, "  ", " ")
                wherePart = Strings.Combine(wherePart, "(([CognomeCliente] & ' ' & [NomeCliente] Like '" & Strings.Replace(Me.m_Nominativo.Value, "'", "''") & "%') Or ([NomeCliente] & ' ' & [CognomeCliente] Like '" & Strings.Replace(Me.m_Nominativo.Value, "'", "''") & "%'))", " AND ")
            End If
            If (Me.m_CategoriaProdotto.IsSet) Then
                Dim cursor As New CProdottiCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IgnoreRights = True
                    cursor.Categoria.InitFrom(Me.m_CategoriaProdotto)

                    Dim list As New System.Collections.ArrayList
                    While Not cursor.EOF
                        list.Add(GetID(cursor.Item))
                        cursor.MoveNext()
                    End While

                    Dim buffer As New System.Text.StringBuilder
                    buffer.Append("[Prodotto] In (0")

                    If (list.Count > 0) Then
                        For i As Integer = 0 To list.Count - 1
                            buffer.Append(",")
                            buffer.Append(list(i))
                        Next

                    End If

                    buffer.Append(")")

                    wherePart = Strings.Combine(wherePart, buffer.ToString, " AND ")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                End Try

            End If
            Return wherePart
        End Function

        Public Overrides Function GetWherePartLimit() As String
            Dim tmpSQL As New System.Text.StringBuilder
            If Not Me.Module.UserCanDoAction("list") Then
                If Me.Module.UserCanDoAction("list_office") Then
                    Dim items As CCollection(Of CUfficio) = Users.CurrentUser.Uffici
                    tmpSQL.Append("[IDPuntoOperativo] In (0, NULL")
                    For i As Integer = 0 To items.Count - 1
                        tmpSQL.Append(",")
                        tmpSQL.Append(DBUtils.DBNumber(GetID(items(i))))
                    Next
                    tmpSQL.Append(")")
                End If
                If Me.Module.UserCanDoAction("list_own") Then
                    If tmpSQL.Length > 0 Then tmpSQL.Append(" OR ")
                    tmpSQL.Append("([CreatoDa]=" & GetID(Users.CurrentUser) & ")")
                End If
                If Me.Module.UserCanDoAction("list_assigned") Then
                    Dim col As CCollaboratore = Finanziaria.Collaboratori.GetItemByUser(Sistema.Users.CurrentUser)
                    If (col IsNot Nothing) Then
                        If tmpSQL.Length > 0 Then tmpSQL.Append(" OR ")
                        tmpSQL.Append("([IDCollaboratore]=" & GetID(col) & ")")
                    End If
                End If
                If tmpSQL.Length = 0 Then tmpSQL.Append("(0<>0)")
            End If
            Return tmpSQL.ToString
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Pratiche"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CPraticaCQSPD
        End Function

        Public Overrides Sub InitFrom(cursor As DBObjectCursorBase)
            With DirectCast(cursor, CPraticheCQSPDCursor)
                For Each tgt In Me.PassaggiDiStato
                    Dim src As CInfoStato = .GetStato(tgt.MacroStato)
                    tgt.CopyFrom(src)
                Next
                Me.m_RinnovaDa = .m_RinnovaDa
            End With
            MyBase.InitFrom(cursor)
        End Sub

        Public Sub SyncInfo()
            Dim arr As Array = Me.GetItemsArray
            Dim buffer As New System.Collections.ArrayList
            For i As Integer = 0 To Arrays.Len(arr) - 1
                Dim r As CPraticaCQSPD = arr.GetValue(i)
                If (r IsNot Nothing) Then
                    buffer.Add(GetID(r))
                End If
            Next
            If (buffer.Count > 0) Then
                'Dim cursor As New CInfoPraticaCursor
                'Dim arrp() As Integer = buffer.ToArray(GetType(Integer))
                'cursor.IDPratica.ValueIn(arrp)
                'cursor.IgnoreRights = True
                'While Not cursor.EOF
                '    Dim info As CInfoPratica = cursor.Item
                '    For i As Integer = 0 To Arrays.Len(arr) - 1
                '        Dim r As CPraticaCQSPD = arr.GetValue(i)
                '        If (GetID(r) = info.IDPratica) Then
                '            info.SetPratica(r)
                '            r.SetInfo(info)
                '        End If
                '    Next
                '    cursor.MoveNext()
                'End While
                'cursor.Dispose()
            End If
        End Sub

        Public Sub SyncStatiLav()
            Dim arr As Array = Me.GetItemsArray
            Dim buffer As New System.Collections.ArrayList
            For i As Integer = 0 To Arrays.Len(arr) - 1
                Dim r As CPraticaCQSPD = arr.GetValue(i)
                If (r IsNot Nothing) Then
                    buffer.Add(GetID(r))
                End If
            Next
            If (buffer.Count > 0) Then
                Dim col As New CCollection(Of CStatoLavorazionePratica)
                Dim cursor As New CStatiLavorazionePraticaCursor
                Dim arrp() As Integer = buffer.ToArray(GetType(Integer))
                cursor.IDPratica.ValueIn(arrp)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Dim stl As CStatoLavorazionePratica = cursor.Item
                    col.Add(stl)
                    cursor.MoveNext()
                End While
                cursor.Dispose()

                For i As Integer = 0 To Arrays.Len(arr) - 1
                    Dim r As CPraticaCQSPD = arr.GetValue(i)
                    If (r IsNot Nothing) Then
                        Dim statilav As New CStatiLavorazionePraticaCollection
                        statilav.SetPratica(r)
                        For Each stl In col
                            If stl.IDPratica = GetID(r) Then
                                statilav.Add(stl)
                            End If
                        Next
                        statilav.Sort()
                        r.SetStatiDiLavorazione(statilav)
                        Dim sta As CStatoLavorazionePratica = statilav.GetItemById(r.IDStatoDiLavorazioneAttuale)
                        If (sta IsNot Nothing) Then r.SetStatoDiLavorazioneAttuale(sta)
                    End If
                Next
            End If
        End Sub

        '--------------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As minidom.XML.XMLWriter)
            writer.WriteAttribute("RinnovaDa", Me.m_RinnovaDa)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("PassaggiDiStato", Me.PassaggiDiStato)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "PassaggiDiStato"
                    Me.m_PassaggiDiStato = New CCollection(Of CInfoStato)
                    Me.m_PassaggiDiStato.AddRange(DirectCast(fieldValue, CCollection))
                Case "RinnovaDa" : Me.m_RinnovaDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Dim ret As New CKeyCollection(Of CCursorField)(MyBase.GetWhereFields())
            ret.RemoveByKey(Me.m_IDScontoAutorizzatoDa.FieldName)
            ret.RemoveByKey(Me.m_Nominativo.FieldName)
            ret.RemoveByKey(Me.m_StatoRichiestaApprovazione.FieldName)
            ret.RemoveByKey(Me.m_ScontoAutorizzatoIl.FieldName)
            ret.RemoveByKey(Me.m_ScontoNomeMotivo.FieldName)
            ret.RemoveByKey(Me.m_IDScontoRichiestoDa.FieldName)
            ret.RemoveByKey(Me.m_CategoriaProdotto.FieldName)
            'ret.RemoveByKey(Me.m_TipoFonteCliente.FieldName)
            'ret.RemoveByKey(Me.m_IDFonteCliente.FieldName)
            Return ret
        End Function

        Protected Overrides Function GetSortPart(field As CCursorField) As String
            Select Case field.FieldName
                Case "Nominativo"
                    Select Case (field.SortOrder)
                        Case SortEnum.SORT_ASC : Return "[CognomeCliente] & ' ' & [NomeCliente] ASC"
                        Case SortEnum.SORT_DESC : Return "[CognomeCliente] & ' ' & [NomeCliente] DESC"
                    End Select
                Case "StatoPratica"
                    Select Case (field.SortOrder)
                        Case SortEnum.SORT_ASC : Return "[Ordine] ASC"
                        Case SortEnum.SORT_DESC : Return "[Ordine] DESC"
                    End Select
                Case Me.m_StatoRichiestaApprovazione.FieldName,
                     Me.m_ScontoAutorizzatoIl.FieldName,
                     Me.m_ScontoNomeMotivo.FieldName,
                     Me.m_IDScontoRichiestoDa.FieldName,
                     Me.m_IDScontoAutorizzatoDa.FieldName ', _
                    ' Me.m_TipoFonteCliente.FieldName, _
                    ' Me.m_IDFonteCliente.FieldName

                    Return ""
                Case Else

            End Select

            Return MyBase.GetSortPart(field)
        End Function

        Protected Overrides Sub SyncPage()
            MyBase.SyncPage()
            Me.SyncInfo()
            Me.SyncStatiLav()
        End Sub

    End Class

End Class
