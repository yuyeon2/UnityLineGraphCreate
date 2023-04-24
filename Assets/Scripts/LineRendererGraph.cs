using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LineRendererGraph : MonoBehaviour
{
    [SerializeField]
    private Sprite circleSprite;
    private RectTransform pointRectTrans, tableTextRectTrans;
    public List<float> valueList;

    public TMP_Text graphMax, graphMin;

    LineRenderer lineRenderer;

    Material material;

    List<GameObject> point;

    public TMP_FontAsset FontAsset;
    public Material FontMaterial;
    ButtonClickEvent btnclickevt;
    private void Awake()
    {
        material = Resources.Load("Material/GraphBGMaterial", typeof(Material)) as Material;

        pointRectTrans = GameObject.Find("Points").GetComponent<RectTransform>();
        tableTextRectTrans = GameObject.Find("TableText").GetComponent<RectTransform>();
        point = new List<GameObject>();
        btnclickevt = GameObject.Find("Canvas/BtnRandomValue").GetComponent<ButtonClickEvent>();
    }

    private void CreatePoint(Vector2 anchoredPosition, int i, int count)
    {
        GameObject gameObject = new GameObject("Point" + i, typeof(SpriteRenderer));
        gameObject.transform.SetParent(pointRectTrans, false);
        gameObject.GetComponent<SpriteRenderer>().sprite = circleSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);

        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(4, 4, 4);

        if (i == 1)
        {
            gameObject.AddComponent<LineRenderer>();

            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 1;
            lineRenderer.endWidth = 1;
            lineRenderer.positionCount = count;
            lineRenderer.material = material;
        }

        point.Add(gameObject);
    }
    private void CreateText(Vector2 anchor_pos, int i)
    {
        GameObject gameObject = new GameObject("DateTxt_" + i, typeof(TextMeshPro));

        gameObject.GetComponent<TMP_Text>().font = FontAsset;
        gameObject.GetComponent<TMP_Text>().fontSharedMaterial = FontMaterial;
        gameObject.transform.SetParent(tableTextRectTrans, false);
        gameObject.GetComponent<TMP_Text>().text = btnclickevt.valueList[i-1].ToString();
        gameObject.GetComponent<TMP_Text>().color = Color.white;
        gameObject.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Midline;
        gameObject.GetComponent<TMP_Text>().fontSize = 80;
        gameObject.GetComponent<TextMeshPro>().sortingOrder = 1;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchor_pos;
        rectTransform.sizeDelta = new Vector2(35, 20);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);

    }

    public void DestroyObj()
    {
        GameObject objPoint = GameObject.Find("Canvas/GraphPanel/Points").gameObject;

        Transform[] childPointList = objPoint.GetComponentsInChildren<Transform>(true);

        if (childPointList != null)
        {
            for (int i = 1; i < childPointList.Length; i++)
            {
                if (childPointList[i] != transform)
                    Destroy(childPointList[i].gameObject);
            }
        }

        GameObject objText = GameObject.Find("Canvas/GraphPanel/TableText").gameObject;

        Transform[] childTextList = objText.GetComponentsInChildren<Transform>(true);

        if (childTextList != null)
        {
            for (int i = 1; i < childTextList.Length; i++)
            {
                if (childTextList[i] != transform)
                    Destroy(childTextList[i].gameObject);
            }
        }

        for (int i = 0; i < valueList.Count; i++)
        {
            Destroy(GameObject.Find("Canvas/GraphPanel/Points/Point_" + (i + 1)));
            Destroy(GameObject.Find("Canvas/GraphPanel/TableText/DateTxt_" + (i + 1)));
        }
    }

    public void ShowGraph(List<int> valueList, int _max, int _min)
    {
        DestroyObj();

        point.Clear();

        //패널 가로 사이즈 576의 영역보다 좀 더 좁게 하기 위해 -20
        //총 값 길이에서 1을 빼고 나누어 포인트 간격 값 설정
        float xSize = (285.54f) / (valueList.Count-1);

        graphMax.text = _max.ToString();
        graphMin.text = _min.ToString();

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition;

            if (i == 0)
            {
                //rectposition 때문인지 첫 포인트가 자꾸 걸쳐서 임의로 앞부분 띄우는 값  
                xPosition = 0;
            }
            else
            {
                xPosition = xSize * i;
            }

            float yPosition;

            if (valueList[i] == _max)
            {
                yPosition = 128.4f;
            }
            else if (valueList[i] == _min)
            {
                yPosition = 0f;
            }
            else
            {
                yPosition = ((valueList[i] * 128.4f) / (_max * 128.4f)) * 100;
            }

            //Y축의 세로 길이를 구함
            //최대,최솟값을 구하고 Y축의 세로 길이와 비율을 맞춰서 계산 
            CreatePoint(new Vector2(xPosition, yPosition), (i + 1), valueList.Count);

            if (valueList.Count < 9)
            {
                if(i == 0)
                {
                    //첫번째 오브젝트 앞부분 띄움
                    CreateText(new Vector2(xPosition + 10, -21.8f), (i + 1));
                }
                else
                {
                    CreateText(new Vector2(xPosition, -21.8f), (i + 1));
                }
            }
            else
            {
                //UI 가로 길이 때문에 9개이상 포인트는 3개의 x축 값만 띄움 
                double spaceValue = Math.Ceiling(Convert.ToDouble(valueList.Count / 2));
                if (i == 0)
                {
                    CreateText(new Vector2(xPosition + 10, -21.8f), (i + 1));
                }
                else
                {
                    CreateText(new Vector2(xPosition, -21.8f), (i + 1));
                }
            }
        }

        for (int i = 0; i < point.Count; i++)
        {
            lineRenderer.SetPosition(i, point[i].GetComponent<Transform>().position);
        }
    }

   
}
