using UnityEngine;

public class TankTurretController : MonoBehaviour
{
    [SerializeField]
    private GameObject turretBarrel;

    [SerializeField] [Range(0, 1.0f)]
    private float turretRotationSpeed = 0.6f;
    private float mouseHorizontal;
    private float mouseHorizontalClamped;

    private void Awake()
    {

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
         mouseHorizontal += Input.GetAxis("Mouse X");
         mouseHorizontalClamped = Mathf.Clamp(mouseHorizontal, -150, 150);
    }

    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, mouseHorizontalClamped, 0), turretRotationSpeed);
    }
}