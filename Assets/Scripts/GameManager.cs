using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip PumpOn;
    public AudioClip PumpRunning;
    public AudioClip PumpOff;
    enum Impeller { Open, SemiOpen, Closed };
    public GameObject LiquidFlow1, LiquidFlow2;
    public PinchSlider viscositySlider;
    public TMP_Text impellerLabel;
    public TMP_Text ViscosityType;
    private float pressureVal;
    private float previousPressureVal;
    public PressureEvent pressureGauge;

    public GameObject ClosedImpeller, SemiOpenImpeller;

    private GameObject currImpeller;
    private bool isPumpRunning;
    // Start is called before the first frame update
    void Start()
    {
        //viscositySlider.OnValueUpdated.AddListener(delegate { ViscosityChangeCheck(); });
        viscositySlider.OnInteractionEnded.AddListener(delegate { ViscosityChangeCheck(); });
        //viscositySlider.OnInteractionEnded.AddListener(delegate { ViscosityChangeCheck(); });
        currImpeller = ClosedImpeller;
        impellerLabel.text = "Closed Impeller";
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
    public void PumpAudioController()
    {
        pressureVal = 55f;
        pressureGauge.UpdateDial(55f);
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

    }

    public void ViscosityChangeCheck()
    {
        Debug.Log(viscositySlider.SliderValue);
        if (viscositySlider.SliderValue >= 0.95f)
            ViscosityType.text = "Solid";
        else if (viscositySlider.SliderValue >= 0.75f)
            ViscosityType.text = "Semi-Solid";
        else if (viscositySlider.SliderValue >= 0.50f)
            ViscosityType.text = "Soft Solids";
        else if (viscositySlider.SliderValue >= 0.05f)
            ViscosityType.text = "Slurry";
        else if (viscositySlider.SliderValue < 0.05f)
            ViscosityType.text = "Clean Liquid";

        if (isPumpRunning)
        {
            float ViscosityPressureVal = pressureVal + (viscositySlider.SliderValue * 10f);
            ViscosityPressureVal = Random.Range(ViscosityPressureVal, ViscosityPressureVal + 3);
            pressureGauge.UpdateDial(ViscosityPressureVal);
        }
        //pressureVal = viscositySlider.SliderValue * 10 + pressureVal;
        //pressureVal = pressureVal * viscositySlider.SliderValue;
        /*
        if (viscositySlider.SliderValue > previousPressureVal)
            pressureVal = viscositySlider.SliderValue * 10 - pressureVal;
        else
        {
            pressureVal = viscositySlider.SliderValue * 10 + pressureVal;
        }
        previousPressureVal = viscositySlider.SliderValue;
        */
    }
    // Update is called once per frame
    void Update()
    {
        if (isPumpRunning)
        {
            currImpeller.transform.Rotate(0, -500 * Time.deltaTime, 0);
            //pressureGauge.UpdateDial(pressureVal);
        }

    }
}
