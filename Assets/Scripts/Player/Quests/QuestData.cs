using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest Details")]
public class QuestData : ScriptableObject
{
    public string QuestName;
    [Multiline]
    public string Objectives;
    public bool IsCompleted = false;
    public QuestData NextQuest;
}
