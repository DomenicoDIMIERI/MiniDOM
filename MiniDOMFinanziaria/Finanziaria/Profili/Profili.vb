Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals

    <Serializable>
    Public NotInheritable Class CProfiliClass
        Inherits CModulesClass(Of CProfilo)

        Friend Sub New()
            MyBase.New("modPreventivatori", GetType(CProfiliCursor), -1)
        End Sub



        'Public Function CreateNew(ByVal profilo As String) As CProfilo
        '    Dim item As New CProfilo
        '    item.Profilo = profilo
        '    item.ProfiloVisibile = profilo
        '    item.Save()
        '    Return item
        'End Function

        Public Sub SetPreventivatoreUtente(ByVal userID As Integer, ByVal prevID As Integer, ByVal allow As Boolean)
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            dbSQL = "SELECT * FROM [tbl_PreventivatoriXUser] WHERE ([Utente]=" & userID & ") And ([Preventivatore]=" & prevID & ");"
            dbRis = Finanziaria.Database.ExecuteReader(dbSQL)
            If dbRis.Read = False Then
                dbRis.Dispose()
                dbSQL = "INSERT INTO [tbl_PreventivatoriXUser] ([Utente], [Preventivatore], [Allow]) VALUES (" & userID & ", " & prevID & ", " & DBUtils.DBBool(allow) & ");"
                Finanziaria.Database.ExecuteCommand(dbSQL)
            Else
                dbRis.Dispose()
                dbSQL = "UPDATE [tbl_PreventivatoriXUser] SET [Allow]=" & DBUtils.DBBool(allow) & " WHERE ([Utente]=" & userID & ") And ([Preventivatore]=" & prevID & ");"
                Finanziaria.Database.ExecuteCommand(dbSQL)
            End If
            dbRis = Nothing
        End Sub


        Public Function GetItemByName(ByVal value As String) As CProfilo
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CProfilo In Me.LoadAll
                If (Strings.Compare(ret.Nome, value) = 0) Then Return ret
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente gli oggetti preventivatori consentiti all'utente ed ai gruppi a cui appartiene l'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPreventivatoriUtente(Optional ByVal onlyValid As Boolean = True) As CCollection(Of CProfilo)
            Dim items As New CCollection(Of CProfilo)
            For Each p As CProfilo In Me.LoadAll
                If p.IsVisibleToUser(Sistema.Users.CurrentUser) Then
                    If (onlyValid) Then
                        If p.IsValid Then items.Add(p)
                    Else
                        items.Add(p)
                    End If
                End If
            Next
            Return items
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente gli oggetti preventivatori consentiti all'utente ed ai gruppi a cui appartiene l'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPreventivatoriUtente(ByVal dataDecorrenza As Date, Optional ByVal onlyValid As Boolean = True) As CCollection(Of CProfilo)
            Dim items As New CCollection(Of CProfilo)
            For Each p As CProfilo In Me.LoadAll
                If p.IsVisibleToUser(Sistema.Users.CurrentUser) Then
                    If (onlyValid) Then
                        If p.IsValid(dataDecorrenza) Then items.Add(p)
                    Else
                        items.Add(p)
                    End If
                End If
            Next
            Return items
        End Function

        Public Function GetPreventivatoriUtenteOffline() As CCollection(Of CProfilo)
            Dim items As CCollection(Of CProfilo) = GetPreventivatoriUtente()
            Dim ret As New CCollection(Of CProfilo)
            For i As Integer = 0 To items.Count - 1
                Dim item As CProfilo = items(i)
                If item.IsOffline Then ret.Add(item)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un array contenente l'elenco dei preventivatori consentiti o negati al solo utente (senza considerare i gruppi 
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="items"></param>
        ''' <param name="arrIDs"></param>
        ''' <param name="arrAllow"></param>
        ''' <param name="arrNegate"></param>
        ''' <remarks></remarks>
        Public Sub GetPreventivatoriPerUtente(
                            ByVal user As CUser,
                            ByRef items() As CProfilo,
                            ByRef arrIDs() As Integer,
                            ByRef arrAllow() As Boolean,
                            ByRef arrNegate() As Boolean
                            )
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Me.GetPreventivatoriPerUtente(GetID(user), items, arrIDs, arrAllow, arrNegate)
        End Sub

        ''' <summary>
        ''' Restituisce un array contenente l'elenco dei preventivatori consentiti o negati al solo utente (senza considerare i gruppi 
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="items"></param>
        ''' <param name="arrIDs"></param>
        ''' <param name="arrAllow"></param>
        ''' <param name="arrNegate"></param>
        ''' <remarks></remarks>
        Public Sub GetPreventivatoriPerUtente(
                            ByVal userID As Integer,
                            ByRef items() As CProfilo,
                            ByRef arrIDs() As Integer,
                            ByRef arrAllow() As Boolean,
                            ByRef arrNegate() As Boolean
                            )

            Erase items
            Erase arrIDs
            Erase arrAllow
            Erase arrNegate

            If (userID = 0) Then Return

            Dim dbSQL As String = "SELECT Count(*) FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" & userID & " AND [Stato]=" & ObjectStatus.OBJECT_VALID
            Dim numItems As Integer = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM (" & dbSQL & ")"))
            If (numItems > 0) Then
                ReDim items(numItems - 1)
                ReDim arrAllow(numItems - 1)
                ReDim arrNegate(numItems - 1)
                ReDim arrIDs(numItems - 1)

                Dim dbRis1 As System.Data.IDataReader = Nothing
                dbSQL = "SELECT * FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" & userID & " AND [Stato]=" & ObjectStatus.OBJECT_VALID
                Try
                    dbRis1 = Finanziaria.Database.ExecuteReader(dbSQL & " ORDER BY [NomeCessionario] ASC, [Nome] ASC, [Profilo] ASC")
                    Dim reader As New DBReader(dbRis1)
                    Dim i As Integer = 0
                    While reader.Read
                        items(i) = Me.GetItemById(Formats.ToInteger(dbRis1("Preventivatore"))) ' CProfilo
                        If (items(i).Stato = ObjectStatus.OBJECT_VALID) Then
                            'Finanziaria.Database.Load(items(i), reader)
                            arrIDs(i) = reader.Read("IDAuth", arrIDs(i))
                            arrAllow(i) = reader.Read("Allow", arrAllow(i))
                            arrNegate(i) = reader.Read("Negate", arrNegate(i))
                            i = i + 1
                        End If
                    End While
                    reader.Dispose()
                    reader = Nothing
                    dbRis1 = Nothing
                Catch ex As Exception
                    Throw
                Finally
                    If (dbRis1 IsNot Nothing) Then dbRis1.Dispose()
                    dbRis1 = Nothing

                End Try
            End If

        End Sub

        Friend Sub InvalidateProdottiProfilo()
            For Each ci As CacheItem In Me.CachedItems
                DirectCast(ci.Item, CProfilo).InvalidateProdottiProfilo()
            Next
        End Sub
    End Class

End Namespace

Partial Public Class Finanziaria

    Private Shared m_Profili As CProfiliClass = Nothing

    Public Shared ReadOnly Property Profili As CProfiliClass
        Get
            If (m_Profili Is Nothing) Then m_Profili = New CProfiliClass
            Return m_Profili
        End Get
    End Property

End Class