using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using minidom.License;
using DMD.Licensing;
using MiniDOM;

namespace MiniDOMLicense
{
    public partial class frmMain : Form
    {

        byte[] _certPubicKeyData;
        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Shown(object sender, EventArgs e)
        {
            //Initialize variables with default values
            MiniDOMLicence _lic = null;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("MiniDOMLicense.MiniDOM.cer").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //var linfo = new DMD.Licensing.Helpers.LicenseInfo(;
            
            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (MiniDOMLicence)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(MiniDOMLicence),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            switch (_status)
            {
                case LicenseStatus.VALID:

                    //TODO: If license is valid, you can do extra checking here
                    //TODO: E.g., check license expiry date if you have added expiry date property to your license entity
                    //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                    //Here for demo, just show the license information and RETURN without additional checking       
                    licInfo.ShowLicenseInfo(_lic);

                    return;

                default:
                    //for the other status of license file, show the warning message
                    //and also popup the activation form for user to activate your application
                    MessageBox.Show(_msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    using (frmActivation frm = new frmActivation())
                    {
                        frm.CertificatePublicKeyData = _certPubicKeyData;
                        frm.ShowDialog();

                        //Exit the application after activation to reload the license file 
                        //Actually it is not nessessary, you may just call the API to reload the license file
                        //Here just simplied the demo process

                        Application.Exit();
                    }
                    break;
            }
        }
    }
}
