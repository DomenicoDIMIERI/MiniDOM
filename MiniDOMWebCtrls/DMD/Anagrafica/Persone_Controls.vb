Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils




Namespace Forms

 

#Region "Distributori"

    Public Class DistributoriModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CDistributoriCursor
        End Function

    End Class




#End Region

#Region "Tipolige Azienda"

    Public Class TipologieAziendaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTipologiaAziendaCursor
        End Function


    End Class



#End Region

#Region "Categorie Azienda"

    Public Class CategorieAziendaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()

        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCategorieAziendaCursor
        End Function

    End Class

#End Region

#Region "Forme Giuridiche Azienda"

    Public Class FormeGiuridicheAziendaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()

        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CFormeGiuridicheAziendaCursor
        End Function


    End Class


#End Region




End Namespace