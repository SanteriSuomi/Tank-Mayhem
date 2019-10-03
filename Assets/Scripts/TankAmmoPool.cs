using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TankAmmoPool : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile = default;
    private List<GameObject> ammoList;

    [SerializeField]
    private int amountToPool = 20;

    private void Start()
    {
        ammoList = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject ammo = Instantiate(projectile);
            ammo.SetActive(false);
            ammoList.Add(ammo);
        }
    }

    public GameObject GetProjectile()
    {
        GameObject firstInactive = ammoList.Where(p => !p.activeSelf).FirstOrDefault();
        return firstInactive;
    }
}
