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
        private ITagRepository tagRepo;
        private ITagService tagService;

        public ImageService(IImageRepository imageRepo, ITagRepository tagRepo, ITagService tagService)
        {
            this.imageRepo = imageRepo;
            this.tagRepo = tagRepo;
            this.tagService = tagService;
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
            var res = await imageRepo.EnqueueWorkItem(JsonConvert.SerializeObject(t));
            //if (res != null)
            //{
            //    var definition = new { image = "", tags = new List<string>() };
            //    var objs = JsonConvert.DeserializeAnonymousType(res, definition);
            //    var imageInfo = (await imageRepo.FindAll(x => x.Id.ToString() == objs.image)).FirstOrDefault();
            //
            //    await tagService.AddToPicture(objs.tags, new List<ImageInfo>() { imageInfo });
            //}

            return result.ImageUri;
        }
    }
}
