Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Internals
Imports minidom.Office

Namespace Internals



    Public NotInheritable Class CStickyNotesClass
        Inherits CModulesClass(Of StickyNote)

        Friend Sub New()
            MyBase.New("modOfficeStickyNotes", GetType(StickyNotesCursor))
        End Sub


    End Class

End Namespace

Partial Class Office


    Private Shared m_StickyNotes As CStickyNotesClass = Nothing

    Public Shared ReadOnly Property StickyNotes As CStickyNotesClass
        Get
            If (m_StickyNotes Is Nothing) Then m_StickyNotes = New CStickyNotesClass
            Return m_StickyNotes
        End Get
    End Property

End Class