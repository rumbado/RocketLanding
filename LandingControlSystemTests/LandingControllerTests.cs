using System.Threading.Tasks;
using LandingControlSystem;
using NUnit.Framework;

namespace LandingControlSystemTests
{
    public class LandingControllerTests
    {
        private const string OUT_OF_PLATFORM = "Out of Platform";
        private const string CLASH = "Clash";
        private const string OK_FOR_LANDING = "Ok For Landing";

        private LandingController _landingController;

        [SetUp]
        public void Setup()
        {
            _landingController = new LandingController();
        }

        [Test]
        public void When_Request_Postion_5_5_Then_Response_Is_OkForLanding()
        {
            Assert.AreEqual(_landingController.CheckPosition(5,5), OK_FOR_LANDING);
        }

        [Test]
        public void When_Request_Postion_16_15_Then_Response_Is_OutOfPlatform()
        {
            Assert.AreEqual(_landingController.CheckPosition(16,15), OUT_OF_PLATFORM);
        }

        [Test]
        public void When_Request_Previously_Checked_One_Then_Response_Is_Clash()
        {
            //Given possition checked
            _landingController.CheckPosition(13,6);

            //Then check it again crash
            Assert.AreEqual(_landingController.CheckPosition(13,6), CLASH);
        }

        [Test]
        public void When_Request_Adjacents_To_Checked_One_Then_Response_Is_Clash()
        {
            //Given possition checked
            _landingController.CheckPosition(11,11);

            //Then check adjacents clashes
            Assert.AreEqual(_landingController.CheckPosition(10,11), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(10,10), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(10,12), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(12,11), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(12,10), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(12,12), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(11,10), CLASH);
            Assert.AreEqual(_landingController.CheckPosition(11,12), CLASH);
        }

               [Test]
        public void When_Two_Rockets_Request_Same_Cell_Only_One_Lands()
        {
            string oneCall = null;
            string otherCall = null;

            //Given two parallel requests
            Parallel.Invoke(() => oneCall = _landingController.CheckPosition(13,6), () => otherCall = _landingController.CheckPosition(13,6));

            //Then one clash and the other lands
            Assert.IsTrue( (oneCall == CLASH && otherCall == OK_FOR_LANDING) ||
                            (oneCall == OK_FOR_LANDING && otherCall == CLASH));
        }

        [Test]
        public void Two_Rockets_Can_Land_At_1_Unit_Of_Distance()
        {
            Assert.AreEqual(_landingController.CheckPosition(8,6), OK_FOR_LANDING);
            Assert.AreEqual(_landingController.CheckPosition(10,6), OK_FOR_LANDING);
        }
    }
}