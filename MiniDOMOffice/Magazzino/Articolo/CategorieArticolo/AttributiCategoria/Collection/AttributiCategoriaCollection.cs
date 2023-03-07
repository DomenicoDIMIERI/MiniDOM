using System;

namespace minidom
{
    public partial class Store
    {

        /// <summary>
    /// Collezione degli attributi specificato per una istanza di un Categoria
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class AttributiCategoriaCollection : CKeyCollection<AttributoCategoria>
        {
            [NonSerialized]
            private CategoriaArticolo m_Categoria;

            public AttributiCategoriaCollection()
            {
                m_Categoria = null;
            }

            public AttributiCategoriaCollection(CategoriaArticolo Categoria) : this()
            {
                Load(Categoria);
            }

            public CategoriaArticolo Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            protected internal virtual void SetCategoria(CategoriaArticolo value)
            {
                m_Categoria = value;
                if (value is object)
                {
                    foreach (AttributoCategoria item in this)
                        item.SetCategoria(value);
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Categoria is object)
                    ((AttributoCategoria)value).SetCategoria(m_Categoria);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Categoria is object)
                    ((AttributoCategoria)newValue).SetCategoria(m_Categoria);
                base.OnSet(index, oldValue, newValue);
            }

            public void Load(CategoriaArticolo Categoria)
            {
                if (Categoria is null)
                    throw new ArgumentNullException("Categoria");
                Clear();
                m_Categoria = Categoria;
                if (DBUtils.GetID(Categoria) == 0)
                    return;
                var cursor = new AttributoCategoriaCursor();
                cursor.IgnoreRights = true;
                cursor.IDCategoria.Value = DBUtils.GetID(Categoria);
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                while (!cursor.EOF())
                {
                    Add(cursor.Item.NomeAttributo, cursor.Item);
                    cursor.MoveNext();
                }

                cursor.Dispose();
                Sort();
            }
        }
    }
}