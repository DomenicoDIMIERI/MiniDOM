using System;
using DMD;
using DMD.Databases;
using minidom.repositories;
using static minidom.Anagrafica;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository per gli oggetti di tipo <see cref="CImpiegato"/>
        /// </summary>
        [Serializable]
        public sealed class CImpieghiRepository
            : CModulesClass<CImpiegato>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CImpieghiRepository()
                : base("modAnaImpieghi", typeof(CImpiegatiCursor), 0)
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

                    if (persona2 is CPersonaFisica )
                    {
                        // Tabella tbl_Impiegati
                        //dbSQL = "SELECT [ID] FROM [tbl_Impiegati] WHERE [Persona]=" + DBUtils.GetID(persona1);
                        using(var cursor = new CImpiegatiCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.PersonaID.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_Impiegati";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "Persona";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Persona = (CPersonaFisica) mi.Persona1;
                                cursor.Item.Save();
                            }
                        }
                         
                    }
                    else
                    {
                        using (var cursor = new CImpiegatiCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.AziendaID.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_Impiegati";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "Azienda";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Azienda = (CAzienda)mi.Persona1;
                                cursor.Item.Save();
                            }
                        }

                        using (var cursor = new CImpiegatiCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDEntePagante.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_Impiegati";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDEntePagante";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.EntePagante = (CAzienda)mi.Persona1;
                                cursor.Item.Save();
                            }
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
                    
                    if (mi.Persona2 is CPersonaFisica)
                    {
                        // Tabella tbl_Impiegati
                        var items = mi.GetAffectedRecorsIDs("tbl_Impiegati", "Persona");
                        using (var cursor = new CImpiegatiCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.IDPersona = mi.IDPersona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                    else
                    {
                        // Tabella tbl_Impiegati (Azienda)
                        var items = mi.GetAffectedRecorsIDs("tbl_Impiegati", "Azienda");
                        using (var cursor = new CImpiegatiCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.IDAzienda = mi.IDPersona2;
                                cursor.Item.NomeAzienda = mi.NomePersona2;
                                cursor.Item.Save();
                            }
                        }
                        // Tabella tbl_Impiegati (Ente Pagante)
                        items = mi.GetAffectedRecorsIDs("tbl_Impiegati", "IDEntePagante");
                        //if (!string.IsNullOrEmpty(items))
                        //        Databases.APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [IDEntePagante]=" + DBUtils.GetID(persona1) + ", [NomeEntePagante]=" + Databases.DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                        using (var cursor = new CImpiegatiCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.IDEntePagante = mi.IDPersona2;
                                cursor.Item.NomeEntePagante = mi.NomePersona2;
                                cursor.Item.Save();
                            }
                        }
 
                    }

                }
            }


        }
    }

    public partial class Anagrafica
    {
        private static CImpieghiRepository m_Impieghi = null;

        /// <summary>
        /// Repository per gli oggetti di tipo <see cref="CImpiegato"/>
        /// </summary>
        public static CImpieghiRepository Impieghi
        {
            get
            {
                if (m_Impieghi is null)
                    m_Impieghi = new CImpieghiRepository();
                return m_Impieghi;
            }
        }
    }
}