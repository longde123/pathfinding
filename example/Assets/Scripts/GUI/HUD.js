
var guiSkin : GUISkin;
private var hp;
private var level;
private var expPoints;
private var nextLevel;
var image : Texture2D;

function OnGUI () {
	// Set up gui skin
	GUI.skin = guiSkin;

	var status = "HP: " + hp + "\n\nLevel: " + level + "\n\nExperience: " + expPoints + "\n\nNext Level: " + nextLevel;
	// Our GUI is laid out for a 1920 x 1200 pixel display (16:10 aspect). The next line makes sure it rescales nicely to other resolutions.
	//GUI.matrix = Matrix4x4.TRS (Vector3(0, 0, 0), Quaternion.identity, Vector3 (Screen.height / resolution, Screen.height / resolution, 1)); 
	GUI.Label(Rect (0,Screen.height - 150, 600, 150),GUIContent(status, image));
	
	
}

	function setHpPoints(hpPoints : int)
	{
		hp = hpPoints;
	}

	function setLevel(lv : int)
	{
		level = lv;

	}

	function setExpPoints(exp : int)
	{
		expPoints = exp;
	}

	function setNextLevel(nextLv : int)
	{
		nextLevel = nextLv;
	}

/*
function DrawLabelBottomRightAligned (pos : Vector2, text : String)
{
	var scaledResolutionWidth = resolution / Screen.height * Screen.width;
	
}
*/