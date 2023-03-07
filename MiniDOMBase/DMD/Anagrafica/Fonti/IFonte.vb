Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

''' <summary>
''' Interfaccia implementata dagli oggetti utilizzabili come fonti di un contatto, di una anagrafica, di una pratica ecc
''' </summary>
''' <remarks></remarks>
Public Interface IFonte

    ''' <summary>
    ''' Nome della fonte
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Nome As String

    ''' <summary>
    ''' Percorso dell'icona associata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property IconURL As String

End Interface

Partial Public Class Anagrafica




End Class