using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntController : MonoBehaviour {

	private GameManager gameManager ;

	private NavMeshAgent ant;
	public NodesController nodes ;

	public List <int> previousNodes = new List<int>();
	public int previousNode;
	public int nextNode; 

	public Vector3 destination;
	public float distance;

	Vector3 nextPosition;

	//public bool running;
	public double ran;


	void Awake()
	{
		//Make NodesController Instance
		nodes = NodesController.Instance;
		gameManager = GameManager.Instance;

	}

	// Use this for initialization
	void Start () {
	//	running = true;
		previousNode = 0;
		previousNodes.Add (previousNode);
		ant = GetComponent<NavMeshAgent>();
		getNextPos ();
		}
	
	// Update is called once per frame
	void Update () {
			
		//if Ant arrive at destination
		distance = Vector3.Distance (this.transform.localPosition ,  nextPosition);
		if (distance <= 0.15f || distance == 0 ) {
			//running = false;
			//nodes.pheromoneDecay(previousNode,nextNode);
			previousNode = nextNode;
			if(!previousNodes.Contains(previousNode))previousNodes.Add (previousNode);
			getNextPos ();
		}

		//Move
		move ();
		//if(ran==0)this.StartSinking ();



	}

	//algorithm of Any System here
	void getNextPos(){

		//Initializing Variables
		int n = nodes.node_numbers;
		float []  prob = new float[n];
		int alpha = nodes.alpha;
		int beta = nodes.beta;

		//Ant System Formula

		//calculate the probability by find the product of T^a and n^b and store prob array
		float sum = 0.0f;
		for(int i = 0; i < n ; i++) {
			if (i == previousNode || previousNodes.Contains(i)) {
				prob [i] = 0.0f;
			} else {


				double t = nodes.pheromonesMatrices [previousNode, i];
				double u;

				u = (double)(1 / nodes.distanceMatrices [previousNode, i]);
				float p = Mathf.Pow ((float)t, alpha) * Mathf.Pow ((float)u, beta);
				prob [i] = p;
				sum += p;
			}
		}

		//cumulative all the result in array... if you want to know why? just ask me. 
		float prev = 0.0f;
		for(int i = 0; i < n ; i++) {
			
			float temp = prob[i];
			prob[i] += prev;
			prev = temp;
		}

		//random the number and find the next Node
		ran = Random.Range(0.0f,sum);
		for(int i = 0; i < n ; i++){
			if(i!=previousNode &&  !previousNodes.Contains(i)){
				if (ran <= prob [i]) {
					nextNode = i;
					nextPosition = nodes.nodesList [i].transform.localPosition;
					//running = true;
					break;
				}
			}
		}
	}
		
	void move(){
		ant.SetDestination (nodes.nodesList[nextNode].transform.localPosition);
	//	destination = nodes.nodesList [nextNode].transform.localPosition;
	}


	public void StartSinking ()
	{
		/*// Find and disable the Nav Mesh Agent.
		GetComponent <NavMeshAgent> ().enabled = false;

		transform.Translate (-Vector3.up * 2.0f * Time.deltaTime);

		// After 2 seconds destory the enemy.

		gameManager.numberAnt--;*/
	}

}
