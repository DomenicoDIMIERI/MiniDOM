using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {

        /// <summary>
    /// Class Generica che consente di accedere al modulo Tabella Finanziaria
    /// </summary>
    /// <remarks></remarks>
        public sealed class CTabelleFinanziarieClass : CModulesClass<Finanziaria.CTabellaFinanziaria>
        {
            private CCollection<Finanziaria.CProdottoXTabellaFin> m_TabelleXProdotto = new CCollection<Finanziaria.CProdottoXTabellaFin>();

            internal CTabelleFinanziarieClass() : base("modCQSPDTabelleFinanziarie", typeof(Finanziaria.CTabelleFinanziarieCursor), -1)
            {
            }

            public CCollection<Finanziaria.CProdottoXTabellaFin> ProdottiRelations
            {
                get
                {
                    lock (cacheLock)
                    {
                        Finanziaria.CProdottoXTabellaFinCursor cursor = null;
                        try
                        {
                            m_TabelleXProdotto = new CCollection<Finanziaria.CProdottoXTabellaFin>();
                            cursor = new Finanziaria.CProdottoXTabellaFinCursor();
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

            public Finanziaria.CProdottoXTabellaFin GetTabellaXProdottoByID(int id)
            {
                if (id == 0)
                    return null;
                var items = ProdottiRelations;
                var ret = items.GetItemById(id);
                if (ret is null)
                {
                    var cursor = new Finanziaria.CProdottoXTabellaFinCursor();
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

            // Public Function GetTabellaXProdottoByID(ByVal value As Integer) As CProdottoXTabellaFin
            // If (value = 0) Then Return Nothing
            // Dim ret As CProdottoXTabellaFin = Me.m_TabelleXProdotto.GetItemById(value)
            // If (ret Is Nothing) Then
            // Dim cursor As New CProdottoXTabellaFinCursor
            // Try
            // cursor.ID.Value = Formats.ToInteger(value)
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
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

            public void InvalidateRelations()
            {
                lock (cacheLock)
                    m_TabelleXProdotto = null;
            }

            internal void UpdateRelations(Finanziaria.CProdottoXTabellaFin rel)
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
        }
    }

    public partial class Finanziaria
    {
        private static CTabelleFinanziarieClass m_TabelleFinanziarie = null;

        public static CTabelleFinanziarieClass TabelleFinanziarie
        {
            get
            {
                if (m_TabelleFinanziarie is null)
                    m_TabelleFinanziarie = new CTabelleFinanziarieClass();
                return m_TabelleFinanziarie;
            }
        }
    }
}