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
        /// Repository di oggetti <see cref="ClasseDispositivo"/> 
        /// </summary>
        public sealed class CClassiDispositiviClass 
            : CModulesClass<ClasseDispositivo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CClassiDispositiviClass() 
                : base("modOfficeDevClass", typeof(minidom.Office.ClassiDispositivoCursor), -1)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeDevClass");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeDevClass");
            //        ret.Description = "Classi Dispositivi";
            //        ret.DisplayName = "Classi Dispositivi";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!minidom.Office.Database.Tables.ContainsKey("tbl_OfficeDevClass"))
            //    {
            //        var table = minidom.Office.Database.Tables.Add("tbl_OfficeDevClass");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("Nome", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Flags", TypeCode.Int32);
            //        field = table.Fields.Add("Params", TypeCode.String);
            //        field.MaxLength = 0;
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
            /// Restituisce il dispositivo in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public minidom.Office.ClasseDispositivo GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;

                foreach (var cls in this.LoadAll())
                {
                    if (string.Compare(cls.Nome, nome, true) == 0)
                        return cls;
                }

                return null;
            }
        }
    }

    public partial class Office
    {
        private static CClassiDispositiviClass m_ClassiDispositivo = null;

        /// <summary>
        /// Repository di oggetti <see cref="ClasseDispositivo"/> 
        /// </summary>
        public static CClassiDispositiviClass ClassiDispositivi
        {
            get
            {
                if (m_ClassiDispositivo is null)
                    m_ClassiDispositivo = new CClassiDispositiviClass();
                return m_ClassiDispositivo;
            }
        }
    }
}