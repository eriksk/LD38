

using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float Time = 1f;

    void Start()
    {
        Destroy(gameObject, Time);
    }
}