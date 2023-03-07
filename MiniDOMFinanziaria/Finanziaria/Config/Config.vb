Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Finanziaria

Namespace Internals



    <Flags>
    Public Enum CQSPDConfigFlags As Integer
        None = 0

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le richiesta
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITRICHIESTA = 1

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITPROPOSTA = 2

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le pratiche
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITPRATICA = 4

        ''' <summary>
        ''' Consenti agli utenti che non dispongono del diritto change_status di modificare gli altri prestiti
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSRE_ALLOWEDITALTRIPRESTITI = 8

        ''' <summary>
        ''' Consente la correzione della decorrenza delle proposte
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITDECORRENZAPROP = 16

        ''' <summary>
        ''' Indica al sistema che deve essere effettuata l'archiviazione automatica delle pratiche liquidate dopo N giorni
        ''' </summary>
        ''' <remarks></remarks>
        ARCHIVIA_AUTOMATICAMENTE = 32

        ''' <summary>
        ''' Indica al sistema di utilizzare l'interfaccia multicessionario
        ''' </summary>
        ''' <remarks></remarks>
        SISTEMA_MULTICESSIONARIO = 64

        ''' <summary>
        ''' Consente di proporre al cliente degli studi di fattibilità bypassando il controllo del supervisore.
        ''' La proposta sarà comunque inviata al supervisore se lo studio di fattibilità viene convertito in pratica.
        ''' </summary>
        ''' <remarks></remarks>
        CONSENTI_PROPOSTE_NONAPPROVATE = 256

    End Enum

    ''' <summary>
    ''' Configurazione del modulo CreditoV
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCQSPDConfig

        Public Sub DoChanged(ByVal paramName As String, ByVal newValue As Object, ByVal oldValue As Object)
            Throw New NotImplementedException()
        End Sub

        Public Function GetValueDate(ByVal paramName As String, ByVal defValue As Date?) As Date?
            Return Finanziaria.Module.Settings.GetValueDate(paramName, defValue)
        End Function

        Public Sub SetValueDate(ByVal paramName As String, ByVal value As Date?)
            Finanziaria.Module.Settings.SetValueDate(paramName, value)
        End Sub

        Public Function GetValueInt(ByVal paramName As String, ByVal defValue As Integer?) As Integer?
            Return Finanziaria.Module.Settings.GetValueInt(paramName, defValue)
        End Function

        Public Sub SetValueInt(ByVal paramName As String, ByVal value As Integer?)
            Finanziaria.Module.Settings.SetValueInt(paramName, value)
        End Sub

        Public Function GetValueDouble(ByVal paramName As String, ByVal defValue As Integer?) As Double?
            Return Finanziaria.Module.Settings.GetValueDouble(paramName, defValue)
        End Function

        Public Sub SetValueDouble(ByVal paramName As String, ByVal value As Double?)
            Finanziaria.Module.Settings.SetValueDouble(paramName, value)
        End Sub

        Public Function GetValueString(ByVal paramName As String, ByVal defValue As String) As String
            Return Finanziaria.Module.Settings.GetValueString(paramName, defValue)
        End Function

        Public Sub SetValueString(ByVal paramName As String, ByVal value As String)
            Finanziaria.Module.Settings.SetValueString(paramName, value)
        End Sub


        Public Sub New()

        End Sub


        Public Property ConsentiProposteSenzaSupervisore As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.CONSENTI_PROPOSTE_NONAPPROVATE)
            End Get
            Set(value As Boolean)
                If (Me.ConsentiProposteSenzaSupervisore = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.CONSENTI_PROPOSTE_NONAPPROVATE, value)
                Me.DoChanged("ConentiProposteSenzaSupervisore", value, Not value)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica al sistema se deve essere utilizzata l'interfaccia per i sistema multicessionari
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SistemaMulticessionario As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.SISTEMA_MULTICESSIONARIO)
            End Get
            Set(value As Boolean)
                If (Me.SistemaMulticessionario = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.SISTEMA_MULTICESSIONARIO, value)
                Me.DoChanged("SistemaMulticessionario", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultima archiviazione automatica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimaArchiviazione As Date?
            Get
                Return Me.GetValueDate("UltimaArchiviazione", Nothing)
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.UltimaArchiviazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.SetValueDate("UltimaArchiviazione", value)
                Me.DoChanged("UltimaArchiviazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve archiviare automaticamente le pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ArchiviaAutomaticamente As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.ARCHIVIA_AUTOMATICAMENTE)
            End Get
            Set(value As Boolean)
                If (Me.ArchiviaAutomaticamente = value) Then Return
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.ARCHIVIA_AUTOMATICAMENTE, value)
                Me.DoChanged("ArchiviaAutomaticamente", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni dopo cui archiviare automaticamente la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiorniArchiviazione As Integer
            Get
                Return Me.GetValueInt("GiorniArchiviazione", 120)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.GiorniArchiviazione
                If (oldValue = value) Then Exit Property
                Me.SetValueInt("GiorniArchiviazione", value)
                Me.DoChanged("GiorniArchiviazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As CQSPDConfigFlags
            Get
                Return CType(Finanziaria.Module.Settings.GetValueInt("Flags", 0), CQSPDConfigFlags)
            End Get
            Set(value As CQSPDConfigFlags)
                Dim oldValue As CQSPDConfigFlags = Me.Flags
                If (oldValue = value) Then Exit Property
                Finanziaria.Module.Settings.SetValueInt("Flags", CInt(value))
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le richieste di finanziamento già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditRichieste As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITRICHIESTA)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditRichieste = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITRICHIESTA, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditProposte As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPROPOSTA)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditProposte = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPROPOSTA, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditPratiche As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPRATICA)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditPratiche = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPRATICA, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditAltriPrestiti As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSRE_ALLOWEDITALTRIPRESTITI)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditAltriPrestiti = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSRE_ALLOWEDITALTRIPRESTITI, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti normali di modificare la decorrenza delle proposte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToCorrectDecorrenzaProposte As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITDECORRENZAPROP)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToCorrectDecorrenzaProposte = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITDECORRENZAPROP, value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore minimo caricabile per l'UpFront da parte di un utente normale.
        ''' Sotto questo valore solo gli operatori con il diritto change_status possono caricare le offerte 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreMinimoUpFront As Decimal
            Get
                Return Me.GetValueDouble("ValoreMinimoUpfront", 0)
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.ValoreMinimoUpFront
                If (oldValue = value) Then Exit Property
                Me.SetValueDouble("ValoreMinimoUpfront", value)
                Me.DoChanged("ValoreMinimoUpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la soglia sotto la quale viene generata una codinzione di attenzione per la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SogliaNotificaUpFront As Decimal
            Get
                Return Me.GetValueDouble("SogliaNotificaUpFront", 0)
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.SogliaNotificaUpFront
                If (oldValue = value) Then Exit Property
                Me.SetValueDouble("SogliaNotificaUpFront", value)
                Me.DoChanged("SogliaNotificaUpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni da sottrarre alla data in cui un prestito diventa rifinanziabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiorniAnticipoRifin As Integer
            Get
                Return Me.GetValueInt("GiorniAnticipoRifin", 120)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.GiorniAnticipoRifin '
                If (oldValue = value) Then Exit Property
                Me.SetValueInt("GiorniAnticipoRifin", value)
                Me.DoChanged("GiorniAnticipoRifin", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imosta il tempo (in percentuale rispetto alla durata) in cui un prestito diventa rifinanziabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PercCompletamentoRifn As Double
            Get
                Return Me.GetValueDouble("PercCompletamentoRifn", 40)
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.PercCompletamentoRifn
                If (oldValue = value) Then Exit Property
                Me.SetValueDouble("PercCompletamentoRifn", value)
                Me.DoChanged("PercCompletamentoRifn", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato predefinito (quando viene caricata una nuova pratica)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStatoPredefinito As Integer
            Get
                Return Me.GetValueInt("IDStatoPredefinito", 0)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoPredefinito
                If (oldValue = value) Then Exit Property '                If (GetID(Me.m_StatoPredefinito) = value) Then Exit Property
                Me.SetValueInt("IDStatoPredefinito", value)
                Me.DoChanged("IDStatoPredefinito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituiscec o imposta lo stato predefinito per le nuove pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoPredefinito As CStatoPratica
            Get
                Return minidom.Finanziaria.StatiPratica.GetItemById(Me.IDStatoPredefinito)
            End Get
            Set(value As CStatoPratica)
                Me.IDStatoPredefinito = DBUtils.GetID(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo da cui vengono inviate le email provenienti da questo modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BCC As String
            Get
                Return Me.GetValueString("BCC", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.BCC
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("BCC", value)
                Me.DoChanged("BCC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo da cui vengono inviate le email provenienti da questo modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromAddress As String
            Get
                Return Me.GetValueString("FromAddress", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.FromAddress
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("FromAddress", value)
                Me.DoChanged("FromAddress", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome visualizzato come mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromDisplayName As String
            Get
                Return Me.GetValueString("FromDisplayName", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.FromDisplayName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("FromDisplayName", value)
                Me.DoChanged("FromDisplayName", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Elenco di indirizzi separati da "," a cui inviare gli avvisi generati per le pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyChangesTo As String
            Get
                Return Me.GetValueString("NotifyChangesTo", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.NotifyChangesTo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("NotifyChangesTo", value)
                Me.DoChanged("NotifyChangesTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail che viene inviata per notificare le condizioni di avviso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WatchSubject As String
            Get
                Return Me.GetValueString("WatchSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.WatchSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("WatchSubject", value)
                Me.DoChanged("WatchSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modello utilizzato per le email inviate per notificare delle condizioni di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WatchTemplate As String
            Get
                Return Me.GetValueString("WatchTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.WatchTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("WatchTemplate", value)
                Me.DoChanged("WatchTemplat", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'inserimento di una nuova richiesta di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaFinanziamentoSubject As String
            Get
                Return Me.GetValueString("RichiestaFinanziamentoSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.RichiestaFinanziamentoSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("RichiestaFinanziamentoSubject", value)
                Me.DoChanged("RichiestaFinanziamentoSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail utilizzata per notificare l'inserimento di una nuova richiesta di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaFinanziamentoTemplate As String
            Get
                Return Me.GetValueString("RichiestaFinanziamentoTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.RichiestaFinanziamentoTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("RichiestaFinanziamentoTemplate", value)
                Me.DoChanged("RichiestaFinanziamentoTemplate", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail che viene inviata per notificare l'eliminazione di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteSubject As String
            Get
                Return Me.GetValueString("DeleteSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.DeleteSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("DeleteSubject", value)
                Me.DoChanged("DeleteSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del modello di mail utilizzato per notificare l'eliminazione di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteTemplate As String
            Get
                Return Me.GetValueString("DeleteTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.DeleteTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("DeleteTemplate", value)
                Me.DoChanged("DeleteTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi email a cui notificare le modifiche fatte alle pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyWarningsTo As String
            Get
                Return Me.GetValueString("NotifyChangesTo", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.NotifyChangesTo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("NotifyChangesTo", value)
                Me.DoChanged("NotifyWarningsTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi email a cui inviare le richieste di approvazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyRichiesteApprovazioneTo As String
            Get
                Return Me.GetValueString("NotifyRichiesteApprovazioneTo", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.NotifyRichiesteApprovazioneTo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("NotifyRichiesteApprovazioneTo", value)
                Me.DoChanged("NotifyRichiesteApprovazioneTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template della mail inviata per notificare le richieste di approvazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateRichiesteApprovazione As String
            Get
                Return Me.GetValueString("TemplateRichiesteApprovazione", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.TemplateRichiesteApprovazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("TemplateRichiesteApprovazione", value)
                Me.DoChanged("TemplateRichiesteApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template della mail che viene inviata per notificare l'approvazione di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateConfermaApprovazione As String
            Get
                Return Me.GetValueString("TemplateConfermaApprovazione", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.TemplateConfermaApprovazione
                If (oldValue = value) Then Exit Property
                Me.SetValueString("TemplateConfermaApprovazione", value)
                Me.DoChanged("TemplateConfermaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template usato per le email inviate per notificare la mancata approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateNegaApprovazione As String
            Get
                Return Me.GetValueString("TemplateNegaApprovazione", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.TemplateNegaApprovazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("TemplateNegaApprovazione", value)
                Me.DoChanged("TemplateNegaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template utilizzato per le email inviate per notificare la modifica di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateChangeStatus As String
            Get
                Return Me.GetValueString("TemplateChangeStatus", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.TemplateChangeStatus
                If (oldValue = value) Then Exit Property
                Me.SetValueString("TemplateChangeStatus", value)
                Me.DoChanged("TemplateChangeStatus", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare le modifiche fatte alle pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChangeStatusSubject As String
            Get
                Return Me.GetValueString("ChangeStatusSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ChangeStatusSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ChangeStatusSubject", value)
                Me.DoChanged("ChangeStatusSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per le richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReqApprSubject As String
            Get
                Return Me.GetValueString("ReqApprSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ReqApprSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ReqApprSubject", value)
                Me.DoChanged("ReqApprSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o impsota il soggetto delle mail inviate per notificare l'approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ApprSubject As String
            Get
                Return Me.GetValueString("ApprSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ApprSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ApprSubject", value)
                Me.DoChanged("ApprSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per negare una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NegaSubject As String
            Get
                Return Me.GetValueString("NegaSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.NegaSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("NegaSubject", value)
                Me.DoChanged("NegaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare le proposte caricate sotto la soglia minima dell'UpFront
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotificaSogliaSubject As String
            Get
                Return Me.GetValueString("NotificaSogliaSubject", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.NotificaSogliaSubject
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.SetValueString("NotificaSogliaSubject", value)
                Me.DoChanged("NotificaSogliaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del templete utilizzato per le email inviate per notificare una proposta caricata sotto la soglia minima dell'UpFront
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotificaSogliaTemplate As String
            Get
                Return Me.GetValueString("NotificaSogliaTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.NotificaSogliaTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("NotificaSogliaTemplate", value)
                Me.DoChanged("NotificaSogliaTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare la creazione di una nuova pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaCreataSubject() As String
            Get
                Return Me.GetValueString("PraticaCreataSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.PraticaCreataSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("PraticaCreataSubject", value)
                Me.DoChanged("PraticaCreataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare la creazione di una nuova pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaCreataTemplate() As String
            Get
                Return Me.GetValueString("PraticaCreataTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.PraticaCreataTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("PraticaCreataSubject", value)
                Me.DoChanged("PraticaCreataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'annullamento di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaAnnullataSubject() As String
            Get
                Return Me.GetValueString("PraticaAnnullataSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.PraticaAnnullataSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("PraticaAnnullataSubject", value)
                Me.DoChanged("PraticaAnnullataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'annullamento di una nuova pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaAnnullataTemplate() As String
            Get
                Return Me.GetValueString("PraticaAnnullataTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.PraticaAnnullataTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("PraticaAnnullataSubject", value)
                Me.DoChanged("PraticaAnnullataTemplate", value, oldValue)
            End Set
        End Property

        'Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
        '    Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

        '    Me.Overflow.SetValueString("ConsulenzaRichiestaApprovazioneSubject", Me.m_ConsulenzaRichiestaApprovazioneSubject)
        '    Me.Overflow.SetValueString("ConsulenzaRichiestaApprovazioneTemplate", Me.m_ConsulenzaRichiestaApprovazioneTemplate)
        '    Me.Overflow.SetValueString("ConsulenzaConfermaApprovazioneSubject", Me.m_ConsulenzaConfermaApprovazioneSubject)
        '    Me.Overflow.SetValueString("ConsulenzaConfermaApprovazioneTemplate", Me.m_ConsulenzaConfermaApprovazioneTemplate)
        '    Me.Overflow.SetValueString("ConsulenzaNegaApprovazioneSubject", Me.m_ConsulenzaNegaApprovazioneSubject)
        '    Me.Overflow.SetValueString("ConsulenzaNegaApprovazioneTemplate", Me.m_ConsulenzaNegaApprovazioneTemplate)

        '    If (ret) Then
        '        'Finanziaria.Pratiche.Module.Settings.Save(True)
        '        SetConfiguration(Me)
        '    End If
        '    Return ret
        'End Function

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'inserimento di un nuovo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaInseritaSubject As String
            Get
                Return Me.GetValueString("ConsulenzaInseritaSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaInseritaSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaInseritaSubject", value)
                Me.DoChanged("ConsulenzaInseritaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso del modello della mail utilizzata per notificare l'inserimento di un nuovo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaInseritaTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaInseritaTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaInseritaTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaInseritaTemplate", value)
                Me.DoChanged("ConsulenzaInseritaTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata quando un cliente accetta uno studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaAccettataSubject As String
            Get
                Return Me.GetValueString("ConsulenzaAccettataSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaAccettataSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaAccettataSubject", value)
                Me.DoChanged("ConsulenzaAccettataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata quando un cliente accetta uno studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaAccettataTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaAccettataTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaAccettataTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaAccettataTemplate", value)
                Me.DoChanged("ConsulenzaAccettataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata quando un operatore boccia lo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaBocciataSubject As String
            Get
                Return Me.GetValueString("ConsulenzaBocciataSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaBocciataSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaBocciataSubject", value)
                Me.DoChanged("ConsulenzaBocciataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata quando un operatore boccia uno studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaBocciataTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaBocciataTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaBocciataTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaBocciataTemplate", value)
                Me.DoChanged("ConsulenzaBocciataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare che un operatore ha proposto uno studio di fattibilità ad un cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaPropostaSubject As String
            Get
                Return Me.GetValueString("ConsulenzaPropostaSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaPropostaSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaPropostaSubject", value)
                Me.DoChanged("ConsulenzaPropostaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare che un operatore ha proposto uno studio di fattibilità ad un cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaPropostaTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaPropostaTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaPropostaTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaPropostaTemplate", value)
                Me.DoChanged("ConsulenzaPropostaTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per noficiare che un cliente ha rifiutato uno studio di fattibilità proposto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRifiutataSubject As String
            Get
                Return Me.GetValueString("ConsulenzaRifiutataSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaRifiutataSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaRifiutataSubject", value)
                Me.DoChanged("ConsulenzaRifiutataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare che un cliente ha rifiutato uno studio di fattibilità proposto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRifiutataTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaRifiutataTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaRifiutataTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaRifiutataTemplate", value)
                Me.DoChanged("ConsulenzaRifiutataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto delle mail inviate per notificare le richieste di approvazione per una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRichiestaApprovazioneSubject As String
            Get
                Return Me.GetValueString("ConsulenzaRichiestaApprovazioneSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaRichiestaApprovazioneSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaRichiestaApprovazioneSubject", value)
                Me.DoChanged("ConsulenzaRichiestaApprovazioneSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare le richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRichiestaApprovazioneTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaRichiestaApprovazioneTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaRichiestaApprovazioneTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaRichiestaApprovazioneTemplate", value)
                Me.DoChanged("ConsulenzaRichiestaApprovazioneTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto delle mail inviate per notificare le autorizzazioni delle richieste di approvazione per una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaConfermaApprovazioneSubject As String
            Get
                Return Me.GetValueString("ConsulenzaConfermaApprovazioneSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaConfermaApprovazioneSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaConfermaApprovazioneSubject", value)
                Me.DoChanged("ConsulenzaConfermaApprovazioneSubject ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare le autorizzazioni delle  richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaConfermaApprovazioneTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaConfermaApprovazioneTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaConfermaApprovazioneTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaConfermaApprovazioneTemplate", value)
                Me.DoChanged("ConsulenzaConfermaApprovazioneTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto delle mail inviate per notificare le negazioni delle richieste di approvazione per una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaNegaApprovazioneSubject As String
            Get
                Return Me.GetValueString("ConsulenzaNegaApprovazioneSubject", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaNegaApprovazioneSubject
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaNegaApprovazioneSubject", value)
                Me.DoChanged("ConsulenzaNegaApprovazioneSubject ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare le negazioni delle  richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaNegaApprovazioneTemplate As String
            Get
                Return Me.GetValueString("ConsulenzaNegaApprovazioneTemplate", "")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ConsulenzaNegaApprovazioneTemplate
                If (oldValue = value) Then Exit Property
                Me.SetValueString("ConsulenzaNegaApprovazioneTemplate", value)
                Me.DoChanged("ConsulenzaNegaApprovazioneTemplate", value, oldValue)
            End Set
        End Property



        Public Overrides Function ToString() As String
            Return "Configurazione Modulo Credito V"
        End Function



    End Class


End Namespace



Partial Public Class Finanziaria



    Private Shared m_Config As CCQSPDConfig = Nothing

    ''' <summary>
    ''' Oggetto che contiene alcuni parametri relativi alla configurazione del modulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Configuration As CCQSPDConfig
        Get
            If m_Config Is Nothing Then m_Config = New CCQSPDConfig
            Return m_Config
        End Get
    End Property


End Class
