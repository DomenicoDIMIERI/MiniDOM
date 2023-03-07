﻿Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Office

Namespace Internals

    ''' <summary>
    ''' Gestione delle uscite
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CSpedizioniClass
        Inherits CModulesClass(Of Spedizione)

        Friend Sub New()
            MyBase.New("modOfficeSpedizioni", GetType(SpedizioniCursor), 0)
        End Sub


        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeSpedizioni")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeSpedizioni")
                ret.Description = "Spedizioni"
                ret.DisplayName = "Spedizioni"
                ret.Parent = minidom.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
             
            Return ret
        End Function

    End Class

End Namespace



Partial Class Office



    Private Shared m_Spedizioni As CSpedizioniClass = Nothing

    Public Shared ReadOnly Property Spedizioni As CSpedizioniClass
        Get
            If (m_Spedizioni Is Nothing) Then m_Spedizioni = New CSpedizioniClass
            Return m_Spedizioni
        End Get
    End Property


End Class