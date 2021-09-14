using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioSource soundSource;
    private List<KeyValuePair<AudioClip,int>> sounds = new List<KeyValuePair<AudioClip, int>>();

    private void Reset()
    {
        soundSource = GetComponent<AudioSource>();

    }

    // Register a audio clip for a sound
    public int RegisterSound(AudioClip clip)
    {
        
        sounds.Add(clip);
        return sounds.Count - 1;
    }
    /// <summary>
    /// Play the sound associated with the index
    /// </summary>
    /// <param name="index">index of the sound to play</param>
    public void PlaySound(int index)
    {
        // Ensure that there is a sound in the sounds list
        if(sounds.Count > index)
        {
            // Ensure that the sound to play is not null
            if(sounds[index].Key != null)
            {
                soundSource.Stop();
                soundSource.clip = sounds[index].Key;
                soundSource.Play();
            }
        }
    }

    public int DeregisterSound()
    {

    }
}

struct AudioLink
{
    int links;
    AudioClip audioClip;

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            AudioLink al = (AudioLink)obj;
            return audioClip == al.audioClip;
        }
    }
}
