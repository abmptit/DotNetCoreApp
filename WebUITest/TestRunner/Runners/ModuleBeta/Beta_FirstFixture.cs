namespace TestRunner.ModuleBeta
{
    using NUnit.Framework;

    [Parallelizable(ParallelScope.All)]
    public class Beta_FirstFixture
    {
        [Test]
        public void Beta_FirstFixture_FirstTest()
        {
            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void Beta_FirstFixture_SecondTest()
        {
            System.Threading.Thread.Sleep(8000);
        }
    }
}
