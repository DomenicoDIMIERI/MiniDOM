Imports minidom
Imports minidom.Databases
Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Public Class Finanziaria


    <Serializable>
    Public Class CStatsItem
        Implements IComparable

        Public Fonte As CFonte
        Public Visualizzazioni As Integer
        Public PrimaVisualizzazione As Date?
        Public UltimaVisualizzazione As Date?
        Public VisualizzazioniPerGiorno As Integer
        Public RichiesteGenerate As Integer
        Public PraticheGenerate As Integer
        Public PratichePerfezionate As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal fonte As CFonte)
            Me.Fonte = fonte
        End Sub

        Private Function CompareTo(obj As CStatsItem) As Integer
            Dim ret As Integer = obj.PraticheGenerate - Me.PraticheGenerate
            If (ret = 0) Then ret = obj.RichiesteGenerate - Me.RichiesteGenerate
            If (ret = 0) Then ret = obj.Visualizzazioni - Me.Visualizzazioni
            If (ret = 0) Then ret = Strings.Compare(Me.Fonte.Nome, obj.Fonte.Nome)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function



    End Class


End Class

