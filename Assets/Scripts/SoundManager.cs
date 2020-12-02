using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    [Space]
    [Header("Engine Loop")]
    [SerializeField] private bool playCarLoop = true;
    public AudioSource carLoop;
    [SerializeField] private Transform player;
    [SerializeField] private float defaultLevitateY = 1.84f;
    [Space]
    [Range(0f, 1f)]
    [SerializeField] private float pitchDamping = 0.5f;
    [Range(-10f, 10f)]
    [SerializeField] private float pitchOffset = 0f;

    [Space]
    [Header("Game Over")]
    [SerializeField] private bool playGameOverSound = true;
    public AudioSource gameOverSound;

    [Space]
    [Header("Coin Collect")]
    [SerializeField] private bool playCoinCollectSound = true;
    public AudioSource coinCollectSound;


    void Start()
    {
        carLoop.loop = true;

        if (playCarLoop) carLoop.Play();

        Player.OnCollectCoin += PlayCoinCollectSound;
        Player.OnGameOver += PlayGameOverSound;
    }

    void Update()
    {
        if (playCarLoop) carLoop.pitch = ((player.position.y + pitchOffset) / defaultLevitateY) * (1 - pitchDamping) + pitchDamping;
    }

    private float GetGroundY()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position, Vector3.down, out hit, 100))
            return hit.point.y;

        return player.position.y;
    }

    public void PlayGameOverSound()
    {
        if (playGameOverSound) gameOverSound.Play();
    }

    public void PlayCoinCollectSound()
    {
        if (playCoinCollectSound) coinCollectSound.Play();
    }
}
