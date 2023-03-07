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

namespace minidom
{

    /// <summary>
    /// Oggetto che racchiude informazioni sullo stato di autorizzazione dell'oggetto
    /// in base all'utente o al gruppo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ItemAllowNegate<T>
    {

        /// <summary>
        /// Oggetto
        /// </summary>
        [NonSerialized] public T Item;

        /// <summary>
        /// UserID
        /// </summary>
        public int UserAuthID;

        /// <summary>
        /// User Allow
        /// </summary>
        public bool UserAllow;

        /// <summary>
        /// User Negate
        /// </summary>
        public bool UserNegate;

        /// <summary>
        /// Group
        /// </summary>
        public int GroupAuthID;

        /// <summary>
        /// Group Allow
        /// </summary>
        public bool GroupAllow;

        /// <summary>
        /// Group Negate
        /// </summary>
        public bool GroupNegate;

        /// <summary>
        /// Costruttore
        /// </summary>
        public ItemAllowNegate()
        {
            //DMDObject.IncreaseCounter(this);
            this.Item = default;
            this.UserAuthID = 0;
            this.UserAllow = false;
            this.UserNegate = false;
            this.GroupAuthID = 0;
            this.GroupAllow = false;
            this.GroupNegate = false;
        }

        /// <summary>
        /// Restituisce true se l'azione è consentita all'utente e al gruppo e se non é negata all'utente o ad uno dei gruppi
        /// </summary>
        /// <returns></returns>
        public bool IsAllowed()
        {
            return (this.UserAllow || this.GroupAllow) && !(this.UserNegate || this.GroupNegate);
        }

        /// <summary>
        /// Restituisce true se l'azione non é consentita
        /// </summary>
        /// <returns></returns>
        public bool IsNegated()
        {
            return !IsAllowed();
        }

        //~ItemAllowNegate()
        //{
        //    //DMDObject.DecreaseCounter(this);
        //}
    }
}