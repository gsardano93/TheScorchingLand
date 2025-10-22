
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private float volume = 1f;
    [SerializeField] AudioClipSO audioClipRefSO;
    AudioSource audioSource;
    AudioClip defaultClip;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        defaultClip = audioSource.clip;
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * volume);
    }
    public AudioClipSO GetAudioClipSO()
    {
        return audioClipRefSO;
    }
    public void ChangeMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void RestoreMusic()
    {
        audioSource.clip = defaultClip;
        audioSource.Play();
    }
}
