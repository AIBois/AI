using UnityEngine;

namespace States.Character
{
    public class CombatCharacterState : CharacterState
    {
        private SquadBase enemySquad;

        public CombatCharacterState(SquadBase enemySquad)
        {
            this.enemySquad = enemySquad;
        }

        protected override void Act()
        {
            var closestEnemy = GetClosestEnemy();
            if (!InRange(closestEnemy.transform.position)) context.MoveTo(closestEnemy.transform.position);
            else Attack(closestEnemy);
        }

        CharacterBase GetClosestEnemy()
        {
            CharacterBase result = null;
            var minDistance = Mathf.Infinity;
            var currentPos = transform.position;
            foreach (var enemy in enemySquad.Units)
            {
                var distance = Vector3.Distance(enemy.transform.position, currentPos);
                if (distance > minDistance) continue;
                result = enemy;
                minDistance = distance;
            }
            return result;
        }
        
        private bool InRange(Vector3 position)
        {
            return Vector3.Distance(position, transform.position) <= context.AttackDistance;
        }

        private void Attack(CharacterBase closestEnemy)
        {
            if(context.ReadyToAttack()) closestEnemy.TakeDamage(context.Attack);
        }
    }
}