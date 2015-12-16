using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    private List<List<Path>> pathList;
    [HideInInspector]
    private List<Node> nodeList;
    [HideInInspector]
    private List<AntController> antList;

	private LogicManager logicManager;
	public GameObject nodePrefab;
    public GameObject antPrefab;
    public GameObject pathPrefab;

	public int maxAnt;
	public int maxNode;
    public float antSpawnDelay;
    public float speedUp = 1;

	public Node holeNode;

    private float elapsedTime;

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
        antList = new List<AntController>();
        elapsedTime = 0;
    }

	// Use this for initialization
	void Start () {
        logicManager = LogicManager.Instance;
        nodeList.Add(holeNode);
        holeNode.setNodeId(0);
        pathList.Add(new List<Path>());
	}
	
	// Update is called once per frame
	void Update () {
        Time.timeScale = speedUp;
		//if there are more than two holes, spawn the ants
		if (elapsedTime >= antSpawnDelay && getNodeListSize() >= 2 && antList.Count < maxAnt) {
            spawnAnt();
            elapsedTime = 0;
		}

		//check amount of nodes on screen
		if(getNodeListSize() < maxNode) OnClick ();

		elapsedTime += Time.deltaTime;
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

    public void spawnAnt()
    {
        GameObject newAnt = Instantiate(antPrefab, holeNode.getPosition(), Quaternion.identity) as GameObject;
        AntController antCtrl = newAnt.GetComponent<AntController>();
        antCtrl.setCurrentNode(holeNode);
        antList.Add(antCtrl);
    }

	public void spawnNode(Vector3 position){
		GameObject newNode = Instantiate (nodePrefab,position,Quaternion.identity) as GameObject;
        addNode(newNode.GetComponent<Node>());
	}

    public void addNode(Node node)
    {
        nodeList.Add(node);
        List<Path> newPaths = new List<Path>();
        pathList.Add(newPaths);

        int nodeListSize = nodeList.Count;
        node.setNodeId(nodeListSize - 1);
        for (int i = 0; i < nodeListSize - 1; i++)
        {
            Node nodeI = getNode(i);
            GameObject pathObj = Instantiate(pathPrefab, (node.getPosition() + nodeI.getPosition()) / 2, Quaternion.identity) as GameObject;
            Path path = pathObj.GetComponent<Path>();
            float distance = Vector3.Distance(node.getPosition(), nodeI.getPosition());
            path.setDistance(distance);
            path.setPosition(node.getPosition(), nodeI.getPosition());
            path.setPheromone(1 / distance);
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
        if (id >= nodeList.Count) Debug.LogError("No node id = " + id);
        return nodeList[id];
    }

    public int getNodeListSize()
    {
        return nodeList.Count;
    }

    public void updatePheromoneLine()
    {
        float maxPheromone = 0;
        for (int i = 0; i < pathList.Count; i++)
        {
            for (int j = 0; j < pathList[i].Count; j++)
            {
                //if (logicManager.getPathProb(pathList[i][j]) > maxPheromone)
                    maxPheromone += logicManager.getPathProb(pathList[i][j]);
            }
        }

        if (maxPheromone < 1e-4) return;

        for (int i = 0; i < pathList.Count; i++)
        {
            for (int j = 0; j < pathList[i].Count; j++)
            {
                pathList[i][j].setLineAlpha(logicManager.getPathProb(pathList[i][j]) / maxPheromone);
            }
        }
    }
}