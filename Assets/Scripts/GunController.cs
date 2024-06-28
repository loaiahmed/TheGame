
using UnityEngine;

public class GunController : MonoBehaviour
{

    public Transform cam;
    public Transform player;
    public float dmg;
    public float range;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            player.transform.forward = cam.transform.forward;
            Shoot();
        }
        
    }
    public void Shoot(){
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)){
            Debug.Log(hit.transform.name);
            EnemyStats enemy = hit.transform.GetComponent<EnemyStats>();

            if(enemy != null){
                enemy.recieveDmg(dmg);
            }
        }
    }
}
