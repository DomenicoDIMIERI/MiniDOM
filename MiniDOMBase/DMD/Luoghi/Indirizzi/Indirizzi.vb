Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Anagrafica

    <Serializable>
    Public NotInheritable Class CIndirizziClass
        Inherits CModulesClass(Of CIndirizzo)

        Friend Sub New()
            MyBase.New("modIndirizzi", GetType(CIndirizziCursor))
        End Sub
         

    End Class


End Class