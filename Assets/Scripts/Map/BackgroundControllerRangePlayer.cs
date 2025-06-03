using Unity.Cinemachine;
using UnityEngine;

public class BackgroundControllerRangePlayer : MonoBehaviour
{
    private float _startPos, _length;
    [SerializeField] CinemachineCamera cam;
    [SerializeField] private float parallexEffect;

    public Transform player1Transform;
    public CinemachineCamera vcam1;

    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;

        player1Transform = GameObject.FindWithTag("Player2").transform;
        cam = player1Transform.Find("vcam2").GetComponent<CinemachineCamera>();
    }

    void FixedUpdate()
    {
        if (cam == null) return;

        float distance = cam.transform.position.x * parallexEffect;
        float movement = cam.transform.position.x * (1 - parallexEffect);

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