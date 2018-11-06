using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class UpdateCharacterFromStringVariable : MonoBehaviour
    {

        public StringVariable curOutfit;
        public BoolVariable isMale;
        public StringVariable maskId;
        public Character character;
        public bool updateOnEnable;
        public Mask hair;

        public void UpdateCharacter()
        {
            character.outfitId = curOutfit.value;
            character.isFemale = !isMale.value;

            if (maskId)
            {
                ResourcesManager rm = Resources.Load("Resources Manager") as ResourcesManager;
                hair = rm.GetMask(maskId.value);
            }

            character.LoadCharacter();
            character.LoadMask(hair);
        }

        public void OnEnable()
        {
            if (updateOnEnable)
                UpdateCharacter();
        }
    }
}