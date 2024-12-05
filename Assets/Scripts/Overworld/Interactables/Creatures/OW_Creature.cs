using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class OW_Creature : Interactable
{

    [SerializedDictionary("Unit", "Amount")]
    public SerializedDictionary<Unit, int> Army;
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
