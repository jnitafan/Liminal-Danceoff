using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMatChange : MonoBehaviour
{
    private float time;
    public bool changeColor = true;
    public Material[] randomTileMat;
    public Material[] randomTileMatMuted;
    public GameObject[] FluroscentLightbulb;
    public GameObject[] SpotLightLightBox;

    [ColorUsageAttribute(true, true)]
    public Color[] randomLightColor = { new Color(174, 71, 217), new Color(34, 169, 238), new Color(0, 200, 130), new Color(174, 71, 217) };
    public float speed = 100f;
    Light LightBoxLight;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (changeColor)
        {
            TileColourChange();
            FluroscentLightbulbColourChange();
            SpotLightLightBoxColourChange();
        }
    }

    void TileColourChange()
    {
        for (int i = 0; i < 40; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponent<MeshRenderer>().material = randomTileMatMuted[Random.Range(0, randomTileMatMuted.Length)];
            changeColor = false;
            StartCoroutine(Timerelement());
        }
    }
    //Dancefloortiles color changing script (uses tile materials,now called light mat materials, located in tile mat materials)
    void FluroscentLightbulbColourChange()
    {
        for (int i = 0; i < FluroscentLightbulb.Length; i++)
        {
            FluroscentLightbulb[i].GetComponent<MeshRenderer>().material = randomTileMat[Random.Range(0, randomTileMat.Length)];
            changeColor = false;
            StartCoroutine(Timerelement());
        }
    }
    //FluroscentLightbulb color changing script (uses tile materials, now called light mat materials, located in tile mat materials)
    void SpotLightLightBoxColourChange()
    {
        /*LightBoxLight = SpotLightLightBox.GetComponent<Light>();
        time = Mathf.PingPong(Time.time *speed, 1);
        LightBoxLight.color = Color.Lerp(randomLightColor[Random.Range(0, randomLightColor.Length)], randomLightColor[Random.Range(0, randomLightColor.Length)], time);
        changeColor = false;
        StartCoroutine(Timerelement());*/

        for (int i = 0; i < SpotLightLightBox.Length; i++)
        {
            LightBoxLight = SpotLightLightBox[i].GetComponent<Light>();
            LightBoxLight.color = randomLightColor[Random.Range(0, randomLightColor.Length)];
            changeColor = false;
            StartCoroutine(Timerelement());
        }
    }
    //SpotLightLightBox color change script (uses tile materials, lerps between colour as spotlight rotates around the club)
    void LaserMachineColourChange()
    {

    }
    IEnumerator Timerelement()
    {
        yield return new WaitForSeconds(30 * Time.deltaTime);
        changeColor = true;
    }
    //Time to be changed and set with BPM of added audio to edit light according to the beat of the song playing.
}
