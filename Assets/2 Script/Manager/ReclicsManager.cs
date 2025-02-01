using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
[DefaultExecutionOrder(1)]
public class ReclicsManager : MonoBehaviour
{
    public static ReclicsManager Instance { get; private set; }
    public ReclicsInfo[] reclicsDatas;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Setting Reclics");
            Instance = this;
            reclicsDatas = GameObject.FindObjectsOfType<ReclicsInfo>();
            MergeSort<ReclicsInfo> mergeSort = new MergeSort<ReclicsInfo>(reclicsDatas);
            reclicsDatas = mergeSort.get();
            Array.Reverse(reclicsDatas);
        }


    }
    void Start()
    {
        StartCoroutine(SetReclicsInfoCorutine());
    }

    IEnumerator SetReclicsInfoCorutine()
    {
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);

        GameData data = GameDataManger.Instance.GetGameData();

        for (int i = 0; i < data.reclicsCount.Count; i++)
        {
            reclicsDatas[i].Setting(data.reclicsLevel[i], data.reclicsCount[i]);
            if (!GameManager.Instance.reclisFin) GameManager.Instance.reclicsDatas.Add(reclicsDatas[i].GetReclicsData());
        }
        GameManager.Instance.reclisFin = true;
    }
}
