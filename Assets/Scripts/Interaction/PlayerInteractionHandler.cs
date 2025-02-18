using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    private Interaction currentInteraction;
    private readonly float scantInterval = 0.5f;
    private float scanCooldown = 0;
    
    
    void Start()
    {
        
    }
	
    void Update()
    {
        if((scanCooldown -= Time.deltaTime) <= 0)
        {
            scanCooldown = scantInterval;
            ScanObjects();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentInteraction != null)
            {
                currentInteraction.Interact();
                ScanObjects();
            }
        }
    }

    private void ScanObjects()
    {
        Interaction nearestInteraction = GetNearestInteraction(transform.position);
        if(nearestInteraction != currentInteraction)
        {
            currentInteraction?.SetActive(false);
            nearestInteraction?.SetActive(true);
            currentInteraction = nearestInteraction;
        }
    }

    public Interaction GetNearestInteraction(Vector3 position)
    {
        float closestDst = -1;
        Interaction closestInteraction = null;

        var interactionList = GameManager.Instance.interactionList;
        foreach (Interaction interaction in interactionList)
        {
            var dst = (interaction.transform.position - position).magnitude;
            var isAvailable = interaction.IsAvailable();
            var isCloseEnough = dst <= interaction.radius;
            var isCacheInvalid = closestDst < 0;
            
            if (isCloseEnough && isAvailable)
            {
                if (isCacheInvalid || dst < closestDst)
                {
                    closestDst = dst;
                    closestInteraction = interaction;
                }
            }
        }

        return closestInteraction;
    }
}
