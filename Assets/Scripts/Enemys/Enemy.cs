﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float defaultMovementSpeed = 4f;
    public float MovementSpeed { get; private set; }

    public int maxHealth;
    private float health;

    private bool isDead;

    private void Start()
    {
        MovementSpeed = defaultMovementSpeed;
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if(health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
    }

    public void Slow(float percentage)
    {
        MovementSpeed = defaultMovementSpeed * (1f - percentage);
    }

    public void IncreaseSpeed(float percentage)
    {
        MovementSpeed = defaultMovementSpeed * (1f + percentage);
    }

    public void ResetSpeed()
    {
        MovementSpeed = defaultMovementSpeed;
    }

    private void Die()
    {
        isDead = true;
        //Particle Effects
        //DestroyParticles
        WaveSpawner.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}