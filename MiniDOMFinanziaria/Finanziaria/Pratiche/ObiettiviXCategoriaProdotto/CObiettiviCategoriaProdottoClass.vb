Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace CQSPDInternals

    Public Class CObiettiviCategoriaProdottoClass
        Inherits CModulesClass(Of Finanziaria.CObiettivoCategoriaProdotto)

        Public Sub New()
            MyBase.New("modCObiettiviCategoriaProdotto", GetType(Finanziaria.CObiettivoCategoriaProdottoCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce tutti gli obiettivi attivi e validi alla data indicata
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetObiettiviAl(ByVal d As Date) As CCollection(Of Finanziaria.CObiettivoCategoriaProdotto)
            Dim ret As New CCollection(Of Finanziaria.CObiettivoCategoriaProdotto)
            SyncLock Me
                For Each o As Finanziaria.CObiettivoCategoriaProdotto In Me.LoadAll
                    If (o.Stato = ObjectStatus.OBJECT_VALID AndAlso o.IsValid(d)) Then
                        ret.Add(o)
                    End If
                Next
            End SyncLock
            ret.Sort()
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce gli obiettivi validi alla data per l'ufficio specificato
        ''' </summary>
        ''' <param name="po"></param>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetObiettiviAl(ByVal po As CUfficio, ByVal d As Date) As CCollection(Of Finanziaria.CObiettivoCategoriaProdotto)
            Dim ret As New CCollection(Of Finanziaria.CObiettivoCategoriaProdotto)
            If (po Is Nothing) Then Throw New ArgumentNullException("po")
            SyncLock Me
                For Each o As Finanziaria.CObiettivoCategoriaProdotto In Me.LoadAll
                    If (o.Stato = ObjectStatus.OBJECT_VALID AndAlso (o.IDPuntoOperativo = 0 OrElse o.IDPuntoOperativo = GetID(po)) AndAlso o.IsValid(d)) Then
                        ret.Add(o)
                    End If
                Next
            End SyncLock
            ret.Sort()
            Return ret
        End Function

    End Class

End Namespace


Partial Public Class Finanziaria


    Private Shared m_ObiettiviXCategoria As CQSPDInternals.CObiettiviCategoriaProdottoClass

    Public Shared ReadOnly Property ObiettiviXCategoria As CQSPDInternals.CObiettiviCategoriaProdottoClass
        Get
            If (m_ObiettiviXCategoria Is Nothing) Then m_ObiettiviXCategoria = New CQSPDInternals.CObiettiviCategoriaProdottoClass
            Return m_ObiettiviXCategoria
        End Get
    End Property






End Class
