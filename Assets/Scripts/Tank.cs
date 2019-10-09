using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    protected abstract void Initialize();

    protected abstract void StartState();

    protected abstract void UpdateState();
 
    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartState();
    }

    private void Update()
    {
        UpdateState();
    }
}
