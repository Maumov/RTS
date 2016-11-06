using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Move : MonoBehaviour,IAction {

	public void OriginPositionAction ()
	{
		
	}

	public void DestinationPositionAcion ()
	{
		
	}

	public Unit target {
		get {
			return null;
		}
		set {
			
		}
	}

	public Vector3 originPosition {
		get {
			return transform.position;
		}
		set {
			
		}
	}

	public Vector3 destinationPosition {
		get {
			return destination;
		}
		set {
			destination = value;
		}
	}
	public float stopThreshHold = 0.1f;
	Vector3 destination;
    NavMeshAgent agent;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
       
	}
    public bool IsTraveling() {
		return (agent.remainingDistance < stopThreshHold);
    }
    public bool IsIdle()
    {
        return !IsTraveling();
    }
    public void SetDestination(Vector3 dest) {
		destinationPosition = dest;
		agent.SetDestination (destinationPosition);
    }

}
