using System.Diagnostics;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stayTime_s = 3f;
    [SerializeField] float waypointDistance = 0.2f;
    [SerializeField] Transform[] waypoints;

    public bool shouldPatrol;

    enum PatrolState {
        Move,
        Stay
    }

    [SerializeField] private PatrolState patrolState;
    [SerializeField] private int currWpIdx;
    [SerializeField] private Stopwatch stayStopwatch;

    private SpriteRenderer rend;


    private void Start() {
        currWpIdx = 0;
        shouldPatrol = true;
        patrolState = PatrolState.Move;
        stayStopwatch = new Stopwatch();
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if(!shouldPatrol) {
            return;
        }

        switch (patrolState) {
            case PatrolState.Move:
            UnityEngine.Debug.Log($"waypoint idx {currWpIdx}");
            // todo fix lol
            transform.position = Vector3.Lerp(transform.position, waypoints[currWpIdx].position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance((Vector2)transform.position, (Vector2)waypoints[currWpIdx].position) < waypointDistance) {
                patrolState = PatrolState.Stay;
                stayStopwatch.Start();
                currWpIdx++;
                currWpIdx = (currWpIdx >= waypoints.Length) ? 0 : currWpIdx;
            }
            break;

            case PatrolState.Stay:
            UnityEngine.Debug.Log($"time {Time.deltaTime} - stayTimerStart {stayStopwatch.Elapsed} > stay time {stayTime_s}");
            if(stayStopwatch.Elapsed.TotalSeconds > stayTime_s) {
                patrolState = PatrolState.Move;
                rend.flipX = (waypoints[currWpIdx].position.x - transform.position.x) < 0;
            }
            break;

            default: break;
        }
    }
}
