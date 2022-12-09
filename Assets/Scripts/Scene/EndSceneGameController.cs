using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneGameController : MonoBehaviour
{
    void Awake()
    {
        FindObjectOfType<SoundManager>().PlayMusic(Sound.END_MUSIC);
    }
}
