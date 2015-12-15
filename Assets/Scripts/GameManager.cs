using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    private List<List<Path>> pathList;
    [HideInInspector]
    private List<Node> nodeList;

	private LogicManager logicManager;
	public GameObject nodePrefab;
    public GameObject antPrefab;
    public GameObject pathPrefab;

	public int numberAnt;
	public int maxAnt;
	public int maxNode;

	public Node holeNode;

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
        pathList = new List<List<Path>>();
        nodeList = new List<Node>();
	}

	// Use this for initialization
	void Start () {
		numberAnt = 0;
        logicManager = LogicManager.Instance;
        nodeList.Add(holeNode);
        pathList.Add(new List<Path>());
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
		GameObject newNode = Instantiate (nodePrefab,position,Quaternion.identity) as GameObject;
        addNode(newNode.GetComponent<Node>());
	}

	public void spawnAnt(){
		GameObject newAnt = Instantiate (Ant,Holes[0],Quaternion.identity) as GameObject;
		numberAnt++;
		CancelInvoke ();
	}

    public void addNode(Node node)
    {
        nodeList.Add(node);
        List<Path> newPaths = new List<Path>();
        pathList.Add(newPaths);

        int nodeListSize = nodeList.Capacity;
        node.setNodeId(nodeListSize - 1);
        for (int i = 0; i < nodeListSize - 1; i++)
        {
            Node nodeI = getNode(i);
            GameObject pathObj = Instantiate(pathPrefab, (node.getPosition() + nodeI.getPosition()) / 2, Quaternion.identity);
            Path path = pathObj.GetComponent<Path>();
            path.setDistance(Vector3.Distance(node.getPosition(), nodeI.getPosition()));
            path.setPheromone(1);
            newPaths.Add(path);
        }
    }

    public Path getPath(int i, int j)
    {
        if (i < j)
        {
            return pathList[j][i];
        }
        return pathList[i][j];
    }

    public Path getPath(Node node1, Node node2)
    {
        return getPath(node1.getNodeId(), node2.getNodeId());
    }

    public Node getNode(int id)
    {
        if (id >= nodeList.Capacity) Debug.LogError("No node id = " + id);
        return nodeList[id];
    }

    public int getNodeListSize()
    {
        return nodeList.Capacity;
    }
}