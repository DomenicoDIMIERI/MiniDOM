Imports minidom.Sistema

Partial Public Class Finanziaria

    ''' <summary>
    ''' Class Generica che consente di accedere al modulo Tabelle TEG Max dall'oggetto Finanziaria
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CTabelleTEGMaxClass
        Inherits CModulesClass(Of CTabellaTEGMax)


        Friend Sub New()
            MyBase.New("modCQSPDTblTEGMax", GetType(CTabelleTEGMaxCursor), -1)
        End Sub
         
    End Class

    Private Shared m_TabelleTEGMax As CTabelleTEGMaxClass = Nothing

    Public Shared ReadOnly Property TabelleTEGMax As CTabelleTEGMaxClass
        Get
            If (m_TabelleTEGMax Is Nothing) Then m_TabelleTEGMax = New CTabelleTEGMaxClass
            Return m_TabelleTEGMax
        End Get
    End Property
End Class
