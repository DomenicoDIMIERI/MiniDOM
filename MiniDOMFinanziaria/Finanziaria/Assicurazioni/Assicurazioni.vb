Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Sistema

Partial Public Class Finanziaria

    Public NotInheritable Class CAssicurazioniClass
        Inherits CModulesClass(Of CAssicurazione)

        Friend Sub New()
            MyBase.New("modAnaAssicurazioni", GetType(CAssicurazioniCursor), -1)
        End Sub

         


    End Class

    Private Shared m_Assicurazioni As CAssicurazioniClass = Nothing

    Public Shared ReadOnly Property Assicurazioni As CAssicurazioniClass
        Get
            If (m_Assicurazioni Is Nothing) Then m_Assicurazioni = New CAssicurazioniClass
            Return m_Assicurazioni
        End Get
    End Property

End Class