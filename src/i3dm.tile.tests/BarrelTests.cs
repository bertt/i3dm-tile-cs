using NUnit.Framework;
using System.Numerics;

namespace i3dm.tile.tests
{
    public class BarrelTests
    {
        [Test]
        public void CreateBarrelTest()
        {
            var rotation_input = new Vector3(0, 0, 0);
            var expectedNormalUp = new Vector3(0, 1, 0);
            var expectedNormalRight = new Vector3(1, 0, 0);

            var actual = GetNormals(rotation_input);

            Assert.IsTrue(expectedNormalUp.Equals(actual.NormalUp));
            Assert.IsTrue(expectedNormalRight.Equals(actual.NormalRight));
        }


        private (Vector3 NormalUp, Vector3 NormalRight) GetNormals (Vector3 rotation)
        {
            var normalUp = new Vector3();
            var normalRight = new Vector3();
            return (normalUp, normalRight);
        }
    }
}
