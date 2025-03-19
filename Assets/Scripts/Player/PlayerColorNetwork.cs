using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Example.ColliderRollbacks;
using FishNet.Demo.AdditiveScenes;

public class PlayerColorNetwork : NetworkBehaviour
{
    [SerializeField] private Color endColor;

    private Renderer playerRenderer;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.F))
        {
            ChangeColorServer(endColor);
        }
    }

    [ServerRpc]
    private void ChangeColorServer(Color newColor)
    {
        ChangeColorObservers(newColor);
    }

    [ObserversRpc]
    private void ChangeColorObservers(Color newColor)
    {
        if (playerRenderer == null)
            playerRenderer = GetComponent<Renderer>();

        playerRenderer.material.color = newColor;
    }
}
