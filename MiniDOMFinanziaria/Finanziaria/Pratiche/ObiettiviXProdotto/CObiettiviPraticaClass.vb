Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Finanziaria
Imports minidom.CQSPDInternals



Namespace CQSPDInternals

    Public Class CObiettivoPraticaClass
        Inherits CModulesClass(Of Finanziaria.CObiettivoPratica)

        Public Sub New()
            MyBase.New("modCQSPDObiettiviPratica", GetType(Finanziaria.CObiettivoPraticaCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce tutti gli obiettivi attivi e validi alla data indicata
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetObiettiviAl(ByVal d As Date) As CCollection(Of Finanziaria.CObiettivoPratica)
            Dim ret As New CCollection(Of Finanziaria.CObiettivoPratica)
            SyncLock Me
                For Each o As Finanziaria.CObiettivoPratica In Me.LoadAll
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
        Public Function GetObiettiviAl(ByVal po As CUfficio, ByVal d As Date) As CCollection(Of Finanziaria.CObiettivoPratica)
            Dim ret As New CCollection(Of Finanziaria.CObiettivoPratica)
            If (po Is Nothing) Then Throw New ArgumentNullException("po")
            SyncLock Me
                For Each o As Finanziaria.CObiettivoPratica In Me.LoadAll
                    If (o.Stato = ObjectStatus.OBJECT_VALID AndAlso (o.IDPuntoOperativo = 0 OrElse o.IDPuntoOperativo = GetID(po)) AndAlso o.IsValid(d)) Then
                        ret.Add(o)
                    End If
                Next
            End SyncLock
            ret.Sort()
            Return ret
        End Function

        Public Function GetItemByName(nome As String) As Finanziaria.CObiettivoPratica
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each o As CObiettivoPratica In Me.LoadAll
                If (Strings.Compare(o.Nome, nome) = 0) Then Return o
            Next
            Return Nothing
        End Function
    End Class

End Namespace


Partial Public Class Finanziaria


    Private Shared m_Obiettivi As CQSPDInternals.CObiettivoPraticaClass

    Public Shared ReadOnly Property Obiettivi As CQSPDInternals.CObiettivoPraticaClass
        Get
            If (m_Obiettivi Is Nothing) Then m_Obiettivi = New CQSPDInternals.CObiettivoPraticaClass
            Return m_Obiettivi
        End Get
    End Property



    


End Class
