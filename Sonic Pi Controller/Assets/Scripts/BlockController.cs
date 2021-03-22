﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockController : MonoBehaviour
{
    public TMPro.TMP_Text blockNameText;
    int id;

    public void ConfigureBlock(int blockId, string action)
    {
        blockNameText.text = action;
        id = blockId;
        
        // Creates the message object
        ActionMessage msg = null;

        //TODO: De momento está hardcodeado para pruebas
        if (action == "synth")
        {
            
            if (id == 1)
            {
                msg = new SynthMessage();
                msg.actionName = action;
                msg.blockId = id;
                (msg as SynthMessage).playerName = "piano";
                (msg as SynthMessage).mode = "tick";
                (msg as SynthMessage).notes = new List<int>(new int[3] { 60, 65, 67 });
                (msg as SynthMessage).numOfNotes = (msg as SynthMessage).notes.Count;
                (msg as SynthMessage).pan = 0;
                (msg as SynthMessage).fx = "echo";
                (msg as SynthMessage).release = 5;
            }
            else
            {
                msg = new PlayerMessage();
                msg.actionName = "sample";
                msg.blockId = id;
                (msg as PlayerMessage).playerName = "bd_haus";
                (msg as PlayerMessage).fx = "echo";
                (msg as PlayerMessage).pan = 0;
            }
        }
        else if (action == "sleep")
        {
            msg = new SleepMessage();
            msg.actionName = action;
            msg.blockId = id;
            (msg as SleepMessage).sleepDuration = 1;
        }

            // Sends the action message
            SonicPiManager.Instance.sendActionMessage(msg);
    
    }
}
