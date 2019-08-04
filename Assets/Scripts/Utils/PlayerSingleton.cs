﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerChangedEvent : UnityEvent<GameObject> { }

public class PlayerSingleton : MonoBehaviour {

    [SerializeField]
    private GameObject currentPlayer;
    private static PlayerSingleton instance;

    public GameObject CurrentPlayer => currentPlayer;
    public static PlayerSingleton Instance => instance;

    public PlayerChangedEvent PlayerChanged;

    public System.Action<GameObject, GameObject> OnPlayerChanged;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Double singleton! removing!");
            Destroy(this);
        }
        instance = this;
    }

    public void SetPlayer(GameObject newPlayer) {
        var statemachine = currentPlayer.GetComponent<CitizenStateMachine>();
        statemachine.SetState(CitizenStateType.Patrolling);
        var oldPlayer = currentPlayer;
        currentPlayer = newPlayer;
        OnPlayerChanged?.Invoke(oldPlayer, newPlayer);
        PlayerChanged?.Invoke(newPlayer);
    }

}