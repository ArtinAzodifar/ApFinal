using UnityEngine;

public class BackgroundControllerRangePlayer : MonoBehaviour
{
    private float _startPos, _length;
    private Transform cam;

    [SerializeField] private float parallaxEffect = 0.5f;

    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;

        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        if (cam == null) return;

        float distance = cam.position.x * parallaxEffect;
        float movement = cam.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(_startPos + distance, transform.position.y, transform.position.z);

        if (movement > _startPos + _length)
        {
            _startPos += _length;
        }
        else if (movement < _startPos - _length)
        {
            _startPos -= _length;
        }
    }
}