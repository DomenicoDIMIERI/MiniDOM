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
        /// Repository di <see cref="MailAccount"/>
        /// </summary>
        /// <remarks></remarks>
        public class CMailAccounts
            : CModulesClass<MailAccount>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CMailAccounts() 
                : base("modOfficeEMailsAcc", typeof(MailAccountCursor), 0)
            {
            }




            // Protected Friend Sub Load()
            // Dim cursor As MailAccountCursor = Nothing
            // Try
            // Me.Clear()

            // cursor = New MailAccountCursor()
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // cursor.DisplayName.SortOrder = SortEnum.SORT_ASC
            // cursor.IgnoreRights = True
            // While Not cursor.EOF
            // Me.Add(cursor.Item)
            // cursor.MoveNext()
            // End While

            // Catch ex As Exception
            // Sistema.Events.NotifyUnhandledException(ex)
            // Throw
            // Finally
            // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            // End Try
            // End Sub



        }
    }
}