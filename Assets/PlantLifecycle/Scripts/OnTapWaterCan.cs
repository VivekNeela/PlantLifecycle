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
        public bool firstClickDone;

        private void Start()
        {
            plm = PlantLifecycleManager.Instance;
        }

        public override void OnMouseDown()
        {
            // GetComponent<BoxCollider2D>().enabled = false;   //clicking only once...

            transform.DOMoveX(3, 1).OnComplete(() =>
            {
                //after the water can pours water we grow the plant by one growth stage...
                transform.DORotate(new Vector3(0, 0, 45), 0.5f).OnComplete(() =>
                {
                    //pour water...

                    StartCoroutine(PlayParticleAndWait(waterSprinkle, 2, () =>
                    {

                        if (!firstClickDone)
                            WaterPlant(PlantGrowthStage.Small);
                        else
                            WaterPlant(PlantGrowthStage.Big);

                    }));
                });

            });

        }


        void WaterPlant(PlantGrowthStage plantGrowthStage)
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            //grow the plant...
            plm.CameraHandler.ZoomToPlant(() =>
            {

                plm.GrowPlant(plantGrowthStage, () =>
                 {
                     if (!firstClickDone)
                     {
                         plm.plantAnimator.transform.DOScale(0.5f, 1.5f);
                         plm.plantAnimator.transform.DOMoveY(-1.5f, 1.5f).OnComplete(() =>
                         {

                             plm.CameraHandler.ResetCamera();
                         });
                     }
                     //  plm.plantAnimator.transform.DOMoveY(-1.5f, 0.5f);

                     //send back gasoline and water can
                     plm.MoveWaterOptions(20, 1, () =>
                     {

                         //bring the sun...
                         if (!firstClickDone)
                         {
                             //  plm.plantAnimator.transform.DOScale(1.5f, 0.5f);
                             //  plm.plantAnimator.transform.DOMoveY(-1.5f, 0.5f);
                             plm.MoveSun(4, () =>
                             {
                                 plm.ScaleSunAnim();
                                 firstClickDone = true;

                             });
                         }
                         else
                         {
                             //  plm.plantAnimator.transform.DOMoveX(-0.1f, 0.5f);w
                            //  plm.plantAnimator.transform.DOScale(0.8f, 0.5f);
                             plm.plantAnimator.transform.DOMoveY(-1f, 1.75f).OnComplete(() =>
                             {
                                 plm.CameraHandler.ResetCamera(() =>
                                 {
                                      plm.EndPanel.SetActive(true);   //end of game...

                                 });


                             });

                         }

                     });
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
