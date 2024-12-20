using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandling : MonoBehaviour
{
    public Weapon currentWeapon;
    public Transform weaponSpot;
    public float bufferTime;
    public float expireTime;
    float bufferTimer;
    float expireTimer;
    int attack_index;

    public void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    public void OnAttack()
    {

    }

    void ExpireTimer()
    {

    }

    void BufferTimer()
    {

    }
}
