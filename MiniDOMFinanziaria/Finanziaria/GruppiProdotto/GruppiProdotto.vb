﻿Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Internals
Imports minidom.Finanziaria

Namespace Internals


    ''' <summary>
    ''' Gruppi prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CGruppiProdottoClass
        Inherits CModulesClass(Of CGruppoProdotti)


        Friend Sub New()
            MyBase.New("modProdGrp", GetType(CGruppoProdottiCursor), -1)
        End Sub



        ''' <summary>
        ''' Restituisce il gruppo prodotto in base al suo nome (la ricerca è limitata ai soli gruppi validi)
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CGruppoProdotti
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CGruppoProdotti In Me.LoadAll
                If (Strings.Compare(ret.Descrizione, value) = 0) Then Return ret
            Next
            Return Nothing
        End Function

        Friend Sub InvalidateTipiProvvigione()
            For Each g As CGruppoProdotti In Me.LoadAll
                g.InvalidateTipiProvvigione
            Next
        End Sub

    End Class

End Namespace

Partial Public Class Finanziaria

    Private Shared m_GruppiProdotto As CGruppiProdottoClass = Nothing

    Public Shared ReadOnly Property GruppiProdotto As CGruppiProdottoClass
        Get
            If m_GruppiProdotto Is Nothing Then m_GruppiProdotto = New CGruppiProdottoClass
            Return m_GruppiProdotto
        End Get
    End Property


End Class