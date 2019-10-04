using UnityEngine;

public class TankController : MonoBehaviour
{
    private Rigidbody rigidBody;

    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float rotationSpeed = 1;

    private float verticalMove;

    private bool pressingE;
    private bool pressingQ;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        verticalMove = Input.GetAxis("Vertical") * moveSpeed;

        pressingE = Input.GetKey(KeyCode.E);
        pressingQ = Input.GetKey(KeyCode.Q);
    }

    private void FixedUpdate()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, verticalMove));

        if (pressingE)
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0), Space.Self);
        }
        else if (pressingQ)
        {
            transform.Rotate(new Vector3(0, -rotationSpeed, 0), Space.Self);
        }
    }
}
