using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using System.Globalization;
using System.IO;
using static Google.Api.Gax.Grpc.Gcp.AffinityConfig.Types;
using static Google.Rpc.Context.AttributeContext.Types;

namespace SignalRProject.Classes.Google
{
    public class TextToSpeach
    {

        public static string CreаteTextToAudiоFile(string text, string filename)
        {

            //auth info
            string keyName = @"ethereal-temple-477211-v5-2521550a8eb0.json";
            string PathName = @"C:\\GoogleCreds\";
            string keyPath = Path.Combine(PathName, keyName);

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyPath);

            //set client and request
            TextToSpeechClient client = TextToSpeechClient.Create();
            SynthesizeSpeechRequest request = new SynthesizeSpeechRequest();
            request.Input = new SynthesisInput();
            request.Input.Text = text;

            request.AudioConfig = new AudioConfig();
            request.AudioConfig.AudioEncoding = AudioEncoding.Mp3;
            request.AudioConfig.SampleRateHertz = 44100;
            request.Voice = new VoiceSelectionParams();
            request.Voice.SsmlGender = SsmlVoiceGender.Male;
            request.Voice.Name = "nl-NL-Chirp3-HD-Achird";
            request.Voice.LanguageCode = "nl-NL";

            //catch response
            SynthesizeSpeechResponse respоnse = client.SynthesizeSpeech(request);




            string Savepath = Path.Combine(Directory.GetCurrentDirectory(), "audio_data/" + filename);


            //save file
            using (var file = System.IO.File.Create(Savepath))
            {
                respоnse.AudioContent.WriteTo(file);
            }

            return Savepath;
        }
    }
}
