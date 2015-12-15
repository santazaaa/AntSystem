using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	public GameObject Image;
	public int a;

	// Use this for initialization
	void Start () {
		Image.GetComponent<Animator>().SetTrigger("Clear");
		Image.GetComponent<Animator>().SetBool("isFade",false);
		Image.GetComponent<Fader> ().a = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
