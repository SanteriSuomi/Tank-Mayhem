using System.Collections;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource tankAudio;

    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float rotationSpeed = 1;
    [SerializeField]
    private float shiftMultiplier = 1.5f;
    [SerializeField]
    private float audioFadeTime = 0.001f;

    private float tankAudioVolumeStart;

    private float verticalMove;
    private bool pressingE;
    private bool pressingQ;
    private bool pressingShift;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        tankAudio = GetComponent<AudioSource>();
        tankAudioVolumeStart = tankAudio.volume;
    }

    private void Update()
    {
        verticalMove = Input.GetAxis("Vertical") * moveSpeed;
        pressingE = Input.GetKey(KeyCode.E);
        pressingQ = Input.GetKey(KeyCode.Q);
        pressingShift = Input.GetKey(KeyCode.LeftShift);
    }

    private void FixedUpdate()
    {
        if (pressingE)
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0), Space.Self);
        }
        else if (pressingQ)
        {
            transform.Rotate(new Vector3(0, -rotationSpeed, 0), Space.Self);
        }

        if (pressingShift)
        {
            MoveForward(verticalMove * shiftMultiplier);
        }
        else
        {
            MoveForward(verticalMove);
        }

        if (pressingE || pressingQ || pressingShift || verticalMove >= 0.75 || verticalMove <= -0.75)
        {
            if (!tankAudio.isPlaying)
            {
                tankAudio.Play();
            }
            else
            {
                tankAudio.volume = tankAudioVolumeStart;
            }
        }
        else
        {
            StartCoroutine(FadeOutVolume());
        }
    }

    private IEnumerator FadeOutVolume()
    {
        while (tankAudio.volume > 0)
        {
            tankAudio.volume -= audioFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void MoveForward(float force)
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, force));
    }
}