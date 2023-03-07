Imports minidom.Sistema

Partial Public Class Finanziaria

    Public NotInheritable Class CPreventiviClass
        Inherits CModulesClass(Of CPreventivo)
        
        Friend Sub New()
            MyBase.New("modPreventivatori", GetType(CPreventivoCursor))
        End Sub
         


    End Class

    Private Shared m_Preventivi As CPreventiviClass = Nothing

    Public Shared ReadOnly Property Preventivi As CPreventiviClass
        Get
            If (m_Preventivi Is Nothing) Then m_Preventivi = New CPreventiviClass
            Return m_Preventivi
        End Get
    End Property

End Class
