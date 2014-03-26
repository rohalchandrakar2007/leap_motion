using UnityEngine;
using System.Collections;
using Leap;
using System;

//using Assets;


public class MainScript : MonoBehaviour
{
    
    //Controller controller;
    //var sphere:GameObject = new GameObject();
    #region             all the variables declaration
    GlowPlane[] gpArray = new GlowPlane[64];                          // for each box in the  chess board
    Controller controller;
    float actualCordX, actualCordZ,actualCordY;
    public GameObject pla;
    public GameObject piece;
    public static Vector3 piece_pos;
    public static bool grabbed =false;
    public GameObject parent;
    public bool isHandOpen;
    private Vector3 temp;
    public GameObject virtualHand = new GameObject();
    const int DIVIDE_FACTOR = 6;
    private float t;
    private double currtime;
    private float temptime;
    private double timeStamp;
    private Quaternion initQuit;
    private Vector3 currVirtualHandPosition;
    private long ideal_count=0;
    private GameObject dust;
    public UnityEngine.Random randnum = new UnityEngine.Random();
    private Vector3 piecePositionDrawMethod, handPositionDrawMetho, planePositionDrawMethod;
    private Hand hand, hand1;
    private Frame frame;
    private GestureList gestLst;
    private  Vector3 OFFSET_PEEP_POSITION_WHLIE_GRABBED;// = new Vector3(-0.4f, 1.75f, 0.2f);
    private  Vector3 OFFSTE_HAND_POSITION_WRT_PIECE;// = new Vector3(-0.2f, 0.1f, 0);
    private bool endPointTrigger = false;
    private bool transitionComplete = true;
    private Vector3 startPoint, endPoint ,stablePieceposition ,stableVelocity ;
    private bool motionForward = true;
    
    #endregion

    #region   constants for the script
    private const float BOARD_WIDTH_X = 4.76f;
    private const float BOARD_LENGTH_Z = 4.80f;
    #endregion
    void Start()
    {
       
        /*  Initialize all the variables abd set up the board and animation*/
        InitBoardGame();
        OFFSET_PEEP_POSITION_WHLIE_GRABBED = new Vector3(-0.4f, 1.75f, 0.2f);
        OFFSTE_HAND_POSITION_WRT_PIECE = new Vector3(-0.2f, 0.1f, 0);
        piece.animation["hold"].speed = 2.5f;
        piece.animation["queen_peep"].speed = 0.4f;
        piece.animation["queen_peep"].wrapMode = WrapMode.Loop;
        piece.animation["ideal_low"].speed = 1.0f;
        piece.animation["ideal_low"].wrapMode = WrapMode.Loop;

        //piece.transform.Rotate(Ve);
      
    }
    void Update()
    {
        //Debug.Log(gpArray[21].X_center.z.ToString()); 
       // if (!motionForward)
       // {
       //     //stableVelocity = ((new Vector3(0, 0, 0) - new Vector3(piece.transform.position.x, 0, piece.transform.position.z)));
       //     stablePieceposition = gpArray[21].X_center;
       //     stableVelocity = ((new Vector3(stablePieceposition.x,0,stablePieceposition.z) - new Vector3(piece.transform.position.x, 0, piece.transform.position.z)));
            
       //     //piece.rigidbody.velocity = new Vector3(0,0,10);
       //     //piece.rigidbody.velocity = new Vector3(0, piece.rigidbody.velocity.y, 0 - piece.transform.position.z);
            
       //     Debug.Log(stablePieceposition.z.ToString());
       //     motionForward = true;
       // }
       ////Debug.Log(stablePieceposition.x.ToString()+"     "+ stablePieceposition.z.ToString());
       //LandingPosition();
        //piece.rigidbody.velocity = (new Vector3(stableVelocity.x,piece.rigidbody.velocity.y/10,stableVelocity.y) )* 10;
        //piece.rigidbody.velocity =  new Vector3(stableVelocity.x,piece.rigidbody.velocity.y,stableVelocity.z);
        //if ((0-piece.transform.position.x) < 0.01f   && (0-piece.transform.position.z)<0.01f)
        //{
        //   // piece.rigidbody.velocity = new Vector3(0, 0, 0);
        //    //piece.rigidbody.MovePosition(gpArray[getPlaneSlote(0.2f, 0.5f)].X_center);
        //   // piece.rigidbody.MovePosition(new Vector3(0,-2.77f,0));
        //}
        //piece.rigidbody.MovePosition(piece.transform.position + stableVelocity);
       SecondaryUpdate();

    }

