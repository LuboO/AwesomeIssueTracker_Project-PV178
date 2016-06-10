using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;
using BussinesLayer.Filters;
using System;
using System.Data.Entity.Core;
using Microsoft.AspNet.Identity;
using DataAccessLayer.Enums;

namespace BussinesLayer.Facades
{
    public class CommentFacade : AITBaseFacade
    {
        public CommentRepository CommentRepository { get; set; }

        public IssueRepository IssueRepository { get; set; }

        public Func<AITUserManager> UserManagerFactory { get; set; }

        public CommentListQuery CommentListQuery { get; set; }

        protected IQuery<CommentDTO> CreateQuery(CommentFilter filter)
        {
            var query = CommentListQuery;
            CommentListQuery.Filter = filter;
            return query;
        }

        public int CreateComment(CommentDTO comment, int issueId, int authorId)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using(var userManager = UserManagerFactory.Invoke())
                {
                    var issue = IssueRepository.GetById(issueId);
                    if (issue == null)
                        throw new ObjectNotFoundException("Issue wasn't found");

                    var user = userManager.FindById(authorId);
                    if (user == null)
                        throw new ObjectNotFoundException("Author wasn't found");

                    var created = Mapper.Map<Comment>(comment);

                    created.IssueId = issue.Id;
                    created.Issue = null;
                    created.AuthorId = user.Id;
                    created.Author = null;

                    issue.ChangeTime = DateTime.Now;
                    issue.ChangeType = IssueChangeType.Comment;
                    issue.NameOfChanger = user.UserName;

                    CommentRepository.Insert(created);
                    IssueRepository.Update(issue);
                    uow.Commit();
                    return created.Id;
                }
            }
        }

        public CommentDTO GetCommentById(int commentId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var comment = CommentRepository.GetById(commentId);
                if (comment == null)
                    return null;

                return Mapper.Map<CommentDTO>(comment);
            }
        }

        public void UpdateComment(CommentDTO comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = CommentRepository.GetById(comment.Id);
                if (retrieved == null)
                    throw new ObjectNotFoundException("Comment wasn't found");

                Mapper.Map(comment, retrieved);
                CommentRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteComment(CommentDTO comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = CommentRepository.GetById(comment.Id);
                if (deleted == null)
                    throw new ObjectNotFoundException("Comment wasn't found");

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

        public List<CommentDTO> GetCommentsByAuthor(int authorId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new CommentFilter() { AuthorId = authorId })
                    .Execute()
                    .ToList();
            }
        }

        public List<CommentDTO> GetCommentsByIssue(int issueId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new CommentFilter() { IssueId = issueId })
                    .Execute()
                    .ToList();
            }
        }
    }
}
