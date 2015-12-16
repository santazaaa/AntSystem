using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BGMusicControl : MonoBehaviour {

	public GameManager gameManager;
	public GameObject bgmObject;

	private AudioSource BGM;

	[HideInInspector]
	public static BGMusicControl Instance
	{
		get
		{
			return _instance;
		}
	}

	private static BGMusicControl _instance;

	// Use this for initialization
	void Start () {
		_instance = this;
		gameManager = GameManager.Instance;
		BGM = bgmObject.GetComponent<AudioSource> ();
	}
	

	public void updateSoundSpeed(){
		float newPitch  = gameManager.speedUp;
		if (newPitch >= 1f) {
			newPitch--;
			newPitch /= (7);
			BGM.pitch = newPitch+1;
		} else {
			BGM.pitch = newPitch;

		}
	}

}
