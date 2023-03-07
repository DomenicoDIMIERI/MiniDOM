using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.internals;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CCampagnaPubblicitaria"/>
        /// </summary>
        [Serializable]
        public sealed class CCampagneClass 
            : CModulesClass<ADV.CCampagnaPubblicitaria>
        {
            private bool m_Checking = false;
            public readonly object exeLock = new object();
            private bool m_Initialized = false;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagneClass() 
                : base("modADV", typeof(ADV.CCampagnaPubblicitariaCursor))
            {
            }

            public override void Initialize()
            {
                if (m_Initialized)
                    return;
                base.Initialize();
                // Me.m_Checking = False
                // Me.m_Timer = New System.Timers.Timer(6000) 'Le campagne vengono controllate ogni minuto
                // Me.m_Timer.Enabled = True
                // AddHandler m_Timer.Elapsed, AddressOf TimerHandler
                minidom.Sistema.RegisteredTypeProviders.Add("CCampagnaPubblicitaria", ADV.Campagne.GetItemById);
                m_Initialized = true;
            }

            public override void Terminate()
            {
                if (!m_Initialized)
                    return;

                // RemoveHandler m_Timer.Elapsed, AddressOf TimerHandler
                // Me.m_Timer.Enabled = False
                // Me.m_Timer = Nothing
                base.Terminate();
                m_Initialized = false;
            }

            // Public Property EnableTimer As Boolean
            // Get
            // Return m_Timer.Enabled
            // End Get
            // Set(value As Boolean)
            // m_Timer.Enabled = value
            // If (value) Then
            // m_Timer.Start()
            // Else
            // m_Timer.Stop()
            // End If
            // End Set
            // End Property

            public ADV.HandlerTipoCampagna GetHandler(ADV.TipoCampagnaPubblicitaria tipoCampagna)
            {
                switch (tipoCampagna)
                {
                    case ADV.TipoCampagnaPubblicitaria.eMail:
                        {
                            return ADV.HandlerTipoCampagnaEMail.Instance;
                        }

                    case ADV.TipoCampagnaPubblicitaria.NonImpostato:
                        {
                            return new ADV.NullCampagnaADVHandler();
                        }

                    case ADV.TipoCampagnaPubblicitaria.Quotidiani:
                        {
                            return new ADV.NullCampagnaADVHandler();
                        }

                    case ADV.TipoCampagnaPubblicitaria.Web:
                        {
                            return new ADV.NullCampagnaADVHandler();
                        }

                    case ADV.TipoCampagnaPubblicitaria.Fax:
                        {
                            return ADV.HandlerTipoCampagnaFax.Instance;
                        }

                    case ADV.TipoCampagnaPubblicitaria.SMS:
                        {
                            return ADV.HandlerTipoCampagnaSMS.Instance;
                        }

                    default:
                        {
                            return null;
                        }
                }
            }

            /// <summary>
            /// Effettuia il controllo e l'invio sincrono delle campagne
            /// </summary>
            /// <remarks></remarks>
            public void Check()
            {
                lock (exeLock)
                {
                    if (m_Checking)
                        return;
                    m_Checking = true;
                    var items = new CCollection<ADV.CCampagnaPubblicitaria>();    // Campagne che sono in conda per essere attivate
                    ADV.CCampagnaPubblicitaria c;
                    Sistema.CalendarSchedule s;

                    using (var cursor = new ADV.CCampagnaPubblicitariaCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.Attiva.Value = true;
                        cursor.StatoCampagna.Value = ADV.StatoCampagnaPubblicitaria.Programmata;
                        cursor.StatoCampagna.Operator = OP.OP_NE;
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            c = cursor.Item;
                            s = c.Programmazione.GetNextSchedule();
                            if (s is object)
                            {
                                var d = s.CalcolaProssimaEsecuzione();
                                if (d.HasValue && d.Value <= DMD.DateUtils.Now())
                                {
                                    minidom.Sistema.ApplicationContext.Log("Accodo la campagna: " + c.ToString());
                                    // c.StatoCampagna = StatoCampagnaPubblicitaria.Programmata
                                    // c.Save()
                                    items.Add(c);
                                }
                            }


                        }
                    }

                    foreach (var currentC in items)
                    {
                        c = currentC;
                        s = c.Programmazione.GetNextSchedule();
                        s.UltimaEsecuzione = DMD.DateUtils.Now();
                        s.ConteggioEsecuzioni += 1;
                        s.Save();
                        c.StatoCampagna = ADV.StatoCampagnaPubblicitaria.Programmata;
                        c.Save();
                        var lista = c.GetListaDiInvio();
                        c.Invia(lista);
                        c.StatoCampagna = ADV.StatoCampagnaPubblicitaria.Eseguita;
                        c.Save();
                    }
                    m_Checking = false;
                }
            }


            // Private Sub DequeueADVs()
            // Dim cursor As New CCampagnaPubblicitariaCursor
            // Dim c As CCampagnaPubblicitaria = Nothing
            // #If Not Debug Then
            // Try
            // #End If
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // cursor.Attiva.Value = True
            // cursor.StatoCampagna.Value = StatoCampagnaPubblicitaria.Programmata
            // cursor.IgnoreRights = True
            // c = cursor.Item
            // #If Not Debug Then
            // Catch ex As Exception
            // Throw
            // Finally
            // #End If
            // cursor.Dispose()
            // #If Not Debug Then
            // End Try
            // #End If

            // If (c IsNot Nothing) Then
            // Dim s As CalendarSchedule = c.Programmazione.GetNextSchedule
            // If (s IsNot Nothing) Then
            // s.UltimaEsecuzione = Now()
            // s.ConteggioEsecuzioni += 1
            // s.Save()
            // c.StatoCampagna = StatoCampagnaPubblicitaria.Eseguita
            // c.Save()
            // Dim lista As CCollection(Of CRisultatoCampagna) = c.GetListaDiInvio
            // Debug.Print("Eseguo la campagna: " & c.ToString)
            // c.Invia(lista)
            // End If
            // End If

            // End Sub

            // Private Sub TimerHandler(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
            // If m_Checking Then Return
            // Me.m_Checking = True

            // Me.Check()
            // Me.m_Checking = False
            // End Sub



            public ADV.CCampagnaPubblicitaria GetItemByName(string nomeCampagna)
            {
                nomeCampagna = DMD.Strings.Trim(nomeCampagna);
                if (string.IsNullOrEmpty(nomeCampagna))
                    return null;
                using (var cursor = new ADV.CCampagnaPubblicitariaCursor())
                {  
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.NomeCampagna.Value = nomeCampagna;
                    return cursor.Item;
                }
                
            }
        }
    }

    public partial class ADV
    {
        private static CCampagneClass m_Campagne = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CCampagnaPubblicitaria"/>
        /// </summary>
        public static CCampagneClass Campagne
        {
            get
            {
                if (m_Campagne is null)
                    m_Campagne = new CCampagneClass();
                return m_Campagne;
            }
        }
    }
}