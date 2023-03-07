using System;
using System.Data;
using DMD.XML;

namespace minidom.Forms
{
    public partial class Utils
    {
        public sealed class CComunicazioniUtilsClass
        {
            internal CComunicazioniUtilsClass()
            {
                DMDObject.IncreaseCounter(this);
            }

            // Public Function CreateElencoIstituti(ByVal selValue As String) As String
            // Throw New NotImplementedException
            // End Function

            // Public Function CreateElencoSottocategorie(ByVal selValue As String) As String
            // Throw New NotImplementedException
            // End Function


            public string CreateElencoCategorie(string selectedItem)
            {
                IDataReader dbRis = null;
                try
                {
                    dbRis = Databases.APPConn.ExecuteReader("SELECT [Categoria] FROM [tbl_Comunicazioni] GROUP BY [Categoria] ORDER BY [Categoria] ASC");
                    var writer = new System.Text.StringBuilder();
                    while (dbRis.Read())
                    {
                        string categoria = Sistema.Formats.ToString(dbRis["Categoria"]);
                        if (!string.IsNullOrEmpty(categoria))
                        {
                            writer.Append("<option");
                            if (DMD.Strings.EQ(selectedItem, categoria, true))
                                writer.Append(" selected");
                            writer.Append(">");
                            writer.Append(categoria);
                            writer.Append("</option>");
                        }
                    }

                    return writer.ToString();
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    if (dbRis is object)
                    {
                        dbRis.Dispose();
                        dbRis = null;
                    }
                }
            }

            ~CComunicazioniUtilsClass()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        private static CComunicazioniUtilsClass m_ComunicazioniUtils = null;

        public static CComunicazioniUtilsClass ComunicazioniUtils
        {
            get
            {
                if (m_ComunicazioniUtils is null)
                    m_ComunicazioniUtils = new CComunicazioniUtilsClass();
                return m_ComunicazioniUtils;
            }
        }
    }
}