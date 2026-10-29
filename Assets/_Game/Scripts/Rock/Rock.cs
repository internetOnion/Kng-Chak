using KngChak.Data;
using UnityEngine;
using System ;



namespace KngChak
{   
    
    public class Rock : MonoBehaviour
    {
        private SpriteRenderer _sr ;

        [SerializeField]private RockTypeDefinition RockType; 

        public bool IsPrimed {get; private set ;}



        public event Action Primed ;
        public event Action Broken ;
        


        public void Prime()
        {
            if(IsPrimed)return ;
            IsPrimed = true;
            Primed?.Invoke();
        }
        public void Break()
        {
            Broken?.Invoke();
        }

        void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            ApplyDefinition();
            
        }


        private void ApplyDefinition()
        {
            if(RockType == null) return ;
            _sr.color = RockType.Tint ;
            transform.localScale = Vector3.one * RockType.Size ;
        }
    }
}
