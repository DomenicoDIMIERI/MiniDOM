Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Internals
Imports System.Threading
Imports System.Timers

Namespace Internals



    Public Class CProcedureClass
        Inherits CModulesClass(Of CProcedura)

#Region "Wrker"

        Private Class Worker
            Public lock As New ManualResetEvent(False)
            Public thread As System.Threading.Thread = Nothing
            Public priority As PriorityEnum
            Public queue As CCollection(Of CProcedura)
            Public startDate As Date
            Public endDate As Date?
            Public c As CProcedura
            Public ForceQuit As Boolean = False

            Public Sub New(ByVal priority As PriorityEnum)
                Me.priority = priority
                Me.queue = New CCollection(Of CProcedura)
                Me.thread = New System.Threading.Thread(AddressOf Me.Work)
                Select Case priority
                    Case PriorityEnum.PRIORITY_HIGHER
                        Me.thread.Priority = ThreadPriority.Highest
                    Case PriorityEnum.PRIORITY_HIGH
                        Me.thread.Priority = ThreadPriority.AboveNormal
                    Case PriorityEnum.PRIORITY_NORMAL
                        Me.thread.Priority = ThreadPriority.Normal
                    Case PriorityEnum.PRIOTITY_LOW
                        Me.thread.Priority = ThreadPriority.BelowNormal
                    Case PriorityEnum.PRIORITY_LOWER
                        Me.thread.Priority = ThreadPriority.Lowest
                End Select
            End Sub

            Public Sub Enqueue(ByVal c As CProcedura)
                SyncLock Me
                    Me.queue.Add(c)
                End SyncLock
            End Sub

            Public Function Dequeue() As CProcedura
                Dim c As CProcedura = Nothing
                SyncLock Me
                    If (Me.queue.Count > 0) Then
                        c = Me.queue(0)
                        Me.queue.RemoveAt(0)
                    End If
                End SyncLock
                Return c
            End Function

            Private Sub Work()
                Do
                    Try
                        Me.lock.WaitOne(5000)
                    Catch ex As Exception
                        Debug.Print(ex.Message)
                    End Try
                    Do
                        Me.c = Me.Dequeue
                        If (c IsNot Nothing) Then
                            Me.startDate = Now
                            Me.endDate = Nothing

                            SyncLock workerLock
                                m_RunningThreads += 1
                            End SyncLock

                            Try
                                Dim d1 As Date = DateUtils.Now
                                Dim erroreP As Exception = Nothing
                                Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: " & c.Nome & " -> Inizio Esecuzione")
                                Try
                                    c.Run()
                                Catch ex As Exception
                                    erroreP = ex
                                End Try

                                Dim d2 As Date = DateUtils.Now
                                Dim s As CalendarSchedule = c.Programmazione.GetNextSchedule
                                s.UltimaEsecuzione = Me.startDate
                                s.ConteggioEsecuzioni += 1
                                s.Save()
                                c.Save()

                                If (erroreP Is Nothing) Then
                                    Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: " & c.Nome & " -> Fine Esecuzione (" & Formats.FormatDurata((d2 - d1).TotalSeconds) & ")")
                                Else
                                    Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: " & c.Nome & " -> Errore (" & Formats.FormatDurata((d2 - d1).TotalSeconds) & ")")
                                    Sistema.ApplicationContext.Log(erroreP.Message)
                                End If
                            Catch ex As Exception
                                Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: Eccezione: " & ex.Message & vbNewLine & ex.StackTrace)

                            End Try
                            'SyncLock queueLock
                            '    m_Queue.Add(o)
                            'End SyncLock

                            SyncLock workerLock
                                m_RunningThreads -= 1
                            End SyncLock

                            Me.endDate = Now
                        End If
                    Loop While (c IsNot Nothing) AndAlso (Not ForceQuit)
                    Me.c = Nothing
                Loop While (Not ForceQuit)
            End Sub

            Public Function IsRunning() As Boolean
                Return Me.c IsNot Nothing
            End Function

            Public Sub Start()
                Me.thread.Start()
            End Sub




        End Class

