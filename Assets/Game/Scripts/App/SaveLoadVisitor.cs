using Modules.Entities;

namespace Game.Game.Scripts.App
{
    public class SaveLoadVisitor : IComponentSaveLoadVisitor

    {
        public SaveLoadVisitor(EntityWorld world, EntityCatalog catalog)
        {
            World = world;
            Catalog = catalog;
        }

        public EntityWorld World { get; }
        public EntityCatalog Catalog { get; }
    }

    public interface IComponentSaveLoadVisitor
    {
        public EntityWorld World { get; }
        public EntityCatalog Catalog { get; }
    }
}