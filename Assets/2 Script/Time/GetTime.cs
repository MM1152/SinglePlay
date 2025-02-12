using System;
using System.Collections;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
public class GetTime : MonoBehaviour {
    public string url = "";
    public static string currentTime;

    void Awake(){
        if(currentTime == null) StartCoroutine(WebChk());
    }
    IEnumerator WebChk(){
        UnityWebRequest request = new UnityWebRequest();
        using(request = UnityWebRequest.Get(url)){
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError) {
                Debug.LogError(request.error);
            }
            else {
                string data = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(data);
                currentTime = dateTime.ToString("yyyy-MM-dd");
                Debug.Log(currentTime); 
            }
        }
    }
    
}