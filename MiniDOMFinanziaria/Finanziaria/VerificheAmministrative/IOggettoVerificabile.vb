Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti su cui � possibile effettuare dei controlli amministrativi
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IOggettoVerificabile

        ''' <summary>
        ''' Restituisce o imposta l'ultima verifica amministrativa effettuata sull'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property UltimaVerifica As VerificaAmministrativa

    End Interface

End Class
