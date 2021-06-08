using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class UnfinishedGamesBrainstormData : ScriptableObject
    {
    public List<BrainStormEntry> Entries = new List<BrainStormEntry>();
    }

[System.Serializable]
public class BrainStormEntry
    {
    public string Title;
    public string Text;
    public int Tag;

    public BrainStormEntry(string title, string text, int tag)
        {
        Title = title;
        Text = text;
        Tag = tag;
        }    
    }
