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
    public partial class Sistema
    {

        /// <summary>
        /// Collezione di azioni valide su un modulo
        /// </summary>
        [Serializable]
        public class CModuleActions 
            : CKeyCollection<CModuleAction>
        {
            [NonSerialized] private CModule m_Module;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleActions()
            {
                m_Module = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="module"></param>
            public CModuleActions(CModule module) : this()
            {
                Load(module);
            }

            /// <summary>
            /// Restituisce un riferimento al modulo
            /// </summary>
            public CModule Module
            {
                get
                {
                    return m_Module;
                }
            }

            /// <summary>
            /// Carica la collezione dal db
            /// </summary>
            /// <param name="module"></param>
            protected internal void Load(CModule module)
            {
                lock (Modules.actionsLock)
                {
                    if (module is null)
                        throw new ArgumentNullException("module");
                    Clear();
                    m_Module = module;
                    
                    foreach (var a in minidom.Sistema.Modules.DefinedActions.LoadAll())
                    {
                        if (DMD.Strings.Compare(a.ModuleName, module.ModuleName, true) == 0)
                        {
                            Add(a.AuthorizationName, a);
                        }
                    }
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CModuleAction value)
            {
                if (m_Module is object)
                    value.SetModule(m_Module);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CModuleAction oldValue, CModuleAction newValue)
            {
                if (m_Module is object)
                    newValue.SetModule(m_Module);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Registra una nuova azione per il modulo specificato. Se l'azione esiste già restituisce quella già registrata
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            public CModuleAction EnsureAction(string actionName)
            {
                return this.RegisterAction(actionName);
            }

            /// <summary>
            /// Registra una nuova azione per il modulo specificato. Se l'azione esiste già restituisce quella già registrata
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModuleAction RegisterAction(string actionName)
            {
                lock (Modules.actionsLock)
                {
                    int i;
                    actionName = DMD.Strings.Trim(actionName);
                    i = IndexOfKey(actionName);
                    if (i >= 0)
                    {
                        return this[i];
                    }
                    else
                    {
                        var item = new CModuleAction();
                        item.AuthorizationName = actionName;
                        item.AuthorizationDescription = actionName;
                        item.Module = m_Module;
                        Add(actionName, item);
                        item.Save();
                        return item;
                    }
                }
            }

            /// <summary>
            /// Rimuove l'azione corrispondente all'ID specifico
            /// </summary>
            /// <param name="actionID"></param>
            /// <remarks></remarks>
            public void UnregisterAction(int actionID)
            {
                UnregisterAction(GetItemById(actionID));
            }

            /// <summary>
            /// Rimuove l'azione
            /// </summary>
            /// <param name="a"></param>
            /// <remarks></remarks>
            public void UnregisterAction(CModuleAction a)
            {
                lock (Modules.actionsLock)
                {
                    if (a is null)
                        throw new ArgumentNullException("a");
                    if (!ReferenceEquals(a.Module, m_Module))
                        throw new ArgumentOutOfRangeException("Per eliminare l'azione di un modulo fare riferimento al modulo stesso");
                    using(var c = new CUserAuthorizationCursor())
                    {
                        c.ActionID.Value = a.ID;
                        c.IgnoreRights = true;
                        while (c.Read())
                        {
                            c.Item.Delete();
                        }
                    }
                    using (var c = new CGroupAuthorizationCursor())
                    {
                        c.ActionID.Value = a.ID;
                        c.IgnoreRights = true;
                        while (c.Read())
                        {
                            c.Item.Delete();
                        }
                    }
                    a.Delete();
                    this.Remove(a);
                    // Modules.DefinedActions.Remove(a)
                }
            }

            /// <summary>
            /// Rimuove l'azione specifica del modulo
            /// </summary>
            /// <param name="actionName"></param>
            /// <remarks></remarks>
            public void UnregisterAction(string actionName)
            {
                UnregisterAction(GetItemByKey(actionName));
            }

            /// <summary>
            /// Restituisce vero se il modu
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IsRegisteredAction(string actionName)
            {
                int i = this.IndexOfKey(actionName);
                if (i >= 0)
                {
                    return DBUtils.GetID(base[i], 0);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}