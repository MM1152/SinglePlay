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

        gameObject.transform.localScale = new Vector3(1 , 1 , 1f);
        base.Awake();
    }
    void Update()
    {
        base.Update();
    }
    private void Start(){
        base.Start();
        archorPhaseStriker.Setting(this);
    }


    
}
