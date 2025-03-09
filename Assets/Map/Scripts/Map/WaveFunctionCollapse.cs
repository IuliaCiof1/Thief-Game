using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class WaveFunctionCollapse : MonoBehaviour
{
    public GameObject allProtoPrefab;
    public float gridOffset = 1;
    public Vector2 size;
    public Vector3 startPosition;
    public List<Cell> cells;
    public Dictionary<Vector2, Cell> activeCells = new Dictionary<Vector2, Cell>();
    public List<Cell> cellsAffected = new List<Cell>();
    public Weights weights;
    public GameObject borderPrefab;

    Cell cell_;

    

    //private void OnValidate()
    //{
    //    print("Initialise");
    //    InitializeWaveFunction();
    //}

    void Awake()
    {
        //print("Initialise");
         //InitializeWaveFunction();
        //StartCoroutine(CollapseOverTime());
    }
    private void LoadData()
    {
        //load dictionary here
    }
    public void InitializeWaveFunction()
    {
        ClearAll();
        for (int x = 0, y = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3 pos = new Vector3(x* gridOffset + startPosition.x, 0, z * gridOffset + startPosition.z);

                if(this.gameObject.transform.childCount>y)//kinda breaks
                {
                    
                    GameObject block = this.transform.GetChild(y).gameObject;
                    block.SetActive(true);
                    block.transform.position = pos;        
                }
                else
                {
                    

//#if UNITY_EDITOR
//                    print("47");
                   
//                        GameObject block = (GameObject)PrefabUtility.InstantiatePrefab(allProtoPrefab as GameObject);
//                        print("48");
//                    print(block.name);
//                        if (block is null)
//                            print("not found block");
//                        else
                        
//                        PrefabUtility.UnpackPrefabInstance(block, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
//                        block.transform.SetParent(this.transform);
//                        block.transform.position = pos;
//#else
                    
                    GameObject block = Instantiate(allProtoPrefab);
                    if (block is null)
                        print("not found block");
                    else
                    {
                        block.transform.SetParent(this.transform);
                        block.transform.position = pos;
                    }

                }

         
                Cell cell = this.transform.GetChild(y).gameObject.GetComponent<Cell>();
                cell.coords = new Vector2(x,z);
                cells.Add(cell);
                activeCells.Add(cell.coords, cell);
                
                y++;
            }
        }
        foreach(Cell c in cells)
            FindNeighbours(c);

        foreach (Cell c in cells)
        {
            c.GenerateWeight(weights);
           
        }

        StartCollapse();
        //CollapseOverTime();

        CreateBorder();
        RandomizeBuildings();

        //If some tiles diddn't generate the mesh, generate a random building
        foreach(Transform child in gameObject.transform)
        {
            if (child.name.Contains(allProtoPrefab.name))
            {
                
                Cell cell = child.GetComponent<Cell>();
                GameObject finalPrefab = Instantiate(child.GetChild(0).gameObject, cell.transform, true);
                finalPrefab.GetComponent<BuildingRandomizer>().RandomizeBuilding();

               
                finalPrefab.transform.localPosition = Vector3.zero;
                finalPrefab.SetActive(true);
            }
        }
    }
    private void CreateBorder()
    {
        //create a GameObject to contain the border
        //to keep things neat in the Hierarchy tab if you decide
        //to start generating huge landscapes later on

        //create border blocks along x axes
        float borderOffset = gridOffset*-0.1f; // Adjust this value as needed to properly position the borders

        // Create border blocks along the x-axis
        for (int x = 0; x <= size.x; x++)
        {
            // Top border (z = -1 - borderOffset)
            DoInstantiate(borderPrefab, new Vector3((x-1) * gridOffset + startPosition.x*-1, 0, -1 * gridOffset + startPosition.z), Quaternion.identity, this.transform);
            // Bottom border (z = size.y + borderOffset)
            DoInstantiate(borderPrefab, new Vector3(x * gridOffset + startPosition.x, 0, size.y * gridOffset + startPosition.z ), Quaternion.Euler(0, 180, 0), this.transform);
        }

        // Create border blocks along the z-axis
        for (int z = 0; z <= size.y; z++)
        {
            // Left border (x = -1 - borderOffset)
            DoInstantiate(borderPrefab, new Vector3(-1 * gridOffset + startPosition.x, 0, z * gridOffset + startPosition.z), Quaternion.Euler(0, 90, 0), this.transform);
            // Right border (x = size.x + borderOffset)
            DoInstantiate(borderPrefab, new Vector3(size.x * gridOffset + startPosition.x, 0, (z - 1) * gridOffset + startPosition.z), Quaternion.Euler(0, -90, 0), this.transform);
        }
    }
    [ContextMenu("Randomize Buildings")]
    public void RandomizeBuildings()
    {
        BuildingRandomizer[] buildings = GetComponentsInChildren<BuildingRandomizer>();
        foreach(BuildingRandomizer b in buildings)
            b.RandomizeBuilding();
    }
    private void DoInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
         Transform temp = ((GameObject)Instantiate(prefab,position,rotation)).transform;
         temp.parent = parent;
     }
    private void FindNeighbours(Cell c)
    {
        c.posZneighbour = GetCell(c.coords.x,c.coords.y+1);
        c.negZneighbour = GetCell(c.coords.x,c.coords.y-1);
        c.posXneighbour = GetCell(c.coords.x+1,c.coords.y);
        c.negXneighbour = GetCell(c.coords.x-1,c.coords.y);
    }
    private Cell GetCell(float x, float z)
    {
        Cell cell = null;
        if(activeCells.TryGetValue(new Vector2(x,z), out cell))
            return cell;
        else
            return null;
    }
    int collapsed;
    public void StartCollapse()
    {
        collapsed=0;
        while (!isCollapsed())
        {
            Iterate();
            
            //if (cell_.name == allProtoPrefab.name)
               
        }

    
    }
    public IEnumerator CollapseOverTime()
    {
        while(!isCollapsed())
        {
            Iterate();
            yield return new WaitForSeconds(0.5f);
        }
    }
    private bool isCollapsed()
    {
        //check if any cells contain more than one entry
        foreach (Cell c in cells)
        {
           
            if (c.possiblePrototypes.Count > 1)
            {
              
                return false;
            }
        }

        return true;
    }
    private void Iterate()
    {
        Cell cell = GetCellWithLowestEntropy();
        
       
        CollapseAt(cell);
        Propagate(cell);
        cell_ = cell;
        
    }
    private Cell GetCellWithLowestEntropy()
    {
        //add randomization in the case of a tie
        List<Cell> cellWithLowestEntropy = new List<Cell>();
        int x = 100000;
        List<Cell> shuffledCells = cells.OrderBy(c => Random.value).ToList();

        foreach (Cell c in cells)
        {
            if(!c.isCollapsed)
            {
                if(c.possiblePrototypes.Count==x)
                {
                    cellWithLowestEntropy.Add(c);
                }
                else if(c.possiblePrototypes.Count<x)
                {
                    cellWithLowestEntropy.Clear();
                    cellWithLowestEntropy.Add(c);
                    x = c.possiblePrototypes.Count;
                }
            }
        }
        return cellWithLowestEntropy[Random.Range(0, cellWithLowestEntropy.Count)];
    }
    private void CollapseAt(Cell cell)
    {
        Prototype finalPrototype;
        int selectedPrototypeIndex = 0;

        if (cell.coords == Vector2.zero)
        {
            // Create a list of indices for the prototypes that are not Road_Straight or Road_Intersection
            List<int> validIndices = new List<int>();
            List<int> validWeights = new List<int>();

            for (int i = 0; i < cell.possiblePrototypes.Count; i++)
            {
                if (!cell.possiblePrototypes[i].name.Contains("Road_Straight") &&
                    !cell.possiblePrototypes[i].name.Contains("Road_Intersection"))
                {
                    validIndices.Add(i);
                    validWeights.Add(cell.prototypeWeights[i]);
                }
            }

            // If there are no valid prototypes after filtering, handle this case
            if (validIndices.Count == 0)
            {
                print("No valid prototypes available for cell at (0,0) after filtering.");
              
            }

            // Select a prototype index from the filtered list using the filtered weights
            int selectedFilteredIndex = SelectPrototype(validWeights);
            selectedPrototypeIndex = validIndices[selectedFilteredIndex];
        }
        else
        {
            // Select a prototype based on cell's weights
            selectedPrototypeIndex = SelectPrototype(cell.prototypeWeights);
        }

        finalPrototype = cell.possiblePrototypes[selectedPrototypeIndex];
       
        // Clear the possiblePrototypes and add only the final selected one
        cell.possiblePrototypes.Clear();
        cell.possiblePrototypes.Add(finalPrototype);

      

        // Instantiate the final prefab
        GameObject finalPrefab = Instantiate(finalPrototype.prefab, cell.transform, true);

        // Rotate and position the prefab
        finalPrefab.transform.Rotate(new Vector3(0f, finalPrototype.meshRotation * 90, 0f), Space.Self);
        finalPrefab.transform.localPosition = Vector3.zero;
        finalPrefab.SetActive(true);

        

        // Set the cell's name and mark it as collapsed
        cell.name = cell.coords.ToString() + "_" + collapsed.ToString();
        collapsed++;
        cell.isCollapsed = true;
    }

    private int SelectPrototype(List<int> prototypeWeights)
    {
        // Weighted random selection of prototype
        int totalWeight = prototypeWeights.Sum();
        int randomWeight = Random.Range(0, totalWeight);

        for (int i = 0; i < prototypeWeights.Count; i++)
        {
            if (randomWeight < prototypeWeights[i])
            {
                return i;
            }
            randomWeight -= prototypeWeights[i];
        }

        

        return 0; // Fallback in case of an error
    }

    private void Propagate(Cell cell)
    {
        cellsAffected.Add(cell);
        int y = 0;
        while (cellsAffected.Count > 0)
        {
            Cell currentCell = cellsAffected[0];
            cellsAffected.RemoveAt(0);

            PropagateToNeighbor(currentCell, currentCell.posXneighbour, GetPossibleSocketsPosX, proto => proto.negX);
            PropagateToNeighbor(currentCell, currentCell.posZneighbour, GetPossibleSocketsPosZ, proto => proto.negZ);
            PropagateToNeighbor(currentCell, currentCell.negXneighbour, GetPossibleSocketsNegX, proto => proto.posX);
            PropagateToNeighbor(currentCell, currentCell.negZneighbour, GetPossibleSocketsNegZ, proto => proto.posZ);

            y++;
        }
    }

    private void PropagateToNeighbor(Cell currentCell, Cell neighborCell, Func<List<Prototype>, List<WFC_Socket>> getPossibleSocketsFunc, Func<Prototype, WFC_Socket> getNeighborSocket)
    {
        if (neighborCell == null)
            return;

        List<WFC_Socket> possibleConnections = getPossibleSocketsFunc(currentCell.possiblePrototypes);
        List<int> indicesToRemove = new List<int>();

       

        for (int i = 0; i < neighborCell.possiblePrototypes.Count; i++)
        {
            if (!possibleConnections.Contains(getNeighborSocket(neighborCell.possiblePrototypes[i])))
            {
                indicesToRemove.Add(i);
            }
        }

        if (indicesToRemove.Count > 0)
        {
            for (int i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                
                neighborCell.possiblePrototypes.RemoveAt(indicesToRemove[i]);
                neighborCell.prototypeWeights.RemoveAt(indicesToRemove[i]);
            }

            cellsAffected.Add(neighborCell);
        }
    }


    //private void Propagate(Cell cell)
    //{
    //    cellsAffected.Add(cell);
    //    int y = 0;
    //    while(cellsAffected.Count > 0)
    //    {
    //        Cell currentCell = cellsAffected[0];
    //        cellsAffected.Remove(currentCell);

    //        //get neighbor to the right
    //        Cell otherCell = currentCell.posXneighbour;
    //        if(otherCell!=null)
    //        {
    //            //Get sockets that we have available on our Right
    //            List<WFC_Socket> possibleConnections = GetPossibleSocketsPosX(currentCell.possiblePrototypes);

    //            bool constrained = false;
    //            for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
    //            {
    //                //if the list of sockets that we have on the right does not contain the connector on the other cell to the left...
    //                if(!possibleConnections.Contains(otherCell.possiblePrototypes[i].negX))
    //                {
    //                    //then that is not a valid possibility and must be removed
    //                    otherCell.possiblePrototypes.RemoveAt(i);
    //                    otherCell.prototypeWeights.RemoveAt(i);
    //                    i-=1;
    //                    constrained = true;
    //                }
    //            }

    //            if(constrained)
    //                cellsAffected.Add(otherCell);
    //        }

    //        otherCell = currentCell.posZneighbour;
    //        if(otherCell!=null)
    //        {
    //            List<WFC_Socket> possibleConnections = GetPossibleSocketsPosZ(currentCell.possiblePrototypes);
    //            bool hasBeenConstrained = false;

    //            //check all neighbours
    //            for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
    //            {
    //                if(!possibleConnections.Contains(otherCell.possiblePrototypes[i].negZ))
    //                {
    //                    otherCell.possiblePrototypes.RemoveAt(i);
    //                    otherCell.prototypeWeights.RemoveAt(i);
    //                    i-=1;
    //                    hasBeenConstrained = true;
    //                }
    //            }
    //            if(hasBeenConstrained)
    //                cellsAffected.Add(otherCell);
    //        }
    //        otherCell = currentCell.negXneighbour;
    //        if(otherCell!=null)
    //        {
    //            List<WFC_Socket> possibleConnections = GetPossibleSocketsNegX(currentCell.possiblePrototypes);
    //            bool hasBeenConstrained = false;
    //            for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
    //            {
    //                if(!possibleConnections.Contains(otherCell.possiblePrototypes[i].posX))
    //                {
    //                    otherCell.possiblePrototypes.RemoveAt(i);
    //                    otherCell.prototypeWeights.RemoveAt(i);
    //                    i-=1;
    //                    hasBeenConstrained = true;
    //                }
    //            }
    //            if(hasBeenConstrained)
    //                cellsAffected.Add(otherCell);
    //        }
    //        otherCell = currentCell.negZneighbour;
    //        if(otherCell!=null)
    //        {
    //            List<WFC_Socket> possibleConnections = GetPossibleSocketsNegZ(currentCell.possiblePrototypes);
    //            bool hasBeenConstrained = false;
    //            for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
    //            {
    //                if(!possibleConnections.Contains(otherCell.possiblePrototypes[i].posZ))
    //                {
    //                    otherCell.possiblePrototypes.RemoveAt(i);
    //                    otherCell.prototypeWeights.RemoveAt(i);
    //                    i-=1;
    //                    hasBeenConstrained = true;
    //                }
    //            }
    //            if(hasBeenConstrained)
    //                cellsAffected.Add(otherCell);
    //        }


    //        // Debug.Log($"There are currently {cellsAffected.Count} cellsAffected");

    //        // if(otherCell!=null)
    //        // {
    //        //     int posible = otherCell.possiblePrototypes.Count;
    //        //     //other cell coords
    //        //     //other cell posible proto

    //        //     //possible neighbors of current cell to the other cell's direction

    //        //     //get all valid neighbors of current cell
    //        //     // Compare

    //        //     //if prototpe is not valid with current cell, remove
    //        //         //add to cells Affected
    //        //     Constrain(otherCell, currentCell);
    //        //     // if(Constrain(otherCell, currentCell))
    //        //     //     cellsAffected.Add(otherCell);
    //        // }
    //        // otherCell = cell.negXneighbour;
    //        // if(otherCell!=null)
    //        // {
    //        //     int posible = otherCell.possiblePrototypes.Count;
    //        //     if(Constrain(otherCell, currentCell))
    //        //         Debug.Log(otherCell.possiblePrototypes.Count);
    //        // }
    //        // otherCell = cell.posZneighbour;
    //        // if(otherCell!=null)
    //        // {
    //        //     int posible = otherCell.possiblePrototypes.Count;
    //        //     if(Constrain(otherCell, currentCell))
    //        //         Debug.Log(otherCell.possiblePrototypes.Count);
    //        // }
    //        // otherCell = cell.negZneighbour;
    //        // if(otherCell!=null)
    //        // {
    //        //     int posible = otherCell.possiblePrototypes.Count;
    //        //     if(Constrain(otherCell, currentCell))
    //        //         Debug.Log(otherCell.possiblePrototypes.Count);
    //        // }
    //        y++;
    //    }
    //}
    private List<WFC_Socket> GetPossibleSocketsNegX(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            // if(!socketsAccepted.Contains(proto.posX))
            //     socketsAccepted.Add(proto.posX);
            if(!socketsAccepted.Contains(proto.negX))
                socketsAccepted.Add(proto.negX);
            // if(!socketsAccepted.Contains(proto.posZ))
            //     socketsAccepted.Add(proto.posZ);
            // if(!socketsAccepted.Contains(proto.negZ))
            //     socketsAccepted.Add(proto.negZ);
        }
        return socketsAccepted;
    }
    private List<WFC_Socket> GetPossibleSocketsNegZ(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if(!socketsAccepted.Contains(proto.negZ))
                socketsAccepted.Add(proto.negZ);
        }
        return socketsAccepted;
    }
    private List<WFC_Socket> GetPossibleSocketsPosZ(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if(!socketsAccepted.Contains(proto.posZ))
                socketsAccepted.Add(proto.posZ);
        }
        return socketsAccepted;
    }
    private List<WFC_Socket> GetPossibleSocketsPosX(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if(!socketsAccepted.Contains(proto.posX))
            {
                // Debug.Log($"Adding {proto.posX}, to the list of accepted sockets for {proto.name}");
                socketsAccepted.Add(proto.posX);
            }
        }
        return socketsAccepted;
    }
    
    private bool Constrain(Cell otherCell, WFC_Socket socketItMustPairWith)
    {
        bool hasBeenConstrained = false;
        
        //check all neighbours
        for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
        {
            // if(otherCell.possiblePrototypes[i])
            // List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
            // socketsAccepted.AddRange(GetPossibleSockets(currentCell.possiblePrototypes));
            // Debug.Log($"Sockets accepted {socketsAccepted.Count}");
            // if(HasAConnector(currentCell.possiblePrototypes[0].negX, otherCell.possiblePrototypes[i].posX))
            // {
            //     otherCell.possiblePrototypes.RemoveAt(i);
            //     i-=1;
            //     hasBeenConstrained = true;
            // }
            // else if(HasAConnector(socketsAccepted, otherCell.possiblePrototypes[i].posZ))
            // {
            //     otherCell.possiblePrototypes.RemoveAt(i);
            //     i-=1;
            //     hasBeenConstrained = true;
            // }
            // else if(HasAConnector(socketsAccepted, otherCell.possiblePrototypes[i].negX))
            // {
            //     otherCell.possiblePrototypes.RemoveAt(i);
            //     i-=1;
            //     hasBeenConstrained = true;
            // }
            // else if(HasAConnector(socketsAccepted, otherCell.possiblePrototypes[i].negZ))
            // {
            //     otherCell.possiblePrototypes.RemoveAt(i);
            //     i-=1;
            //     hasBeenConstrained = true;
            // }
        }
        return hasBeenConstrained;
    }
    // private bool Constrain(Cell otherCell, Cell currentCell)
    // {
    //     bool hasBeenConstrained = false;
        
    //     //check all neighbours
    //     for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
    //     {
    //         List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
    //         // socketsAccepted.AddRange(GetPossibleSockets(currentCell.possiblePrototypes));
    //         Debug.Log($"Sockets accepted {socketsAccepted.Count}");
    //         if(HasAConnector(currentCell.possiblePrototypes[0].negX, otherCell.possiblePrototypes[i].posX))
    //         {
    //             otherCell.possiblePrototypes.RemoveAt(i);
    //             i-=1;
    //             hasBeenConstrained = true;
    //         }
    //         else if(HasAConnector(socketsAccepted, otherCell.possiblePrototypes[i].posZ))
    //         {
    //             otherCell.possiblePrototypes.RemoveAt(i);
    //             i-=1;
    //             hasBeenConstrained = true;
    //         }
    //         else if(HasAConnector(socketsAccepted, otherCell.possiblePrototypes[i].negX))
    //         {
    //             otherCell.possiblePrototypes.RemoveAt(i);
    //             i-=1;
    //             hasBeenConstrained = true;
    //         }
    //         else if(HasAConnector(socketsAccepted, otherCell.possiblePrototypes[i].negZ))
    //         {
    //             otherCell.possiblePrototypes.RemoveAt(i);
    //             i-=1;
    //             hasBeenConstrained = true;
    //         }
    //     }
    //     return hasBeenConstrained;
    // }
    private bool HasAConnector(List<WFC_Socket> socketsAccepted, WFC_Socket thisSocket)
    {
        foreach (WFC_Socket s in socketsAccepted)
        {
            if(s == thisSocket)
                return true;
        }
        return false;
    }
    private List<WFC_Socket> GetPossibleSockets(List<Prototype> possibleNeighbors)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in possibleNeighbors)
        {
            if(!socketsAccepted.Contains(proto.posX))
                socketsAccepted.Add(proto.posX);
            if(!socketsAccepted.Contains(proto.negX))
                socketsAccepted.Add(proto.negX);
            if(!socketsAccepted.Contains(proto.posZ))
                socketsAccepted.Add(proto.posZ);
            if(!socketsAccepted.Contains(proto.negZ))
                socketsAccepted.Add(proto.negZ);
        }
        return socketsAccepted;
    }
    public void ClearAll()
    {
        cells.Clear();
        activeCells.Clear();
        for(int i = this.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
    }
}
