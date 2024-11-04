using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorText : MonoBehaviour
{
    [SerializeField] Text floorText;

    // Update is called once per frame
    void Update()
    {
        floorText.text = GameManager.Instance.gameLevel + " 층";
    }
}
