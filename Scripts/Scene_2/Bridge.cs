using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bridge : MonoBehaviour
{
    public Transform startTrans, endTrans; 
    public GameObject midSection, extention; 

    public float bridgeLength; 
    private int midCount, extenCountOneSide;  
    private float distance, extDistanceOneSide;

    private Vector3 unitVector;
     
    public Text distanceText, midSectionText, extentionText;

    void Start()
    {
        Fill();
    }

    private void UpdateUI()
    {
        distanceText.text = "Distance:  " + distance.ToString("F2");
        midSectionText.text = midCount.ToString();
        extentionText.text = (extenCountOneSide * 2).ToString();

        if(distance <= 0)
        {
            distanceText.color = new Color(1, 0.3f, 0.3f);
        }
        else
        {
            distanceText.color = new Color(1, 1, 1);
        }
    }

    public void Fill()
    {
        Debug.Log("Fill");
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        //distance = Vector3.Distance(startTrans.position, endTrans.position);
        distance = startTrans.position.x - endTrans.transform.position.x;

        unitVector = Vector3.Normalize(startTrans.position - endTrans.position);
        midCount = (int)((distance - bridgeLength) / 8);
        extDistanceOneSide = (distance - bridgeLength - midCount * 8) / 2;
        extenCountOneSide = (int)(extDistanceOneSide + 2);

        for (int i = 0; i < midCount; i++)
        {
            GameObject mid = GameObject.Instantiate(midSection, endTrans.position + (extDistanceOneSide + bridgeLength + i * bridgeLength) * unitVector, Quaternion.identity);
            mid.transform.SetParent(transform);
        }

        for (int i = 0; i < extenCountOneSide; i++)
        {
            GameObject leftExt = GameObject.Instantiate(extention, endTrans.position + ( bridgeLength/2 + i ) * unitVector, Quaternion.identity);
            GameObject rightExt =  GameObject.Instantiate(extention, startTrans.position - ( bridgeLength/2 + i ) * unitVector, Quaternion.identity);
            leftExt.transform.SetParent(transform);
            rightExt.transform.SetParent(transform);
        }

        UpdateUI();
    }

}
