using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Create Skill", order = 1)]
public class Skill : ScriptableObject{

    public string skillName;
    public int skillID;
    public bool useSkillID = true;

    public enum blastType
    {
        ROOT, FREEZE, DAMAGE, SUSPEND
    }

    public blastType myType;
}
