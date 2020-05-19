using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[ExecuteAlways]
public class TimeCycleScript : MonoBehaviour
{
    //Scene References
    [Header("Time of Day Variables")]
    [SerializeField] private Light DirectionalLight;
    [GradientUsage(true)] public Gradient AmbientColor;
    [GradientUsage(true)] public Gradient DirectionalColor;
    [GradientUsage(true)] public Gradient FogColor;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [Header("Clouds Variables")]
    [Range(0, 0.25f)]
    public float windSpeed = 1.0f;
    public AnimationCurve cloudVisiblityCurve;
    public GameObject Clouds_low;
    private MeshRenderer CloudRenderer_low;
    public GameObject Clouds_high;
    private MeshRenderer CloudRenderer_high;
    [Header("Stars Variables")]
    public GameObject StarsWhite;
    private ParticleSystem StarsWhiteParticle;
    public GameObject StarsBlue;
    private ParticleSystem StarsBlueParticle;
    public GameObject StarsViolet;
    private ParticleSystem StarsVioletParticle;
    public float cloudAlpha = 0.0f;
    public float starsAlpha = 0.0f;
    public GameObject AudioController;
    private AudioSource nightSfx;




    private void Start()
    {
        CloudRenderer_low = Clouds_low.GetComponent<MeshRenderer>();
        CloudRenderer_high = Clouds_high.GetComponent<MeshRenderer>();
        StarsWhiteParticle = StarsWhite.GetComponent<ParticleSystem>();
        StarsBlueParticle = StarsBlue.GetComponent<ParticleSystem>();
        StarsVioletParticle = StarsViolet.GetComponent<ParticleSystem>();
        nightSfx = AudioController.GetComponent<AudioSource>();

    }

    private void Update()
    {

        //rotate clouds every frame
        Clouds_low.transform.Rotate(0f, windSpeed / 2, 0f);
        Clouds_high.transform.Rotate(0f, windSpeed, 0f);

        //set cloud alpha dependant on time of day
        cloudAlpha = cloudVisiblityCurve.Evaluate(Time.time);

        CloudRenderer_low.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, cloudAlpha));
        CloudRenderer_high.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, cloudAlpha));

        if (TimeOfDay > 6f || 18f > TimeOfDay) //Day
        {
            StarsWhiteParticle.Stop();
            StarsBlueParticle.Stop();
            StarsVioletParticle.Stop();
            nightSfx.volume = 0.0f;
        }

        if (TimeOfDay < 6f || 18f < TimeOfDay) //Night
        {
            StarsWhiteParticle.Play();
            StarsBlueParticle.Play();
            StarsVioletParticle.Play();
            nightSfx.volume = 0.1f;
        }
        // print(TimeOfDay / 24f);

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
    }
}