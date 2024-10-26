namespace ShootEmUp
{
    public class BulletSpawner : SpawnerAbstract<Bullet>
    {
        public override void Awake()
        {
            base.Awake();
            Prewarm(7);
        }
    }
}