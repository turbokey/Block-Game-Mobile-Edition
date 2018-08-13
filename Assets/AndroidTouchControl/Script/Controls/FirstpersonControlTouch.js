//This script create a character with touch control
//It include 3 control scheme, you can change control setup in inspector or via control options
//1.Full joystick control
//2.Move with joystick look tilting your devise
//3.move with joystick look by dragging finger ower the screen
//Include AutoJump abilitie when collides in horisontal axis
//Full support iOS and Android, v1.1 (more update coming soon)
//v1.1 Added virtual mouse, Added new blank X Button
//Head@Hunter

@script RequireComponent( CharacterController )
@script RequireComponent( Camera )

var moveTouchPad : Joystick; //Move Joystick
var touchFilter : GUITexture; //Touch Filter (need to ignore screen part near left joystick when Drag To Look enabled)
var rotateTouchPadDrag : GameObject; //Rotate Joystick (to hide it when joy control disabled)
var jumpTouchPad: GUITexture; //Jump Button
var xButton: GUITexture; //X Button (you can assign any action on it in X Button part)
var virtualMouseBack: GUITexture; //Virtual Mouse Background
var rotateTouchPad : Joystick; //Rotate Joystick					

var cameraPivot : Transform; //Camera transform					
public var autoJump = true; //Auto jump enabled
public var virtualMouse = false; //Jump Button follow finger 
public var joyRotation = false; //Joy rotation control scheme
public var dragToLook = false; //Drag To Look control scheme
public var tiltRotation = false; //Tilt rotation control scheme
public var showControlOptions = true; //Control options
var forwardSpeed : float = 4; //Forward move speed
var backwardSpeed : float = 1; //Backward move speed
var sidestepSpeed : float = 1; //Sidestep move speed
var jumpSpeed : float = 15; //Jump speed
var inAirMultiplier : float = 0.25;	//Moving speed when in the air				
var rotationSpeed : Vector2 = Vector2( 50, 25 ); //Rotation speed with rotation joystick
var tiltPositiveYAxis = 0.6; /////////////
var tiltNegativeYAxis = 0.4; ///Tilt speed
var tiltXAxisMinimum = 0.1;  /////////////

public var dragSensitivityX : float = 5.0f; //Drag To Look X axis speed
public var dragSensitivityY : float = 5.0f; //Drag To Look Y axis speed
public var dragInvertX = false; //Drag To Look invert X axis
public var dragInvertY = false; //Drag to Look invert Y axis

private var Timer : float = 2; //Timer for proper auto jump	
private var thisTransform : Transform; //Player transform
private var character : CharacterController; //Character
private var cameraVelocity : Vector3; //Camera velocity
private var velocity : Vector3;	//Velocity					
private var canJump = true; //Auto jump check
private var jump = false; //Jump check
                                                        
//Outo jump when collide with object in horizontal axis
function OnControllerColliderHit (hit : ControllerColliderHit) {
    //Check auto jump
    if (autoJump == true){
	    //Check if collide in horizontal axis 
		if (Mathf.Abs(hit.normal.y) < 0.5){
		   jump = true; //Jump on
		}
	}
}                                                                                           
function OnGUI() {

    //This swow how simple to change control options
    if(showControlOptions) {
        GUILayout.BeginHorizontal ("box");
        joyRotation = GUILayout.Toggle (joyRotation,  "Joystick Rotation");
        tiltRotation = GUILayout.Toggle (tiltRotation, "Tilt Rotation");
        dragToLook = GUILayout.Toggle (dragToLook, "Drag To Look");
        GUILayout.EndHorizontal ();
        GUILayout.BeginHorizontal ("box");
       autoJump = GUILayout.Toggle (autoJump, "Auto Jump");
        virtualMouse = GUILayout.Toggle (virtualMouse, "Virtual Mouse");
        GUILayout.EndHorizontal ();
    }
}                                                                                                                                                                                                                                                                                                                  
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
function Start()
{	
	thisTransform = GetComponent( Transform );
	character = GetComponent( CharacterController );
	
	//If spawn poin exist, move chartacter to it position		
	var spawn = GameObject.Find( "PlayerSpawn" );
	if ( spawn )
	thisTransform.position = spawn.transform.position;
}

function OnEndGame()
{
	moveTouchPad.Disable();
	if ( rotateTouchPad )
		rotateTouchPad.Disable();	

	this.enabled = false;
}

