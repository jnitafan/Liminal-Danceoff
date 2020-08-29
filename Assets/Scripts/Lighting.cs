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
    public bool IsON = true;
    public float BPM = 60f;
    private float BPMPercentage; //BPM converted to a coundown percentage from 1 to 0 (ask jacob for an explanation or look at voidUpdate)

    [Space(10)]
    [Header("Static Light Settings")]
    public GameObject danceTiles;
    private List<Renderer> danceTileArray = new List<Renderer>(); //Array of gameobjects inside dancetiles
    public Transform LongLights;
    private List<Renderer> FluroscentLightbulbs = new List<Renderer>();
    public Transform LightBoxes;
    private List<Light> spotLights = new List<Light>();
    public Transform LaserMachines;
    private List<LineRenderer> Lazers = new List<LineRenderer>();
    public MeshRenderer LiminalSign;
    public MeshRenderer CeilingLights;
    private List<int> nextLightPerLightIndex = new List<int>(); //keep in memory the next color chosen for each single light
    [Space(5)]
    [ColorUsage(true, true)]
    public Color BaseColor;

    [Space(10)]
    [Header("Colours")]
    [ColorUsage(true, true)]
    public Color[] lightColours = {
    };


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

        //add all the lightbulbs in the array
        foreach (Light Lightbulb in LightBoxes.GetComponentsInChildren(typeof(Light)))
        {
            spotLights.Add(Lightbulb);
        }

        //add all the lazerbeam components
        foreach (LineRenderer lazer in LaserMachines.GetComponentsInChildren(typeof(LineRenderer)))
        {
            Lazers.Add(lazer);
        }

        //This code is quite a bit messy, but it only runs once so YOLO lmao
        foreach (Transform LongLight in LongLights)
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
    }

    //Run perframe
    void Update()
    {
        // Lerp the BPM into a percentage, setting the value to 1 every beat, and decreasing as a percentage of time until the next beat will happen
        BPMPercentage = Mathf.InverseLerp(-1, 1, Mathf.Cos(((Time.time * Mathf.PI) * (BPM / 60f)) % Mathf.PI));

        ChangeColors();

    }

    // This function changes all the colors of the attached gameobjects
    void ChangeColors()
    {
        for (int i = 0; i < danceTileArray.Count; i++)
        {
            // change the tile color depending on the 
            danceTileArray[i].material.SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[i]], BPMPercentage));

            //yeah i am not running two for loops in a timeperiod of a frame/millisecond
            if (i < FluroscentLightbulbs.Count)
            {
                FluroscentLightbulbs[i].material.SetColor("_EmissionColor", lightColours[nextLightPerLightIndex[i]]);
            }

            if (i < spotLights.Count)
            {
                spotLights[i].color = Color.Lerp(Color.black, lightColours[nextLightPerLightIndex[i]], BPMPercentage);
            }

            if (i < Lazers.Count)
            {
                Lazers[i].startColor = lightColours[nextLightPerLightIndex[0]];
                Lazers[i].endColor = lightColours[nextLightPerLightIndex[1]];
            }
        }

        // NOTE: the rest of the lighting color change code are in the function runPerBeat to save on performance (as it changes when the engine can handle, not forcing it per frame.)
        // I might optimize it later to have everything not tied to running on the framerate if i have time later (Iteration 2 Mabye, but for now this is good)
        // but if you want to have a go at it, try find a way to move the rest of this entire ChangeColours() Function inside the IEnumerator runPerBeat() Coroutine
        // I have no clue why adding a forloop inside a coroutine makes it fail to run whats inside the for loop. im a noob -____-

    }

    // Simple twoliner that randomizes each single light color depending on what lights there are.
    void randomizeColorMemory()
    {
        for (int i = 0; i < danceTileArray.Count; i++)
        {
            nextLightPerLightIndex[i] = Random.Range(0, lightColours.Length);
        }
    }

    //functions that run on a coroutine (much less resource intensive than loops on the update and can be paused)
    IEnumerator runPerBeat()
    {
        while (IsON)
        {

            // NOTE: (Continued from above) this was the rest of the code in ChangeColors() above. If we put everything inside changecolors() in this function, it will be hella optimized.
            LiminalSign.material.SetColor("_EmissionColor", lightColours[nextLightPerLightIndex[3]]);
            CeilingLights.materials[0].SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[0]], BPMPercentage));
            CeilingLights.materials[1].SetColor("_EmissionColor", Color.Lerp(BaseColor, lightColours[nextLightPerLightIndex[1]], BPMPercentage));

            randomizeColorMemory();

            // There is a bug here, the coroutine is not synced with the time.time function, if the application lags out then this will be desynced. (fix iteration 2 mabye.)
            yield return new WaitForSeconds(60f / BPM);

        }
    }
}
