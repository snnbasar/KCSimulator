using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float inspectSensitivity = 20f;

    [SerializeField, Range(0, 24)] private int birGundeCikacakKarincaSayisi;

    public List<KarincaHub> karincaHublar = new List<KarincaHub>();


    public bool IsInspecting;
    public bool IsMarketOpen;
    public bool IsInventoryOpen;

    public int currentTime;
    public int yayinParasiVerilecekSaat;

    public Transform ItemSpawnPoint;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CanControlPlayer(true);
    }

    private void Update()
    {

        currentTime = (int)LightingManager.instance.TimeOfDay;
        CheckForKarincaDogurma();
        CheckYayinParasi();
    }


    public void CanUseCursor(bool status)
    {
        
        switch (status)
        {
            case true:
                if (IsInspecting || IsInventoryOpen || IsMarketOpen)
                {

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                break;
            case false:
                if (!IsInspecting && !IsInventoryOpen && !IsMarketOpen)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                break;
        }
    }

    public void CanControlPlayer(bool status)
    {
        switch (status)
        {
            case true:
                if (!IsInspecting && !IsInventoryOpen && !IsMarketOpen)
                {
                    KarakterController.instance.canMove = true;
                    KarakterController.instance.ResetAxis();
                    FPSCAMController.instance.canMove = true;
                }
                break;
            case false:
                if (IsInspecting || IsInventoryOpen || IsMarketOpen)
                {
                    KarakterController.instance.canMove = false;
                    KarakterController.instance.ResetAxis();
                    FPSCAMController.instance.canMove = false;
                }
                break;
        }
    }


    public void RegisterKarincaHub(KarincaHub hub)
    {
        karincaHublar.Add(hub);
    }

    public void UnRegisterKarincaHub(KarincaHub hub)
    {
        karincaHublar.Remove(hub);
    }



    bool gunlukDogum;
    float oldTime;
    private void CheckForKarincaDogurma()
    {


        float dogurmaSaati = 24 / birGundeCikacakKarincaSayisi;

        if (!gunlukDogum && (currentTime % dogurmaSaati) == 0)
        {
            KarincaDogur();
            gunlukDogum = true;
            oldTime = currentTime;
        }
        if (oldTime != currentTime)
            gunlukDogum = false;
    }
    public void KarincaDogur() 
    {
        foreach (KarincaHub hub in karincaHublar)
        {
            hub.KarincaCikar();
        }

    }



    bool yayinParasiVerildi;

    float oldTimeForYayinciParasi;
    private void CheckYayinParasi()
    {
        if(!yayinParasiVerildi && currentTime == yayinParasiVerilecekSaat)
        {
            GiveYayýnParasý();
            yayinParasiVerildi = true;
            oldTimeForYayinciParasi = currentTime;
        }
        if (oldTimeForYayinciParasi != currentTime)
            yayinParasiVerildi = false;
    }
    public void GiveYayýnParasý()
    {
        int verilecekPara = 0;
        foreach (KarincaHub hub in karincaHublar)
        {
            if(hub.IsInYayinRoom)
                verilecekPara += (hub.karincaSayisi * Economy.instance.HubKazancCarpani) - (int)(hub.currentPislik * Economy.instance.KarincaPislikCarpani);
        }

        Economy.instance.ParaArttir(verilecekPara, "Yayýn Gelirleri");
        print("Yayýn Parasý: " + verilecekPara);
    }



    public async void SatinAlinaniSpawnla(GameObject item)
    {
        await Task.Delay(2* 1000);
        Instantiate(item, ItemSpawnPoint.position, Quaternion.identity);
    }


}
