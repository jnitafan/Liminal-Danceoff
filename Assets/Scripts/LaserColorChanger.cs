using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserColorChanger : MonoBehaviour
{

    public bool changeColor = true;
    public GameObject[] LaserBeams;

    [ColorUsageAttribute(true, true)]
    public Color[] LaserColors = { new Color32(174, 71, 217, 255), new Color32(34, 169, 238, 255), new Color32(0, 209, 205, 255), new Color32(0, 200, 130, 255) };
    public Color randomColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (changeColor)
        {
            LaserColorChange();
            
        }
        
        
    }


    void LaserColorChange()
    {
        randomColor = LaserColors[Random.Range(0, LaserColors.Length)];

        for (int i = 0; i < LaserBeams.Length; i++)
        {
            LaserBeams[i].GetComponent<LineRenderer>().SetColors(randomColor, randomColor);


            changeColor = false;
            StartCoroutine(Timerelement());
           
            
        }   
        

        

    }


    IEnumerator Timerelement()
    {
        yield return new WaitForSeconds(30* Time.deltaTime);
        changeColor = true;
    }
}
