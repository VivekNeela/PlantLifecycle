using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMKOC.PlantLifecycle
{
    public class OnTapSeedBag : OnTapObject
    {
        [SerializeField] private PlantGrowthStage currentPlantState;

        public override void OnMouseDown()
        {
            //sow the seed...
            
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
            if (currentPlantState == PlantGrowthStage.Big)
            {

                currentPlantState = 0;
                PlantLifecycleManager.Instance.GrowPlant(currentPlantState);
            }
            else
            {
                currentPlantState += 1;
                PlantLifecycleManager.Instance.GrowPlant(currentPlantState);
            }

        }
    }
}
