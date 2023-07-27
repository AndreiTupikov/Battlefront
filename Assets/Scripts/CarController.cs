using UnityEngine;

public class CarController : TransportController
{
    [SerializeField] private Animator[] wheelsAnimators;
    [SerializeField] private Transform turret;
    private Transform[] forwardWhels;
    private bool isMoving;

    private void Start()
    {
        forwardWhels = new Transform[2];
        for (int i = 0; i < forwardWhels.Length; i++) forwardWhels[i] = wheelsAnimators[i].GetComponent<Transform>();
    }

    private void Update()
    {
        TurnToTarget(turret);
        if (target != null)
        {
            if (moveDirection != Vector2.zero)
            {
                Move();
                if (!isMoving) WheelsControl(true);
            }
            else
            {
                if (gunReady) Shooting(turret);
                if (isMoving) WheelsControl(false);
            }
        }
        else if(isMoving) WheelsControl(false);
    }

    private void Move()
    {
        float newAngle = Vector2.SignedAngle(Vector2.up, moveDirection);
        float oldAngle = transform.rotation.eulerAngles.z;
        float rotateAngle = newAngle - oldAngle;
        if (rotateAngle > 180) rotateAngle -= 360;
        else if (rotateAngle < -180) rotateAngle += 360;
        TurnForwardWheels(rotateAngle, oldAngle);
        transform.Rotate(rotateAngle * Time.deltaTime * new Vector3(0, 0, 1));
        transform.Translate(Time.deltaTime * movingSpeed * new Vector3(0, 1, 0));
    }

    private void TurnForwardWheels(float rotateAngle, float carBodyAngle)
    {
        if (rotateAngle > 15) rotateAngle = 15;
        else if (rotateAngle < -15) rotateAngle = -15;
        rotateAngle += carBodyAngle;
        foreach (Transform t in forwardWhels) t.rotation = Quaternion.Euler(0, 0, rotateAngle);
    }

    private void WheelsControl(bool moving)
    {
        isMoving = moving;
        foreach (var wheel in wheelsAnimators)
        {
            wheel.SetBool("Moving", moving);
        }
    }
}
