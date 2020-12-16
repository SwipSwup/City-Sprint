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

    [Space]
    [Header("Ambient Sounds")]
    [SerializeField] private bool playAmbientSounds = true;
    [SerializeField] private float ambientSoundDelaySec = 5f;
    private float delayLeft;
    [SerializeField] private float ambientSoundChancePerDelay = 0.2f;
    public AudioSource[] ambientSounds;

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

        if (playCarLoop) carLoop.Play();
        InvokeRepeating("PlayAmbientSound", 1f, 1f);

        Player.OnCollectCoin += PlayCoinCollectSound;
        Player.OnGameOver += PlayGameOverSound;

        UISoundEvents.SoundContinueButton += PlaySoundContinueButton;
        UISoundEvents.SoundGeneralButton += PlaySoundGeneralButton;
        UISoundEvents.SoundPauseButton += PlaySoundPauseButton;
        UISoundEvents.SoundStartGameButton += PlaySoundStartGameButton;
    }

    private void OnDestroy()
    {
        Player.OnCollectCoin -= PlayCoinCollectSound;
        Player.OnGameOver -= PlayGameOverSound;

        UISoundEvents.SoundContinueButton -= PlaySoundContinueButton;
        UISoundEvents.SoundGeneralButton -= PlaySoundGeneralButton;
        UISoundEvents.SoundPauseButton -= PlaySoundPauseButton;
        UISoundEvents.SoundStartGameButton -= PlaySoundStartGameButton;
    }

    void Update()
    {
        if (playCarLoop)
        {
            float newPitch = ((player.position.y + pitchOffset) / defaultLevitateY) * (1 - pitchDamping) + pitchDamping;
            if (carLoop.pitch == newPitch) return;

            carLoop.pitch = newPitch;
            Debug.Log("pitchChange");
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
        if (playAmbientSounds && ambientSounds.Length > 0 && Random.value >  1 - ambientSoundChancePerDelay)
        {
            ambientSounds[Random.Range(0, ambientSounds.Length - 1)].Play();
        }
    }

    private void PlayGameOverSound()
    {
        if (playGameOverSound) gameOverSound.Play();
    }

    private void PlayCoinCollectSound()
    {
        if (playCoinCollectSound) coinCollectSound.Play();
    }

    public void PlaySoundContinueButton()
    {
        if (playButtonSounds) continueButtonSound.Play();
    }
    public void PlaySoundGeneralButton()
    {
        if (playButtonSounds) generalButtonSound.Play();
    }
    public void PlaySoundPauseButton()
    {
        if (playButtonSounds) pauseButtonSound.Play();
    }
    public void PlaySoundStartGameButton()
    {
        if (playButtonSounds) startGameButtonSound.Play();
    }
}
