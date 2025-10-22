using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 2f;
    public float offsetY = 2f;  // 카메라가 플레이어 위쪽을 약간 더 보게

    void LateUpdate()
    {
        if (player == null) return;

        // 카메라가 플레이어보다 위로만 따라가도록
        if (player.position.y > transform.position.y - offsetY)
        {
            Vector3 targetPos = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
    }
}
