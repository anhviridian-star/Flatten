using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockController
{
    private Dictionary<Collider2D, Block> colliderToBlockDict = new();
    private List<Collider2D> currentTransparentColliders = new();

    private Block GetBlockByCollider(Collider2D collider)
    {
        if (colliderToBlockDict.ContainsKey(collider))
        {
            return colliderToBlockDict[collider];
        }
        else
        {
            colliderToBlockDict.Add(collider, collider.gameObject.GetComponent<Block>());
            return colliderToBlockDict[collider];
        }
    }

    public void SetTransparentBlocksByColliders(Collider2D[] collider2D)
    {
        //Debug.Log("Overlap Colliders Count: " + collider2D.Length);
        for (int i = 0; i < currentTransparentColliders.Count; i++)
        {
            if (!collider2D.Contains(currentTransparentColliders[i]))
            {
                GetBlockByCollider(currentTransparentColliders[i]).SetTransparent(false);
            }
        }

        currentTransparentColliders.Clear();
        for (int i = 0; i < collider2D.Length; i++)
        {
            currentTransparentColliders.Add(collider2D[i]);
            GetBlockByCollider(collider2D[i]).SetTransparent(true);
        }
    }
}
