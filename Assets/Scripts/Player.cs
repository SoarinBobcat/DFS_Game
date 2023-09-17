using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;

    int up;
    int down;
    int left;
    int right;
    int hMove;
    int vMove;
    Vector3 move;

    bool schmovin = true;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if (node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CurrentNode);

        if (moving == false)
        {
            if (schmovin == true)
            {
                //Implement inputs and event-callbacks here
                up = Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0;
                down = Input.GetKeyDown(KeyCode.DownArrow) ? -1 : 0;
                left = Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : 0;
                right = Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0;

                hMove = left + right;
                vMove = up + down;

                move = new Vector3(hMove, 0, vMove);
            }
            else
            {
                schmovin = false;
            }

            if ((move != Vector3.zero) && (!moving))
            {
                float distance = 7.5f;
                Debug.DrawRay(transform.position, move*distance, Color.green, 5);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, move, out hit, distance))
                {
                    Debug.Log(hit.collider.name);
                    MoveToNode(hit.collider.gameObject.GetComponent<Node>());
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
    }

    //Implement mouse interaction method here
    public void MoveByMouse(int dir)
    {
        switch (dir)
        {
            case 0:
                move = new Vector3(0, 0, 1);
                break;
            case 1:
                move = new Vector3(1, 0, 0);
                break;
            case 2:
                move = new Vector3(0, 0, -1);
                break;
            case 3:
                move = new Vector3(-1, 0, 0);
                break;
        }

        schmovin = false;
    }

    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }
}