#End Region

        Private Const WORKER_GRANULARITY_MILLI As Integer = 60 * 1000 '1 minuto
        Private stopping As Boolean = False
        Private m_Running As Boolean = False
        Private Shared schedulerLock As New Object
        Private Shared workerLock As New Object
        Private WithEvents m_SchedulerTimer As System.Timers.Timer
        Private Shared m_RunningThreads As Integer = 0
        Private workers As Worker()

        Public Sub New()
            MyBase.New("modCalendarProcs", GetType(CProcedureCursor), -1)

            'Me.m_Scheduler = Nothing
            Me.m_SchedulerTimer = New System.Timers.Timer(WORKER_GRANULARITY_MILLI)

            Me.workers = Array.CreateInstance(GetType(Worker), 5)
            Me.workers(0) = New Worker(PriorityEnum.PRIORITY_HIGHER)
            Me.workers(1) = New Worker(PriorityEnum.PRIORITY_HIGH)
            Me.workers(2) = New Worker(PriorityEnum.PRIORITY_NORMAL)
            Me.workers(3) = New Worker(PriorityEnum.PRIOTITY_LOW)
            Me.workers(4) = New Worker(PriorityEnum.PRIORITY_LOWER)
        End Sub

        Private Function GetWorker(ByVal priority As PriorityEnum) As Worker
            Select Case priority
                Case PriorityEnum.PRIORITY_HIGHER : Return Me.workers(0)
                Case PriorityEnum.PRIORITY_HIGH : Return Me.workers(1)
                Case PriorityEnum.PRIORITY_NORMAL : Return Me.workers(2)
                Case PriorityEnum.PRIOTITY_LOW : Return Me.workers(3)
                Case PriorityEnum.PRIORITY_LOWER : Return Me.workers(4)
            End Select
            Return Nothing
        End Function



        Public Function CountRunningThreads() As Integer
            Return m_RunningThreads
        End Function


        Public Function GetItemByName(ByVal name As String) As CProcedura
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim items As CCollection(Of CProcedura) = Me.LoadAll
            For Each p As CProcedura In items
                If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(p.Nome, name, CompareMethod.Text) = 0) Then Return p
            Next
            Return Nothing
        End Function

        Public Sub StartBackgroundWorker()
            SyncLock schedulerLock
                Me.stopping = False
                For Each w As Worker In Me.workers
                    w.Start()
                Next
                Me.m_SchedulerTimer.Enabled = True
            End SyncLock
        End Sub

        Public Sub StopBackgroundWorker()
            SyncLock schedulerLock
                Me.stopping = True
                For Each w As Worker In Me.workers
                    w.ForceQuit = True
                    If (w.thread.IsAlive) Then w.thread.Join(5000)
                Next
                Me.m_SchedulerTimer.Enabled = False
            End SyncLock
        End Sub

        Private Sub SchedulerThread()
            SyncLock schedulerLock
                Try
                    If Me.m_Running Then Return
                    Me.m_Running = True

                    'While (Not Me.stopping)
                    Dim d As Date = DateUtils.Now
                    Dim tutte As CCollection(Of CProcedura) = Me.LoadAll
                    Dim c As CProcedura
                    Dim s As CalendarSchedule
                    For Each c In tutte
                        If (c.Stato <> ObjectStatus.OBJECT_VALID OrElse TestFlag(c.Flags, ProceduraFlags.Disabilitata)) Then Continue For
                        s = c.Programmazione.GetNextSchedule
                        If (s IsNot Nothing) Then
                            Dim dNext As Date? = s.CalcolaProssimaEsecuzione
                            If (dNext.HasValue) AndAlso (dNext.Value <= DateUtils.Now()) Then
                                'SyncLock Me.queueLock
                                Dim w As Worker = Me.GetWorker(c.Priority)
                                w.Enqueue(c)
                                w.lock.Set()
                            End If
                        End If
                        If (Me.stopping) Then Exit For
                    Next

                Catch ex As Exception
                    Try
                        Sistema.ApplicationContext.Log("Procedures: Scheduler Thread Crashed: " & ex.Message)

                    Catch ex1 As Exception

                    End Try
                End Try
                Me.m_Running = False
            End SyncLock
        End Sub

        Private Sub m_SchedulerTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles m_SchedulerTimer.Elapsed
            Me.SchedulerThread()
        End Sub
    End Class

End Namespace

Partial Class Sistema


    Private Shared m_Procedure As CProcedureClass = Nothing

    Public Shared ReadOnly Property Procedure As CProcedureClass
        Get
            If (m_Procedure Is Nothing) Then m_Procedure = New CProcedureClass
            Return m_Procedure
        End Get
    End Property

End Class

