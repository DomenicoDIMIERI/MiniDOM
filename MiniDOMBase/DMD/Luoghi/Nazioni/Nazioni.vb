Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Anagrafica

    <Serializable>
    Public NotInheritable Class CNazioniClass
        Inherits CModulesClass(Of CNazione)

        Friend Sub New()
            MyBase.New("modNazioni", GetType(CNazioniCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As CNazione
            name = Trim(name)
            If (name = "") Then Return Nothing
            For Each item As CNazione In Me.LoadAll
                If (Strings.Compare(item.Nome, name) = 0) Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByCodiceCatastale(code As String) As CNazione
            code = Trim(code)
            If (code = "") Then Return Nothing
            For Each c As CNazione In Me.LoadAll
                If (Strings.Compare(c.CodiceCatasto, code) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Private Function Find_Compare(ByVal a As String, ByVal b As String, ByVal strict As Boolean) As Boolean
            If (strict) Then Return Strings.InStr(a, b, CompareMethod.Text) > 0
            a = Strings.OnlyCharsAndNumbers(a)
            b = Strings.OnlyCharsAndNumbers(b)
            Return Strings.InStr(a, b, CompareMethod.Text) > 0
        End Function

        Public Function Find(ByVal value As String, Optional ByVal strict As Boolean = False) As CCollection(Of Luogo)
            Dim col As New CCollection(Of Luogo)
            Dim citta As String = Luoghi.GetComune(value)
            Dim provincia As String = Luoghi.GetProvincia(value)

            For Each n As CNazione In Me.LoadAll
                If (Me.Find_Compare(n.Nome, citta, strict)) Then
                    col.Add(n)
                End If
            Next

            Return col
        End Function

    End Class

   


End Class