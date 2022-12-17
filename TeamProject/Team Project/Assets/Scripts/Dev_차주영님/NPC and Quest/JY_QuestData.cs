using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Quest_Data", menuName ="Quest_Data")]
public class JY_QuestData : ScriptableObject
{
    [SerializeField]
    private string questName;
    public string QUESTNAME { get => questName; }
    [SerializeField]
    private string questContent;
    public string QUESTCONTENT { get => questContent; }
    [SerializeField]
    private string questNPC;
    public string QUESTNPC { get => questNPC; }
    [SerializeField]
    private string questTarget;
    public string QUESTTARGET { get => questTarget; }
    [SerializeField]
    private int questGoal;
    public int QUESTGOAL { get => questGoal; }

    [SerializeField]
    private string questDialog_1;
    public string QUESTDIALOG_1 { get => questDialog_1; }
    [SerializeField]
    private string questDialog_2;
    public string QUESTDIALOG_2 { get => questDialog_2; }
    [SerializeField]
    private string questDialog_Reward;
    public string QUESTDIALOG_REWARD { get => questDialog_Reward; }
    [SerializeField]
    private string questDialog_3;
    public string QUESTDIALOG_3 { get => questDialog_3; }
    [SerializeField]
    private string questDialog_4;
    public string QUESTDIALOG_4 { get => questDialog_4; }
    [SerializeField]
    private string questDialog_5;
    public string QUESTDIALOG_5 { get => questDialog_5; }
}
