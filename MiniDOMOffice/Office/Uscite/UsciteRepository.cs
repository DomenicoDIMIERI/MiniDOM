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
        /// Repository di <see cref="Uscita"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed partial class CUsciteClass 
            : CModulesClass<Uscita>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CUsciteClass() 
                : base("modOfficeUscite", typeof(UsciteCursor))
            {
            }

            [NonSerialized] private CommissioniPerUscite _CommissioniPerUscite = null;

            /// <summary>
            /// Repository di <see cref="CommissionePerUscita"/>
            /// </summary>
            public CommissioniPerUscite CommissioniPerUscite
            {
                get
                {
                    if (this._CommissioniPerUscite is null)
                        this._CommissioniPerUscite = new CommissioniPerUscite();
                    return this._CommissioniPerUscite;
                }
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeUscite");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeUscite");
            //        ret.Description = "Uscite";
            //        ret.DisplayName = "Uscite";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!minidom.Office.Database.Tables.ContainsKey("tbl_OfficeUscite"))
            //    {
            //        var table = minidom.Office.Database.Tables.Add("tbl_OfficeUscite");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("IDOperatore", TypeCode.Int32);
            //        field = table.Fields.Add("NomeOperatore", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("OraUscita", TypeCode.DateTime);
            //        field = table.Fields.Add("OraRientro", TypeCode.DateTime);
            //        field = table.Fields.Add("DistanzaPercorsa", TypeCode.Single);
            //        field = table.Fields.Add("IDVeicoloUsato", TypeCode.Int32);
            //        field = table.Fields.Add("LitriCarburante", TypeCode.Single);
            //        field = table.Fields.Add("Rimborso", TypeCode.Decimal);
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
            /// Restituisce l'ultima uscita registrata dall'operatore
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public Uscita GetUltimaUscita(CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                if (DBUtils.GetID(user) == 0)
                    return null;

                using (var cursor = new UsciteCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.PageSize = 1;
                    cursor.IDOperatore.Value = DBUtils.GetID(user, 0);
                    cursor.OraUscita.SortOrder = SortEnum.SORT_DESC;
                    cursor.OraRientro.Value = default;
                    cursor.OraRientro.IncludeNulls = true;
                    // cursor.OraUscita.Value = Now
                    // cursor.OraUscita.Operator = OP.OP_LE
                    return cursor.Item;
                }
                     
            }

            /// <summary>
            /// Restituisce le statistiche sulle uscite registrate per il punto operativo
            /// </summary>
            /// <param name="po"></param>
            /// <param name="di"></param>
            /// <param name="df"></param>
            /// <returns></returns>
            public RUStats GetStats(CUfficio po, DateTime? di, DateTime? df)
            {
                // If (di.HasValue andalso Calendar.Compare(di, df)=0) then df = Calendar.DateAdd (
                using (var cursor = new minidom.Office.UsciteCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (po is object)
                    {
                        // cursor.IDPuntoOperativo.Value = po
                        var lst = new List<int>();
                        foreach (var user in po.Utenti)
                        {
                            if (user.Stato == ObjectStatus.OBJECT_VALID)
                            {
                                lst.Add(DBUtils.GetID(user, 0));
                            }
                        }

                        cursor.WhereClauses *= cursor.Field("IDOperatore").In(lst.ToArray());
                    }

                    if (di.HasValue)
                    {
                        cursor.OraUscita.Value = di;
                        cursor.OraUscita.Operator = OP.OP_GE;
                        if (df.HasValue)
                        {
                            cursor.OraUscita.Value1 = DMD.DateUtils.GetLastSecond(df);
                            cursor.OraUscita.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (df.HasValue)
                    {
                        cursor.OraUscita.Value = DMD.DateUtils.GetLastSecond(df);
                        cursor.OraUscita.Operator = OP.OP_LE;
                    }

                    var ret = new RUStats();
                    
                    while (cursor.Read())
                    {
                        string nomeOperatore = "";
                        if (cursor.Item.Operatore is object)
                            nomeOperatore = cursor.Item.NomeOperatore;

                        var item = ret.items.GetItemByKey(nomeOperatore);
                        if (item is null)
                        {
                            item = new RUStatItem(cursor.Item.Operatore);
                            ret.items.Add(nomeOperatore, item);
                        }

                        item.Uscite.Add(cursor.Item);
                         
                    }

                    return ret;
                }
                 
            }
        }
    }

    public partial class Office
    {
        private static CUsciteClass m_Uscite = null;

        /// <summary>
        /// Repository di <see cref="Uscita"/>
        /// </summary>
        public static CUsciteClass Uscite
        {
            get
            {
                if (m_Uscite is null)
                    m_Uscite = new CUsciteClass();
                return m_Uscite;
            }
        }
    }
}