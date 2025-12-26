using Oyster.Commands;
using Oyster.Core.Interfaces.Commands;

namespace UnitTests.Tests.Commands
{
    [TestClass]
    public sealed class act_append
    {
        // Const
        private const string TEST_STRING = "\"test\"";

        [TestMethod]
        public void NoParameters()
        {
            // Attempt to make command with no parameters
            ISpeechCommand? command = Act_Append.MakeSelf(Array.Empty<string>());

            // Fail if not null
            Assert.IsNull(command);
        }
        [TestMethod]
        public void MinimalParameters()
        {
            // Attempt to make command with only required parameters
            ISpeechCommand? command = Act_Append.MakeSelf([TEST_STRING]);

            // Fail if null
            Assert.IsNotNull(command);
        }
        [TestMethod]
        public void WaitForUserInput_Default()
        {
            // Make default
            Act_Append command = (Act_Append?)Act_Append.MakeSelf([TEST_STRING])!;

            // Fail if WaitForUserInput does not match its default value
            Assert.AreEqual(Act_Append.DEFAULT_WAITFORUSERINPUT, command.WaitForUserInput);
        }
        [TestMethod]
        public void WaitForUserInput_Set()
        {
            // Make version with WaitForUserInput directly set
            Act_Append command = (Act_Append?)Act_Append.MakeSelf([TEST_STRING, $"{Act_Append.PARAMETER_WAITFORUSERINPUT_NAME}={!Act_Append.DEFAULT_WAITFORUSERINPUT}"])!;

            // Fail if WaitForUserInput does not match the inverse of its default value
            Assert.AreEqual(!Act_Append.DEFAULT_WAITFORUSERINPUT, command.WaitForUserInput);
        }
        [TestMethod]
        public void Instant_Default()
        {
            // Make default
            Act_Append command = (Act_Append?)Act_Append.MakeSelf([TEST_STRING])!;

            // Fail if Instant does not match its default value
            Assert.AreEqual(Act_Append.DEFAULT_INSTANT, command.Instant);
        }
        [TestMethod]
        public void Instant_Set()
        {
            // Make version with Instant directly set
            Act_Append command = (Act_Append?)Act_Append.MakeSelf([TEST_STRING, $"{Act_Append.PARAMETER_INSTANT_NAME}={!Act_Append.DEFAULT_INSTANT}"])!;

            // Fail if Instant does not match the inverse of its default value
            Assert.AreEqual(!Act_Append.DEFAULT_INSTANT, command.Instant);
        }
        [TestMethod]
        public void Mute_Default()
        {
            // Make default
            Act_Append command = (Act_Append?)Act_Append.MakeSelf([TEST_STRING])!;

            // Fail if Mute does not match its default value
            Assert.AreEqual(Act_Append.DEFUALT_MUTE, command.Mute);
        }
        [TestMethod]
        public void Mute_Set()
        {
            // Make version with Mute directly set
            Act_Append command = (Act_Append?)Act_Append.MakeSelf([TEST_STRING, $"{Act_Append.PARAMETER_MUTE_NAME}={!Act_Append.DEFUALT_MUTE}"])!;

            // Fail if Mute does not match the inverse of its default value
            Assert.AreEqual(!Act_Append.DEFUALT_MUTE, command.Mute);
        }
    }
}
