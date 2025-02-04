using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMKOC.PlantLifecycle
{
    public class OnTapSeedBag : OnTapObject
    {
        [SerializeField] private PlantLifecycleManager plm;
        [SerializeField] private SeedType seedType;
        [SerializeField] private Transform seed;
        [SerializeField] private List<Sprite> plantSprites;


        // [SerializeField] private PlantGrowthStage currentPlantState;

        private void Start()
        {
            plm = PlantLifecycleManager.Instance;
        }


        public override void OnMouseDown()
        {
            //sow the seed...
            //sow the seed...lets do this on click...
            //assign the plant sprites to plant sprites list...

            // for (int i = 0; i < plm.plantSpriteslist.Count; i++)
            // {
            //     if (plm.plantSpriteslist[i].seedType == this.seedType)
            //     {
            //         plm.PlantSprites = this.plantSprites;
            //     }
            //     else
            //         continue;
            // }


            plm.SetPlantSprites(this.plantSprites);

            //set the seed transform of plm to this seed...
            plm.seed = this.seed;


            plm.SowSeed();

        }








        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }







        //might use this function later
        private void OnTapGrowPlant()
        {
            // if (currentPlantState == PlantGrowthStage.Big)
            // {

            //     currentPlantState = 0;
            //     PlantLifecycleManager.Instance.GrowPlant(currentPlantState);
            // }
            // else
            // {
            //     currentPlantState += 1;
            //     PlantLifecycleManager.Instance.GrowPlant(currentPlantState);
            // }

        }
    }
}
