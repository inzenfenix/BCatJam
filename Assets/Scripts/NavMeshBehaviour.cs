using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBehaviour : MonoBehaviour
{
    private NavMeshSurface surface;

    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    private void OnEnable()
    {
        DestroyableBehaviour.onObjectDestroyed += BakeMesh;
    }

    private void OnDisable()
    {
        DestroyableBehaviour.onObjectDestroyed -= BakeMesh;
    }

    private void BakeMesh(object sender, EventArgs e)
    {
        //surface.BuildNavMesh();
    }
}