function Update()
{
    if ( character.isGrounded )
		velocity = Vector3.zero;
		
	//Fix camera X axis
	if(transform.localEulerAngles.x >= 80 && transform.localEulerAngles.x < 295){
         if(transform.localEulerAngles.x < 180){
              transform.localEulerAngles.x = 80.9;
         }else{
              transform.localEulerAngles.x  = 295.1;
         }
    }
    //Fix camera Z axis
    if(transform.localEulerAngles.z != 0){
       transform.localEulerAngles.z = 0;
    }
		
	var movement = thisTransform.TransformDirection( Vector3( moveTouchPad.position.x, 0, moveTouchPad.position.y ) );
    
	movement.y = 0;
	movement.Normalize();

	var absJoyPos = Vector2( Mathf.Abs( moveTouchPad.position.x ), Mathf.Abs( moveTouchPad.position.y ) );	
	if ( absJoyPos.y > absJoyPos.x )
	{
	
		if ( moveTouchPad.position.y > 0 )
			movement *= forwardSpeed * absJoyPos.y;
			
		else
			movement *= backwardSpeed * absJoyPos.y;
	}
	else
		movement *= sidestepSpeed * absJoyPos.x;
	
	//Timer for proper auto jump		
	if(Timer > 0)
	Timer -= Time.deltaTime;
		
	if(Timer < 0)
	Timer = 0;
	
	if(Timer > 0)
	jump = false;
			
	//Auto jump check
	if(Timer == 0){
	    if (character.isGrounded){
			if (jump){ 		
			 		velocity = character.velocity;
			 		velocity.y = jumpSpeed;    		
			jump = false; //Jump once
			Timer = 0.8;
			}	
	    }
	}else{			
		velocity.y += Physics.gravity.y * Time.deltaTime;
		
		movement.x *= inAirMultiplier;
		movement.z *= inAirMultiplier;
	}		
	movement += velocity;	
	movement += Physics.gravity;
	movement *= Time.deltaTime;
	character.Move( movement );
					
	//Jump part		
	if ( character.isGrounded )
	{	 
		if (Input.touchCount == 1) 
   		{	
    		var f0 = Input.GetTouch(0);
    
        	if (f0.phase == TouchPhase.Began) 
        	{
          		 if (jumpTouchPad.HitTest(f0.position))
           		 {		
			 		velocity = character.velocity;
			 		velocity.y = jumpSpeed;			
           	    	
          		 }
       		 }
    	}
    
   	 	if (Input.touchCount == 2) 
    	{		
    		var f1 = Input.GetTouch(1);
       	 	if (f1.phase == TouchPhase.Began) 
        	{    	
           		if (jumpTouchPad.HitTest(f1.position))
           		{	
			 		velocity = character.velocity;
			 		velocity.y = jumpSpeed;			
           		}
        	}
    	}
	}else{			
		velocity.y += Physics.gravity.y * Time.deltaTime;

		movement.x *= inAirMultiplier;
		movement.z *= inAirMultiplier;
	}
	movement += velocity;	
	movement += Physics.gravity;
	movement *= Time.deltaTime;
	character.Move( movement );
	
	if ( character.isGrounded )
		velocity = Vector3.zero;
		
	//Joy rotation part
	if(joyRotation) {   
	    rotateTouchPadDrag.SetActiveRecursively(true);  
		var camRotation = Vector2.zero;
		
		if ( rotateTouchPad )
		    camRotation = rotateTouchPad.position;
		    camRotation.x *= rotationSpeed.x;
		    camRotation.y *= rotationSpeed.y;
		    camRotation *= Time.deltaTime;
		    thisTransform.Rotate( 0, camRotation.x, 0, Space.World );
		    cameraPivot.Rotate( -camRotation.y, 0, 0 );
	}
	
	//Tilt rotation part	
	if (tiltRotation) {
		    rotateTouchPadDrag.SetActiveRecursively(false);   
		    var camRotationTilt = Vector2.zero;
			var acceleration = Input.acceleration;
			var absTiltX = Mathf.Abs( acceleration.x );
			if ( acceleration.z < 0 && acceleration.x < 0 ) {
				if ( absTiltX >= tiltPositiveYAxis )
					camRotationTilt.y = (absTiltX - tiltPositiveYAxis) / (1 - tiltPositiveYAxis);
				else if ( absTiltX <= tiltNegativeYAxis )
					camRotationTilt.y = -( tiltNegativeYAxis - absTiltX) / tiltNegativeYAxis;
			}
			
			if ( Mathf.Abs( acceleration.y ) >= tiltXAxisMinimum )
				camRotationTilt.x = -(acceleration.y - tiltXAxisMinimum) / (1 - tiltXAxisMinimum);
			    camRotationTilt.x *= rotationSpeed.x;
		        camRotationTilt.y *= rotationSpeed.y;
		        camRotationTilt *= Time.deltaTime;
		        thisTransform.Rotate( 0, camRotationTilt.x, 0, Space.World );
		        cameraPivot.Rotate( -camRotationTilt.y, 0, 0 );	
	}
    
    //Drag to look part
	if(dragToLook){
	
	    rotateTouchPadDrag.SetActiveRecursively(false);
        /*jumpTouchPad.transform.position.x = Input.mousePosition.x/Screen.width + 0.1;
	    jumpTouchPad.transform.position.y = Input.mousePosition.y/Screen.height - 0.5;
	    xButton.transform.position.x = Input.mousePosition.x/Screen.width + 0.093;
	    xButton.transform.position.y = Input.mousePosition.y/Screen.height - 0.72;
	    virtualMouseBack.transform.position.x = Input.mousePosition.x/Screen.width - 0.13;
	    virtualMouseBack.transform.position.y = Input.mousePosition.y/Screen.height - 0.25;*/
	    
	    if (Input.touchCount == 1) 
   		{	
   		    var f2 = Input.GetTouch(0);
        	if (f2.phase == TouchPhase.Moved) {
				if (touchFilter.HitTest(f2.position)){return;}
				if(!virtualMouse){
				    if (jumpTouchPad.HitTest(f2.position)){return;}		
				    if (xButton.HitTest(f2.position)){return;}		
                }
                
                //Adjust jump button and x button position to finger position
                if(virtualMouse){
                    jumpTouchPad.transform.position.x = f2.position.x/Screen.width + 0.1;
                    jumpTouchPad.transform.position.y = f2.position.y/Screen.height - 0.5;
                    xButton.transform.position.x = f2.position.x/Screen.width + 0.093;
	                xButton.transform.position.y = f2.position.y/Screen.height - 0.72;
	                virtualMouseBack.transform.position.x = f2.position.x/Screen.width - 0.13;
	                virtualMouseBack.transform.position.y = f2.position.y/Screen.height - 0.25;
                }
                var delta = f2.deltaPosition;
                var rotationZ = delta.x * dragSensitivityX * Time.deltaTime;
                rotationZ = dragInvertX ? rotationZ : rotationZ * -1;
                var rotationX = delta.y * dragSensitivityY * Time.deltaTime;
                rotationX = dragInvertY ? rotationX : rotationX * -1;
                transform.localEulerAngles += new Vector3(rotationX, rotationZ, 0);
            }
            
            //Back default jump button and x button position when release finger
            if (f2.phase == TouchPhase.Ended) {
                jumpTouchPad.transform.position.x = 1;
                jumpTouchPad.transform.position.y = 0;
                xButton.transform.position.x = 1;
	            xButton.transform.position.y = -0.1;
	            virtualMouseBack.transform.position.x = -10;
	            virtualMouseBack.transform.position.y = -10;
            }
        }
        
        if (Input.touchCount == 2) 
   		{	
   		    var f3 = Input.GetTouch(1);
        	if (f3.phase == TouchPhase.Moved) {
				if (touchFilter.HitTest(f3.position)){return;}
         	    if (jumpTouchPad.HitTest(f3.position)){return;}		
                if (xButton.HitTest(f3.position)){return;}	
                
                var delta2 = f3.deltaPosition;
                var rotation2Z = delta2.x * dragSensitivityX * Time.deltaTime;
                rotation2Z = dragInvertX ? rotation2Z : rotation2Z * -1;
                var rotation2X = delta2.y * dragSensitivityY * Time.deltaTime;
                rotation2X = dragInvertY ? rotation2X : rotation2X * -1;
                transform.localEulerAngles += new Vector3(rotation2X, rotation2Z, 0);
            }
        }
	}
	
	//X Button Part
	if (Input.touchCount == 1) {
	    var f4 = Input.GetTouch(0);
	    if (f4.phase == TouchPhase.Began) {
	        if (xButton.HitTest(f4.position)) {	
	        
	        //Here your action
	        
	        }
	    }
	}
	if (Input.touchCount == 2) {
	    var f5 = Input.GetTouch(1);
	    if (f5.phase == TouchPhase.Began) {
	        if (xButton.HitTest(f5.position)) {	
	        
	        //Here your action
	        
	        }
	    }
	}
}



