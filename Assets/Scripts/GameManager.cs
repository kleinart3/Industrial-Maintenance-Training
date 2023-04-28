using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject VertCorrectAlignScreen, VertIncorrectAlignScreen;
    public GameObject VertIncorrectLine, VertIncorrectSlice, VertCorrectLine, VertCorrectSlice;
    public GameObject BracketLaserVis1, BracketLaserVis2;
    public GameObject TabletScreen;
    public GameObject TabletText1, TabletText2, TabletText3;
    public GameObject[] XRayObjects;
    public GameObject Motor;
    public Material XRayMat;
    public Material originalMat;
    public Material HotMat;
    public AudioSource audio;
    public AudioClip PumpOn;
    public AudioClip PumpRunning;
    public AudioClip PumpOff;
    enum Impeller { Open, SemiOpen, Closed };
    //public GameObject[] ShaftRotateParts;
    public GameObject Shaft1;
    public GameObject Shaft2;
    public GameObject LiquidFlow1, LiquidFlow2, LiquidFlow3, LiquidFlow4;
    public TMP_Text impellerLabel;
    public TMP_Text ViscosityType;
    private float pressureVal;
    private float previousPressureVal;
    public PressureEvent pressureGauge;
    public bool hasFile = false;
    public bool isHot;

    public GameObject ClosedImpeller, SemiOpenImpeller;

    private GameObject currImpeller;
    private bool isPumpRunning;
    private bool isXRayMode = false;
    private bool isAligned;
    private bool bracket1, bracket2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currImpeller = ClosedImpeller;
        isHot = true;
        
    }
    public void BracketSnap1()
    {
        bracket1 = true;
        if (bracket2)
        {
            TabletScreen.SetActive(true);
            BracketLaserVis1.SetActive(true);
            BracketLaserVis2.SetActive(true);
            TabletText1.SetActive(true);
            TabletText2.SetActive(true);
            TabletText3.SetActive(true);
        }
    }

    public void BracketSnap2()
    {
        bracket2 = true;
        if (bracket1)
        {
            TabletScreen.SetActive(true);
            BracketLaserVis1.SetActive(true);
            BracketLaserVis2.SetActive(true);
            TabletText1.SetActive(true);
            TabletText2.SetActive(true);
            TabletText3.SetActive(true);
        }
    }

    public void LoseBracket()
    {
        TabletScreen.SetActive(false);
        BracketLaserVis1.SetActive(false);
        BracketLaserVis2.SetActive(false);
        TabletText1.SetActive(false);
        TabletText2.SetActive(false);
        TabletText3.SetActive(false);
    }
    public void ValveOpenClose()
    {
        
    }
    public void fileOnOff()
    {
        hasFile = !hasFile;
    }
    public void HasCooled()
    {
        isHot = false;
        if (isXRayMode)
        {
            Motor.GetComponent<MeshRenderer>().material = XRayMat;
        }
    }

    public void VertAlign()
    {
        isAligned = true;
    }

    public void SetAlignCanvas()
    {
        if (isAligned)
        {
            VertCorrectAlignScreen.SetActive(true);
            VertCorrectLine.SetActive(true);
            VertCorrectSlice.SetActive(true);
            VertIncorrectLine.SetActive(false);
            VertIncorrectSlice.SetActive(false);
        }
        else
        {
            VertIncorrectAlignScreen.SetActive(true);
            VertCorrectLine.SetActive(false);
            VertCorrectSlice.SetActive(false);
            VertIncorrectLine.SetActive(true);
            VertIncorrectSlice.SetActive(true);
        }
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
            Shaft1.transform.Rotate(500 * Time.deltaTime, 0, 0);
            Shaft2.transform.Rotate(0, 500 * Time.deltaTime, 0);
            //foreach (GameObject go in ShaftRotateParts)
            //{
            //    go.transform.Rotate(500 * Time.deltaTime, 0, 0);
            //}
        }

    }

    public void XrayMode()
    {
        isXRayMode = !isXRayMode;

        if (isXRayMode)
        {
            foreach (GameObject go in XRayObjects)
            {
                go.GetComponent<MeshRenderer>().material = XRayMat;
            }
            Debug.Log("X-Ray Mode Enabled");
            if (isHot)
            {
                Motor.GetComponent<MeshRenderer>().material = HotMat;
            }
        }
        else
        {
            foreach (GameObject go in XRayObjects)
            {
                go.GetComponent<MeshRenderer>().material = originalMat;
            }
        }

    }
}
