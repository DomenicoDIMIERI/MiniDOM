Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria


    Public Class COffertaCQSCalculator

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)

        Private m_TANDigits As Integer
        Private m_TAEGDigits As Integer
        Private m_OneriErariali As Decimal
        Private m_Diretta As Boolean
        Private m_NettoAllaMano As Decimal
        Private m_TAN As Double
        Private m_TAEG As Double
        Private m_Rata As Decimal
        Private m_Durata As Integer
        Private m_CapitaleFinanziato As Decimal
        Private m_Interessi As Decimal
        Private m_NettoErogato As Decimal
        Private m_ProvvMax As Decimal
        Private m_Commissioni As Decimal
        Private m_ProvvTAN As Decimal
        Private m_Spese As Decimal
        Private m_Sconto As Double
        Private m_ValoreSconto As Decimal
        Private m_CommissioniTotaliAgenzia As Decimal
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_Cessionari As CCollection(Of CCQSPDCessionarioClass)
        Private m_Profili As CCollection(Of CProfilo)
        Private m_Prodotti As CCollection(Of CCQSPDProdotto)
        Private m_Tabelle As CCollection(Of CProdottoXTabellaFin)
        Private m_Assicurazioni As CCollection(Of CProdottoXTabellaAss)
        Private m_TabelleSpese As CCollection(Of CProdottoXTabellaSpesa)
        Private m_Offerta As COffertaCQS
        Private m_Profilo As CProfilo
        Private m_Prodotto As CCQSPDProdotto
        Private m_TabellaFinRel As CProdottoXTabellaFin
        Private m_TabellaAssRel As CProdottoXTabellaAss
        Private m_TabellaSpeseRel As CProdottoXTabellaSpesa
        Private m_DataCaricamento As Date
        Private m_Decorrenza As Date
        Private m_Cliente As CPersonaFisica
        Private m_Estinzioni As CCollection(Of EstinzioneXEstintore)
        Private m_W As FinestraLavorazione
        Private m_Collaboratori As CCollection(Of ClienteXCollaboratore)
        Private m_Collaboratore As CCollaboratore
        Private m_ClienteXCollaboratore As ClienteXCollaboratore
        Private m_CollaboratoriXCliente As CCollection(Of ClienteXCollaboratore)
        Private m_SommaMontanteResiduo As Decimal
        Private m_SommaEstinzioni As Decimal
        Private m_Calculated As Boolean
        Private m_User As CUser

        Public Sub New()
            Me.m_TANDigits = 2
            Me.m_TAEGDigits = 2
            Me.m_OneriErariali = 0
            Me.m_Diretta = True
            Me.m_Cessionario = Nothing
            Me.m_Profili = Nothing
            Me.m_Prodotti = Nothing
            Me.m_Tabelle = Nothing
            Me.m_Assicurazioni = Nothing
            Me.m_TabelleSpese = Nothing
            Me.m_Offerta = Nothing
            Me.m_Profilo = Nothing
            Me.m_Prodotto = Nothing
            Me.m_TabellaFinRel = Nothing
            Me.m_TabellaAssRel = Nothing
            Me.m_TabellaSpeseRel = Nothing
            Me.m_CommissioniTotaliAgenzia = 0
            Me.m_ValoreSconto = 0
            Me.m_DataCaricamento = DateUtils.ToDay
            Me.m_Decorrenza = Finanziaria.CalcolaDecorrenza(Me.m_DataCaricamento)
            Me.m_Cliente = Nothing
            Me.m_Estinzioni = Nothing
            Me.m_W = Nothing
            Me.m_Collaboratori = Nothing
            Me.m_Collaboratore = Nothing
            Me.m_ClienteXCollaboratore = Nothing
            Me.m_CollaboratoriXCliente = Nothing
            Me.m_SommaMontanteResiduo = 0
            Me.m_SommaEstinzioni = 0
            Me.m_Calculated = False
            Me.m_User = Sistema.Users.CurrentUser
        End Sub

        Public Property ValoreSconto As Decimal
            Get
                Me.Validate()
                Return Me.m_ValoreSconto
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_ValoreSconto
                If (oldValue = value) Then Return
                Me.m_ValoreSconto = value
                Me.DoChanged("ValoreSconto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente configurato per il calcolatore
        ''' </summary>
        ''' <returns></returns>
        Public Property User As CUser
            Get
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_User
                If (oldValue Is value) Then Return
                Me.m_User = value
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il cliente  
        ''' </summary>
        ''' <returns></returns>
        Public Property Cliente As CPersonaFisica
            Get
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue Is value) Then Return
                Me.SetCliente(value)
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata del prestito in mesi
        ''' </summary>
        ''' <returns></returns>
        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.Durata
                If (oldValue = value) Then Return
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        Public Function CalcolaEta(ByVal data As Date) As Double
            Return Me.m_Cliente.Eta(data)
        End Function

        Public Function CalcolaAnzianita(ByVal data As Date) As Double
            Return Me.m_Cliente.ImpiegoPrincipale.Anzianita(data)
        End Function

        Public Function CalcolaEtaAInizioFinanziamento() As Double
            Return Me.CalcolaEta(Me.DataDecorrenza)
        End Function

        Public Function CalcolaEtaAFineFinanziamento() As Double
            Dim durata As Integer = Me.Durata
            Dim decorrenza As Date = Me.DataDecorrenza
            Return Me.CalcolaEta(DateUtils.DateAdd("M", durata, decorrenza))
        End Function

        Public Function CalcolaAnzianitaAInizioFinanziamento() As Double
            Return Me.CalcolaAnzianita(Me.DataDecorrenza)
        End Function

        Public Function CalcolaAnzianitaAFineFinanziamento() As Double
            Dim durata As Integer = Me.Durata
            Dim decorrenza As Date = Me.DataDecorrenza
            Return Me.CalcolaAnzianita(DateUtils.DateAdd("M", durata, decorrenza))
        End Function

        Public Sub SetOfferta(ByVal value As COffertaCQS)
            'Dim i As Integer
            Dim ddec As Date = Me.DataDecorrenza

            Me.m_Collaboratore = value.Collaboratore
            Me.m_ClienteXCollaboratore = value.ClienteXCollaboratore

            Me.m_Estinzioni = New CEstinzioniXEstintoreCollection(value)
            Me.m_Cessionario = value.Cessionario
            Me.m_Profili = New CCollection(Of CProfilo)
            Me.m_Profilo = value.Profilo

            Dim profili As CCollection(Of CProfilo) = Finanziaria.Profili.GetPreventivatoriUtente(ddec, True)
            For Each prof As CProfilo In profili
                If (prof.IDCessionario = GetID(Me.m_Cessionario) AndAlso prof.IsValid(ddec)) Then
                    Me.m_Profili.Add(prof)
                End If
            Next

            Me.m_Prodotto = value.Prodotto

            Me.m_Tabelle = New CCollection(Of CProdottoXTabellaFin)
            Me.m_Assicurazioni = Nothing
            Me.m_TabelleSpese = Nothing
            Me.m_TabellaAssRel = value.TabellaAssicurativaRel
            Me.m_TabellaSpeseRel = value.TabellaSpese
            Me.m_TabellaFinRel = value.TabellaFinanziariaRel

            Me.m_OneriErariali = value.OneriErariali
            Me.m_TAN = value.TAN
            Me.m_TAEG = value.TAEG
            Me.m_Rata = value.Rata
            Me.m_Durata = value.Durata
            Me.m_CapitaleFinanziato = value.CapitaleFinanziato
            Me.m_Interessi = value.Interessi
            Me.m_NettoErogato = value.NettoRicavo
            Me.m_NettoAllaMano = Me.m_NettoErogato
            Me.m_ProvvMax = value.ValoreProvvigioneMassima
            Me.m_Commissioni = value.ValoreSpread
            Me.m_ProvvTAN = value.ValoreProvvTAN
            Me.m_Spese = value.Imposte
            Me.m_Sconto = value.ValoreRiduzioneProvvigionale
            Dim daEstinguere As Decimal = 0
            Dim residuo As Decimal = value.getSommaDebitiResidui(Me.m_Estinzioni)
            Dim ml As Decimal = (value.MontanteLordo - residuo)

            For Each est As EstinzioneXEstintore In Me.m_Estinzioni
                If (est.Selezionata) Then
                    daEstinguere += est.TotaleDaRimborsare
                End If
            Next

            Me.m_NettoAllaMano = Me.m_NettoErogato - daEstinguere


            If (Me.m_Prodotto IsNot Nothing) Then
                For Each tbl As CProdottoXTabellaFin In Me.m_Prodotto.TabelleFinanziarieRelations
                    If (tbl.Stato = ObjectStatus.OBJECT_VALID AndAlso tbl.Tabella IsNot Nothing AndAlso tbl.Tabella.Stato = ObjectStatus.OBJECT_VALID AndAlso tbl.Tabella.IsValid(ddec)) Then
                        Me.m_Tabelle.Add(tbl)
                    End If
                Next


            End If



            Me.m_Diretta = Sistema.TestFlag(value.Flags, OffertaFlags.DirettaCollaboratore)


            Dim provvcoll As Decimal = value.getSommaProvvigioniA(CQSPDTipoSoggetto.Collaboratore) - value.ValoreRiduzioneProvvigionale

            Me.m_Estinzioni = Nothing
        End Sub

        Protected Sub DoChanged(ByVal propName As String, ByVal newValue As Object, ByVal oldValue As Object)
            Me.m_Calculated = False
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propName, newValue, oldValue))
        End Sub

        Public Property DataDecorrenza As Date
            Get
                Return Me.m_Decorrenza
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Decorrenza
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Decorrenza = value
                Me.doDecorrenzaChanged()
                Me.DoChanged("DataDecorrenza", value, oldValue)
            End Set
        End Property

        Public Property Diretta As Boolean
            Get
                Return Me.m_Diretta
            End Get
            Set(value As Boolean)
                If (Me.Diretta = value) Then Return
                Me.m_Diretta = value
                Me.DoChanged("Diretta", value, Not value)
            End Set
        End Property

        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then
                    Me.m_Collaboratore = Finanziaria.Collaboratori.GetItemByUser(Me.User)
                End If
                If (Me.m_Collaboratore Is Nothing) Then
                    Dim w As FinestraLavorazione = Me.FinestraCorrente
                    If (w IsNot Nothing) Then Me.m_Collaboratore = w.Collaboratore
                End If
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.m_Collaboratore
                If (oldValue Is value) Then Return
                Me.m_Collaboratore = value
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        'Public Property Collaboratore As CCollaboratore
        '    Get
        '        Return Me.m_Collaboratore
        '    End Get
        '    Set(value As CCollaboratore)
        '        Dim oldValue As CCollaboratore = Me.m_Collaboratore
        '        If (value Is oldValue) Then Return
        '        Me.m_Collaboratore = value
        '        'If (cbo.options.length === 2) Then cbo.selectedIndex = 1
        '        'document.getElementById(Me.getCUID() + "trCessionario").style.display = (cbo.options.length > 2) ? "" : "none"
        '        Me.doCessionarioChanged()
        '        Me._checkButtons()
        '        Me.DoChanged("Collaboratore", value, oldValue)
        '    End Set
        'End Property

        Public Property FinestraCorrente As FinestraLavorazione
            Get
                If (Me.m_W Is Nothing AndAlso Me.m_Cliente IsNot Nothing) Then
                    Me.m_W = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(Me.m_Cliente)
                End If
                Return Me.m_W
            End Get
            Set(value As FinestraLavorazione)
                Dim oldValue As FinestraLavorazione = Me.m_W
                If (value Is oldValue) Then Return
                Me.m_W = value
                Me.DoChanged("FinestraCorrente", value, oldValue)
            End Set
        End Property

        Protected Sub doProfiloChanged()
            'Me.m_Profilo = Me.m_Profili.GetItemById(GetFieldValue(Me.getCUID() + "profilo"))
            Me.m_Prodotti = Nothing
            Me.m_Prodotto = Nothing
            'If (Me.m_Prodotti.Count = 1) Then Me.m_Prodotto = Me.m_Prodotti(0)
            Me.doProdottoChanged()
        End Sub

        Protected Sub doProdottoChanged()
            Dim ddec As Date = Me.DataDecorrenza

            'Me.m_Prodotto = Me.m_Prodotti.GetItemById(GetFieldValue(Me.getCUID() + "prod"))
            Me.m_Tabelle = Nothing : Me.m_TabellaFinRel = Nothing
            Me.m_Assicurazioni = Nothing : Me.m_TabellaAssRel = Nothing
            Me.m_TabelleSpese = Nothing : Me.m_TabellaSpeseRel = Nothing


            'If (Me.m_Cliente IsNot Nothing OrElse Me.m_ClienteXCollaboratore IsNot Nothing) Then
            '    Me.ChangeEstinzione(0, False)
            'End If

            'Me.doSpeseChanged()
        End Sub

        Public ReadOnly Property Prodotti As CCollection(Of CCQSPDProdotto)
            Get
                If (Me.m_Prodotti Is Nothing) Then
                    Me.m_Prodotti = Me.GetElencoProdotti(Me.Profilo, Me.DataCaricamento)
                    If (Me.m_Prodotti.Count = 1) Then
                        Me.m_Prodotto = Me.m_Prodotti(0)
                    End If
                End If
                Return Me.m_Prodotti
            End Get
        End Property

        Public Property TabellaAssicurativaRel As CProdottoXTabellaAss
            Get
                If (Me.m_TabellaAssRel Is Nothing AndAlso Me.TabelleAssicurative.Count = 1) Then
                    Me.m_TabellaAssRel = Me.TabelleAssicurative(0)
                End If
                Return Me.m_TabellaAssRel
            End Get
            Set(value As CProdottoXTabellaAss)
                Dim oldValue As CProdottoXTabellaAss = Me.TabellaAssicurativaRel
                If (value Is oldValue) Then Return
                Me.m_TabellaAssRel = value
                Me.DoChanged("TabellaAssicurativaRel", value, oldValue)
            End Set
        End Property

        Public Property TabellaFinanziariaRel As CProdottoXTabellaFin
            Get
                If (Me.m_TabellaFinRel Is Nothing AndAlso Me.TabelleFinanziarie.Count = 1) Then
                    Me.m_TabellaFinRel = Me.TabelleFinanziarie(0)
                End If
                Return Me.m_TabellaFinRel
            End Get
            Set(value As CProdottoXTabellaFin)
                Dim oldValue As CProdottoXTabellaFin = Me.TabellaFinanziariaRel
                If (value Is oldValue) Then Return
                Me.m_TabellaFinRel = value
                Me.DoChanged("TabellaFinanziariaRel", value, oldValue)
            End Set
        End Property

        Public Property TabellaSpeseRel As CProdottoXTabellaSpesa
            Get
                If (Me.m_TabellaSpeseRel Is Nothing AndAlso Me.TabelleSpese.Count = 1) Then
                    Me.m_TabellaSpeseRel = Me.m_TabelleSpese(0)
                End If
                Return Me.m_TabellaSpeseRel
            End Get
            Set(value As CProdottoXTabellaSpesa)
                Dim oldValue As CProdottoXTabellaSpesa = Me.TabellaSpeseRel
                If (value Is oldValue) Then Return
                Me.m_TabellaSpeseRel = value
                Me.DoChanged("TabellaSpeseRel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione di profili validi per la configurazione corrente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Profili As CCollection(Of CProfilo)
            Get
                If (Me.m_Profili Is Nothing) Then
                    Me.m_Profili = New CCollection(Of CProfilo)

                    Dim ddec As Date = Me.DataCaricamento
                    Dim items As CCollection = Nothing
                    Dim col As CCollaboratore = Me.Collaboratore

                    If (col IsNot Nothing) Then
                        items = New CCollection()
                        If (col.ListinoPredefinito IsNot Nothing) Then
                            items.Add(col.ListinoPredefinito)
                        End If
                    Else
                        items = Finanziaria.Profili.GetPreventivatoriUtente(ddec, True)
                    End If

                    For Each prof As CProfilo In items
                        If (prof.IDCessionario = GetID(Me.m_Cessionario) AndAlso prof.IsValid(ddec)) Then
                            Me.m_Profili.Add(prof)
                        End If
                    Next

                    If (Me.m_Profili.Count = 1) Then Me.m_Profilo = Me.m_Profili(0)
                End If
                Return Me.m_Profili
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle tabelle finanziarie valide per la configurazione corrente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TabelleFinanziarie As CCollection(Of CProdottoXTabellaFin)
            Get
                If (Me.m_Tabelle Is Nothing) Then
                    Me.m_Tabelle = New CCollection(Of CProdottoXTabellaFin)
                    If (Me.Prodotto IsNot Nothing) Then
                        Dim ddec As Date = Me.DataCaricamento
                        For Each tbl As CProdottoXTabellaFin In Me.Prodotto.TabelleFinanziarieRelations
                            If (tbl.Stato = ObjectStatus.OBJECT_VALID AndAlso tbl.Tabella IsNot Nothing AndAlso tbl.Tabella.Stato = ObjectStatus.OBJECT_VALID AndAlso tbl.Tabella.IsValid(ddec)) Then
                                Me.m_Tabelle.Add(tbl)
                            End If
                        Next
                    End If
                    If (Me.m_Tabelle.Count = 1) Then Me.m_TabellaFinRel = Me.m_Tabelle(0)
                End If
                Return Me.m_Tabelle
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle tabelle assicurative valide per la configurazione corrente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TabelleAssicurative As CCollection(Of CProdottoXTabellaAss)
            Get
                If (Me.m_Assicurazioni Is Nothing) Then
                    Me.m_Assicurazioni = New CCollection(Of CProdottoXTabellaAss)
                    If (Me.Prodotto IsNot Nothing) Then
                        Dim ddec As Date = Me.DataCaricamento
                        For Each tbl As CProdottoXTabellaAss In Me.Prodotto.TabelleAssicurativeRelations
                            If (tbl.Stato = ObjectStatus.OBJECT_VALID) Then
                                Me.m_Assicurazioni.Add(tbl)
                            End If
                        Next
                    End If
                    If (Me.m_Assicurazioni.Count = 1) Then Me.m_TabellaAssRel = Me.m_Assicurazioni(0)
                End If
                Return Me.m_Assicurazioni
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle tabelle spese valide per la configurazione corrente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TabelleSpese As CCollection(Of CProdottoXTabellaSpesa)
            Get
                If (Me.m_TabelleSpese Is Nothing) Then
                    Me.m_TabelleSpese = New CCollection(Of CProdottoXTabellaSpesa)
                    If (Me.Prodotto IsNot Nothing) Then
                        Dim ddec As Date = Me.DataCaricamento
                        For Each tbl As CProdottoXTabellaSpesa In Me.Prodotto.TabelleSpese
                            If (tbl.Stato = ObjectStatus.OBJECT_VALID AndAlso tbl.IsValid(ddec) AndAlso tbl.TabellaSpese IsNot Nothing AndAlso tbl.TabellaSpese.IsValid(ddec)) Then
                                Me.m_TabelleSpese.Add(tbl)
                            End If
                        Next
                    End If
                    If (Me.m_TabelleSpese.Count = 1) Then Me.m_TabellaSpeseRel = Me.m_TabelleSpese(0)
                End If
                Return Me.m_TabelleSpese
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il cessionario selezionato per il calcolo
        ''' </summary>
        ''' <returns></returns>
        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing AndAlso Me.Cessionari.Count = 1) Then
                    Me.m_Cessionario = Me.Cessionari(0)
                End If
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.m_Cessionario
                If (value Is oldValue) Then Return
                Me.m_Cessionario = value
                Me.doCessionarioChanged()
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'elenco dei cessionari validi per la configurazione corrente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Cessionari As CCollection(Of CCQSPDCessionarioClass)
            Get
                If (Me.m_Cessionari Is Nothing) Then
                    Me.m_Cessionari = New CCollection(Of CCQSPDCessionarioClass)
                    Dim ddec As Date = Me.DataDecorrenza
                    Dim items As CCollection(Of CCQSPDCessionarioClass) = Finanziaria.Cessionari.GetAllCessionari()
                    For Each c As CCQSPDCessionarioClass In items
                        If (c.Stato = ObjectStatus.OBJECT_VALID AndAlso c.IsValid(ddec) AndAlso c.Visibile AndAlso c.UsabileInPratiche) Then
                            Me.m_Cessionari.Add(c)
                        End If
                    Next
                End If
                Return Me.m_Cessionari
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'elenco dei collaboratori a cui è stato assegnato il cliente nella data specificata
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        Public Function GetCollaboratori(ByVal d As Date) As CCollection(Of CCollaboratore)
            Dim ret As New CCollection(Of CCollaboratore)
            Dim items As CCollection(Of ClienteXCollaboratore) = Me.CollaboratoriXCliente
            For Each c As ClienteXCollaboratore In items
                Dim col As CCollaboratore = Nothing
                If (c.Stato = ObjectStatus.OBJECT_VALID) Then
                    col = c.Collaboratore
                End If
                If (col IsNot Nothing AndAlso col.Stato = ObjectStatus.OBJECT_VALID) Then
                    ret.Add(col)
                End If
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la collezione dei prodotti inclusi nel profilo e validi alla data specificata
        ''' </summary>
        ''' <param name="profilo"></param>
        ''' <param name="ddec"></param>
        ''' <returns></returns>
        Public Function GetElencoProdotti(ByVal profilo As CProfilo, ByVal ddec As Date) As CCollection(Of CCQSPDProdotto)
            Dim ret As New CCollection(Of CCQSPDProdotto)
            If (profilo IsNot Nothing) Then
                Dim prodotti As CCollection(Of CCQSPDProdotto) = profilo.ProdottiXProfiloRelations().GetProdotti()
                For Each prodotto As CCQSPDProdotto In prodotti
                    If (prodotto.IsValid(ddec)) Then
                        ret.Add(prodotto)
                    End If
                Next
            End If
            Return ret
        End Function

        Public Function IsRapportini() As Boolean

            Return Finanziaria.Consulenze.Module.UserCanDoAction("showprovvigioni")
        End Function


        ''' <summary>
        ''' Restituisce o imposta la relazione tra il cliente ed il collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property ClienteXCollaboratore As ClienteXCollaboratore
            Get
                If (Me.m_ClienteXCollaboratore Is Nothing) Then
                    If (Me.m_Cliente IsNot Nothing AndAlso Me.m_Collaboratore IsNot Nothing) Then
                        Me.m_ClienteXCollaboratore = Finanziaria.Collaboratori.ClientiXCollaboratori.GetItemByPersonaECollaboratore(Me.m_Cliente, Me.m_Collaboratore)
                    End If
                End If
                Return Me.m_ClienteXCollaboratore
            End Get
            Set(value As ClienteXCollaboratore)
                Dim oldValue As ClienteXCollaboratore = Me.m_ClienteXCollaboratore
                If (value Is oldValue) Then Return
                Me.m_ClienteXCollaboratore = value
                Me.m_Diretta = Not Sistema.TestFlag(value.Flags, ClienteCollaboratoreFlags.AssegnatoDaAgenzia) ' value.getAssegnatoDa() === Nothing
                Me.m_Estinzioni = Nothing
                'Me.chengeEstinzione(0, False)
                Me.DoChanged("ClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        Protected Sub Inizializza()
            Me.m_W = Nothing
            Me.m_Estinzioni = Nothing
            If (Me.CollaboratoriXCliente.Count = 1) Then Me.m_ClienteXCollaboratore = Me.CollaboratoriXCliente(0)
            'Me.doCollaboratoreChanged()
        End Sub

        Protected Sub doDecorrenzaChanged()
            Me.m_Profili = Nothing
            Me.m_Prodotti = Nothing
            Me.m_Cessionari = Nothing
        End Sub



        Protected Sub doCessionarioChanged()
            Me.m_Profili = Nothing
            Me.m_Prodotti = Nothing
        End Sub

        Private Sub _aggiornaEstinzione()
            If (Me.m_Cliente IsNot Nothing OrElse Me.m_ClienteXCollaboratore IsNot Nothing) Then Return

            Dim items As CCollection(Of EstinzioneXEstintore) = Me.EstinzioniXEstintore()
            If (items.Count() = 0) Then Return
            Dim est As EstinzioneXEstintore = items(0)
            If (Me.m_Prodotto IsNot Nothing) Then
                est.Parametro = Me.m_Prodotto.IdTipoContratto
            End If
            est.Tipo = IIf(est.Parametro = "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA)
            est.DataCaricamento = Me.DataCaricamento
            est.DataEstinzione = Me.DataDecorrenza
            est.Rata = Me.m_SommaMontanteResiduo
            est.NumeroQuoteResidue = 1
            est.TotaleDaEstinguere = Me.m_SommaEstinzioni
        End Sub


        Private Sub doResiduoChanged()
            'Me.Ricalcola()
            Me.Invalidate()
        End Sub

        Private Sub doValoreEstinzioneChanged()
            'Me.Ricalcola()
            Me.Invalidate()
        End Sub


        Public ReadOnly Property CollaboratoriXCliente As CCollection(Of ClienteXCollaboratore)
            Get
                If (Me.m_CollaboratoriXCliente Is Nothing) Then
                    Me.m_CollaboratoriXCliente = New CCollection(Of ClienteXCollaboratore)
                    If (GetID(Me.m_Cliente) <> 0) Then
                        Dim col As CCollaboratore = Finanziaria.Collaboratori.GetItemByUser(Me.User)
                        Dim cursor As New ClienteXCollaboratoreCursor
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor.IgnoreRights = True
                        cursor.IDPersona.Value = GetID(Me.m_Cliente)
                        If (col IsNot Nothing) Then cursor.IDCollaboratore.Value = GetID(col)
                        While (Not cursor.EOF())
                            Dim cli As ClienteXCollaboratore = cursor.Item
                            Me.m_CollaboratoriXCliente.Add(cli)
                            cursor.MoveNext()
                        End While
                        cursor.Dispose()
                    End If
                End If
                Return Me.m_CollaboratoriXCliente
            End Get
        End Property

        Protected Friend Sub SetCliente(ByVal p As CPersonaFisica)
            Me.m_Cliente = p
            Me.m_CollaboratoriXCliente = Nothing
            Me.m_Cessionari = Nothing
            Me.m_Profili = Nothing
            Me.m_TabellaAssRel = Nothing
            Me.m_Estinzioni = Nothing

            'Me.chengeEstinzione(0, False)
        End Sub

        Public Property DataCaricamento As Date
            Get
                Return Me.m_DataCaricamento
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataCaricamento
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataCaricamento = value
                Me.m_Estinzioni = Nothing
                Me.m_Cessionari = Nothing
                Me.m_Cessionario = Nothing

                Me.DoChanged("DataCaricamento", value, oldValue)
            End Set
        End Property

        Public Sub ChangeEstinzione(ByVal i As Integer, ByVal selezionata As Boolean)
            Dim sum As Decimal = 0
            Dim sum1 As Decimal = 0
            Dim items As CCollection(Of EstinzioneXEstintore) = Me.EstinzioniXEstintore
            items(i).Selezionata = selezionata


            Dim dcar As Date = Me.DataCaricamento
            Dim ddec As Date = Me.DataDecorrenza
            Dim nomeCess As String = ""
            If (Me.Cessionario IsNot Nothing) Then nomeCess = Me.Cessionario.Nome

            For Each est As EstinzioneXEstintore In items
                If (GetID(est) <> 0) Then
                    Dim dest As Date = ddec
                    Dim IsInterno As Boolean = nomeCess <> "" AndAlso Strings.Compare(nomeCess, est.NomeCessionario, CompareMethod.Text) = 0
                    Dim resid As Integer = 0
                    If (est.DataFine.HasValue) Then resid = Math.Max(0, DateUtils.DateDiff("M", ddec, est.DataFine.Value) + 1)
                    If (Formats.ToInteger(est.Durata) > 0) Then resid = Math.Min(resid, est.Durata)
                    If (IsInterno) Then resid += 1

                    est.Parametro = ""
                    If (Me.m_Prodotto IsNot Nothing) Then est.Parametro = Me.m_Prodotto.IdTipoContratto
                    est.DataCaricamento = dcar
                    est.DataEstinzione = dest
                    est.AggiornaValori()
                    est.NumeroQuoteResidue = resid
                    est.AggiornaCalcolo()
                    If (est.Selezionata) Then
                        sum += est.MontanteResiduo
                        sum1 += est.TotaleDaRimborsare
                    End If
                End If
            Next
            Me.m_SommaMontanteResiduo = sum
            Me.m_SommaEstinzioni = sum1

            Me.Invalidate()
        End Sub

        Private Function isValid(ByVal est As CEstinzione) As Boolean
            Dim ret As Boolean
            ret = (est IsNot Nothing) AndAlso
                  (est.Stato = ObjectStatus.OBJECT_VALID) AndAlso
                  (est.Scadenza.HasValue)
            If (Not ret) Then Return ret
            'var durata = //Formats.ToInteger(est.getDurata()) > 0
            Select Case (est.Tipo)
                Case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO,
                        TipoEstinzione.ESTINZIONE_PRESTITODELEGA,
                        TipoEstinzione.ESTINZIONE_CQP
                    'case TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Private Function CalcolaResiduo(ByVal est As CEstinzione) As Decimal
            Dim nomeCess As String = ""
            If (Me.Cessionario IsNot Nothing) Then nomeCess = Me.Cessionario.Nome
            Dim tipoContratto As String = ""
            If (Me.m_Prodotto IsNot Nothing) Then tipoContratto = Me.m_Prodotto.IdTipoContratto
            If (tipoContratto = "") Then tipoContratto = "C"
            Dim tp As TipoEstinzione = IIf(tipoContratto = "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA)
            Dim dec As Date = Me.DataDecorrenza
            If (Not isValid(est)) Then Return 0
            Dim nmCess As String = est.NomeIstituto
            Dim nRate As Integer = Math.Max(0, 1 + DateUtils.DateDiff("M", dec, est.Scadenza.Value))
            Dim estTP As TipoEstinzione = est.Tipo
            If (estTP = TipoEstinzione.ESTINZIONE_CQP OrElse estTP = TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO) Then
                estTP = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO
            End If
            If (estTP <> tp) Then Return 0

            If ((tp <> TipoEstinzione.ESTINZIONE_NO) AndAlso (estTP = tp) AndAlso nomeCess <> "" AndAlso Strings.Compare(nomeCess, nmCess) = 0) Then
                Dim DeltaML_AggiuntiRate As Integer = Formats.ToInteger(Finanziaria.Configuration().GetValueInt("DeltaML_AggiuntiRate", 0))
                nRate = nRate + DeltaML_AggiuntiRate
            End If

            Dim calculator As New CEstinzioneCalculator()
            calculator.Rata = Formats.ToValuta(est.Rata)
            calculator.Durata = Formats.ToInteger(est.Durata)
            calculator.TAN = Formats.ToDouble(est.TAN)
            calculator.PenaleEstinzione = Formats.ToDouble(est.PenaleEstinzione)
            calculator.NumeroRateResidue = nRate
            Dim res As Decimal = calculator.DebitoResiduo

            Return Math.Max(0, res)
        End Function

        Protected Friend Sub RimuoviCliente()
            Me.SetCliente(Nothing)
        End Sub

        Protected Friend Sub doSave()
            If (Me.m_Cliente Is Nothing) Then
                Me._doSaveCollab()
            Else
                Me._doSaveCiente()
            End If
        End Sub

        Protected Friend Sub _doSaveCiente()
            'If (Calendar.Compare(Calendar.ToDay(), Me.txtDecorrenza.getValue()) >= 0)Then Throw "Decorrenza non valida!"
            Dim cu As CUser = Me.User
            Dim p As CPersonaFisica = Me.m_Cliente
            If (p Is Nothing) Then Throw New ArgumentNullException("Cliente non selezionato")
            Dim po As CUfficio = p.PuntoOperativo
            Dim w As FinestraLavorazione = Me.FinestraCorrente
            If (w Is Nothing) Then Throw New ArgumentNullException("Cliente non lavorabile (finestra di lavorazione nulla)")
            Dim ric As CRichiestaFinanziamento = w.RichiestaFinanziamento
            If (ric Is Nothing) Then
                ric = New CRichiestaFinanziamento
                ric.Cliente = p
                ric.Data = DateUtils.Now()
                ric.AssegnatoA = cu
                ric.PresaInCaricoDa = cu
                ric.PuntoOperativo = po
                ric.StatoRichiesta = StatoRichiestaFinanziamento.EVASA
                ric.Stato = ObjectStatus.OBJECT_VALID
                ric.FinestraLavorazione = w
                ric.Note = "Richiesta creata automaticamente nel Preventivatore 2018"
                ric.Save()
            End If

            w.RichiestaFinanziamento = ric
            Dim off As COffertaCQS = Me.m_Offerta
            off.Cliente = p
            'off.setPuntoOperativo(po)
            off.Stato = ObjectStatus.OBJECT_VALID
            ' off.setFinestraCorrente(w)
            off.Save()

            Dim cons As New CQSPDConsulenza()
            If (Me.m_Prodotto.IdTipoContratto() = "C") Then
                cons.OffertaCQS = off
            Else
                cons.OffertaPD = off
            End If

            Dim sf As CQSPDStudioDiFattibilita = Nothing ' (w.getStudioDiFattibilita() === Nothing) ? null : w.getStudioDiFattibilita().getStudioDiFattibilita()
            If (sf Is Nothing) Then sf = Finanziaria.StudiDiFattibilita.GetUltimoStudioDiFattibilita(p)
            If (sf Is Nothing OrElse DateUtils.Compare(DateUtils.GetDatePart(sf.Data), DateUtils.ToDay()) < 0) Then
                sf = New CQSPDStudioDiFattibilita
                sf.Cliente = p
                sf.Data = off.CreatoIl
                sf.Richiesta = ric
                sf.PuntoOperativo = p.PuntoOperativo
                sf.Consulente = Finanziaria.Consulenti.GetItemByUser(cu)
                sf.OraInizio = sf.Data
                sf.Stato = ObjectStatus.OBJECT_VALID
                sf.Save()
            End If
            sf.Proposte().Add(cons)
            cons.DataConsulenza = off.CreatoIl
            cons.InseritoDa = cu
            cons.StatoConsulenza = StatiConsulenza.INSERITA
            cons.Cliente = p
            cons.PuntoOperativo = p.PuntoOperativo
            cons.Consulente = Finanziaria.Consulenti.GetItemByUser(cu)
            cons.Descrizione = "Preventivo generato dal Preventivatore Prestitalia 2018"
            cons.FinestraLavorazione = w
            cons.Collaboratore = Me.Collaboratore
            'cons.setStato(ObjectStatus.OBJECT_VALID)
            cons.Save()
            'cons.Estinzioni().PreparaEstinzini(Me.txtDecorrenza.getValue())
            Dim sum As Decimal = 0
            Dim items As CCollection(Of EstinzioneXEstintore) = Me.EstinzioniXEstintore()
            For Each ex As EstinzioneXEstintore In items
                If (ex.Selezionata) Then
                    ex.Estintore = cons
                    ex.Parametro = Me.m_Prodotto.IdTipoContratto
                    'ex.setDataEstinzione(Me.txtDecorrenza.getValue())
                    'ex.AggiornaValori()
                    cons.Estinzioni().Add(ex)
                    ex.Save(True)
                    sum += ex.TotaleDaRimborsare
                End If
            Next
            cons.NettoRicavo = off.NettoRicavo
            cons.SommaEstinzioni = sum
            cons.SommaTrattenuteVolontarie = 0
            cons.SommaPignoramenti = 0
            cons.Save()

            w.StudioDiFattibilita = cons
            w.Save()

        End Sub

        Private Sub _doSaveCollab()
            'If (Calendar.Compare(Calendar.ToDay(), Me.txtDecorrenza.getValue()) >= 0)Then Throw "Decorrenza non valida!"
            Dim cu As CUser = Me.User
            Dim p As ClienteXCollaboratore = Me.m_ClienteXCollaboratore
            If (p Is Nothing) Then Throw New ArgumentNullException("Cliente non selezionato")
            Dim off As COffertaCQS = Me.m_Offerta
            off.ClienteXCollaboratore = p
            off.Cliente = p.Persona
            'if (p.getPersona() !== null) off.setPuntoOperativo(p.getPersona().getPuntoOperativo())
            off.Stato = ObjectStatus.OBJECT_VALID
            off.Save()

            'var i, exi, prov, 
            Dim items As CCollection(Of EstinzioneXEstintore) = Me.EstinzioniXEstintore
            For Each exi As EstinzioneXEstintore In items
                exi.Estintore = off
                'exi.AggiornaValori()
                exi.Save(True)
            Next

            For Each prov As CCQSPDProvvigioneXOfferta In off.Provvigioni
                prov.Save(True)
            Next

        End Sub

        Private ReadOnly Property NomeCessionario As String
            Get
                If (Me.Cessionario Is Nothing) Then
                    Return ""
                Else
                    Return Me.Cessionario.Nome
                End If
            End Get
        End Property

        Private ReadOnly Property Parametro As String
            Get
                If (Me.Prodotto Is Nothing) Then Return ""
                Return Me.Prodotto.IdTipoContratto
            End Get
        End Property

        Public ReadOnly Property EstinzioniXEstintore As CCollection(Of EstinzioneXEstintore)
            Get
                'var i, items, est, ex
                Dim items As CCollection(Of CEstinzione)
                Dim ddec As Date = Me.DataDecorrenza
                Dim dcar As Date = Me.DataCaricamento
                Dim dest As Date = ddec
                Dim nomeCess As String = Me.NomeCessionario
                Dim ex As EstinzioneXEstintore

                If (Me.m_Estinzioni Is Nothing) Then
                    If (Me.m_Cliente IsNot Nothing) Then
                        items = Finanziaria.Estinzioni.GetEstinzioniByPersona(Me.m_Cliente)
                    ElseIf (Me.m_ClienteXCollaboratore IsNot Nothing) Then
                        items = New CCollection(Of CEstinzione)
                        Dim cursor As New CEstinzioniCursor
                        cursor.IDClienteXCollaboratore.Value = GetID(Me.m_ClienteXCollaboratore)
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor.IgnoreRights = True
                        While (Not cursor.EOF())
                            items.Add(cursor.Item)
                            cursor.MoveNext()
                        End While
                        cursor.Dispose()
                    Else
                        Me.m_Estinzioni = New CCollection(Of EstinzioneXEstintore)
                        ex = New EstinzioneXEstintore()
                        ex.Selezionata = True
                        ex.Stato = ObjectStatus.OBJECT_VALID
                        ex.Parametro = Me.Parametro
                        ex.Tipo = IIf(ex.Parametro = "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA)
                        ex.DataEstinzione = dest
                        ex.Rata = 0
                        ex.NumeroQuoteResidue = 1
                        ex.TotaleDaEstinguere = 0
                        Me.m_Estinzioni.Add(ex)
                        Return Me.m_Estinzioni
                    End If

                    Me.m_Estinzioni = New CCollection(Of EstinzioneXEstintore)
                    For Each est As CEstinzione In items
                        If (est.Stato = ObjectStatus.OBJECT_VALID AndAlso Not est.Estinta AndAlso est.IsInCorso(dcar)) Then
                            dest = ddec ' Calendar.Max(est.getDataEstinzione(), ddec)
                            Dim IsInterno As Boolean = Strings.Compare(nomeCess, est.NomeIstituto, CompareMethod.Text) = 0
                            If (est.Scadenza.HasValue = False AndAlso est.DataInizio.HasValue AndAlso Formats.ToInteger(est.Durata) > 0) Then
                                est.Scadenza = DateUtils.GetLastMonthDay(DateUtils.DateAdd("M", est.Durata.Value, est.DataInizio.Value))
                            End If
                            Dim resid As Integer = 0
                            If (est.Scadenza.HasValue) Then resid = Math.Max(0, DateUtils.DateDiff("M", ddec, est.Scadenza.Value) + 1)
                            If (Formats.ToInteger(est.Durata) > 0) Then resid = Math.Min(resid, est.Durata)
                            If (IsInterno) Then resid += 1
                            ex = New EstinzioneXEstintore
                            ex.Estinzione = est
                            ex.Estintore = Nothing
                            ex.Parametro = Me.Parametro
                            ex.Stato = ObjectStatus.OBJECT_VALID
                            ex.DataCaricamento = dcar ' //(IsInterno)? dcar : Calendar.DateAdd("M", -1, ddec))
                            ex.DataEstinzione = dest
                            ex.AggiornaCalcolo()
                            Me.m_Estinzioni.Add(ex)
                            ex.Save()
                        End If
                    Next
                End If
                Return Me.m_Estinzioni
            End Get
        End Property

        Private Function CalcolaProvvigioneCollaboratore(ByVal o As COffertaCQS) As Decimal
            Dim ret As Decimal = 0
            For Each item As CCQSPDProvvigioneXOfferta In o.Provvigioni
                If (item.PagataA = CQSPDTipoSoggetto.Collaboratore) Then
                    ret += Formats.ToDouble(item.Valore, 0.0)
                End If
            Next
            Return ret
        End Function

        Public Property Profilo As CProfilo
            Get
                If (Me.m_Profilo Is Nothing AndAlso Me.Profili.Count = 1) Then
                    Me.m_Profilo = Me.Profili(0)
                End If
                Return Me.m_Profilo
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.m_Profilo
                If (oldValue Is value) Then Return
                Me.m_Profilo = value
                Me.DoChanged("Profilo", value, oldValue)
            End Set
        End Property

        Public Property Prodotto As CCQSPDProdotto
            Get
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.m_Prodotto
                If (oldValue Is value) Then Return
                Me.m_Prodotto = value
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        Public Property Rata As Decimal
            Get
                Return Me.m_Rata
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Rata
                If (oldValue = value) Then Return
                Me.m_Rata = value
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        Public Property TAN As Double
            Get
                Me.Validate()
                Return Me.m_TAN
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAN
                If (oldValue = value) Then Return
                Me.m_TAN = value
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

        Public Property TAEG As Double
            Get
                Me.Validate()
                Return Me.m_TAEG
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAEG
                If (oldValue = value) Then Return
                Me.m_TAEG = value
                Me.DoChanged("TAEG", value, oldValue)
            End Set
        End Property

        Public Property ValoreProvvigioneMassima As Decimal
            Get
                Me.Validate()
                Return Me.m_ProvvMax
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_ProvvMax
                If (oldValue = value) Then Return
                Me.m_ProvvMax = value
                Me.DoChanged("ValoreProvvigioneMassima", value, oldValue)
            End Set
        End Property

        Public Property Commissioni As Decimal
            Get
                Return Me.m_Commissioni
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Commissioni
                If (oldValue = value) Then Return
                Me.m_Commissioni = value
                Me.DoChanged("Commissioni", value, oldValue)
            End Set
        End Property

        Public Property CapitaleFinanziato As Decimal
            Get
                Me.Validate()
                Return Me.m_CapitaleFinanziato
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_CapitaleFinanziato
                If (oldValue = value) Then Return
                Me.m_CapitaleFinanziato = value
                Me.DoChanged("CapitaleFinanziato", value, oldValue)
            End Set
        End Property

        Public Property NettoErogato As Decimal
            Get
                Me.Validate()
                Return Me.m_NettoErogato
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_NettoErogato
                If (oldValue = value) Then Return
                Me.m_NettoErogato = value
                Me.DoChanged("NettoErogato", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property NettoAllaMano As Decimal
            Get
                Return Me.NettoErogato - Me.SommaEstinzioni
            End Get
        End Property

        Public Property OneriErariali As Decimal
            Get
                Me.Validate()
                Return Me.m_OneriErariali
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_OneriErariali
                If (oldValue = value) Then Return
                Me.m_OneriErariali = value
                Me.DoChanged("OneriErariali", value, oldValue)
            End Set
        End Property

        Public Property Spese As Decimal
            Get
                Return Me.m_Spese
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Spese
                If (oldValue = value) Then Return
                Me.m_Spese = value
                Me.DoChanged("Spese", value, oldValue)
            End Set
        End Property

        Public Property ValoreProvvigioneTAN As Decimal
            Get
                Me.Validate()
                Return Me.m_ProvvTAN
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_ProvvTAN
                If (oldValue = value) Then Return
                Me.m_ProvvTAN = value
                Me.DoChanged("ValoreProvvigioneTAN", value, oldValue)
            End Set
        End Property

        Private Function PrepareOfferta() As COffertaCQS
            Dim o As New COffertaCQS
            o.DataCaricamento = Me.DataCaricamento
            o.DataDecorrenza = Me.DataDecorrenza
            o.OffertaLibera = True
            o.Cessionario = Me.Cessionario
            o.Profilo = Me.Profilo
            o.Prodotto = Me.Prodotto
            o.Rata = Me.Rata
            o.Durata = Me.Durata
            o.TAN = Me.TAN
            o.TAEG = Me.TAEG
            o.ValoreProvvigioneMassima = Me.ValoreProvvigioneMassima
            o.ValoreSpread = Me.Commissioni
            o.ValoreUpFront = Me.Commissioni
            o.ValoreRiduzioneProvvigionale = Me.ValoreSconto
            o.PremioDaCessionario = 0
            o.PremioDaCessionario1 = 0
            o.CapitaleFinanziato = Me.CapitaleFinanziato
            o.NettoRicavo = Me.NettoErogato
            o.OneriErariali = Me.OneriErariali
            o.TabellaFinanziariaRel = Me.TabellaFinanziariaRel
            o.TabellaAssicurativaRel = Me.TabellaAssicurativaRel
            o.TabellaSpese = Me.TabellaSpeseRel
            o.Imposte = Me.Spese
            o.ValoreProvvTAN = Me.ValoreProvvigioneTAN
            'Me.m_ClienteXCollaboratore = Finanziaria.Collaboratori.ClientiXCollaboratori().GetItemById(colID)
            Dim col As CCollaboratore = Nothing
            If (Me.ClienteXCollaboratore IsNot Nothing) Then col = Me.ClienteXCollaboratore.Collaboratore
            o.Collaboratore = col
            o.ClienteXCollaboratore = Me.ClienteXCollaboratore
            o.Flags = Sistema.SetFlag(o.Flags, OffertaFlags.DirettaCollaboratore, Me.m_Diretta)
            o.PuntoOperativo = Nothing
            If (Me.m_Cliente IsNot Nothing) Then o.PuntoOperativo = Me.m_Cliente.PuntoOperativo
            o.Save()

            Dim estinzioni As CCollection(Of EstinzioneXEstintore) = Me.EstinzioniXEstintore
            o.SincronizzaProvvigioni(estinzioni)
            For Each provv1 As CCQSPDProvvigioneXOfferta In o.Provvigioni
                provv1.Aggiorna(o, estinzioni)
                Select Case (provv1.PagataDa())
                    Case CQSPDTipoSoggetto.Cessionario
                    Case CQSPDTipoSoggetto.Agenzia
                        Select Case (provv1.PagataA)
                            Case CQSPDTipoSoggetto.Cessionario
                            Case CQSPDTipoSoggetto.Agenzia
                            Case CQSPDTipoSoggetto.Collaboratore
                            Case CQSPDTipoSoggetto.Cliente
                        End Select
                    Case CQSPDTipoSoggetto.Collaboratore
                    Case CQSPDTipoSoggetto.Cliente
                        Select Case (provv1.PagataA)
                            Case CQSPDTipoSoggetto.Cessionario
                            Case CQSPDTipoSoggetto.Agenzia
                            Case CQSPDTipoSoggetto.Collaboratore
                                Me.m_NettoErogato -= provv1.Valore
                                o.NettoRicavo = Me.m_NettoErogato
                                Dim c As New CTAEGCalculator
                                c.Rata = o.Rata
                                c.Durata = o.Durata
                                c.NettoRicavo = Me.m_NettoErogato
                                Me.m_TAEG = c.Calc()
                                o.TAEG = Me.m_TAEG

                            Case CQSPDTipoSoggetto.Cliente

                        End Select
                End Select
            Next

            'Me.ctrlProvv.setItem(o)
            'Me.ctrlProvv.setEstinzioni(Me.m_Estinzioni)

            Return o
        End Function

        Private Function CheckExiType(ByVal exi As EstinzioneXEstintore) As Boolean
            If (exi.Tipo = TipoEstinzione.ESTINZIONE_CQP OrElse exi.Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO) Then
                Return Me.Parametro = "C"
            ElseIf (exi.Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA) Then
                Return Me.Parametro = "D"
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Restituisce o imposta la somma dei montanti residui da restituire
        ''' </summary>
        ''' <returns></returns>
        Public Property SommaMontantiResidui As Decimal
            Get
                If (Me.m_Cliente Is Nothing AndAlso Me.m_ClienteXCollaboratore Is Nothing) Then
                    Return Me.m_SommaMontanteResiduo
                Else
                    Dim ret As Decimal = 0
                    For Each exi As EstinzioneXEstintore In Me.EstinzioniXEstintore
                        If (exi.Selezionata AndAlso exi.MontanteResiduo.HasValue AndAlso Me.CheckExiType(exi)) Then
                            ret += exi.MontanteResiduo
                        End If
                    Next
                    Return ret
                End If
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaMontanteResiduo
                If (oldValue = value) Then Return
                Me.m_SommaMontanteResiduo = value
                Me.DoChanged("SommaMontanteResiduo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la somma dei prestiti da rimborsare
        ''' </summary>
        ''' <returns></returns>
        Public Property SommaEstinzioni As Decimal
            Get
                If (Me.m_Cliente Is Nothing AndAlso Me.m_ClienteXCollaboratore Is Nothing) Then
                    Return Me.m_SommaEstinzioni
                Else
                    Dim ret As Decimal = 0
                    For Each exi As EstinzioneXEstintore In Me.EstinzioniXEstintore
                        If (exi.Selezionata) Then
                            ret += exi.TotaleDaRimborsare
                        End If
                    Next
                    Return ret
                End If
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaEstinzioni
                If (oldValue = value) Then Return
                Me.m_SommaEstinzioni = value
                Me.DoChanged("SommaEstinzioni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale di sconto applicata
        ''' </summary>
        ''' <returns></returns>
        Public Property Sconto As Double
            Get
                Return Me.m_Sconto
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Sconto
                If (oldValue = value) Then Return
                Me.m_Sconto = value
                Me.DoChanged("Sconto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il montante lordo dell'operazione (rata * durata)
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property MontanteLordo As Decimal
            Get
                Return Me.Rata * Me.Durata
            End Get
        End Property

        Public ReadOnly Property CommissioniTotaliAgenzia As Decimal
            Get
                Me.Validate()
                Return Me.m_CommissioniTotaliAgenzia
            End Get
        End Property

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub

        Public Sub Validate()
            If (Me.m_Calculated) Then Return
            Me.Ricalcola()
        End Sub

        Private Sub Ricalcola()
            'Me.m_ClienteXCollaboratore = Finanziaria.Collaboratori.ClientiXCollaboratori.GetItemById(Me.m_)
            'Me.m_Diretta = True
            Me.m_Calculated = True

            If (Me.ClienteXCollaboratore IsNot Nothing) Then
                Me.m_Diretta = Not Sistema.TestFlag(Me.m_ClienteXCollaboratore.Flags, ClienteCollaboratoreFlags.AssegnatoDaAgenzia)
            End If

            Dim cess As CCQSPDCessionarioClass = Me.Cessionario
            Dim prof As CProfilo = Me.Profilo
            Dim p As CCQSPDProdotto = Me.Prodotto
            Dim relFin As CProdottoXTabellaFin = Me.TabellaFinanziariaRel
            Dim relAss As CProdottoXTabellaAss = Me.TabellaAssicurativaRel
            Dim relSpe As CProdottoXTabellaSpesa = Me.TabellaSpeseRel

            Dim decorrenza As Date = Me.DataDecorrenza

            Dim tabella As CTabellaFinanziaria = Nothing

            If (relFin IsNot Nothing) Then tabella = relFin.Tabella

            Dim TAN As Double = 0
            If (tabella IsNot Nothing AndAlso Not tabella.TANVariabile) Then
                TAN = tabella.TAN
            End If

            Dim pmax As Double = 0
            If (tabella IsNot Nothing) Then
                pmax = tabella.ProvvMax
            End If

            Dim upfrontmax As Decimal = 0
            If (tabella IsNot Nothing) Then
                upfrontmax = tabella.UpFrontMax
            End If

            Dim rata As Decimal = Me.Rata
            If (rata < 0) Then Throw New ArgumentOutOfRangeException("Rata non valida")

            Dim durata As Integer = Me.Durata
            If (durata < 12 OrElse durata > 120) Then Throw New ArgumentOutOfRangeException("Durata non valida")

            'Sincronizziamo il valore residuo (per nessun cliente selezionato)
            Me._aggiornaEstinzione()

            Dim residuo As Decimal = Me.SommaMontantiResidui
            Dim daEstinguere As Decimal = Me.SommaEstinzioni


            Dim ml As Decimal = rata * durata
            Dim cf As Decimal = 0
            If (rata > 0 AndAlso durata > 0 AndAlso TAN > 0) Then
                Dim ti As New CTANInverter
                ti.Rata = rata
                ti.Durata = durata
                ti.TAN = TAN
                cf = ti.Calc()
            End If

            Dim interessi As Decimal = ml - cf
            'var residuo = Formats.ParseValuta(GetFieldValue(Me.getCUID() + "estinzione"))
            If (residuo < 0) Then Throw New ArgumentOutOfRangeException("Montante residuo non valido")

            Dim sconto As Double = Me.Sconto
            If (sconto < 0 OrElse sconto > pmax) Then Throw New ArgumentNullException("Riduzione provvigionale non valida")

            Dim commissioni As Decimal = Math.Max(0, (pmax - sconto) * (ml - residuo) / 100)

            Me.m_ValoreSconto = 0
            If (upfrontmax > 0) Then
                commissioni = Math.Min(upfrontmax, (pmax - sconto) * (ml - residuo) / 100)
                Me.m_ValoreSconto = IIf((ml - residuo) > 0, sconto * (ml - residuo) / 100, 0)
            End If

            Dim pTAN As Double = 0
            Dim baseML As Decimal = ml
            If (tabella IsNot Nothing) Then
                If (residuo = 0) Then
                    pTAN = tabella.Sconto
                Else
                    pTAN = tabella.ProvvTANR
                End If
                If (tabella.TipoCalcoloProvvTAN = TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI) Then
                    baseML = ml - residuo
                End If
            End If

            Dim provvagee As Decimal = commissioni + baseML * pTAN / 100
            Dim provvage As Decimal = IIf(baseML > 0, provvagee * 100 / baseML, 0)

            Dim tabellaSpese As CTabellaSpese = Nothing
            If (Me.TabellaSpeseRel IsNot Nothing) Then
                tabellaSpese = Me.m_TabellaSpeseRel.TabellaSpese
            End If

            Dim spese As Decimal = 0
            If (tabella IsNot Nothing) Then
                spese = tabellaSpese.SpeseFisse(durata)
            End If

            Dim oneri As Decimal = 0
            If (tabella IsNot Nothing) Then
                oneri = tabellaSpese.Rivalsa(durata)
            End If

            Dim nettoErogato As Decimal = Math.Max(0, cf - commissioni - spese)

            Dim TAEG As Double = 0
            If (rata > 0 AndAlso durata > 0 AndAlso nettoErogato > 0) Then
                Dim c As New CTAEGCalculator
                c.Rata = rata
                c.Durata = durata
                c.NettoRicavo = nettoErogato
                TAEG = c.Calc()
            End If

            '***
            Dim nettoAllaMano As Decimal = nettoErogato
            'Me.m_DaEstinguere = 0
            Dim estinzioni As CCollection(Of EstinzioneXEstintore) = Me.EstinzioniXEstintore

            nettoAllaMano = nettoErogato - Me.SommaEstinzioni

            Me.m_OneriErariali = oneri
            Me.m_TAN = TAN
            Me.m_TAEG = TAEG
            Me.m_Rata = rata
            Me.m_Durata = durata
            Me.m_CapitaleFinanziato = cf
            Me.m_Interessi = interessi
            Me.m_NettoErogato = nettoErogato
            Me.m_NettoAllaMano = nettoAllaMano
            Me.m_ProvvMax = Math.Max(0, pmax * (ml - residuo) / 100)
            Me.m_Commissioni = commissioni
            Me.m_ProvvTAN = provvagee - commissioni
            Me.m_Spese = spese
            Me.m_CommissioniTotaliAgenzia = provvagee

            Dim o As COffertaCQS = Me.PrepareOfferta()

            Me.m_NettoAllaMano = Me.m_NettoErogato - Me.SommaEstinzioni
            nettoErogato = Me.m_NettoErogato
            nettoAllaMano = Me.m_NettoAllaMano
            TAEG = Me.m_TAEG

            Me.m_Offerta = o

            If (Me.m_Collaboratore IsNot Nothing) Then
                Dim scontoe As Decimal = sconto * (ml - residuo) / 100
                Dim provvcoll As Decimal = Me.CalcolaProvvigioneCollaboratore(o) - scontoe
            End If


        End Sub

        Public Function GetCollaboratoreByName(ByVal nome As String) As CCollaboratore
            nome = Strings.Trim(nome)
            For Each c As CCollaboratore In Me.GetCollaboratori(Me.DataCaricamento)
                If (Strings.Compare(c.NomePersona, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectCollaboratore(ByVal nome As String)
            Me.Collaboratore = Me.GetCollaboratoreByName(nome)
        End Sub

        Public Function GetCessionarioByName(ByVal nome As String) As CCQSPDCessionarioClass
            nome = Strings.Trim(nome)
            For Each c As CCQSPDCessionarioClass In Me.Cessionari
                If (Strings.Compare(c.Nome, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectCessionario(ByVal nome As String)
            Me.Cessionario = Me.GetCessionarioByName(nome)
        End Sub

        Public Function GetProfiloByName(ByVal nome As String) As CProfilo
            nome = Strings.Trim(nome)
            For Each c As CProfilo In Me.Profili
                If (Strings.Compare(c.Nome, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectProfilo(ByVal nome As String)
            Me.Profilo = Me.GetProfiloByName(nome)
        End Sub

        Public Function GetProdottoByName(ByVal nome As String) As CCQSPDProdotto
            nome = Strings.Trim(nome)
            For Each c As CCQSPDProdotto In Me.Prodotti
                If (Strings.Compare(c.Nome, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectProdotto(ByVal nome As String)
            Me.Prodotto = Me.GetProdottoByName(nome)
        End Sub

        Public Function GetTabellaFinanziariaByName(ByVal nome As String) As CProdottoXTabellaFin
            nome = Strings.Trim(nome)
            For Each c As CProdottoXTabellaFin In Me.TabelleFinanziarie
                If (c.Tabella IsNot Nothing AndAlso Strings.Compare(c.Tabella.Nome, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectTabellaFinanziaria(ByVal nome As String)
            Me.TabellaFinanziariaRel = Me.GetTabellaFinanziariaByName(nome)
        End Sub

        Public Function GetTabellaAssicurativaByName(ByVal nome As String) As CProdottoXTabellaAss
            nome = Strings.Trim(nome)
            For Each c As CProdottoXTabellaAss In Me.TabelleAssicurative
                If (Strings.Compare(c.Descrizione, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectTabellaAssicurativa(ByVal nome As String)
            Me.TabellaAssicurativaRel = Me.GetTabellaAssicurativaByName(nome)
        End Sub

        Public Function GetTabellaSpeseByName(ByVal nome As String) As CProdottoXTabellaSpesa
            nome = Strings.Trim(nome)
            For Each c As CProdottoXTabellaSpesa In Me.TabelleSpese
                If (Strings.Compare(c.Nome, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Public Sub SelectTabellaSpese(ByVal nome As String)
            Me.TabellaSpeseRel = Me.GetTabellaSpeseByName(nome)
        End Sub

    End Class


End Class