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
        /// Rappresenta un device di tipo rilevatore presenze
        /// TODO javascript RilevatorePresenze
        /// </summary>
        [Serializable]
        public class RilevatorePresenze 
            : CRegisteredDevice
        {
             
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public RilevatorePresenze()
            {
                 
                 
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.RilevatoriPresenze;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeRilevP";
            }
              

            /// <summary>
            /// Sincronizza l'orario sul dispositivo
            /// </summary>
            public void SincronizzaOrario()
            {
                var driver = RilevatoriPresenze.Drivers.Find(this);
                if (driver is null)
                    throw new Exception("Nessun driver installato per il tipo di rilevatore:" + Tipo);
                driver.SincronizzaOrario(this);
            }

            /// <summary>
            /// Scarica le presenze
            /// </summary>
            public CCollection<MarcaturaIngressoUscita> ScaricaNuoveMarcature()
            {
                var driver = RilevatoriPresenze.Drivers.Find(this);
                if (driver is null)
                    throw new Exception("Nessun driver installato per il tipo di rilevatore:" + Tipo);
                return driver.ScaricaNuoveMarcature(this);
            }
  
        }
    }
}