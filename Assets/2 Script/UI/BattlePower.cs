using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BattlePower : MonoBehaviour
{
    [SerializeField] Text text;
    int battlePower;
    StringBuilder sb;
    public void SettingBattlePower(int power){
        StartCoroutine(UpdateNumber(power));
    }

    IEnumerator UpdateNumber(int number){
        if(number < 0) {
            for(int i = battlePower; i >= battlePower + number; i -= 5) {
                text.text = i.ToString();
                yield return null;
            }
        }
        else {
            for(int i = battlePower; i <= battlePower + number; i += 5) {
                text.text = i.ToString();
                yield return null;
            }
        }

        battlePower += number;
        text.text = battlePower.ToString();
    }
}
