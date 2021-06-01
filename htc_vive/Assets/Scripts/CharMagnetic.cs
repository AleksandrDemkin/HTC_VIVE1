using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharSpell
{

    [System.Serializable]
    public struct MagnitePoint
    {
        public List<SpringJoint> JointList;
        public List<Rigidbody> RG;
        public List<ParticleSystem> HighLight;
        public Transform BlueObj, RedObj;
        public Vector3 BluePos, RedPos;
        
    }
    public class CharMagnetic : MonoBehaviour
    {
        [SerializeField] float SpellDistance = 20;
        [SerializeField] float MaxMagniteForce = 50;
        [SerializeField] MagnitePoint MagniteSpell;
        [SerializeField] Transform BlueHolder, RedHolder;
        [SerializeField] Material BlueMat, RedMat, YellowMat;
        [SerializeField] ParticleSystem hl_Reference;

        public void SetBlue(Transform trans)
        {
            MagniteSpell.BlueObj = trans;
            MagniteSpell.BluePos = trans.position;
            Highlighting(true, trans);
            CheckToJoint();
        }

        public void SetRed(Transform trans)
        {
            MagniteSpell.RedObj = trans;
            MagniteSpell.RedPos = trans.position;
            Highlighting(false, trans);
            CheckToJoint();
        }

        public void SetBlue(Vector3 trans)
        {
            MagniteSpell.BlueObj = BlueHolder;
            MagniteSpell.BluePos = trans;
            BlueHolder.position = trans;
            BlueHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }
        
        public void SetRed(Vector3 trans)
        {
            MagniteSpell.RedObj = RedHolder;
            MagniteSpell.RedPos = trans;
            RedHolder.position = trans;
            RedHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }

        private void CheckToJoint()
        {
            if (MagniteSpell.BlueObj != null && MagniteSpell.RedObj != null)
            {
                if (Vector3.Distance(MagniteSpell.RedPos, MagniteSpell.BluePos) < SpellDistance) CheckToJoint();
                else EreaseSpell();
            }
        }

        private void CreateJoint()
        {
            SpringJoint sp = MagniteSpell.BlueObj.gameObject.AddComponent<SpringJoint>();
            sp.autoConfigureConnectedAnchor = false;
            sp.anchor = Vector3.zero;
            sp.connectedAnchor = Vector3.zero;
            sp.enableCollision = true;
            sp.enablePreprocessing = false;

            sp.connectedBody = MagniteSpell.RedObj.GetComponent<Rigidbody>();

            EreaseSpell();
            MagniteSpell.JointList.Add(sp);

            Rigidbody rg = sp.GetComponent<Rigidbody>();
            MagniteSpell.RG.Add(rg);
            AddRG(sp.connectedBody);
        }
        private void AddRG(Rigidbody RG)
        {
            if (MagniteSpell.RG == null) { return; }

            for (int i = 0; i < MagniteSpell.RG.Count; i++)
            {
                if (RG == MagniteSpell.RG[i]) break;

                if (i == MagniteSpell.RG.Count - 1) { MagniteSpell.RG.Add(RG); }
                break;
            }
        }

        private void Highlighting(bool IsBlue, Transform trans)
        {
            ParticleSystem ps = Instantiate(hl_Reference, trans, false);

            if (IsBlue) ps.GetComponent<Renderer>().material = BlueMat;
            else ps.GetComponent<Renderer>().material = RedMat;

            MagniteSpell.HighLight.Add(ps);
        }

        private void EreaseSpell()
        {
            MagniteSpell.BlueObj = null;
            MagniteSpell.RedObj = null;

            for(int i = 0; i < MagniteSpell.HighLight.Count; i++)
                MagniteSpell.HighLight[i].GetComponent<Renderer>().material = YellowMat;
        }

        public void DestroyAllJoints()
        {
            for (int i = 0; i < MagniteSpell.JointList.Count; i++)
            {
                Destroy(MagniteSpell.JointList[i]);
            }

            for (int i = 0; i < MagniteSpell.RG.Count; i++)
            {
                MagniteSpell.RG[i].angularDrag =0.05f;
                MagniteSpell.RG[i].drag = 0;
                MagniteSpell.RG[i].WakeUp();
            }
            MagniteSpell.JointList.Clear();
            MagniteSpell.RG.Clear();
            EreaseSpell();

            for (int i = 0; i < MagniteSpell.HighLight.Count; i++)
                Destroy(MagniteSpell.HighLight[i]);
            
            MagniteSpell.HighLight.Clear();
            DisableHolders();
        }

        private void DisableHolders()
        {
            BlueHolder.GetChild(0).gameObject.SetActive(false);
            RedHolder.GetChild(0).gameObject.SetActive(false);
        }

        public void ChangeSpringPower(float fNum)
        {
            if (MagniteSpell.JointList.Count > 0)
            {
                for (int i = 0; i < MagniteSpell.JointList.Count; i++)
                {
                    MagniteSpell.JointList[i].spring += fNum;
                    MagniteSpell.JointList[i].damper += fNum;
                    MagniteSpell.JointList[i].damper = Mathf.Clamp(MagniteSpell.JointList[i].damper, 0, MaxMagniteForce);
                    MagniteSpell.JointList[i].spring = Mathf.Clamp(MagniteSpell.JointList[i].spring, 0, MaxMagniteForce);
                }
                for (int i = 0; i < MagniteSpell.RG.Count; i++)
                {
                    MagniteSpell.RG[i].WakeUp();
                    MagniteSpell.RG[i].angularDrag += fNum;
                    MagniteSpell.RG[i].drag += fNum;
                    MagniteSpell.RG[i].angularDrag = Mathf.Clamp(MagniteSpell.RG[i].angularDrag, 0, MaxMagniteForce);
                    MagniteSpell.RG[i].drag = Mathf.Clamp(MagniteSpell.RG[i].drag, 0, MaxMagniteForce);
                }
            }
        }
    }
}
