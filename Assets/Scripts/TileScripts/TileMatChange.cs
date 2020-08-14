using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMatChange : MonoBehaviour
{
    public bool changeColor = true;
    public Material[] randomTileMat;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (changeColor)
        {
            for (int i = 0; i < 24; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.GetComponent<MeshRenderer>().material = randomTileMat[Random.Range(0, randomTileMat.Length)];
                changeColor = false;
                StartCoroutine(Timer());
            }
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(16 * Time.deltaTime);
        changeColor = true;
    }
}
