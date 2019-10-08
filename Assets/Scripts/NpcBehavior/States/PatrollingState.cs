﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingState : CitizenState {
    public override CitizenStateType StateName => CitizenStateType.Patrolling;

    [SerializeField]
    private Transform[] path;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private SuspicionMeter suspision;

    [SerializeField]
    private CitizenInfo info;

    private int currentDestination = 0;
    private bool wasBeingpossessed = false;

    public override void EnterState() {
        suspision.StartWatching();
        SetDestination();
    }

    public override void LeaveState() {
        suspision.StopWatching();
    }

    public override void UpdateState() {
        if (info.IsBeingPossessed && !wasBeingpossessed) {
            agent.isStopped = true;
        } else if (!info.IsBeingPossessed && wasBeingpossessed) {
            SetDestination();
        }

        wasBeingpossessed = info.IsBeingPossessed;

        if (agent.remainingDistance != Mathf.Infinity && agent.remainingDistance < 0.3f) {
            currentDestination++;
            currentDestination = (int) Mathf.Clamp((int) Mathf.Repeat((float) currentDestination, (float) path.Length - 1), 0, Mathf.Infinity);
            SetDestination();
        }
    }

    private void SetDestination() {
        agent.destination = path[currentDestination].position;
    }

}