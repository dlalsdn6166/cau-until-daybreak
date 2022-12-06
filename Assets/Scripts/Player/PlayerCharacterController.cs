using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PlayerCharacterController : MonoBehaviour
{
    public Camera playerCamera;
    public CharacterController controller;

    public float hp = 1000;
    public float speed = 6;
    public float jumpForce = 8;
    public float RotationSpeed = 5;

    public float interactRange = 4;

    private Vector3 movement = Vector3.zero;
    private float pitch = 0;

    public bool Interactable { get; private set; }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == enemyMask)
        {
            hp -= collision.impulse.magnitude;
            if (hp < 0)
                Dead();
        }
    }

    private void Dead() => GameManager.Instance.playerdead();

    private int enemyMask;
    private void Awake()
    {
        enemyMask = LayerMask.NameToLayer("AI");
    }

    void Update()
    {
        if (GameManager.Instance.Paused)
            return;

        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = transform.TransformDirection(input);
        if (controller.isGrounded)
        {
            movement = input * speed;
            if (Input.GetButton("Jump"))
                movement.y += jumpForce;
        }
        else
        {
            // TODO player air move
        }

        transform.Rotate(new Vector3(0f, (Input.GetAxis("Mouse X") * RotationSpeed), 0f), Space.Self);

        pitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        movement += Physics.gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Physics.Raycast(ray, out RaycastHit hit, interactRange);
        var interactable = hit.collider?.GetComponent<Interactable>();
        Interactable = interactable;
        if (interactable)
            if (interactable.IsValid && Input.GetKeyDown(KeyCode.E))
                interactable.Interact();
    }
}

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();
    public abstract bool IsValid { get; protected set; }
}