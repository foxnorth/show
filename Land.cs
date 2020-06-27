using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    private Building[,] buildings = new Building[100, 100];
    private Tile[,] tiles = new Tile[100, 100];

    private Kingdom kingdom;

    private void Start()
    {
        kingdom = transform.Find("Kingdom").GetComponent<Kingdom>();
        CalculateLandCost();
    }

    public void AddBuilding(Building building, Vector3 position) {
        int length = (int)building.transform.Find("Model").localScale.x;
        int width = (int)building.transform.Find("Model").localScale.z;

        int xSideSteps = (length - 1) / 2;
        bool xIsEven = length % 2 == 0;
        if (xIsEven)
        {
            position -= new Vector3(0.5f, 0, 0);
            xSideSteps = length / 2;
        }

        int ySideSteps = (width - 1) / 2;
        bool yIsEven = width % 2 == 0;
        if (yIsEven)
        {
            position -= new Vector3(0, 0, 0.5f);
            ySideSteps = width / 2;
        }

        Building buildingToAdd = Instantiate(building, position, Quaternion.identity);

        int xStartPos = (int)position.x;
        int zStartPos = (int)position.z;

        for (int i = 0; i <= xSideSteps; i++)
        {
            for (int j = 0; j <= ySideSteps; j++)
            {
                buildings[xStartPos + i, zStartPos + j] = buildingToAdd;

                if ((!xIsEven || i != xSideSteps) && (!yIsEven || j != ySideSteps))
                {
                    buildings[xStartPos - i, zStartPos - j] = buildingToAdd;
                    buildings[xStartPos - i, zStartPos + j] = buildingToAdd;
                    buildings[xStartPos + i, zStartPos - j] = buildingToAdd;
                }
            }
        }

        kingdom.buildOrders.Add(buildingToAdd);
    }

    public Building CheckForBuildingAtPosition(Vector3 position)
    {
        Building tile = buildings[(int)position.x, (int)position.z];
        if (tile != null)
        {
            Debug.Log("Can't place here!");
        }
        
        return tile;
    }

    public void RemoveBuilding(Vector3 position)
    {
        Destroy(buildings[(int)position.x, (int)position.z].gameObject);
        buildings[(int)position.x, (int)position.z] = null;
    }

    public Vector3 CalculateGridPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), .01f, Mathf.Round(position.z));
    }

    public bool isGroundClear(Vector3 position, Building building)
    {
        bool groundOccupied = false;

        int length = (int)building.transform.Find("Model").localScale.x;
        int width = (int)building.transform.Find("Model").localScale.z;

        int xSideSteps = (length - 1) / 2;
        bool xIsEven = length % 2 == 0;
        if (xIsEven)
        {
            position -= new Vector3(0.5f, 0, 0);
            xSideSteps = length / 2;
        }

        int ySideSteps = (width - 1) / 2;
        bool yIsEven = width % 2 == 0;
        if (yIsEven)
        {
            position -= new Vector3(0, 0, 0.5f);
            ySideSteps = width / 2;
        }

        int xStartPos = (int)position.x;
        int zStartPos = (int)position.z;

        for (int i = 0; i <= xSideSteps; i++)
        {

            if (xStartPos - i < 0)
                groundOccupied = true;

            for (int j = 0; j <= ySideSteps; j++)
            {
                if (zStartPos - j < 0)
                    groundOccupied = true;

                if (groundOccupied)
                    return false;

                groundOccupied = (buildings[xStartPos + i, zStartPos + j] != null) || groundOccupied;

                if ((!xIsEven || i != xSideSteps) && (!yIsEven || j != ySideSteps))
                {
                    groundOccupied = (buildings[xStartPos - i, zStartPos - j] != null) || groundOccupied;
                    groundOccupied = (buildings[xStartPos - i, zStartPos + j] != null) || groundOccupied;
                    groundOccupied = (buildings[xStartPos + i, zStartPos - j] != null) || groundOccupied;
                }
            }
        }

        return !groundOccupied;
    }

    public void CalculateLandCost()
    {
        
    }
}
