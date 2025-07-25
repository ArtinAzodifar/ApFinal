using UnityEngine;

public class tmep2 : MonoBehaviour
{
    private float _startPos, _length;
    [SerializeField] private float parallaxEffect = 0.5f;

    public Transform cam;

    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    void FixedUpdate()
    {
        if (cam == null) return;

        float camX = cam.position.x;
        float distance = camX * parallaxEffect;
        float movement = camX * (1 - parallaxEffect);

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