using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{

    //    __ _       _     _   _                             _       _     _  _     _ _           _             _ 
    //   / /(_) __ _| |__ | |_(_)_ __   __ _   ___  ___ _ __(_)_ __ | |_  | || |   | (_)_ __ ___ (_)_ __   __ _| |
    //  / / | |/ _` | '_ \| __| | '_ \ / _` | / __|/ __| '__| | '_ \| __| | || |_  | | | '_ ` _ \| | '_ \ / _` | |
    // / /__| | (_| | | | | |_| | | | | (_| | \__ \ (__| |  | | |_) | |_  |__   _| | | | | | | | | | | | | (_| | |
    // \____/_|\__, |_| |_|\__|_|_| |_|\__, | |___/\___|_|  |_| .__/ \__|    |_|   |_|_|_| |_| |_|_|_| |_|\__,_|_|
    //         |___/                   |___/                  |_|                                                 

    // Variable Declaration
    [Header("Main Components")]
    public bool isON = true;
    private float delaySync;
    public float BPM = 60f;
    private float[] curves = { 0, 0, 0, 1 }; //create float array with 4 elements. elements shown below
    public enum curveEnum : int
    {
        strobe,
        sine,
        square,
        constant
    }
    [Space(10)]
    [Header("Static Light Settings")]
    public GameObject danceTiles;
    public curveEnum danceTileLighting;
    private List<Renderer> danceTileArray = new List<Renderer>(); //Array of gameobjects inside dancetiles
    //unused materialpropertyblock instantiation
    //private MaterialPropertyBlock tilePropBlock;
    private List<Light> danceTileLightArray = new List<Light>();
    [Space(3)]
    public GameObject wallEmissions;
    public curveEnum wallEmissionsLighting;
    private List<Renderer> wallEmissionsArray = new List<Renderer>();
    [Space(3)]
    public Transform longLights;
    public curveEnum longLightLighting;
    private List<Renderer> FluroscentLightbulbs = new List<Renderer>();
    [Space(3)]
    public Transform lightBoxes;
    public curveEnum lightBoxesLighting;
    private List<Light> spotLights = new List<Light>();
    [Space(3)]
    public Transform laserMachines;
    public curveEnum laserMachineLighting;
    private List<LineRenderer> Lazers = new List<LineRenderer>();
    [Space(3)]
    public MeshRenderer LiminalSign;
    public curveEnum LiminalSignLighting;
    [Space(3)]
    public MeshRenderer CeilingLights;
    public curveEnum CeilingLightsLighting;
    [Space(3)]
    private List<int> nextLightPerLightIndex = new List<int>(); //keep in memory the next color chosen for each single light
    [Space(5)]
    [ColorUsage(true, true)]
    public Color BaseColor;

    [Space(10)]
    [Header("Colours")]
    [ColorUsage(true, true)]
    public Color[] lightColours = { };



    // Instantiate all Arrays with their respective GameObject
    void Start()
    {



        // Count the number of children inside the object DanceTiles, and iterate through them
        foreach (Renderer tileRenderer in danceTiles.GetComponentsInChildren(typeof(Renderer)))
        {
            //Assign each dance tile renderer (the component responsible for color) in the array, and fill tilearray memory with default colours
            danceTileArray.Add(tileRenderer);
            nextLightPerLightIndex.Add(0);
        }

        // Unused materialpropertyblock lines
        // tilePropBlock = new MaterialPropertyBlock();
        // danceTileArray[0].GetPropertyBlock(tilePropBlock);
        // tilePropBlock.Clear();


        foreach (Light tileLight in danceTiles.GetComponentsInChildren(typeof(Light)))
        {
            danceTileLightArray.Add(tileLight);
        }

        foreach (Renderer tileRenderer in wallEmissions.GetComponentsInChildren(typeof(Renderer)))
        {
            wallEmissionsArray.Add(tileRenderer);
        }
        //add all the lightbulbs in the array
        foreach (Light Lightbulb in lightBoxes.GetComponentsInChildren(typeof(Light)))
        {
            spotLights.Add(Lightbulb);
        }

        //add all the lazerbeam components
        foreach (LineRenderer lazer in laserMachines.GetComponentsInChildren(typeof(LineRenderer)))
        {
            Lazers.Add(lazer);
        }

        //This code is quite a bit messy, but it only runs once so YOLO lmao
        foreach (Transform LongLight in longLights)
        {
            //Get the child of the child inside the LongLEDs -> LongLight Prefab
            foreach (Transform FluroscentLightbulb in LongLight)
            {
                foreach (Renderer MeshRenderer in FluroscentLightbulb.GetComponentsInChildren(typeof(Renderer)))
                {
                    FluroscentLightbulbs.Add(MeshRenderer);
                }
            }
        }

        // Run the Update Colours function every time the beat happens in seconds. 60/BPM = the amount of time in seconds a beat occurs.
        StartCoroutine(runPerBeat());
        StartCoroutine(runAMAP());
    }

    //Run perframe
    void Update()
    {
        // Lerp the BPM into a percentage, setting the value to 1 every beat, and decreasing as a percentage of time until the next beat will happen
        curves[0] = Mathf.InverseLerp(-1, 1, Mathf.Cos(((Time.time * Mathf.PI) * (BPM / 60f)) % Mathf.PI));
        curves[1] = Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time * (BPM / 60f)));
        curves[2] = Mathf.InverseLerp(-1, 1, Mathf.Sign(Mathf.Sin(2f * Time.time * Mathf.PI)));
        //curves[3] = 1; //moved this line to the start(); function because its a constant.
        delaySync = Time.time % (60 / BPM);
    }

    // Simple twoliner that randomizes each single light color depending on what lights there are.
    // functions that run on a coroutine (much less resource intensive than loops on the update and can be paused)
    IEnumerator runPerBeat()
    {
        while (isON)
        {
            for (int i = 0; i < danceTileArray.Count; i++)
            {
                nextLightPerLightIndex[i] = Random.Range(0, lightColours.Length);
            }
            // There is a bug here, the coroutine is not synced with the time.time function, if the application lags out then this will be desynced. (fix iteration 2 mabye.)
            yield return new WaitForSecondsRealtime((60f / BPM) - delaySync);
        }
        yield return null;
    }

    // AMAP = run AS MUCH AS POSSIBLE MAXIMUM POWAR (lol its in a coroutine so its more like as much as you want, no pressure computer :) smiley face)  
    IEnumerator runAMAP()
    {


        while (isON)
        {

            // yes, i know. hard coded values. ill rework this entire area later if i have got the time

            // TODO: rewrite this section with material property blocks
            danceTileLightArray[0].color = Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[1]], curves[(int)danceTileLighting]);
            danceTileLightArray[1].color = Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[16]], curves[(int)danceTileLighting]);
            danceTileLightArray[2].color = Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[13]], curves[(int)danceTileLighting]);

            for (int i = 0; i < danceTileArray.Count; i++)
            {

                // since LWRP doesnt really like materialpropertyblocks, since it already conflicts with the LWRP SRP Batching protocols.
                //tilePropBlock.SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[i]], curves[(int)danceTileLighting]));
                //danceTileArray[i].SetPropertyBlock(tilePropBlock);

                // change the tile color depending on the 
                danceTileArray[i].material.SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[i]], curves[(int)danceTileLighting]));

                //yeah i am not running two for loops in a timeperiod of a frame/millisecond, if checks are way more optimal
                if (i < FluroscentLightbulbs.Count)
                {
                    FluroscentLightbulbs[i].material.SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[i]], curves[(int)longLightLighting]));

                    if (i < spotLights.Count)
                    {
                        spotLights[i].color = Color.Lerp(Color.black, lightColours[nextLightPerLightIndex[i]], curves[(int)lightBoxesLighting]);

                        if (i < wallEmissionsArray.Count)
                        {
                            wallEmissionsArray[i].material.SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[4]], curves[(int)wallEmissionsLighting]));
                        }
                    }

                    if (i < Lazers.Count)
                    {
                        Lazers[i].startColor = Color.Lerp(Color.black, lightColours[nextLightPerLightIndex[3]], curves[(int)laserMachineLighting]);
                        Lazers[i].endColor = Color.Lerp(Color.black, lightColours[nextLightPerLightIndex[3]], curves[(int)laserMachineLighting]);
                    }
                }
            }

            LiminalSign.material.SetColor("_EmissionColor", Color.Lerp(Color.black, lightColours[nextLightPerLightIndex[2]], curves[(int)LiminalSignLighting]));
            CeilingLights.materials[0].SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[0]], curves[(int)CeilingLightsLighting]));
            CeilingLights.materials[1].SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[1]], curves[(int)CeilingLightsLighting]));

            yield return new WaitForSecondsRealtime(0 - delaySync);
        }
    }

}
