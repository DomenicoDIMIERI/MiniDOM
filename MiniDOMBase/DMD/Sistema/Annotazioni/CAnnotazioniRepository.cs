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
        /// Repository delle annotazioni
        /// </summary>
        [Serializable]
        public class CAnnotazioniClass 
            : CModulesClass<Sistema.CAnnotazione>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CAnnotazioniClass() 
                : base("modAnnotazioni", typeof(Sistema.CAnnotazioniCursor))
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

                    // Tabella tbl_Annotazioni
                    //dbSQL = "SELECT [ID] FROM [tbl_Annotazioni] 
                    //WHERE [OwnerType]='" + DMD.RunTime.vbTypeName(persona) + "' And [OwnerID]=" + DBUtils.GetID(persona1);
                    using(var cursor = new CAnnotazioniCursor() )
                    {
                        cursor.IgnoreRights = true;
                        cursor.OwnerType.Value = DMD.RunTime.vbTypeName(mi.Persona2);
                        cursor.OwnerID.Value = mi.IDPersona2;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Annotazioni";
                            rec.FieldName = "OwnerID";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.SetOwner(mi.Persona1);
                            cursor.Item.Save();
                        }
                    }
                      
                    //Sistema.Annotazioni.Database.ExecuteCommand("UPDATE [tbl_Annotazioni] SET [OwnerID]=" + DBUtils.GetID(persona) + " WHERE [OwnerType]='" + DMD.RunTime.vbTypeName(persona) + "' And [OwnerID]=" + DBUtils.GetID(persona1));


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
                    // Tabella tbl_Annotazioni
                    var items = mi.GetAffectedRecorsIDs("tbl_Annotazioni", "OwnerID");
                    //Sistema.Annotazioni.Database.ExecuteCommand(DMD.Strings.JoinW("UPDATE [tbl_Annotazioni] SET [OwnerID]=", DBUtils.GetID(persona1).ToString(), " WHERE [ID] In (", items, ")"));
                    using (var cursor = new CAnnotazioniCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.SetOwner(mi.Persona2);
                            cursor.Item.Save();
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce l'annotazione creata dalla sorgente
            /// </summary>
            /// <param name="sourceName"></param>
            /// <param name="sourceParam"></param>
            /// <returns></returns>
            public Sistema.CAnnotazione GetItemBySource(string sourceName, string sourceParam)
            {
                sourceName = DMD.Strings.Trim(sourceName);
                sourceParam = DMD.Strings.Trim(sourceParam);
                if (string.IsNullOrEmpty(sourceName) && string.IsNullOrEmpty(sourceParam))
                    return null;

                using (var cursor = new Sistema.CAnnotazioniCursor())
                { 
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    cursor.SourceName.Value = sourceName;
                    cursor.SourceParam.Value = sourceParam;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce una collezione inizializzata con i tipo di contesto registrati per l'oggetto
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
            /// Restituisce una collezione inizializzata con i tipo di contesto registrati per l'oggetto
            /// </summary>
            /// <param name="objectType"></param>
            /// <param name="objectID"></param>
            /// <returns></returns>
            public CCollection<string> GetTipiContestoPerOggetto(string objectType, int objectID)
            {
                //var dbSQL = new System.Text.StringBuilder();
                //dbSQL.Append("SELECT Count(*), [TipoContesto], [IDContesto] FROM [tbl_Annotazioni] WHERE [OwnerID]=");
                //dbSQL.Append(objectID);
                //dbSQL.Append(" AND [OwnerType]=");
                //dbSQL.Append(Databases.DBUtils.DBString(objectType));
                //dbSQL.Append(" AND Not ([TipoContesto] Is Null) AND [Stato]=");
                //dbSQL.Append((int)ObjectStatus.OBJECT_VALID);
                //dbSQL.Append(" GROUP BY [TipoContesto], [IDContesto]");
                //dbRis = Database.ExecuteReader(dbSQL.ToString());
                var dic = new Dictionary<string, string>();
                using(var cursor = new CAnnotazioniCursor())
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
                foreach(var str in dic.Keys)
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
        private static CAnnotazioniClass m_Annotazioni = null;

        /// <summary>
        /// Repository delle annotazioni
        /// </summary>
        public static CAnnotazioniClass Annotazioni
        {
            get
            {
                if (m_Annotazioni is null)
                    m_Annotazioni = new CAnnotazioniClass();
                return m_Annotazioni;
            }
        }
    }
}