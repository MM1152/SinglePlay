using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateSummonUnitViewer : MonoBehaviour
{
    [SerializeField] GameObject summonUnitViewer;
    List<SummonUnitViewer> viewerList = new List<SummonUnitViewer>();

    public void CreateViewer(Unit unit)
    {
        foreach (SummonUnitViewer view in viewerList)
        {
            if (view.unitName == unit.name)
            {
                view.unit = unit;
                return;
            }
        }

        GameObject viewer = Instantiate(summonUnitViewer, transform);
        SummonUnitViewer unitViewer = viewer.GetComponent<SummonUnitViewer>();
        viewerList.Add(unitViewer);
        unitViewer.unit = unit;
    }

    public void ChnageFormUnit(ChangeForm changeForm)
    {
        foreach (SummonUnitViewer view in viewerList)
        {
            if (view.unitName == changeForm.GetUnit().name) {
                view.unit = changeForm.GetChangeUnitData();
                if(view.cameraMoveMent.GetCameraTarget() == changeForm.GetUnit()) {
                    view.cameraMoveMent.SettingCameraTarget(changeForm.GetChangeUnitData());
                }
                
            }
        }
    }
}
