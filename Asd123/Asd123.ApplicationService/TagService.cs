using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Asd123.Domain;
using Asd13.Repository.EF;
using System.Linq;

namespace Asd123.ApplicationService
{
    public class TagService : ITagService
    {
        private ITagRepository _tagRepo;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }

        public async Task AddToPicture(IEnumerable<string> texts, IEnumerable<ImageInfo> pictures)
        {
            foreach (var tagText in texts)
            {
                var tag = await GetByText(tagText);
                if(tag == null)
                {
                    tag = await CreateTag(tagText);
                }
                foreach (var pic in pictures)
                {
                    tag.PictureTags.Add(new PictureTag
                    {
                        Tag = tag,
                        Image = pic
                    });
                }

                await _tagRepo.Update(tag);
            }
        }

        public async Task<Tag> CreateTag(string text)
        {
            Tag tag = new Tag();
            tag.Text = text;
            await _tagRepo.Create(tag);
            return tag;
        }

        public async Task DeleteTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> GetById(Guid id)
        {
            var tag = await _tagRepo.FindByIdentifier(id);
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetByPicture(ImageInfo picture)
        {
            var tags = await _tagRepo.FindByPicture(picture.Id);
            return tags;
        }

        public async Task<Tag> GetByText(string text)
        {
            var tags = await _tagRepo.FindAll(x => x.Text == text);
            return tags.FirstOrDefault();
        }

        public async Task RemoveFromPicture(IEnumerable<string> texts, IEnumerable<ImageInfo> pictures)
        {
            throw new NotImplementedException();
        }
    }
}
