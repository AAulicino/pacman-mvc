using NUnit.Framework;
using UnityEngine;

namespace Tests.Core.Collectibles
{
    public class CollectibleModelTests
    {
        class PublicProperties : CollectibleModelTests
        {
            [Test]
            public void Equals_Position ()
            {
                CollectibleModel model = new CollectibleModel(Vector2Int.one, default);
                Assert.AreEqual(Vector2Int.one, model.Position);
            }

            [Test]
            public void Equals_CollectibleType ()
            {
                CollectibleModel model = new CollectibleModel(default, CollectibleType.PowerUp);
                Assert.AreEqual(CollectibleType.PowerUp, model.Type);
            }
        }
    }
}
