using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour {

    private GameManager gameManager;

    public float alpha;
    public float beta;
    public float rho;
    public float Q;

    public float maxAlpha = 20;
    public float maxBeta = 20;
    public float maxRho = 1;
    public float maxQ = 50;

    private float currentMaxPheromone;

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
        currentMaxPheromone = 0;
    }

	// Use this for initialization
	void Start () 
    {
        gameManager = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetPheromone()
    {
        for(int i = 0; i < gameManager.getNodeListSize(); i++)
        {
            for(int j = 0; j < i; j++)
            {
                Path path = gameManager.getPath(i, j);
                path.setPheromone(1 / path.getDistance());
                path.setLineAlpha(0);
            }
        }
    }

    public float getPheromoneUpdate(float distance)
    {
        //Respect to ant id = k :D
        return Q / distance;
    }

    public void pheromoneDecay(List<Node> nodeList)
    {
        float distance = getTotalDistance(nodeList);
        int allNSize = gameManager.getNodeListSize();;

        // Decay pheromone in all path with rho
        for (int i = 0; i < allNSize - 1; i++)
        {
            for(int j = 0; j < i; j++)
            {
                Path path = gameManager.getPath(i, j);
                path.setPheromone((1 - rho) * path.getPheromone());
            }
        }

        // Add delta T to tour path only
        for (int i = 0; i < nodeList.Count - 1; i++)
        {
            Node nodeI = nodeList[i];
            Node nodeJ = nodeList[i + 1];

            float deltaT = getPheromoneUpdate(distance);

            Path path = gameManager.getPath(nodeI.getNodeId(), nodeJ.getNodeId());
           
            path.setPheromone(path.getPheromone() + deltaT);

            Debug.Log("Delta T = " + deltaT + " New P of path " + nodeI.getNodeId() + " " + nodeJ.getNodeId() + " = " + path.getPheromone());
        }

        //gameManager.updatePheromoneLine();
    }

    public Node getNextNode(List<Node> prevNodes, Node currentNode)
    {
        int nodeListSize = gameManager.getNodeListSize();

        if (prevNodes.Count >= gameManager.getNodeListSize())return prevNodes[0];

        float[] prob = new float[nodeListSize];
        float sum = 0.0f;
        List<Path> paths = new List<Path>();

        for (int i = 0; i < nodeListSize; i++)
        {
            Node tempNode = gameManager.getNode(i);
            if (prevNodes.Contains(tempNode) || i == currentNode.getNodeId())
            {
                prob[i] = 0.0f;
            }
            else
            {
                Path path = gameManager.getPath(currentNode.getNodeId(), i);
                float p = getPathProb(path);
                prob[i] = p;
                sum += p;
                paths.Add(path);
            }
        }

        for (int i = 0; i < paths.Count; i++)
        {
            Path path = paths[i];
            // Update line alpha with prob
            path.setLineAlpha(getPathProb(path) / sum);
        }

        // Random prob with cumulative prob
        float rand = Random.Range(0.0f, sum);
        sum = 0;
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
        int nodeListSize = nodeList.Count;
        for(int i = 0; i < nodeListSize - 1; i++)
        {
            totalDistance += gameManager.getPath(nodeList[i], nodeList[i+1]).getDistance();
        }
        return totalDistance;
    }
}
