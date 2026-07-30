using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bg_adudio;
    [SerializeField] internal AudioSource audioPlayer_wl;
    [SerializeField] internal AudioSource audioPlayer_button;
    [SerializeField] internal AudioSource audioSpin_button;
    [SerializeField] private AudioClip[] clips;


    private void Start()
    {
        if (bg_adudio) bg_adudio.Play();

        audioPlayer_button.clip = clips[clips.Length - 1];
        audioSpin_button.clip = clips[clips.Length - 2];
    }

    private bool isForceMuted = false;
    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();

    private IEnumerable<AudioSource> AllSources()
    {
        yield return bg_adudio;
        yield return audioPlayer_wl;
        yield return audioPlayer_button;
        yield return audioSpin_button;
    }

    // Focus-driven — called from BOTH the WebGL/JS OnFocusChanged path and OnApplicationFocus.
    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        foreach (var source in AllSources())
        {
            if (source == null) continue;
            if (forceMute)
            {
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
            else
            {
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
        }
    }

    internal void PlayWLAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "spin":
                index = 0;
                audioPlayer_wl.loop = true;
                break;
            case "win":
                index = 1;
                break;
            case "lose":
                index = 2;
                break;
            case "spinStop":
                index = 3;
                break;
            case "megaWin":
                index = 4;
                break;
        }
        StopWLAaudio();
        audioPlayer_wl.clip = clips[index];
        audioPlayer_wl.Play();

    }
    internal void PlayButtonAudio()
    {
        audioPlayer_button.Play();
    }

    internal void PlaySpinButtonAudio()
    {
        audioSpin_button.Play();
    }

    internal void StopWLAaudio()
    {
        audioPlayer_wl.Stop();
        audioPlayer_wl.loop = false;
    }

    internal void StopButtonAudio() {

        audioPlayer_button.Stop();

    }

    internal void StopBgAudio() {
        bg_adudio.Stop();

    }

    internal void ToggleMute(bool toggle, string type="all") {

        switch (type)
        {
            case "bg":
                bg_adudio.mute = toggle;
                preFocusMuteState[bg_adudio] = toggle;
                break;
            case "button":
                audioPlayer_button.mute = toggle;
                audioSpin_button.mute = toggle;
                preFocusMuteState[audioPlayer_button] = toggle;
                preFocusMuteState[audioSpin_button] = toggle;
                break;
            case "wl":
                audioPlayer_wl.mute = toggle;
                preFocusMuteState[audioPlayer_wl] = toggle;
                break;
            case "all":
                audioPlayer_wl.mute = toggle;
                bg_adudio.mute = toggle;
                audioPlayer_button.mute = toggle;
                audioSpin_button.mute = toggle;
                preFocusMuteState[audioPlayer_wl] = toggle;
                preFocusMuteState[bg_adudio] = toggle;
                preFocusMuteState[audioPlayer_button] = toggle;
                preFocusMuteState[audioSpin_button] = toggle;
                break;
        }
    }

}
