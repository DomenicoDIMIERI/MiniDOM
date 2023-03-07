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

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Flag definiti su un device
        /// </summary>
        [Flags]
        public enum DeviceFlags
        {
            /// <summary>
            /// Nessun fla
            /// </summary>
            None = 0,

            /// <summary>
            /// Se vero indica che il tipo di dispositivo supporta le funzioni di localizzazione GPS
            /// </summary>
            CanGPS = 1,

            /// <summary>
            /// Se vero indica che il dispositivo può stampare dei documenti cartacei
            /// </summary>
            CanPrint = 2,

            /// <summary>
            /// Se vero indica che il dispositivo può effettuare scansioni
            /// </summary>
            CanScan = 4,

            /// <summary>
            /// Se vero indica che il dispositivo supporta il riconoscimento tramite impronte digitali
            /// </summary>
            CanFingerprint = 8,

            /// <summary>
            /// Se vero indica che il dispositivo può effettuare riconoscimenti tramite metodi diversi dall'impronta digitale
            /// </summary>
            CanRecognizeOther = 16,

            /// <summary>
            /// Se vero indica che il dispositivo è utilizzabile come destkop
            /// </summary>
            IsDesktop = 32,

            /// <summary>
            /// Se vero indica che il dispositivo è utilizzabile come telefono per le conversazioni
            /// </summary>
            IsPhone = 64,

            /// <summary>
            /// Se vero indica che il dispositivo supporta l'invio o la ricezione di SMS
            /// </summary>
            IsSMS = 128
        }

        /// <summary>
        /// Rappresenta la categoria di un dispositivo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ClasseDispositivo 
            : minidom.Databases.DBObject
        {
            private string m_Nome;
            private string m_IconUrl;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ClasseDispositivo()
            {
                m_Nome = "";
                m_Flags = (int)DeviceFlags.None;
                m_IconUrl = "";
            }


            /// <summary>
            /// Restituisce o imposta il nome del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags che descrivo la categoria
            /// </summary>
            /// <returns></returns>
            public new DeviceFlags Flags
            {
                get
                {
                    return (DeviceFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

          //TODO eliminare params in javascript

            /// <summary>
            /// Restituisce o imposta l'icona associata alla classe di dispositivi
            /// </summary>
            public string IconURL
            {
                get
                {
                    return m_IconUrl;
                }

                set
                {
                    string oldValue = m_IconUrl;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconUrl = value;
                    DoChanged("IconUrl", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is ClasseDispositivo) && this.Equals((ClasseDispositivo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ClasseDispositivo obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_IconUrl, obj.m_IconUrl)
                    ;

            }


            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.ClassiDispositivi;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeDevClass";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_IconUrl = reader.Read("IconUrl", this.m_IconUrl);
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
                writer.Write("IconUrl", m_IconUrl);
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
                c = table.Fields.Ensure("IconUrl", typeof(string), 255);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey );
                //c = table.Fields.Ensure("IconUrl", typeof(string), 255);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IconUrl", m_IconUrl);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome": this.m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "IconUrl": this.m_IconUrl = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            
        }
    }
}