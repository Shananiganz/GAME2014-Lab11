using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeathPlaneController : MonoBehaviour
{
    public Transform currentCheckPoint;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            var player = other.gameObject.GetComponent<PlayerBehaviour>();
            player.life.UpdateLife(-1);
            player.health.ResetHealth();
            ReSpawn(other.gameObject);

            if (player.life.value > 0)
            {
                ReSpawn(other.gameObject);
                FindObjectOfType<SoundManager>().PlaySoundFX(Channel.PLAYER_DEATH_FX, Sound.DEATH);
            }
        }
    }

    public void ReSpawn(GameObject go)
    {
        go.transform.position = currentCheckPoint.position;
    }
}
