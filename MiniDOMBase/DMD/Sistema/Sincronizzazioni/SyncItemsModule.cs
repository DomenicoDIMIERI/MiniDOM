using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="SyncItem"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class SyncItemsModule 
            : CModulesClass<SyncItem>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public SyncItemsModule() 
                : base("modSyncItems", typeof(Sistema.SyncItemsCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce la collezione degli oggetti sincronizzati 
            /// </summary>
            /// <param name="remoteSite"></param>
            /// <param name="itemType"></param>
            /// <param name="remoteID"></param>
            /// <returns></returns>
            public CCollection<SyncItem> GetMatchs(string remoteSite, string itemType, int remoteID)
            {
                var ret = new CCollection<SyncItem>();
                using (var cursor = new SyncItemsCursor())
                {
                    while (cursor.Read())
                    {
                        cursor.IgnoreRights = true;
                        cursor.RemoteSite.Value = remoteSite;
                        cursor.RemoteID.Value = remoteID;
                        cursor.ItemType.Value = itemType;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        ret.Add(cursor.Item);
                    }
                }
                return ret;
            }

            /// <summary>
            /// Restituisce la collezione degli oggetti sincronizzati 
            /// </summary>
            /// <param name="remoteSite"></param>
            /// <param name="itemType"></param>
            /// <param name="remoteID"></param>
            /// <param name="syncDate"></param>
            /// <returns></returns>
            public SyncItem GetMatch(string remoteSite, string itemType, int remoteID, DateTime syncDate)
            {
                using (var cursor = new SyncItemsCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.RemoteSite.Value = remoteSite;
                    cursor.RemoteID.Value = remoteID;
                    cursor.ItemType.Value = itemType;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.SyncDate.Value = syncDate;
                    return cursor.Item;
                }                 
            }

            /// <summary>
            /// Restituisce l'ID dell'oggetto locale corrispondente
            /// </summary>
            /// <param name="remoteSite"></param>
            /// <param name="itemType"></param>
            /// <param name="remoteID"></param>
            /// <returns></returns>
            public CCollection<int> MatchLocalItemIDs(string remoteSite, string itemType, int remoteID)
            {
                var items = GetMatchs(remoteSite, itemType, remoteID);
                var ret = new CCollection<int>();
                foreach(var item in items)
                {
                    ret.Add(item.LocalID);
                }
                return ret;
            }

            /// <summary>
            /// Restituisce la collezione degli oggetti che corrispondono alle sincronizzazioni
            /// </summary>
            /// <param name="remoteSite"></param>
            /// <param name="itemType"></param>
            /// <param name="remoteID"></param>
            /// <returns></returns>
            public CCollection MatchLocalObjects(string remoteSite, string itemType, int remoteID)
            {
                var localIDs = MatchLocalItemIDs(remoteSite, itemType, remoteID);
                var ret = new CCollection();
                foreach (var id in localIDs)
                {
                    ret.Add(Sistema.ApplicationContext.GetItemByTypeAndId(itemType, id));
                }
                return ret;
            }

            /// <summary>
            /// Informa della sincronizzazione di un oggetto
            /// </summary>
            /// <param name="remoteSite"></param>
            /// <param name="itemType"></param>
            /// <param name="remoteID"></param>
            /// <param name="localID"></param>
            /// <param name="syncDate"></param>
            /// <returns></returns>
            public SyncItem SetMatch(string remoteSite, string itemType, int remoteID, int localID, DateTime syncDate)
            {
                var item = GetMatch(remoteSite, itemType, remoteID, syncDate);
                if (item is null)
                {
                    item = new Sistema.SyncItem();
                    item.RemoteSite = remoteSite;
                    item.ItemType = itemType;
                    item.RemoteID = remoteID;
                    item.SyncDate = syncDate;
                }
                item.LocalID = localID;
                item.Stato = ObjectStatus.OBJECT_VALID;
                item.Save();
                return item;
            }
        }
    }

    public partial class Sistema
    {
        private static SyncItemsModule m_SyncItems = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="SyncItem"/>
        /// </summary>
        public static SyncItemsModule SyncItems
        {
            get
            {
                if (m_SyncItems is null)
                    m_SyncItems = new SyncItemsModule();
                return m_SyncItems;
            }
        }
    }
}