using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntController : MonoBehaviour {

	private GameManager gameManager ;

	private NavMeshAgent navMeshAgent;
	private LogicManager logicManager ;

    private List<Node> previousNodes;
	private Node currentNode;

	private float totalDistance;

	void Awake()
	{
        previousNodes = new List<Node>();

	}

	// Use this for initialization
	void Start () {
        logicManager = LogicManager.Instance;
        gameManager = GameManager.Instance;

  		navMeshAgent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {
			
        if (navMeshAgent.remainingDistance <= 0.15f) // Reach destination
        {
            if(currentNode.getNodeId() == 0 && previousNodes.Capacity > 1)
            {
                // Come back home, reset statistic
                logicManager.pheromoneDecay(previousNodes);
                previousNodes.Clear();
                totalDistance = 0;
            }
			if(!previousNodes.Contains(currentNode))previousNodes.Add (currentNode);
            Node nextNode = logicManager.getNextNode(previousNodes, currentNode);
            currentNode = nextNode;
            moveTo(nextNode.getPosition());
		}

	}
		
	void moveTo(Vector3 targetPos){
		navMeshAgent.SetDestination (targetPos);
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
