using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ProjectOxford.Vision;
using System.Configuration;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AsdWebJob123
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        public static async Task ProcessQueueMessage([QueueTrigger("imageprocess")] string message, TextWriter log, [Queue("imageprocessresult")] IAsyncCollector<string> output)
        {
            var definition = new { imageinfoid = "", imageurl="" };
            var imageInfo = JsonConvert.DeserializeAnonymousType(message, definition);
            
            log.WriteLine(message);
            VisionServiceClient VisionServiceClient = new VisionServiceClient("cf14e7fc78a447bb85835cc1d11eec27", "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0");
            AnalysisResult analysisResult = VisionServiceClient.GetTagsAsync(imageInfo.imageurl).Result;
            var tags = analysisResult.Tags.Select(x => x.Name);

            var retVal = new
            {
                image = imageInfo.imageinfoid,
                tags = tags
            };

            //var queue = client.GetQueueReference("imageprocessresult");
            //await queue.CreateIfNotExistsAsync();
            var retMessage = JsonConvert.SerializeObject(retVal);
            await output.AddAsync(retMessage);
            //await queue.AddMessageAsync(retMessage);

        }
    }
}
