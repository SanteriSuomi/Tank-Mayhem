using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    protected abstract float HitPoints { get; set; }

    protected abstract void Initialize();

    protected abstract void UpdateState();

    protected abstract void UpdateStateFixed();
 
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        UpdateStateFixed();
    }
}
