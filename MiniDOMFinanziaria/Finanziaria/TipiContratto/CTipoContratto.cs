using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum TipoContrattoFlags : int
        {
            None = 0,
            Attivo = 1
        }

        public class CTipoContratto : Databases.DBObjectBase, IComparable
        {
            private string m_IdTipoContratto;
            private string m_Descrizione;
            private TipoContrattoFlags m_Flags;

            public CTipoContratto()
            {
                m_IdTipoContratto = DMD.Strings.vbNullString;
                m_Descrizione = DMD.Strings.vbNullString;
                m_Flags = TipoContrattoFlags.Attivo;
            }

            /// <summary>
        /// Restituisce o imposta una stringa univoca
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IdTipoContratto
            {
                get
                {
                    return m_IdTipoContratto;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_IdTipoContratto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IdTipoContratto = value;
                    DoChanged("IdTipoContratto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la descrizione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta i flags
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoContrattoFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica se questo oggetto è arrivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, TipoContrattoFlags.Attivo);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, TipoContrattoFlags.Attivo, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IdTipoContratto = reader.Read("IdTipoContratto", this.m_IdTipoContratto);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_Flags = reader.Read("Flags", this.m_Flags);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IdTipoContratto", m_IdTipoContratto);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Flags", m_Flags);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IdTipoContratto", m_IdTipoContratto);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IdTipoContratto":
                        {
                            m_IdTipoContratto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (TipoContrattoFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return TipiContratto.Module;
            }

            public override string GetTableName()
            {
                return "Tipocontratto";
            }

            public int CompareTo(CTipoContratto obj)
            {
                return DMD.Strings.Compare(m_Descrizione, obj.m_Descrizione, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CTipoContratto)obj);
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                TipiContratto.UpdateCached(this);
            }
        }
    }
}