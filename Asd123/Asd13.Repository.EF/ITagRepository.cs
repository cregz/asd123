using Asd123.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asd13.Repository.EF
{
    public interface ITagRepository
    {
        Task Create(Tag entity);
        Task<Tag> FindByIdentifier(Guid id);
        Task Delete(Guid id);
        Task<IReadOnlyCollection<Tag>> FindAll(Expression<Func<Tag, bool>> filterExpression);
        Task Update(Tag tag);
        Task<IEnumerable<Tag>> FindByPicture(Guid picId);
    }
}
