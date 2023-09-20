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

    float up;
    float down;
    float left;
    float right;
    float hMove;
    float vMove;
    Vector3 move;

    bool schmovin = true;
    bool moved = false;

    public Color green = Color.green;
    public Color red = Color.red;
    public Color white = Color.white;

    [SerializeField] GameObject upButt;
    [SerializeField] GameObject downButt;
    [SerializeField] GameObject leftButt;
    [SerializeField] GameObject rightButt;

    float[] timer = {0,0,0,0};
    float time = 0.2f;

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
    void FixedUpdate()
    {
        Debug.Log(hMove);

        TargetNode = TargetNode == null ? CurrentNode : TargetNode;

        for (int i=0; i < 4; i++)
        {
            if (timer[i] > 0)
            {
                timer[i] -= 1 * Time.deltaTime;
            }
            else
            {
                switch (i)
                {
                    case 0:
                        upButt.GetComponent<Image>().color = white;
                        break;
                    case 1:
                        rightButt.GetComponent<Image>().color = white;
                        break;
                    case 2:
                        downButt.GetComponent<Image>().color = white;
                        break;
                    case 3:
                        leftButt.GetComponent<Image>().color = white;
                        break;
                }
            }
        }

        if (moving == false)
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                moved = false;
            }

            if (schmovin == true)
            {
                //Implement inputs and event-callbacks here
                /*up = Input.GetButtonDown("Up") ? 1 : 0;
                down = Input.GetButtonDown("Down") ? -1 : 0;
                left = Input.GetButtonDown("Left") ? -1 : 0;
                right = Input.GetButtonDown("Right") ? 1 : 0;

                hMove = left + right;
                vMove = up + down;*/

                if (moved == false)
                {
                    hMove = Input.GetAxisRaw("Horizontal");
                    vMove = Input.GetAxisRaw("Vertical");
                }
                else
                {
                    hMove = 0;
                    vMove = 0;
                }

                move = new Vector3(hMove, 0, vMove);
            }
            else
            {
                schmovin = true;
            }

            if ((move != Vector3.zero) && (!moving))
            {
                float distance = 7.5f;
                Debug.DrawRay(transform.position, move*distance, Color.green, 5);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, move, out hit, distance))
                {
                    //if (hit.collider.gameObject.GetComponent<Node>() != CurrentNode)
                    MoveToNode(hit.collider.gameObject.GetComponent<Node>());

                    /*switch (Mathf.Sign(move.x))
                    {
                        case -1:
                            leftButt.GetComponent<Image>().color = green;
                            timer[3] = time;
                            break;
                        case 1:
                            rightButt.GetComponent<Image>().color = green;
                            timer[1] = time;
                            break;
                        case 0:
                            switch (Mathf.Sign(move.z))
                            {
                                case -1:
                                    downButt.GetComponent<Image>().color = green;
                                    timer[2] = time;
                                    break;
                                case 1:
                                    upButt.GetComponent<Image>().color = green;
                                    timer[0] = time;
                                    break;
                            }
                            break;
                    }*/
                    if (move.x > 0)
                    {
                        rightButt.GetComponent<Image>().color = green;
                        timer[1] = time;
                    }
                    else if (move.x < 0)
                    {
                        leftButt.GetComponent<Image>().color = green;
                        timer[3] = time;
                    }
                    else
                    {
                        if (move.z > 0)
                        {
                            upButt.GetComponent<Image>().color = green;
                            timer[0] = time;
                        }
                        else if (move.z < 0)
                        {
                            downButt.GetComponent<Image>().color = green;
                            timer[2] = time;
                        }
                    }

                    moved = true;
                }
                else
                {
                    /*switch (Mathf.Sign(move.x))
                    {
                        case -1:
                            leftButt.GetComponent<Image>().color = red;
                            timer[3] = time;
                            break;
                        case 1:
                            rightButt.GetComponent<Image>().color = red;
                            timer[1] = time;
                            break;
                        case 0:
                            switch (Mathf.Sign(move.z))
                            {
                                case -1:
                                    downButt.GetComponent<Image>().color = red;
                                    timer[2] = time;
                                    break;
                                case 1:
                                    upButt.GetComponent<Image>().color = red;
                                    timer[0] = time;
                                    break;
                            }
                            break;
                    }*/

                    if (move.x > 0)
                    {
                        rightButt.GetComponent<Image>().color = red;
                        timer[1] = time;
                    }
                    else if (move.x < 0)
                    {
                        leftButt.GetComponent<Image>().color = red;
                        timer[3] = time;
                    }
                    else
                    {
                        if (move.z > 0)
                        {
                            upButt.GetComponent<Image>().color = red;
                            timer[0] = time;
                        }
                        else if (move.z < 0)
                        {
                            downButt.GetComponent<Image>().color = red;
                            timer[2] = time;
                        }
                    }
                }
            }
        }
        else
        {
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > speed * Time.deltaTime)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
                transform.position = CurrentNode.location;
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
            moving = true;
        }
    }
}