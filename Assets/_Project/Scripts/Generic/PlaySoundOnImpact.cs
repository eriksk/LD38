

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnImpact : MonoBehaviour
{
    public float Treshold = 1f;
    public AudioClip Clip;

    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        var mag = collision.impulse.magnitude;

        if(mag > Treshold)
        {
            _source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            _source.PlayOneShot(Clip);
        }
    }
}