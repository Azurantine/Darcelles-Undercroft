using UnityEngine;

namespace DarcellesUndercroft
{
    public class Abilities : Character
    {
        protected Character character;

        protected override void Initialisation()
        {
            base.Initialisation();
            character = GetComponent<Character>();
        }
    }
}