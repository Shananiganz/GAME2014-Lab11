using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject miniMap;

    private void Start()
    {
        miniMap = GameObject.Find("MiniMap");
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnYButtonPressed()
    {
        miniMap.SetActive(!miniMap.activeInHierarchy);
    }
}
