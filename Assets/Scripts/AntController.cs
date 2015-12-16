using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntController : MonoBehaviour {

	private GameManager gameManager ;

	private NavMeshAgent navMeshAgent;
	private LogicManager logicManager ;

    private List<Node> previousNodes;
    private Node currentNode;

    [HideInInspector]
    public bool isWaiting;

	void Awake()
	{
        previousNodes = new List<Node>();
        isWaiting = false;
	}

	// Use this for initialization
	void Start () {
        logicManager = LogicManager.Instance;
        gameManager = GameManager.Instance;

  		navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isWaiting) return;
        if (Vector3.Distance(transform.position, navMeshAgent.destination) <= 5f)  // Reach destination
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.ResetPath();
            if(currentNode.getNodeId() == 0 && previousNodes.Count > 1)
            {                
                isWaiting = true;
                GameManager.Instance.reachHome();
                LogicManager.Instance.addDeltaPheromoneToPaths(previousNodes);
                return;
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

    public Node getCurrentNode()
    {
        return currentNode;
    }

    public List<Node> getPrevNodes()
    {
        return previousNodes;
    }

}
