using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMKOC.PlantLifecycle
{
    public class OnTapSun : OnTapObject
    {
        public override void OnMouseDown()
        {
            //grow the plant to medium after we tap on the sun...
            if (PlantLifecycleManager.Instance.CurrentPlantGrowthStage == PlantGrowthStage.Medium)
            {
                PlantLifecycleManager.Instance.GrowPlant(PlantGrowthStage.Big);
                return;
            }
            PlantLifecycleManager.Instance.GrowPlant(PlantGrowthStage.Medium, () =>
            {
                PlantLifecycleManager.Instance.MoveSun(15, () =>
                {
                    //move the water can in again...
                    PlantLifecycleManager.Instance.MoveWaterCan(6, 1);
                });
            });
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}
