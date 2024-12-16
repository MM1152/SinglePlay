using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSummonUnitViewer : MonoBehaviour
{
    [SerializeField] GameObject summonUnitViewer;

    public void CreateViewer(Unit unit){
        GameObject viewer = Instantiate(summonUnitViewer , transform);
        viewer.GetComponent<SummonUnitViewer>().unit = unit;
    }
}
