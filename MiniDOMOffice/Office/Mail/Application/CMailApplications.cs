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
    /// Repository di <see cref="MailApplication"/>
    /// </summary>
    [Serializable]
    public class CMailApplications 
        : CModulesClass<MailApplication>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CMailApplications() 
            : base("modMailApps", typeof(MailApplicationCursor), -1)
        {
        }

        /// <summary>
        /// Restituisce l'applicazione associata all'utente
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public minidom.Office.MailApplication GetItemByUser(CUser user)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            if (DBUtils.GetID(user, 0) == 0)
                return null;

            using (var cursor = new MailApplicationCursor())
            { 
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.UserID.Value = DBUtils.GetID(user);
                return cursor.Item;
            }
             
        }

        /// <summary>
        /// Questa funzione cerca l'applicazione associata all'utente e se non la trova la crea.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public MailApplication GetUserApp(CUser user)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            
            if (DBUtils.GetID(user, 0) == 0)
                throw new ArgumentNullException("userid");

            var app = GetItemByUser(user);
            if (app is null)
            {
                app = new MailApplication();
                app.User = user;
                app.Stato = ObjectStatus.OBJECT_VALID;
                app.Save();
            }

            return app;
        }
    }
}