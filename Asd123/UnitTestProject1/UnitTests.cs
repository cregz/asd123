using Asd123.ApplicationService;
using Asd123.Domain;
using Asd123.Repository.EF;
using Asd13.Repository.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    public class UnitTests
    {
        Mock<ITagRepository> mockTagRepo;
        TagService ts;
        Tag tag;
        Mock<ApplicationDbContext> mockDB;
        List<Tag> tagok;

        [SetUp]
        public void SetUp()
        {
            mockTagRepo = new Mock<ITagRepository>();
            ts = new TagService(mockTagRepo.Object);
            mockDB = new Mock<ApplicationDbContext>();
            tagok = new List<Tag>();

            tagok.Add(tag);

            tag = new Tag();
            tag.Text = "asd";

            //mockTagRepo.Setup(x => x.Create(tag)).Returns(new Task(CreateAction));
            mockTagRepo.Setup(x => x.FindAll(a => a.Text == tag.Text)).Returns(new Task<IReadOnlyCollection<Tag>>(FindAllAction));
            mockDB.Setup(x => x.Set<Tag>()).Returns(GetDbSetMock(tagok).Object);

        }

        /*public void CreateAction()
        {

        }*/

        public List<Tag> FindAllAction()
        {
            tagok = new List<Tag>();
            tagok.Add(tag);
            return tagok;
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(IEnumerable<T> items = null) where T : class
        {
            if (items == null)
            {
                items = new T[0];
            }

            var dbSetMock = new Mock<DbSet<T>>();
            var q = dbSetMock.As<IQueryable<T>>();

            q.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            return dbSetMock;
        }

        [Test]
        public void Test1()
        {
            var res = ts.GetByText(tag.Text);
            
            NUnit.Framework.Assert.That(res.Result.Text == tag.Text);
        }
    }
}
