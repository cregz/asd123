﻿using Asd123.Domain;
using Asd123.Repository.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asd13.Repository.EF
{
    public class ImageRepository : GenericCrudRepository<ImageInfo>, IImageRepository
    {
        private CloudStorageAccount storageAccount;

        public ImageRepository(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            storageAccount = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
            Context = dbContext;
        }

        public async Task<ImageUploadResult> UploadImage(byte[] imageBytes)
        {
            // TODO: error handling
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();
            BlobContainerPermissions permissions = await container.GetPermissionsAsync();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);
            var fileId = Guid.NewGuid();
            var blob = container.GetBlockBlobReference(fileId.ToString());
            await blob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

            return new ImageUploadResult
            {
                ImageId = fileId,
                ImageUri = blob.Uri
            };
        }

        public async Task<string> EnqueueWorkItem(string imageUrl)
        {
            // TODO: error handling + retry policy
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("imageprocess");
            await queue.CreateIfNotExistsAsync();
            var message = new CloudQueueMessage(imageUrl);
            await queue.AddMessageAsync(message);

            
            //var resultqueue = queueClient.GetQueueReference("imageprocessresult");
            //CloudQueueMessage result = null;
            //int i = 0;
            //do
            //{
            //    await Task.Delay(10);
            //    result = await resultqueue.GetMessageAsync();
            //    i++;
            //} while (result == null && i< 10);
            //if (result != null)
            //{
            //    string returnVal = result.AsString;
            //    await resultqueue.DeleteMessageAsync(result);
            //    return returnVal;
            //}
            return null;
        }

        public async Task<ImageInfo> FindByIdentifier(string imageIdentifier)
        {
            var imageInfos = await FindAll(u => u.ImageId == imageIdentifier);


            return imageInfos.FirstOrDefault();
        }
    }
}
