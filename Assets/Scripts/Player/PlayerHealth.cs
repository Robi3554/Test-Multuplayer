using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;

public class PlayerHealth : NetworkBehaviour
{
    private readonly SyncVar<float> health = new SyncVar<float>();

    [SerializeField]
    private float healthValue;

    //private TMP_Text guiHealthText;

    public TMP_Text healthText;

    private float amount = 1;

    private void Awake()
    {
        health.OnChange += OnHealthChanged;

        //guiHealthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        health.Value = healthValue;
    }

    private void OnHealthChanged(float oldValue, float newValue, bool asServer)
    {
        UpdateHealthUI(newValue);
        //Debug
        healthValue = newValue;
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha3))
            LoseHealth(amount);

        if (Input.GetKeyDown(KeyCode.Alpha4) && health.Value < 10)
            GainHealth(amount);
    }

    private void UpdateHealthUI(float value)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {value}";
            //guiHealthText.text = value.ToString();
        }
    }

    [ObserversRpc]
    public void LoseHealth(float amount)
    {
        health.Value -= amount;
    }

    [ObserversRpc]
    public void GainHealth(float amount)
    {
        health.Value += amount;
    }

    private void OnDestroy()
    {
        health.OnChange -= OnHealthChanged;
    }
}
