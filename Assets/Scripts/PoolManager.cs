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
    private int ammoPoolAmount = 50;

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
    // Return ammo from the stack.
    public GameObject PopAmmo()
    {
        return ammoStack.Pop();
    }
    // Push new ammo to the stack.
    public void PushAmmo(GameObject ammo)
    {
        ammoStack.Push(ammo);
    }
}
