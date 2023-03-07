
namespace minidom.repositories
{

    /// <summary>
    /// Repository di <see cref="MailFolder"/>
    /// </summary>
    public class CFoldersClass
        : CModulesClass<MailFolder>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CFoldersClass() 
            : base("modOfficeEMailsFolders", typeof(MailFolderCursor), 0)
        {
        }
    }
}