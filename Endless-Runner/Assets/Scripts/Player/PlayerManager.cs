using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerManager : GameStateManager
{  
    protected const float defHungerDecrease = -0.025f; // (do not change) drop amount when hunger is invoked, change hungerInvokeRateIncrement or defHungerInvokeRate to speed up getting hungry
    protected float hungerDropInvokeDelay = 1f;
    
    protected float hungerDropRate;
    protected const float hungerInvokeRateIncrement = -0.05f;
    protected const float defHungerInvokeRate = 1.2f;

    protected const float defTotalHunger = 100;
    protected static float currentHunger;
    
}
