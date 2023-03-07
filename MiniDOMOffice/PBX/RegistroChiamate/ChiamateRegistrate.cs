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
        /// Repository di <see cref="ChiamataRegistrata"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class ChiamateRegistrate
            : CModulesClass<ChiamataRegistrata>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public ChiamateRegistrate() 
                : base("modOfficeRegistroChiamate", typeof(minidom.Office.ChiamataRegistrataCursor), 0)
            {
            }
        }
    }

    public partial class Office
    {
        private static ChiamateRegistrate m_ChiamateRegistrate = null;

        /// <summary>
        /// Repository di <see cref="ChiamataRegistrata"/>
        /// </summary>
        /// <remarks></remarks>
        public static ChiamateRegistrate ChiamateRegistrate
        {
            get
            {
                if (m_ChiamateRegistrate is null)
                    m_ChiamateRegistrate = new ChiamateRegistrate();
                return m_ChiamateRegistrate;
            }
        }
    }
}