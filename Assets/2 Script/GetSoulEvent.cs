using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSoulEvent : MonoBehaviour
{
    public Queue<Text> texts = new Queue<Text>();
    public RectTransform rect;
    public Font font;
    private void Awake() {
        GameManager.Instance.dropSoul += ShowGetSoulEvent;
        rect = GetComponent<RectTransform>();
    }
    public void ShowGetSoulEvent(UnitData unit = null){
        Text curText;
        if(texts.Count == 0) {
            GameObject texts = new GameObject();
            texts.transform.SetParent(transform);
            texts.AddComponent<Text>();
            texts.SetActive(false);

            curText = texts.GetComponent<Text>();
            curText.font = font;
            curText.fontStyle = FontStyle.Bold;
            curText.fontSize = 25;
            curText.horizontalOverflow = HorizontalWrapMode.Overflow;

            RectTransform rect = curText.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(180f , 40f);
        }
        else {
            curText = texts.Dequeue();
            curText.transform.SetParent(transform);
        }

        curText.text = "Get Soul";
        rect.sizeDelta += new Vector2(0f , 40f);
        curText.gameObject.SetActive(true);
        StartCoroutine(ReturnObject(curText));
    }

    public IEnumerator ReturnObject(Text text){
        yield return new WaitForSeconds(3f);
         rect.sizeDelta -= new Vector2(0f , 40f);
        texts.Enqueue(text);
        text.gameObject.SetActive(false);
    }
}
