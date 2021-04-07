﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMenuManager : MonoBehaviour
{
    [SerializeField]
    NoteSelector notePanelPF;
    
    BlockAttributes attributes;
    NoteSelector notePanel;

    List<int> notes;        // List of notes to be played 
    int numOfNotes = 0;     // Number of notes
    string mode = "tick";   // Notes play mode

    private void Awake()
    {
        attributes = GetComponent<BlockAttributes>();
        // TODO: PROVISIONAL
        /*notes = new List<int>(new int[3] { 60, 65, 67 });
        numOfNotes = notes.Count;*/
    }

    private void Start()
    {
        SynthMessage msg = (attributes.GetActionMessage() as SynthMessage);
        notes = msg.notes;
        numOfNotes = msg.numOfNotes;
        mode = msg.mode;
    }

    public void SpawnNotePanel()
    {
        if (!notePanel)
        {
            notePanel = Instantiate(notePanelPF, LoopManager.instance.canvas.transform);
            // Moves the panel on top of the UI and to the center of the screen
            notePanel.transform.SetAsLastSibling();
            notePanel.transform.localPosition = new Vector3(0, 0, 0);
            notePanel.Configure(this);
        }
        else
            notePanel.gameObject.SetActive(true);
        
    }

    #region Note Selection Methods
    public void AddNote(string note)
    {
        int num = TranslateNote(note);
        ActionMessage msg = attributes.GetActionMessage();
        (msg as SynthMessage).notes.Add(num);
        (msg as SynthMessage).numOfNotes++;

        notes = (msg as SynthMessage).notes;
        numOfNotes = (msg as SynthMessage).numOfNotes;
    }

    //Translate notes from English musical nomenclature to sonic pi's number system
    int TranslateNote(string note)
    {
        int aux = 0;
        switch (note[0])
        {
            case 'C':
                if (note[1] == '#') aux = 12 * (int.Parse(note[2].ToString()) + 1) + 1;
                else aux = 12 * (int.Parse(note[1].ToString()) + 1);
                break;
            case 'D':
                if (note[1] == '#') aux = 12 * (int.Parse(note[2].ToString()) + 1) + 3;
                else aux = 12 * (int.Parse(note[1].ToString()) + 1) + 2;
                break;
            case 'E':
                aux = 12 * (int.Parse(note[1].ToString()) + 1) + 4;
                break;
            case 'F':
                if (note[1] == '#') aux = 12 * (int.Parse(note[2].ToString()) + 1) + 6;
                else aux = 12 * (int.Parse(note[1].ToString()) + 1) + 5;
                break;
            case 'G':
                if (note[1] == '#') aux = 12 * (int.Parse(note[2].ToString()) + 1) + 8;
                else aux = 12 * (int.Parse(note[1].ToString()) + 1) + 7;
                break;
            case 'A':
                if (note[1] == '#') aux = 12 * (int.Parse(note[2].ToString()) + 1) + 10;
                else aux = 12 * (int.Parse(note[1].ToString()) + 1) + 9;
                break;
            case 'B':
                aux = 12 * (int.Parse(note[1].ToString()) + 1) + 11;
                break;
        }
        return aux;
    }
    #endregion
}
