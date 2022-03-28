using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class YemekManager : MonoBehaviour
{
    public static YemekManager instance;
    public float aclikDusmeHizi;

    public TextMeshProUGUI yemekText;
    public int maxAclik;
    private float _aclik;
    public float aclik { get { return _aclik; } set 
        {
            if (value <= 0)
                _aclik = 0;
            else
                _aclik = value;
            if (_aclik >= maxAclik)
                _aclik = maxAclik;
            yemekText.text = ((int)_aclik).ToString();
        } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        aclik = maxAclik;
    }
    private void Update()
    {
        if (aclik <= 0)
            return;
        aclik -= aclikDusmeHizi * Time.deltaTime;
    }






    public async void YemekYedim(float besin)
    {
        SoundManager.instance.PlaySound(Soundlar.Eating);
        float aas = 0;
        while (aas < besin)
        {
            float bss = besin * 0.002f;
            aclik += bss;
            aas += bss;
            if (aclik >= maxAclik)
                break;
            await Task.Yield();
        }
    }
}
