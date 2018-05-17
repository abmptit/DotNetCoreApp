namespace TestRunner.Runners
{
    using NUnit.Framework;

    [Parallelizable(ParallelScope.All)]
    public class Beta_SecondFixture
    {
        [Test]
        public void Beta_SecondFixture_FirstTest()
        {
            System.Threading.Thread.Sleep(5000);
        }

        [Test]
        public void Beta_SecondFixture_SecondTest()
        {
            System.Threading.Thread.Sleep(6000);
        }
    }
}
