private var characterController : CharacterController;
private var test : Component;

function Start()
{
	animation.wrapMode = WrapMode.Loop;
	characterController = GetComponent(CharacterController);
	animation["idle"].layer = 1;
	animation["run"].layer = 1;
	animation["attack"].layer = 2;
	
}
function Update ()
{
	if(characterController.velocity.z > .2 || characterController.velocity.z < -.2 || characterController.velocity.x > .2 || characterController.velocity.x < -.2)
	{
		animation.CrossFade("run");
	}
	else
	{
		animation.CrossFade("idle");
	}
}

function attack()
{
	animation.Stop("run");
	animation.CrossFade("attack");
}