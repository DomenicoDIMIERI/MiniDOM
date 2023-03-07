Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.CustomerCalls
Imports minidom.Internals

Namespace Internals


    Public NotInheritable Class CCRMClass
        Inherits CModulesClass(Of CContattoUtente)

        ''' <summary>
        ''' Evento generato quando viene memorizzato un nuovo contatto (telefonata effettuata o ricevuta)
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event NuovoContatto(ByVal e As ContattoEventArgs)

        ''' <summary>
        ''' Evento generato quando un contatto viene eliminato
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ContattoEliminato(ByVal e As ContattoEventArgs)

        ''' <summary>
        ''' Evento generato quando un contatto viene modificato
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ContattoModificato(ByVal e As ContattoEventArgs)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event DatabaseChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione del CRM
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ''' <summary>
        ''' Evento generato quando viene ricevuto un Fax
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event FaxReceived(ByVal sender As Object, ByVal e As ContattoEventArgs)

        ''' <summary>
        ''' Evento generato quando un fax viene inviato correttamente
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event FaxSent(ByVal sender As Object, ByVal e As ContattoEventArgs)

        ''' <summary>
        ''' Evento generato quando l'invio di un fax fallisce
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event FaxFailed(ByVal sender As Object, ByVal e As ContattoEventArgs)


        Private m_Database As CDBConnection = Nothing
        Private m_TelDB As CDBConnection = Nothing
        Private m_StatsDB As CDBConnection = Nothing

        Friend Sub New()
            MyBase.New("modCustomerCalls", GetType(CCustomerCallsCursor))
            AddHandler Anagrafica.PersonaMerged, AddressOf HandlePeronaMerged
            AddHandler Anagrafica.PersonaUnMerged, AddressOf HandlePeronaUnMerged
            AddHandler Anagrafica.PersonaModified, AddressOf HandlePeronaModified
            AddHandler Anagrafica.PersonaCreated, AddressOf HandlePeronaModified
            AddHandler Anagrafica.Ricontatti.RicontattoModified, AddressOf HandleRicontattoModified
            AddHandler Anagrafica.Ricontatti.RicontattoCreated, AddressOf HandleRicontattoModified

            AddHandler Me.NuovoContatto, AddressOf handleNuovoContatto
            AddHandler Me.ContattoEliminato, AddressOf handleEliminaContatto
            AddHandler Me.ContattoModificato, AddressOf handleContattoModificato
            AddHandler Sistema.FaxService.FaxDelivered, AddressOf handleFaxDelivered
            AddHandler Sistema.FaxService.FaxFailed, AddressOf handleFaxFailed
            AddHandler Sistema.FaxService.FaxReceived, AddressOf handleFaxReceived

            Sistema.Types.RegisteredTypeProviders.Add("CTelefonata", AddressOf Me.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CVisita", AddressOf Me.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("SMSMessage", AddressOf Me.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("FaxDocument", AddressOf Me.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CAppunto", AddressOf Me.GetItemById)
        End Sub

        Public Function GetContattiByPersona(ByVal p As CPersona) As CCollection(Of CContattoUtente)
            Dim ret As New CCollection(Of CContattoUtente)
            Dim cursor As New CCustomerCallsCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.IDPersona.Value = GetID(p)
            While Not cursor.EOF
                Dim c As CContattoUtente = cursor.Item
                c.SetPersona(p)
                ret.Add(c)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret
        End Function


        Public Function GetUltimoContatto(ByVal p As CPersona) As minidom.CustomerCalls.CContattoUtente
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Return Me.GetUltimoContatto(GetID(p))
        End Function

        Public Function GetUltimoContatto(ByVal pID As Integer) As minidom.CustomerCalls.CContattoUtente
            If (pID = 0) Then Return Nothing
            Dim info As CPersonStats = Me.GetContattoInfo(pID)
            Dim ret As CContattoUtente = info.UltimoContattoOk
            If (ret Is Nothing) Then ret = info.UltimoContattoNo

            Return ret

             
        End Function

        Public Function GetContattoInfo(ByVal persona As CPersona) As CPersonStats
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ret As CPersonStats = Me.GetContattoInfo(GetID(persona))
            ret.SetPersona(persona)
            Return ret
        End Function

        Public Function GetContattoInfo(ByVal pID As Integer) As CPersonStats
            If (pID = 0) Then Return Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim dbSQL As String = "SELECT * FROM [tbl_UltimaChiamata] WHERE [IDPersona]=" & pID
                Dim ret As New CPersonStats
                ret.IDPersona = pID
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then CRM.StatsDB.Load(ret, dbRis)
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

        End Function

        ''' <summary>
        ''' Aggiorna la tabella delle statistiche impostando il contatto come ultimo contatto
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <param name="telefonata"></param>
        ''' <remarks></remarks>
        Public Sub SetUltimoContatto(ByVal persona As CPersona, ByVal telefonata As CContattoUtente)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            If (telefonata Is Nothing) Then Throw New ArgumentNullException("telefonata")

            Dim info As CPersonStats = Me.GetContattoInfo(persona)
            info.AggiornaContatto(telefonata)

        End Sub

        ''' <summary>
        ''' Restituisce o imposta il database principale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Database As CDBConnection
            Get
                If (Me.m_Database Is Nothing) Then Return APPConn
                Return Me.m_Database
            End Get
            Set(value As CDBConnection)
                If (Me.Database Is value) Then Exit Property
                Me.m_Database = value
                Me.OnDatabaseChanged(New System.EventArgs)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il database in cui vengono memorizzate le statistiche sul CRM.
        ''' Per impostazione predefinita viene utilizzato il valore restituito dalla propriet� TelDB della stessa classe
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatsDB As CDBConnection
            Get
                If (Me.m_StatsDB Is Nothing) Then Return Me.TelDB
                Return Me.m_StatsDB
            End Get
            Set(value As CDBConnection)
                If (Me.StatsDB Is value) Then Exit Property
                Me.m_StatsDB = value
                Me.OnDatabaseChanged(New System.EventArgs)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il database in cui vengono memorizzate telefonate.
        ''' Per impostazione predefinita viene utilizzato il vaore restituito dalla propriet� Database della stessa classe
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TelDB As CDBConnection
            Get
                If (Me.m_TelDB Is Nothing) Then Return Me.Database
                Return Me.m_TelDB
            End Get
            Set(value As CDBConnection)
                If (Me.TelDB Is value) Then Exit Property
                Me.m_TelDB = value
                Me.OnDatabaseChanged(New System.EventArgs)
            End Set
        End Property



        Protected Sub OnDatabaseChanged(ByVal e As System.EventArgs)
            RaiseEvent DatabaseChanged(Me, e)
        End Sub


        Private Sub handleFaxDelivered(ByVal sender As Object, ByVal e As FaxDeliverEventArgs)
            Dim cursor As FaxDocumentsCursor = Nothing

            Try
                Dim job As FaxJob = e.Job
                Dim fax As FaxDocument = Nothing

                If (job.JobID = "") Then Throw New ArgumentNullException("jobid")

                cursor = New FaxDocumentsCursor
                cursor.MessageID.Value = job.JobID
                cursor.IgnoreRights = True
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                fax = cursor.Item
                cursor.Dispose() : cursor = Nothing

                If (fax IsNot Nothing) Then
                    fax.StatoConversazione = StatoConversazione.CONCLUSO
                    fax.Esito = EsitoChiamata.RIPOSTA
                    fax.DettaglioEsito = "Inviato"
                    fax.Save()
                    fax.DataRicezione = e.Job.Date
                    Me.Module.DispatchEvent(New EventDescription("fax_sent", "Fax Inviato", fax))
                    Me.OnFaxSent(New ContattoEventArgs(fax))
                End If

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Protected Sub OnFaxSent(ByVal e As ContattoEventArgs)
            RaiseEvent FaxSent(Me, e)
        End Sub

        Private Sub handleFaxFailed(ByVal sender As Object, ByVal e As FaxJobEventArgs)
            Dim cursor As FaxDocumentsCursor = Nothing

            Try
                Dim job As FaxJob = e.Job
                Dim fax As FaxDocument = Nothing

                If (job.JobID = "") Then Throw New ArgumentNullException("jobid")

                cursor = New FaxDocumentsCursor
                cursor.MessageID.Value = job.JobID
                cursor.IgnoreRights = True
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                fax = cursor.Item
                cursor.Dispose() : cursor = Nothing

                If (fax IsNot Nothing) Then
                    fax.DettaglioEsito = e.Job.JobStatusMessage
                    fax.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    fax.StatoConversazione = StatoConversazione.CONCLUSO
                    fax.DataRicezione = e.Job.Date
                    fax.Save()
                    Me.Module.DispatchEvent(New EventDescription("fax_failed", "Invio Fax Fallito", fax))
                    Me.OnFaxFailed(New ContattoEventArgs(fax))
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Protected Sub OnFaxFailed(ByVal e As ContattoEventArgs)
            RaiseEvent FaxFailed(Me, e)
        End Sub

        Private faxLock As New Object


        Private Sub handleFaxReceived(ByVal sender As Object, ByVal e As FaxReceivedEventArgs)
            Dim fax As New FaxDocument
            Dim job As FaxJob = e.Job

            Dim att As New CAttachment
            att.Stato = Databases.ObjectStatus.OBJECT_VALID
            att.Testo = "Fax Ricevuto da " & job.Options.SenderName & " (" & job.Options.SenderNumber & ")"
            att.Tipo = "Fax"

            Dim path As String = "/public/Received Fax Documents/"
            Dim name As String
            Dim ext As String = minidom.Sistema.FileSystem.GetExtensionName(job.Options.FileName)

            SyncLock Me.faxLock
                minidom.Sistema.FileSystem.CreateRecursiveFolder(ApplicationContext.MapPath(path))

                name = Sistema.ASPSecurity.GetRandomKey(25)
                While minidom.Sistema.FileSystem.FileExists(ApplicationContext.MapPath(path & name & "." & ext))
                    name = Sistema.ASPSecurity.GetRandomKey(25)
                End While
                minidom.Sistema.FileSystem.MoveFile(job.Options.FileName, ApplicationContext.MapPath(path & name & "." & ext))
            End SyncLock

            att.URL = path & name & "." & ext
            att.Save()

            Dim driver As BaseFaxDriver = e.Job.Driver
            Dim modemName As String = e.Job.Options.ModemName
            Dim modem As FaxDriverModem = driver.GetModem(modemName)
            If (modem Is Nothing AndAlso driver.Modems.Count > 0) Then modem = driver.Modems(0)

            fax.Stato = Databases.ObjectStatus.OBJECT_VALID
            fax.StatoConversazione = StatoConversazione.CONCLUSO
            fax.Ricevuta = True
            fax.NomeIndirizzo = job.Options.SenderName
            fax.NumeroOIndirizzo = Formats.ParsePhoneNumber(job.Options.SenderNumber)
            fax.Data = job.Date
            fax.Operatore = Sistema.Users.KnownUsers.SystemUser
            fax.Azienda = Anagrafica.Aziende.AziendaPrincipale
            fax.Attachment = att
            fax.AccoltoDa = Sistema.Users.KnownUsers.SystemUser
            fax.DataRicezione = Now
            fax.Note = "Fax ricevuto da " & job.Options.SenderName
            fax.Esito = EsitoChiamata.RIPOSTA
            fax.DettaglioEsito = "Ricevuto"
            fax.Options.SetValueString("FaxDriver", driver.GetUniqueID)
            fax.Options.SetValueString("FaxModemName", modemName)
            fax.Options.SetValueString("FaxJob", e.Job.JobID)
            fax.Options.SetValueString("FaxRecipient", e.Job.Options.RecipientName)
            fax.Options.SetValueString("FaxSenderName", e.Job.Options.SenderName)
            fax.Options.SetValueString("FaxSenderNumber", e.Job.Options.SenderNumber)
            If modem IsNot Nothing Then fax.PuntoOperativo = modem.PuntoOperativo
            fax.Save()

            'Dim cursor As New CContattoCursor
            'cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID
            'cursor.Valore.Value = Formats.ParsePhoneNumber(fax.NumeroOIndirizzo)
            'cursor.IgnoreRights = True
            'Dim contatto As CContatto = cursor.Item
            'cursor.Dispose()

            'If (contatto IsNot Nothing) Then
            '    Dim p As CPersona = contatto.Persona
            '    If (p.Stato = Databases.ObjectStatus.OBJECT_VALID) Then

            '    End If
            'End If

            Me.Module.DispatchEvent(New EventDescription("fax_received", "Fax Ricevuto", fax))
            Me.OnFaxReceived(New ContattoEventArgs(fax))
        End Sub

        Protected Sub OnFaxReceived(ByVal e As ContattoEventArgs)
            RaiseEvent FaxReceived(Me, e)
        End Sub

        Private Sub handleNuovoContatto(ByVal e As CustomerCalls.ContattoEventArgs)
            Dim ufficio As CUfficio = e.Contatto.PuntoOperativo
            Dim operatore As CUser = e.Contatto.Operatore
            Dim data As Date = DateUtils.GetDatePart(e.Contatto.Data)
            Dim item As CRMStats = CRMStats.GetStats(ufficio, operatore, data)
            item.NotifyNew(e.Contatto)
            item.Save(True)
        End Sub

        Private Sub handleContattoModificato(ByVal e As CustomerCalls.ContattoEventArgs)
            Dim data As Date = DateUtils.GetDatePart(e.Contatto.Data)
            Dim idpo As Integer = e.Contatto.IDPuntoOperativo
            Dim op As Integer = e.Contatto.IDOperatore
            Try
                Me.StatsDB.ExecuteCommand("UPDATE [tbl_CRMStats] SET [Ricalcola]=TRUE WHERE [IDPuntoOperativo]=" & DBUtils.DBNumber(idpo) & " AND [idOperatore]=" & DBUtils.DBNumber(op) & " AND [Data]=" & DBUtils.DBDate(data))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            End Try
        End Sub

        Private Sub handleEliminaContatto(ByVal e As CustomerCalls.ContattoEventArgs)
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim item As New CRMStats
                Dim idPO As Integer = e.Contatto.IDPuntoOperativo
                Dim idOperatore As Integer = e.Contatto.IDOperatore
                Dim data As Date = DateUtils.GetDatePart(e.Contatto.Data)
                dbRis = Me.StatsDB.ExecuteReader("SELECT * FROM [tbl_CRMStats] WHERE [IDPuntoOperativo]=" & DBUtils.DBNumber(idPO) & " AND [idOperatore]=" & DBUtils.DBNumber(idOperatore) & " AND [Data]=" & DBUtils.DBDate(data))
                If (dbRis.Read) Then Me.StatsDB.Load(item, dbRis)
                item.IDOperatore = idOperatore
                item.Data = data

                item.Ricalcola = True
                item.Save(True)

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

        End Sub


        Public Overrides Function GetItemById(id As Integer) As CContattoUtente
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                If (id = 0) Then Return Nothing
                Dim dbSQL As String = "SELECT * FROM [tbl_Telefonate] WHERE [ID]=" & id
                Dim ret As CContattoUtente = Nothing
                dbRis = Me.TelDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret = Types.CreateInstance(Formats.ToString(dbRis("ClassName")))
                    Me.TelDB.Load(ret, dbRis)
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function


        Private m_CRMGroup As CGroup = Nothing

        Public ReadOnly Property CRMGroup As CGroup
            Get
                SyncLock Me
                    If (Me.m_CRMGroup Is Nothing) Then
                        Me.m_CRMGroup = Sistema.Groups.GetItemByName("CRM")
                    End If
                    If (Me.m_CRMGroup Is Nothing) Then
                        Me.m_CRMGroup = New CGroup("CRM")
                        Me.m_CRMGroup.Stato = ObjectStatus.OBJECT_VALID
                        Me.m_CRMGroup.Save()
                    End If
                    Return m_CRMGroup
                End SyncLock
            End Get
        End Property

        Friend Sub onNuovoContatto(ByVal e As ContattoEventArgs)
            If (e.Contatto.Persona IsNot Nothing) Then
                CRM.SetUltimoContatto(e.Contatto.Persona, e.Contatto)
            End If

            RaiseEvent NuovoContatto(e)
        End Sub

        Friend Sub onContattoEliminato(ByVal e As ContattoEventArgs)
            RaiseEvent ContattoEliminato(e)
        End Sub

        Friend Sub onContattoModificato(ByVal e As ContattoEventArgs)
            RaiseEvent ContattoModificato(e)
        End Sub

        Public Function GetContattiByContesto(ByVal tipoContesto As String, ByVal idContesto As Integer) As CCollection(Of CContattoUtente)
            Dim cursor As New CCustomerCallsCursor
            Try
                Dim ret As New CCollection(Of CContattoUtente)
                If (tipoContesto = "" AndAlso idContesto = 0) Then Return ret
                If (tipoContesto <> "") Then cursor.Contesto.Value = tipoContesto
                If (idContesto <> 0) Then cursor.IDContesto.Value = idContesto
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Data.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function ContaContattiInAttesa(ByVal po As Integer?, ByVal op As Integer?) As Integer
            Return Me.GetVisiteInAttesa(po, op).Count
        End Function

        Public Function GetTelefonateInAttesa(ByVal po As Integer?, ByVal op As Integer?) As CCollection(Of ContattoInAttesaInfo)
            Dim ret As New CCollection(Of ContattoInAttesaInfo)
            Dim items As CCollection(Of CTelefonata) = Telefonate.InAttesa
            Dim user As CUser

            If (op.HasValue = False) Then
                user = Sistema.Users.CurrentUser
            Else
                user = Sistema.Users.GetItemById(op.Value)
            End If

            If (po.HasValue) Then
                For Each v As CTelefonata In items
                    If (v.IDPuntoOperativo = 0) OrElse
                       ((po.Value = v.IDPuntoOperativo) AndAlso user.Uffici.HasOffice(v.IDPuntoOperativo)) Then
                        ret.Add(New ContattoInAttesaInfo(v))
                    End If
                Next
            Else
                For Each v As CTelefonata In items
                    ret.Add(New ContattoInAttesaInfo(v))
                Next
            End If


            Return ret
        End Function

        Public Function GetVisiteInAttesa(ByVal po As Integer?, ByVal op As Integer?) As CCollection(Of ContattoInAttesaInfo)
            Dim ret As New CCollection(Of ContattoInAttesaInfo)
            Dim items As CCollection(Of CVisita) = Visite.InAttesa
            Dim user As CUser
            If (op.HasValue = False) Then
                user = Sistema.Users.CurrentUser
            Else
                user = Sistema.Users.GetItemById(op.Value)
            End If

            If (po.HasValue) Then
                For Each v As CVisita In items
                    If (v.IDPuntoOperativo = 0) OrElse
                       ((po.Value = v.IDPuntoOperativo) AndAlso user.Uffici.HasOffice(v.IDPuntoOperativo)) Then
                        ret.Add(New ContattoInAttesaInfo(v))
                    End If
                Next
            Else
                For Each v As CVisita In items
                    ret.Add(New ContattoInAttesaInfo(v))
                Next
            End If

            Return ret
        End Function

        Private Sub HandlePeronaMerged(ByVal e As MergePersonaEventArgs)
            Dim dbRis As System.Data.IDataReader = Nothing

            SyncLock Anagrafica.lock
                Try
                    Dim mi As CMergePersona = e.MI
                    Dim persona As CPersona = mi.Persona1
                    Dim persona1 As CPersona = mi.Persona2

                    Dim dbSQL As String
                    Dim rec As CMergePersonaRecord


                    'Tabella tbl_Telefonate 
                    dbSQL = "SELECT [ID], [NomePersona] FROM [tbl_Telefonate] WHERE [IDPersona]=" & GetID(persona1)
                    dbRis = TelDB.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_Telefonate"
                        rec.FieldName = "IDPersona"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomePersona", Formats.ToString(dbRis("NomePersona")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose()
                    dbRis = Nothing
                    TelDB.ExecuteCommand("UPDATE [tbl_Telefonate] SET [IDPersona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona]=" & GetID(persona1))


                    Dim c1 As CPersonStats = CRM.GetContattoInfo(persona)
                    If (c1 IsNot Nothing) Then c1.Ricalcola()
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

            End SyncLock
        End Sub



        Private Sub HandlePeronaUnMerged(ByVal e As MergePersonaEventArgs)
            SyncLock Anagrafica.lock
                Dim mi As CMergePersona = e.MI
                Dim persona As CPersona = mi.Persona1
                Dim persona1 As CPersona = mi.Persona2
                Dim items As String

                'Tabella tbl_Telefonate
                items = mi.GetAffectedRecors("tbl_Telefonate", "IDPersona")
                If (items <> "") Then TelDB.ExecuteCommand("UPDATE [tbl_Telefonate] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

                ''Tabella tbl_TelefonateQuick
                'items = mi.GetAffectedRecors("tbl_TelefonateQuick", "IDPersona")
                'If (items <> "") Then TelDB.ExecuteCommand("UPDATE [tbl_TelefonateQuick] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")

                Dim c1 As CPersonStats = CRM.GetContattoInfo(persona)
                Dim c2 As CPersonStats = CRM.GetContattoInfo(persona1)
                If (c1 IsNot Nothing) Then c1.Ricalcola()
                If (c2 IsNot Nothing) Then c2.Ricalcola()
            End SyncLock
        End Sub


        Private Sub HandlePeronaModified(ByVal e As PersonaEventArgs)
            SyncLock Anagrafica.lock
                Dim p As CPersona = e.Persona
                Dim c As CPersonStats = CRM.GetContattoInfo(p)
                If (c IsNot Nothing) Then
                    c.AggiornaPersona()
                    c.Save()
                End If
            End SyncLock
        End Sub

        Private Sub HandleRicontattoModified(ByVal sender As Object, ByVal e As RicontattoEventArgs)
            Dim ric As CRicontatto = e.Ricontatto
            If (ric.Persona Is Nothing) Then Exit Sub
            Dim c As CPersonStats = CRM.GetContattoInfo(ric.Persona)
            If (c IsNot Nothing) Then
                c.AggiornaPersona()
                c.AggiornaProssimoRicontatto()
                c.Save()
            End If
        End Sub

        Private m_Config As CCRMConfig = Nothing


        ''' <summary>
        ''' Restituisce la configurazione del CRM
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Config As CCRMConfig
            Get
                SyncLock Me
                    If (Me.m_Config Is Nothing) Then
                        Me.m_Config = New CCRMConfig
                        Me.m_Config.Load()
                    End If
                    Return Me.m_Config
                End SyncLock
            End Get
        End Property

        Friend Sub SetConfig(ByVal value As CCRMConfig)
            SyncLock Me
                Me.m_Config = value
            End SyncLock
            Me.doConfigurationChanged(New System.EventArgs)
        End Sub

        Protected Sub doConfigurationChanged(ByVal e As System.EventArgs)
            RaiseEvent ConfigurationChanged(Me, e)
        End Sub

        Private m_Handlers As CCollection(Of IStoricoHandler)

        Public ReadOnly Property Handlers As CCollection(Of IStoricoHandler)
            Get
                SyncLock Me
                    If (Me.m_Handlers Is Nothing) Then Me.m_Handlers = New CCollection(Of IStoricoHandler)
                    Return Me.m_Handlers
                End SyncLock
            End Get
        End Property

        Public Function GetStoricoContatti(ByVal filter As CRMFindFilter) As CCollection(Of StoricoAction)
            Dim items As New CCollection(Of StoricoAction)
            If (filter.Dal.HasValue) Then filter.Dal = DateUtils.GetDatePart(filter.Dal)
            If (filter.Al.HasValue) Then filter.Al = DateUtils.DateAdd(DateInterval.Second, 24 * 3600 - 1, DateUtils.GetDatePart(filter.Al))

            For Each h As IStoricoHandler In Me.Handlers
#If (Not DEBUG) Then
                Try
#End If
                Dim t1 As Date = DateUtils.Now
                    h.Aggiungi(items, filter)
                    Dim t2 As Date = DateUtils.Now
                    If (t2 - t1).TotalMilliseconds > 1000 Then
                    Sistema.ApplicationContext.Log(TypeName(filter) & " - " & filter.ToString() & ": Troppo tempo " & ((t2 - t1).TotalMilliseconds / 1000) & " s")
                End If
#If (Not DEBUG) Then
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
#End If
            Next

            items.Sort()
            Return items
        End Function

        ''' <summary>
        ''' Restituisce la collezione dei tipi di oggetto supportati da GetStoricoContatti.
        ''' Le coppie restituite sono del indicano la classe ed il nome "amichevole" della classe
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetHandledTypes() As CKeyCollection(Of String)
            SyncLock Me
                Dim ret As New CKeyCollection(Of String)
                For Each h As IStoricoHandler In Me.Handlers
                    Dim tmp As CKeyCollection(Of String) = h.GetHandledTypes
                    For Each k As String In tmp.Keys
                        If ret.ContainsKey(k) = False Then ret.Add(k, tmp(k))
                    Next
                Next
                Return ret
            End SyncLock
        End Function

        ''' <summary>
        ''' Restituisce la collezione dei contatti fatti dall'operatore nel periodo indicato suddivisi per persona e raggruppati per tipoPeriodo
        ''' </summary>
        ''' <param name="tipoPeriodo"></param>
        ''' <param name="cursor"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AggregatoContatti(ByVal cursor As CCustomerCallsCursor, ByVal tipoPeriodo As String) As CKeyCollection(Of CPersonaXPeriodo)
            Dim tmp As New CKeyCollection(Of CPersonaXPeriodo)

            'cursor.ClassName .Value = "CTelefonata"
            Select Case LCase(Trim(tipoPeriodo))
                Case "anno"
                    Using dbRis As System.Data.IDataReader = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona], [NomePersona], [Esito], Year([Data]) as [Anno], Count(*) AS [CNT] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona], [NomePersona], [Esito], Year([Data])")
                        While dbRis.Read
                            Dim pid As Integer = Formats.ToInteger(dbRis("IDPersona"))
                            Dim nome As String = Formats.ToString(dbRis("NomePersona"))
                            Dim esito As Integer = Formats.ToInteger(dbRis("Esito"))
                            Dim anno As Integer = Formats.ToInteger(dbRis("Anno"))
                            Dim cnt As Integer = Formats.ToInteger(dbRis("CNT"))
                            Dim key As String = "P" & pid & "_" & anno
                            Dim item As CPersonaXPeriodo = tmp.GetItemByKey(key)
                            If (item Is Nothing) Then
                                item = New CPersonaXPeriodo
                                item.IDPersona = pid
                                item.NomePersona = nome
                                item.Data = DateUtils.MakeDate(anno, 1, 1)
                                tmp.Add(key, item)
                            End If
                            If (esito = 1) Then item.ConteggioRisposte += cnt
                            item.ConteggioChiamate += cnt
                        End While
                    End Using
                Case "mese"
                    Using dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona], [NomePersona], [Esito], Left([DataStr], 6) AS [Mese], Count(*) AS [CNT] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona], [NomePersona],[Esito], Left([DataStr], 6)")
                        While dbRis.Read
                            Dim pid As Integer = Formats.ToInteger(dbRis("IDPersona"))
                            Dim nome As String = Formats.ToString(dbRis("NomePersona"))
                            Dim esito As Integer = Formats.ToInteger(dbRis("Esito"))
                            Dim annomese As String = Formats.ToString(dbRis("Mese"))
                            Dim anno As Integer = Formats.ToInteger(Left(annomese, 4))
                            Dim mese As Integer = Formats.ToInteger(Mid(annomese, 5))
                            Dim cnt As Integer = Formats.ToInteger(dbRis("CNT"))
                            Dim key As String = "P" & pid & "_" & annomese
                            Dim item As CPersonaXPeriodo = tmp.GetItemByKey(key)
                            If (item Is Nothing) Then
                                item = New CPersonaXPeriodo
                                item.IDPersona = pid
                                item.NomePersona = nome
                                item.Data = DateUtils.MakeDate(anno, mese, 1)
                                tmp.Add(key, item)
                            End If
                            If (esito = 1) Then item.ConteggioRisposte += cnt
                            item.ConteggioChiamate += cnt
                        End While
                    End Using

                Case "giorno"
                    Using dbRis = CustomerCalls.CRM.TelDB.ExecuteReader("SELECT [IDPersona], [NomePersona], [Esito], Left([DataStr], 8) as [Data], Count(*) as [CNT] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona], [NomePersona],[Esito], Left([DataStr], 8)")
                        While dbRis.Read
                            Dim pid As Integer = Formats.ToInteger(dbRis("IDPersona"))
                            Dim nome As String = Formats.ToString(dbRis("NomePersona"))
                            Dim esito As Integer = Formats.ToInteger(dbRis("Esito"))
                            Dim annomesegiorno As String = Formats.ToString(dbRis("Data"))
                            Dim anno As Integer = Formats.ToInteger(Left(annomesegiorno, 4))
                            Dim mese As Integer = Formats.ToInteger(Mid(annomesegiorno, 5, 2))
                            Dim giorno As Integer = Formats.ToInteger(Mid(annomesegiorno, 7))
                            Dim cnt As Integer = Formats.ToInteger(dbRis("CNT"))
                            Dim key As String = "P" & pid & "_" & annomesegiorno
                            Dim item As CPersonaXPeriodo = tmp.GetItemByKey(key)
                            If (item Is Nothing) Then
                                item = New CPersonaXPeriodo
                                item.IDPersona = pid
                                item.NomePersona = nome
                                item.Data = DateUtils.MakeDate(anno, mese, giorno)
                                tmp.Add(key, item)
                            End If
                            If (esito = 1) Then item.ConteggioRisposte += cnt
                            item.ConteggioChiamate += cnt
                        End While
                    End Using
                Case Else
                    Throw New ArgumentOutOfRangeException("tipoPeriodo non supportato")
            End Select


            Return tmp ' New CCollection(Of CPersonaXPeriodo)(tmp)
        End Function

        ''' <summary>
        ''' Restituisce la collezione del utenti abilitati al CRM e visibili all'utente specificato
        ''' </summary>
        ''' <returns></returns>
        Public Function GetOperatoriCRM(ByVal u As CUser) As CCollection(Of CUser)
            Dim CRMGroup = Sistema.Groups.GetItemByName("CRM")
            Dim ret As New CKeyCollection()
            For Each ufficio As CUfficio In u.Uffici
                For Each user In ufficio.Utenti
                    If (user.Stato = ObjectStatus.OBJECT_VALID AndAlso Not ret.ContainsKey(user.UserName)) Then
                        If (CRMGroup.Members.GetItemById(GetID(user)) IsNot Nothing) Then ret.Add(user.UserName, user)
                    End If
                Next
            Next
            ret.Sort()
            Return New CCollection(Of CUser)(ret)
        End Function

        ''' <summary>
        ''' Restituisce la collezione del utenti abilitati al CRM e visibili all'utente corrente
        ''' </summary>
        ''' <returns></returns>
        Public Function GetOperatoriCRM() As CCollection(Of CUser)
            Return Me.GetOperatoriCRM(Sistema.Users.CurrentUser)
        End Function

        Public Function GetStatistichePerOperatore(ByVal cursor As CRMStatsCursor) As CCollection(Of CRMStats)
            Dim users As CCollection(Of CUser) = Me.GetOperatoriCRM()
            Dim lst As New List(Of Integer)(users.Count + 1)
            For i As Integer = 0 To users.Count - 1
                lst.Add(GetID(users(i)))
            Next

            Dim sum As CRMStats
            Dim ret As New CKeyCollection(Of CRMStats)
            cursor.IDOperatore.ValueIn(lst.ToArray())

            While Not cursor.EOF
                Dim item As CRMStats = cursor.Item
                Dim key As String = "K" & item.IDOperatore & "_" & item.Data
                sum = ret.GetItemByKey(key)
                If (sum Is Nothing) Then
                    ret.Add(key, item)
                Else
                    sum = sum.AggregaCon(item)
                    ret(key) = sum
                End If
                cursor.MoveNext()
            End While

            cursor.Dispose()

            Return New CCollection(Of CRMStats)(ret)
        End Function

        Public Function AggregaStatisticheOperatorePerData(ByVal cursor As CRMStatsCursor) As CCollection(Of CRMStats)
            Dim users As CCollection(Of CUser) = Me.GetOperatoriCRM()
            Dim lst As New List(Of Integer)(users.Count + 1)
            For i As Integer = 0 To users.Count - 1
                lst.Add(GetID(users(i)))
            Next

            Dim sum As CRMStats
            Dim ret As New CKeyCollection(Of CRMStats)
            cursor.IDOperatore.ValueIn(lst.ToArray())

            While Not cursor.EOF
                Dim item As CRMStats = cursor.Item
                Dim key As String = "K" & item.IDOperatore
                sum = ret.GetItemByKey(key)
                If (sum Is Nothing) Then
                    ret.Add(key, item)
                Else
                    sum = sum.AggregaCon(item)
                    ret(key) = sum
                End If
                cursor.MoveNext()
            End While

            cursor.Dispose()

            Return New CCollection(Of CRMStats)(ret)
        End Function

        Private Class SortCPXP
            Implements IComparer

            Public Sub New()
                DMDObject.IncreaseCounter(Me)
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMDObject.DecreaseCounter(Me)
            End Sub

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
                Dim a As CPersonaXPeriodo = x
                Dim b As CPersonaXPeriodo = y
                Return a.IDPersona.CompareTo(b.IDPersona)
            End Function

        End Class

        ''' <summary>
        ''' Ricalcola le statistiche CRM per la data specificata
        ''' </summary>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        Public Sub RicalcolaStatistiche(ByVal data As Date)
            data = DateUtils.GetDatePart(data)
            Dim toDate As Date = DateUtils.DateAdd(DateInterval.Second, 3600 * 24 - 1, data)
            Dim dbSQL As String = "SELECT [IDPuntoOperativo], [IDOperatore], [ClassName], [Ricevuta], Count(*) As [Num],  Min([Durata]) As [MinLen], Max([Durata]) As [MaxLen], Sum([Durata]) As [TotLen], Min([Attesa]) As [MinWait], Max([Attesa]) As [MaxWait], Sum([Attesa]) As [TotWait] FROM [tbl_Telefonate] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [DataStr]>='" & DBUtils.ToDBDateStr(data) & "' AND [DataStr]<'" & DBUtils.ToDBDateStr(toDate) & "' GROUP BY [IDPuntoOperativo], [IDOperatore], [ClassName], [Ricevuta]"
            Using dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                While (dbRis.Read)
                    Dim IDPuntoOperativo As Integer = Formats.ToInteger(dbRis("IDPuntoOperativo"))
                    Dim IDOperatore As Integer = Formats.ToInteger(dbRis("IDOperatore"))
                    Dim className As String = Formats.ToString(dbRis("ClassName"))
                    Dim ricevuta As Boolean = Formats.ToBool(dbRis("Ricevuta"))

                    Dim num As Integer = Formats.ToInteger(dbRis("Num"))
                    Dim MinLen As Double = Formats.ToDouble(dbRis("MinLen"))
                    Dim MaxLen As Double = Formats.ToDouble(dbRis("MaxLen"))
                    Dim TotLen As Double = Formats.ToDouble(dbRis("TotLen"))
                    Dim MinWait As Double = Formats.ToDouble(dbRis("MinWait"))
                    Dim MaxWait As Double = Formats.ToDouble(dbRis("MaxWait"))
                    Dim TotWait As Double = Formats.ToDouble(dbRis("TotWait"))
                    Dim item As CRMStats = Nothing

                    Using dbRis1 = Me.StatsDB.ExecuteReader("SELECT * FROM [tbl_CRMStats] WHERE [IDPuntoOperativo]=" & DBUtils.DBNumber(IDPuntoOperativo) & " AND [IDOperatore]=" & DBUtils.DBNumber(IDOperatore) & " AND [Data]=" & DBUtils.DBDate(data))
                        item = New CRMStats()
                        If (dbRis1.Read()) Then Me.StatsDB.Load(item, dbRis1)
                    End Using
                    If (item Is Nothing) Then Continue While

                    item.IDPuntoOperativo = IDPuntoOperativo
                    item.IDOperatore = IDOperatore
                    item.Data = data
                    If (ricevuta) Then
                        Select Case className
                            Case "CTelefonata"
                                item.InCallMaxLen = MaxLen
                                item.InCallMaxWait = MaxWait
                                item.InCallMinLen = MinLen
                                item.InCallMinWait = MinWait
                                item.InCallNum = num
                            Case "CVisita"
                                item.InDateMaxLen = MaxLen
                                item.InDateMaxWait = MaxWait
                                item.InDateMinLen = MinLen
                                item.InDateMinWait = MinWait
                                item.InDateNum = num
                            Case "SMSMessage"
                                item.InSMSNum = num

                            Case "FaxDocument"
                                item.InFAXNum = num

                        End Select
                    Else
                        Select Case className
                            Case "CTelefonata"
                                item.OutCallMaxLen = MaxLen
                                item.OutCallMaxWait = MaxWait
                                item.OutCallMinLen = MinLen
                                item.OutCallMinWait = MinWait
                                item.OutCallNum = num
                            Case "CVisita"
                                item.OutDateMaxLen = MaxLen
                                item.OutDateMaxWait = MaxWait
                                item.OutDateMinLen = MinLen
                                item.OutDateMinWait = MinWait
                                item.OutDateNum = num

                            Case "SMSMessage"
                                item.OutSMSNum = num

                            Case "FaxDocument"
                                item.OutFAXNum = num

                        End Select
                    End If
                    item.Save(True)

                End While
            End Using

        End Sub

    End Class

   
End Namespace

Public NotInheritable Class CustomerCalls


    Shared Sub New()
        
    End Sub


    Private Shared m_CRM As CCRMClass = Nothing

    Public Shared ReadOnly Property CRM As CCRMClass
        Get
            If (m_CRM Is Nothing) Then m_CRM = New CCRMClass
            Return m_CRM
        End Get
    End Property


End Class