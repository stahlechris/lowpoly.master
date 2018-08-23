using UnityEngine;
//CharacterStats simply is a container for stats a character currently has
namespace LowPoly.Character
{
    public class CharacterStats: MonoBehaviour
    {
        public Stat agility; //increases minimum weapon attack speed
        public Stat strength; //increases minimum weapon damage amount
        public Stat stamina; //increases max hp AND max sprint
        public Stat spirit; //increases mana regen rate
        public Stat intellect; //increases max mana
        public Stat coordination; //decreases miss chance
        public Stat luck; //increases chance for better loot AND critical strike
        public Stat charm; //increases chance for better scenario from speaking with npc
        public Stat armor; //decreases amount of damage received on every hit
    }
}