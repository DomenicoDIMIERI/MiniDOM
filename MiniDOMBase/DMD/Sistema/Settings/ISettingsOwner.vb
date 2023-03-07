Imports minidom.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti che possiedono collezioni di tipo CSettings
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ISettingsOwner

        ''' <summary>
        ''' Restituisce la collezione delle impostazioni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Settings As CSettings

        ''' <summary>
        ''' Informa l'oggetto che un parametro è stato modificato
        ''' </summary>
        ''' <param name="e"></param>
        Sub NotifySettingsChanged(ByVal e As CSettingsChangedEventArgs)
    End Interface


End Class
