namespace TestRunner.Runners
{
    using NUnit.Framework;

    [Parallelizable(ParallelScope.All)]
    public class Alpha_SecondFixture
    {
        [Test]
        public void Alpha_SecondFixture_FirstTest()
        {
            System.Threading.Thread.Sleep(5000);
        }

        [Test]
        public void Alpha_SecondFixture_SecondTest()
        {
            System.Threading.Thread.Sleep(6000);
        }
    }
}
