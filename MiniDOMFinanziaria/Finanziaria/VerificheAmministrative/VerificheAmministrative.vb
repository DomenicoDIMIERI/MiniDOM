Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals


    Public NotInheritable Class VerificheAmministrativeClass
        Inherits CModulesClass(Of VerificaAmministrativa)

        Friend Sub New()
            MyBase.New("modCQSPDVerificheAmministrative", GetType(VerificheAmministrativeCursor))
        End Sub

        Protected Overrides Function CreateModuleInfo() As CModule
            Dim m As CModule = MyBase.CreateModuleInfo()
            m.DisplayName = "Verifiche Amministrative"
            m.ClassHandler = "VerificheAmministrativeHandler"
            m.Save()
            Return m
        End Function
      
    End Class

End Namespace


Partial Public Class Finanziaria

    Private Shared m_VerificheAmministrative As VerificheAmministrativeClass = Nothing

    Public Shared ReadOnly Property VerificheAmministrative As VerificheAmministrativeClass
        Get
            If (m_VerificheAmministrative Is Nothing) Then m_VerificheAmministrative = New VerificheAmministrativeClass
            Return m_VerificheAmministrative
        End Get
    End Property


End Class
