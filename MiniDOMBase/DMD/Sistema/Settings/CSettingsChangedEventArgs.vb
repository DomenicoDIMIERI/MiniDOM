Imports minidom.Databases

Partial Public Class Sistema

    Public Enum SettingChangedEnum As Integer
        Added
        Removed
        Changed
    End Enum

    <Serializable>
    Public Class CSettingsChangedEventArgs
        Inherits System.EventArgs


        Private m_Setting As CSetting
        Private m_Action As SettingChangedEnum

        Public Sub New()
            Me.m_Setting = Nothing
        End Sub

        Public Sub New(ByVal c As CSetting, ByVal a As SettingChangedEnum)
            If (c Is Nothing) Then Throw New ArgumentNullException("c")
            Me.m_Setting = c
            Me.m_Action = a
        End Sub

        Public ReadOnly Property Setting As CSetting
            Get
                Return Me.m_Setting
            End Get
        End Property

        Public ReadOnly Property Action As SettingChangedEnum
            Get
                Return Me.m_Action
            End Get
        End Property

    End Class


End Class
