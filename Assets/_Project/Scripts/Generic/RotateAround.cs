using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform Target;
    public float Speed = 1f;

    private float _rotation;
    private float _distance;

    void Start()
    {
        if(Target == null) return;

        _distance = Vector3.Distance(transform.position, Target.position);

    }

    void Update()
    {
        if(Target == null) return;

        _rotation += Speed * Time.deltaTime;

        var target = Quaternion.Euler(0f, _rotation, 0f) * new Vector3(_distance, 0f, 0f);
        target.y = transform.position.y;
        transform.position = target;
    }
}