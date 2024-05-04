﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [Space, Header("Audio Properties")]
    [SerializeField, Min(0f)] Vector2 _randomPitchRange = new Vector2(0.5f, 1f); 

    [Space, Header("Clips")]
    [SerializeField] SoundClip _gunCock;

    public enum Sound
    {
    }

    ISoundEffectClip GetSoundEffectClip(Sound sound)
    {
        switch (sound)
        {
        }

        return null;
    }

    public float PlaySoundAtPosition(Vector2 position, Sound sound, bool isRandomPitch = false, bool isAffectedByTimeScale = true)
    {
        GameObject audioParent = CreateSoundObject();
        AudioSource sauce = CreateDaSauce();
        sauce.Play();
        Destroy(audioParent, sauce.clip.length);
        return sauce.clip.length;

        GameObject CreateSoundObject()
        {
            GameObject parent = new GameObject("Sound: " + sound.ToString());
            parent.transform.position = position;
            return parent;
        }
        AudioSource CreateDaSauce()
        {
            ISoundEffectClip sfxClip = GetSoundEffectClip(sound);
            AudioSource source = audioParent.AddComponent<AudioSource>();
            source.clip = sfxClip.GetAudioClip();
            source.volume = sfxClip.GetVolume();
            if (isRandomPitch)
                source.pitch = UnityEngine.Random.Range(_randomPitchRange.x, _randomPitchRange.y);
            if (isAffectedByTimeScale)
                source.pitch *= Time.timeScale;
            return source;
        }
    }
}

public interface ISoundEffectClip
{
    AudioClip GetAudioClip();
    float GetVolume();
}

[Serializable]
public class SoundClip : ISoundEffectClip
{
    [SerializeField] AudioClip _clip;
    [SerializeField, Range(0,1f)] float _volume = 1f;

    public AudioClip GetAudioClip()
    {
        return _clip;
    }

    public float GetVolume()
    {
        return _volume;
    }
}

[Serializable]
public class RandomSoundClip : ISoundEffectClip
{
    [SerializeField] List<AudioClip> _clips;
    [SerializeField, Range(0,1f)] float _volume = 1f;

    public AudioClip GetAudioClip()
    {
        int index = UnityEngine.Random.Range(0, _clips.Count);
        return _clips[index];
    }

    public float GetVolume()
    {
        return _volume;
    }
}
