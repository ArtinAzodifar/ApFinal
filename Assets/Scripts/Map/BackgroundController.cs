using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float _startPos, _length;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallexEffect;
    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void FixedUpdate()
    {
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
