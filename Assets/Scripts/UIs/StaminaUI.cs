using UnityEngine;
using UnityEngine.UI;


public class StaminaUI : MonoBehaviour
{
    public Image fill;
    public Stamina player_stamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();

    }

    void UIUpdate()
    {
        if (!player_stamina)
            return;

        var ref_stamina = player_stamina.GetStamina();
        var cur_sta = ref_stamina.Item1;
        var max_sta = ref_stamina.Item2;

        fill.fillAmount = cur_sta / max_sta;
    }
}
