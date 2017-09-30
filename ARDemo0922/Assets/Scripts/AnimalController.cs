using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    public List<Animator> animators = new List<Animator>();

    public void doBtnWalk() {
        foreach (Animator animator in animators) {
            if (animator.gameObject.activeSelf) {
                animator.SetBool("Runable", false);
            }
        }
    }

    public void doBtnRun()
    {
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.activeSelf)
            {
                animator.SetBool("Runable", true);
            }
        }
    }
}
