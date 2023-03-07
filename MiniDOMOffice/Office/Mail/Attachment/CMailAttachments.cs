using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom.repositories
{

    /// <summary>
    /// Repository di <see cref="MailAttachment"/>
    /// </summary>
    [Serializable]
    public class CMailAttachments 
        : CModulesClass<MailAttachment>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CMailAttachments() 
            : base("modOfficeEMailsAtt", typeof(MailAttachmentCursor), 0)
        {
        }
    }
}