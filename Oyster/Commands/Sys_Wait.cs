using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Sys_Wait : A_Command, ITakesTime
    {
        // Const
        private const bool DEFAULT_CANSKIP = false;
        private const string PARAMETER_CANSKIP_NAME = "canskip";
        private const int MILLISECONDS_IN_A_SECOND = 1000;

        // Private Variables
        private bool _canSkip;
        private float _waitTime;
        private float _timer;

        // Constructor
        public Sys_Wait(
            int waitTimeInMilliseconds,
            bool canSkip
            )
        {
            // Pass in values
            _waitTime = (float)waitTimeInMilliseconds / MILLISECONDS_IN_A_SECOND;
            _canSkip = canSkip;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Read in first parameter as an integer
            int waitTime = 0;
            bool success = LoadParameterValue(rawParameters[0], ref waitTime);

            // On fail return null
            if (!success) return null;

            // Set up dict for other parameters
            Dictionary<string, (object value, Type type)> optionals = new Dictionary<string, (object value, Type type)>
            {
                { PARAMETER_CANSKIP_NAME, (DEFAULT_CANSKIP, typeof(bool)) }
            };

            // Now read in optionals
            string[] optionalParameters = new string[rawParameters.Length - 1];
            Array.Copy(rawParameters, 1, optionalParameters, 0, optionalParameters.Length);
            LoadOptionalParameterValues(optionalParameters, ref optionals);

            // And now make and return
            return new Sys_Wait(waitTime, (bool)optionals[PARAMETER_CANSKIP_NAME].value);
        }
        public override bool Run()
        {
            // Are we at goal?
            if (_timer >= _waitTime)
            {
                // Return true
                return true;
            }

            // Increment timer
            _timer += Definitions.SECONDS_PER_TICK;

            // Return false for now
            return false;
        }
        public void MakeItGoFaster()
        {
            if (_canSkip) { _timer = _waitTime; }
        }
    }
}
