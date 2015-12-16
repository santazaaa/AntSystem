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

    public float speedUp;
    public int antNumber;

	public Node holeNode;

    private float elapsedTime;

    [HideInInspector]
    public int antAtHomeCount;

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
		speedUp = 1;
        antAtHomeCount = 0;
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
        if (elapsedTime >= antSpawnDelay && getNodeListSize() >= 2 && antList.Count < antNumber)
        {
            spawnAnt();
            elapsedTime = 0;
		}

		//check amount of nodes on screen
		if(getNodeListSize() < maxNode) OnClick ();

        Debug.Log("Current ants at home " + antAtHomeCount);
        if (getNodeListSize() >= 2 && antAtHomeCount > 0)
        {
            if(antAtHomeCount == antList.Count) // All ants are at home
            {
                // Update pheromone
                foreach(List<Path> paths in pathList)
                {
                    foreach(Path path in paths)
                    {
                        path.updatePheromone(LogicManager.Instance.rho);
                    }
                    
                }
                // Let ants freeeeee!
                foreach(AntController ant in antList)
                {
                    if (!ant.isWaiting) continue;
                    ant.getPrevNodes().Clear();
                    ant.setCurrentNode(holeNode);
                    ant.isWaiting = false;
                }

                antAtHomeCount = 0;
            }
        }


		elapsedTime += Time.deltaTime;
	}


	public void OnClick()
	{
        #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                        Vector3 mousePos = Input.mousePosition;
                        PlayerController.Instance.OnTouch(mousePos);
                }
        #else
                if (Input.GetMouseButtonDown(0))
                {
                    if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) // Pointer is not over UIs
                    {
                        Vector3 mousePos = Input.mousePosition;
                        //click to spawn node
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(mousePos);
                        if (Physics.Raycast(ray, out hit))
                            if (hit.collider != null)
                                spawnNode(hit.point);

                    }
                }
        #endif

		
	}

    public void clearAll()
    {
        removeAnt(antList.Count);
        for(int i = 0; i < pathList.Count; i++)
        {
            List<Path> paths = pathList[i];
            for (int j = paths.Count - 1; j >= 0; j--)
            {
                Path p = paths[j];
                paths.RemoveAt(j);
                Destroy(p.gameObject);
            }
        }
        pathList.Clear();
        for(int i = nodeList.Count - 1; i >= 0; i--)
        {
            Node node = nodeList[i];
            if (node.getNodeId() == 0) continue;
            nodeList.RemoveAt(i);
            Destroy(node.gameObject);
        }
        // Initialize new value
        pathList.Add(new List<Path>());
        elapsedTime = 0;
        antNumber = 0;
		antAtHomeCount = 0;
    }
    
    public void setAntNumber(int number)
    {
        if (antList.Count == number) return;
        if(antList.Count < number)
        {
            antNumber = number;
        }
        else
        {
            Debug.Log("Remove ants " + (antList.Count - number));
            removeAnt(antList.Count - number);
            antNumber = number;
        }
    }

    public void reachHome()
    {
        antAtHomeCount++;
        Debug.Log("Reachhome = " + antAtHomeCount);
    }

    public void spawnAnt()
    {
        GameObject newAnt = Instantiate(antPrefab, holeNode.getPosition(), Quaternion.identity) as GameObject;
        AntController antCtrl = newAnt.GetComponent<AntController>();
        antCtrl.isWaiting = false;
        antCtrl.setCurrentNode(holeNode);
        antList.Add(antCtrl);
    }

    public void removeAnt(int number)
    {
        if(antList.Count >= number)
        {
            int count = 0;
            for(int i = antList.Count - 1; count < number && i >= 0; i--)
            {
                count++;
                
                AntController antCtrl = antList[i];
                antList.RemoveAt(i);
                if (antCtrl.isWaiting) antAtHomeCount--;
                Destroy(antCtrl.gameObject);
            }
        }
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

    public void ResetAnt()
    {
        removeAnt(antList.Count);
        logicManager.ResetPheromone();
        antAtHomeCount = 0;
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