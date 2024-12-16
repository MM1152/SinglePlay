using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCharacterStatusView : MonoBehaviour{
    [SerializeField] CharacterStatusViewer characterStatusView;
    bool isSelect;
    private void Update()
    {
        if (Input.touchCount > 0 && !isSelect)
        {
            isSelect = true;
            RaycastHit2D hit = Physics2D.Raycast(Input.GetTouch(0).position, Camera.main.transform.position);
            
            if (hit.collider != null && hit.collider.gameObject == this.gameObject && !characterStatusView.isOpen)
            {
                Debug.Log(hit.collider);
                characterStatusView.isOpen = true;
            }
            else
            {
                characterStatusView.isOpen = false;
            }
        }
        if(Input.touchCount == 0) {
            isSelect = false;
        }
    }

}
