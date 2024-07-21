using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyScript : MonoBehaviour
{
    public int id;
    
    private void OnMouseDown()
    {
        GameScript.Instance.Spawn(id,this.gameObject);
    }
}
