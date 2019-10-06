using System.Collections.Generic;
using UnityEngine;

public class AmmoPoolManager : MonoBehaviour
{
    // Implement a static property for accessing this manager globally (singleton pattern).
    public static AmmoPoolManager Instance { get; private set; }

    [SerializeField]
    private GameObject heavyProjectile = default;
    private Stack<GameObject> heavyAmmoStack;
    [SerializeField]
    private Transform heavyAmmoParent = default;
    [SerializeField]
    private int heavyAmmoPoolAmount = 50;

    [SerializeField]
    private GameObject lightProjectile = default;
    private Stack<GameObject> lightAmmoStack;
    [SerializeField]
    private Transform lightAmmoParent = default;
    [SerializeField]
    private int lightAmmoPoolAmount = 100;

    private void Awake()
    {
        // Ensure that this instance of this script is the only one.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Initialize the ammo pools.
        // Heavy ammo.
        heavyAmmoStack = new Stack<GameObject>();
        for (int i = 0; i < heavyAmmoPoolAmount; i++)
        {
            GameObject ammo = Instantiate(heavyProjectile);
            ammo.transform.SetParent(heavyAmmoParent);
            ammo.SetActive(false);
            heavyAmmoStack.Push(ammo);
        }
        // Light ammo.
        lightAmmoStack = new Stack<GameObject>();
        for (int i = 0; i < lightAmmoPoolAmount; i++)
        {
            GameObject ammo = Instantiate(lightProjectile);
            ammo.transform.SetParent(lightAmmoParent);
            ammo.SetActive(false);
            lightAmmoStack.Push(ammo);
        }
    }
    // Return ammo from the stack.
    public GameObject PopHeavyAmmo()
    {
        return heavyAmmoStack.Pop();
    }
    // Push new ammo to the stack.
    public void PushHeavyAmmo(GameObject ammo)
    {
        heavyAmmoStack.Push(ammo);
    }
    /*
    public GameObject PopLightAmmo()
    {
        return lightAmmoStack.Pop();
    }

    public void PushLightAmmo(GameObject ammo)
    {
        lightAmmoStack.Push(ammo);
    }
    */
}