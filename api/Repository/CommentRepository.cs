using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public Task<bool> CommentExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> Delete(int id)
        {
            var commentModel=await _context.Comments.FirstOrDefaultAsync(c => c.Id==id);
            if(commentModel==null)
            {
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id==id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if(existingComment==null)
            {
                return null;
            }
            existingComment.Title=commentModel.Title;
            existingComment.Content=commentModel.Content;
            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}