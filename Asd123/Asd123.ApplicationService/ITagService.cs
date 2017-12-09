using Asd123.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Asd123.ApplicationService
{
    public interface ITagService
    {
        Task<Tag> GetByText(string text);
        Task<Tag> GetById(Guid id);
        Task<IEnumerable<Tag>> GetByPicture(ImageInfo picture);
        Task AddToPicture(IEnumerable<string> texts, IEnumerable<ImageInfo> pictures);
        Task RemoveFromPicture(IEnumerable<string> texts, IEnumerable<ImageInfo> pictures);
        Task<Tag> CreateTag(string text);
        Task DeleteTag(Tag tag);
    }
}
