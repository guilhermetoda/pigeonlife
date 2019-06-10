  // Patrol.cs
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections;
using TMPro;

public class Patrol : MonoBehaviour {

        public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;

        private bool stop =false;
        private bool _followingPlayer = false;

        public Transform pigeon;

        private bool moving;
        public float enemyWaitTime = 2f;

        private string state;
        [SerializeField] TextMeshPro _textMesh;
        
    

        void Start () {
            agent = GetComponent<NavMeshAgent>();
            state = "Patrol";
            _textMesh.text = state;
            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            GotoNextPoint();
        }

    

    public string GetState() 
    {
        return state;
    }

    public void SetState(string newState) 
    {
        state = newState;
        _textMesh.text = state;
    }

    IEnumerator WaitAndGo(float time, Transform follow)
    {
        
        yield return new WaitForSeconds(time);
        GoToPlayer(follow);
    }

    public void NoTargetAlert() 
    {
        StartCoroutine(WaitForTarget(3));
    }

    IEnumerator WaitForTarget(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("END Wait");
        if (state == "NoTargetAlert") 
        {
            Debug.Log("IF");
            SetState("Patrol");
            _followingPlayer = false;
            GotoNextPoint();
            pigeon.GetComponent<PlayerMovement>().NotAlert();
            pigeon = null;
        }
    }

    public void SetNewDestination(Transform dest) 
    {
        agent.destination = dest.position;
        agent.isStopped = false;
        stop = false;
    }

    public void GoToPlayer(Transform follow)
    {
        //agent.destination = playerPosition.position;
        agent.destination = follow.position;
        agent.isStopped = false;
        stop = false;
        _followingPlayer = true;
        SetState("Chase");
    }

    public void Alert(Transform follow) 
    {
        pigeon = follow;
        pigeon.GetComponent<PlayerMovement>().SetAlertMode();
        stop = true;
        agent.isStopped = true;
        StartCoroutine(WaitAndGo(1, follow));
        // Wait for 3s and go check it out guys
        
    }


    void GotoNextPoint() {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;
            
            agent.isStopped = false;
            stop = false;
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }


        void Update () {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !_followingPlayer)
                GotoNextPoint();
        }
    }