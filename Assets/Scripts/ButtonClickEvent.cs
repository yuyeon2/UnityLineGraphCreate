using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonClickEvent : MonoBehaviour
{
    public TMP_Text inputMax;
    public TMP_Text inputMin;

    [HideInInspector]
    public List<int> valueList;

    LineRendererGraph lineGraph;

    private void Start()
    {
        valueList = new List<int>();
        lineGraph = GameObject.Find("GraphPanel").GetComponent<LineRendererGraph>();
    }

    public void RandomValueSetBtnClick()
    {
        int randomValue;
        
        valueList.Clear();

        //point 개수 3~10 범위설정
        int randomCount = UnityEngine.Random.Range(3, 10);

        for (int i = 0; i < randomCount; i++)
        {
            //1~100 사이 랜덤 값 
            int random = UnityEngine.Random.Range(1, 100);
            randomValue = random;

            valueList.Add(randomValue);
        }

        int max = 0;
        int min = valueList[0];

        for (int i = 0; i < valueList.Count; i++)
        {
            int tempNum = valueList[i];

            if (tempNum > max)
            {
                max = tempNum;
            }
            else if (tempNum < min)
            {
                min = tempNum;
            }
        }

        inputMax.text = max.ToString();
        inputMin.text = min.ToString();

        lineGraph.ShowGraph(valueList, max, min);
    }
}
