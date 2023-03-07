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
        /// Repository di <see cref="OffertaDiLavoro"/>
        /// </summary>
        [Serializable]
        public sealed class COfferteDiLavoroClass
            : CModulesClass<OffertaDiLavoro>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public COfferteDiLavoroClass() 
                : base("modOfficeOfferteDiLavoro", typeof(OffertaDiLavoroCursor))
            {
            }


            /// <summary>
            /// Restituisce la collezione delle candidature associate all'offerta di lavoro
            /// </summary>
            /// <param name="offerta"></param>
            /// <returns></returns>
            public CCollection<Candidatura> GetCandidature(OffertaDiLavoro offerta)
            {
                if (offerta is null)
                    throw new ArgumentNullException("offerta");

                var ret = new CCollection<Candidatura>();

                using (var cursor = new CandidaturaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDOfferta.Value = DBUtils.GetID(offerta, 0);
                    while (cursor.Read())
                    {
                        var item = cursor.Item;
                        item.SetOfferta(offerta);
                        ret.Add(item);
                    }
                }

                return ret;
            }
        }

        private static COfferteDiLavoroClass m_OfferteDiLavoro = null;

        /// <summary>
        /// Repository di <see cref="OffertaDiLavoro"/>
        /// </summary>
        public static COfferteDiLavoroClass OfferteDiLavoro
        {
            get
            {
                if (m_OfferteDiLavoro is null)
                    m_OfferteDiLavoro = new COfferteDiLavoroClass();
                return m_OfferteDiLavoro;
            }
        }
    }
}