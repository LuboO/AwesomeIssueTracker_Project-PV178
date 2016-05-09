using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using AutoMapper;
using System.Collections.Generic;

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

            if(Filter.CommentId != null)
                query = query
                    .Where(c => c.Id == Filter.CommentId);
            if (Filter.AuthorId != null)
                query = query
                    .Where(c => c.AuthorId == Filter.AuthorId);
            if (Filter.IssueId != null)
                query = query
                    .Where(c => c.IssueId == Filter.IssueId);

            return (Mapper.Map<List<CommentDTO>>(query)).AsQueryable();
        }
    }
}
