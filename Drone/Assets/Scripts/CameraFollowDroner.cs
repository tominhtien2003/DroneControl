using UnityEngine;

public class CameraFollowDroner : MonoBehaviour
{
    [SerializeField] private Transform droner;

    private void LateUpdate()
    {
        CameraFollowDroneLook();
    }
    public void CameraFollowDroneLook()
    {
        Vector3 posDroner = droner.position;

        transform.position = new Vector3(posDroner.x, transform.position.y, posDroner.x);

        Vector3 eulerAnge = droner.eulerAngles;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, eulerAnge.y, transform.eulerAngles.z);
    }
}
