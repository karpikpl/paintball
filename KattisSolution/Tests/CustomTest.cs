using System.IO;
using System.Text;
using NUnit.Framework;

namespace KattisSolution.Tests
{
    [TestFixture]
    [Category("sample")]
    public class CustomTest
    {
        [Test]
        public void SampleTest_WithStringData_Should_Pass1()
        {
            // Arrange
            const string expectedAnswer = "3\n2\n1\n";
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes(@"3 3
1 2
2 3
1 3")))
            using (var output = new MemoryStream())
            {
                // Act
                Program.Solve(input, output);
                var result = Encoding.UTF8.GetString(output.ToArray());

                // Assert
                CollectionAssert.AreEquivalent(expectedAnswer.Split('\n'),result.Split('\n'));
            }
        }

        [Test]
        public void SampleTest_WithStringData_Should_Pass2()
        {
            // Arrange
            const string expectedAnswer = "Impossible\n";
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes(@"3 2
1 2
1 3")))
            using (var output = new MemoryStream())
            {
                // Act
                Program.Solve(input, output);
                var result = Encoding.UTF8.GetString(output.ToArray());

                // Assert
                CollectionAssert.AreEquivalent(expectedAnswer.Split('\n'), result.Split('\n'));
            }
        }
    }
}
