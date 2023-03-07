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
        /// Repository di <see cref="Dispositivo"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CDispositiviClass
            : CModulesClass<Dispositivo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDispositiviClass() 
                : base("modOfficeDevices", typeof(minidom.Office.DispositivoCursor), -1)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeDevices");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeDevices");
            //        ret.Description = "Dispositivi";
            //        ret.DisplayName = "Dispositivi";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!minidom.Office.Database.Tables.ContainsKey("tbl_OfficeDevices"))
            //    {
            //        var table = minidom.Office.Database.Tables.Add("tbl_OfficeDevices");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("UserID", TypeCode.Int32);
            //        field = table.Fields.Add("Nome", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Tipo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Modello", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("DataAcquisto", TypeCode.DateTime);
            //        field = table.Fields.Add("DataDismissione", TypeCode.DateTime);
            //        field = table.Fields.Add("StatoDispositivo", TypeCode.Int32);
            //        field = table.Fields.Add("Seriale", TypeCode.String);
            //        field.MaxLength = 255;
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
            /// Restituisce l'elemento in base al numero seriale
            /// </summary>
            /// <param name="seriale"></param>
            /// <returns></returns>
            public minidom.Office.Dispositivo GetItemBySeriale(string seriale)
            {
                seriale = Strings.Trim(seriale);
                if (string.IsNullOrEmpty(seriale))
                    return null;
                foreach (var ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.Seriale, seriale, true) == 0)
                        return ret;
                }

                return null;
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Dispositivo GetItemByName(string name)
            {
                name = Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (var ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.Nome, name, true) == 0)
                        return ret;
                }

                return null;
            }
        }
    }

    public partial class Office
    {
        private static CDispositiviClass m_Dispotivi = null;

        /// <summary>
        /// Repository di <see cref="Dispositivo"/>
        /// </summary>
        public static CDispositiviClass Dispositivi
        {
            get
            {
                if (m_Dispotivi is null)
                    m_Dispotivi = new CDispositiviClass();
                return m_Dispotivi;
            }
        }
    }
}