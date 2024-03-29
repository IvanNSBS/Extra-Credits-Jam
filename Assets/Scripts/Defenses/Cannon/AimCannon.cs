﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCannon : Tower
{
    public float TimeAiming;


    float currentTime = 0;
    [SerializeField]
    Transform target;
    float distance;
    private List<Transform> enemies;

    private void Start()
    {
        this.enemies = WaveSpawner.Instance.GetEnemiesTransform();
    }

    private void Update()
    {
        if (target)
        {
            LookAtTarget();
            this.currentTime += Time.deltaTime;
            if (this.currentTime >= TimeAiming) 
            {
                if (!base.stalled) 
                {
                    Fire();
                }
                ResetTimer();
                this.target = null;
            }
        }
        else 
        {
            TryLockTarget();
        }
    }

    protected override bool Fire() 
    {
        Vector3 direction = cannons[0].transform.position - transform.position;
        cannons[0].Fire(direction);
        return true;
    }

    public float turnSpeed = 20f;

    void LookAtTarget()
    {
        float dist = 0.1f;
        Vector3 targetDir = target.position - transform.position;
        
        float angle = Vector3.Angle(targetDir, Vector3.right);
        
        angle *= Mathf.Deg2Rad;
        angle += Mathf.PI / 2;
        Vector3 right = new Vector3(0, dist, 0);
        if (targetDir.y > 0) 
        {
            right *= -1;
        }
        
        cannons[0].transform.localPosition = right * Mathf.Cos(angle) + new Vector3(dist,0,0) * Mathf.Sin(angle);

        //Vector3 targetDir = target.position - transform.position;
        //Quaternion LookRotation = Quaternion.LookRotation(targetDir);
        //Vector3 rotation = Quaternion.Lerp(transform.rotation, LookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        ////Debug.Log(rotation.z);
        //transform.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        //if (targetDir.x < 0f)
        //{
        //    transform.Rotate(0, 0, 180f);
        //}


        //Debug.Log(cannon.transform.localPosition.normalized);


        //transform.eulerAngles = new Vector3(0,0,Mathf.Rad2Deg * angle);
        //transform.LookAt(target.position);
    }

    void ResetTimer() 
    {
        this.currentTime = 0;
    }

    void TryLockTarget() 
    {
        target = null;
        if(enemies.Count > 0)
        {
            foreach (Transform enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (!target || distance < this.distance)
                {
                    this.target = enemy.transform;
                    this.distance = distance;
                }
            }
        } 
    }
}
