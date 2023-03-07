Imports minidom
Imports minidom.Sistema
Imports minidom.Databases



Partial Public Class Anagrafica

    Public Class CPersonaCursor
        Inherits DBObjectCursorPO(Of CPersona)

        Private m_DettaglioEsito As New CCursorFieldObj(Of String)("DettaglioEsito")
        Private m_DettaglioEsito1 As New CCursorFieldObj(Of String)("DettagllioEsito")
        Private m_TipoPersona As New CCursorField(Of TipoPersona)("TipoPersona")
        Private m_Nominativo As New CCursorFieldObj(Of String)("Trim([Nome] & ' ' & [Cognome])")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Cognome As New CCursorFieldObj(Of String)("Cognome")
        Private m_Alias1 As New CCursorFieldObj(Of String)("Alias1")
        Private m_Alias2 As New CCursorFieldObj(Of String)("Alias2")
        Private m_Professione As New CCursorFieldObj(Of String)("Professione")
        Private m_Titolo As New CCursorFieldObj(Of String)("Titolo")
        Private m_Sesso As New CCursorFieldObj(Of String)("Sesso")
        Private m_FormaGiuridica As New CCursorFieldObj(Of String)("TipoAzienda")
        Private m_CodiceFiscale As New CCursorFieldObj(Of String)("CodiceFiscale")
        Private m_PartitaIVA As New CCursorFieldObj(Of String)("PartitaIVA")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_Deceduto As New CCursorField(Of Boolean)("Deceduto")
        Private m_NomeFonte As New CCursorFieldObj(Of String)("NomeFonte")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")

        Private m_IDStatoAttuale As New CCursorField(Of Integer)("IDStatoAttuale")

        Private m_DataNascita As New CCursorField(Of Date)("DataNascita")
        Private m_DataMorte As New CCursorField(Of Date)("DataMorte")

        Private m_NatoA_Citta As New CCursorFieldObj(Of String)("NatoA_Citta")
        Private m_NatoA_Provincia As New CCursorFieldObj(Of String)("NatoA_Provincia")

        Private m_MortoA_Citta As New CCursorFieldObj(Of String)("MortoA_Citta")
        Private m_MortoA_Provincia As New CCursorFieldObj(Of String)("MortoA_Provincia")

        Private m_ResidenteA_Citta As New CCursorFieldObj(Of String)("ResidenteA_Citta")
        Private m_ResidenteA_Provincia As New CCursorFieldObj(Of String)("ResidenteA_Provincia")
        Private m_ResidenteA_Via As New CCursorFieldObj(Of String)("ResidenteA_Via")
        Private m_ResidenteA_CAP As New CCursorFieldObj(Of String)("ResidenteA_CAP")

        Private m_DomiciliatoA_Citta As New CCursorFieldObj(Of String)("DomiciliatoA_Citta")
        Private m_DomiciliatoA_Provincia As New CCursorFieldObj(Of String)("DomiciliatoA_Provincia")
        Private m_DomiciliatoA_Via As New CCursorFieldObj(Of String)("DomiciliatoA_Via")
        Private m_DomiciliatoA_CAP As New CCursorFieldObj(Of String)("DomiciliatoA_CAP")

        
        Private m_Telefono As New CCursorFieldObj(Of String)("Telefono")
        Private m_eMail As New CCursorFieldObj(Of String)("e-Mail")
        Private m_WebSite As New CCursorFieldObj(Of String)("WebSite")

        Private m_Eta As New CCursorField(Of Double)("Eta")

        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")

        Private m_PFlags As New CCursorField(Of PFlags)("PFlags")
        Private m_NFlags As New CCursorField(Of PFlags)("NFlags")

        Private m_IDReferente1 As New CCursorField(Of Integer)("Referente1")
        Private m_IDReferente2 As New CCursorField(Of Integer)("Referente2")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDStatoAttuale As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoAttuale
            End Get
        End Property


        Public ReadOnly Property IDReferente1 As CCursorField(Of Integer)
            Get
                Return Me.m_IDReferente1
            End Get
        End Property

        Public ReadOnly Property IDReferente2 As CCursorField(Of Integer)
            Get
                Return Me.m_IDReferente2
            End Get
        End Property

        Public ReadOnly Property PFlags As CCursorField(Of PFlags)
            Get
                Return Me.m_PFlags
            End Get
        End Property

        Public ReadOnly Property NFlags As CCursorField(Of PFlags)
            Get
                Return Me.m_NFlags
            End Get
        End Property

        Public ReadOnly Property Eta As CCursorField(Of Double)
            Get
                Return Me.m_Eta
            End Get
        End Property

        Public ReadOnly Property Deceduto As CCursorField(Of Boolean)
            Get
                Return Me.m_Deceduto
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property DettaglioEsito As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsito
            End Get
        End Property

        Public ReadOnly Property DettaglioEsito1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsito1
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

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property Nominativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Cognome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Cognome
            End Get
        End Property

        Public ReadOnly Property Alias1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Alias1
            End Get
        End Property

        Public ReadOnly Property Alias2 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Alias2
            End Get
        End Property

        Public ReadOnly Property Professione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Professione
            End Get
        End Property

        Public ReadOnly Property Titolo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Titolo
            End Get
        End Property

        Public ReadOnly Property Sesso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sesso
            End Get
        End Property

        Public ReadOnly Property FormaGiuridica As CCursorFieldObj(Of String)
            Get
                Return Me.m_FormaGiuridica
            End Get
        End Property

        Public ReadOnly Property DataNascita As CCursorField(Of Date)
            Get
                Return Me.m_DataNascita
            End Get
        End Property

        Public ReadOnly Property DataMorte As CCursorField(Of Date)
            Get
                Return Me.m_DataMorte
            End Get
        End Property

        Public ReadOnly Property NatoA_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NatoA_Citta
            End Get
        End Property

        Public ReadOnly Property NatoA_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_NatoA_Provincia
            End Get
        End Property

        Public ReadOnly Property MortoA_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_MortoA_Citta
            End Get
        End Property

        Public ReadOnly Property MortoA_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_MortoA_Provincia
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Citta
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Provincia
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Via
            End Get
        End Property

        Public ReadOnly Property ResidenteA_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_CAP
            End Get
        End Property

        Public ReadOnly Property DomiciliatoA_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_DomiciliatoA_Citta
            End Get
        End Property

        Public ReadOnly Property DomiciliatoA_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_DomiciliatoA_Provincia
            End Get
        End Property

        Public ReadOnly Property DomiciliatoA_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_DomiciliatoA_Via
            End Get
        End Property

        Public ReadOnly Property DomiciliatoA_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_DomiciliatoA_CAP
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

        Public ReadOnly Property TipoPersona As CCursorField(Of TipoPersona)
            Get
                Return Me.m_TipoPersona
            End Get
        End Property

        Public ReadOnly Property Telefono As CCursorFieldObj(Of String)
            Get
                Return Me.m_Telefono
            End Get
        End Property




        Public ReadOnly Property eMail As CCursorFieldObj(Of String)
            Get
                Return Me.m_eMail
            End Get
        End Property

        Public ReadOnly Property WebSite As CCursorFieldObj(Of String)
            Get
                Return Me.m_WebSite
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            If dbRis IsNot Nothing Then
                Return Anagrafica.Persone.Instantiate(dbRis.GetValue(Of Integer)("TipoPersona", 0))
            Else
                Return Anagrafica.Persone.Instantiate(Me.TipoPersona.Value)
            End If
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Persone"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Persone.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetWhereFields()

            ret.Remove(Me.m_Nominativo)

            ret.RemoveByKey("ResidenteA_Citta")
            ret.RemoveByKey("ResidenteA_Provincia")
            ret.RemoveByKey("ResidenteA_Via")
            ret.RemoveByKey("ResidenteA_CAP")

            ret.RemoveByKey("DomiciliatoA_Citta")
            ret.RemoveByKey("DomiciliatoA_Provincia")
            ret.RemoveByKey("DomiciliatoA_Via")
            ret.RemoveByKey("DomiciliatoA_CAP")

            ret.RemoveByKey("Eta")

            ret.RemoveByKey("e-Mail")
            ret.RemoveByKey("Telefono")
            ret.RemoveByKey("WebSite")

            Return ret
        End Function

        Private Function IsResidenzaSet() As Boolean
            Return Me.m_ResidenteA_Citta.IsSet OrElse Me.m_ResidenteA_Provincia.IsSet OrElse Me.m_ResidenteA_CAP.IsSet OrElse Me.m_ResidenteA_Via.IsSet
        End Function

        Private Function IsDomicilioSet() As Boolean
            Return Me.m_DomiciliatoA_Citta.IsSet OrElse Me.m_DomiciliatoA_Provincia.IsSet OrElse Me.m_DomiciliatoA_CAP.IsSet OrElse Me.m_DomiciliatoA_Via.IsSet
        End Function

        Public Overrides Function GetWherePart() As String
            Dim ret As String = MyBase.GetWherePart()
            Dim w As String
            Dim buf As System.Text.StringBuilder

            If (Me.m_Nominativo.IsSet AndAlso Strings.Trim(Me.m_Nominativo.Value) <> "") Then
                Dim frs As CCollection(Of Internals.CIndexingService.CResult) = Sistema.IndexingService.Find(Me.m_Nominativo.Value, Nothing)
                buf = New System.Text.StringBuilder
                For Each o As Internals.CIndexingService.CResult In frs
                    If (buf.Length > 0) Then buf.Append(",")
                    buf.Append(o.OwnerID)
                Next
                If (buf.Length > 0) Then
                    w = "[ID] In (" & buf.ToString & ")"
                Else
                    w = "(0<>0)"
                End If
                ret = Strings.Combine(ret, w, " AND ")
            End If

            If (Me.m_Eta.IsSet) Then
                Dim d As Date = DateUtils.Now
                Dim d1 As Date?
                Dim d2 As Date?
                If (Me.m_Eta.Value.HasValue) Then d1 = DateUtils.DateAdd(DateInterval.Year, -Me.m_Eta.Value.Value, d)
                If (Me.m_Eta.Value1.HasValue) Then
                    d2 = DateUtils.DateAdd(DateInterval.Year, -Me.m_Eta.Value1.Value, d)
                    d2 = DateUtils.GetLastSecond(d2)
                End If
                w = ""
                Select Case Me.m_Eta.Operator
                    Case OP.OP_BETWEEN
                        w = " [DataNascita]>=" & DBUtils.DBDate(d1) & " AND [DataNascita]<" & DBUtils.DBDate(d2)
                    Case OP.OP_EQ, OP.OP_LIKE
                        d1 = DateUtils.GetDatePart(d1)
                        d2 = d1
                        d1 = DateUtils.DateAdd(DateInterval.Year, -1, d1)
                        d2 = DateUtils.DateAdd(DateInterval.Year, 1, d1)
                        d2 = DateUtils.GetLastSecond(d2)
                        w = "[DataNascita]>" & DBUtils.DBDate(d1) & " AND [DataNascita]<=" & DBUtils.DBDate(d2)
                    Case OP.OP_GE
                        d2 = DateUtils.DateAdd(DateInterval.Year, 1, d1)
                        d2 = DateUtils.GetLastSecond(d2)
                        w = "[DataNascita]<" & DBUtils.DBDate(d2)
                    Case OP.OP_GT
                        d1 = DateUtils.GetDatePart(d1)
                        d1 = DateUtils.DateAdd(DateInterval.Year, -1, d1)
                        d1 = DateUtils.DateAdd(DateInterval.Day, -1, d1)
                        d1 = DateUtils.GetLastSecond(d1)
                        w = "[DataNascita]<=" & DBUtils.DBDate(d1)
                    Case OP.OP_LE
                        d1 = DateUtils.GetDatePart(d1)
                        w = "[DataNascita]>=" & DBUtils.DBDate(d1)
                    Case OP.OP_LT
                        d1 = DateUtils.DateAdd(DateInterval.Day, 1, DateUtils.GetDatePart(d1))
                        w = "[DataNascita]>=" & DBUtils.DBDate(d1)
                    Case OP.OP_NE, OP.OP_NOTLIKE
                        d1 = DateUtils.GetDatePart(d1)
                        d2 = d1
                        d1 = DateUtils.DateAdd(DateInterval.Year, -1, d1)
                        d2 = DateUtils.DateAdd(DateInterval.Year, 1, d1)
                        d2 = DateUtils.GetLastSecond(d2)
                        w = "([DataNascita]<=" & DBUtils.DBDate(d1) & " OR [DataNascita]>" & DBUtils.DBDate(d2) & ")"
                End Select
                ret = Strings.Combine(ret, w, " AND ")
            End If
            Return ret
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
                If Me.Module.UserCanDoAction("list_assegnati") Then
                    If tmpSQL.Length > 0 Then tmpSQL.Append(" OR ")
                    tmpSQL.Append("([IDReferente1]=" & GetID(Sistema.Users.CurrentUser) & ")")
                    tmpSQL.Append(" OR ")
                    tmpSQL.Append("([IDReferente2]=" & GetID(Sistema.Users.CurrentUser) & ")")
                End If
                If tmpSQL.Length = 0 Then tmpSQL.Append("(0<>0)")
            End If
            Return tmpSQL.ToString
        End Function

        Protected Overrides Function GetSortFields() As CKeyCollection(Of CCursorField)
            Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetSortFields()

            ret.RemoveByKey("ResidenteA_Citta")
            ret.RemoveByKey("ResidenteA_Provincia")
            ret.RemoveByKey("ResidenteA_Via")
            ret.RemoveByKey("ResidenteA_CAP")

            ret.RemoveByKey("DomiciliatoA_Citta")
            ret.RemoveByKey("DomiciliatoA_Provincia")
            ret.RemoveByKey("DomiciliatoA_Via")
            ret.RemoveByKey("DomiciliatoA_CAP")

            ret.RemoveByKey("Eta")

            ret.RemoveByKey("e-Mail")
            ret.RemoveByKey("Telefono")
            ret.RemoveByKey("WebSite")

            Return ret
        End Function

        Public Overrides Function GetSQL() As String
            Dim ret As New System.Text.StringBuilder
            Dim wherePart1 As String = ""
            Dim wherePart2 As String = ""

            Dim orPart As String = ""

            If (Me.IsResidenzaSet OrElse Me.IsDomicilioSet) Then
                wherePart1 = "" : wherePart2 = "" : orPart = ""
                If (Me.IsResidenzaSet) Then
                    wherePart1 = "[TipoIndirizzo]='Residenza'"
                    If (Me.m_ResidenteA_CAP.IsSet) Then wherePart1 = Strings.Combine(wherePart1, Me.m_ResidenteA_CAP.GetSQL("CAP"), " AND ")
                    If (Me.m_ResidenteA_Citta.IsSet) Then wherePart1 = Strings.Combine(wherePart1, Me.m_ResidenteA_Citta.GetSQL("Citta"), " AND ")
                    If (Me.m_ResidenteA_Provincia.IsSet) Then wherePart1 = Strings.Combine(wherePart1, Me.m_ResidenteA_Provincia.GetSQL("Provincia"), " AND ")
                    If Me.m_ResidenteA_Via.IsSet Then
                        Dim via As String = Me.m_ResidenteA_Via.Value
                        Dim tmp As New CIndirizzo
                        tmp.ToponimoViaECivico = via
                        If (tmp.Toponimo <> "") Then
                            wherePart1 = Strings.Combine(wherePart1, "[Toponimo]=" & DBUtils.DBString(tmp.Toponimo), " AND ")
                        End If
                        If (tmp.Via <> "") Then
                            Me.m_ResidenteA_Via.Value = tmp.Via
                            wherePart1 = Strings.Combine(wherePart1, Me.m_ResidenteA_Via.GetSQL("[Via]"), " AND ")
                            Me.m_ResidenteA_Via.Value = via
                        End If
                        If (tmp.Civico <> "") Then
                            wherePart1 = Strings.Combine(wherePart1, "[Civico]=" & DBUtils.DBString(tmp.Civico), " AND ")
                        End If
                    End If
                End If
                If (Me.IsDomicilioSet) Then
                    wherePart2 = "[TipoIndirizzo]='Domicilio'"
                    If (Me.m_DomiciliatoA_CAP.IsSet) Then wherePart2 = Strings.Combine(wherePart2, Me.m_DomiciliatoA_CAP.GetSQL("CAP"), " AND ")
                    If (Me.m_DomiciliatoA_Citta.IsSet) Then wherePart2 = Strings.Combine(wherePart2, Me.m_DomiciliatoA_Citta.GetSQL("Citta"), " AND ")
                    If (Me.m_DomiciliatoA_Provincia.IsSet) Then wherePart2 = Strings.Combine(wherePart2, Me.m_DomiciliatoA_Provincia.GetSQL("Provincia"), " AND ")
                    If (Me.m_DomiciliatoA_Via.IsSet) Then
                        'wherePart2 = Strings.Combine(wherePart2, Me.m_DomiciliatoA_Via.GetSQL("Via"), " AND ")
                        Dim via As String = Me.m_DomiciliatoA_Via.Value
                        Dim tmp As New CIndirizzo
                        tmp.ToponimoViaECivico = via
                        If (tmp.Toponimo <> "") Then
                            wherePart2 = Strings.Combine(wherePart2, "[Toponimo]=" & DBUtils.DBString(tmp.Toponimo), " AND ")
                        End If
                        If (tmp.Via <> "") Then
                            Me.m_DomiciliatoA_Via.Value = tmp.Via
                            wherePart2 = Strings.Combine(wherePart2, Me.m_DomiciliatoA_Via.GetSQL("[Via]"), " AND ")
                            Me.m_DomiciliatoA_Via.Value = via
                        End If
                        If (tmp.Civico <> "") Then
                            wherePart2 = Strings.Combine(wherePart2, "[Civico]=" & DBUtils.DBString(tmp.Civico), " AND ")
                        End If
                    End If
                End If

                orPart = Strings.Combine(wherePart1, wherePart2, " AND ", True)

                ret.Append("SELECT [tbl_Persone].* FROM [tbl_Persone] INNER JOIN (")
                ret.Append("SELECT [Persona] AS [IDPersonaInd] FROM [tbl_Indirizzi] WHERE ")
                ret.Append(orPart)
                ret.Append(" GROUP BY [Persona]) AS [T1] ON [tbl_Persone].[ID] = [T1].[IDPersonaInd] ")

                wherePart1 = Me.GetWherePart
                If (wherePart1 <> "") Then
                    ret.Append(" WHERE ")
                    ret.Append(wherePart1)
                End If
            Else
                ret.Append(MyBase.GetSQL())
            End If

            If (Me.m_Telefono.IsSet OrElse Me.m_eMail.IsSet OrElse Me.m_WebSite.IsSet) Then
                Dim inSQL As String = ret.ToString

                orPart = ""
                If (Me.m_Telefono.IsSet) Then
                    wherePart1 = "LCase([Tipo]) In ('telefono', 'fax', 'cellulare') AND " & Me.m_Telefono.GetSQL("Valore")
                    orPart = wherePart1
                End If
                If (Me.m_eMail.IsSet) Then
                    wherePart1 = "LCase([Tipo]) In ('e-mail', 'pec') AND " & Me.m_eMail.GetSQL("Valore")
                    orPart = Strings.Combine(orPart, wherePart1, " OR ")
                End If
                If (Me.m_WebSite.IsSet) Then
                    wherePart1 = "LCase([Tipo])='website' AND " & Me.m_WebSite.GetSQL("Valore")
                    orPart = Strings.Combine(orPart, wherePart1, " OR ")
                End If

                ret.Clear()
                ret.Append("SELECT [T2].* FROM (")
                ret.Append(inSQL)
                ret.Append(") AS [T2] INNER JOIN (")
                ret.Append("SELECT [Persona] As [IDPersonaT] FROM [tbl_Contatti] WHERE [Stato]=")
                ret.Append(ObjectStatus.OBJECT_VALID)
                ret.Append(" AND (")
                ret.Append(orPart)
                ret.Append(")")
                ret.Append(" GROUP BY [Persona]) AS [T3] ON [T2].[ID]=[T3].[IDPersonaT]")
            End If

            Return ret.ToString
        End Function



    End Class

End Class