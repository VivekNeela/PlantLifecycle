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
        [SerializeField] private List<GroundSprite> groundSprites;
        [SerializeField] private AnimationHandler sonuAnimHandler;

        //plant related gameobjects...
        [SerializeField] private Transform seedBag;
        [SerializeField] private Transform soilHeap;
        [SerializeField] private Transform plant;
        [SerializeField] private Transform seed;
        [SerializeField] private Transform hole;
        [SerializeField] private Transform waterCan;
        [SerializeField] private Transform gasoline;

        //other plant related stuff...
        [SerializeField] private List<Sprite> plantSprites;
        [SerializeField] private PlantGrowthStage currentPlantGrowthStage;



        private void Start()
        {
            plant.GetComponent<SpriteRenderer>().sprite = null;
            plant.DOLocalMoveY(-3.0f, 0.2f);
            plant.DOScale(0, 0.2f);


            soilHeap.DOScale(0, 0);
            soilHeap.DOMoveX(1.8f, 0);

            hole.DOScale(0, 0);

            seedBag.DOMoveX(15, 2);

            sonuAnimHandler.transform.DOMoveX(0, 4).OnComplete(() =>
            {
                sonuAnimHandler.SetTrigger("dig");
            });

        }


        public void MoveSeedBag()
        {
            seedBag.DOMoveX(5, 2).OnComplete(() =>
            {
                //sow the seed...
                SowSeed(() =>
                {
                    //move the soil heap...
                    soilHeap.DOLocalMoveX(hole.localPosition.x, 1f).OnComplete(() =>
                    {
                        // seed.SetParent(this.transform);   //dont need to do this...
                        seed.GetComponent<SpriteRenderer>().enabled = false;
                        //move the seedbag back...
                        seedBag.DOMoveX(15, 2).OnComplete(() =>
                        {
                            //bring in water and gasoline as options for growing the plant...
                            waterCan.DOLocalMoveX(-6, 0.5f);
                            gasoline.DOLocalMoveX(6, 0.5f);
                        });

                    });
                });
            });
        }



        //will call this whenever we grow the plant by any action...
        // [Button]
        public void GrowPlant(PlantGrowthStage plantGrowthStage)
        {
            switch (plantGrowthStage)
            {
                case PlantGrowthStage.None:
                    plant.GetComponent<SpriteRenderer>().sprite = null;
                    plant.DOScale(0, 0.2f);
                    plant.DOLocalMoveY(-3.0f, 0.2f);
                    break;
                case PlantGrowthStage.Small:
                    SetPlant(-2.0f, 0.2f);
                    break;
                case PlantGrowthStage.Medium:
                    SetPlant(-1.65f, 0.3f);
                    break;
                case PlantGrowthStage.Big:
                    SetPlant(-1.2f, 0.4f);
                    break;
                default:
                    break;
            }

            void SetPlant(float yPos, float scale)
            {
                plant.GetComponent<SpriteRenderer>().sprite = plantSprites[(int)plantGrowthStage];
                plant.DOScale(scale, 0.2f);
                plant.DOLocalMoveY(yPos, 0.2f);
            }
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


        public void SowSeed(Action onComplete)
        {
            var pos = new Vector3(hole.transform.position.x, hole.transform.position.y + 0.25f, hole.transform.position.z);
            seed.DOJump(pos, 5f, 1, 1f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
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


}
