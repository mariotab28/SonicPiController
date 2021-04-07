﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSelector : MonoBehaviour
{
    NoteMenuManager noteMenu;

    // List of note buttons (NoteUpdater objects)
    List<NoteUpdater> noteButtons = new List<NoteUpdater>();

    int octave_ = 4;

    void Start()
    {
        UpdateButtonsOctave();
    }

    public void AddNoteButton(NoteUpdater noteButton)
    {
        noteButtons.Add(noteButton);
    }

    public void UpOctave()
    {
        if(octave_ < 8) octave_++;
        UpdateButtonsOctave();
    }

    public void DownOctave()
    {
        if(octave_ > 1) octave_--;
        UpdateButtonsOctave();
    }

    void UpdateButtonsOctave()
    {
        foreach (NoteUpdater noteButton in noteButtons)
            noteButton.UpdateOctave(octave_);
    }

    public int GetOctave()
    {
        return octave_;
    }

    public void Configure(NoteMenuManager noteMenu)
    {
        this.noteMenu = noteMenu;
    }

    public void AddNote(string note)
    {
        noteMenu.AddNote(note);
    }

}
