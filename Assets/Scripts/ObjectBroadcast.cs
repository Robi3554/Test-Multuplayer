using UnityEngine;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using NUnit.Framework;
using FishNet.Transporting;
using System;

public class ObjectBroadcast : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();

    public int transformIndex;

    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<PositionIndex>(OnPosBroadcast);
        InstanceFinder.ServerManager.RegisterBroadcast<PositionIndex>(OnClientPosBroadcast);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int nextIndex = transformIndex + 1;
            if (nextIndex >= positions.Count)
            {
                nextIndex = 0;
            }

            if (InstanceFinder.IsServerStarted)
            {
                InstanceFinder.ServerManager.Broadcast(new PositionIndex() { tIndex = nextIndex });
            }
            else if(InstanceFinder.IsClientStarted)
            {
                InstanceFinder.ClientManager.Broadcast(new PositionIndex() { tIndex = nextIndex });
            }
        }

        transform.position = positions[transformIndex].position;
    }

    private void OnPosBroadcast(PositionIndex posStruct, Channel channel)
    {
        transformIndex = posStruct.tIndex;
    }

    private void OnClientPosBroadcast(NetworkConnection networkConnection ,PositionIndex posStruct, Channel channel)
    {
        InstanceFinder.ServerManager.Broadcast(posStruct);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<PositionIndex>(OnPosBroadcast);
        InstanceFinder.ServerManager.UnregisterBroadcast<PositionIndex>(OnClientPosBroadcast);
    }

    public struct PositionIndex : IBroadcast
    {
        public int tIndex;
    }
}