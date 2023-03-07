Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Internals

    Partial Public Class DateUtilsClass



        Public NotInheritable Class CScheduledTasksClass
            Inherits CModulesClass(Of CalendarSchedule)

            Friend Sub New()
                MyBase.New("modScheduledTasks", GetType(CalendarScheduleCursor))
            End Sub


        End Class

        Private m_ScheduldTasks As CScheduledTasksClass = Nothing

        Public ReadOnly Property ScheduledTasks As CScheduledTasksClass
            Get
                If (Me.m_ScheduldTasks Is Nothing) Then Me.m_ScheduldTasks = New CScheduledTasksClass
                Return Me.m_ScheduldTasks
            End Get
        End Property

    End Class


End Namespace