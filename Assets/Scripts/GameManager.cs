using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public NodesController nodes ;
	public GameObject node;
	public GameObject Ant;

	public float gameTime;
	public int numberAnt;
	public int maxAnt;
	public int maxNode;

	public Vector3 [] Holes = new Vector3[2];

	[HideInInspector]
	public static GameManager Instance
	{ 
		get
		{
			return _instance;
		}
	}

	private static GameManager _instance;



	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		numberAnt = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		//Set Start Point and Destination Point for ants ...actually this is useless and pointless lol 
		if (nodes.node_numbers == 2) {
				Holes [0] = nodes.nodesList [0].transform.localPosition;
				Holes [1] = nodes.nodesList [1].transform.localPosition;
			}

		//if there are more than two holes, spawn the ants
		if (nodes.node_numbers >= 2) {
			if (numberAnt <= maxAnt - 1) {
				InvokeRepeating("spawnAnt", 0.5f, 0.5f); 
			}
		}

		//check amount of nodes on screen
		if(nodes.node_numbers<=maxNode)OnClick ();

		gameTime += Time.deltaTime;
	}


	public void OnClick()
	{
		//click to spawn node
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit))
			if (hit.collider != null)
				spawnNode (hit.point);

		}
	}

	public void spawnNode(Vector3 position){
		GameObject newNode = Instantiate (node,position,Quaternion.identity) as GameObject;
		nodes.addNode (newNode);
		Debug.Log ("pikachu");

	}

	public void spawnAnt(){
		GameObject newAnt = Instantiate (Ant,Holes[0],Quaternion.identity) as GameObject;
		numberAnt++;
		CancelInvoke ();
	}


}