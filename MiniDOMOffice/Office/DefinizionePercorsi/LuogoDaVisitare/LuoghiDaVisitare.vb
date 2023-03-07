Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Internals
Imports minidom.Office

Namespace Internals

    Public NotInheritable Class CLuoghiDaVisitareClass
        Inherits CModulesClass(Of LuogoDaVisitare)

        Friend Sub New()
            MyBase.New("modOfficePercorsiLuoghiDaVisitare", GetType(LuoghiDaVisitareCursor))
        End Sub


    End Class

End Namespace

Partial Class Office

    Partial Class CPercorsiDefinitiClass



        Private m_LuoghiDaVisitare As CLuoghiDaVisitareClass = Nothing

        Public ReadOnly Property LuoghiDaVisitare As CLuoghiDaVisitareClass
            Get
                If (m_LuoghiDaVisitare Is Nothing) Then m_LuoghiDaVisitare = New CLuoghiDaVisitareClass
                Return m_LuoghiDaVisitare
            End Get
        End Property


    End Class

End Class