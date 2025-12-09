using UnityEngine;

public class playerAnimator : BasicBehaviour
{
    private Animator animator = null;

    protected override void OnStart()
    {
        base.OnStart();
        animator = GetComponent<Animator>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        float horizontalkey = Input.GetAxis("Horizontal");
       

        if (horizontalkey > 0f)
        {
            transform.localScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
            animator.SetBool("walk", true);
        }
        else if (horizontalkey < 0f)
        {
            transform.localScale = new Vector3((float)-0.3, (float)0.3, (float)0.3);
            animator.SetBool("walk", true);
        }
        else
        {
           
            animator.SetBool("walk",false);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}