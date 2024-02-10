using UnityEngine;

public class CarProximitySensorSoundController : MonoBehaviour
{
    public AudioClip farDistanceSound;
    public AudioClip midDistanceSound;
    public AudioClip closeDistanceSound;
    [Range(1f, 5f)]
    public float pitchMultiplier;
    
    public Transform listenerTransform; // Assign the listener's transform in the Inspector
    public float maxDistance = 10f;

    public void PlaySound(float normalizedDistance, AudioSource audioSource)
    {
        PlayDistanceSound(audioSource, normalizedDistance);
    }

    void PlayDistanceSound(AudioSource audioSource, float speed)
    {
        if (audioSource != null)
        {
            float targetPitch = pitchMultiplier * speed;
            audioSource.pitch = targetPitch < 0.8f ? 0.8f : targetPitch;

            if(audioSource.isPlaying && audioSource.clip == midDistanceSound)
                return;

            audioSource.clip = midDistanceSound;
            SetStereoPan(audioSource);
            audioSource.Play();
        }
    }
    
    void SetStereoPan(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.panStereo = GetStereoPan();
        }
    }

    float GetStereoPan()
    {
        if (listenerTransform == null)
        {
            listenerTransform = this.transform;
        }
        
        float stereoPan = Mathf.Clamp01((transform.position.x - listenerTransform.position.x) / maxDistance);
        return (transform.position.z < listenerTransform.position.z) ? -stereoPan : stereoPan;
    }
}
