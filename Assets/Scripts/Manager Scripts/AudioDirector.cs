using UnityEngine;
using System.Collections;

public class AudioDirector : MonoBehaviour
{
    #region Variables
    public AudioSource soundEffectsSource;
    public AudioSource loopedSoundEffectsSource;
    public AudioSource backgroundMusicSource;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;
    #endregion

    public void PlayAudio (AudioSource _channel, AudioClip _clip)
    {
        _channel.clip = _clip;
        if (!_channel.isPlaying) { _channel.Play(); }
    }

    public void RandomSoundEffects(AudioSource _channel, params AudioClip[] _clips)
    {
        int randomIndex = Random.Range(0, _clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        _channel.pitch = randomPitch;
        _channel.clip = _clips[randomIndex];
        _channel.Play();
    }

    public void StopAudio(AudioSource _channel)
    {
        if (_channel.isPlaying) { _channel.Stop(); }
    }
}
