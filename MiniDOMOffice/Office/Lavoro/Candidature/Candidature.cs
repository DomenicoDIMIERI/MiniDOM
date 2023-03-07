﻿using System;
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
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="Candidatura"/>
        /// </summary>
        [Serializable]
        public class CCandidatureClass 
            : CModulesClass<Candidatura>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCandidatureClass() 
                : base("modOfficeCandidature", typeof(CandidaturaCursor), 0)
            {
            }

            /// <summary>
            /// Inizializza le risorse
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaMerged += this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged += this.HandlePersonaUnMerged;
            }

            /// <summary>
            /// Rilascia le risorse
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaMerged -= this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged -= this.HandlePersonaUnMerged;
                base.Terminate();
            }

            /// <summary>
            /// Gestisce il merge delle persone
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            protected virtual void HandlePersonaMerged(object sender, MergePersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    if (persona1 is CPersonaFisica)
                    {
                        // Tabella tbl_OfficeEstrattiC 
                        using (var cursor = new CandidaturaCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDCandidato.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_OfficeCandidature";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDCandidato";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Candidato = (CPersonaFisica)mi.Persona1;
                                cursor.Item.Save();
                            }
                        }
                    }
                     

                }
            }

            /// <summary>
            /// Gestisce l'evento Unmerge di una persona fisica o di un'azienda
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected virtual void HandlePersonaUnMerged(object sender, MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    if (mi.Persona1 is CPersonaFisica)
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_OfficeCandidature", "IDCandidato");
                        using (var cursor = new CandidaturaCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.Candidato = (CPersonaFisica)mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                     
                }
            }

        }
    }

    public partial class Office
    {
        private static CCandidatureClass m_Candidature = null;

        /// <summary>
        /// Repository di <see cref="Candidatura"/>
        /// </summary>
        public static CCandidatureClass Candidature
        {
            get
            {
                if (m_Candidature is null)
                    m_Candidature = new CCandidatureClass();
                return m_Candidature;
            }
        }
    }
}