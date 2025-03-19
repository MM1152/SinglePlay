using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            Debug.Log($"Unit Name : {view.unitName}");
            if (view.unitName == unit.name || (unit.unit.changeFormInfo != null && view.unitName == unit.unit.changeFormInfo.SummonPrefeb.name + "(Clone)"))
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
