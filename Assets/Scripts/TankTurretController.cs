using UnityEngine;

public class TankTurretController : MonoBehaviour
{
    [SerializeField]
    private Transform turretBarrelBase = default;

    [SerializeField] [Range(0, 1.0f)]
    private float turretRotationSpeed = 0.6f;
    [SerializeField] [Range(0, 1.0f)]
    private float barrelRotationSpeed = 0.5f;

    private float mouseHorizontal;
    private float mouseHorizontalClamp;
    private float mouseVertical;
    private float mouseVerticalClamp;

    [SerializeField]
    private int horizontalClampRange = 150;
    [SerializeField]
    private int verticalClampRange = 60;


    private void Update()
    {
        // Get the vertical and horizontal input's from mouse and clamp their values to a certain min-max range.
        mouseHorizontal += Input.GetAxis("Mouse X");
        mouseHorizontalClamp = Mathf.Clamp(mouseHorizontal, -horizontalClampRange, horizontalClampRange);

        mouseVertical += Input.GetAxis("Mouse Y");
        mouseVerticalClamp = Mathf.Clamp(mouseVertical, -verticalClampRange, 0);
    }

    private void FixedUpdate()
    {
        // Rotate the turret body around it's local position.
        transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, mouseHorizontalClamp, 0), turretRotationSpeed);
        // Rotate the turret barrel up and down using a empty gameobject base.
        turretBarrelBase.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(mouseVerticalClamp, 0, 0), barrelRotationSpeed);
    }
}