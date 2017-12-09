using Asd123.Domain;
using Asd13.Repository.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asd123.ApplicationService
{
    public class ImageService : IImageService
    {
        private IImageRepository imageRepo;

        public ImageService(IImageRepository imageRepo)
        {
            this.imageRepo = imageRepo;
        }

        public async Task<IReadOnlyCollection<ImageInfo>> FetchImagesOfUser(User user)
        {
            return await imageRepo.FindAll(x => x.UploadedBy.Id == user.Id);
        }

        public async Task<Uri> UploadImage(byte[] imageBytes, User uploader, string name)
        {
            ImageUploadResult result = await imageRepo.UploadImage(imageBytes);
            ImageInfo info = new ImageInfo()
            {
                ImageId = result.ImageId.ToString(),
                UploadedBy = uploader,
                ImageUri = result.ImageUri.ToString(),
                Name = name
            };
            await imageRepo.Create(info);
            var t = new { imageinfoid = info.Id.ToString(), imageurl = info.ImageUri };
            await imageRepo.EnqueueWorkItem(JsonConvert.SerializeObject(t));
            return result.ImageUri;
        }
    }
}
