﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    #region Singleton Constructors
    public static LoopManager instance;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            instance = this;
            loops = new List<LoopBlock>();
            DontDestroyOnLoad(gameObject);
        }
    }


    #endregion

    int loopCount = 0;

    List<LoopBlock> loops;
    [SerializeField] GameObject loopContainerGO;
    [SerializeField] GameObject loopPF;

    public Canvas canvas;
    [SerializeField] GameObject destroyZone;
    [SerializeField] GameObject addLoopZone;


    private void Start()
    {
        // Add an empty loop on Start
        AddLoop();
    }

    /**********************************/
    //  LOOP METHODS
    /**********************************/

    /// <summary>
    /// Tell each active loop to send its messages
    /// </summary>
    public void RunLoops()
    {
        //List<List<ActionMessage>> loopMsgs = new List<List<ActionMessage>>();
        List<object> msgValues = new List<object>();
        List<object> auxValuesList = new List<object>();
        int numberOfLoops = 0;

        foreach (LoopBlock loop in loops)
        {
            // Get message group from loop
            List<ActionMessage> msgGroup = loop.GetActionMessages();

            // Check if the msgGroup is empty
            if (msgGroup != null)
            {
                // Increase number of loop messages
                numberOfLoops++;
                // Add id of the loop
                msgValues.Add(loop.GetLoopId());
                int valuesCount = 0;

                foreach (ActionMessage message in msgGroup)
                {
                    List<object> valueList = message.ToObjectList();
                    // Increment values count
                    valuesCount += valueList.Count;
                    // Append to message group value list
                    auxValuesList.AddRange(valueList);
                }

                // Add number of values
                msgValues.Add(valuesCount);
                // Add number of commands
                msgValues.Add(msgGroup.Count);
                // Add all command values
                msgValues.AddRange(auxValuesList);

                // Clear auxiliar value list
                auxValuesList.Clear();
                // Clear loop messages
                loop.ClearMessages();
            }
        }

        // Add number of loops at the start
        msgValues.Insert(0, numberOfLoops);

        // Send message to Sonic Pi
        SonicPiManager.instance.SendActionMessageGroup(msgValues);
    }

    /// <summary>
    /// Creates a new loop and adds it to the loop manager
    /// </summary>
    public LoopBlock AddLoop()
    {
        // Check loop number limit
        if (loopCount >= SonicPiManager.instance.GetNumberOfLoops()) return null;

        // Creates the loop Object
        GameObject newLoopParent = Instantiate(loopPF, loopContainerGO.transform);

        // Initialize the loop
        LoopBlock newLoop = newLoopParent.GetComponentInChildren<LoopBlock>();
        if (!newLoop)
        {
            Debug.LogError("Error: Loop component not found in new loop object.");
            Destroy(newLoopParent);
            return null;
        }
        newLoop.Init(loopCount);

        // Add loop to list of loops
        loops.Add(newLoop);

        // Send message to sonic pi asking for loop creation
        //SonicPiManager.instance.SendNewLoopMessage();

        loopCount++;

        return newLoop;
    }

    public void RemoveLoop(int loopId)
    {
        // Send message to Sonic Pi
        SonicPiManager.instance.SendDeleteLoopMessage(loopId);

        // Remove from list of loops
        loops.RemoveAt(loopId);
        loopCount--;
        // Update other loops id
        for (int i = loopId; i < loops.Count; i++)
        {
            loops[i].SetLoopId(i);
            loops[i].SetChangedLoop(true);
        }
    }

    /**********************************/
    //  BLOCK METHODS
    /**********************************/
    public void RemoveBlockFromLoop(int loopId, int blockId)
    {
        loops[loopId].RemoveBlockAt(blockId);
    }

    public void EditBlockAttribute(int loopId, int blockId, string attribute, float value)
    {
        ActionMessage msg = new EditMessage();
        msg.loopId = loopId;
        msg.blockId = blockId;
        msg.actionName = "edit";
        (msg as EditMessage).attributeName = attribute;
        (msg as EditMessage).newValue = value;
        loops[loopId].AddMessage(msg);
    }

    public void AddBlockToLoop(int loopId, int blockId, string action)
    {
        // Ask the loop to add the block
        loops[loopId].AddBlock(action, blockId);
    }

    // Change the position of a block inside its loop
    public void ChangeBlockPosition(int loopId, BlockShape block, int newBlockId)
    {
        loops[loopId].ChangeBlockPosition(block, newBlockId);
    }

    public void AddMessage(int loopId, ActionMessage message)
    {
        loops[loopId].AddMessage(message);
    }

    public void SetDestroyZone(bool active)
    {
        destroyZone.SetActive(active);
    }

    public void SetAddLoopZone(bool active)
    {
        addLoopZone.SetActive(active);
    }
}
