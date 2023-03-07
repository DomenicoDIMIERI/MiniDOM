Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria



    Public Enum StatoEsportazione As Integer
        ''' <summary>
        ''' Record non esportato
        ''' </summary>
        ''' <remarks></remarks>
        NonEsportato = 0

        ''' <summary>
        ''' Esportato verso il sistema remoto
        ''' </summary>
        ''' <remarks></remarks>
        Esportato = 1

        ''' <summary>
        ''' Importato dal sistema remoto
        ''' </summary>
        ''' <remarks></remarks>
        Importato = 2

        '''' <summary>
        '''' L'esportazione è stata
        '''' </summary>
        'Revocato = 3


        ''' <summary>
        ''' Si è verificato un errore
        ''' </summary>
        ''' <remarks></remarks>
        Errore = 255
    End Enum

    <Flags>
    Public Enum FlagsEsportazione As Integer
        None = 0


    End Enum

    Public Enum StatoConfermaEsportazione As Integer
        ''' <summary>
        ''' La richiesta è stata inviata per confronto
        ''' </summary>
        Inviato = 0

        ''' <summary>
        ''' La richiesta è stata confermata (da elaborare sul sistema remoto)
        ''' </summary>
        Confermato = 1

        ''' <summary>
        ''' La richiesta è stata revocata del sistema che l'ha inviata
        ''' </summary>
        Revocato = 2

        ''' <summary>
        ''' La richiesta è stata annullata dal sistema su cui è arrivata
        ''' </summary>
        Rifiutata = 3
    End Enum

    ''' <summary>
    ''' Rappresenta una importazione o una esportazione 
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImportExport
        Inherits DBObjectPO

        Private m_Esportazione As Boolean       'Vero se si tratta di un'esportazione

        Private m_DataEsportazione As Date      'Data e ora dell'importazione/esportazione
        Private m_IDEsportataDa As Integer      'Nel caso delle esportazioni rappresenta l'operatore che ha esportato l'anagrafica.
        Private m_EsportataDa As CUser          'Utente che ha esportato l'anagrafica
        Private m_NomeEsportataDa As String     'Nome dell'utente che ha esportato l'anagrafica

        Private m_DataPresaInCarico As Date?    'Data di presa in carico
        Private m_IDPresaInCaricoDa As Integer  'ID dell'utente che ha preso in carico l'anagrafica
        Private m_PresaInCaricoDa As CUser      'Utente che ha preso in carico l'anagrafica
        Private m_NomePresaInCaricoDa As String 'Nome dell'utente che ha preso in carico l'anagrafica

        Private m_IDPersonaEsportata As Integer 'ID della persona esportata
        Private m_PersonaEsportata As CPersona  'Persona esportata
        Private m_NomePersonaEsportata As String 'Nome della persona esportata

        Private m_IDPersonaImportata As Integer 'ID della persona importata
        Private m_PersonaImportata As CPersona  'Persona importata
        Private m_NomePersonaImportata As String 'Nome della persona importata

        Private m_IDFinestraLavorazioneEsportata As Integer 'ID della finestra di lavorazione 
        Private m_FinestraLavorazioneEsportata As FinestraLavorazione

        Private m_IDFinestraLavorazioneImportata As Integer
        Private m_FinestraLavorazioneImportata As FinestraLavorazione

        Private m_AltriPrestiti As CCollection(Of CEstinzione)
        Private m_RichiesteFinanziamento As CCollection(Of CRichiestaFinanziamento)
        Private m_Documenti As CCollection(Of CAttachment)
        Private m_Consulenze As CCollection(Of CQSPDConsulenza)
        Private m_Pratiche As CCollection(Of CPraticaCQSPD)

        Private m_Corrispondenze As CCollection(Of CImportExportMatch)

        Private m_Flags As FlagsEsportazione

        Private m_Attributi As CKeyCollection

        Private m_StatoRemoto As StatoEsportazione
        Private m_DettaglioStatoRemoto As String

        Private m_SourceID As Integer
        Private m_Source As CImportExportSource

        Private m_SharedKey As String

        Private m_DataUltimoAggiornamento As Date?

        Private m_StatoConferma As StatoConfermaEsportazione
        Private m_DataEsportazioneOk As Date?
        Private m_IDOperatoreConferma As Integer
        Private m_OperatoreConferma As CUser
        Private m_NomeOperatoreConferma As String

        Private m_MessaggioEsportazione As String
        Private m_MessaggioImportazione As String

        Private m_MotivoRichiesta As String
        Private m_ImportoRichiesto As Decimal?
        Private m_RataMassima As Decimal?
        Private m_DurataMassima As Integer?
        Private m_RataProposta As Decimal?
        Private m_DurataProposta As Integer?
        Private m_NettoRicavoProposto As Decimal?
        Private m_NettoAllaManoProposto As Decimal?
        Private m_TANProposto As Double?
        Private m_TAEGProposto As Double?
        Private m_ValoreProvvigioneProposta As Decimal?

        Private m_MessaggioConferma As String

        Public Sub New()
            Me.m_Esportazione = False

            Me.m_DataEsportazione = Nothing
            Me.m_IDEsportataDa = 0
            Me.m_EsportataDa = Nothing
            Me.m_NomeEsportataDa = ""

            Me.m_DataPresaInCarico = Nothing
            Me.m_IDPresaInCaricoDa = 0
            Me.m_PresaInCaricoDa = Nothing
            Me.m_NomePresaInCaricoDa = ""

            Me.m_IDPersonaEsportata = 0
            Me.m_PersonaEsportata = Nothing
            Me.m_NomePersonaEsportata = ""

            Me.m_IDPersonaImportata = 0
            Me.m_PersonaImportata = Nothing
            Me.m_NomePersonaImportata = ""

            Me.m_IDFinestraLavorazioneEsportata = 0
            Me.m_FinestraLavorazioneEsportata = Nothing

            Me.m_IDFinestraLavorazioneImportata = 0
            Me.m_FinestraLavorazioneImportata = Nothing

            Me.m_AltriPrestiti = Nothing
            Me.m_RichiesteFinanziamento = Nothing
            Me.m_RichiesteFinanziamento = Nothing
            Me.m_Documenti = Nothing
            Me.m_Consulenze = Nothing
            Me.m_Pratiche = Nothing
            Me.m_Corrispondenze = Nothing

            Me.m_Flags = 0

            Me.m_Attributi = Nothing

            Me.m_StatoRemoto = StatoEsportazione.NonEsportato

            Me.m_SourceID = 0
            Me.m_Source = Nothing

            Me.m_DettaglioStatoRemoto = ""

            Me.m_SourceID = 0
            Me.m_Source = Nothing

            Me.m_SharedKey = ""

            Me.m_DataUltimoAggiornamento = Nothing
            Me.m_DataEsportazioneOk = Nothing
            Me.m_StatoConferma = StatoConfermaEsportazione.Inviato

            Me.m_IDOperatoreConferma = 0
            Me.m_OperatoreConferma = Nothing
            Me.m_NomeOperatoreConferma = ""
            Me.m_MessaggioConferma = ""

            Me.m_MessaggioEsportazione = ""
            Me.m_MessaggioImportazione = ""

            Me.m_MotivoRichiesta = ""
            Me.m_ImportoRichiesto = Nothing
            Me.m_RataMassima = Nothing
            Me.m_DurataMassima = Nothing
            Me.m_RataProposta = Nothing
            Me.m_DurataProposta = Nothing
            Me.m_NettoRicavoProposto = Nothing
            Me.m_NettoAllaManoProposto = Nothing
            Me.m_TANProposto = Nothing
            Me.m_TAEGProposto = Nothing
            Me.m_ValoreProvvigioneProposta = Nothing
        End Sub

        Public Property ValoreProvvigioneProposta As Decimal?
            Get
                Return Me.m_ValoreProvvigioneProposta
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreProvvigioneProposta
                If (oldValue = value) Then Return
                Me.m_ValoreProvvigioneProposta = value
                Me.DoChanged("ValoreProvvigioneProposta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo della richiesta confronto
        ''' </summary>
        ''' <returns></returns>
        Public Property MotivoRichiesta As String
            Get
                Return Me.m_MotivoRichiesta
            End Get
            Set(value As String)
                Dim oldValue As Integer = Me.m_MotivoRichiesta
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_MotivoRichiesta = value
                Me.DoChanged("MotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'importo richiesto dal cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property ImportoRichiesto As Decimal?
            Get
                Return Me.m_ImportoRichiesto
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ImportoRichiesto
                If (oldValue = value) Then Return
                Me.m_ImportoRichiesto = value
                Me.DoChanged("ImportoRichiesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la rata massima richiesta dal cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property RataMassima As Decimal?
            Get
                Return Me.m_RataMassima
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RataMassima
                If (oldValue = value) Then Return
                Me.m_RataMassima = value
                Me.DoChanged("RataMassima", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata massima richiesta dal cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property DurataMassima As Integer?
            Get
                Return Me.m_DurataMassima
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_DurataMassima
                If (oldValue = value) Then Return
                Me.m_DurataMassima = value
                Me.DoChanged("DurataMassima", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la rata dell'offerta migliore proposta al cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property RataProposta As Decimal?
            Get
                Return Me.m_RataProposta
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RataProposta
                If (oldValue = value) Then Return
                Me.m_RataProposta = value
                Me.DoChanged("RataProposta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata dell'offerta migliore proposta al cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property DurataProposta As Integer?
            Get
                Return Me.m_DurataProposta
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_DurataProposta
                If (oldValue = value) Then Return
                Me.m_DurataProposta = value
                Me.DoChanged("DurataProposta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il netto ricavo 
        ''' </summary>
        ''' <returns></returns>
        Public Property NettoRicavoProposto As Decimal?
            Get
                Return Me.m_NettoRicavoProposto
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_NettoRicavoProposto
                If (oldValue = value) Then Return
                Me.m_NettoRicavoProposto = value
                Me.DoChanged("NettoRicavoProposto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il netto alla mano proposto al cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property NettoAllaManoProposto As Decimal?
            Get
                Return Me.m_NettoAllaManoProposto
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_NettoAllaManoProposto
                If (oldValue = value) Then Return
                Me.m_NettoAllaManoProposto = value
                Me.DoChanged("NettoAllaManoProposto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TAN dell'offerta proposta al cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property TANProposto As Double?
            Get
                Return Me.m_TANProposto
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TANProposto
                If (oldValue = value) Then Return
                Me.m_TANProposto = value
                Me.DoChanged("TANProposto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TAEG dell'offerta proposta al cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property TAEGProposto As Double?
            Get
                Return Me.m_TAEGProposto
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAEGProposto
                If (oldValue = value) Then Return
                Me.m_TAEGProposto = value
                Me.DoChanged("TAEGProposto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il messaggio inviato da chi ha effettuato l'esportazione
        ''' </summary>
        ''' <returns></returns>
        Public Property MessaggioEsportazione As String
            Get
                Return Me.m_MessaggioEsportazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MessaggioEsportazione
                If (oldValue = value) Then Return
                value = Strings.Trim(value)
                Me.m_MessaggioEsportazione = value
                Me.DoChanged("MessaggioEsportazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il messaggio inviato da chi ha confermato l'esportazione
        ''' </summary>
        ''' <returns></returns>
        Public Property MessaggioConferma As String
            Get
                Return Me.m_MessaggioConferma
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MessaggioConferma
                If (oldValue = value) Then Return
                value = Strings.Trim(value)
                Me.m_MessaggioConferma = value
                Me.DoChanged("MessaggioConferma", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il messaggio inviato da chi ha effettuato l'importazione
        ''' </summary>
        ''' <returns></returns>
        Public Property MessaggioImportazione As String
            Get
                Return Me.m_MessaggioImportazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MessaggioImportazione
                If (oldValue = value) Then Return
                value = Strings.Trim(value)
                Me.m_MessaggioImportazione = value
                Me.DoChanged("MessaggioImportazione", value, oldValue)
            End Set
        End Property

        Public Property IDOperatoreConferma As Integer
            Get
                Return GetID(Me.m_OperatoreConferma, Me.m_IDOperatoreConferma)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreConferma
                If (oldValue = value) Then Return
                Me.m_IDOperatoreConferma = value
                Me.m_OperatoreConferma = Nothing
                Me.DoChanged("IDOperatoreConferma", value, oldValue)
            End Set
        End Property

        Public Property OperatoreConferma As CUser
            Get
                If (Me.m_OperatoreConferma Is Nothing) Then Me.m_OperatoreConferma = Sistema.Users.GetItemById(Me.m_IDOperatoreConferma)
                Return Me.m_OperatoreConferma
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreConferma
                If (oldValue Is value) Then Return
                Me.m_OperatoreConferma = value
                Me.m_IDOperatoreConferma = GetID(value)
                Me.m_NomeOperatoreConferma = ""
                If (value IsNot Nothing) Then Me.m_NomeOperatoreConferma = value.Nominativo
                Me.DoChanged("OperatoreConferma", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatoreConferma As String
            Get
                Return Me.m_NomeOperatoreConferma
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatoreConferma
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeOperatoreConferma = value
                Me.DoChanged("NomeOperatoreConferma", value, oldValue)
            End Set
        End Property

        Public Property StatoConferma As StatoConfermaEsportazione
            Get
                Return Me.m_StatoConferma
            End Get
            Set(value As StatoConfermaEsportazione)
                Dim oldValue As StatoConfermaEsportazione = Me.m_StatoConferma
                If (oldValue = value) Then Return
                Me.m_StatoConferma = value
                Me.DoChanged("StatoConferma", value, oldValue)
            End Set
        End Property

        Public Property DataEsportazioneOk As Date?
            Get
                Return Me.m_DataEsportazioneOk
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEsportazioneOk
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataEsportazioneOk = value
                Me.DoChanged("DataEsportazioneOk", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo aggiornamento del record
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataUltimoAggiornamento As Date?
            Get
                Return Me.m_DataUltimoAggiornamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataUltimoAggiornamento
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataUltimoAggiornamento = value
                Me.DoChanged("DataUltimoAggiornamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se si tratta di una esportazione o di una importazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Esportazione As Boolean
            Get
                Return Me.m_Esportazione
            End Get
            Set(value As Boolean)
                If (Me.m_Esportazione = value) Then Return
                Me.m_Esportazione = value
                Me.DoChanged("Esportazione", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di esportazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEsportazione As Date
            Get
                Return Me.m_DataEsportazione
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataEsportazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataEsportazione = value
                Me.DoChanged("DataEsportazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha esportato la scheda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDEsportataDa As Integer
            Get
                Return GetID(Me.m_EsportataDa, Me.m_IDEsportataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEsportataDa
                If (oldValue = value) Then Exit Property
                Me.m_IDEsportataDa = value
                Me.m_EsportataDa = Nothing
                Me.DoChanged("IDEsportataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha esportato la scheda.
        ''' Se si tratta di un utente remoto (Esportazione = False) la proprietà restituisce NULL e solo i campi IDEsportatoDa e NomeEsportatoDa identificano l'utente remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsportataDa As CUser
            Get
                If (Not Me.m_Esportazione) Then Return Nothing
                If (Me.m_EsportataDa Is Nothing) Then Me.m_EsportataDa = Sistema.Users.GetItemById(Me.m_IDEsportataDa)
                Return Me.m_EsportataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.EsportataDa
                If (oldValue Is value) Then Exit Property
                Me.m_IDEsportataDa = GetID(value)
                Me.m_EsportataDa = value
                Me.m_NomeEsportataDa = ""
                If (value IsNot Nothing) Then Me.m_NomeEsportataDa = value.Nominativo
                Me.DoChanged("EsportataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha esportato la scheda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeEsportataDa As String
            Get
                Return Me.m_NomeEsportataDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeEsportataDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeEsportataDa = value
                Me.DoChanged("NomeEsportataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui l'utente remoto ha preso in carico l'esportazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPresaInCarico As Date?
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPresaInCarico
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha preso in carico l'oggetto.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPresaInCaricoDa As Integer
            Get
                Return GetID(Me.m_PresaInCaricoDa, Me.m_IDPresaInCaricoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPresaInCaricoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDPresaInCaricoDa = value
                Me.m_PresaInCaricoDa = Nothing
                Me.DoChanged("IDPresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha preso in carico la scheda.
        ''' Se si tratta di un utente remoto (Esportazione = True) viene restituito NULL e solo l'ID ed il nome identificano l'utente remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresaInCaricoDa As CUser
            Get
                If (Me.m_Esportazione) Then Return Nothing
                If (Me.m_PresaInCaricoDa Is Nothing) Then Me.m_PresaInCaricoDa = Sistema.Users.GetItemById(Me.m_IDPresaInCaricoDa)
                Return Me.m_PresaInCaricoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.PresaInCaricoDa
                If (oldValue Is value) Then Exit Property
                Me.m_PresaInCaricoDa = value
                Me.m_IDPresaInCaricoDa = GetID(value)
                Me.m_NomePresaInCaricoDa = ""
                If (value IsNot Nothing) Then Me.m_NomePresaInCaricoDa = value.Nominativo
                Me.DoChanged("NomePresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha preso in carico la scheda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePresaInCaricoDa As String
            Get
                Return Me.m_NomePresaInCaricoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePresaInCaricoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePresaInCaricoDa = value
                Me.DoChanged("NomePresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona esportata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersonaEsportata As Integer
            Get
                Return GetID(Me.m_PersonaEsportata, Me.m_IDPersonaEsportata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersonaEsportata
                If (oldValue = value) Then Exit Property
                Me.m_IDPersonaEsportata = value
                Me.m_PersonaEsportata = Nothing
                Me.DoChanged("IDPersonaEsportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona esportata.
        ''' Se si tratta di una persona remoto (Esportazione = False) la proprietà restituisce NULL e solo l'ID ed il nome identificano la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PersonaEsportata As CPersona
            Get
                If (Not Me.m_Esportazione) Then Return Nothing
                If (Me.m_PersonaEsportata Is Nothing) Then Me.m_PersonaEsportata = Anagrafica.Persone.GetItemById(Me.m_IDPersonaEsportata)
                Return Me.m_PersonaEsportata
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_PersonaEsportata
                If (oldValue Is value) Then Exit Property
                Me.m_PersonaEsportata = value
                Me.m_IDPersonaEsportata = GetID(value)
                Me.m_NomePersonaEsportata = ""
                If (value IsNot Nothing) Then Me.m_NomePersonaEsportata = value.Nominativo
                Me.DoChanged("PersonaEsportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona esportata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersonaEsportata As String
            Get
                Return Me.m_NomePersonaEsportata
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersonaEsportata
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersonaEsportata = value
                Me.DoChanged("NomePersonaEsportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona importata.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersonaImportata As Integer
            Get
                Return GetID(Me.m_PersonaImportata, Me.m_IDPersonaImportata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersonaImportata
                If (oldValue = value) Then Exit Property
                Me.m_IDPersonaImportata = value
                Me.m_PersonaImportata = Nothing
                Me.DoChanged("IDPersonaImportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona importata.
        ''' Se si tratta di un'esportazione il sistema restituisce NULL e solo l'ID ed il nome identificano la persona sul sistema remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PersonaImportata As CPersona
            Get
                If (Me.m_Esportazione) Then Return Nothing
                If (Me.m_PersonaImportata Is Nothing) Then Me.m_PersonaImportata = Anagrafica.Persone.GetItemById(Me.m_IDPersonaImportata)
                Return Me.m_PersonaImportata
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_PersonaImportata
                If (oldValue Is value) Then Exit Property
                Me.m_PersonaImportata = value
                Me.m_IDPersonaImportata = GetID(value)
                Me.m_NomePersonaImportata = ""
                If (value IsNot Nothing) Then Me.m_NomePersonaImportata = value.Nominativo
                Me.DoChanged("PersonaImportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona importata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersonaImportata As String
            Get
                Return Me.m_NomePersonaImportata
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersonaImportata
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersonaImportata = value
                Me.DoChanged("NomePersonaImportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della finestra di lavorazione esportata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFinestraLavorazioneEsportata As Integer
            Get
                Return GetID(Me.m_FinestraLavorazioneEsportata, Me.m_IDFinestraLavorazioneEsportata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFinestraLavorazioneEsportata
                If (oldValue = value) Then Exit Property
                Me.m_IDFinestraLavorazioneEsportata = value
                Me.m_FinestraLavorazioneEsportata = Nothing
                Me.DoChanged("IDFinestraLavorazioneEsportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la finestra di lavorazione esportata.
        ''' Se si tratta di una importazione la proprietà restituisce NULL e solo l'ID identifica la finestra sul sistema remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FinestraLavorazioneEsportata As FinestraLavorazione
            Get
                If (Not Me.m_Esportazione) Then Return Nothing
                If (Me.m_FinestraLavorazioneEsportata Is Nothing) Then Me.m_FinestraLavorazioneEsportata = Finanziaria.FinestreDiLavorazione.GetItemById(Me.m_IDFinestraLavorazioneEsportata)
                Return Me.m_FinestraLavorazioneEsportata
            End Get
            Set(value As FinestraLavorazione)
                Dim oldValue As FinestraLavorazione = Me.m_FinestraLavorazioneEsportata
                If (oldValue Is value) Then Exit Property
                Me.m_IDFinestraLavorazioneEsportata = GetID(value)
                Me.m_FinestraLavorazioneEsportata = value
                Me.DoChanged("FinestraLavorazioneEsportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della finestra di lavorazione importata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFinestraLavorazioneImportata As Integer
            Get
                Return GetID(Me.m_FinestraLavorazioneImportata, Me.m_IDFinestraLavorazioneImportata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFinestraLavorazioneImportata
                If (oldValue = value) Then Exit Property
                Me.m_IDFinestraLavorazioneImportata = value
                Me.m_FinestraLavorazioneImportata = Nothing
                Me.DoChanged("IDFinestraLavorazioneImportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la finestra di lavorazione importata.
        ''' In caso si tratti di una esportazione la proporietà restituisce NULL e solo l'ID identifica la finestra remota
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FinestraLavorazioneImportata As FinestraLavorazione
            Get
                If (Me.m_Esportazione) Then Return Nothing
                If (Me.m_FinestraLavorazioneImportata Is Nothing) Then Me.m_FinestraLavorazioneImportata = Finanziaria.FinestreDiLavorazione.GetItemById(Me.m_IDFinestraLavorazioneImportata)
                Return Me.m_FinestraLavorazioneImportata
            End Get
            Set(value As FinestraLavorazione)
                Dim oldValue As FinestraLavorazione = Me.m_FinestraLavorazioneImportata
                If (oldValue Is value) Then Exit Property
                Me.m_IDFinestraLavorazioneImportata = GetID(value)
                Me.m_FinestraLavorazioneImportata = value
                Me.DoChanged("FinestraLavorazioneImportata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei prestiti che sono stati esportati verso il server remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AltriPrestiti As CCollection(Of CEstinzione)
            Get
                If (Me.m_AltriPrestiti Is Nothing) Then Me.m_AltriPrestiti = New CCollection(Of CEstinzione)
                Return Me.m_AltriPrestiti
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle richieste di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RichiesteFinanziamento As CCollection(Of CRichiestaFinanziamento)
            Get
                If (Me.m_RichiesteFinanziamento Is Nothing) Then Me.m_RichiesteFinanziamento = New CCollection(Of CRichiestaFinanziamento)
                Return Me.m_RichiesteFinanziamento
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei documenti che sono stati esportati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Documenti As CCollection(Of CAttachment)
            Get
                If (Me.m_Documenti Is Nothing) Then Me.m_Documenti = New CCollection(Of CAttachment)
                Return Me.m_Documenti
            End Get
        End Property

        Public ReadOnly Property Consulenze As CCollection(Of CQSPDConsulenza)
            Get
                If (Me.m_Consulenze Is Nothing) Then Me.m_Consulenze = New CCollection(Of CQSPDConsulenza)
                Return Me.m_Consulenze
            End Get
        End Property

        Public ReadOnly Property Pratiche As CCollection(Of CPraticaCQSPD)
            Get
                If (Me.m_Pratiche Is Nothing) Then Me.m_Pratiche = New CCollection(Of CPraticaCQSPD)
                Return Me.m_Pratiche
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle corrispondenze tra gli oggetti locali e gli oggetti remoti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Corrispondenze As CCollection(Of CImportExportMatch)
            Get
                If (Me.m_Corrispondenze Is Nothing) Then Me.m_Corrispondenze = New CCollection(Of CImportExportMatch)
                Return Me.m_Corrispondenze
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As FlagsEsportazione
            Get
                Return Me.m_Flags
            End Get
            Set(value As FlagsEsportazione)
                Dim oldValue As FlagsEsportazione = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione degli attributi aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato dell'esportazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoRemoto As StatoEsportazione
            Get
                Return Me.m_StatoRemoto
            End Get
            Set(value As StatoEsportazione)
                Dim oldValue As StatoEsportazione = Me.m_StatoRemoto
                If (oldValue = value) Then Exit Property
                Me.m_StatoRemoto = value
                Me.DoChanged("StatoRemoto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive lo stato remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioStatoRemoto As String
            Get
                Return Me.m_DettaglioStatoRemoto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioStatoRemoto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStatoRemoto = value
                Me.DoChanged("DettaglioStatoRemoto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto usato per importare/esportare 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
            Get
                Return GetID(Me.m_Source, Me.m_SourceID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto usato per importare/esportare 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source As CImportExportSource
            Get
                If (Me.m_Source Is Nothing) Then Me.m_Source = Finanziaria.ImportExportSources.GetItemById(Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As CImportExportSource)
                Dim oldValue As CImportExportSource = Me.Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_SourceID = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la chiave generata dal sistema di esportazione e condivisa con il sistema di importazione.
        ''' Questa chiave è univoca per ogni record di esportazione (nel sistema sorgente) ed è necessaria per i successivi aggiornamenti ricevuti dal sistema destinazione.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SharedKey As String
            Get
                Return Me.m_SharedKey
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SharedKey
                If (oldValue = value) Then Exit Property
                Me.m_SharedKey = value
                Me.DoChanged("SharedKey", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Finanziaria.ImportExport.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDImportExport"
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Esportazione", Me.m_Esportazione)
            writer.Write("DataEsportazione", Me.m_DataEsportazione)
            writer.Write("IDEsportataDa", Me.IDEsportataDa)
            writer.Write("NomeEsportataDa", Me.m_NomeEsportataDa)
            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.Write("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.Write("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            writer.Write("IDPersonaEsportata", Me.IDPersonaEsportata)
            writer.Write("NomePersonaEsportata", Me.m_NomePersonaEsportata)
            writer.Write("IDPersonaImportata", Me.IDPersonaImportata)
            writer.Write("NomePersonaImportata", Me.m_NomePersonaImportata)
            writer.Write("IDFinestraLavorazioneEsportata", Me.IDFinestraLavorazioneEsportata)
            writer.Write("IDFinestraLavorazioneImportata", Me.IDFinestraLavorazioneImportata)
            writer.Write("AltriPrestiti", XML.Utils.Serializer.Serialize(Me.AltriPrestiti))
            writer.Write("RichiesteFinanziamento", XML.Utils.Serializer.Serialize(Me.RichiesteFinanziamento))
            writer.Write("Documenti", XML.Utils.Serializer.Serialize(Me.Documenti))
            writer.Write("Consulenze", XML.Utils.Serializer.Serialize(Me.Consulenze))
            writer.Write("Pratiche", XML.Utils.Serializer.Serialize(Me.Pratiche))
            writer.Write("Corrispondenze", XML.Utils.Serializer.Serialize(Me.Corrispondenze))
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("StatoRemoto", Me.m_StatoRemoto)
            writer.Write("DettaglioStatoRemoto", Me.m_DettaglioStatoRemoto)
            writer.Write("SourceID", Me.SourceID)
            writer.Write("SharedKey", Me.m_SharedKey)
            writer.Write("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            writer.Write("DataEsportazioneOk", Me.m_DataEsportazioneOk)
            writer.Write("StatoConferma", Me.m_StatoConferma)
            writer.Write("IDOperatoreConferma", Me.IDOperatoreConferma)
            writer.Write("NomeOperatoreConferma", Me.m_NomeOperatoreConferma)
            writer.Write("MessaggioEsportazione", Me.m_MessaggioEsportazione)
            writer.Write("MessaggioImportazione", Me.m_MessaggioImportazione)
            writer.Write("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.Write("ImportoRichiesto", Me.m_ImportoRichiesto)
            writer.Write("RataMassima", Me.m_RataMassima)
            writer.Write("DurataMassima", Me.m_DurataMassima)
            writer.Write("RataProposta", Me.m_RataProposta)
            writer.Write("DurataProposta", Me.m_DurataProposta)
            writer.Write("NettoRicavoProposto", Me.m_NettoRicavoProposto)
            writer.Write("NettoAllaManoProposto", Me.m_NettoAllaManoProposto)
            writer.Write("TANProposto", Me.m_TANProposto)
            writer.Write("TAEGProposto", Me.m_TAEGProposto)
            writer.Write("ValoreProvvigioneProposta", Me.m_ValoreProvvigioneProposta)
            writer.Write("MessaggioConferma", Me.m_MessaggioConferma)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Esportazione = reader.Read("Esportazione", Me.m_Esportazione)
            Me.m_DataEsportazione = reader.Read("DataEsportazione", Me.m_DataEsportazione)
            Me.m_IDEsportataDa = reader.Read("IDEsportataDa", Me.m_IDEsportataDa)
            Me.m_NomeEsportataDa = reader.Read("NomeEsportataDa", Me.m_NomeEsportataDa)
            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)
            Me.m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", Me.m_IDPresaInCaricoDa)
            Me.m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            Me.m_IDPersonaEsportata = reader.Read("IDPersonaEsportata", Me.m_IDPersonaEsportata)
            Me.m_NomePersonaEsportata = reader.Read("NomePersonaEsportata", Me.m_NomePersonaEsportata)
            Me.m_IDPersonaImportata = reader.Read("IDPersonaImportata", Me.m_IDPersonaImportata)
            Me.m_NomePersonaImportata = reader.Read("NomePersonaImportata", Me.m_NomePersonaImportata)
            Me.m_IDFinestraLavorazioneEsportata = reader.Read("IDFinestraLavorazioneEsportata", Me.m_IDFinestraLavorazioneEsportata)
            Me.m_IDFinestraLavorazioneImportata = reader.Read("IDFinestraLavorazioneImportata", Me.m_IDFinestraLavorazioneImportata)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_StatoRemoto = reader.Read("StatoRemoto", Me.m_StatoRemoto)
            Me.m_DettaglioStatoRemoto = reader.Read("DettaglioStatoRemoto", Me.m_DettaglioStatoRemoto)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_SharedKey = reader.Read("SharedKey", Me.m_SharedKey)
            Me.m_DataUltimoAggiornamento = reader.Read("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            Me.m_DataEsportazioneOk = reader.Read("DataEsportazioneOk", Me.m_DataEsportazioneOk)
            Me.m_StatoConferma = reader.Read("StatoConferma", Me.m_StatoConferma)
            Me.m_IDOperatoreConferma = reader.Read("IDOperatoreConferma", Me.m_IDOperatoreConferma)
            Me.m_NomeOperatoreConferma = reader.Read("NomeOperatoreConferma", Me.m_NomeOperatoreConferma)
            Me.m_MessaggioEsportazione = reader.Read("MessaggioEsportazione", Me.m_MessaggioEsportazione)
            Me.m_MessaggioImportazione = reader.Read("MessaggioImportazione", Me.m_MessaggioImportazione)
            Me.m_MotivoRichiesta = reader.Read("MotivoRichiesta", Me.m_MotivoRichiesta)
            Me.m_ImportoRichiesto = reader.Read("ImportoRichiesto", Me.m_ImportoRichiesto)
            Me.m_RataMassima = reader.Read("RataMassima", Me.m_RataMassima)
            Me.m_DurataMassima = reader.Read("DurataMassima", Me.m_DurataMassima)
            Me.m_RataProposta = reader.Read("RataProposta", Me.m_RataProposta)
            Me.m_DurataProposta = reader.Read("DurataProposta", Me.m_DurataProposta)
            Me.m_NettoRicavoProposto = reader.Read("NettoRicavoProposto", Me.m_NettoRicavoProposto)
            Me.m_NettoAllaManoProposto = reader.Read("NettoAllaManoProposto", Me.m_NettoAllaManoProposto)
            Me.m_TANProposto = reader.Read("TANProposto", Me.m_TANProposto)
            Me.m_TAEGProposto = reader.Read("TAEGProposto", Me.m_TAEGProposto)
            Me.m_ValoreProvvigioneProposta = reader.Read("ValoreProvvigioneProposta", Me.m_ValoreProvvigioneProposta)
            Me.m_MessaggioConferma = reader.Read("MessaggioConferma", Me.m_MessaggioConferma)

            Dim tmp As String = ""
            tmp = reader.Read("Attributi", tmp)
            If (tmp <> "") Then Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)

            Me.m_AltriPrestiti = New CCollection(Of CEstinzione)
            tmp = reader.Read("AltriPrestiti", "")
            If (tmp <> "") Then Me.m_AltriPrestiti.AddRange(XML.Utils.Serializer.Deserialize(tmp))

            Try
                Me.m_RichiesteFinanziamento = New CCollection(Of CRichiestaFinanziamento)
                Me.m_RichiesteFinanziamento.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("RichiesteFinanziamento", "")))
            Catch ex As Exception
                Me.m_RichiesteFinanziamento = Nothing
            End Try

            Try
                Me.m_Documenti = New CCollection(Of CAttachment)
                Me.m_Documenti.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Documenti", "")))
            Catch ex As Exception
                Me.m_Documenti = Nothing
            End Try

            Try
                Me.m_Consulenze = New CCollection(Of CQSPDConsulenza)
                Me.m_Consulenze.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Consulenze", "")))
            Catch ex As Exception
                Me.m_Consulenze = Nothing
            End Try

            Try
                Me.m_Pratiche = New CCollection(Of CPraticaCQSPD)
                Me.m_Pratiche.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Pratiche", "")))
            Catch ex As Exception
                Me.m_Pratiche = Nothing
            End Try

            Try
                Me.m_Corrispondenze = New CCollection(Of CImportExportMatch)
                Me.m_Corrispondenze.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Corrispondenze", "")))
            Catch ex As Exception
                Me.m_Corrispondenze = Nothing
            End Try

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Esportazione", Me.m_Esportazione)
            writer.WriteAttribute("DataEsportazione", Me.m_DataEsportazione)
            writer.WriteAttribute("IDEsportataDa", Me.IDEsportataDa)
            writer.WriteAttribute("NomeEsportataDa", Me.m_NomeEsportataDa)
            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.WriteAttribute("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            writer.WriteAttribute("IDPersonaEsportata", Me.IDPersonaEsportata)
            writer.WriteAttribute("NomePersonaEsportata", Me.m_NomePersonaEsportata)
            writer.WriteAttribute("IDPersonaImportata", Me.IDPersonaImportata)
            writer.WriteAttribute("NomePersonaImportata", Me.m_NomePersonaImportata)
            writer.WriteAttribute("IDFinestraLavorazioneEsportata", Me.IDFinestraLavorazioneEsportata)
            writer.WriteAttribute("IDFinestraLavorazioneImportata", Me.IDFinestraLavorazioneImportata)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DettaglioStatoRemoto", Me.m_DettaglioStatoRemoto)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("SharedKey", Me.m_SharedKey)
            writer.WriteAttribute("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            writer.WriteAttribute("StatoRemoto", Me.m_StatoRemoto)
            writer.WriteAttribute("DataEsportazioneOk", Me.m_DataEsportazioneOk)
            writer.WriteAttribute("StatoConferma", Me.m_StatoConferma)
            writer.WriteAttribute("IDOperatoreConferma", Me.IDOperatoreConferma)
            writer.WriteAttribute("NomeOperatoreConferma", Me.m_NomeOperatoreConferma)

            writer.WriteAttribute("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.WriteAttribute("ImportoRichiesto", Me.m_ImportoRichiesto)
            writer.WriteAttribute("RataMassima", Me.m_RataMassima)
            writer.WriteAttribute("DurataMassima", Me.m_DurataMassima)
            writer.WriteAttribute("RataProposta", Me.m_RataProposta)
            writer.WriteAttribute("DurataProposta", Me.m_DurataProposta)
            writer.WriteAttribute("NettoRicavoProposto", Me.m_NettoRicavoProposto)
            writer.WriteAttribute("NettoAllaManoProposto", Me.m_NettoAllaManoProposto)
            writer.WriteAttribute("TANProposto", Me.m_TANProposto)
            writer.WriteAttribute("TAEGProposto", Me.m_TAEGProposto)
            writer.WriteAttribute("ValoreProvvigioneProposta", Me.m_ValoreProvvigioneProposta)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("AltriPrestiti", Me.AltriPrestiti)
            writer.WriteTag("RichiesteFinanziamento", Me.RichiesteFinanziamento)
            writer.WriteTag("Documenti", Me.Documenti)
            writer.WriteTag("Consulenze", Me.Consulenze)
            writer.WriteTag("Pratiche", Me.Pratiche)
            writer.WriteTag("Corrispondenze", Me.Corrispondenze)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("MessaggioEsportazione", Me.m_MessaggioEsportazione)
            writer.WriteTag("MessaggioImportazione", Me.m_MessaggioImportazione)
            writer.WriteTag("MessaggioConferma", Me.m_MessaggioConferma)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Esportazione" : Me.m_Esportazione = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataEsportazione" : Me.m_DataEsportazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDEsportataDa" : Me.m_IDEsportataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeEsportataDa" : Me.m_NomeEsportataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPresaInCaricoDa" : Me.m_IDPresaInCaricoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePresaInCaricoDa" : Me.m_NomePresaInCaricoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPersonaEsportata" : Me.m_IDPersonaEsportata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersonaEsportata" : Me.m_NomePersonaEsportata = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPersonaImportata" : Me.m_IDPersonaImportata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersonaImportata" : Me.m_NomePersonaImportata = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFinestraLavorazioneEsportata" : Me.m_IDFinestraLavorazioneEsportata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDFinestraLavorazioneImportata" : Me.m_IDFinestraLavorazioneImportata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStatoRemoto" : Me.m_DettaglioStatoRemoto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SharedKey" : Me.m_SharedKey = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataUltimoAggiornamento" : Me.m_DataUltimoAggiornamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoRemoto" : Me.m_StatoRemoto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AltriPrestiti" : Me.m_AltriPrestiti = New CCollection(Of CEstinzione) : Me.m_AltriPrestiti.AddRange(fieldValue)
                Case "RichiesteFinanziamento" : Me.m_RichiesteFinanziamento = New CCollection(Of CRichiestaFinanziamento) : Me.m_RichiesteFinanziamento.AddRange(fieldValue)
                Case "Documenti" : Me.m_Documenti = New CCollection(Of CAttachment) : Me.m_Documenti.AddRange(fieldValue)
                Case "Consulenze" : Me.m_Consulenze = New CCollection(Of CQSPDConsulenza) : Me.m_Consulenze.AddRange(fieldValue)
                Case "Pratiche" : Me.m_Pratiche = New CCollection(Of CPraticaCQSPD) : Me.m_Pratiche.AddRange(fieldValue)
                Case "Corrispondenze" : Me.m_Corrispondenze = New CCollection(Of CImportExportMatch) : Me.m_Corrispondenze.AddRange(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case "DataEsportazioneOk" : Me.m_DataEsportazioneOk = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoConferma" : Me.m_StatoConferma = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOperatoreConferma" : Me.m_IDOperatoreConferma = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatoreConferma" : Me.m_NomeOperatoreConferma = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MessaggioEsportazione" : Me.m_MessaggioEsportazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MessaggioImportazione" : Me.m_MessaggioImportazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MessaggioConferma" : Me.m_MessaggioConferma = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "MotivoRichiesta" : Me.m_MotivoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ImportoRichiesto" : Me.m_ImportoRichiesto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RataMassima" : Me.m_RataMassima = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DurataMassima" : Me.m_DurataMassima = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RataProposta" : Me.m_RataProposta = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DurataProposta" : Me.m_DurataProposta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NettoRicavoProposto" : Me.m_NettoRicavoProposto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NettoAllaManoProposto" : Me.m_NettoAllaManoProposto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TANProposto" : Me.m_TANProposto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEGProposto" : Me.m_TAEGProposto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValoreProvvigioneProposta" : Me.m_ValoreProvvigioneProposta = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Sub Esporta()
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            If (Me.m_StatoRemoto <> StatoEsportazione.NonEsportato) Then Throw New InvalidOperationException("Già esportato")
            Me.m_SharedKey = Sistema.ASPSecurity.GetRandomKey(128)
            Me.EsportataDa = Sistema.Users.CurrentUser
            Me.DataEsportazione = DateUtils.Now
            Me.Source.Esporta(Me)
            Me.m_StatoRemoto = StatoEsportazione.Esportato
            Me.m_DettaglioStatoRemoto = "Richiesta Confronto"
            Me.Save(True)
        End Sub

        Public Sub ConfermaEsportazione(ByVal message As String)
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            message = Strings.Trim(message)
            Me.Source.ConfermaEsportazione(Me)
            Me.DataEsportazioneOk = DateUtils.Now
            Me.StatoConferma = StatoConfermaEsportazione.Confermato
            Me.MessaggioConferma = message
            Me.m_DettaglioStatoRemoto = "Esportazione Confermata"
            Me.OperatoreConferma = Sistema.Users.CurrentUser
            Me.Save(True)
        End Sub

        Public Sub AnnullaEsportazione(ByVal message As String)
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            message = Strings.Trim(message)
            If (message = "") Then Throw New ArgumentNullException("E' necessario specificare il motivo dell'annullamento")
            Me.Source.AnnullaEsportazione(Me)
            Me.DataEsportazioneOk = DateUtils.Now
            Me.StatoConferma = StatoConfermaEsportazione.Revocato
            Me.MessaggioConferma = message
            Me.m_DettaglioStatoRemoto = "Esportazione Revocata"
            Me.OperatoreConferma = Sistema.Users.CurrentUser
            Me.Save(True)
        End Sub

        Public Sub RifiutaCliente(ByVal message As String)
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            Me.Source.RifiutaCliente(Me)
            message = Strings.Trim(message)
            If (message = "") Then Throw New ArgumentNullException("E' necessario specificare il motivo del rifiuto")
            Me.DataEsportazioneOk = DateUtils.Now
            Me.StatoConferma = StatoConfermaEsportazione.Rifiutata
            Me.MessaggioConferma = message
            Me.m_DettaglioStatoRemoto = "Cliente Rifiutato"
            Me.OperatoreConferma = Sistema.Users.CurrentUser
            Me.Save(True)
        End Sub


        Public Sub Importa()
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            If (Me.m_StatoRemoto = StatoEsportazione.Importato) Then Throw New InvalidOperationException("Già importato")
            Try
                Me.PresaInCaricoDa = Sistema.Users.CurrentUser
                Me.DataPresaInCarico = DateUtils.Now
                Me.Source.Importa(Me)
                Me.m_StatoRemoto = StatoEsportazione.Importato
                Me.m_DettaglioStatoRemoto = "Importato"
            Catch ex As Exception
                Me.m_StatoRemoto = StatoEsportazione.Errore
                Me.m_DettaglioStatoRemoto = "Errore di Importazione: " & ex.Message
            End Try
            Me.Save(True)
        End Sub

        Public Sub PrendiInCarico()
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            If (Me.m_StatoRemoto <> StatoEsportazione.Importato) Then Throw New InvalidOperationException("Non importato")
            Try
                Me.PresaInCaricoDa = Sistema.Users.CurrentUser
                Me.DataPresaInCarico = DateUtils.Now
                Me.FinestraLavorazioneImportata = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(Me.PersonaImportata)
                Me.Source.Importa(Me)
                Me.m_StatoRemoto = StatoEsportazione.Importato
                Me.m_DettaglioStatoRemoto = "Importato"
            Catch ex As Exception
                Me.m_StatoRemoto = StatoEsportazione.Errore
                Me.m_DettaglioStatoRemoto = "Errore di Importazione: " & ex.Message
            End Try
            Me.Save(True)
        End Sub

        Public Sub Sincronizza(ByVal oggetti As CKeyCollection)
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            If (Me.m_StatoRemoto <> StatoEsportazione.Importato) Then Throw New InvalidOperationException("Non importato")
            Try


                Me.Source.Sincronizza(Me, oggetti)
                Me.m_DataUltimoAggiornamento = DateUtils.Now


            Catch ex As Exception
                Me.m_StatoRemoto = StatoEsportazione.Errore
                Me.m_DettaglioStatoRemoto = "Errore di Sincronizzazione: " & ex.Message
            End Try
            Me.Save(True)
        End Sub

        Public Sub Sollecita()
            If (Me.Source Is Nothing) Then Throw New ArgumentNullException("Sorgente di esportazione non definita")
            'If (Me.m_StatoRemoto <> StatoEsportazione.Importato) Then Throw New InvalidOperationException("Non importato")
            Try


                Me.Source.Sollecita(Me)
                Me.m_DataUltimoAggiornamento = DateUtils.Now


            Catch ex As Exception

            End Try
            Me.Save(True)
        End Sub

    End Class





End Class