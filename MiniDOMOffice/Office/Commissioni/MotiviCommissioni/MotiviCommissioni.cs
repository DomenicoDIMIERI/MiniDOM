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
        /// Repository di oggetti <see cref="MotivoCommissione"/>
        /// </summary>
        [Serializable]
        public partial class CMotiviCommissioniClass
            : CModulesClass<MotivoCommissione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMotiviCommissioniClass() 
                : base("modOfficeMotiviCommissioni", typeof(MotivoCommissioneCursor), -1)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeMotiviCommissioni");
            //        ret.Description = "Motivi Commissioni";
            //        ret.DisplayName = "Motivi Commissioni";
            //        ret.Parent = Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!Database.Tables.ContainsKey("tbl_OfficeCommissioniM"))
            //    {
            //        var table = Database.Tables.Add("tbl_OfficeCommissioniM");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("Motivo", TypeCode.String);
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

            //public CCollection<string> GetMotiviAsCollection()
            //{
            //    // Dim cursor As New MotivoCommissioneCursor
            //    // Try
            //    // Dim ret As New CCollection(Of String)
            //    // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            //    // cursor.Motivo.SortOrder = SortEnum.SORT_ASC
            //    // cursor.IgnoreRights = True
            //    // While Not cursor.EOF
            //    // ret.Add(cursor.Item.Motivo)
            //    // cursor.MoveNext()
            //    // End While
            //    // Return ret
            //    // Catch ex As Exception
            //    // Throw
            //    // Finally
            //    // cursor.Dispose()
            //    // End Try
            //    var ret = new CCollection<string>();
            //    foreach (MotivoCommissione m in LoadAll())
            //        ret.Add(m.Motivo);
            //    ret.Sort();
            //    return ret;
            //}
        }

        /// <summary>
        /// Gestione delle commissioni
        /// </summary>
        /// <remarks></remarks>
        public sealed partial class CCommissioniClass
        {  

            private CMotiviCommissioniClass m_Motivi = null;

            /// <summary>
            /// Repository di oggetti <see cref="MotivoCommissione"/>
            /// </summary>
            public CMotiviCommissioniClass Motivi 
            {
                get
                {
                    if (m_Motivi is null)
                        m_Motivi = new CMotiviCommissioniClass();
                    return m_Motivi;
                }
            }
        }
    }

    
}