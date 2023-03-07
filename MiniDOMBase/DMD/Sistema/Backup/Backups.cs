using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CBackup"/>
        /// </summary>
        [Serializable]
        public sealed partial class CBackupsClass
            : CModulesClass<CBackup>
        {


            /// <summary>
            /// Costruttore
            /// </summary>
            public CBackupsClass() 
                : base("modBackups", typeof(CBackupCursor))
            {
            }

            /// <summary>
            /// Effettua il backup
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="compressionMethod"></param>
            /// <param name="compressionLevel"></param>
            /// <returns></returns>
            public CBackup Create(
                            DateTime? fromDate, 
                            CompressionMethods compressionMethod, 
                            CompressionLevels compressionLevel
                            )
            {
                CBackup bk;
                bk = new CBackup();
                bk.FileDate = DMD.DateUtils.Now();
                bk.Name = bk.GetDefaultName();
                bk.FileName = Configuration.BackupFolder + @"\" + bk.Name;
                bk.CompressionLevel = compressionLevel;
                bk.CompressionMethod = compressionMethod;
                Module.DispatchEvent(new EventDescription("backup_begin", "Inizio il backup ", bk));
                object dirs = Configuration.GetIncludedDirs();
                ApplicationContext.EnterMaintenance();

                // System.Threading.Thread.Sleep(3000)
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                bk.Create(fromDate);
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                ApplicationContext.QuitMaintenance();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

                // If (bk IsNot Nothing) Then
                bk.Stato = ObjectStatus.OBJECT_VALID;
                bk.Save();
                Module.DispatchEvent(new EventDescription("backup_end", "Fine del backup ", bk));
                // End If

                return bk;
            }

            /// <summary>
            /// Ripristina gli elementi del backup corrispondente all'id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public CBackup Restore(int id)
            {
                return Restore(GetItemById(id));
            }

            /// <summary>
            /// Ripristina gli elementi del backup 
            /// </summary>
            /// <param name="bk"></param>
            /// <returns></returns>
            public CBackup Restore(CBackup bk)
            {
                if (bk is null)
                    throw new ArgumentNullException("Backup non trovato");
                Module.DispatchEvent(new EventDescription("restore_begin", "Inizio il ripristino del sistema dal file: " + bk.FileName, bk));
                ApplicationContext.EnterMaintenance();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                bk.Restore();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                ApplicationContext.QuitMaintenance();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                Module.DispatchEvent(new EventDescription("restore_end", "Fine del ripristino del sistema dal file: " + bk.FileName, bk));
                return bk;
            }

            /// <summary>
            /// Elimina il backup
            /// </summary>
            /// <param name="bkID"></param>
            /// <returns></returns>
            public CBackup Delete(int bkID)
            {
                var bk = GetItemById(bkID);
                bk.Destroy();
                return bk;
            }


            /// <summary>
            /// Restituisce l'elemento in base al suo nome nel DB
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CBackup GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                using (var cursor = new CBackupCursor())
                {
                    cursor.Name.Value = name;
                    cursor.IgnoreRights = true;
                    cursor.PageSize = 1;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    return cursor.Item;
                }

            }

            /// <summary>
            /// Restituisce l'ultimo backup effettuato
            /// </summary>
            /// <returns></returns>
            public CBackup GetLastFullBackup()
            {
                using (var cursor = new CBackupCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.FileDate.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }

            }

            public override void Initialize()
            {
                base.Initialize();
                Configuration.Load();
            }

            /// <summary>
            /// Verifica se deve essere effettuato il backup sulla base della configurazione attuale ed eventualmente lo effettua
            /// </summary>
            /// <remarks></remarks>
            public void CreateFullBackup()
            {
                var d = DMD.DateUtils.Now();
                var config = minidom.Sistema.Backups.Configuration;
                Create(default, config.CompressionMethod, config.CompressionLevel);
                config.LastFullBackupDate = d;
                config.Save();
            }

            /// <summary>
            /// Verifica se deve essere effettuato il backup sulla base della configurazione attuale ed eventualmente lo effettua
            /// </summary>
            /// <remarks></remarks>
            public void CreatePartialBackup(DateTime fromDate)
            {
                var config = minidom.Sistema.Backups.Configuration;
                Create(fromDate, config.CompressionMethod, config.CompressionLevel);
                config.LastPartialBackupDate = fromDate;
                config.Save();
            }
        }

    }

    public partial class Sistema
    {
     
        private static CBackupsClass m_Backups = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CBackup"/>
        /// </summary>
        public static CBackupsClass Backups
        {
            get
            {
                if (m_Backups is null)
                    m_Backups = new CBackupsClass();
                return m_Backups;
            }
        }
    }
}