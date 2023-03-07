Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals


    <Serializable>
    Public NotInheritable Class CAreaManagersClass
        Inherits CModulesClass(Of CAreaManager)

        Friend Sub New()
            MyBase.New("modAreaManager", GetType(CAreaManagerCursor), -1)
        End Sub


        ''' <summary>
        ''' Restituisce l'area manager corrispondente alla persona specificata
        ''' </summary>
        ''' <param name="personID"></param>
        ''' <returns></returns>
        Public Function GetItemByPersona(ByVal personID As Integer) As CAreaManager
            If (personID = 0) Then Return Nothing
            For Each item As CAreaManager In Me.LoadAll
                If (item.PersonaID = personID) Then Return item
            Next
            Return Nothing
        End Function

    End Class

End Namespace

Partial Public Class Finanziaria


    Private Shared m_AreaManagers As CAreaManagersClass = Nothing

    ''' <summary>
    ''' Modulo Area Managers
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property AreaManagers As CAreaManagersClass
        Get
            If (m_AreaManagers Is Nothing) Then m_AreaManagers = New CAreaManagersClass
            Return m_AreaManagers
        End Get
    End Property


End Class
