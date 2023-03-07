Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Office
Imports minidom.Internals

Namespace Internals

    Public NotInheritable Class CartucceTonerClass
        Inherits CModulesClass(Of CartucciaToner)

        Friend Sub New()
            MyBase.New("modCartucceToner", GetType(CartucceTonerCursor), 0)
        End Sub


    End Class

End Namespace

Partial Class Office



    Private Shared m_CartucceToners As CartucceTonerClass = Nothing

    Public Shared ReadOnly Property CartucceToners As CartucceTonerClass
        Get
            If (m_CartucceToners Is Nothing) Then m_CartucceToners = New CartucceTonerClass
            Return m_CartucceToners
        End Get
    End Property

End Class