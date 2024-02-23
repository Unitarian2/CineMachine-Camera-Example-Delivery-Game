using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Grid grid;

    public string unitGameObjectName;

    public string prevName;
    public string nextName;

    [System.NonSerialized] public Unit prev;
    [System.NonSerialized] public Unit next;

    public void InitUnit(Grid grid, Vector3 startPos)
    {
        unitGameObjectName = gameObject.name;

        this.grid = grid;
        transform.position = startPos;//Bu þimdilik duplicate olarak position belirliyor ama ilerde unit'leri rastgele þekilde spawn edersek bu lazým olacak.

        prev = null; next = null;

        grid.AddBuilding(this, isNewUnit: true);


    }

    public void InitPlayerUnit(Grid grid, Vector3 startPos)
    {
        unitGameObjectName = gameObject.name;

        this.grid = grid;
        transform.position = startPos;//Bu þimdilik duplicate olarak position belirliyor ama ilerde unit'leri rastgele þekilde spawn edersek bu lazým olacak.

        prev = null; next = null;

        grid.AddPlayer(this);


    }

}
