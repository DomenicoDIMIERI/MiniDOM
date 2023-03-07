﻿Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria

    ''' <summary>
    ''' Collezione di convenzioni associate ad un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CProdottoXConvenzioneCollection
        Inherits CCollection(Of CProdottoXConvenzione)

        <NonSerialized> Private m_Prodotto As CCQSPDProdotto

        Public Sub New()
            Me.m_Prodotto = Nothing
        End Sub

        Public Sub New(ByVal prodotto As CCQSPDProdotto)
            Me.New
            Me.Load(prodotto)
        End Sub

        Public Function Create(ByVal nome As String, ByVal convenzione As CQSPDConvenzione) As CProdottoXConvenzione
            Dim item As CProdottoXConvenzione = Me.GetItemByName(nome)
            If (item IsNot Nothing) Then Throw New DuplicateNameException("nome")
            item = New CProdottoXConvenzione
            With item
                .Nome = nome
                .Convenzione = convenzione
                .Stato = ObjectStatus.OBJECT_VALID
            End With
            Me.Add(item)
            item.Save()
            Return item
        End Function

        Public Function GetItemByName(ByVal nome As String) As CProdottoXConvenzione
            For Each rel As CProdottoXConvenzione In Me
                If (Strings.Compare(rel.Nome, nome) = 0) Then Return rel
            Next
            Return Nothing
        End Function

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Prodotto IsNot Nothing) Then DirectCast(value, CProdottoXConvenzione).SetProdotto(Me.m_Prodotto)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Prodotto IsNot Nothing) Then DirectCast(newValue, CProdottoXConvenzione).SetProdotto(Me.m_Prodotto)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Sub Load(ByVal prodotto As CCQSPDProdotto)
            If (prodotto Is Nothing) Then Throw New ArgumentNullException("prodotto")
            Me.Clear()
            Me.SetProdotto(prodotto)
            If (GetID(prodotto) = 0) Then Exit Sub
            Dim cursor As CProdottoXConvenzioneCursor = Nothing
#If Not DEBUG Then
            try
#End If
            cursor = New CProdottoXConvenzioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDProdotto.Value = GetID(prodotto)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            Me.Sort()
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Protected Friend Overridable Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            If (value IsNot Nothing) Then
                For Each item As CProdottoXConvenzione In Me
                    item.SetProdotto(value)
                Next
            End If
        End Sub


    End Class

End Class