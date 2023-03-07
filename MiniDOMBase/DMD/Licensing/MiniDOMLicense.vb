Imports DMD.Licensing
Imports System.ComponentModel
Imports System.Xml.Serialization

Namespace License

    Public Class MiniDOMLicence
        Inherits LicenseEntity

        '<DisplayName("Enable Feature 01")>
        '<Category("License Options")>
        '<XmlElement("EnableFeature01")>
        '<ShowInLicenseInfo(True, "Enable Feature 01", FormatType.String)>
        'Public Property EnableFeature01 As Boolean

        <DisplayName("SiteURL")>
        <Category("License Options")>
        <XmlElement("SiteURL")>
        <ShowInLicenseInfo(True, "SiteURL", FormatType.String)>
        Public Property SiteURL As String

        <DisplayName("ExpireDate")>
        <Category("License Options")>
        <XmlElement("ExpireDate")>
        <ShowInLicenseInfo(True, "ExpireDate", FormatType.Date)>
        Public Property ExpireDate As DateTime?

        <DisplayName("ExpireCount")>
        <Category("License Options")>
        <XmlElement("ExpireCount")>
        <ShowInLicenseInfo(True, "ExpireCount", FormatType.Integer)>
        Public Property ExpireCount As Integer

        <DisplayName("Modules")>
        <Category("License Options")>
        <XmlElement("Modules")>
        <ShowInLicenseInfo(True, "Modules", FormatType.EnumDescription)>
        Public Property AllowedModules As String()

        Public Sub New()
            'Initialize app name for the license
            Me.AppName = "MiniDOM"
        End Sub

        Public Overrides Function GetHashCode() As Integer
            Return MyBase.GetHashCode()
        End Function

        Public Overrides Function DoExtraValidation(ByRef validationMsg As String) As LicenseStatus
            Dim _licStatus As LicenseStatus = LicenseStatus.UNDEFINED
            validationMsg = String.Empty

            Select Case (Me.Type)
                Case LicenseTypes.Single
                    'For Single License, check whether UID Is matched
                    If (Me.UID = LicenseHandler.GenerateUID(Me.AppName)) Then
                        _licStatus = LicenseStatus.VALID
                    Else
                        validationMsg = "The license is NOT for this copy!"
                        _licStatus = LicenseStatus.INVALID
                    End If
                Case LicenseTypes.Volume
                    'No UID checking for Volume License
                    _licStatus = LicenseStatus.VALID
                Case Else
                    validationMsg = "Invalid license"
                    _licStatus = LicenseStatus.INVALID
            End Select

            If (_licStatus = LicenseStatus.VALID) Then
                If (Me.ExpireDate.HasValue) Then
                    If (Me.ExpireDate.Value < Date.Now) Then
                        validationMsg = "The license is expired"
                        _licStatus = LicenseStatus.INVALID
                    End If
                End If
            End If

            Return _licStatus
        End Function

    End Class


End Namespace
