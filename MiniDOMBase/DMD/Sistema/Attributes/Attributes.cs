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

namespace minidom.repositories
{

    /// <summary>
    /// Repository degli attributi degli oggetti
    /// </summary>
    [Serializable]
    public class CAttributesClass
        : CModulesClass<CObjectAttribute>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CAttributesClass()
            : base("modAttributes", typeof(CObjectAttributeCursor), 0)
        {
             
        }

        

        /// <summary>
        /// Restituisce il cursore degli attributi per l'oggetto
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CObjectAttributeCursor GetAttributeCursor(object obj)
        {
            var cursor = new CObjectAttributeCursor();
            cursor.ObjectID.Value = DBUtils.GetID(obj, 0);
            cursor.ObjectType.Value = DMD.RunTime.vbTypeName(obj);
            return cursor;
        }

        /// <summary>
        /// Restituisce l'attributo dell'oggetto
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public CObjectAttribute GetAttributeObject(object obj, string attributeName)
        {
            using (var cursor = this.GetAttributeCursor(obj))
            {
                cursor.AttributeName.Value = attributeName;
                return cursor.Item;
            }
        }

        /// <summary>
        /// Restituisce il valore dell'attributo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public string GetAttributeValue(object obj, string attributeName)
        {
            var a = this.GetAttributeObject(obj, attributeName);
            if (a is null)
                return null;
            return a.AttributeValue;
        }

        /// <summary>
        /// Imposta il valore dell'attributo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public CObjectAttribute SetAttributeValue(object obj, string attributeName, object attributeValue)
        {
            var a = GetAttributeObject(obj, attributeName);
            if (a is null)
            {
                a = new CObjectAttribute();
                a.Object = obj;
                a.AttributeName = attributeName;
            }

            a.AttributeValue = DMD.Strings.CStr(attributeValue);
            a.Save();

            return a;
        }
    }
}


namespace minidom
{
    public partial class Sistema
    {
        private static CAttributesClass m_Attributi = null;

        /// <summary>
        /// Repository degli attributi
        /// </summary>
        public static CAttributesClass Attributi
        {
            get
            {
                if (m_Attributi is null) m_Attributi = new CAttributesClass();
                return m_Attributi;
            }
        }
    }
}