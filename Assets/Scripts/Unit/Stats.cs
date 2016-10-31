using UnityEngine;
using System.Collections;

public enum DamageType{Normal }

public class Stats : MonoBehaviour {
	[Header ("Stats")]
	public float health = 100f;


    public float GetHealth() {
        return health;
    }
    public void GetDamage(float damage, DamageType damType) {
        switch (damType){
            case DamageType.Normal:
                health -= damage;
                break;

        }
        if (health <= 0f) {
            Die();
        }
    }
    private void Die() {

    }
	
}
