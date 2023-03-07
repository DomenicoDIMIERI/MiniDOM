Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals

    <Serializable>
    Public NotInheritable Class CZoneClass
        Inherits CModulesClass(Of CZona)

        Friend Sub New()
            MyBase.New("modZoneGeografiche", GetType(CZonaCursor), -1)
        End Sub


    End Class

End Namespace

Partial Public Class Finanziaria



    Private Shared m_Zone As CZoneClass = Nothing

    Public Shared ReadOnly Property Zone As CZoneClass
        Get
            If (m_Zone Is Nothing) Then m_Zone = New CZoneClass
            Return m_Zone
        End Get
    End Property
End Class