using UnityEngine;
using UnityEngine.InputSystem;

namespace NetDinamica.AppFast
{
    public class PlayAnimations : MonoBehaviour
    {
        Animator anim;
        bool facingRight = true;

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                anim.Play("Idle");
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                anim.Play("Walk");
            }
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                anim.Play("Attack");
            }
            if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                anim.Play("Hurt");
            }
            else if (Keyboard.current.digit9Key.wasPressedThisFrame)
            {
                anim.Play("Die");
            }
            else if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                Flip();
            }
        }

        void Flip()
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
