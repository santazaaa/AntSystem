using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour {

    private GameManager gameManager;

    public int alpha;
    public int beta;
    public float rho;
    public float Q;

    [HideInInspector]
    public static LogicManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static LogicManager _instance;

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        gameManager = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float getPheromoneUpdate(float distance)
    {
        //Respect to ant id = k :D
        return Q / distance;
    }

    public void pheromoneDecay(List<Node> nodeList)
    {
        float distance = getTotalDistance(nodeList);
        for (int i = 0; i < gameManager.getNodeListSize(); i++)
        {
            for (int j = 0; j < i; j++)
            {
                float deltaT = getPheromoneUpdate(distance);
                Path path = gameManager.getPath(i, j);
                float temp = (1 - rho) * path.getPheromone() + deltaT;
                path.setPheromone(temp);
            }
        }
    }

    public Node getNextNode(List<Node> prevNodes, Node currentNode)
    {
        int nodeListSize = gameManager.getNodeListSize();

        if (prevNodes.Capacity >= gameManager.getNodeListSize()) return prevNodes[0];

        float[] prob = new float[nodeListSize];
        for (int i = 0; i < nodeListSize; i++)
        {
            Node tempNode = gameManager.getNode(i);
            if (prevNodes.Contains(tempNode) || i == currentNode.getNodeId())
            {
                prob[i] = 0.0f;
            }
            else
            {
                float p = getPathProb(gameManager.getPath(currentNode.getNodeId(), i));
                prob[i] = p;
            }
        }

        // Random prob with cumulative prob
        float sum = 0.0f;
        float rand = Random.Range(0.0f, 1.0f);
        for(int i = 0; i < prob.Length; i++)
        {
            sum += prob[i];
            if (rand <= sum) return gameManager.getNode(i);
        }

        Debug.Log("No next node for nodeId = " + currentNode.getNodeId());
        return currentNode;
    }

    public float getPathProb(Path path)
    {
        float t = path.getPheromone();
        float neta = 1.0f / path.getDistance();
        float p = Mathf.Pow((float)t, alpha) * Mathf.Pow((float)neta, beta);
        return p;
    }

    public float getTotalDistance(List<Node> nodeList)
    {
        float totalDistance = 0;
        int nodeListSize = nodeList.Capacity;
        for(int i = 0; i < nodeListSize - 1; i++)
        {
            totalDistance += gameManager.getPath(nodeList[i], nodeList[i+1]).getDistance();
        }
        return totalDistance;
    }
}
