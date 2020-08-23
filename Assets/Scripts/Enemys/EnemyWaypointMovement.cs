﻿using UnityEngine;
using System.Collections.Generic;
using TinyGecko.Pathfinding2D;

[RequireComponent(typeof(Enemy))]
public class EnemyWaypointMovement : MonoBehaviour
{
    public WaypointType type = WaypointType.Normal;
    private Pathfinder pf;
    private Queue<GridCel> path;
    private Transform targetTower;
    private Vector3 target;
    private int currentWaypointIndex = 0;

    private Enemy enemy;

    public enum WaypointType 
    {
        Normal,
        Hijacker
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        if (this.type == WaypointType.Normal)
        {
            target = Waypoints.waypoints[0].position;
        }
        else 
        {
            SetPathHijacker();
        }
    }

    void SetPathHijacker() 
    {
        this.pf = new Pathfinder(WorldGrid.Instance);
        var sm = StructureManager.Instance;
        targetTower = sm.PlacedStructures[Random.Range(0, sm.PlacedStructures.Count)].transform;
        path = pf.FindPath(transform.position, targetTower.GetComponent<Structure>());
        target = path.Dequeue().worldPos;
    }

    private void Update()
    {
        if (type == WaypointType.Hijacker && !targetTower) 
        {
            SetPathHijacker();
        }
        Vector3 waypointDirection = (target - transform.position).normalized * enemy.MovementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target);
        if (distance <= Vector3.Distance(transform.position + waypointDirection,target)) 
        {
            GetNextWaypoint();
        }
        else
        {
            transform.Translate(waypointDirection, Space.World);
        }
    }

    private void GetNextWaypoint()
    {
        if (this.type == WaypointType.Normal)
        {
            if (currentWaypointIndex >= Waypoints.waypoints.Length - 1)
            {
                ReachObjective();
                return;
            }

            currentWaypointIndex++;
            target = Waypoints.waypoints[currentWaypointIndex].position;
        }
        else 
        {
            if (path.Count == 0) 
            {
                ReachObjective();
                return;
            }
            target = path.Dequeue().worldPos;
        }
    }

    private void ReachObjective()
    {
        //Decrease the Player RAM or CPU
        if (type == WaypointType.Normal)
        {
            PlayerStatus.Instance.UsedMemory += enemy.memoryUsage;
            WaveSpawner.Instance.RemoveEnemy(enemy);
            Destroy(gameObject);
        }
        else 
        {
            enemy.HijackTower(targetTower.GetComponent<Tower>());
        }
    }
}
