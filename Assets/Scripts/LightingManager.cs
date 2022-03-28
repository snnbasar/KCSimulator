using TMPro;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    public static LightingManager instance;

    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    [SerializeField, Range(0, 1)] private float daySpeed;

    private int _saat;
    public int saat { get { return _saat; } set
        {
            _saat = value;
        } }

    public TextMeshProUGUI saatText;

    float dakika;
    string saatString;
    string dakikaString;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime * daySpeed;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            saat = (int)TimeOfDay;
            dakika = Mathf.Floor(((TimeOfDay - saat) * 100) / 100 * 60);
            if (saat < 10)
                saatString = "0" + saat;
            else
                saatString = saat.ToString();
            if (dakika < 10)
                dakikaString = "0" + dakika;
            else
                dakikaString = dakika.ToString();
            saatText.text = saatString + ":" + dakikaString;
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
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        /*float exposure;
        if(TimeOfDay > 6 && TimeOfDay < 20)
        {
            if (TimeOfDay < 12)
            {
                exposure = timePercent * 2;
                RenderSettings.skybox.SetFloat("_Exposure", exposure);
            }
            else
            {
                exposure = (1 - timePercent) * 2;
                RenderSettings.skybox.SetFloat("_Exposure", exposure);
            }
        }*/
        //RenderSettings.skybox.SetFloat("_Rotation", timePercent * 360);
        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

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
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}