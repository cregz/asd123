using Asd123.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asd13.Repository.EF
{
    public interface IImageRepository
    {
        Task<ImageUploadResult> UploadImage(byte[] imageBytes);
        Task<string> EnqueueWorkItem(string imageUrl);

        Task Create(ImageInfo entity);
        Task<ImageInfo> FindByIdentifier(string userIdentifier);
        Task<IReadOnlyCollection<ImageInfo>> FindAll(Expression<Func<ImageInfo, bool>> filterExpression);
    }
}
