using System.Linq;
using AutoMapper.QueryableExtensions;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BussinesLayer.Queries
{
    public class CommentListQuery : AITQuery<CommentDTO>
    {
        public CommentFilter Filter { get; set; }

        public CommentListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<CommentDTO> GetQueryable()
        {
            IQueryable<Comment> query = Context.Comments;
            if(Filter.CommentId > 0)
            {
                query = query
                    .Where(c => c.Id == Filter.CommentId);
            }
            return query.ProjectTo<CommentDTO>();
        }
    }
}
