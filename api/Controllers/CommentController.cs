using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Azure.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly ICommentRepository _commentRepo;

        public CommentController(ICommentRepository commentRepo,
        IStockRepository stockRepo)
        {
            _stockRepo=stockRepo;
            _commentRepo=commentRepo;            
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments=await _commentRepo.GetAllAsync();
            var commentDtos=comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment=await _commentRepo.GetByIdAsync(id);
            if(comment==null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }
            var commentModel=commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id=commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment= await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
            if(comment==null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepo.Delete(id);
            if (commentModel==null)
            {
                return NotFound("No sush comment");
            }
            return Ok(commentModel);
        }

    }
}