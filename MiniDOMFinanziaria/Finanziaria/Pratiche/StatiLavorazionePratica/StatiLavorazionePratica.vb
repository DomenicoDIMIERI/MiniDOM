Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    ''' <summary>
    ''' Gestione degli stati di lavorazione
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CStatiLavorazionePraticaCLass
        Inherits CModulesClass(Of CStatoLavorazionePratica)

        Friend Sub New()
            MyBase.New("modCQSPDStatiLav", GetType(CStatiLavorazionePraticaCursor))
        End Sub
         
    End Class

    Private Shared m_StatiDiLavorazionePratica As CStatiLavorazionePraticaCLass = Nothing

    Public Shared ReadOnly Property StatiLavorazionePratica As CStatiLavorazionePraticaCLass
        Get
            If (m_StatiDiLavorazionePratica Is Nothing) Then m_StatiDiLavorazionePratica = New CStatiLavorazionePraticaCLass
            Return m_StatiDiLavorazionePratica
        End Get
    End Property

End Class