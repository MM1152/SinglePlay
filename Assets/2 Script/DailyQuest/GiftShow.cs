using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GiftShow : MonoBehaviour {
    
    private int giftValue;
    private int soulValue;

    [SerializeField] Text soulValueText;
    [SerializeField] Image giftImage;
    [SerializeField] Text giftValueText;    

    public void Setting(int soulValue , int giftValue , Sprite giftImage) {
        this.giftImage.sprite = giftImage;
        this.giftValue = giftValue;
        this.soulValue = soulValue;
        Show();
    }

    private void Show(){
        soulValueText.text = soulValue.ToString();
        if(giftValue != 0) { 
            giftValueText.text = giftValue.ToString();
            giftImage.gameObject.SetActive(false);
            giftValueText.gameObject.SetActive(true);
        }
        else {
            giftImage.gameObject.SetActive(false);
            giftValueText.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
    } 

    private void Update(){
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            gameObject.SetActive(false);
        }
    }
}