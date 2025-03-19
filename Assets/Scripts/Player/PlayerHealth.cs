using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : NetworkBehaviour
{
    private readonly SyncVar<float> health = new SyncVar<float>();

    [SerializeField]
    private float healthValue;

    public TMP_Text healthText;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            GetComponent<PlayerHealth>().enabled = false;
        }
    }

    private void Awake()
    {
        health.OnChange += OnHealthChanged;
    }

    private void Start()
    {
        health.Value = healthValue;
    }

    private void OnHealthChanged(float oldValue, float newValue, bool asServer)
    {
        UpdateHealthUI(newValue);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
            LoseHealth();

        if (Input.GetKeyDown(KeyCode.Alpha4) && health.Value < 10)
            GainHealth();
    }

    private void UpdateHealthUI(float value)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {value}";
        }
    }

    [ServerRpc]
    private void LoseHealth()
    {
        health.Value--;
    }

    [ServerRpc]
    private void GainHealth()
    {
        health.Value++;
    }

    private void OnDestroy()
    {
        health.OnChange -= OnHealthChanged;
    }
}
