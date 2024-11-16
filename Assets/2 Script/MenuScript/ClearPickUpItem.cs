using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPickUpItem : MonoBehaviour
{
    [SerializeField] Transform clearTarget;
    private void OnDisable() {
        foreach(Transform child in clearTarget.transform) {
            Destroy(child.gameObject);
        }
    }
}
