Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Internals

Namespace Internals

    'Classe globale per la gestione delle manutnzioni sulle postazioni di lavoro
    Public Class CVociManutenzioneClass
        Inherits CModulesClass(Of VoceManutenzione)

        Friend Sub New()
            MyBase.New("modManutenziniVoci", GetType(VociManutenzioneCursor), 0)
        End Sub


    End Class
End Namespace

