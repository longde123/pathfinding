function Update ()
{
	flashLight();
}

function flashLight()
{
	gameObject.light.intensity = 0;
	yield WaitForSeconds(0.1);
	gameObject.light.intensity = 1;
	yield WaitForSeconds(0.1);
	gameObject.light.intensity = 0;
	yield WaitForSeconds(0.1);
	gameObject.light.intensity = 3;
	yield WaitForSeconds(0.5);
	gameObject.light.intensity = 0;
	yield WaitForSeconds(0.1);
	gameObject.light.intensity = 3;
	yield WaitForSeconds(0.5);
	gameObject.light.intensity = 5;
}