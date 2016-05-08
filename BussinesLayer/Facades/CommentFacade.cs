using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;

namespace BussinesLayer.Facades
{
    public class CommentFacade : AITBaseFacade
    {
        public CommentRepository CommentRepository { get; set; }

        public CommentListQuery CommentListQuery { get; set; }

        protected IQuery<CommentDTO> CreateQuery(CommentFilter filter)
        {
            var query = CommentListQuery;
            CommentListQuery.Filter = filter;
            return query;
        }

        public void CreateComment(CommentDTO comment)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Comment>(comment);
                CommentRepository.Insert(created);
                uow.Commit();
            }
        }

        public CommentDTO GetCommentById(int commentId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var comment = CommentRepository
                    .GetById(commentId);
                return Mapper.Map<CommentDTO>(comment);
            }
        }

        public void UpdateComment(CommentDTO comment)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = CommentRepository.GetById(comment.Id);
                Mapper.Map(comment, retrieved);
                CommentRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteComment(CommentDTO comment)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = CommentRepository.GetById(comment.Id);
                CommentRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<CommentDTO> GetAllComments()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new CommentFilter())
                    .Execute()
                    .ToList();
            }
        }
    }
}
