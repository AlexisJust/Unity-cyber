using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{

    public class StatesManager : MonoBehaviour
    {

        public ControllerStates states;
        public ControllerStats stats;
        public InputVariables inp;
        public WeaponManager w_manager;
        public ResourcesManager r_manager;
        public Character character;
        public ObjectPooler objPool;

        int health = 100;

        [System.Serializable]
        public class InputVariables
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 aimPosition;
            public Vector3 rotateDirection;
            public float targetSpread;
        }

        [System.Serializable]
        public class ControllerStates
        {
            public bool onGround;
            public bool isAiming;
            public bool isCrouching;
            public bool isRunning;
            public bool isInteracting;
            public bool isDead;

        }

        #region References
        public Animator anim;
        public GameObject activeModel;
        [HideInInspector]
        public AnimatorHook a_hook;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public Collider controllerCollider;

        List<Collider> ragdollColliders = new List<Collider>();
        List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        public LayerMask ignoreLayers;
        public LayerMask ignoreForGround;

        //[HideInInspector]
        //public Transform referencesParent;

        [HideInInspector]
        public Transform mTransform;
        public CharState curStates;
        public float delta;



        #endregion

        #region Init

        public void LoadPlayerProfile(PlayerProfile p)
        {
            w_manager.mw_id = p.mw_id.value;
            w_manager.sw_id = p.sw_id.value;

            character = GetComponent<Character>();
            character.isFemale = !p.isMale;
            character.outfitId = p.outfitID.value;
            character.maskObj = r_manager.GetMask(p.mask_id.value);
        }

        public void Init()
        {
            if(r_manager == null)
            {
                r_manager = Resources.Load("Resources Manager") as ResourcesManager;
            }

            if(objPool == null)
            {
                objPool = Resources.Load("Object Pooler") as ObjectPooler;
            }
            
            mTransform = this.transform;
            SetupAnimator();
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            controllerCollider = GetComponent<Collider>();
            character = GetComponent<Character>();


            SetupRagdoll();

            ignoreLayers = ~(1 << 9);
            ignoreForGround = ~(1 << 9 | 1 << 10);

            a_hook = activeModel.AddComponent<AnimatorHook>();
            a_hook.Init(this);

            Init_WeaponManager();

            if (character != null)
                character.Init(this);

        }

        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            if (anim == null)
                anim = activeModel.GetComponentInChildren<Animator>();

            anim.applyRootMotion = false;
        }

        void SetupRagdoll()
        {
            Rigidbody[] rigids = activeModel.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody r in rigids)
            {
                if (r == rigid)
                {
                    continue;
                }

                Collider c = r.gameObject.GetComponent<Collider>();
                c.isTrigger = true;
                ragdollRigids.Add(r);
                ragdollColliders.Add(c);
                r.isKinematic = true;
                r.gameObject.layer = 10;
            }
        }

        #endregion

        #region FixedUpdate

        public void FixedTick(float d)
        {
            delta = d;
            switch (curStates)
            {
                case CharState.normal:
                    states.onGround = OnGround();
                    if (states.isAiming)
                        MovementAiming();

                    else
                        MovementNormal();

                    RotationNormal();
                    break;
                case CharState.onAir:
                    rigid.drag = 0;
                    states.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }


        void MovementNormal()
        {
            if (inp.moveAmount > 0.05f)
                rigid.drag = 0;
            else
                rigid.drag = 4;

            float speed = stats.walkSpeed;
            if (states.isRunning)
                speed = stats.runSpeed;
            if (states.isCrouching)
                speed = stats.crouchSpeed;

            Vector3 dir = Vector3.zero;
            dir = mTransform.forward * (speed * inp.moveAmount);
            rigid.velocity = dir;
        }

        void RotationNormal()
        {
            if (!states.isAiming)
                inp.rotateDirection = inp.moveDirection;

            Vector3 targetDir = inp.rotateDirection;
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = mTransform.forward;

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(mTransform.rotation, lookDir, stats.rotateSpeed * delta);
            mTransform.rotation = targetRot;
        }

        void MovementAiming()
        {
            float speed = stats.aimSpeed;
            Vector3 v = inp.moveDirection * speed;
            rigid.velocity = v;
        }

        #endregion

        #region Update

        float rT;

        public void Tick(float d)
        {
            delta = d;

            switch (curStates)
            {
                case CharState.normal:
                    states.onGround = OnGround();
                    HandleAnimationAll();
                    a_hook.Tick();
                    w_manager.Tick(delta);

                    if (states.isInteracting)
                    {
                        rT += delta;
                        if (rT > 3)
                        {
                            states.isInteracting = false;
                            rT = 0;
                        }
                    }
                    break;
                case CharState.onAir:
                    states.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        void HandleAnimationAll()
        {
            anim.SetBool(StaticStrings.sprint, states.isRunning);
            anim.SetBool(StaticStrings.aiming, states.isAiming);
            anim.SetBool(StaticStrings.crouch, states.isCrouching);

            if (states.isAiming)
            {
                HandleAnimationsAiming();
            }
            else
            {
                HandleAnimationsNormal();
            }
        }

        void HandleAnimationsNormal()
        {
            if (inp.moveAmount > 0.05f)
                rigid.drag = 0;
            else
                rigid.drag = 4;

            float anim_v = inp.moveAmount;
            anim.SetFloat(StaticStrings.vertical, anim_v, 0.15f, delta);
        }

        void HandleAnimationsAiming()
        {
            float v = inp.vertical;
            float h = inp.horizontal;

            anim.SetFloat(StaticStrings.horizontal, h, 0.2f, delta);
            anim.SetFloat(StaticStrings.vertical, v, 0.2f, delta);
        }
        #endregion

        #region Manager Functions

        public void Init_WeaponManager()
        {
            CreateRuntimeWeapon(w_manager.mw_id, ref w_manager.m_weapon); //Create Primary Weapon
            CreateRuntimeWeapon(w_manager.sw_id, ref w_manager.s_weapon); //Create Secondary Weapon
            EquipRuntimeWeapon(w_manager.m_weapon); //Equip Primary Weapon
            w_manager.isMain = true;
        }

        public void CreateRuntimeWeapon(string id, ref RuntimeWeapon r_w_m)
        {

            if (r_w_m.m_instance != null)
                Destroy(r_w_m.m_instance);

            if (string.IsNullOrEmpty(id))
                return;

            Weapon w = r_manager.GetWeapon(id);
            RuntimeWeapon rw = r_manager.runtime.WeaponToRuntimeWeapon(w);

            GameObject go = Instantiate(w.modelPrefab);
            rw.m_instance = go;
            rw.w_actual = w;
            rw.w_hook = go.GetComponent<WeaponHook>();
            //go.SetActive(false);

            Transform b = character.GetBone(rw.w_actual.holsterBone);
            go.transform.parent = b;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;

            r_w_m = rw;
        }

        public void EquipRuntimeWeapon(RuntimeWeapon rw) //Switch weapon (Primary / secondary) + Spawn / dispawn weapon on holster
        {

            if (w_manager.GetCurrent() != null)
            {
                UnequipWeapon(w_manager.GetCurrent());
            }

            if (rw.m_instance == null)
                return;

            Transform p = anim.GetBoneTransform(HumanBodyBones.RightHand);
            rw.m_instance.transform.parent = p;
            rw.m_instance.transform.localPosition = Vector3.zero;
            rw.m_instance.transform.localEulerAngles = Vector3.zero;
            rw.m_instance.transform.localScale = Vector3.one;

            rw.m_instance.SetActive(true);
            a_hook.EquipWeapon(rw);
            rw.w_hook.Init();

            anim.SetFloat(StaticStrings.weaponType, rw.w_actual.weaponType);
            w_manager.SetCurrent(rw);
        }

        public void UnequipWeapon(RuntimeWeapon rw) //Switch weapon (Primary / secondary) + Spawn / dispawn weapon on holster
        {
            Transform b = character.GetBone(rw.w_actual.holsterBone);
            rw.m_instance.transform.parent = b;
            rw.m_instance.transform.localPosition = Vector3.zero;
            rw.m_instance.transform.localEulerAngles = Vector3.zero;
            rw.m_instance.transform.localScale = Vector3.one;
        }

        public bool ShootWeapon(float t)
        {
            bool retVal = false;

            RuntimeWeapon c = w_manager.GetCurrent();

            if (c.curAmmo > 0)
            {

                if (t - c.lastFired > c.w_actual.stats.base_fireRate)
                {
                    c.lastFired = t;
                    retVal = true;
                    c.ShootWeapon();
                    a_hook.RecoilAnim();
                    HandleShootingLogic(c);
                }
            }

            return retVal;
        }

        void HandleShootingLogic(RuntimeWeapon c)
        {

            Vector3 origin = a_hook.aimPivot.position;
            origin += a_hook.aimPivot.forward * 0.5f;

            
            for (int i=0; i < c.w_actual.stats.bulletAmount; i++)
            {
                Vector3 targetPosition = inp.aimPosition;
                Vector3 offset = mTransform.forward;
                offset += mTransform.up;

                float spread = c.w_actual.stats.GetSpread();
                spread *= 0.01f;

                if (inp.targetSpread < 0.1f)
                    inp.targetSpread = 1;

                spread *= inp.targetSpread;
                spread *= 0.5f;

                offset.x *= Random.Range(-spread, spread);
                offset.y *= Random.Range(-spread, spread);
                offset.z *= Random.Range(-spread, spread);
                targetPosition += offset;

                Vector3 targetDirection = targetPosition - origin;
                bool isHit = false;
                RaycastHit hit = Ballistics.RaycastShoot(origin, targetDirection, ref isHit, ignoreLayers);

                if (isHit)
                {
                    HandleDamage(hit, c, targetDirection);
                }
            }
        }

        void HandleDamage(RaycastHit h, RuntimeWeapon c, Vector3 d)
        {

            Rigidbody rigid = h.transform.GetComponent<Rigidbody>();
            if(rigid != null)
            {
                if (!rigid.isKinematic)
                    rigid.AddForceAtPosition(d * 20, h.point, ForceMode.Impulse);
            }

            StatesManager st = h.transform.GetComponentInParent<StatesManager>();
            if(st != null)
            {
                if(st != this)
                {
                    if (st.states.isDead)
                        return;

                    HitPosition hitPos = h.transform.GetComponent<HitPosition>();
                    GameObject p = objPool.RequestObject("BloodSplat_FX");
                    p.transform.position = h.point;
                    st.GetHit(this, c, hitPos);

                }
            }
            else
            {
                GameObject p = objPool.RequestObject("bullet_hit");
                p.transform.position = h.point;
                p.transform.LookAt(a_hook.aimPivot.position);
            }
        }

        public bool Reload()
        {
            bool retVal = false;
            RuntimeWeapon c = w_manager.GetCurrent();
            if (c.curAmmo < c.w_actual.stats.base_magAmmo)
            {
                if (c.w_actual.stats.base_magAmmo <= c.curCarrying)
                {
                    c.curAmmo = c.w_actual.stats.base_magAmmo;
                    c.curCarrying -= c.curAmmo;
                }
                else
                {
                    c.curAmmo = c.curCarrying;
                    c.curCarrying = 0;
                }

                retVal = true;
                anim.CrossFade("Rifle Reload", 0.2f);
                states.isInteracting = true;

                if (c.w_actual.audio.reload != null)
                {
                    c.w_hook.PlayAudio(c.w_actual.audio.reload);
                }
                
            }


            return retVal;
        }

        public void SwitchWeapon()
        {
            if (states.isInteracting)
                return;

            w_manager.isMain = !w_manager.isMain;
            EquipRuntimeWeapon((w_manager.isMain) ? w_manager.m_weapon : w_manager.s_weapon);
        }

        public void GetHit(StatesManager s, RuntimeWeapon c, HitPosition hp)
        {
            HumanBodyBones t = HumanBodyBones.Chest;

            if (hp != null)
            {
                t = hp.bone;
            }

            float baseDamage = c.stats.GetDamage();
            //float baseDamage = 25;
            

            switch (t)
            {
                case HumanBodyBones.Head:
                    anim.Play("damage 2");
                    baseDamage *= 2;
                    baseDamage += Random.Range(-5, 5);
                    break;
                case HumanBodyBones.Chest:
                    anim.Play("damage 2");
                    baseDamage += Random.Range(-12, 12);
                    break;
                case HumanBodyBones.LeftUpperArm:
                    baseDamage *= .5f;
                    anim.Play("damage 1");
                    baseDamage += Random.Range(-12, 12);
                    break;
                case HumanBodyBones.RightUpperArm:
                    baseDamage *= .5f;
                    anim.Play("damage 1");
                    baseDamage += Random.Range(-12, 12);
                    break;
            }

            health -= Mathf.RoundToInt(baseDamage);
            if(health < 0)
            {
                a_hook.enabled = false;
                StartCoroutine(EnableRagdoll_AfterDelay(.1f));
                states.isDead = true;
            }

        }

        IEnumerator EnableRagdoll_AfterDelay(float t)
        {
            yield return new WaitForSeconds(t);
            //controllerCollider.enabled = false;
            EnableRagdoll_Actual();
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
            Destroy_AfterDelay(30f);
        }

        IEnumerator Destroy_AfterDelay(float t)
        {
            activeModel.transform.parent = null;
            Destroy(this.gameObject);
            yield return new WaitForSeconds(t);
            
        }

        void EnableRagdoll_Actual()
        {
            for (int i = 0; i < ragdollColliders.Count; i++)
            {
                ragdollColliders[i].isTrigger = false;
                ragdollRigids[i].isKinematic = false;
            }
        }

        #endregion

        public float minDistance = .2f;
        public float maxDistance = .5f;

        bool OnGround() //Movement on ground    The commented part is not working on Mesh collider
        {
            bool retVal = false;

            Vector3 origin = mTransform.position;
            origin.y += 0.6f;
            Vector3 dir = -Vector3.up;
            float dis = 0.7f;

            RaycastHit forward;
            RaycastHit backward;

            float targetDistance = Mathf.Lerp(minDistance, maxDistance, inp.moveAmount);

            Vector3 f = origin + mTransform.forward * targetDistance;
            Vector3 b = origin + mTransform.forward * -targetDistance;
            Vector3 forwardPosition = mTransform.position;
            Vector3 backwardPosition = mTransform.position;

            if(Physics.Raycast(f,dir,out forward, dis, ignoreForGround))
            {
                retVal = true;
                forwardPosition = forward.point;
            }

            if (Physics.Raycast(b, dir, out backward, dis, ignoreForGround))
            {
                retVal = true;
                backwardPosition = backward.point;
            }

            if (retVal)
            {
                float d = forwardPosition.y - backwardPosition.y;
                d *= 0.5f;
                Vector3 tp = mTransform.position;
                tp.y = d + backwardPosition.y;
                mTransform.position = tp;
            }
            
             return retVal;

            /*
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreForGround))
            {
                Vector3 tp = hit.point;
                mTransform.position = tp;
                return true;
            }

            return false;
            */
            


        }

        public enum CharState
        {
            normal, onAir, cover, vaulting
        }

    }
}
