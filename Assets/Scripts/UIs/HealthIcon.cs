using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour
{
    public Image Fill;

    public void SetState(bool value)
    {
        Fill.enabled = value;
    }
}
