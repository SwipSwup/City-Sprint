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
    [SerializeField] private float carLoopWithSilencer = 0.5f;
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

    [Space]
    [Header("Coin Collect")]
    [SerializeField] private bool playCoinCollectSound = true;
    public AudioSource coinCollectSound;

    [Space]
    [Header("Ambient Sounds")]
    [SerializeField] private bool playAmbientSounds = true;
    private bool ambientSoundsActive = true;
    [SerializeField] private float ambientSoundDelaySec = 5f;
    private float delayLeft;
    [SerializeField] private float ambientSoundChancePerDelay = 0.2f;
    public AudioSource[] ambientSounds;

    [Space]
    [Header("City Sounds Loop")]
    [SerializeField] private bool playCitySounds = true;
    public AudioSource citySounds;

    [Space]
    [Header("UI Button Sounds")]
    [SerializeField] private bool playButtonSounds = true;
    public AudioSource continueButtonSound;
    public AudioSource generalButtonSound;
    public AudioSource pauseButtonSound;
    public AudioSource startGameButtonSound;
    public AudioSource fanFareSound;
    public AudioSource scoreCountSound;

    [Space]
    [Header("Knoard Testing")]
    public bool knoardPlayGameOver = false;
    public bool knoardPlayCoinCollect = false;

    [Space]
    public bool knoardPlayCarPassingBy = false;
    public bool knoardPlayCitySounds = false;
    public bool knoardPlaySirens = false;

    [Space]
    public bool knoardPlayContinueButton = false;
    public bool knoardPlayGeneralButton = false;
    public bool knoardPlayPauseButton = false;
    public bool knoardPlayStartGame = false;
    public bool knoardPlayFanFare = false;
    public bool knoardPlayScoreCount = false;


    void Start()
    {
        carLoop.loop = true;
        citySounds.loop = true;

        PlayCitySoundsLoop();
        PlayCarLoop();
        InvokeRepeating("PlayAmbientSound", 1f, 1f);

        Player.OnCollectCoin += PlayCoinCollectSound;
        Player.OnGameOver += PlayGameOverSound;

        Player.OnGameOver += StopCarLoop;
        PlayerInput.OnScreenTab += PlayCitySoundsLoop;
        PlayerInput.OnScreenTab += MakeCarLoopSilent;
        PlayerInput.OnScreenTab += PlayCarLoop;

        UISoundEvents.SoundContinueButton += PlaySoundContinueButton;
        UISoundEvents.SoundGeneralButton += PlaySoundGeneralButton;
        UISoundEvents.SoundStartGameButton += PlaySoundStartGameButton;
        UISoundEvents.SoundStartGameButton += StopCitySoundsLoop;
        UISoundEvents.SoundStartGameButton += stopAllAmbientSounds;
        UISoundEvents.SoundStartGameButton += MakeCarLoopLoud;

        InGameUIHandler.OnPause += TogglePause;
    }

    private void OnDestroy()
    {
        Player.OnCollectCoin -= PlayCoinCollectSound;
        Player.OnGameOver -= PlayGameOverSound;

        Player.OnGameOver -= StopCarLoop;
        PlayerInput.OnScreenTab -= PlayCitySoundsLoop;
        PlayerInput.OnScreenTab -= MakeCarLoopSilent;
        PlayerInput.OnScreenTab -= PlayCarLoop;

        UISoundEvents.SoundContinueButton -= PlaySoundContinueButton;
        UISoundEvents.SoundGeneralButton -= PlaySoundGeneralButton;
        UISoundEvents.SoundStartGameButton -= PlaySoundStartGameButton;
        UISoundEvents.SoundStartGameButton -= StopCitySoundsLoop;
        UISoundEvents.SoundStartGameButton -= stopAllAmbientSounds;
        UISoundEvents.SoundStartGameButton -= MakeCarLoopLoud;

        InGameUIHandler.OnPause -= TogglePause;
    }

    void Update()
    {
        if (logPlayerY) Debug.Log("PlayerY: " + player.position.y);

        if (playCarLoop && playSounds)
        {
            float newPitch = (player.position.y - defaultLevitateY + pitchOffset) * (1 - pitchDamping) + 1;
            if (carLoop.pitch != newPitch) carLoop.pitch = newPitch;
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



        if (knoardPlayCarPassingBy)
        {
            knoardPlayCarPassingBy = false;
            ambientSounds[0].Play();
        }
        if (knoardPlaySirens)
        {
            knoardPlaySirens = false;
            ambientSounds[1].Play();
        }
        if (knoardPlayCitySounds)
        {
            knoardPlayCitySounds = false;
            citySounds.Play();
        }



        if (knoardPlayContinueButton)
        {
            knoardPlayContinueButton = false;
            continueButtonSound.Play();
        }
        if (knoardPlayGeneralButton)
        {
            knoardPlayGeneralButton = false;
            generalButtonSound.Play();
        }
        if (knoardPlayPauseButton)
        {
            knoardPlayPauseButton = false;
            pauseButtonSound.Play();
        }
        if (knoardPlayStartGame)
        {
            knoardPlayStartGame = false;
            startGameButtonSound.Play();
        }
        if (knoardPlayFanFare)
        {
            knoardPlayFanFare = false;
            fanFareSound.Play();
        }
        if (knoardPlayScoreCount)
        {
            knoardPlayScoreCount = false;
            scoreCountSound.Play();
        }
    }

    private void PlayAmbientSound()
    {
        if (!ambientSoundsActive || !playAmbientSounds || !playSounds || ambientSounds.Length < 1) return;

        if (delayLeft > 0)
        {
            delayLeft--;
            return;
        }
        delayLeft = ambientSoundDelaySec;
        if (Random.value < ambientSoundChancePerDelay)
        {
            ambientSounds[Random.Range(0, ambientSounds.Length - 1)].Play();
        }
    }

    private void stopAllAmbientSounds()
    {
        foreach (AudioSource source in ambientSounds)
        {
            source.Stop();
        }
    }

    private void PlayGameOverSound()
    {
        if (playGameOverSound && playSounds) gameOverSound.Play();
    }

    private void StopCarLoop()
    {
        carLoop.Stop();
    }

    private void PlayCarLoop()
    {
        if (playCarLoop && playSounds) carLoop.Play();
    }

    private void MakeCarLoopSilent()
    {
        carLoop.volume = carLoopWithSilencer;
    }

    private void MakeCarLoopLoud()
    {
        carLoop.volume = 1;
    }

    private void StopCitySoundsLoop()
    {
        citySounds.Stop();
        ambientSoundsActive = false;
    }

    private void PlayCitySoundsLoop()
    {
        if (!carLoop.isPlaying && playCitySounds && playSounds)
        {
            citySounds.Play();
            ambientSoundsActive = true;
        }
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

    public void TogglePause()
    {
        if (playSounds)
        {
            StopCarLoop();
            PlaySoundPauseButton();
            playSounds = false;
        }
        else
        {
            playSounds = true;
            PlayCarLoop();
            PlaySoundContinueButton();
        }
    }
}
