using UnityEngine;
using FishNet.Object;

public class PlayerSpawnObject : NetworkBehaviour
{
    public GameObject obj;

    public GameObject spawnedObj;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
    }

    private void Update()
    {
        if(spawnedObj == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnObject(obj, transform, this);
        }

        if (spawnedObj != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            DespawnObject(spawnedObj);
        }
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
        ServerManager.Spawn(spawned);
        SetSapwnedObj(spawned, script);
    }

    [ObserversRpc]
    public void SetSapwnedObj(GameObject obj, PlayerSpawnObject script)
    {
        script.spawnedObj = obj;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }

}