    private void SecondaryUpdate()
    {
       try
        {
            
            frame = controller.Frame();
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
            gestLst = frame.Gestures();
            foreach (Gesture ges in gestLst)
            { 

                /*getting the SWIPE GESTURE  */
                if((ges.Type == Gesture.GestureType.TYPESWIPE)&&(piece.rigidbody.isKinematic == false))
            {
               // respawn();
            }
            }
          
            if ((!frame.Hands.Empty))
            
            {

                
                hand = frame.Hands[0];
                hand1 = frame.Hands[1];
                //actualCordX = ((((100 - (float)hand.PalmPosition.x) / 100) * BOARD_WIDTH_X) - (BOARD_WIDTH_X/2));
               // actualCordZ = ((((100 - (float)hand.PalmPosition.z) / 100) * BOARD_LENGTH_Z) - (BOARD_LENGTH_Z/2));
                actualCordX = (hand.PalmPosition.x / 100) * -2.38f;
                actualCordZ = (hand.PalmPosition.z / 100) * -2.4f;
                
                //Debug.Log(hand.PalmNormal.x.ToString());
                actualCordY = (hand.PalmPosition.y/40) - 5;


                

                
                

                #region for incase it need to move the piece discreatly on the board
                //if (piece.rigidbody.velocity.y == 0.0f)
                    //{

                    //    piece.transform.position = gpArray[getPlaneSlote(pla.transform.position.x, pla.transform.position.z)].X_center;
                    //}
                    #endregion
                 
         
               

                //pla.transform.position = gpArray[getPlaneSlote(actualCordX, -actualCordZ)].X_center;
               
                //////////////////////////     codition  when palm is open   ////////////////////////////
              
                if (hand.Fingers.Count > 2)
                {
                    
                    piece.rigidbody.isKinematic = false;
                    if (!isHandOpen)
                    {
                        virtualHand.animation.Play("release");
                        //isHandOpen = true;
                    }
                    isHandOpen = true;



                    if (grabbed == true)
                    {
                    stablePieceposition = gpArray[getPlaneSlote(piece.transform.position.x, piece.transform.position.z)].X_center;
                    stableVelocity = ((new Vector3(stablePieceposition.x, 0, stablePieceposition.z) - new Vector3(piece.transform.position.x, 0, piece.transform.position.z)));
                    //piece.rigidbody.velocity = stableVelocity*2;
                    //piece.rigidbody.MovePosition();
                    Debug.Log("Only once on releasing the piece!!!");
                    }
                    //else 
                    //{
                    //    stablePieceposition = piece.transform.position;
                    //}

                    grabbed = false;
                   
                    endPointTrigger = false; 
                    motionForward = true;
                    pla.renderer.material.color = Color.green;

                }
                                     ////////////////    codition when palm is closed/////////////////////     
                else if (hand.Fingers.Count < 3)
                {
                    if (isHandOpen)
                    {
                        virtualHand.animation.Play("handsSkin");
                       
                    }
                    isHandOpen = false;
                   
                    pla.renderer.material = Resources.Load("n", typeof(Material)) as Material;
                    if ((pla.transform.position.x == piece.transform.position.x) && ((pla.transform.position.z == piece.transform.position.z)) && (grabbed == false))
                    {
                        
                        grabbed = true;

                        if (!endPointTrigger)
                        {
                            startPoint = virtualHand.transform.position;
                            endPoint = new Vector3(piece.transform.position.x-0.3f,-1.8f,piece.transform.position.z);
                            endPointTrigger = true;
                            transitionComplete = false;
                        }
                        piece.rigidbody.isKinematic = true;

                       
                    }
                    if (grabbed == true)
                    {
                        
                     
                        if ((!motionForward  && transitionComplete )||(motionForward && transitionComplete))
                        {
                           
                            //piece.transform.position = virtualHand.transform.position - new Vector3(-0.4f, 1.15f, 0.5f);
                          
                        }
                      
                        
                    }

                }
              
               
                //pla.transform.position = gpArray[getPlaneSlote(actualCordX, -actualCordZ)].X_center;
                DrawPlane();
                DrawPiece();
                DrawHand();
                
                piece.animation.Play(GetAnimationStringPiece());
            }
            else 
            {
                virtualHand.transform.position = new Vector3(80,80,80);
            }


            
           
        }
        catch(Exception e)
        {}
    }
    private void LandingPosition()
    {
        piece.rigidbody.velocity = new Vector3(stableVelocity.x*2,piece.rigidbody.velocity.y,stableVelocity.z*2);
        if ((Math.Abs(stablePieceposition.x - piece.transform.position.x) < 0.02f || Math.Abs(stablePieceposition.z - piece.transform.position.z) < 0.02f) )
        {
            Debug.Log("fresh entered");
            piece.rigidbody.velocity = new Vector3(0,piece.rigidbody.velocity.y,0);
            if(piece.transform.position.y < -2.6f)
            piece.rigidbody.MovePosition(stablePieceposition);
        }
    }
    private void DrawPiece()
    {

        if (grabbed)
        {

            if (motionForward)
            {

            }

            else
                piece.transform.position = virtualHand.transform.position - new Vector3(-0.4f, 1.15f, 0.5f);

        }
        else //if (!piece.rigidbody.isKinematic )
        {
            LandingPosition();
           
        }
       

        if (piece.rigidbody.velocity.x == 0.0f && piece.transform.position.y < -2.6f)
        {

            //LandingPosition();
            piece.transform.position = gpArray[getPlaneSlote(stablePieceposition.x - 0.05f, stablePieceposition.z + 0.05f)].X_center - new Vector3(0,0.04f,0);

        }
    }
    private void DrawPlane()
    {
        if (grabbed)
        {
            pla.transform.position = gpArray[getPlaneSlote(piece.transform.position.x, piece.transform.position.z)].X_centerforplane;
        }
        else 
        {
            pla.transform.position = gpArray[getPlaneSlote(actualCordX, -actualCordZ)].X_centerforplane;
        }
    }
    private void DrawHand()
    {
        if (grabbed)
        {
            if (grabbed && !transitionComplete)
            {
                if (virtualHand.transform.position == endPoint)
                {
                    motionForward = false;
                }
                if (!motionForward && virtualHand.transform.position == startPoint)
                {
                    transitionComplete = true;
                }
                if (motionForward)
                {
                    iTween.MoveTo(virtualHand, endPoint, 0.25f);
                }
                else
                {
                    iTween.MoveTo(virtualHand, startPoint, 0.25f);
                }

            }

            else
                virtualHand.transform.position = new Vector3(actualCordX, actualCordY, -actualCordZ) + OFFSTE_HAND_POSITION_WRT_PIECE;

        }
        else
            virtualHand.transform.position = new Vector3(actualCordX, actualCordY, -actualCordZ) + OFFSTE_HAND_POSITION_WRT_PIECE;


    }
    private void DrawcallsPieces()
    {   
        piece.transform.position = piecePositionDrawMethod;

    }
    private Vector3 Vector3(int p1, int p2, int p3)
    {
        throw new System.NotImplementedException();
    }
    private String GetAnimationStringPiece()
    {
        String tempAnimationString = "ideal_low";
        if ((grabbed == false))
        {
            
            if ((Time.time % 5) < 2)
            {
                tempAnimationString = "queen_peep";
            }
            else
            {
                tempAnimationString = "ideal_low";
            }

        }
       
        if (grabbed && (hand.Fingers.Count < 3))
        {
            tempAnimationString = "hold";
        }
        

        return tempAnimationString;

    }
    private String GetAnimationStringHand()
    {
        String tempAnimationString ="";
        if ((!isHandOpen) && (hand.Fingers.Count > 2))
        {
            tempAnimationString = "release";
        }

        if (isHandOpen && (hand.Fingers.Count < 3))
        {
            tempAnimationString = "handsSkin";
        }
        return tempAnimationString;
    }
    public int getPlaneSlote(float localx , float localz)
    {
        for (int planeCount = 0; planeCount < 64; planeCount++)
        {
            if((gpArray[planeCount].X_range_low<localx)&& (gpArray[planeCount].X_range_high>localx)&&(gpArray[planeCount].Z_range_low<localz)&&(gpArray[planeCount].Z_range_high>localz))
            {
                return planeCount;
            }

        }
        return 100;
    }
    private void respawn()
    {
        piece.transform.position = gpArray[23].X_center;
        piece.transform.rotation = initQuit;
        grabbed = false;
        isHandOpen = true;
        piece.rigidbody.isKinematic = true;
    }
    private Vector3 animateHand(Vector3 sartCord , Vector3 endCord)
    {
        Vector3 tepcordVector = sartCord - endCord;
        return  ((-1)*tepcordVector / 10)+sartCord ;
    }
    private void InitBoardGame()
    {

        t = Time.time;
        int count = 0;
        isHandOpen = true; ;
        controller = new Controller();
        pla.transform.position = new Vector3(0, -2.70f, 0);

        for (float i = -2.38f; i < 2.38; i = i + 0.595f)
        {
            for (float j = -2.4f; j < 2.4; j = j + 0.6f)
            {

                gpArray[count] = new GlowPlane(i, i + 0.595f, j, j + 0.6f);
                count++;
            }

        }

        piece.transform.position = gpArray[23].X_center;
        piece_pos = gpArray[23].X_center;
        stablePieceposition = gpArray[23].X_center;
        initQuit = piece.transform.rotation;
       // virtualHand.transform.Rotate(new Vector3(0,0,1),330,Space.);
        //OFFSET_PEEP_POSITION_WHLIE_GRABBED = new Vector3(-0.4f, 1.75f, 0.2f);
        //OFFSTE_HAND_POSITION_WRT_PIECE = new Vector3(-0.2f, 0.1f, 0);
        piece.rigidbody.isKinematic = true;
        //piece.animation["hold"].speed = 2.5f;
        //piece.animation["queen_peep"].speed = 0.4f;
        //piece.animation["queen_peep"].wrapMode = WrapMode.Loop;
        //piece.animation["ideal_peep"].speed = 1.0f;
        //piece.animation["ideal_peep"].wrapMode = WrapMode.Loop;
    
    }
}

public class GlowPlane
{
    public float X_range_low;
    public float X_range_high;
    public float Z_range_low;
    public float Z_range_high;
    public Vector3 X_center , X_centerforplane;
    public  GlowPlane(float X_range_l,float X_range_h,float Z_range_l,float Z_range_h)
{
    X_range_low = X_range_l;
    X_range_high = X_range_h;
    Z_range_low = Z_range_l;
    Z_range_high = Z_range_h;
        X_center.x= (X_range_l+X_range_h)/2;
        X_center.z = (Z_range_l+Z_range_h)/2;
        X_center.y = -2.73f;
        X_centerforplane.x = (X_range_l + X_range_h) / 2;
        X_centerforplane.z = (Z_range_l + Z_range_h) / 2;
        X_centerforplane.y = -2.73f;
             
}
    

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 50), "Top-left");
        
    }
}

