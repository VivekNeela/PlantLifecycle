using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private PlantLifecycleManager plm;
        public Action OnAnimEnd;

        private void OnValidate()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }

        private void Start()
        {
            plm = PlantLifecycleManager.Instance;
        }

        //not using anymore...
        // public void DigHole(GroundState groundState)   //this function is on the animation as an anim event...
        // {
        //     plm.SetGroundState(groundState);
        // }

        public void SetTrigger(string name) => animator.SetTrigger(name);

        public void OnAnimationEnded()   //on animation event, in the end...
        {
            Debug.Log("Digging Animation Ended...");
            OnAnimEnd?.Invoke();

            transform.DOMoveX(-15, 2).OnComplete(() =>
            {
                plm.MoveSeedBag();
            });
        }


        public void HeapSoil(float soilScale)  //on animation event...
        {
            plm.ScaleSoil(soilScale);
        }

        public void ScaleHole(float holeScale)  //on animation event...
        {
            plm.ScaleHole(holeScale);
        }


    }
}
