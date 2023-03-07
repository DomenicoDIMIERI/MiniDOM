Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti che possono estinguere un finanziamento in corso
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IEstintore

        ReadOnly Property Estinzioni As CEstinzioniXEstintoreCollection

        ReadOnly Property DataCaricamento As Date

        Property DataDecorrenza As Date?

        Property Cliente As CPersonaFisica

    End Interface

End Class
