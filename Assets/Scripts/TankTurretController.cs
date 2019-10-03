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
    private float mouseHorizontalClamped;
    private float mouseVerticalTurretBase;
    private float mouseVerticalTurretBaseClamped;

    [SerializeField]
    private int turretBodyHorizontalClampRange = 150;
    [SerializeField]
    private float turrentBarrelVerticalClampRange = 37.5f;

    private void Update()
    {
        // Get the vertical and horizontal input's from mouse and clamp their values to a certain min-max range.
        mouseHorizontal += Input.GetAxis("Mouse X");
        mouseHorizontalClamped = Mathf.Clamp(mouseHorizontal, -turretBodyHorizontalClampRange, turretBodyHorizontalClampRange);

        mouseVerticalTurretBase += Input.GetAxis("Mouse Y");
        mouseVerticalTurretBaseClamped = Mathf.Clamp(mouseVerticalTurretBase, -turrentBarrelVerticalClampRange, turrentBarrelVerticalClampRange);
    }

    private void FixedUpdate()
    {
        // Rotate the turret body around it's local position.
        transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, mouseHorizontalClamped, 0), turretRotationSpeed);
        // Rotate the turret barrel up and down using a empty gameobject base.
        turretBarrelBase.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(mouseVerticalTurretBaseClamped, 0, 0), barrelRotationSpeed);
    }
}