using System;
using UnityEngine;

public class UISoundEvents : MonoBehaviour
{
    public void PlaySoundContinueButton() => SoundContinueButton?.Invoke();
    public void PlaySoundGeneralButton() => SoundGeneralButton?.Invoke();
    public void PlaySoundPauseButton() => SoundPauseButton?.Invoke();
    public void PlaySoundStartGameButton() => SoundStartGameButton?.Invoke();

    public static Action SoundContinueButton;
    public static Action SoundGeneralButton;
    public static Action SoundPauseButton;
    public static Action SoundStartGameButton;
}
