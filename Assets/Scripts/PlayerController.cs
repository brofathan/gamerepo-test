using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed;
        private bool isMoving;
        private Vector2 input;
        private Animator animator;

        public LayerMask solidObjectsLayer;
        public LayerMask interactableLayer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void HandleUpdate()
        {
            if (!isMoving)
            {
                input = Vector2.zero;

                if (Keyboard.current != null)
                {
                    if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
                    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x -= 1f;
                    if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y += 1f;
                    if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y -= 1f;
                }

                if (input.x != 0f) input.y = 0f; // Prevent diagonal movement

                if (input != Vector2.zero)
                {
                    animator.SetFloat("MoveX", input.x);
                    animator.SetFloat("MoveY", input.y);

                    var targetPos = transform.position;
                    targetPos.x += (input.x * 1f);
                    targetPos.y += (input.y * 1f);

                    if (isWalkable(targetPos))
                    {
                        // input.Normalize();
                        // transform.Translate(input * moveSpeed * Time.deltaTime);
                        StartCoroutine(Move(targetPos));
                    }
                }
            }

            animator.SetBool("isMoving", isMoving);

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                Interact();
            }
        }

        void Interact()
        {
            var facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
            var interactPos = transform.position + facingDir;

            // Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);
            var collider = Physics2D.OverlapCircle(interactPos, 0.1f, interactableLayer);
            if (collider != null)
            {
                collider.GetComponent<Interactable>()?.Interact();
            }
        }

        IEnumerator Move(Vector3 targetPos)
        {
            isMoving = true;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            isMoving = false;
        }

        private bool isWalkable(Vector3 targetPos)
        {
            if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}