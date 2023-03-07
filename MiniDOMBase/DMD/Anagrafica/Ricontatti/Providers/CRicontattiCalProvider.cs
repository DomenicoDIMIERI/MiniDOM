using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Provider di eventi di tipo ricontatto per il calendario
        /// </summary>
        public class CRicontattiCalProvider 
            : Sistema.BaseCalendarActivitiesProvider
        {

            /// <summary>
            /// Istanzia un nuovo elemento
            /// </summary>
            /// <returns></returns>
            public override object InstantiateNewItem()
            {
                return new CRicontatto();
            }

            /// <summary>
            /// Restituisce una descrizione del comando
            /// </summary>
            /// <returns></returns>
            public override string GetCreateCommand()
            {
                return "/?_m=_a=create";
            }

            /// <summary>
            /// Restituisce una descrizione breve del provider
            /// </summary>
            /// <returns></returns>
            public override string GetShortDescription()
            {
                return "Ricontatto";
            }
            
            /// <summary>
            /// Restituisce un array contenete i tipi supportati
            /// </summary>
            /// <returns></returns>
            public override Type[] GetSupportedTypes()
            {
                return new[] { typeof(CRicontatto) };
            }

            /// <summary>
            /// Restituisce le scadenze comprese tra le date specificate
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate)
            {
                // Dim dbSQL, wherePart, tmpSQL As String
                // Dim item As CCalendarActivity
                // Dim items As New CCollection(Of ICalendarActivity)
                // wherePart = "([Stato]=" & ObjectStatus.OBJECT_VALID & ") And (([StatoRicontatto]=1) Or ([StatoRicontatto]=3))"
                // tmpSQL = ""
                // If Not Types.IsNull(fromDate) Then tmpSQL = DMD.Strings.Combine(tmpSQL, "([DataPrevista]>=" & DBUtils.DBDate(fromDate) & ")", " AND ")
                // If Not Types.IsNull(toDate) Then tmpSQL = DMD.Strings.Combine(tmpSQL, "([DataPrevista]<=" & DBUtils.DBDate(toDate) & ")", " AND ")
                // If tmpSQL <> "" Then wherePart = DMD.Strings.Combine(wherePart, "(" & tmpSQL & ")", " AND ")
                // If Not Anagrafica.Ricontatti.Module.UserCanDoAction("list") Then
                // tmpSQL = "(0<>0)"
                // If (Anagrafica.Ricontatti.Module.UserCanDoAction("list_office")) Then tmpSQL = DMD.Strings.Combine(tmpSQL, "([NomePuntoOperativo] In (SELECT DISTINCT [Nome] FROM [tbl_AziendaUffici] INNER JOIN [tbl_UtentiXUfficio] ON [tbl_AziendaUffici].[ID]=[tbl_UtentiXUfficio].[Ufficio] WHERE [Utente]=" & GetID(Users.CurrentUser) & "))", " OR ")
                // If (Anagrafica.Ricontatti.Module.UserCanDoAction("list_own")) Then tmpSQL = DMD.Strings.Combine(tmpSQL, "([CreatoDa]=" & GetID(Users.CurrentUser) & ")", " OR ")
                // wherePart = DMD.Strings.Combine(wherePart, "(" & tmpSQL & ")", " AND ")
                // End If
                // dbSQL = "SELECT * FROM [tbl_Ricontatti] WHERE " & wherePart
                // items.Comparer = Calendar.DefaultComparer
                // items.Sorted = False

                // Dim reader As New DBReader(APPConn.Tables("tbl_Ricontatti"), dbSQL)
                // While reader.Read
                // Dim ric As New CRicontatto
                // item = New CCalendarActivity
                // item.Tag = ric
                // APPConn.Load(ric, reader)
                // With item
                // .GiornataIntera = False
                // .DataInizio = ric.DataPrevista
                // .DataFine = ric.DataPrevista
                // .Descrizione = ric.Note
                // .Luogo = ric.NomePuntoOperativo
                // .Note = ""
                // .StatoAttivita = 1 '.Tag.StatoAppuntamento
                // .Operatore = ric.Operatore
                // .AssegnatoA = ric.AssegnatoA
                // '.Promemoria = .Tag.Promemoria
                // '.Ripetizione = .Tag.Ripetizione
                // End With
                // items.Add(item)
                // End While
                // reader.Dispose()

                // items.Sorted = True
                // Return items
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce le persone attive
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            public override CCollection<Sistema.CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0)
            {
                // Return Ricontatti.GetActivePersons(nomeLista, fromDate, toDate, ufficio, operatore, Nothing)
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce le attività pendenti
            /// </summary>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                return new CCollection<Sistema.ICalendarActivity>();
            }

            /// <summary>
            /// Restituisce la lista delle cose da fare
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                return new CCollection<Sistema.ICalendarActivity>();
            }

            /// <summary>
            /// Restituisce il nome univoco del provider
            /// </summary>
            public override string UniqueName
            {
                get
                {
                    return "RICALLCALPROV";
                }
            }

            /// <summary>
            /// Elimina l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public override void DeleteActivity(Sistema.ICalendarActivity item, bool force = false)
            {
                CRicontatto ric = (CRicontatto)item.Tag;
                ric.Delete(force);
            }

            /// <summary>
            /// Salva l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public override void SaveActivity(Sistema.ICalendarActivity item, bool force = false)
            {
                CRicontatto ric = (CRicontatto)item.Tag;
                ric.Save(force);
            }
        }
    }
}