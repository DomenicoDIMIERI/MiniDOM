Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace CQSPDInternals

    <Serializable>
    Public Class CMotiviScontoPraticaClass
        Inherits CModulesClass(Of Finanziaria.CMotivoScontoPratica)

        Public Sub New()
            MyBase.New("modCQSPDMotiviScontoPratica", GetType(Finanziaria.CMotivoScontoPraticaCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce un oggetto in base al suo nome (la ricerca è fatta solo sui motivi attivi)
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As Finanziaria.CMotivoScontoPratica
            Dim items As CCollection(Of Finanziaria.CMotivoScontoPratica) = Me.LoadAll
            value = LCase(Trim(value))
            If (value = "") Then Return Nothing
            For Each m As Finanziaria.CMotivoScontoPratica In items
                If (m.Attivo AndAlso LCase(m.Nome) = value) Then Return m
            Next

            Return Nothing
        End Function
    End Class

End Namespace


Partial Public Class Finanziaria


    Private Shared m_MotiviSconto As CQSPDInternals.CMotiviScontoPraticaClass

    Public Shared ReadOnly Property MotiviSconto As CQSPDInternals.CMotiviScontoPraticaClass
        Get
            If (m_MotiviSconto Is Nothing) Then m_MotiviSconto = New CQSPDInternals.CMotiviScontoPraticaClass
            Return m_MotiviSconto
        End Get
    End Property



    


End Class
