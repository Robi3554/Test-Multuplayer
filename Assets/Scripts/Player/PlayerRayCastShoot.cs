using UnityEngine;
using FishNet.Object;
using FishNet.Example.ColliderRollbacks;
using System.Globalization;

public class PlayerRayCastShoot : NetworkBehaviour
{
    private float nextFireTime;

    public float damage;
    public float timeBetweenShots;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            GetComponent<PlayerRayCastShoot>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && nextFireTime <= 0)
        {
            nextFireTime = timeBetweenShots;

            ShootServer(damage, Camera.main.transform.position, Camera.main.transform.forward);
        }

        if(nextFireTime > 0)
            nextFireTime -= Time.deltaTime;
    }

    [ServerRpc]
    private void ShootServer(float damage, Vector3 pos, Vector3 dir)
    {
        if(Physics.Raycast(pos, dir, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Ray Hit!!!");
                hit.transform.GetComponent<PlayerHealth>().LoseHealth(damage);
            }
        }
    }
}
