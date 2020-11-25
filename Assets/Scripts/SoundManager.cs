using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource carLoop;
    [SerializeField] private bool playCarLoop = true;
    [SerializeField] private float volumeCarLoop = 1;

    private List<PitchChange> activePitchChanges = new List<PitchChange>();

    private class PitchChange
    {
        private float seconds;
        private float secondsLeft;

        private float ogPitch;
        private float maxPitch;
        private float curPitch;

        private int delta = 1;
        private int stage = 1;
        private bool isDone = false;

        AudioSource source;

        public PitchChange(AudioSource source, float seconds, float maxPitch)
        {
            this.seconds = seconds;
            this.maxPitch = maxPitch;
            this.source = source;

            secondsLeft = seconds;
            ogPitch = source.pitch;
            curPitch = ogPitch;
        }

        public void UpdatePitch(float dTime)
        {
            if (isDone) return;

            if (stage == 1) curPitch = ogPitch + (maxPitch - ogPitch) * ((1 - secondsLeft / seconds) * 2);
            else curPitch = ogPitch + (maxPitch - ogPitch) * ((secondsLeft / seconds) * 2);

            //curPitch = (stage == 1) ? ogPitch + (maxPitch - ogPitch) * (1 - secondsLeft / seconds * 2) : ogPitch + (maxPitch - ogPitch) * (secondsLeft / seconds * 2);

            source.pitch = curPitch;
            secondsLeft -= dTime;

            if (secondsLeft <= seconds / 2)
            {
                stage = 2;
            }

            if (secondsLeft < 0)
            {
                curPitch = ogPitch;
                source.pitch = curPitch;
                isDone = true;
            }
        }

        public bool IsDone() => isDone;
    }

    // Start is called before the first frame update
    void Start()
    {
        carLoop.maxDistance = -1;
        carLoop.loop = true;

        if (playCarLoop) carLoop.Play();


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < activePitchChanges.Count - 1; i++)
        {
            PitchChange pitchChange = activePitchChanges[i];
            pitchChange.UpdatePitch(Time.deltaTime);
            if (pitchChange.IsDone())
            {
                activePitchChanges.Remove(pitchChange);
            }
        }
    }

    public void PitchIncrease(AudioSource source, float seconds, float maxPitch)
    {
        activePitchChanges.Add(new PitchChange(source, seconds, maxPitch));
    }
}
