using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    public GameObject[] Clouds;
    private MeshRenderer CloudRenderer1;
    private MeshRenderer CloudRenderer2;
    public float alpha = 0.0f;


    private void Start()
    {
        CloudRenderer1 = Clouds[0].GetComponent<MeshRenderer>();
        CloudRenderer2 = Clouds[1].GetComponent<MeshRenderer>();
    }

    private void Update()
    {

        //rotate clouds every frame
        Clouds[0].transform.Rotate(0f, windSpeed / 2, 0f);
        Clouds[1].transform.Rotate(0f, windSpeed, 0f);

        //set cloud alpha dependant on time of day
        alpha = TimeOfDay / 24f;
        if (alpha > 0.5)
        {
            alpha = (1 - alpha) + 0.05f;
        }

        CloudRenderer1.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, alpha));
        CloudRenderer2.sharedMaterial.SetColor("_CloudColor", new Color(1, 1, 1, alpha));

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

        Clouds = GameObject.FindGameObjectsWithTag("Clouds");
    }
}