using System;
using System.Threading.Tasks;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using Console = UnityEngine.Debug;
using Boids;

namespace Boids.Editor.Tests
{

    public static class Utils
    {
        public static T RunAsyncMethodSync<T>(Func<Task<T>> asyncFunc)
            =>  Task.Run(async () => await asyncFunc()).GetAwaiter().GetResult();

        public static void RunAsyncMethodSync(Func<Task> asyncFunc) 
            =>  Task.Run(async () => await asyncFunc()).GetAwaiter().GetResult();
    }

    [TestFixture]
    public class FieldTest
    {
        [OneTimeSetUp]
        public void Init()
        {

        }

        [OneTimeTearDown]
        public void Cleanup()
        {

        }

        [Test]
        public void Test()
        {

        }

        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator TestBoidsWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}