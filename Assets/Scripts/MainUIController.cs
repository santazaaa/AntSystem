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
    public Text QValueText;
    public Slider QSlider;

    // apply
    public Button applyButton;

    // Top panels
    // reset & start
    public Button resetButton;
    public Button clearButton;

	// Use this for initialization
	void Start () {
	    // Set min, max value
        antSpeedSlider.minValue = 0;
        antSpeedSlider.maxValue = 15;
        antSpeedSlider.value = 1;

        antNumberSlider.minValue = 0;
        antNumberSlider.maxValue = GameManager.Instance.maxAnt;
        antNumberSlider.value = Mathf.Min(3, GameManager.Instance.maxAnt);

        rhoSlider.minValue = 0;
        rhoSlider.maxValue = LogicManager.Instance.maxRho;
        rhoSlider.value = 0.2f;

        alphaSlider.minValue = 0;
        alphaSlider.maxValue = LogicManager.Instance.maxAlpha;
        alphaSlider.value = 1;

        betaSlider.minValue = 0;
        betaSlider.maxValue = LogicManager.Instance.maxBeta;
        betaSlider.value = 1;

        QSlider.minValue = 1;
        QSlider.maxValue = LogicManager.Instance.maxQ;
        QSlider.value = 1;
        
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
        antNumberValueText.text = ((int)antNumberSlider.value).ToString();
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

    public void OnQValueChange()
    {
        QValueText.text = QSlider.value.ToString();
    }

    public void OnApplyClick()
    {
        GameManager.Instance.speedUp = antSpeedSlider.value;
        GameManager.Instance.setAntNumber((int)antNumberSlider.value);
        LogicManager.Instance.rho = rhoSlider.value;
        LogicManager.Instance.alpha = alphaSlider.value;
        LogicManager.Instance.beta = betaSlider.value;
        LogicManager.Instance.Q = QSlider.value;
    }

    public void OnClearClick()
    {
        GameManager.Instance.clearAll();
    }

    public void OnResetClick()
    {
        GameManager.Instance.ResetAnt();
    }

}
