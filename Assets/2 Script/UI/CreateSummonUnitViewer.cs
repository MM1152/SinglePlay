using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateSummonUnitViewer : MonoBehaviour
{
    [SerializeField] GameObject summonUnitViewer;
    List<SummonUnitViewer> viewerList = new List<SummonUnitViewer>();

    public void CreateViewer(Unit unit){
        GameObject viewer = Instantiate(summonUnitViewer , transform);
        SummonUnitViewer unitViewer = viewer.GetComponent<SummonUnitViewer>();
        viewerList.Add(unitViewer);
        unitViewer.unit = unit;
    }

    public void RedirectUnit(Unit unit) {
        foreach(SummonUnitViewer view in viewerList) {
            if(view.unitName == unit.name) {
                view.unit = unit;
            }
        }
    }
}
