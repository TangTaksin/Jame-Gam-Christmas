using UnityEngine;

public class LayerTriggerCheck : MonoBehaviour
{
    public LayerMask maskToCheck;
    public GameObject detectedGameObject;
    bool detected;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == maskToCheck)
        {
            detected = true;
            detectedGameObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == maskToCheck)
        {
            detected = false;
        }
    }
}
