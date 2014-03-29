private var walkSpeed : float = 5.0;
private var gravity = 100.0;
private var moveDirection : Vector3 = Vector3.zero;
private var charController : CharacterController;
private var isMoving = false;
private var fixed = false;
private var attacks;
private var lastAttack = 0;
//private var skeleton : SkeletonFixer;
function Start()
{
	charController = GetComponent(CharacterController);
	animation.wrapMode = WrapMode.Loop;
	animation["leftattack"].speed = animation["leftattack"].clip.length;
	animation["rightattack"].speed = animation["rightattack"].clip.length;
	attacks = new Array();
	attacks.push("leftattack");
	attacks.push("rightattack");
	//skeleton = GameObject.Find("Player/Bip01").GetComponent(SkeletonFixer);
}

function Update()
{
	if(charController.isGrounded == true){
		if(Input.GetAxis("Vertical") > .1){
			isMoving = true;
			if(Input.GetButton("Run")){
				animation.CrossFade("attackrunforward");
				walkSpeed = 10;
			}else{
				animation["attackwalkforward"].speed = 1;
				animation.CrossFade("attackwalkforward");
				walkSpeed = 5;
			}
			fixed = false;
		}else if(Input.GetAxis("Vertical") < -.1){
			isMoving = true;
			animation["attackwalkforward"].speed = -1;
			animation.CrossFade("attackwalkforward");
			walkSpeed = 5;
			fixed = false;
		}else if(Input.GetButton("Attack")){
			DoAttack();
			GameObject.Find("RaycastSender").SendMessage("AttackEnemy");
		}else{
				//skeleton.Fix();
				animation.CrossFade("attackidle");
				isMoving = false;
		}
		
		// Create an animation cycle for when the character is turning on the spot
		if(Input.GetAxis("Horizontal") && !Input.GetAxis("Vertical")){
			animation.CrossFade("attackwalkforward");
			isMoving = true;
		}
		
		transform.eulerAngles.y += Input.GetAxis("Horizontal") * 4;
		
		// Calculate the movement direction (forward motion)
		moveDirection = Vector3(0,0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		
	}
	
	moveDirection.y -= gravity * Time.deltaTime;
	charController.Move(moveDirection * (Time.deltaTime * walkSpeed));
	
}

function DoAttack()
{
	animation.Play(attacks[lastAttack]);
	yield WaitForSeconds(animation[attacks[lastAttack]].clip.length);
	lastAttack = (lastAttack + 1) % 2;
}

function IsMoving()
{
	return isMoving;
}