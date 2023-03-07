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

    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="Veicolo"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CVeicoliClass
            : CModulesClass<Veicolo>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CVeicoliClass() 
                : base("modOfficeVeicoli", typeof(VeicoliCursor), -1)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeVeicoli");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeVeicoli");
            //        ret.Description = "Veicoli";
            //        ret.DisplayName = "Veicoli";
            //        ret.Parent = Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!Database.Tables.ContainsKey("tbl_OfficeVeicoli"))
            //    {
            //        var table = Database.Tables.Add("tbl_OfficeVeicoli");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("Nome", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Tipo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Modello", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("DataAcquisto", TypeCode.DateTime);
            //        field = table.Fields.Add("DataDismissione", TypeCode.DateTime);
            //        field = table.Fields.Add("StatoVeicolo", TypeCode.Int32);
            //        field = table.Fields.Add("Alimentazione", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Seriale", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("IconURL", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("KmALitro", TypeCode.Single);
            //        field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32);
            //        field = table.Fields.Add("NomePuntoOperativo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("CreatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("CreatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("ModificatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("ModificatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("Stato", TypeCode.Int32);
            //        table.Create();
            //    }

            //    return ret;
            //}

            /// <summary>
            /// Restituisce il veicolo in base al numero seriale
            /// </summary>
            /// <param name="seriale"></param>
            /// <returns></returns>
            public Veicolo GetItemBySeriale(string seriale)
            {
                seriale = Strings.Trim(seriale);
                if (string.IsNullOrEmpty(seriale))
                    return null;
                foreach (Veicolo item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Seriale, seriale, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Interpreta la targa del veicolo
            /// </summary>
            /// <param name="targa"></param>
            /// <returns></returns>
            public string ParseTarga(string targa)
            {
                return DMD.Strings.UCase(DMD.Strings.Replace(targa, " ", ""));
            }

            /// <summary>
            /// Formatta la targa del veicolo
            /// </summary>
            /// <param name="targa"></param>
            /// <returns></returns>
            public string FormatTarga(string targa)
            {
                return DMD.Strings.UCase(DMD.Strings.Replace(targa, " ", ""));
            }
        }

    }

    public partial class Office
    {

      
        private static CVeicoliClass m_Veicoli = null;

        /// <summary>
        /// Repository di <see cref="Veicolo"/>
        /// </summary>
        public static CVeicoliClass Veicoli
        {
            get
            {
                if (m_Veicoli is null)
                    m_Veicoli = new CVeicoliClass();
                return m_Veicoli;
            }
        }
    }
}