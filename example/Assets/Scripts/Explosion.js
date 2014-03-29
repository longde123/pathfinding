var explosive : Transform;

function Update () {
}

function OnTriggerEnter(collision : Collider)
{
	Instantiate(explosive, gameObject.transform.position, Quaternion.identity);
	yield WaitForSeconds(0.3);
	
	gameObject.rigidbody.AddExplosionForce(20, gameObject.transform.position, 5);
	gameObject.rigidbody.useGravity = true;
	audio.Play();
}