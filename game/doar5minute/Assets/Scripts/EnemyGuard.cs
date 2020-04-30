﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuard : AbstractEnemy
{
    public float speed = 5;           // Guard speed
    public float waitTime = 1f;       // Time for which the guard will stop at some point
    int faceDirection;                // Direction Guard is facing => 1 means right; -1 means left

    public Transform pathHolder;      // a gameObject that denotes the path, it has children that denote a waypoint

    private void Start()
    {
        Vector2[] waypoints = new Vector2[pathHolder.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }
        StartCoroutine(FollowPath(waypoints));
    }

    IEnumerator FollowPath(Vector2[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector2 targetWaypoint = waypoints[targetWaypointIndex];

        while (true)
        {
            if (transform.position.x < targetWaypoint.x)
                faceDirection = 1;
            else
                faceDirection = -1;
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if((Vector2)transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex+1)% waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Vector2 startPosition = pathHolder.GetChild(0).position;
        Vector2 previousPosition = startPosition;

        foreach(Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
