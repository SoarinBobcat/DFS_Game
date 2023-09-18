using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node targetNode;
    private Vector3 currentDir;
    private Vector3 targetDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        InitializeAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (targetNode != null)
            {
                targetDir = targetNode.transform.position - transform.position;
                targetDir.y = 0;
                targetDir = targetDir.normalized;
                currentDir = Vector3.Lerp(currentDir,targetDir,8*Time.deltaTime);

                //If within 0.25 units of the current node.
                if (Vector3.Distance(transform.position, targetNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                //Implement path finding here
                else
                {
                    DFSAlgo();
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);

            speed += 1f*Time.deltaTime;
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        targetNode = GameManager.Instance.Nodes[0];
        currentDir = targetNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
        currentDir.y = 0;
        targetDir = targetNode.transform.position - transform.position;
        targetDir = targetDir.normalized;
        targetDir.y = 0;
    }

    //Implement DFS algorithm method here
    void DFSAlgo()
    {
        //Node variable currently being searched
        Node currNode = GameManager.Instance.Nodes[0];
        Node dest = GameManager.Instance.Player.CurrentNode;

        //bool for whether or not the target has been found
        bool found = false;

        //Array for storing unsearched nodes; the stack
        List<Node> stack = new List<Node>();
        List<Node> searched = new List<Node>();

        //Debug loop limit
        int debug_loopLimit = 0;
        //Debug end :)

        while (!found)
        {
            searched.Add(currNode);
            stack.Remove(currNode);

            if (currNode == dest)
            {
                targetNode = currNode;
                return;
            }
            else
            {
                foreach (Node child in currNode.Children)
                {
                    if (!stack.Contains(child) || !searched.Contains(child))
                    {
                        stack.Add(child);
                    }
                }

                //Update currNode to the top of the stack
                if (stack.Count != 0)
                {
                    currNode = stack[stack.Count-1];
                }
                else
                {
                    Debug.Log("Destination not found");
                    return;
                }
            }

            //Debug loop
            debug_loopLimit++;
            if (debug_loopLimit > 100)
            {
                found = true;
            }
        }
    }


    /*void DFSAlgo(Node root, Node dest)
    {
        visited.Append(root);
        for (var i = 0; i < int root.Children.Count; i++)
        {
            if (!found)
            {
                //Assign parent if a node has none
                if (root.Children[i].parent != null)
                {
                    root.Children[i].parent = root.Children[i].parent.Append(root);
                }

                // If destination is found return the path
                if (root.Children[i] == dest)
                {
                    found = true;
                    break;
                }

                //Begin a recursion
                DFSAlgo(root.Children[i], dest);

                continue;
            }
            break;
        }

        path = path.Append(dest);
        visited.Remove(dest);
        var curr = dest.parent;
        while (visited != null)
        {
            path.InsertRange(0, curr.parent);
            curr = curr.parent;
            if (curr == root)
            {
                return;
            }
        }
        return;
    }*/
}