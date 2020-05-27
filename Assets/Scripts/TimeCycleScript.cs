using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

[ExecuteAlways]
public class TimeCycleScript : MonoBehaviour
{
    // Scene References.
    [Header("Time of Day Variables")]
    [SerializeField] private Light DirectionalLight;
    [GradientUsage(true)] public Gradient AmbientColor;
    [GradientUsage(true)] public Gradient DirectionalColor;
    [GradientUsage(true)] public Gradient FogColor;
    // Variables.
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    // Cloud related variables.
    [Header("Clouds Variables")]
    [Range(0, 0.25f)]
    public float windSpeed = 1.0f;
    public float cloudAlpha;
    public AnimationCurve cloudVisiblityCurve;
    public GameObject Clouds_low;
    private MeshRenderer CloudRenderer_low;
    public GameObject Clouds_high;
    private MeshRenderer CloudRenderer_high;

    // Star related variables.
    [Header("Stars Variables")]
    public GameObject StarsWhite;
    private ParticleSystem StarsWhiteParticle;
    public GameObject StarsBlue;
    private ParticleSystem StarsBlueParticle;
    public GameObject StarsViolet;
    private ParticleSystem StarsVioletParticle;
    
    // Audio related variables.
    public GameObject AudioController;
    private AudioSource nightSfx;

    private void Start()
    {
        // Initialize components.
        CloudRenderer_low = Clouds_low.GetComponent<MeshRenderer>();
        CloudRenderer_high = Clouds_high.GetComponent<MeshRenderer>();
        StarsWhiteParticle = StarsWhite.GetComponent<ParticleSystem>();
        StarsBlueParticle = StarsBlue.GetComponent<ParticleSystem>();
        StarsVioletParticle = StarsViolet.GetComponent<ParticleSystem>();
        nightSfx = AudioController.GetComponent<AudioSource>();

        // CLoud colors.
        CloudRenderer_low.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, cloudAlpha));
        CloudRenderer_high.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, cloudAlpha));

        // Cloud visibilty.
        cloudAlpha = 0f;
    }

    private void Update()
    {

        // Rotate clouds every frame.
        Clouds_low.transform.Rotate(0f, windSpeed / 2, 0f);
        Clouds_high.transform.Rotate(0f, windSpeed, 0f);

        // Set cloud alpha dependant on time of day.
        cloudAlpha = cloudVisiblityCurve.Evaluate(Time.time);

        CloudRenderer_low.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, cloudAlpha));
        CloudRenderer_high.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, cloudAlpha));

        // Day time stars removal.
        if (TimeOfDay > 6f || 18f > TimeOfDay)
        {
            StarsWhiteParticle.Stop();
            StarsBlueParticle.Stop();
            StarsVioletParticle.Stop();
        }

        // Sound effects during night time.
        nightSfx.volume = Mathf.Lerp(0.0f, 0.5f, (((TimeOfDay - 12) % 24f + 24f) % 24f) / 24f);

        if (nightSfx.volume > 0.25f)
        {
            nightSfx.volume = 1f - nightSfx.volume;
        }
        // (x%m + m)%m;

        // Stars during night time.
        if (TimeOfDay < 6f || 18f < TimeOfDay)
        {
            StarsWhiteParticle.Play();
            StarsBlueParticle.Play();
            StarsVioletParticle.Play();
        }

        if (Application.isPlaying)
        {
            // Replace with a reference to the game time.
            TimeOfDay += Time.deltaTime / 3f;
            TimeOfDay %= 24; // Modulus to ensure always between 0-24.
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        // Set ambient and fog.
        RenderSettings.ambientLight = AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = FogColor.Evaluate(timePercent);

        // If the directional light is set then rotate and set it's color, the rotation is rarely used because it casts tall shadows unless the value is clamped.
        if (DirectionalLight != null)
        {
            DirectionalLight.color = DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    // Try to find a directional light to use if one have not been set.
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        // Search for lighting tab sun.
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
    }
}