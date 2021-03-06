﻿using Assets.Scripts.Broadcast;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;

    public ManagerStatus Status { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting...");
        _network = service;

        Health = 50;
        MaxHealth = 100;

        Status = ManagerStatus.Started;
    }

    public void UpdateData(int health, int maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        Health += value;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        else if (Health < 0)
        {
            Health = 0;
        }

        if (Health == 0)
        {
            Messenger.Broadcast(GameEvent.LevelFailed);
        }
        Messenger.Broadcast(GameEvent.HealthUpdated);
    }

    public void Respawn()
    {
        UpdateData(50, 100);
    }
}
