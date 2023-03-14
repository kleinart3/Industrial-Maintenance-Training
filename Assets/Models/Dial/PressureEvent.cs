using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PressureEvent : MonoBehaviour
{
    public TMP_Text PressureText;
    public Image progress;

    private float dialFillAmount = 200f; // Currently hard coded to the value of Kyle's step motor

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateDial(float val)
    {
        float newVal = Mathf.Round(val);
        PressureText.text = newVal.ToString();
        //progress.fillAmount = Mathf.Round(((val / dialFillAmount)*100f) / 100f); // Formula for adjusting the dial fill
        progress.fillAmount = ((val * 100f) / dialFillAmount) / 100f;
    }

}
