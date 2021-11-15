using NUnit.Framework;

namespace Tests.Core.Map
{
    public class MapParserTests
    {
        [Test]
        public void Parse_01 ()
        {
            string map = @"
            000
            111
            000";

            Tile[,] result = MapParser.Parse(map);

            Tile[,] expected = new[,] // Tile constructor is flipped [y,x] instead of [x,y]
            {
                { Tile.Path, Tile.Wall, Tile.Path },
                { Tile.Path, Tile.Wall, Tile.Path },
                { Tile.Path, Tile.Wall, Tile.Path }
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parse_02 ()
        {
            string map = "010";

            Tile[,] result = MapParser.Parse(map);

            Tile[,] expected = new[,] // Tile constructor is flipped [y,x] instead of [x,y]
            {
                { Tile.Path },
                { Tile.Wall },
                { Tile.Path }
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parse_03 ()
        {
            string map = "0\n1\n0";

            Tile[,] result = MapParser.Parse(map);

            Tile[,] expected = new[,] // Tile constructor is flipped [y,x] instead of [x,y]
            {
                { Tile.Path, Tile.Wall, Tile.Path },
            };

            Assert.AreEqual(expected, result);
        }
    }
}
