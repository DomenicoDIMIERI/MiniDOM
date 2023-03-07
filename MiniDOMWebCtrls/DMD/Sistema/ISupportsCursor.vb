Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils



Namespace Forms

    ''' <summary>
    ''' Interfaccia implementata dai controlli che utilizzano un cursore
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ISupportsCursor

        ''' <summary>
        ''' Restituisce o imposta il cursore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Cursor As DBObjectCursorBase



    End Interface

 

End Namespace