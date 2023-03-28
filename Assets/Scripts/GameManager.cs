using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioSource audio;
    public AudioClip PumpOn;
    public AudioClip PumpRunning;
    public AudioClip PumpOff;
    enum Impeller { Open, SemiOpen, Closed };
    public GameObject LiquidFlow1, LiquidFlow2, LiquidFlow3, LiquidFlow4;
    public TMP_Text impellerLabel;
    public TMP_Text ViscosityType;
    private float pressureVal;
    private float previousPressureVal;
    public PressureEvent pressureGauge;
    public bool hasFile = false;

    public GameObject ClosedImpeller, SemiOpenImpeller;

    private GameObject currImpeller;
    private bool isPumpRunning;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //viscositySlider.OnInteractionEnded.AddListener(delegate { ViscosityChangeCheck(); });
        currImpeller = ClosedImpeller;
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        
    }

    public void fileOnOff()
    {
        hasFile = !hasFile;
    }
    public void ImpellerSwap()
    {
        if (ClosedImpeller.activeInHierarchy)
        {
            ClosedImpeller.SetActive(false);
            SemiOpenImpeller.SetActive(true);
            currImpeller = SemiOpenImpeller;
            impellerLabel.text = "Semi-Open" + '\n' + "Impeller";
            if (isPumpRunning)
            {
                pressureGauge.UpdateDial(pressureVal -= 25f);
            }
        }
        else
        {
            ClosedImpeller.SetActive(true);
            SemiOpenImpeller.SetActive(false);
            currImpeller = ClosedImpeller;
            impellerLabel.text = "Closed Impeller";
            if (isPumpRunning)
            {
                pressureGauge.UpdateDial(pressureVal += 25f);
            }
        }
    }

    public void PumpTurnOn()
    {
        isPumpRunning = true;
        StartCoroutine(startPump());
    }

    public void PumpTurnOff()
    {
        isPumpRunning = false;
        audio.clip = PumpOff;
        audio.loop = false;
        audio.Play();
        LiquidFlow1.SetActive(false);
        LiquidFlow2.SetActive(false);
        LiquidFlow3.SetActive(false);
        LiquidFlow4.SetActive(false);
    }
    public void PumpAudioController()
    {
        isPumpRunning = !isPumpRunning;
        if (isPumpRunning)
            StartCoroutine(startPump());
        else {
            audio.clip = PumpOff;
            audio.loop = false;
            audio.Play();
            LiquidFlow1.SetActive(false);
            LiquidFlow2.SetActive(false);
        }

    }

    IEnumerator startPump()
    {
        pressureVal = 55f;
        audio.clip = PumpOn;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = PumpRunning;
        audio.loop = true;
        audio.Play();
        LiquidFlow1.SetActive(true);
        LiquidFlow2.SetActive(true);
        LiquidFlow3.SetActive(true);
        LiquidFlow4.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        if (isPumpRunning)
        {
            currImpeller.transform.Rotate(0, 500 * Time.deltaTime, 0);
        }

    }

    public void XrayMode()
    {
        Debug.Log("X-Ray Mode Enabled");
    }
}
