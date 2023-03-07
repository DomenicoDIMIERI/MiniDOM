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
using static minidom.CustomerCalls;


namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="CRMStatisticheOperatore"/>
        /// </summary>
        [Serializable]
        public class CRMStatisticheOperatoreCursor 
            : minidom.Databases.DBObjectCursorBase<CRMStatisticheOperatore>
        {
            private DBCursorField<int> m_IDPuntoOperativo = new DBCursorField<int>("IDPuntoOperativo");
            private DBCursorStringField m_NomePuntoOperativo = new DBCursorStringField("NomePuntoOperativo");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<bool> m_Ricalcola = new DBCursorField<bool>("Ricalcola");

            //private DBCursorField<int> m_InCallNum = new DBCursorField<int>("InCallNum");
            //private DBCursorField<double> m_InCallMinLen = new DBCursorField<double>("InCallMinLen");
            //private DBCursorField<double> m_InCallMaxLen = new DBCursorField<double>("InCallMaxLen");
            //private DBCursorField<double> m_InCallTotLen = new DBCursorField<double>("InCallTotLen");
            //private DBCursorField<double> m_InCallMinWait = new DBCursorField<double>("InCallMinWait");
            //private DBCursorField<double> m_InCallMaxWait = new DBCursorField<double>("InCallMaxWait");
            //private DBCursorField<double> m_InCallTotWait = new DBCursorField<double>("InCallTotWait");
            //private DBCursorField<decimal> m_InCallCost = new DBCursorField<decimal>("InCallCost");
            //private DBCursorField<int> m_OutCallNum = new DBCursorField<int>("OutCallNum");
            //private DBCursorField<double> m_OutCallMinLen = new DBCursorField<double>("OutCallMinLen");
            //private DBCursorField<double> m_OutCallMaxLen = new DBCursorField<double>("OutCallMaxLen");
            //private DBCursorField<double> m_OutCallTotLen = new DBCursorField<double>("OutCallTotLen");
            //private DBCursorField<double> m_OutCallMinWait = new DBCursorField<double>("OutCallMinWait");
            //private DBCursorField<double> m_OutCallMaxWait = new DBCursorField<double>("OutCallMaxWait");
            //private DBCursorField<double> m_OutCallTotWait = new DBCursorField<double>("OutCallTotWait");
            //private DBCursorField<decimal> m_OutCallCost = new DBCursorField<decimal>("OutCallCost");
            //private DBCursorField<int> m_InDateNum = new DBCursorField<int>("InDateNum");
            //private DBCursorField<double> m_InDateMinLen = new DBCursorField<double>("InDateMinLen");
            //private DBCursorField<double> m_InDateMaxLen = new DBCursorField<double>("InDateMaxLen");
            //private DBCursorField<double> m_InDateTotLen = new DBCursorField<double>("InDateTotLen");
            //private DBCursorField<double> m_InDateMinWait = new DBCursorField<double>("InDateMinWait");
            //private DBCursorField<double> m_InDateMaxWait = new DBCursorField<double>("InDateMaxWait");
            //private DBCursorField<double> m_InDateTotWait = new DBCursorField<double>("InDateTotWait");
            //private DBCursorField<decimal> m_InDateCost = new DBCursorField<decimal>("InDateCost");
            //private DBCursorField<int> m_OutDateNum = new DBCursorField<int>("OutDateNum");
            //private DBCursorField<double> m_OutDateMinLen = new DBCursorField<double>("OutDateMinLen");
            //private DBCursorField<double> m_OutDateMaxLen = new DBCursorField<double>("OutDateMaxLen");
            //private DBCursorField<double> m_OutDateTotLen = new DBCursorField<double>("OutDateTotLen");
            //private DBCursorField<double> m_OutDateMinWait = new DBCursorField<double>("OutDateMinWait");
            //private DBCursorField<double> m_OutDateMaxWait = new DBCursorField<double>("OutDateMaxWait");
            //private DBCursorField<double> m_OutDateTotWait = new DBCursorField<double>("OutDateTotWait");
            //private DBCursorField<decimal> m_OutDateCost = new DBCursorField<decimal>("OutDateCost");
            //private DBCursorField<int> m_InSMSNum = new DBCursorField<int>("InSMSNum");
            //private DBCursorField<double> m_InSMSMinLen = new DBCursorField<double>("InSMSMinLen");
            //private DBCursorField<double> m_InSMSMaxLen = new DBCursorField<double>("InSMSMaxLen");
            //private DBCursorField<double> m_InSMSTotLen = new DBCursorField<double>("InSMSTotLen");
            //private DBCursorField<decimal> m_InSMSCost = new DBCursorField<decimal>("InSMSCost");
            //private DBCursorField<int> m_OutSMSNum = new DBCursorField<int>("OutSMSNum");
            //private DBCursorField<double> m_OutSMSMinLen = new DBCursorField<double>("OutSMSMinLen");
            //private DBCursorField<double> m_OutSMSMaxLen = new DBCursorField<double>("OutSMSMaxLen");
            //private DBCursorField<double> m_OutSMSTotLen = new DBCursorField<double>("OutSMSTotLen");
            //private DBCursorField<decimal> m_OutSMSCost = new DBCursorField<decimal>("OutSMSCost");
            //private DBCursorField<int> m_InFAXNum = new DBCursorField<int>("InFAXNum");
            //private DBCursorField<double> m_InFAXMinLen = new DBCursorField<double>("InFAXMinLen");
            //private DBCursorField<double> m_InFAXMaxLen = new DBCursorField<double>("InFAXMaxLen");
            //private DBCursorField<double> m_InFAXTotLen = new DBCursorField<double>("InFAXTotLen");
            //private DBCursorField<decimal> m_InFAXCost = new DBCursorField<decimal>("InFAXCost");
            //private DBCursorField<int> m_OutFAXNum = new DBCursorField<int>("OutFAXNum");
            //private DBCursorField<double> m_OutFAXMinLen = new DBCursorField<double>("OutFAXMinLen");
            //private DBCursorField<double> m_OutFAXMaxLen = new DBCursorField<double>("OutFAXMaxLen");
            //private DBCursorField<double> m_OutFAXTotLen = new DBCursorField<double>("OutFAXTotLen");
            //private DBCursorField<decimal> m_OutFAXCost = new DBCursorField<decimal>("OutFAXCost");
            //private DBCursorField<int> m_InEMAILNum = new DBCursorField<int>("InEMAILNum");
            //private DBCursorField<double> m_InEMAILMinLen = new DBCursorField<double>("InEMAILMinLen");
            //private DBCursorField<double> m_InEMAILMaxLen = new DBCursorField<double>("InEMAILMaxLen");
            //private DBCursorField<double> m_InEMAILTotLen = new DBCursorField<double>("InEMAILTotLen");
            //private DBCursorField<decimal> m_InEMAILCost = new DBCursorField<decimal>("InEMAILCost");
            //private DBCursorField<int> m_OutEMAILNum = new DBCursorField<int>("OutEMAILNum");
            //private DBCursorField<double> m_OutEMAILMinLen = new DBCursorField<double>("OutEMAILMinLen");
            //private DBCursorField<double> m_OutEMAILMaxLen = new DBCursorField<double>("OutEMAILMaxLen");
            //private DBCursorField<double> m_OutEMAILTotLen = new DBCursorField<double>("OutEMAILTotLen");
            //private DBCursorField<decimal> m_OutEMAILCost = new DBCursorField<decimal>("OutEMAILCost");
            //private DBCursorField<int> m_InTelegramNum = new DBCursorField<int>("InTelegramNum");
            //private DBCursorField<double> m_InTelegramMinLen = new DBCursorField<double>("InTelegramMinLen");
            //private DBCursorField<double> m_InTelegramMaxLen = new DBCursorField<double>("InTelegramMaxLen");
            //private DBCursorField<double> m_InTelegramTotLen = new DBCursorField<double>("InTelegramTotLen");
            //private DBCursorField<decimal> m_InTelegramCost = new DBCursorField<decimal>("InTelegramCost");
            //private DBCursorField<int> m_OutTelegramNum = new DBCursorField<int>("OutTelegramNum");
            //private DBCursorField<double> m_OutTelegramMinLen = new DBCursorField<double>("OutTelegramMinLen");
            //private DBCursorField<double> m_OutTelegramMaxLen = new DBCursorField<double>("OutTelegramMaxLen");
            //private DBCursorField<double> m_OutTelegramTotLen = new DBCursorField<double>("OutTelegramTotLen");
            //private DBCursorField<decimal> m_OutTelegramCost = new DBCursorField<decimal>("OutTelegramCost");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatisticheOperatoreCursor()
            {
            }

            /// <summary>
            /// IDPuntoOperativo
            /// </summary>
            public DBCursorField<int> IDPuntoOperativo
            {
                get
                {
                    return m_IDPuntoOperativo;
                }
            }

            /// <summary>
            /// NomePuntoOperativo
            /// </summary>
            public DBCursorStringField NomePuntoOperativo
            {
                get
                {
                    return m_NomePuntoOperativo;
                }
            }

            /// <summary>
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            ///// <summary>
            ///// InCallNum
            ///// </summary>
            //public DBCursorField<int> InCallNum
            //{
            //    get
            //    {
            //        return m_InCallNum;
            //    }
            //}

            //public DBCursorField<double> InCallMinLen
            //{
            //    get
            //    {
            //        return m_InCallMinLen;
            //    }
            //}

            //public DBCursorField<double> InCallMaxLen
            //{
            //    get
            //    {
            //        return m_InCallMaxLen;
            //    }
            //}

            //public DBCursorField<double> InCallTotLen
            //{
            //    get
            //    {
            //        return m_InCallTotLen;
            //    }
            //}

            //public DBCursorField<double> InCallMinWait
            //{
            //    get
            //    {
            //        return m_InCallMinWait;
            //    }
            //}

            //public DBCursorField<double> InCallMaxWait
            //{
            //    get
            //    {
            //        return m_InCallMaxWait;
            //    }
            //}

            //public DBCursorField<double> InCallTotWait
            //{
            //    get
            //    {
            //        return m_InCallTotWait;
            //    }
            //}

            //public DBCursorField<decimal> InCallCost
            //{
            //    get
            //    {
            //        return m_InCallCost;
            //    }
            //}

            //public DBCursorField<int> OutCallNum
            //{
            //    get
            //    {
            //        return m_OutCallNum;
            //    }
            //}

            //public DBCursorField<double> OutCallMinLen
            //{
            //    get
            //    {
            //        return m_OutCallMinLen;
            //    }
            //}

            //public DBCursorField<double> OutCallMaxLen
            //{
            //    get
            //    {
            //        return m_OutCallMaxLen;
            //    }
            //}

            //public DBCursorField<double> OutCallTotLen
            //{
            //    get
            //    {
            //        return m_OutCallTotLen;
            //    }
            //}

            //public DBCursorField<double> OutCallMinWait
            //{
            //    get
            //    {
            //        return m_OutCallMinWait;
            //    }
            //}

            //public DBCursorField<double> OutCallMaxWait
            //{
            //    get
            //    {
            //        return m_OutCallMaxWait;
            //    }
            //}

            //public DBCursorField<double> OutCallTotWait
            //{
            //    get
            //    {
            //        return m_OutCallTotWait;
            //    }
            //}

            //public DBCursorField<decimal> OutCallCost
            //{
            //    get
            //    {
            //        return m_OutCallCost;
            //    }
            //}

            //public DBCursorField<int> InDateNum
            //{
            //    get
            //    {
            //        return m_InDateNum;
            //    }
            //}

            //public DBCursorField<double> InDateMinLen
            //{
            //    get
            //    {
            //        return m_InDateMinLen;
            //    }
            //}

            //public DBCursorField<double> InDateMaxLen
            //{
            //    get
            //    {
            //        return m_InDateMaxLen;
            //    }
            //}

            //public DBCursorField<double> InDateTotLen
            //{
            //    get
            //    {
            //        return m_InDateTotLen;
            //    }
            //}

            //public DBCursorField<double> InDateMinWait
            //{
            //    get
            //    {
            //        return m_InDateMinWait;
            //    }
            //}

            //public DBCursorField<double> InDateMaxWait
            //{
            //    get
            //    {
            //        return m_InDateMaxWait;
            //    }
            //}

            //public DBCursorField<double> InDateTotWait
            //{
            //    get
            //    {
            //        return m_InDateTotWait;
            //    }
            //}

            //public DBCursorField<decimal> InDateCost
            //{
            //    get
            //    {
            //        return m_InDateCost;
            //    }
            //}

            //public DBCursorField<int> OutDateNum
            //{
            //    get
            //    {
            //        return m_OutDateNum;
            //    }
            //}

            //public DBCursorField<double> OutDateMinLen
            //{
            //    get
            //    {
            //        return m_OutDateMinLen;
            //    }
            //}

            //public DBCursorField<double> OutDateMaxLen
            //{
            //    get
            //    {
            //        return m_OutDateMaxLen;
            //    }
            //}

            //public DBCursorField<double> OutDateTotLen
            //{
            //    get
            //    {
            //        return m_OutDateTotLen;
            //    }
            //}

            //public DBCursorField<double> OutDateMinWait
            //{
            //    get
            //    {
            //        return m_OutDateMinWait;
            //    }
            //}

            //public DBCursorField<double> OutDateMaxWait
            //{
            //    get
            //    {
            //        return m_OutDateMaxWait;
            //    }
            //}

            //public DBCursorField<double> OutDateTotWait
            //{
            //    get
            //    {
            //        return m_OutDateTotWait;
            //    }
            //}

            //public DBCursorField<decimal> OutDateCost
            //{
            //    get
            //    {
            //        return m_OutDateCost;
            //    }
            //}

            //public DBCursorField<int> InSMSNum
            //{
            //    get
            //    {
            //        return m_InSMSNum;
            //    }
            //}

            //public DBCursorField<double> InSMSMinLen
            //{
            //    get
            //    {
            //        return m_InSMSMinLen;
            //    }
            //}

            //public DBCursorField<double> InSMSMaxLen
            //{
            //    get
            //    {
            //        return m_InSMSMaxLen;
            //    }
            //}

            //public DBCursorField<double> InSMSTotLen
            //{
            //    get
            //    {
            //        return m_InSMSTotLen;
            //    }
            //}

            //public DBCursorField<decimal> InSMSCost
            //{
            //    get
            //    {
            //        return m_InSMSCost;
            //    }
            //}

            //public DBCursorField<int> OutSMSNum
            //{
            //    get
            //    {
            //        return m_OutSMSNum;
            //    }
            //}

            //public DBCursorField<double> OutSMSMinLen
            //{
            //    get
            //    {
            //        return m_OutSMSMinLen;
            //    }
            //}

            //public DBCursorField<double> OutSMSMaxLen
            //{
            //    get
            //    {
            //        return m_OutSMSMaxLen;
            //    }
            //}

            //public DBCursorField<double> OutSMSTotLen
            //{
            //    get
            //    {
            //        return m_OutSMSTotLen;
            //    }
            //}

            //public DBCursorField<decimal> OutSMSCost
            //{
            //    get
            //    {
            //        return m_OutSMSCost;
            //    }
            //}

            //public DBCursorField<int> InFAXNum
            //{
            //    get
            //    {
            //        return m_InFAXNum;
            //    }
            //}

            //public DBCursorField<double> InFAXMinLen
            //{
            //    get
            //    {
            //        return m_InFAXMinLen;
            //    }
            //}

            //public DBCursorField<double> InFAXMaxLen
            //{
            //    get
            //    {
            //        return m_InFAXMaxLen;
            //    }
            //}

            //public DBCursorField<double> InFAXTotLen
            //{
            //    get
            //    {
            //        return m_InFAXTotLen;
            //    }
            //}

            //public DBCursorField<decimal> InFAXCost
            //{
            //    get
            //    {
            //        return m_InFAXCost;
            //    }
            //}

            //public DBCursorField<int> OutFAXNum
            //{
            //    get
            //    {
            //        return m_OutFAXNum;
            //    }
            //}

            //public DBCursorField<double> OutFAXMinLen
            //{
            //    get
            //    {
            //        return m_OutFAXMinLen;
            //    }
            //}

            //public DBCursorField<double> OutFAXMaxLen
            //{
            //    get
            //    {
            //        return m_OutFAXMaxLen;
            //    }
            //}

            //public DBCursorField<double> OutFAXTotLen
            //{
            //    get
            //    {
            //        return m_OutFAXTotLen;
            //    }
            //}

            //public DBCursorField<decimal> OutFAXCost
            //{
            //    get
            //    {
            //        return m_OutFAXCost;
            //    }
            //}

            //public DBCursorField<int> InEMAILNum
            //{
            //    get
            //    {
            //        return m_InEMAILNum;
            //    }
            //}

            //public DBCursorField<double> InEMAILMinLen
            //{
            //    get
            //    {
            //        return m_InEMAILMinLen;
            //    }
            //}

            //public DBCursorField<double> InEMAILMaxLen
            //{
            //    get
            //    {
            //        return m_InEMAILMaxLen;
            //    }
            //}

            //public DBCursorField<double> InEMAILTotLen
            //{
            //    get
            //    {
            //        return m_InEMAILTotLen;
            //    }
            //}

            //public DBCursorField<decimal> InEMAILCost
            //{
            //    get
            //    {
            //        return m_InEMAILCost;
            //    }
            //}

            //public DBCursorField<int> OutEMAILNum
            //{
            //    get
            //    {
            //        return m_OutEMAILNum;
            //    }
            //}

            //public DBCursorField<double> OutEMAILMinLen
            //{
            //    get
            //    {
            //        return m_OutEMAILMinLen;
            //    }
            //}

            //public DBCursorField<double> OutEMAILMaxLen
            //{
            //    get
            //    {
            //        return m_OutEMAILMaxLen;
            //    }
            //}

            //public DBCursorField<double> OutEMAILTotLen
            //{
            //    get
            //    {
            //        return m_OutEMAILTotLen;
            //    }
            //}

            //public DBCursorField<decimal> OutEMAILCost
            //{
            //    get
            //    {
            //        return m_OutEMAILCost;
            //    }
            //}

            //public DBCursorField<int> InTelegramNum
            //{
            //    get
            //    {
            //        return m_InTelegramNum;
            //    }
            //}

            //public DBCursorField<double> InTelegramMinLen
            //{
            //    get
            //    {
            //        return m_InTelegramMinLen;
            //    }
            //}

            //public DBCursorField<double> InTelegramMaxLen
            //{
            //    get
            //    {
            //        return m_InTelegramMaxLen;
            //    }
            //}

            //public DBCursorField<double> InTelegramTotLen
            //{
            //    get
            //    {
            //        return m_InTelegramTotLen;
            //    }
            //}

            //public DBCursorField<decimal> InTelegramCost
            //{
            //    get
            //    {
            //        return m_InTelegramCost;
            //    }
            //}

            //public DBCursorField<int> OutTelegramNum
            //{
            //    get
            //    {
            //        return m_OutTelegramNum;
            //    }
            //}

            //public DBCursorField<double> OutTelegramMinLen
            //{
            //    get
            //    {
            //        return m_OutTelegramMinLen;
            //    }
            //}

            //public DBCursorField<double> OutTelegramMaxLen
            //{
            //    get
            //    {
            //        return m_OutTelegramMaxLen;
            //    }
            //}

            //public DBCursorField<double> OutTelegramTotLen
            //{
            //    get
            //    {
            //        return m_OutTelegramTotLen;
            //    }
            //}

            //public DBCursorField<decimal> OutTelegramCost
            //{
            //    get
            //    {
            //        return m_OutTelegramCost;
            //    }
            //}

            /// <summary>
            /// Ricalcola
            /// </summary>
            public DBCursorField<bool> Ricalcola
            {
                get
                {
                    return m_Ricalcola;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.Statistiche.StatisticheOperatore;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CRMStats";
            //}
        }
    }
}