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
    namespace repositories
    {

        /// <summary>
        /// Reposutory di oggetti di tipo <see cref="CAttachment"/>
        /// </summary>
        [Serializable]
        public class CAttachmentsClass 
            : CModulesClass<Sistema.CAttachment>
        { 

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAttachmentsClass()
                : base("modAttachments", typeof(Sistema.CAttachmentsCursor))
            {
            }

            /// <summary>
            /// Inizializza il modulo
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaCreated += HandlePeronaModified;
                minidom.Anagrafica.PersonaDeleted += HandlePeronaModified;
                minidom.Anagrafica.PersonaModified += HandlePeronaModified;
                minidom.Anagrafica.PersonaMerged += HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
            }

            /// <summary>
            /// Termina il modulo
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaCreated -= HandlePeronaModified;
                minidom.Anagrafica.PersonaDeleted -= HandlePeronaModified;
                minidom.Anagrafica.PersonaModified -= HandlePeronaModified;
                minidom.Anagrafica.PersonaMerged -= HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
                base.Terminate();
            }

            /// <summary>
            /// Gestisce l'evento PersonaMoficied
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaModified(PersonaEventArgs e)
            {
                lock (this.cacheLock)
                {


                }
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaMerged(MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;

                    // Tabella tbl_Attachments
                    //dbSQL = "SELECT [ID] FROM [tbl_Attachments] 
                    //WHERE [OwnerType]='" + DMD.RunTime.vbTypeName(persona) + "' And [OwnerID]=" + DBUtils.GetID(persona1);
                    using(var cursor = new CAttachmentsCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.OwnerID.Value = mi.IDPersona2;
                        cursor.OwnerType.Value = DMD.RunTime.vbTypeName(mi.Persona2);
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Attachments";
                            rec.FieldName = "OwnerID";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.SetOwner(mi.Persona1);
                            cursor.Item.Save();
                        }
                    }

                }
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaUnMerged(MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    // Tabella tbl_Attachments
                    var items = mi.GetAffectedRecorsIDs("tbl_Attachments", "OwnerID");
                    //if (!string.IsNullOrEmpty(items))
                    //    Sistema.Attachments.Database.ExecuteCommand(DMD.Strings.JoinW("UPDATE [tbl_Attachments] SET [OwnerID]=", DBUtils.GetID(persona1).ToString(), " WHERE [ID] In (", items, ")"));
                    using (var cursor = new CAttachmentsCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.IDOwner = mi.IDPersona2;
                            cursor.Item.Save();
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce i tipi di contesto validi per l'oggetto
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public CCollection<string> GetTipiContestoPerOggetto(object obj)
            {
                if (obj is null)
                    throw new ArgumentNullException("obj");
                return GetTipiContestoPerOggetto(DMD.RunTime.vbTypeName(obj), DBUtils.GetID(obj, 0));
            }

            /// <summary>
            /// Restituisce i tipi di contesto validi per l'oggetto
            /// </summary>
            /// <param name="objectType"></param>
            /// <param name="objectID"></param>
            /// <returns></returns>
            public CCollection<string> GetTipiContestoPerOggetto(string objectType, int objectID)
            {
                var dic = new Dictionary<string, string>();
                using (var cursor = new CAttachmentsCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.OwnerID.Value = objectID;
                    cursor.OwnerType.Value = objectType;
                    cursor.TipoContesto.Value = null;
                    cursor.TipoContesto.Operator = OP.OP_NE;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        string tipoContesto = cursor.Item.TipoContesto;
                        int idContesto = cursor.Item.IDContesto;
                        var buff = new System.Text.StringBuilder();
                        buff.Append(tipoContesto);
                        buff.Append(" (");
                        buff.Append(DMD.Strings.PadLeft(Strings.CStr(idContesto), '0', 8));
                        buff.Append(")");

                        var str = buff.ToString();
                        if (!dic.ContainsKey(str))
                            dic.Add(str, str);
                    }

                }

                var ret = new CCollection<string>();
                foreach (var str in dic.Keys)
                {
                    ret.Add(str);
                }
                ret.Sort();

                return ret;
            }
        }
    }

    public partial class Sistema
    {
        private static CAttachmentsClass m_Attachments = null;

        /// <summary>
        /// Repository degli allegati
        /// </summary>
        public static CAttachmentsClass Attachments
        {
            get
            {
                if (m_Attachments is null)
                    m_Attachments = new CAttachmentsClass();
                return m_Attachments;
            }
        }
    }
}