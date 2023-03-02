using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    private CropData currentCrop;
    private int plantDay;
    private int daysSinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    public void Plant(CropData crop)
    {
        currentCrop = crop;
        plantDay = GameManager.instance.currentDay;
        daysSinceLastWatered = 1;
        UpdateCropSprite();

        onPlantCrop?.Invoke(crop);
    }

    public void NewDayCheck()
    {
        if(daysSinceLastWatered > 3)
        {
            Destroy(gameObject);
        }

        UpdateCropSprite();
    }

    private void UpdateCropSprite()
    {
        int cropProgress = CropProgress();
        if(cropProgress < currentCrop.daysToGrow)
        {
            sr.sprite = currentCrop.growProgressSprites[cropProgress];
        } else
        {
            sr.sprite = currentCrop.readyToHarvestSprite;
        }
    }

    public void Water()
    {
        daysSinceLastWatered = 0;
    }

    public void Harvest()
    {
        if (CanHarvest())
        {
            onHarvestCrop?.Invoke(currentCrop);
            Destroy(gameObject);
        }
    }

    private int CropProgress()
    {
        return GameManager.instance.currentDay - plantDay;
    }

    public bool CanHarvest()
    {
        return CropProgress() >= currentCrop.daysToGrow;
    }
}
