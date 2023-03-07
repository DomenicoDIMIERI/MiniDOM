using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class CTabelleAssicurativeClass : CModulesClass<Finanziaria.CTabellaAssicurativa>
        {
            private CCollection<Finanziaria.CProdottoXTabellaAss> m_TabelleXProdotto = null;

            internal CTabelleAssicurativeClass() : base("modFinTblAss", typeof(Finanziaria.CTabelleAssicurativeCursor), -1)
            {
            }

            public CCollection<Finanziaria.CProdottoXTabellaAss> ProdottiRelations
            {
                get
                {
                    lock (cacheLock)
                    {
                        Finanziaria.CProdottoXTabellaAssCursor cursor = null;
                        try
                        {
                            m_TabelleXProdotto = new CCollection<Finanziaria.CProdottoXTabellaAss>();
                            cursor = new Finanziaria.CProdottoXTabellaAssCursor();
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IgnoreRights = true;
                            while (!cursor.EOF())
                            {
                                m_TabelleXProdotto.Add(cursor.Item);
                                cursor.MoveNext();
                            }
                        }
                        catch (Exception ex)
                        {
                            m_TabelleXProdotto = null;
                            Sistema.Events.NotifyUnhandledException(ex);
                            throw;
                        }
                        finally
                        {
                            if (cursor is object)
                            {
                                cursor.Dispose();
                                cursor = null;
                            }
                        }

                        return m_TabelleXProdotto;
                    }
                }
            }

            public Finanziaria.CProdottoXTabellaAss GetTabellaXProdottoByID(int id)
            {
                if (id == 0)
                    return null;
                var items = ProdottiRelations;
                var ret = items.GetItemById(id);
                if (ret is null)
                {
                    var cursor = new Finanziaria.CProdottoXTabellaAssCursor();
                    try
                    {
                        cursor.ID.Value = id;
                        cursor.IgnoreRights = true;
                        ret = cursor.Item;
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                        throw;
                    }
                    finally
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }

                return ret;
            }

            public void InvalidateRelations()
            {
                lock (cacheLock)
                    m_TabelleXProdotto = null;
            }

            internal void UpdateRelations(Finanziaria.CProdottoXTabellaAss rel)
            {
                lock (cacheLock)
                {
                    if (rel is null)
                        throw new ArgumentNullException("rel");
                    if (m_TabelleXProdotto is null)
                        return;
                    var oldRel = m_TabelleXProdotto.GetItemById(DBUtils.GetID(rel));
                    if (ReferenceEquals(oldRel, rel))
                        return;
                    if (oldRel is object)
                        m_TabelleXProdotto.Remove(oldRel);
                    if (rel is object)
                        m_TabelleXProdotto.Add(rel);
                }
            }


            // Public Function GetTabellaXProdottoByID(ByVal value As Integer) As CProdottoXTabellaAss
            // If (value = 0) Then Return Nothing
            // Dim ret As CProdottoXTabellaAss = Me.m_TabelleXProdotto.GetItemById(value)
            // If (ret Is Nothing) Then
            // Dim cursor As New CProdottoXTabellaAssCursor
            // Try
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
            // cursor.ID.Value = value
            // ret = cursor.Item
            // If (ret IsNot Nothing) Then Me.m_TabelleXProdotto.Add(ret)
            // Catch ex As Exception
            // Sistema.Events.NotifyUnhandledException(ex)
            // Throw
            // Finally
            // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            // End Try
            // End If
            // Return ret
            // End Function



        }
    }

    public partial class Finanziaria
    {
        private static CTabelleAssicurativeClass m_TabelleAssicurative = null;

        public static CTabelleAssicurativeClass TabelleAssicurative
        {
            get
            {
                if (m_TabelleAssicurative is null)
                    m_TabelleAssicurative = new CTabelleAssicurativeClass();
                return m_TabelleAssicurative;
            }
        }
    }
}