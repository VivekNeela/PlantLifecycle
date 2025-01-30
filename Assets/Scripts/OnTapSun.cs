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
            PlantLifecycleManager.Instance.GrowPlant(PlantGrowthStage.Medium,()=>{PlantLifecycleManager.Instance.ScaleSunStop();});
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}
