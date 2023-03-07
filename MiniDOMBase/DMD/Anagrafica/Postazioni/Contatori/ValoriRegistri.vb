Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Internals

Namespace Internals

    'Classe globale per l'accesso agli uffici
    Public Class CValoriRegistriClass
        Inherits CModulesClass(Of ValoreRegistroContatore)

        Friend Sub New()
            MyBase.New("modPostazioniRegistri", GetType(ValoreRegistroCursor), 0)
        End Sub


    End Class
End Namespace
