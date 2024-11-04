using System.Collections;
using System.Collections.Generic;


public class Economy
{
    public Dictionary<ResourceData.ResourceType, int> ResourceAmount { get; private set; }
    
    public Dictionary<ResourceData.ResourceType, int> DailyIncome { get; private set; }
}
