using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Space]
    [Header("General")]
    [SerializeField] private bool playSounds = true;
    [SerializeField] private bool logPlayerY = false;

    [Space]
    [Header("Engine Loop")]
    [SerializeField] private bool playCarLoop = true;
    public AudioSource carLoop;
    [SerializeField] private Transform player;
    [SerializeField] private float defaultLevitateY = 1.84f;
    [Space]
    [Tooltip("Controls the maximum altitude the pitch can change in")]
    [Range(0f, 1f)]
    [SerializeField] private float pitchDamping = 0.5f;
    [Tooltip("Adds a general offset to the pitch")]
    [Range(-10f, 10f)]
    [SerializeField] private float pitchOffset = 0f;

    [Space]
    [Header("Game Over")]
    [SerializeField] private bool playGameOverSound = true;
    public AudioSource gameOverSound;
    public bool knoardPlayGameOver = false;

    [Space]
    [Header("Coin Collect")]
    [SerializeField] private bool playCoinCollectSound = true;
    public AudioSource coinCollectSound;
    public bool knoardPlayCoinCollect = false;

    [Space]
    [Header("Ambient Sounds")]
    [SerializeField] private bool playAmbientSounds = true;
    [SerializeField] private float ambientSoundDelaySec = 5f;
    private float delayLeft;
    [SerializeField] private float ambientSoundChancePerDelay = 0.2f;
    public AudioSource[] ambientSounds;
    public bool knoardPlayAmbientSound = false;

    [Space]
    [Header("UI Button Sounds")]
    [SerializeField] private bool playButtonSounds = true;
    public AudioSource continueButtonSound;
    public AudioSource generalButtonSound;
    public AudioSource pauseButtonSound;
    public AudioSource startGameButtonSound;

    void Start()
    {
        carLoop.loop = true;

        PlayCarLoop();
        InvokeRepeating("PlayAmbientSound", 1f, 1f);

        Player.OnCollectCoin += PlayCoinCollectSound;
        Player.OnGameOver += PlayGameOverSound;

        Player.OnGameOver += StopCarLoop;
        PlayerInput.OnScreenTab += PlayCarLoop;

        UISoundEvents.SoundContinueButton += PlaySoundContinueButton;
        UISoundEvents.SoundGeneralButton += PlaySoundGeneralButton;
        UISoundEvents.SoundPauseButton += PlaySoundPauseButton;
        UISoundEvents.SoundStartGameButton += PlaySoundStartGameButton;

        InGameUIHandler.OnPause += ToggleMuteAllSounds;
    }

    private void OnDestroy()
    {
        Player.OnCollectCoin -= PlayCoinCollectSound;
        Player.OnGameOver -= PlayGameOverSound;

        Player.OnGameOver -= StopCarLoop;
        PlayerInput.OnScreenTab -= PlayCarLoop;

        UISoundEvents.SoundContinueButton -= PlaySoundContinueButton;
        UISoundEvents.SoundGeneralButton -= PlaySoundGeneralButton;
        UISoundEvents.SoundPauseButton -= PlaySoundPauseButton;
        UISoundEvents.SoundStartGameButton -= PlaySoundStartGameButton;

        InGameUIHandler.OnPause -= ToggleMuteAllSounds;
    }

    void Update()
    {
        if (logPlayerY) Debug.Log("PlayerY: " + player.position.y);

        if (playCarLoop && playSounds)
        {
            float newPitch = (player.position.y - defaultLevitateY + pitchOffset) * (1 - pitchDamping) + 1;
            if (carLoop.pitch != newPitch) carLoop.pitch = newPitch;
        }

        if (knoardPlayAmbientSound)
        {
            knoardPlayAmbientSound = false;
            ambientSounds[Random.Range(0, ambientSounds.Length - 1)].Play();
        }
        if (knoardPlayCoinCollect)
        {
            knoardPlayCoinCollect = false;
            PlayCoinCollectSound();
        }
        if (knoardPlayGameOver)
        {
            knoardPlayGameOver = false;
            PlayGameOverSound();
        }
    }

    private float GetGroundY()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position, Vector3.down, out hit, 100))
            return hit.point.y;

        return player.position.y;
    }

    private void PlayAmbientSound()
    {
        if (delayLeft > 0)
        {
            delayLeft--;
            return;
        }
        delayLeft = ambientSoundDelaySec;
        if (playAmbientSounds && playSounds && ambientSounds.Length > 0 && Random.value > 1 - ambientSoundChancePerDelay)
        {
            ambientSounds[Random.Range(0, ambientSounds.Length - 1)].Play();
        }
    }

    private void PlayGameOverSound()
    {
        if (playGameOverSound && playSounds) gameOverSound.Play();
    }

    private void StopCarLoop() => carLoop.Stop();

    private void PlayCarLoop()
    {
        if (playCarLoop && playSounds) carLoop.Play();
    }

    private void PlayCoinCollectSound()
    {
        if (playCoinCollectSound && playSounds) coinCollectSound.Play();
    }

    public void PlaySoundContinueButton()
    {
        if (playButtonSounds && playSounds) continueButtonSound.Play();
    }
    public void PlaySoundGeneralButton()
    {
        if (playButtonSounds && playSounds) generalButtonSound.Play();
    }
    public void PlaySoundPauseButton()
    {
        if (playButtonSounds && playSounds) pauseButtonSound.Play();
    }
    public void PlaySoundStartGameButton()
    {
        if (playButtonSounds && playSounds) startGameButtonSound.Play();
    }

    public void ToggleMuteAllSounds()
    {
        if (playSounds)
        {
            playSounds = false;
            StopCarLoop();
        }
        else
        {
            playSounds = true;
            PlayCarLoop();
        }
    }
}
