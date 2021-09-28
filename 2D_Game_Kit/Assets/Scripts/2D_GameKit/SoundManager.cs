using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioSource soundSource;
    private List<AudioLink> audioLinks = new List<AudioLink>();

    private void Reset()
    {
        soundSource = GetComponent<AudioSource>();

    }

    // Register a audio clip for a sound
    public int RegisterSound(AudioClip clip)
    {
        AudioLink newLink = new AudioLink(audioLinks.Count, clip);
        if(audioLinks.Contains(newLink))
        {
            int index = audioLinks.IndexOf(newLink);
            return audioLinks[index].linkID;
        }

        audioLinks.Add(newLink);
        return audioLinks.Count - 1;
    }
    /// <summary>
    /// Play the sound associated with the index
    /// </summary>
    /// <param name="index">index of the sound to play</param>
    public void PlaySound(int index)
    {
        // Ensure that there is a sound in the sounds list
        if(audioLinks.Count > index)
        {
            // Ensure that the sound to play is not null
            if(audioLinks[index].audioClip != null)
            {
                soundSource.Stop();
                soundSource.clip = audioLinks[index].audioClip;
                soundSource.Play();
            }
        }
    }

    public void DeregisterSound()
    {

    }
}

internal struct AudioLink
{
    public int linkID;
    public AudioClip audioClip;

    public AudioLink(int id, AudioClip audioClip)
    {
        this.linkID = id;
        this.audioClip = audioClip ?? throw new ArgumentNullException(nameof(audioClip));
    }

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null))
        {
            return false;
        }
        else if ((!this.GetType().Equals(obj.GetType())))
        {
            return false;
        }
        else
        {
            //they are both Audio Links so see if they are the same
            AudioLink al = (AudioLink)obj;
            return audioClip == al.audioClip;
        }
    }
}
