Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals



    ''' <summary>
    ''' Modulo per la gestione dei conteggi estintivi
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CConteggiEstintiviClass
        Inherits CModulesClass(Of CConteggioEstintivo)

        Public Event Segnalata(ByVal sender As Object, ByVal e As ItemEventArgs)

        Public Event PresaInCarico(ByVal sender As Object, ByVal e As ItemEventArgs)

        'Public Event Richiesta(ByVal sender As Object, ByVal e As ItemEventArgs)


        Public Sub New()
            MyBase.New("modCQSPDConteggiEstintivi", GetType(CConteggioEstintivoCursor), 0)
        End Sub

        Friend Sub doOnSegnalata(ByVal e As ItemEventArgs)
            Dim richiesta As CConteggioEstintivo = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                    cliente.SetFlag(PFlags.Cliente, True)
                    cliente.Save()
                End If
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.isClienteInAcquisizione = False
                info.isClienteAcquisito = True
                info.AggiungiAttenzione(richiesta, "Richiesta CE non ancora presa in carico", "Richiesta CE " & GetID(richiesta))
                info.AggiornaOperazione(richiesta, "Richiesta CE Segnalata")
            End If

            RaiseEvent Segnalata(Me, e)
            Me.Module.DispatchEvent(New EventDescription("segnalata", "Richiesta CE Segnalata", e.Item))
        End Sub

        Friend Sub doOnPresaInCarico(ByVal e As ItemEventArgs)
            Dim richiesta As CConteggioEstintivo = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                    cliente.SetFlag(PFlags.Cliente, True)
                    cliente.Save()
                End If
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.isClienteInAcquisizione = False
                info.isClienteAcquisito = True
                info.RimuoviAttenzione(richiesta, "Richiesta CE " & GetID(richiesta))
                info.AggiornaOperazione(richiesta, "Richiesta CE Presa in carico da " & richiesta.PresaInCaricoDaNome)
            End If

            RaiseEvent PresaInCarico(Me, e)
            Me.Module.DispatchEvent(New EventDescription("presaincarico", "Richiesta CE Presa In Carico", e.Item))
        End Sub

        Function ParseTemplate(template As String, richiesta As CConteggioEstintivo, ByVal context As CKeyCollection) As String
            Dim currentUser As CUser = context("CurrentUser")
            Dim baseURL As String = context("BaseURL")

            template = Replace(template, "%%ID%%", GetID(richiesta))
            template = Replace(template, "%%NUMEROPRATICA%%", richiesta.NumeroPratica)
            template = Replace(template, "%%USERNAME%%", currentUser.Nominativo)
            template = Replace(template, "%%NOMINATIVOCLIENTE%%", richiesta.NomeCliente)
            template = Replace(template, "%%NOMEISTITUTO%%", richiesta.NomeIstituto)
            template = Replace(template, "%%IDCLIENTE%%", richiesta.IDCliente)
            template = Replace(template, "%%NOMEPRESAINCARICODA%%", richiesta.PresaInCaricoDaNome)
            template = Replace(template, "%%BASEURL%%", baseURL)

            Return template
        End Function

        Protected Friend Shadows Sub doItemCreated(e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

        ''' <summary>
        ''' Restituisce tutte le richieste di conteggio effettuate per il cliente specificato
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Function GetConteggiByPersona(ByVal value As CPersona) As CCollection(Of CConteggioEstintivo)
            If (value Is Nothing) Then Throw New ArgumentNullException("Persona")
            Dim ret As New CCollection(Of CConteggioEstintivo)
            If (GetID(value) = 0) Then Return ret
            Using cursor As New CConteggioEstintivoCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDCliente.Value = GetID(value)
                cursor.DataRichiesta.SortOrder = SortEnum.SORT_ASC
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Dim r As CConteggioEstintivo = cursor.Item
                    r.SetCliente(value)
                    ret.Add(r)
                    cursor.MoveNext()
                End While
            End Using
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce l'ultima richiesta di finanziamento fatta dal cliente specificato
        ''' </summary>
        ''' <param name="p">[in] Persona</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUltimoConteggio(ByVal p As CPersonaFisica) As CConteggioEstintivo
            If (p Is Nothing) Then Throw New ArgumentNullException("persona")
            If (GetID(p) = 0) Then Return Nothing
            Using cursor As New CConteggioEstintivoCursor()
                cursor.IDCliente.Value = GetID(p)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.DataRichiesta.SortOrder = SortEnum.SORT_DESC
                cursor.PageSize = 1
                cursor.IgnoreRights = True
                Dim ret As CConteggioEstintivo = cursor.Item
                If (ret IsNot Nothing) Then ret.SetCliente(p)
                Return ret
            End Using
        End Function

        Public Overrides Sub Initialize()
            MyBase.Initialize()

            Dim table As CDBTable
            Dim col As CDBEntityField

            table = Finanziaria.Database.Tables.GetItemByKey("tbl_RichiesteFinanziamentiC")

            col = table.Fields.Alter("IDEstinzione", GetType(Integer))
            col = table.Fields.Alter("IDDOCConteggio", GetType(Integer))

            table.Update()
        End Sub

    End Class


End Namespace

Partial Public Class Finanziaria

    Private Shared m_ConteggiEstintivi As CConteggiEstintiviClass = Nothing

    ''' <summary>
    ''' Accede al modulo dei conteggi estintivi
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ConteggiEstintivi As CConteggiEstintiviClass
        Get
            If m_ConteggiEstintivi Is Nothing Then m_ConteggiEstintivi = New CConteggiEstintiviClass()
            Return m_ConteggiEstintivi
        End Get
    End Property
End Class
