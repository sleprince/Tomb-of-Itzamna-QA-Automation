using System;
using System.Diagnostics;
using NUnit.Framework;
using gdio.unity_api;
using gdio.unity_api.v2;
using gdio.common.objects;

namespace DemoTest
{
    [TestFixture]
    public class UnitTest
    {

        ApiClient api = new ApiClient();

        [OneTimeSetUp]
        public void Connect()
        {


            //ApiClient api 
            api.Connect("localhost", 19734, false, 30);


            // Enable input hooking
            api.EnableHooks(HookingObject.ALL);

            //Start the Game - in this example we're waiting for an object called "QuitButton" to become active
            //api.WaitForObject("//*[@name='QuitButton']");
            //api.Wait(3000);
            //api.ClickObject(MouseButtons.LEFT, "//*[@name='QuitButton']", 30);

            Test1();

        }

        [Test]
        public void Test1()
        {
            //first example test after start has been clicked, take a screenshot
            //api.CaptureScreenshot("start.jpg");
            api.WaitForObject("//*[@name='ControlsButton']");
            api.Wait(3000);
            api.ClickObject(MouseButtons.LEFT, "//*[@name='ControlsButton']", 30);

            Test2();

        }

        [Test]
        public void Test2()
        {
            // Do something else. Tests should be able to run independently after the steps in [OneTimeSetup] and should use try/catch blocks to avoid exiting prematurely on failure
            api.WaitForObject("//*[@name='StartButton']");
            api.Wait(3000);
            api.ClickObject(MouseButtons.LEFT, "//*[@name='StartButton']", 30);
            api.Wait(3000);

            api.WaitForObject("//*[@name='QuitButton']");
            api.Wait(3000);
            api.ClickObject(MouseButtons.LEFT, "//*[@name='QuitButton']", 30);


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