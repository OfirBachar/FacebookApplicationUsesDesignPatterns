using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace BasicFacebookFeatures
{
    class SpeakItem : ISpeakable
    {
        public PhotoProxy Adoptee { get; set; }

        public void Speak()
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100; 
            synthesizer.Rate = -2;
            synthesizer.Speak(Adoptee.SpeakString());
        }
    }
}
