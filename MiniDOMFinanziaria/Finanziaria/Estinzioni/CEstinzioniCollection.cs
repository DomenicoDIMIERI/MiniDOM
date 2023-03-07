using System;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Rappresenta una collezione di estinzioni
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CEstinzioniCollection : CCollection<CEstinzione>
        {
            public CEstinzioniCollection()
            {
            }

            public CEstinzione Add(TipoEstinzione tipo, CCQSPDCessionarioClass istituto)
            {
                var item = new CEstinzione();
                item.Tipo = tipo;
                item.Istituto = istituto;
                Add(item);
                return item;
            }
        }
    }
}