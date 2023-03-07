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
    /// Anomalia
    /// </summary>
    [Serializable]
    public class Anomalia 
        : IComparable, IDMDXMLSerializable, IComparable<Anomalia>
    {
        /// <summary>
        /// Oggetto
        /// </summary>
        [NonSerialized] public object Oggetto;

        /// <summary>
        /// Id dell'ufficio
        /// </summary>
        public int IDUfficio;
        [NonSerialized] private CUfficio m_Ufficio;
        
        /// <summary>
        /// Id dell'operatore
        /// </summary>
        public int IDOperatore;
        private Sistema.CUser m_Operatore;
        
        /// <summary>
        /// Descrizione
        /// </summary>
        public string Descrizione;

        /// <summary>
        /// Importnza
        /// </summary>
        public int Importanza;

        /// <summary>
        /// Costruttore
        /// </summary>
        public Anomalia()
        {
            //DMDObject.IncreaseCounter(this);
            this.Oggetto = null;
            this.IDUfficio = 0;
            this.m_Ufficio = null;
            this.IDOperatore = 0;
            this.m_Operatore = null;
            this.Descrizione = "";
            this.Importanza = 0;
        }

        /// <summary>
        /// Restituisce o imposta l'ufficio
        /// </summary>
        public CUfficio Ufficio
        {
            get
            {
                if (m_Ufficio is null)
                    m_Ufficio = Anagrafica.Uffici.GetItemById(IDUfficio);
                return m_Ufficio;
            }

            set
            {
                m_Ufficio = value;
                IDUfficio = DBUtils.GetID(value, 0);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'operatore
        /// </summary>
        public Sistema.CUser Operatore
        {
            get
            {
                if (m_Operatore is null)
                    m_Operatore = Sistema.Users.GetItemById(IDOperatore);
                return m_Operatore;
            }

            set
            {
                m_Operatore = value;
                IDOperatore = DBUtils.GetID(value, 0);
            }
        }

        /// <summary>
        /// Compara due oggetti
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo(Anomalia obj)
        {
            int ret = DMD.Arrays.Compare(this.Importanza, obj.Importanza);
            if (ret == 0) ret = DMD.Strings.Compare(this.Descrizione, obj.Descrizione, true);
            return ret;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((Anomalia)obj);
        }

        /// <summary>
        /// Deserializzazione xml
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        protected internal virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "IDOperatore":
                    {
                        IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "IDUfficio":
                    {
                        IDUfficio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Descrizione":
                    {
                        Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Importanza":
                    {
                        Importanza = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Oggetto":
                    {
                        Oggetto = DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }
            }
        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected internal virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IDOperaore", IDOperatore);
            writer.WriteAttribute("IDUfficio", IDUfficio);
            writer.WriteAttribute("Descrizione", Descrizione);
            writer.WriteAttribute("Importanza", Importanza);
            writer.WriteTag("Oggetto", Oggetto);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

         
    }
}