using System;
using System.ComponentModel;
using System.Xml.Serialization;
using DMD;
using DMD.Licensing;

namespace minidom.License
{

    /// <summary>
    /// Licenza
    /// </summary>
    [Serializable]
    public class MiniDOMLicence 
        : LicenseEntity
    {

        // <DisplayName("Enable Feature 01")>
        // <Category("License Options")>
        // <XmlElement("EnableFeature01")>
        // <ShowInLicenseInfo(True, "Enable Feature 01", FormatType.String)>
        // Public Property EnableFeature01 As Boolean
        
        /// <summary>
        /// Costruttore
        /// </summary>
        public MiniDOMLicence()
        {
            // Initialize app name for the license
            AppName = "MiniDOM";
        }

        /// <summary>
        /// URL
        /// </summary>
        [DisplayName("SiteURL")]
        [Category("License Options")]
        [XmlElement("SiteURL")]
        [ShowInLicenseInfo(true, "SiteURL", FormatType.String)]
        public string SiteURL { get; set; }

        /// <summary>
        /// Data di scadenza della licenza
        /// </summary>
        [DisplayName("ExpireDate")]
        [Category("License Options")]
        [XmlElement("ExpireDate")]
        [ShowInLicenseInfo(true, "ExpireDate", FormatType.Date)]
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// Numero di esecuzioni massime
        /// </summary>
        [DisplayName("ExpireCount")]
        [Category("License Options")]
        [XmlElement("ExpireCount")]
        [ShowInLicenseInfo(true, "ExpireCount", FormatType.Integer)]
        public int ExpireCount { get; set; }

        /// <summary>
        /// Moduli consentiti
        /// </summary>
        [DisplayName("Modules")]
        [Category("License Options")]
        [XmlElement("Modules")]
        [ShowInLicenseInfo(true, "Modules", FormatType.EnumDescription)]
        public string[] AllowedModules { get; set; }

        
       
        /// <summary>
        /// Validazione 
        /// </summary>
        /// <param name="validationMsg"></param>
        /// <returns></returns>
        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            var _licStatus = LicenseStatus.UNDEFINED;
            validationMsg = string.Empty;
            switch (Type)
            {
                case LicenseTypes.Single:
                    {
                        // For Single License, check whether UID Is matched
                        if ((UID ?? "") == (LicenseHandler.GenerateUID(AppName) ?? ""))
                        {
                            _licStatus = LicenseStatus.VALID;
                        }
                        else
                        {
                            validationMsg = "The license is NOT for this copy!";
                            _licStatus = LicenseStatus.INVALID;
                        }

                        break;
                    }

                case LicenseTypes.Volume:
                    {
                        // No UID checking for Volume License
                        _licStatus = LicenseStatus.VALID;
                        break;
                    }

                default:
                    {
                        validationMsg = "Invalid license";
                        _licStatus = LicenseStatus.INVALID;
                        break;
                    }
            }

            if (_licStatus == LicenseStatus.VALID)
            {
                if (ExpireDate.HasValue)
                {
                    if (ExpireDate.Value < DateTime.Now)
                    {
                        validationMsg = "The license is expired";
                        _licStatus = LicenseStatus.INVALID;
                    }
                }
            }

            return _licStatus;
        }
    }
}