using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerPhaseStriker : PhaseStriker
{
    private void Awake() {
        archorPhaseStriker = Instantiate(archor , transform.parent).GetComponent<ArchorPhaseStriker>();
        archorPhaseStriker.canFollow = false;
        archorPhaseStriker.gameObject.AddComponent<Summon>();
        archorPhaseStriker.gameObject.SetActive(false);

        base.Awake();
    }

    private void Start(){
        base.Start();
        archorPhaseStriker.Setting(this);
    }


    
}
