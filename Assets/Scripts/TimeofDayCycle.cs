using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeofDayCycle : MonoBehaviour
{
    PostProcessVolume m_Volume;
    // Vignette m_Vignette;
    ColorGrading m_ColorGrading;

    private float cycleTime = 0;
    public float timeSpeedMultiplier;

    public GameObject directionalLight;

    [DisplayName("LiftDay")]
    [Trackball(TrackballAttribute.Mode.Lift)]
    public Vector4Parameter liftDay;

    [DisplayName("LiftNight")]
    [Trackball(TrackballAttribute.Mode.Lift)]
    public Vector4Parameter liftNight;

    [DisplayName("GammaDay")]
    [Trackball(TrackballAttribute.Mode.Gamma)]
    public Vector4Parameter gammaDay;

    [DisplayName("GammaNight")]
    [Trackball(TrackballAttribute.Mode.Gamma)]
    public Vector4Parameter gammaNight;

    [DisplayName("GainDay")]
    [Trackball(TrackballAttribute.Mode.Gain)]
    public Vector4Parameter gainDay;

    [DisplayName("GainNight")]
    [Trackball(TrackballAttribute.Mode.Gain)]
    public Vector4Parameter gainNight;


    void Start()
    {
        directionalLight.GetComponent<Transform>();
        directionalLight.transform.rotation = new Quaternion(0, 0, 0, 0);


        m_ColorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        m_ColorGrading.enabled.Override(true);
        m_ColorGrading.lift.Override(liftDay);
        m_ColorGrading.gamma.Override(gammaDay);
        m_ColorGrading.gain.Override(gainDay);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_ColorGrading);
    }

    void Update()
    {
        cycleTime = cycleTime + Time.deltaTime;

        float timeChange = Mathf.Sin(cycleTime * timeSpeedMultiplier);
        float normalizedFloat;

        normalizedFloat = (timeChange - (-1)) / (1 - (-1));
        timeChange = Mathf.Clamp(timeChange, -1, 1);
        normalizedFloat = Mathf.Clamp(normalizedFloat, 0, 1);

        m_ColorGrading.lift.value = Vector4.Lerp(liftDay, liftNight, normalizedFloat);
        m_ColorGrading.gamma.value = Vector4.Lerp(gammaDay, gammaNight, normalizedFloat);
        m_ColorGrading.gain.value = Vector4.Lerp(gainDay, gainNight, normalizedFloat);

        directionalLight.transform.Rotate(normalizedFloat, 0, 0);
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}