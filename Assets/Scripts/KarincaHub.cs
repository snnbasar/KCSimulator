using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class KarincaHub : MonoBehaviour
{
    [Header("Hub Ozellikleri")]
    [SerializeField] private bool godMode;

    [SerializeField] private int maxkarincaSayisi;
    [SerializeField] private int _karincaSayisi;
    public int karincaSayisi { get { return _karincaSayisi; } set
        {
            _karincaSayisi = value;
            UpdateMaxAclik();
            CheckForHubRegister();
            AclikHiziArttir();
        } }



    [SerializeField] private float maxAclik;
    [SerializeField] private float _currentAclik;
    public float currentAclik { get { return _currentAclik; } set {
            _currentAclik = value;
            CheckForDead();
        } }



    [SerializeField] private float AclikAzalmaHizi;
    [SerializeField] private float randomPislikTime;


    [SerializeField] private int maxPislik;
    [SerializeField] private int _currentPislik;
    public int currentPislik
    {
        get { return _currentPislik; }
        set
        {
            _currentPislik = value;
            
        }
    }
    public bool IsInYayinRoom;

    [Header("Karinca Ozellikleri")]
    [SerializeField] private float timeBetweenKarincasToGoPosition;
    [SerializeField] private float karincaSpeed;
    [SerializeField, Range(0, 1)] private float karincaProteinDegeri = 0.3f;
    [SerializeField] private float karincaDestroyTime;


    public List<KarincaSingle> karincalar = new List<KarincaSingle>();

    [SerializeField] private GameObject karincaPrefab;
    [SerializeField] private GameObject pislikPrefab;
    public List<PislikSingle> karincaPislikler = new List<PislikSingle>();

    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;



    bool registered;
    float oldAclikAzalmaHizi;

    bool isInspecting;






    void Start()
    {
        currentAclik = maxAclik;
        oldAclikAzalmaHizi = AclikAzalmaHizi;
        RegisterCurrentKarincaAndStartAntsToGoPlaces();
        CheckForHubRegister();
        RandomPislikInvoke();
    }

    

    private void Update()
    {
        if (godMode)
            return;
        currentAclik -= AclikAzalmaHizi * Time.deltaTime;
    }


    private void UpdateMaxAclik()
    {
        int pay = 15;
        if (karincaSayisi > pay && maxAclik < 2)
            maxAclik++;
        else if (karincaSayisi > pay * 2 && maxAclik < 3)
            maxAclik++;
        else if (karincaSayisi > pay * 3 && maxAclik < 4)
            maxAclik++;
    }





    #region Dying

    private void CheckForDead()
    {
        if(currentAclik <= 0)
        {
            KillRandomKarinca();
            if (karincaSayisi <= 0)
                return;
            SetCurrentAclik(karincaProteinDegeri);
        }
    }

    private void KillRandomKarinca()
    {
        karincalar[Random.Range(0, karincalar.Count)].KillMe();
    }
    #endregion







    #region Aclik
    private void AclikHiziArttir()
    {
        AclikAzalmaHizi = oldAclikAzalmaHizi * karincaSayisi;
    }


    public async void SetCurrentAclik(float protein)
    {
        float aas = 0;
        while(aas < protein)
        {
            float bss = protein * 0.002f;
            currentAclik += bss;
            aas += bss;
            if (currentAclik >= maxAclik)
                break;
            await Task.Yield();
        }
    }

    public void KarincaBesle(Besin besin)
    {
        besin.transform.parent = this.transform;
        besin.transform.localPosition = -Vector3.up * 0.4f;
        besin.transform.localRotation = Quaternion.identity;

        besin.GetMeReadyForEatingInKarincaHub(this);

        Vector3 goPos = new Vector3(besin.transform.localPosition.x, -0.3f, besin.transform.localPosition.z);
        foreach (KarincaSingle karinca in karincalar)
        {
            SendKarinca(karinca, goPos);
        }
    }
    #endregion






    private void RegisterCurrentKarincaAndStartAntsToGoPlaces()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Karinca"))
            {
                RegisterMe(transform.GetChild(i).GetComponent<KarincaSingle>());
            }
        }
        if (karincaSayisi <= 0)
            return;
        InvokeRepeating("SendRandomKarinca", 1, timeBetweenKarincasToGoPosition);
        KarincalariDagit();
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(pos2.localPosition.x, pos1.localPosition.x);
        float y = Random.Range(pos2.localPosition.y, pos1.localPosition.y);
        float z = Random.Range(pos2.localPosition.z, pos1.localPosition.z);
        Vector3 rndmpos = new Vector3(x, y, z);
        return rndmpos;
    }






    #region RegisterSeyleri
    public void RegisterMe(KarincaSingle karinca)
    {
        karincalar.Add(karinca);
        karinca.karincaHub = this;
        karincaSayisi++;
        karinca.karincaSpeed = karincaSpeed;
        karinca.karincaDestroyTime = karincaDestroyTime;
    }

    private void CheckForHubRegister()
    {
        if (!registered && karincaSayisi > 0)
            HubRegister();
        if (registered && karincaSayisi <= 0)
            HubUnRegister();
    }
    private void HubRegister()
    {
        GameManager.instance.RegisterKarincaHub(this);
        registered = true;
    }


    public void UnRegisterMe(KarincaSingle karinca)
    {
        karincaSayisi--;
        karincalar.Remove(karinca);
    }
    private void HubUnRegister()
    {
        GameManager.instance.UnRegisterKarincaHub(this);
        registered = false;
    }






    #endregion

    private void SendRandomKarinca()
    {
        if (karincaSayisi <= 0)
            return;
        Vector3 goPos = GetRandomPos();
        KarincaSingle rndmKarinca = karincalar[Random.Range(0, karincalar.Count)];
        SendKarinca(rndmKarinca, goPos);
    }

    private void SendKarinca(KarincaSingle karinca, Vector3 goPos)
    {
        karinca.SendMeSomewhere(goPos);
    }

    public void KarincalariDagit()
    {
        foreach (KarincaSingle karinca in karincalar)
        {
            Vector3 goPos = GetRandomPos();
            SendKarinca(karinca, goPos);
        }
    }




    public void KarincaCikar() // Calls on GameManager
    {
        if (karincaSayisi == maxkarincaSayisi)
            return;
        
        GameObject newKarinca = Instantiate(karincaPrefab, pos2.position, Quaternion.identity);
        newKarinca.transform.parent = this.transform;
        KarincaSingle karinca = newKarinca.GetComponent<KarincaSingle>();
        RegisterMe(karinca);
        if(IsInYayinRoom)
            Economy.instance.ParaArttir(Economy.instance.SingleKarincaKazanc, "Karýnca Doðdu");
        //SendKarinca(karinca, GetRandomPos());
    }







    #region Pislik
    private void RandomPislikInvoke()
    {
        InvokeRepeating("SpawnRandomPislik", randomPislikTime, randomPislikTime);
    }
    private void SpawnRandomPislik()
    {
        if (currentPislik == maxPislik)
            return;
        SpawnPislik(GetRandomPos());
    }
    public void SpawnPislik(Vector3 pos)
    {
        if (isInspecting)
            return;
        GameObject pislik = Instantiate(pislikPrefab);
        /*if (isInspecting)
            pislik.transform.position = transform.TransformPoint(pos);
        else
        {
            pislik.transform.parent = this.transform;
            pislik.transform.localPosition = pos;
        }*/

        pislik.transform.parent = this.transform;
        pislik.transform.localPosition = pos;
        pislik.transform.rotation = Random.rotation;
        PislikSingle pislikSingle = pislik.GetComponent<PislikSingle>();
        pislikSingle.RegisterKarincaHub(this);
    }
    #endregion






    public void ChangeToInspectionMode(bool sts)
    {
        switch (sts)
        {
            case true:
                GetComponent<Collider>().enabled = false;
                break;
            case false:
                GetComponent<Collider>().enabled = true;
                break;
        }

        isInspecting = sts;
    }

    public void SetPislikParents(bool prnt)
    {
        switch (prnt)
        {
            case true:
                foreach (PislikSingle pislik in karincaPislikler)
                {
                    pislik.transform.parent = this.transform;
                }
                break;
            case false:
                foreach (PislikSingle pislik in karincaPislikler)
                {
                    pislik.transform.parent = null;
                }
                break;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YayinRoom"))
        {
            IsInYayinRoom = true;
            YayinRoom yayinRoom = other.GetComponent<YayinRoom>();
            yayinRoom.RegisterKarincaHub(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("YayinRoom"))
        {
            IsInYayinRoom = false;
            YayinRoom yayinRoom = other.GetComponent<YayinRoom>();
            yayinRoom.UnRegisterKarincaHub(this);
        }
    }
}
