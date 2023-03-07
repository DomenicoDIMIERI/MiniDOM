
namespace minidom
{
    public partial class Databases
    {
        public class CMdbDBConnection : COleDBConnection
        {
            public CMdbDBConnection()
            {
            }

            public CMdbDBConnection(string fileName)
            {
                Path = fileName;
            }

            public CMdbDBConnection(string fileName, string userName, string password)
            {
                Path = fileName;
                SetCredentials(userName, password);
            }
        }
    }
}