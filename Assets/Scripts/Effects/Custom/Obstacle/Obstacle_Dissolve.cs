namespace mytest.Effects.Custom.Obstacle
{
    /// <summary>
    /// Преграда, которая уничтожается эффектом Dissolve
    /// </summary>
    public class Obstacle_Dissolve : Obstacle_Base
    {
        public DissolveEffectBehaviour DissolveBehaviour;

        void Start()
        {
            DissolveBehaviour.OnDissolveFinished += Disable;
        }

        public override void DisableObstacle()
        {
            DissolveBehaviour.Dissolve();
        }
    }
}
