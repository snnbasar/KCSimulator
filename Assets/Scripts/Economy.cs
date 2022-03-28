using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Economy : MonoBehaviour
{
    public static Economy instance;

    private int _paraEffectText;
    public int paraEffectText { get { return _paraEffectText; } set 
        {
            _paraEffectText = value;
            UpdateParaText();
        } }
    private int _para;
    public int para { get { return _para; } set
        {
            _para = value;
        } }

    [Header("Karýnca Para Ayarlarý")]
    public int HubKazancCarpani;
    public int SingleKarincaKazanc;
    public int SingleKarincaKayip;
    public float KarincaPislikCarpani;

    [Header("Effect")]
    [SerializeField] private TextMeshProUGUI paraText;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject paraEffectPrefab;
    [SerializeField] private float paraEffectDuration;
    [SerializeField] private Vector3 paraEffectPosition;
    [SerializeField] private Ease paraEffectEase;

    private void Awake()
    {
        instance = this;
        paraEffectText = para;
    }


    private void UpdateParaText()
    {
        paraText.text = paraEffectText.ToString() + " TL";
    }
    
    [ContextMenu("10TL Kazan")]
    public void ParaArttirTest()
    {
        ParaArttir(10, "Test");
    }
    [ContextMenu("10TL Kaybet")]
    public void ParaAzaltTest()
    {
        ParaAzalt(10, "Test");
    }



    public void ParaArttir(int value, string info)
    {
        para += value;
        SoundManager.instance.PlaySound(Soundlar.MoneyIn);
        GameObject paraEffect = Instantiate(paraEffectPrefab);

        paraEffect.transform.SetParent(canvas);
        RectTransform rectTransform = paraEffect.GetComponent<RectTransform>();
        rectTransform.position = paraEffectPosition;
        paraEffect.GetComponent<TextMeshProUGUI>().text = "+" + value.ToString() + " " + info;
        rectTransform.DOScale(Vector3.one * 0.7f, paraEffectDuration).From(Vector3.one).SetEase(paraEffectEase);
        rectTransform.DOLocalMove(paraText.transform.localPosition, paraEffectDuration).SetEase(paraEffectEase).OnComplete(() =>
        {
            paraEffectText += value;
            Destroy(paraEffect);
        });
    }


    public void ParaAzalt(int value, string info)
    {
        para -= value;
        SoundManager.instance.PlaySound(Soundlar.MoneyOut);
        GameObject paraEffect = Instantiate(paraEffectPrefab);

        paraEffect.transform.SetParent(canvas);
        RectTransform rectTransform = paraEffect.GetComponent<RectTransform>();
        rectTransform.position = paraText.transform.position;
        paraEffect.GetComponent<TextMeshProUGUI>().text = "-" + value.ToString() + " " + info;
        rectTransform.DOScale(Vector3.one * 0.7f, paraEffectDuration).From(Vector3.one).SetEase(paraEffectEase);
        rectTransform.DOMove(paraEffectPosition, paraEffectDuration).SetEase(paraEffectEase).OnComplete(() =>
        {
            paraEffectText -= value;
            Destroy(paraEffect);
        });
    }




}
