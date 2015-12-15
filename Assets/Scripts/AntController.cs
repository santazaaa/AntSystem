using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntController : MonoBehaviour {

	private GameManager gameManager ;

	private NavMeshAgent navMeshAgent;
	private LogicManager logicManager ;

    private List<Node> previousNodes;
    private Node currentNode;

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

        if (Vector3.Distance(transform.position, navMeshAgent.destination) <= 1.5f)  // Reach destination
        {
            navMeshAgent.velocity = Vector3.zero;
            if(currentNode.getNodeId() == 0 && previousNodes.Count > 1)
            {
                // Come back home, reset statistic
                logicManager.pheromoneDecay(previousNodes);
                previousNodes.Clear();
            }
			previousNodes.Add (currentNode);
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

    public void setCurrentNode(Node node)
    {
        currentNode = node;
    }

}
