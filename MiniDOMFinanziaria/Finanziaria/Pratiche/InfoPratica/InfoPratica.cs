using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public sealed class CInfoPraticaClass : CModulesClass<Finanziaria.CInfoPratica>
        {
            public CInfoPraticaClass() : base("modInfoPraticaCQSPD", typeof(Finanziaria.CInfoPraticaCursor), 0)
            {
            }
            // Friend Sub New()
            // DMDObject.IncreaseCounter(Me)
            // End Sub

            // Protected Overrides Sub Finalize()
            // MyBase.Finalize()
            // DMDObject.DecreaseCounter(Me)
            // End Sub

            public override Finanziaria.CInfoPratica GetItemById(int id)
            {
                if (id == 0)
                    return null;
                var cursor = new Finanziaria.CInfoPraticaCursor();
                cursor.IgnoreRights = true;
                cursor.PageSize = 1;
                cursor.ID.Value = id;
                var ret = cursor.Item;
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }

            public Finanziaria.CInfoPratica GetItemByPratica(Finanziaria.CPraticaCQSPD pratica)
            {
                if (pratica is null)
                    throw new ArgumentNullException("pratica");
                if (DBUtils.GetID(pratica) == 0)
                    return null;
                var cursor = new Finanziaria.CInfoPraticaCursor();
                cursor.IDPratica.Value = DBUtils.GetID(pratica);
                cursor.IgnoreRights = true;
                cursor.PageSize = 1;
                var ret = cursor.Item;
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CInfoPraticaClass m_InfoPratica = null;

        public static CInfoPraticaClass InfoPratica
        {
            get
            {
                if (m_InfoPratica is null)
                    m_InfoPratica = new CInfoPraticaClass();
                return m_InfoPratica;
            }
        }
    }
}