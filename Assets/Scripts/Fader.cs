using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	public GameObject Image;


	// Use this for initialization
	void Start () {
		Image.GetComponent<Animator>().SetTrigger("Clear");
		Image.GetComponent<Animator>().SetBool("isFade",false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
