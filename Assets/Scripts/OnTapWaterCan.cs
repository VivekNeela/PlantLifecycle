using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMKOC.PlantLifecycle
{
    public class OnTapWaterCan : OnTapObject
    {

        private PlantLifecycleManager plm;
        public ParticleSystem waterSprinkle;
        // public bool selected;

        private void Start()
        {
            plm = PlantLifecycleManager.Instance;
        }

        public override void OnMouseDown()
        {
            GetComponent<BoxCollider2D>().enabled = false;   //clicking only once...
            transform.DOMoveX(3, 1).OnComplete(() =>
            {
                //after the water can pours water we grow the plant by one growth stage...
                transform.DORotate(new Vector3(0, 0, 45), 0.5f).OnComplete(() =>
                {
                    //pour water...

                    StartCoroutine(PlayParticleAndWait(waterSprinkle, 2, () =>
                    {
                        //grow the plant...
                        plm.GrowPlant(PlantGrowthStage.Small, () =>
                         {
                             //send back gasoline and water can
                             plm.MoveWaterOptions(20, 1, () =>
                             {
                                 //bring the sun...
                                 plm.MoveSun(4, () =>
                                 {
                                     plm.ScaleSunAnim();
                                 });
                             });
                         });

                    }));
                });

            });
        }



        private IEnumerator PlayParticleAndWait(ParticleSystem particleSystem, float waitTime, Action onComplete)
        {
            particleSystem.Play(); // Start the particle effect

            // Wait until the particle system is no longer playing
            while (particleSystem.isPlaying)
            {
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
            onComplete?.Invoke(); // Call the onComplete function after effect ends
        }



        public override void OnPointerDown(PointerEventData eventData) { }

        public override void OnPointerUp(PointerEventData eventData) { }
    }
}
