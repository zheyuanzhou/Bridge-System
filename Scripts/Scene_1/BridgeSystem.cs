using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BridgeSystem : MonoBehaviour
{
    [SerializeField] private float distance;
    private float newDistance;

    [Header("GameObject")]
    public GameObject startSection;
    public GameObject endSection;
    public GameObject midSection;
    public GameObject extention;

    private float startWidth, endWidth, midSectionWidth, extentionWidth;

    //public int preExtentionCount;
    [SerializeField] private int midSectionCount, extentionCount;
    private float space;
    private float centerPosX;

    public List<GameObject> containers = new List<GameObject>();

    public Text midSectionText, extentionText, distanceText;

    private void Awake()
    {
        startWidth = startSection.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        endWidth = endSection.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        midSectionWidth = midSection.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        extentionWidth = extention.GetComponent<MeshRenderer>().bounds.size.x;

        //Debug.Log("A width: " + startWidth + " | " + "B width: " + endWidth);
        Debug.Log("MID SECTION WIDTH: " + midSectionWidth);
        Debug.Log("EXTENTION WIDTH: " + extentionWidth);
    }

    private void Start()
    {
        Init();
        newDistance = distance;
    }

    private void UpdateUI()
    {
        midSectionText.text = midSectionCount.ToString();
        extentionText.text = extentionCount.ToString();
    }

    private void Update()
    {
        distance = Vector3.Distance(startSection.transform.position, endSection.transform.position) - (startWidth / 2 + endWidth / 2);
        centerPosX = (startSection.transform.position.x + endSection.transform.position.x) / 2;

        distanceText.text = "Distance:  " + distance.ToString("F2");
        if (startSection.transform.position.x < endSection.transform.position.x)
        {
            distanceText.color = new Color(1, 0.3f, 0.3f);
        }
        else
        {
            distanceText.color = new Color(1, 1, 1);
        }

        if (Mathf.Abs(distance - newDistance) >= (1 * extentionWidth))
        {
            Clear();
            newDistance = distance;
            Init();//TODO Remove outside of the Update
            UpdateUI();
        }
        else
        {
            return;
        }
        Debug.Log("-----DISTANCE-----" + distance);
    }

    private void Clear()
    {
        for(int i = 0; i < containers.Count; i++)
        {
            Destroy(containers[i]);
        }
        containers.Clear();
    }

    private void Init()
    {
        //distance = Vector3.Distance(startSection.transform.position, endSection.transform.position) - (startWidth / 2 + endWidth / 2);
        //centerPosX = (startSection.transform.position.x + endSection.transform.position.x) / 2;

        //if (distance >= midSectionWidth)
        if(Mathf.FloorToInt(distance / midSectionWidth) >= 1)
        {
            #region Calculate the Count Number of MidSection and Filter/Extention

            midSectionCount = Mathf.FloorToInt(distance / midSectionWidth);
            Debug.Log("--Mid Section Count--: " + midSectionCount);

            float extentionDis = distance - (midSectionWidth * midSectionCount);
            extentionCount = Mathf.FloorToInt(extentionDis / extentionWidth);
            Debug.Log("--Extention Count--: " + extentionCount);

            #endregion


            space = (distance - (midSectionWidth * midSectionCount) - (extentionWidth * extentionCount)) / (midSectionCount + extentionCount + 1);

            //-=-=-=-=-=-=-=-=-=-=-=-
            if(midSectionCount % 2 != 0)//ODD 
            {
                Debug.Log("Odd");
                for(int i = 1; i < midSectionCount + 1; i++)
                {
                    GameObject go = Instantiate(midSection, new Vector2(centerPosX + Mathf.Pow(-1, i) * ((Mathf.CeilToInt(i / 2)) * (space + midSectionWidth)), 0), Quaternion.identity);
                    containers.Add(go);
                }
            }
            else
            {
                Debug.Log("Even");
                float mixDis = space + midSectionWidth;
                for(int i = 0; i < midSectionCount; i++)
                {
                    if(i % 2 == 0)
                    {
                        GameObject go = Instantiate(midSection, new Vector2(centerPosX + ((i + 1) * mixDis) * 0.5f, 0), Quaternion.identity);
                        containers.Add(go);
                        Debug.Log("Left");
                    }
                    else//MARKER ODD
                    {
                        GameObject go = Instantiate(midSection, new Vector2(centerPosX - ((i / 2) * mixDis) - 0.5f * midSectionWidth, 0), Quaternion.identity);//MARKER Correct
                        containers.Add(go);
                        Debug.Log("Right");
                    }
                }
            }

            if (extentionCount >= 2)//Only greater than 2 can run
            {
                if(extentionCount % 2 != 0)//If Number is ODD Number
                {
                    extentionCount -= 1;//TODO
                }

                if(midSectionCount % 2 != 0)//Mid Section is ODD Number
                {
                    for(int i = 0; i < extentionCount; i++)//MARKER CHANGE 0->2
                    {
                        float newPosX = (midSectionCount * 0.5f * midSectionWidth) + ((midSectionCount + 1) * 0.5f * space) + (Mathf.CeilToInt((i - 1) * 0.5f) * (space + extentionWidth));
                        GameObject go = Instantiate(extention, new Vector2(centerPosX + Mathf.Pow(-1, i) * (newPosX + 0.5f * extentionWidth), 0), Quaternion.identity);
                        containers.Add(go);
                    }
                }
                else
                {
                    for(int i =1; i < extentionCount + 1; i++)//MARKER CHANGE 1->3
                    {
                        //float newPosX = ((midSectionCount + 1) * 0.5f * space) + (midSectionCount * 0.5f * midSectionWidth) + (Mathf.CeilToInt((i - 1) * 0.5f) * (space + extentionWidth));
                        if(i % 2 ==0)
                        {
                            float newPosX = ((midSectionCount + 1) * 0.5f * space) + (midSectionCount * 0.5f * midSectionWidth) + (Mathf.CeilToInt((i - 1) * 0.5f) * (space) + (i * 0.5f) * extentionWidth);
                            GameObject go = Instantiate(extention, new Vector2(centerPosX + Mathf.Pow(-1, i) * (newPosX - 0.25f * extentionWidth), 0), Quaternion.identity);
                            containers.Add(go);
                        }
                        else
                        {
                            float newPosX = ((midSectionCount + 1) * 0.5f * space) + (midSectionCount * 0.5f * midSectionWidth) + (Mathf.CeilToInt((i - 1) * 0.5f) * (space) + ((i+1) * 0.5f) * extentionWidth);
                            GameObject go = Instantiate(extention, new Vector2(centerPosX + Mathf.Pow(-1, i) * (newPosX - 0.25f * extentionWidth), 0), Quaternion.identity);
                            containers.Add(go);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        } 
        else
        {
            midSectionCount = 0;
            Debug.Log("Mid Section Count: " + midSectionCount);//MARKER SHOULD BE 0 ANY TIME

            extentionCount = Mathf.FloorToInt(distance / extentionWidth);
            Debug.Log("Extention Count: " + extentionCount);

            //-=-=-=-==-=-=

            if(extentionCount != 0)
            {
                switch (extentionCount)
                {
                    case 1:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go = Instantiate(extention, new Vector2(centerPosX, 0), Quaternion.identity);//Center Extention
                        containers.Add(go);
                        break;
                    case 2:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go_1 = Instantiate(extention, new Vector2(centerPosX + 0.5f * space + 0.5f * extentionWidth, 0), Quaternion.identity);
                        GameObject go_2 = Instantiate(extention, new Vector2(centerPosX - 0.5f * space - 0.5f * extentionWidth, 0), Quaternion.identity);
                        containers.Add(go_1);
                        containers.Add(go_2);
                        break;
                    case 3:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go_3 = Instantiate(extention, new Vector2(centerPosX, 0), Quaternion.identity);
                        GameObject go_4 = Instantiate(extention, new Vector2(centerPosX + space + extentionWidth, 0), Quaternion.identity);
                        GameObject go_5 = Instantiate(extention, new Vector2(centerPosX - space - extentionWidth, 0), Quaternion.identity);
                        containers.Add(go_3);
                        containers.Add(go_4);
                        containers.Add(go_5);
                        break;
                    case 4:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go_6 = Instantiate(extention, new Vector2(centerPosX + 0.5f * space + extentionWidth / 2, 0), Quaternion.identity);
                        GameObject go_7 = Instantiate(extention, new Vector2(centerPosX - 0.5f * space - extentionWidth / 2, 0), Quaternion.identity);
                        GameObject go_8 = Instantiate(extention, new Vector2(centerPosX + 1.5f * space + 1.5f * extentionWidth, 0), Quaternion.identity);
                        GameObject go_9 = Instantiate(extention, new Vector2(centerPosX - 1.5f * space - 1.5f * extentionWidth, 0), Quaternion.identity);
                        containers.Add(go_6);
                        containers.Add(go_7);
                        containers.Add(go_8);
                        containers.Add(go_9);
                        break;
                    case 5:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go_10 = Instantiate(extention, new Vector2(centerPosX, 0), Quaternion.identity);
                        GameObject go_11 = Instantiate(extention, new Vector2(centerPosX + space + extentionWidth, 0), Quaternion.identity);
                        GameObject go_12 = Instantiate(extention, new Vector2(centerPosX - space - extentionWidth, 0), Quaternion.identity);
                        GameObject go_13 = Instantiate(extention, new Vector2(centerPosX + 2 * extentionWidth + 2 * space, 0), Quaternion.identity);
                        GameObject go_14 = Instantiate(extention, new Vector2(centerPosX - 2 * extentionWidth - 2 * space, 0), Quaternion.identity);
                        containers.Add(go_10);
                        containers.Add(go_11);
                        containers.Add(go_12);
                        containers.Add(go_13);
                        containers.Add(go_14);
                        break;
                    case 6:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go_15 = Instantiate(extention, new Vector2(centerPosX + 0.5f * space + extentionWidth / 2, 0), Quaternion.identity);
                        GameObject go_16 = Instantiate(extention, new Vector2(centerPosX - 0.5f * space - extentionWidth / 2, 0), Quaternion.identity);
                        GameObject go_17 = Instantiate(extention, new Vector2(centerPosX + 1.5f * space + 1.5f * extentionWidth, 0), Quaternion.identity);
                        GameObject go_18 = Instantiate(extention, new Vector2(centerPosX - 1.5f * space - 1.5f * extentionWidth, 0), Quaternion.identity);
                        GameObject go_19 = Instantiate(extention, new Vector2(centerPosX + 2.5f * space + 2.5f * extentionWidth, 0), Quaternion.identity);
                        GameObject go_20 = Instantiate(extention, new Vector2(centerPosX - 2.5f * space - 2.5f * extentionWidth, 0), Quaternion.identity);
                        containers.Add(go_15);
                        containers.Add(go_16);
                        containers.Add(go_17);
                        containers.Add(go_18);
                        containers.Add(go_19);
                        containers.Add(go_20);
                        break;
                    case 7:
                        space = (distance - (extentionWidth * extentionCount)) / (extentionCount + 1);
                        GameObject go_21 = Instantiate(extention, new Vector2(centerPosX, 0), Quaternion.identity);
                        GameObject go_22 = Instantiate(extention, new Vector2(centerPosX + space + extentionWidth, 0), Quaternion.identity);
                        GameObject go_23 = Instantiate(extention, new Vector2(centerPosX - space - extentionWidth, 0), Quaternion.identity);
                        GameObject go_24 = Instantiate(extention, new Vector2(centerPosX + 2 * extentionWidth + 2 * space, 0), Quaternion.identity);
                        GameObject go_25 = Instantiate(extention, new Vector2(centerPosX - 2 * extentionWidth - 2 * space, 0), Quaternion.identity);
                        GameObject go_26 = Instantiate(extention, new Vector2(centerPosX + 3 * extentionWidth + 3 * space, 0), Quaternion.identity);
                        GameObject go_27 = Instantiate(extention, new Vector2(centerPosX - 3 * extentionWidth - 3 * space, 0), Quaternion.identity);
                        containers.Add(go_21);
                        containers.Add(go_22);
                        containers.Add(go_23);
                        containers.Add(go_24);
                        containers.Add(go_25);
                        containers.Add(go_26);
                        containers.Add(go_27);
                        break;
                }
            }
            else
            {
                return;
            }
        }
    }

}
