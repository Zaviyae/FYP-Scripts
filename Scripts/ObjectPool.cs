using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool  {

    public Stack<GameObject> inactivePool;
    public List<GameObject> activePool;
    public GameObject myObject;
    GameObject empty;
    int startNum;


    public ObjectPool(GameObject obj, int startNumber)
    {
        startNum = startNumber;
        inactivePool = new Stack<GameObject>();
        activePool = new List<GameObject>();

        empty = GameObject.Instantiate(new GameObject());
        empty.name = obj.name + " Pool";

        for(int i = 0; i <= startNumber; i++)
        {
            push(GameObject.Instantiate(obj));
        }

        

    }


    public GameObject get()
    {
        if (inactivePool.Count > 0 && inactivePool.Peek())
        {
            checkUp();
            GameObject popped = inactivePool.Pop();
            popped.transform.parent = null;
            activePool.Add(popped);
 
            
            return popped;
        }
        else
        {
            return GameObject.Instantiate(myObject);
        }

    }

    void push(GameObject temp)
    {
        temp.AddComponent<DisableAfter>();
        temp.GetComponent<DisableAfter>().time = 10;
        temp.transform.parent = empty.transform;
        temp.SetActive(false);
        inactivePool.Push(temp);
    }

    void checkUp()
    {
        
            foreach (GameObject o in activePool.ToArray())
            {
                if (!o.activeSelf)
                {
                
                    push(o);
                    activePool.Remove(o);
                    
                }
            }
 
    }
}
