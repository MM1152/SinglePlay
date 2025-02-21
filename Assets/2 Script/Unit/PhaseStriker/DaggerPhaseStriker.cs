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
    void Update()
    {
        base.Update();
    }
    private void Start(){
        base.Start();
        archorPhaseStriker.Setting(this);
        gameObject.transform.localScale = new Vector3(0.6f , 0.6f , 1f);
    }


    
}
