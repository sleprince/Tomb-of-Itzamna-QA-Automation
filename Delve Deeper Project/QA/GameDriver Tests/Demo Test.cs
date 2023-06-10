using System;
using System.Diagnostics;
using NUnit.Framework;
using gdio.unity_api;
using gdio.unity_api.v2;
//using gdio.unity_api.utilities;
using gdio.common.objects;

namespace DemoTest
{
    [TestFixture]
    public class UnitTest
    {
        //These parameters can be used to override settings used to test when running from the NUnit command line
        public string testMode = TestContext.Parameters.Get("Mode", "IDE");
        public string pathToExe = TestContext.Parameters.Get("pathToExe", null); // replace null with the path to your executable as needed

        ApiClient api;

        [OneTimeSetUp]
        public void Connect()
        {
            api.Connect("localhost", 19734, false, 30);


            // Enable input hooking
            api.EnableHooks(HookingObject.ALL);

            //Start the Game - in this example we're waiting for an object called "StartButton" to become active, then clicking it.
            api.WaitForObject("//*[@name='StartButton']");
            api.ClickObject(MouseButtons.LEFT, "//*[@name='StartButton']", 30);
            api.Wait(3000);
        }

        [Test]
        public void Test1()
        {
            //first example test after start has been clicked, take a screenshot
            api.CaptureScreenshot("start.jpg");
        }

        [Test]
        public void Test2()
        {
            // Do something else. Tests should be able to run independently after the steps in [OneTimeSetup] and should use try/catch blocks to avoid exiting prematurely on failure
        }

        [OneTimeTearDown]
        public void Disconnect()
        {
            // Disconnect the GameDriver client from the agent
            api.DisableHooks(HookingObject.ALL);
            api.Wait(2000);
            api.Disconnect();
            api.Wait(2000);
        }
    }
}