using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    // Configuration
    // Ant speed
    public Text antSpeedValueText;
    public Slider antSpeedSlider;

    // Ant number
    public Text antNumberValueText;
    public Slider antNumberSlider;

    // rho
    public Text rhoValueText;
    public Slider rhoSlider;

    // alpha
    public Text alphaValueText;
    public Slider alphaSlider;

    // beta
    public Text betaValueText;
    public Slider betaSlider;

    // timescale
    public Text timeScaleValueText;
    public Slider timeScaleSlider;

    // apply
    public Button applyButton;

    // Top panels
    // pause & resume
    public Button pauseButton;
    public Button resumeButton;

    // reset & start
    public Button resetButton;
    public Button startButton;

	// Use this for initialization
	void Start () {
	    // Set min, max value
	}
	
	// Update is called once per frame
	void Update () {
	
	}
 
    public void OnAntSpeedValueChange()
    {
        antSpeedValueText.text = antSpeedSlider.value.ToString();
    }

    public void OnAntNumberValueChange()
    {
        antNumberValueText.text = antNumberSlider.value.ToString();
    }

    public void OnRhoValueChange()
    {
        rhoValueText.text = rhoSlider.value.ToString();
    }

    public void OnAlphaValueChange()
    {
        alphaValueText.text = alphaSlider.value.ToString();
    }

    public void OnBetaValueChange()
    {
        betaValueText.text = betaSlider.value.ToString();
    }

    public void OnTimeScaleValueChange()
    {
        timeScaleValueText.text = timeScaleSlider.value.ToString();
    }

    public void OnApplyClick()
    {

    }

    public void OnStartClick()
    {

    }

    public void OnResetClick()
    {

    }

    public void OnPauseClick()
    {

    }

    public void OnResumeClick()
    {

    }

}
