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
    public partial class Sistema
    {

        /// <summary>
        /// Provider registrabile su db
        /// </summary>
        [Serializable]
        public class CRegisteredCalendarProvider 
            : Databases.DBObjectBase
        {

            private string m_Nome;
            private Type m_Type;
            [NonSerialized] private ICalendarProvider m_Instance;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredCalendarProvider()
            {
                this.m_Nome = string.Empty;
                this.m_Type = null;
                this.m_Instance = null;
                this.m_Flags = 0;
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="type"></param>
            public CRegisteredCalendarProvider(string nome, Type type)
                : this()
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException("nome");
                if (type is null)
                    throw new ArgumentNullException("type");
                m_Nome = nome;
                m_Type = type;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Calendar.RegisteredCalendarProviders;
            }

             
            

            /// <summary>
            /// Nome del provider
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Tipo dell'oggetto che implementa l'interfaccia <see cref="ICalendarProvider"/>
            /// </summary>
            public Type Type
            {
                get
                {
                    return m_Type;
                }
            }

            /// <summary>
            /// Istanza dell'oggetto <see cref="ICalendarProvider" />
            /// </summary>
            public ICalendarProvider Instance
            {
                get
                {
                    if (m_Instance is null)
                    {
                        m_Instance = (ICalendarProvider)DMD.RunTime.CreateInstance(Type);
                    }

                    return m_Instance;
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_RegisteredCalendarProviders";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Type = DMD.RunTime.FindType(reader.Read("ClassName", ""));
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("ClassName", (this.m_Type is null)? "": m_Type.FullName);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("ClassName", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "ClassName", "Flags" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("ClassName", (this.m_Type is null) ? "" : m_Type.FullName);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName)
                {
                    case "Nome": this.m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "ClassName": this.m_Type = DMD.RunTime.FindType( DMD.XML.Utils.Serializer.DeserializeString(fieldValue)); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
                        
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome, this.m_Type);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CRegisteredCalendarProvider) && this.Equals((CRegisteredCalendarProvider)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRegisteredCalendarProvider obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && this.m_Type.Equals(obj.m_Type)
                    ;
                    //CKeyCollection m_Parameters;
            }

             
        }
    }
}