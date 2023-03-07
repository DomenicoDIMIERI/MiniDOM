Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria


    '.---------------------------------------------

    ''' <summary>
    ''' Classe che consente di accedere alle regole di passaggi di stato
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CStatiPratRulesClass
        Inherits CModulesClass(Of CStatoPratRule)

        Friend Sub New()
            MyBase.New("modCQSPDStPrtRul", GetType(CStatoPratRuleCursor), -1)
        End Sub
         

    End Class

    Private Shared m_StatiPratRules As CStatiPratRulesClass = Nothing

    Public Shared ReadOnly Property StatiPratRules As CStatiPratRulesClass
        Get
            If (m_StatiPratRules Is Nothing) Then m_StatiPratRules = New CStatiPratRulesClass
            Return m_StatiPratRules
        End Get
    End Property

End Class
