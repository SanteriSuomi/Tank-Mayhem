using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Implement a static property for accessing this manager globally (singleton pattern).
    public static PoolManager Instance { get; private set; }

    [SerializeField]
    private GameObject projectile = default;
    private Stack<GameObject> ammoStack;

    [SerializeField]
    private Transform ammoParent = default;

    [SerializeField]
    private int ammoPoolAmount = 500;

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
        // Initialize the ammo pool.
        ammoStack = new Stack<GameObject>();
        for (int i = 0; i < ammoPoolAmount; i++)
        {
            GameObject ammo = Instantiate(projectile);
            ammo.transform.SetParent(ammoParent);
            ammo.SetActive(false);
            ammoStack.Push(ammo);
        }
    }

    public GameObject PopAmmo()
    {
        // Return ammo from the stack.
        return ammoStack.Pop();
    }

    public void PushAmmo(GameObject ammo)
    {
        // Push new ammo to the stack.
        ammoStack.Push(ammo);
    }
}