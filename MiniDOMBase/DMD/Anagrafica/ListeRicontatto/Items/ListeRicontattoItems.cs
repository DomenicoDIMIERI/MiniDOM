using System;
using DMD;
using DMD.Databases;
using static minidom.Anagrafica;

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti <see cref="ListaRicontattoItem"/>
    /// </summary>
    [Serializable]
    public sealed class CListeRicontattoItemsClass 
        : CModulesClass<Anagrafica.ListaRicontattoItem>
    {
        /// <summary>
        /// Costruttore
        /// </summary>
        public CListeRicontattoItemsClass() 
            : base("modListeRicontattoItem", typeof(Anagrafica.ListaRicontattoItemCursor), 0)
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
        /// Gestisce l'evento PersonaMerged
        /// </summary>
        /// <param name="e"></param>
        private void HandlePeronaMerged(MergePersonaEventArgs e)
        {
            lock (this.cacheLock)
            {
                var mi = e.MI;
                var persona = mi.Persona1;
                var persona1 = mi.Persona2;
                CMergePersonaRecord rec;
                
                // Tabella tbl_ListeRicontattoItems 
                using(var cursor = new ListaRicontattoItemCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.IDPersona.Value = mi.IDPersona2;
                    while (cursor.Read ())
                    {
                        rec = new CMergePersonaRecord();
                        rec.NomeTabella = "tbl_ListeRicontattoItems";
                        rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                        rec.FieldName = "IDPersona";
                        mi.TabelleModificate.Add(rec);

                        cursor.Item.Persona = mi.Persona1;
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
                var items = mi.GetAffectedRecorsIDs("tbl_ListeRicontattoItems", "IDPersona");
                using (var cursor = new ListaRicontattoItemCursor())
                {
                    cursor.ID.ValueIn(items);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        cursor.Item.Persona = mi.Persona2;
                        cursor.Item.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Gestisce l'evento PersonaChanged
        /// </summary>
        /// <param name="e"></param>
        private void HandlePeronaModified(PersonaEventArgs e)
        {
            lock (this.cacheLock)
            {
                var p = e.Persona;
#if (!DEBUG)
                try {
#endif
                //ListeRicontatto.Database.ExecuteCommand("UPDATE [tbl_ListeRicontattoItems] "
                // SET [DettaglioStato]=" + Databases.DBUtils.DBString(p.DettaglioEsito) + ", 
                //     [DettaglioStato1]=" + Databases.DBUtils.DBString(p.DettaglioEsito1) + 
                //" WHERE [IDPersona]=" + DBUtils.GetID(p) + " AND 
                // [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [StatoRicontatto]=" + ((int)StatoRicontatto.PROGRAMMATO).ToString());
                using(var cursor = new ListaRicontattoItemCursor())
                {
                    cursor.IDPersona.Value = DBUtils.GetID(p, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoRicontatto.ValueIn(new StatoRicontatto[] { StatoRicontatto.PROGRAMMATO, StatoRicontatto.RIMANDATO });
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        //cursor.Item.PuntoOperativo = p.PuntoOperativo;
                        cursor.Item.DettaglioStato = p.DettaglioEsito;
                        cursor.Item.DettaglioStato1 = p.DettaglioEsito1;
                        cursor.Item.Save();
                    }
                }
#if (!DEBUG)
                } catch (Exception ex) { Sistema.Events.NotifyUnhandledException(ex); }
#endif
                // End If
            }
        }


        /// <summary>
        /// Restituisce l'elemento in base all'oggetto che lo ha programmato
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ListaRicontattoItem GetRicontattoBySource(object source)
        {
            return GetRicontattoBySource(DMD.RunTime.vbTypeName(source), DBUtils.GetID(source).ToString());
        }

        /// <summary>
        /// Restituisce l'elemento in base all'oggetto che lo ha programmato
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public ListaRicontattoItem GetRicontattoBySource(string sourceName, string param)
        {
            sourceName = DMD.Strings.Trim(sourceName);
            param = DMD.Strings.Trim(param);
            if (string.IsNullOrEmpty(sourceName) && string.IsNullOrEmpty(param))
                return null;

            using (var cursor = new ListaRicontattoItemCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.SourceName.Value = sourceName;
                cursor.SourceParam.Value = param;
                cursor.ID.SortOrder = SortEnum.SORT_DESC;
                cursor.IgnoreRights = true;
                return cursor.Item;
            }

        }
    }
}
