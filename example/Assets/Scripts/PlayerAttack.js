private var attacks;
private var lastAttack = 0;
private var busy = false;

function Start(){
	animation["leftattack"].speed = animation["leftattack"].clip.length;
	animation["rightattack"].speed = animation["rightattack"].clip.length;
	animation["powerattack"].speed = animation["powerattack"].clip.length;
	attacks = new Array();
	attacks.push("leftattack");
	attacks.push("rightattack");
	attacks.push("powerattack");
}

function Update () {
	var controller : PlayerController = GetComponent(PlayerController);
	
	if(!busy && Input.GetButton("Attack") && !controller.IsMoving()){
		busy = true;
		//GameObject.Find("RaycastSender").SendMessage("AttackEnemy");
		DoAttack();
		//busy = true;
		//var skeleton : SkeletonFixer = GameObject.Find("Player/Bip01").GetComponent(SkeletonFixer);
		//skeleton.Fix();
	}
}

function DoAttack()
{
	//var skeleton : SkeletonFixer = GameObject.Find("Player/Bip01").GetComponent(SkeletonFixer);

	Debug.Log("Attack");
	animation.Play(attacks[lastAttack]);
	
	yield WaitForSeconds(animation[attacks[lastAttack]].clip.length);
	//lastAttack = (lastAttack + 1) % 3;
	//yield WaitForSeconds(attackHitTime);
	//yield WaitForSeconds(attackTime - attackHitTime);
	busy = false;
	
}