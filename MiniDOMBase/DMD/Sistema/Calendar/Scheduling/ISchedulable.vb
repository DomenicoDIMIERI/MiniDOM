Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Class Sistema

    Public Interface ISchedulable


        ReadOnly Property Programmazione As MultipleScheduleCollection

        ''' <summary>
        ''' Notifica all'oggetto che implementa la classe che la programmazione � stata cambiata esternamente e deve essere ricaricata
        ''' </summary>
        Sub InvalidateProgrammazione()

        Sub NotifySchedule(ByVal s As CalendarSchedule)

    End Interface
End Class