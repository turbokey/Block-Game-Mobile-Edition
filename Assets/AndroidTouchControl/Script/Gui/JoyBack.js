//This script create visible effect when you touch Joystick

var joybackButton: GUITexture;

function Start(){
    joybackButton.color.a =.10;
    joybackButton.pixelInset = Rect (-Screen.height/6,-Screen.height/8,Screen.height/4, Screen.height/4);
}

function CustomUpdate () {
	
   var count = Input.touchCount;
   for(var i : int = 0;i < count; i++)
   {	
    	var firstTouch = Input.GetTouch(i);
        if (firstTouch.phase == TouchPhase.Began) 
        {
           if (joybackButton.HitTest(firstTouch.position))
           {	          	
           	    joybackButton.color.a =.50;    
           }
        }
        if (firstTouch.phase == TouchPhase.Ended) 
        {
           		joybackButton.color.a =.10;     
        }
    }
}