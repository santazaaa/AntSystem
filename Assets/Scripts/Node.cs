using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

    private int nodeId;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    public int getNodeId()
    {
        return nodeId;
    }

    public void setNodeId(int _id)
    {
        this.nodeId = _id;
    }
}
