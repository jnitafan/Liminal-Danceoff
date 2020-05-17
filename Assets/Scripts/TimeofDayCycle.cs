using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeofDayCycle : MonoBehaviour
{
    PostProcessVolume m_Volume;
    // Vignette m_Vignette;
    ColorGrading m_ColorGrading;

    public int cycleTime;

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
    [DisplayName("GainDay")]
    [Trackball(TrackballAttribute.Mode.Gain)]
    public Vector4Parameter gainNight;

    void Start()
    {

        m_ColorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        m_ColorGrading.enabled.Override(true);
        m_ColorGrading.lift.Override(liftDay);
        m_ColorGrading.gamma.Override(gammaDay);
        m_ColorGrading.gain.Override(gainDay);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_ColorGrading);
    }

    void Update()
    {
        m_ColorGrading.lift.value = Vector4.Lerp(liftDay, liftNight, Mathf.Sin(cycleTime * Time.realtimeSinceStartup));
        m_ColorGrading.gamma.value = Vector4.Lerp(gammaDay, gammaNight, Mathf.Sin(cycleTime * Time.realtimeSinceStartup));
        m_ColorGrading.gain.value = Vector4.Lerp(gainDay, gainNight, Mathf.Sin(cycleTime * Time.realtimeSinceStartup));
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}