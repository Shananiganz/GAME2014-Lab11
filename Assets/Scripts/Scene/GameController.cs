using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject onScreenControls;
    public GameObject miniMap;
    public SoundManager soundManager;

    void Awake()
    {
        onScreenControls = GameObject.Find("OnScreenControls");
        onScreenControls.SetActive(Application.isMobilePlatform);
        soundManager = FindObjectOfType<SoundManager>();
        miniMap = GameObject.Find("MiniMap");
    }

    private void Start()
    {
        if (miniMap)
        {
            miniMap.SetActive(false);
        }

        BulletManager.Instance().BuildBulletPool();
        soundManager.PlayMusic(Sound.MAIN_MUSIC);
    }

    private void Update()
    {
        if ((miniMap) && (Input.GetKeyDown(KeyCode.M)))
        {
            miniMap.SetActive(!miniMap.activeInHierarchy);
        }
    }
}
