using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float offsetY = 2f;

    [Header("Control Dialogue")]
    public bool followPlayer = true;

    void Start()
    {
        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x,
                target.position.y + offsetY,
                -10f
            );
        }
    }

    void LateUpdate()
    {
        if (!followPlayer)
            return;

        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x,
                target.position.y + offsetY,
                -10f
            );
        }
    }
}