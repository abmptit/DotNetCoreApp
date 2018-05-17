﻿namespace TestRunner.ModuleAlpha
{
    using NUnit.Framework;

    [Parallelizable(ParallelScope.All)]
    public class Alpha_FirstFixture
    {
        [Test]
        public void Alpha_FirstFixture_FirstTest()
        {
            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void Alpha_FirstFixture_SecondTest()
        {
            System.Threading.Thread.Sleep(8000);
        }
    }
}
