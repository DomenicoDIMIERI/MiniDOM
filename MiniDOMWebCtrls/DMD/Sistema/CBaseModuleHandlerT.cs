using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom.Forms
{

    /// <summary>
    /// Handler per un modulo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CBaseModuleHandler<T<> 
        : CBaseModuleHandler
    {
       
        /// <summary>
        /// Costruttore
        /// </summary>
        public CBaseModuleHandler()
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="support"></param>
        public CBaseModuleHandler(ModuleSupportFlags support) 
            : base(support)
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="module"></param>
        /// <param name="support"></param>
        /// <param name="useLocal"></param>
        public CBaseModuleHandler(
                            CModule module, 
                            ModuleSupportFlags support = ModuleSupportFlags.SNone, 
                            bool useLocal = true
            ) 
            : base(module, support, useLocal)
        {
        }

        /// <summary>
        /// Restituisce true se l'utente può creare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanCreate(T item)
        {
            return base.CanCreate(item);
        }

        /// <summary>
        /// Restituisce true se l'utente può creare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public sealed override bool CanCreate(object item)
        {
            return this.CanCreate((T)item);
        }

        /// <summary>
        /// Restituisce true se l'utente può eliminare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanDelete(T item)
        {
            return base.CanDelete(item);
        }

        /// <summary>
        /// Restituisce true se l'utente può eliminare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public sealed override bool CanDelete(object item)
        {
            return this.CanDelete((T)item);
        }

        /// <summary>
        /// Restituisce true se l'utente può modificare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanEdit(T item)
        {
            return base.CanEdit(item);
        }

        /// <summary>
        /// Restituisce true se l'utente può modificare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public sealed override bool CanEdit(object item)
        {
            return this.CanEdit((T)item);
        }

        /// <summary>
        /// Restituisce true se l'utente può visualizzare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanList(T item)
        {
            return base.CanList(item);
        }

        /// <summary>
        /// Restituisce true se l'utente può visualizzare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public sealed override bool CanList(object item)
        {
            return this.CanList((T)item);
        }

        /// <summary>
        /// Restituisce true se l'utente può stampare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanPrint(T item)
        {
            return base.CanPrint(item);
        }

        /// <summary>
        /// Restituisce true se l'utente può stampare l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public sealed override bool CanPrint(object item)
        {
            return this.CanPrint((T)item);
        }
         

        protected sealed virtual string InternalEdit(object renderer, DBObjectBase value)
        {
              

            string ret = ""; // Me.list(renderer)
            ret += "<script type=\"text/javascript\">" + DMD.Strings.vbNewLine;
            ret += "Window.addListener(\"onload\", new Function('setTimeout(SystemUtils.EditModuleItem(" + Databases.GetID(Module) + ", " + Databases.GetID(value) + "), 500)'));";
            ret += "</script>";
            return ret;
        }

        protected virtual string InternalEdit(object renderer, T value)
        {
            base.InternalEdit(renderer, value);
        }

        protected sealed override void OnBeforeSave(object item)
        {
            this.OnBeforeSave((T)item);
        }

        protected virtual void OnBeforeSave(T item)
        {
        }

        protected sealed override void OnAfterSave(object item)
        {
            this.OnAfterSave((T)item);
        }

        protected virtual void OnAfterSave(T item)
        {
        }


        protected virtual object GetColumnValue(object renderer, T item, string key)
        {
            return base.GetColumnValue(renderer, item, key);
        }

        protected sealed override object GetColumnValue(object renderer, object item, string key)
        {
            return this.GetColumnValue(renderer, (T)item, key); 
        }

        protected sealed override void SetColumnValue(object renderer, object item, string key, object value)
        {
            this.GetColumnValue(renderer, (T)item, key, value);
        }

        protected virtual void SetColumnValue(object renderer, T item, string key, object value)
        {
            base.SetColumnValue(renderer, item, key, value);
        }
               
          
    }
}