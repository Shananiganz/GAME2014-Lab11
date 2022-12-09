using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("Health Properties")]
    public int value;

    [Header("Display Properties")]
    public Slider healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
    }
    public void ResetHealth()
    {
        healthBar.value = 100;
        value = (int)healthBar.value;
    }

    public void UpdateHealth(int amount)
    {
        healthBar.value += amount;

        if(healthBar.value < 0)
        {
            healthBar.value = 0;
        }
        else if (healthBar.value > 100)
        {
            healthBar.value = 100;
        }

        value = (int)healthBar.value;
    }
}
