using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    public abstract float HitPoints { get; set; }

    protected abstract void Initialize();

    protected abstract void UpdateState();
 
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateState();
    }
}
