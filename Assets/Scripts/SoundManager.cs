using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Soundlar
{
    PickUp,
    MoneyIn,
    MoneyOut,
    DoorOpen,
    DoorClose,
    DrawerOpen,
    DrawerClose,
    CabinetOpen,
    CabinetClose, 
    Eating

}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> sounds = new List<AudioSource>();
    public List<AudioSource> musics = new List<AudioSource>();
    private Soundlar soundlar;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        /*for (int i = 0; i < transform.childCount; i++)
        {
            sounds.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }

        Transform musicObject = transform.GetChild(0).GetChild(0);
        for (int i = 0; i < musicObject.childCount; i++)
        {
            musics.Add(musicObject.GetChild(i).GetComponent<AudioSource>());
        }*/
        PlayRandomMusic();
    }

    private void PlayRandomMusic()
    {
        if (musics.Count == 0)
            return;
        int rndmMusic;
        rndmMusic = Random.Range(0, musics.Count);
        musics[rndmMusic].Play();
            
    }

   

    public void PlaySound(Soundlar soundfx)
    {
        soundlar = soundfx;
        SoundsIndexes();
    }

    private void SoundsIndexes()
    {
        switch (soundlar)
        {
            case Soundlar.PickUp:
                sounds[0].Play();
                break;
            case Soundlar.MoneyIn:
                sounds[1].Play();
                break;
            case Soundlar.MoneyOut:
                sounds[2].Play();
                break;
            case Soundlar.DoorOpen:
                sounds[3].Play();
                break;
            case Soundlar.DoorClose:
                sounds[4].Play();
                break;
            case Soundlar.DrawerOpen:
                sounds[5].Play();
                break;
            case Soundlar.DrawerClose:
                sounds[6].Play();
                break;
            case Soundlar.CabinetOpen:
                sounds[7].Play();
                break;
            case Soundlar.CabinetClose:
                sounds[8].Play();
                break;
            case Soundlar.Eating:
                sounds[9].Play();
                break;
            default:
                break;
        }
    }

}
