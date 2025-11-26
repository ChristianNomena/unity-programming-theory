using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float awaitTimePerProjectile = 0.5f;
    [SerializeField] private Camera cam;

    // Contrôlent où le projectile apparaît en avant et en hauteur par rapport au joueur
    [SerializeField] private float projectileSpawnDistance = 1.0f;
    [SerializeField] private float projectileSpawnHeight = 0.5f;

    private Animator animator;

    private float horizontalInput;
    private float verticalInput;
    private Transform cameraTransform;
    private bool canShoot = true;

    void Start()
    {
        animator = GetComponent<Animator>();

        cameraTransform = cam.transform;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if ((projectilePrefab != null) && canShoot)
            {
                SpawnProjectile();
            }
        }

        Vector2 moveValue = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        horizontalInput = moveValue.x;
        verticalInput = moveValue.y;
    }

    void FixedUpdate()
    {
        Move();
        FollowMouseCursor();
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(awaitTimePerProjectile);
        canShoot = true;
    }

    // ABSTRACTION
    private void SpawnProjectile()
    {
        Vector3 spawnPos = transform.position + transform.forward * projectileSpawnDistance + Vector3.up * projectileSpawnHeight;
        Instantiate(projectilePrefab, spawnPos, transform.rotation);
        StartCoroutine(ShootCooldown());
    }

    // ABSTRACTION
    private void FollowMouseCursor()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 lookDir = hitPoint - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.000001f)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }

    // ABSTRACTION
    private void Move()
    {
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDirection = camRight * horizontalInput + camForward * verticalInput;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        Animate(moveDirection);

        transform.Translate(speed * Time.fixedDeltaTime * moveDirection, Space.World);
    }

    // ABSTRACTION
    private void Animate(Vector3 moveDirection)
    {
        bool isMoving = moveDirection.sqrMagnitude > 0.0001f;
        if (isMoving)
        {
            animator.SetFloat("Speed_f", moveDirection.magnitude);
        }
        else
        {
            animator.SetFloat("Speed_f", 0f);
        }
    }
}
