using TMPro;
using UnityEngine;

public class UIHeightAndVelocity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private Transform drone;
    [SerializeField] private Transform terrain;
    private void Update()
    {
        heightText.text = "Height : " + Mathf.Round(drone.position.y - terrain.position.y) + "m";
        velocityText.text = "Velocity : " + drone.GetComponent<DronerController>().GetMoveVelocity() + "km/h";
    }
}
