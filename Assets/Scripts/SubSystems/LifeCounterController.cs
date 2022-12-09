using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LifeCounterController : MonoBehaviour
{
    [Header("Life Properties")]
    public int value;

    private Image lifeImage;

    void Start()
    {
        lifeImage = GetComponent<Image>();
    }

    public void ResetLives()
    {
        value = 3;
        lifeImage.sprite = Resources.Load<Sprite>("Sprites/Life-3");
    }

    public void UpdateLife(int amount)
    {
        value += amount;
        
        if (value < 0)
        {
            value = 0;
        }
        else if (value > 3)
        {
            value = 3;
        }
        lifeImage.sprite = Resources.Load<Sprite>($"Sprites/Life-{value}");
    }
}
