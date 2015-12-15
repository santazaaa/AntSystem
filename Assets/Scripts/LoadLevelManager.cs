using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadLevelManager : MonoBehaviour {

//	private ScreenFadeInOut screen;
	public GameObject Image;
	bool isLoading;

	void Start(){
		
	}

	void Awake(){
		isLoading = false;
		//screen = ScreenFadeInOut.Instance;
//		screen.FadeToBlack ();
	}

	public void ChangeLevel(string levelName){
		if (isLoading)
			return;
		Image.GetComponent<Animator>().SetBool("isFade",true);
		StartCoroutine (loadLevel (levelName));

	}

	IEnumerator loadLevel(string levelName)
	{
		isLoading = true;
		yield return new WaitForSeconds (1.0f);
		Application.LoadLevel (levelName);
	}

}
