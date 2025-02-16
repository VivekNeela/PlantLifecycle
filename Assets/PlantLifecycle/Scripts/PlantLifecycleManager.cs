using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TMKOC.PlantLifecycle
{
    public class PlantLifecycleManager : SingletonMonobehaviour<PlantLifecycleManager>
    {

        [SerializeField] private Image BgImage;
        [SerializeField] private Image groundImage;
        private List<GroundSprite> groundSprites;
        [SerializeField] private AnimationHandler sonuAnimHandler;

        //plant related gameobjects...
        [SerializeField] private Transform seedBags;
        [SerializeField] private Transform soilHeap;
        [SerializeField] private Transform plant;
        public Animator plantAnimator;

        public Transform seed;
        [SerializeField] private Transform hole;
        [SerializeField] private Transform waterCan;
        [SerializeField] private Transform gasoline;
        [SerializeField] private Transform sun;

        //more plant related stuff...
        [SerializeField] private List<Sprite> plantSprites;
        public List<Sprite> PlantSprites { get => plantSprites; set => plantSprites = value; }

        [SerializeField] private PlantGrowthStage currentPlantGrowthStage;
        public PlantGrowthStage CurrentPlantGrowthStage { get => currentPlantGrowthStage; set => currentPlantGrowthStage = value; }

        [SerializeField] private GameObject endPanel;
        public GameObject EndPanel { get => endPanel; set => endPanel = value; }

        public Material sunraysMat;
        [SerializeField] private bool inRayTransition;
        public bool InRayTransition { get => inRayTransition; set => inRayTransition = value; }

        [SerializeField] private CameraHandler cameraHandler;
        public CameraHandler CameraHandler { get => cameraHandler; set => cameraHandler = value; }


        private void Start()
        {
            plant.GetComponent<SpriteRenderer>().sprite = null;
            plant.DOLocalMove(new Vector2(-0.25f, -3), 0.2f);
            plant.DOScale(0, 0.2f);


            soilHeap.DOScale(0, 0);
            soilHeap.DOMoveX(1.8f, 0);

            hole.DOScale(0, 0);

            waterCan.DOMoveX(20, 0);
            gasoline.DOMoveX(-20, 0);

            seedBags.DOMoveX(15, 2);

            sonuAnimHandler.transform.DOMoveX(0, 4).OnComplete(() =>
            {
                sonuAnimHandler.SetTrigger("dig");
            });

        }


        public void MoveSeedBag()
        {
            seedBags.DOMoveX(5, 2).OnComplete(() =>
            {
                //maybe do somthing later on... 
            });
        }


        public void MoveWaterOptions(float xPos, float duration, Action onComplete = null)
        {
            waterCan.DOLocalMoveX(xPos, duration);
            gasoline.DOLocalMoveX(-xPos, duration).OnComplete(() => { onComplete?.Invoke(); });
        }

        public void MoveWaterCan(float xPos, float duration, Action onComplete = null)
        {
            waterCan.DOLocalMoveX(xPos, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public void MoveSun(float xPos, Action onComplete = null)
        {
            sun.DOLocalMoveX(xPos, 1f).OnComplete(() => { onComplete?.Invoke(); });
        }

        public void ScaleSunAnim()
        {
            //need to scale the sun down and up...

            Scale(1.3f);

            void Scale(float val)
            {
                var ogScale = sun.localScale;
                sun.DOScale(sun.localScale * val, 1).OnComplete(() =>
                {
                    sun.DOScale(ogScale, 1).OnComplete(() =>
                    {
                        Scale(val);
                    });
                });
            }
        }


        public void ScaleSunStop() => sun.DOKill();


        //will call this whenever we grow the plant by any action...
        // [Button]
        public void GrowPlant(PlantGrowthStage plantGrowthStage, Action onComplete = null)   //not gonna call this if we use anim....
        {
            switch (plantGrowthStage)
            {
                case PlantGrowthStage.None:
                    CurrentPlantGrowthStage = PlantGrowthStage.None;
                    plant.GetComponent<SpriteRenderer>().sprite = null;
                    plant.DOScale(0, 0.2f);
                    plant.DOLocalMoveY(-3.0f, 0.2f);

                    break;
                case PlantGrowthStage.Small:
                    CurrentPlantGrowthStage = PlantGrowthStage.Small;
                    SetPlant(-2.0f, 0.2f, onComplete);
                    break;
                case PlantGrowthStage.Medium:
                    CurrentPlantGrowthStage = PlantGrowthStage.Medium;
                    SetPlant(-1.65f, 0.3f, onComplete);
                    break;
                case PlantGrowthStage.Big:
                    CurrentPlantGrowthStage = PlantGrowthStage.Big;
                    SetPlant(-1.2f, 0.4f, onComplete);
                    break;
                default:
                    break;
            }

            void SetPlant(float yPos, float scale, Action onComplete)   //dotween animation...
            {
                //need to do this when we are halfway through the scaling tween
                if (plantGrowthStage == PlantGrowthStage.Small)
                    plant.GetComponent<SpriteRenderer>().sprite = PlantSprites[(int)plantGrowthStage];
                //the duration below needs to be same or the animation looks out of sync...
                Tween tw = null;
                tw = plant.DOScale(scale, 1f).OnUpdate(() =>
                {
                    float progress = tw.ElapsedPercentage();
                    if (progress >= 0.5f) // When the tween is 50% complete
                    {
                        // DoSomething();
                        Debug.Log("halfway through the tween...");
                        plant.GetComponent<SpriteRenderer>().sprite = PlantSprites[(int)plantGrowthStage];
                    }
                });

                plant.DOLocalMoveY(yPos, 1f).OnComplete(() => { onComplete?.Invoke(); });   //just so that we can do something after this over...
                plantAnimator.SetTrigger("grow");

            }

        }

        public void SetPlantAnimator(RuntimeAnimatorController runtimeAnimatorController)
        {
            plantAnimator.runtimeAnimatorController = runtimeAnimatorController;
        }

        public void SetPlantAnimatorX(float xPos)
        {
            plantAnimator.transform.DOMoveX(xPos, 0);
        }


        //not using rn...
        public void SetGroundState(GroundState groundState)
        {
            for (int i = 0; i < groundSprites.Count; i++)
            {
                if (groundSprites[i].groundState == groundState)
                    groundImage.sprite = groundSprites[i].sprite;
                else
                    continue;
            }
        }


        public void ScaleSoil(float soilScale)
        {
            soilHeap.DOScale(soilScale, 0.2f);
        }

        public void ScaleHole(float holeScale)
        {
            hole.DOScale(holeScale, 0.2f);
        }


        private void SowSeed(Action onComplete)
        {
            var pos = new Vector3(hole.transform.position.x, hole.transform.position.y + 0.25f, hole.transform.position.z);
            seed.DOJump(pos, 5f, 1, 1f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }


        public void SowSeed()
        {
            SowSeed(() =>
            {
                //move the soil heap...
                soilHeap.DOLocalMoveX(hole.localPosition.x, 1f).OnComplete(() =>
                {
                    // seed.SetParent(this.transform);   //dont need to do this...
                    seed.GetComponent<SpriteRenderer>().enabled = false;
                    //move the seedbag back...
                    seedBags.DOMoveX(15, 2).OnComplete(() =>
                    {
                        //bring in water and gasoline as options for growing the plant...
                        MoveWaterOptions(6, 1f);

                    });
                });
            });
        }


        public void SetPlantSprites(List<Sprite> sprites) => PlantSprites = sprites;



        [Button]
        public void ShootSunray(Action callback) => StartCoroutine(SunlightTransition(1f, .5f, 2f, callback));
        [Button]
        public void RetractSunray(Action callback) => StartCoroutine(SunlightTransition(.5f, 1f, 2, callback));

        public IEnumerator SunlightTransition(float a, float b, float duration, Action callback)
        {
            inRayTransition = true;
            float time = 0f;
            while (time < duration)
            {
                //runs for duration seconds...
                float t = time / duration;
                float value = Mathf.Lerp(a, b, t);
                sunraysMat.SetFloat("_xPos", value);
                time += Time.deltaTime;
                yield return null;
            }
            sunraysMat.SetFloat("_xPos", b);
            inRayTransition = false;
            callback?.Invoke();
        }
    }


    [Serializable]
    public struct GroundSprite
    {
        public GroundState groundState;
        public Sprite sprite;
    }

    public enum GroundState
    {
        Flat,
        FlatUnderground,
        Hole,
        HoleUnderground,
    }

    public enum PlantGrowthStage
    {
        None = 0,
        Small = 1,
        Medium = 2,
        Big = 3,
    }

    public enum SeedType
    {
        None = 0,
        Sunflower = 1,
        Rose = 2,
        Jasmine = 3,

    }


}
