@script AddComponentMenu("Camera-Control/Camera Controller")

var target : Transform;
private var distance = 5.0;
private var xSpeed = 200.0;
private var ySpeed = 100.0;
private var yMinLimit = -40;
private var yMaxLimit = 80;
private var cameraMode = "third";
private var zoomMinLimit = 2;
private var zoomMaxLimit = 8;

private var x = 0.0;
private var y = 0.0;

function Start()
{
	var angles = transform.eulerAngles;
	x = angles.y;
	y = angles.x;
}

function LateUpdate()
{
	if(Input.GetButton("CameraToggleFirst"))
	{
		cameraMode = "first";
		distance = 0.0;
	}
	if(Input.GetButton("CameraToggleThird"))
	{
		cameraMode = "third";
		distance = 1.0;
	}
	if(cameraMode == "third")
	{
		var characterRotation = target.transform.eulerAngles.y;
		var cameraRotation = transform.eulerAngles.y;
		
		if(Input.GetButton("CameraZoomIn") && distance > zoomMinLimit)
		{
			distance = distance - 1 / (xSpeed * 0.02);
		}
		if(Input.GetButton("CameraZoomOut") && distance < zoomMaxLimit)
		{
			distance = distance + 1 / (xSpeed * 0.02);
		}
		
		x += Input.GetAxis("CameraX") * xSpeed * 0.02;
		y += Input.GetAxis("CameraY") * ySpeed * 0.02;
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		var rotation;
		var position;
		
		if(Input.GetAxis("Vertical") || Input.GetAxis("Horizontal"))
		{
			rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(y, characterRotation, 0), Time.deltaTime * 3);
			position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
			x = characterRotation;
		}
		else
		{
			rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(y, x, 0), Time.deltaTime * 3);
			position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
		}
		
		transform.rotation = rotation;
		transform.position = position;
	}
	else
	{
		characterRotation = target.transform.eulerAngles.y;
		x = characterRotation;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, characterRotation, 0), Time.deltaTime * 3);
		transform.position = transform.rotation * Vector3(0.0, 0.0, -distance) + target.position;
	}
}

static function ClampAngle(angle : float, min : float, max : float)
{
	if(angle < -360)
	{
		angle += 360;
	}
	if(angle > 360)
	{
		angle -= 360;
	}
	
	return Mathf.Clamp(angle, min, max);
}