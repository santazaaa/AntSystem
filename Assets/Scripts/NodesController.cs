using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodesController : MonoBehaviour {

	private GameManager gameManager ;


	public List<GameObject> nodesList = new List<GameObject>();
	public float [,] distanceMatrices = new float[100,100]; 
	public double [,] pheromonesMatrices =  new double[100,100]; 

	public float gameTime;
	public int node_numbers;

	public int alpha;
	public int beta;

	//public float timetodecay;
	public float p;

	private int timetreshold;

	public float Q;


	[HideInInspector]
	public static NodesController Instance
	{ 
		get
		{
			return _instance;
		}
	}

	private static NodesController _instance;


	void Awake()
	{
		_instance = this;
		gameManager = GameManager.Instance;
	}


	// Use this for initialization
	void Start () {
		node_numbers = 0;
		//gameTime = gameManager.gameTime;
	//	timetreshold = 1000;
	}
	
	// Update is called once per frame
	void Update () {
	//	gameTime = gameManager.gameTime;
//		if (gameTime % timetreshold == 0)
//			Debug.Log ("test");
		//InvokeRepeating("pheromoneDecay",timetodecay,timetodecay);
	}

	public void addNode(GameObject node){
		nodesList.Add (node) ;

		//Add new node to Matrices
		int n = node_numbers;

		for (int i = 0; i < node_numbers; i++) {
			if (i != n) {
				float distance = Mathf.Abs (Vector3.Distance (nodesList [i].transform.localPosition, nodesList [n].transform.localPosition));
				distanceMatrices [i, n] = distance;
				distanceMatrices [n, i] = distance;

				pheromonesMatrices [i, n] = 1.0;
				pheromonesMatrices [n, i] = 1.0;
			}
		}
		distanceMatrices [n,n] = 0.0f;
		pheromonesMatrices [n, n] = 0.000001;
		node_numbers++;
	}

	public double updatePheromone(int previousNode,int nextNode){
		return Q/distanceMatrices[previousNode,nextNode];
	}

	public void pheromoneDecay(int previousNode,int nextNode){
		for (int i = 0; i < node_numbers; i++) {
			for (int j = 0; j < node_numbers; j++) {
				double deltaT;

				if (i == j || previousNode == nextNode) {
					deltaT = 0.0;
				} else {
					deltaT = updatePheromone (previousNode, nextNode);
				}
				double temp = (1 - p) * pheromonesMatrices [i, j] + deltaT;
				pheromonesMatrices [i, j] = temp;
				pheromonesMatrices [j, i] = temp;
			}	
		}
	}

}
