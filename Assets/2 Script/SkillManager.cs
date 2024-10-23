using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get ; private set ;}

    public bool SummonSkill;

    private void Awake() {
        Instance = this;
    }

    public void UnLockSkill(string data) {
        
        switch (data) {
            case "SummonSkill" : 
                SummonSkill = true;
                break;
        }
    }

}
