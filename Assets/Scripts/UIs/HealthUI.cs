using UnityEngine;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public List<HealthIcon> Icons = new List<HealthIcon>();
    public GameObject IconPrefab;
    public Transform Container;
    public Health player_health;

    private void Start()
    {
        if (player_health)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        var cur_hp = player_health.current_health;
        var max_hp = player_health.max_health;

        for (int i = 1; i < max_hp; ++i)
        {
            if (Icons.Count < max_hp)
            {
                var new_icon = GameObject.Instantiate(IconPrefab);
                new_icon.transform.SetParent(Container);
                Icons.Add(new_icon.GetComponent<HealthIcon>());
            }

            if (i <= cur_hp)
                Icons[i].SetState(true);
            else
                Icons[i].SetState(false);
        }       
    }
}
