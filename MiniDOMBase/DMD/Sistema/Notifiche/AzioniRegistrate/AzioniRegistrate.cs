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

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti di tipo <see cref="AzioneRegistrata"/>
    /// </summary>
    [Serializable]
    public class AzioniRegistrateRepository
        : CModulesClass<AzioneRegistrata>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public AzioniRegistrateRepository()
            : base("modNotificheAzioniRegistrate", typeof(AzioneRegistrataCursor ), -1)
        {

        }

        /// <summary>
        /// Restituisce l'azione registrata in base al nome
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public AzioneRegistrata GetItemByName(string sourceName, string actionType)
        {
            sourceName = DMD.Strings.Trim(sourceName);
            actionType = DMD.Strings.Trim(actionType);
            if (string.IsNullOrEmpty(actionType))
                throw new ArgumentNullException("actionType");

            foreach (var o in this.LoadAll())
            {
                if (
                       DMD.Strings.EQ(o.SourceName, sourceName) 
                    && DMD.Strings.EQ(o.ActionType, actionType)
                    )
                {
                    return o;
                }
            }
            return null;
        }

        /// <summary>
        /// Restituisce l'azione registrata in base al nome
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="azione"></param>
        /// <returns></returns>
        public AzioneRegistrata GetItemByName(string sourceName, AzioneEseguibile azione)
        {
            if (azione is null)
                throw new ArgumentNullException("azione");
            return this.GetItemByName(sourceName, DMD.RunTime.vbTypeName(azione));
        }

        /// <summary>
        /// Restituisce true se l'azione è registrata
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="azione"></param>
        /// <returns></returns>
        public bool IsAzioneRegistrata(string sourceName, AzioneEseguibile azione)
        {
            return this.GetItemByName(sourceName, azione) is object;
        }

        /// <summary>
        /// Registra un'azione definita sul tipo di oggetti specificato
        /// </summary>
        /// <param name="sourceName">[in] Tipo di sorgente. Se NULL l'azione sarà disponibile su tutto</param>
        /// <param name="azione">[in] Azione da registrare per il tipo di sorgente specificato</param>
        /// <remarks></remarks>
        public AzioneRegistrata RegisterAzione(string sourceName, AzioneEseguibile azione)
        {
            lock (this)
            {
                if (azione is null)
                    throw new ArgumentNullException("azione");
                sourceName = DMD.Strings.Trim(sourceName);
                var item = this.GetItemByName(sourceName, azione);
                if (item is object )
                    throw new ArgumentException("Azione già registrata per il tipo specificato");

                item = new AzioneRegistrata();
                item.ActionType = DMD.RunTime.vbTypeName(azione);
                item.SourceName = sourceName;
                item.Stato = ObjectStatus.OBJECT_VALID;
                item.Save();
                //RegisteredActions.Add(item);
                return item;
            }
        }

        /// <summary>
        /// Rimuove la registrazione dell'azione
        /// </summary>
        /// <param name="sourceName">[in] Tipo di sorgente. Se NULL l'azione sarà rimossa solo se definita su tutto</param>
        /// <param name="azione">[in] Azione da rimuovere</param>
        /// <remarks></remarks>
        public AzioneRegistrata UnregisterAzione(string sourceName, AzioneEseguibile azione)
        {
            lock (this)
            {
                if (azione is null)
                    throw new ArgumentNullException("azione");
                sourceName = DMD.Strings.Trim(sourceName);
                var item = this.GetItemByName(sourceName, azione);
                if (item is null)
                    throw new ArgumentNullException("Azione non registrata per il tipo specificato");
                item.Delete();
                return item;
            }
        }

        /// <summary>
        /// Restituisce una collezione contenente tutte le azioni registrate per la notifica
        /// </summary>
        /// <param name="sourceName">[in] Tipo di sorgente. Non può essere NULL</param>
        /// <remarks></remarks>
        public CCollection<AzioneRegistrata> GetRegisteredActions(string sourceName)
        {
            lock (this)
            {
                sourceName = DMD.Strings.Trim(sourceName);
                if (string.IsNullOrEmpty(sourceName))
                    throw new ArgumentNullException("sourceType");

                var ret = new CCollection<AzioneRegistrata>();
                foreach (AzioneRegistrata item in this.LoadAll())
                {
                    if (string.IsNullOrEmpty(item.SourceName) || (item.SourceName ?? "") == (sourceName ?? ""))
                    {
                        ret.Add(item);
                    }
                }

                return ret;
            }
        }
    }


}
 