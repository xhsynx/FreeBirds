using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FreeBirds.Services;
using FreeBirds.DTOs;

namespace FreeBirds.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDTO>>> GetAllExams()
        {
            var exams = await _examService.GetAllExamsAsync();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDTO>> GetExamById(Guid id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
                return NotFound();

            return Ok(exam);
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<ExamDTO>>> GetExamsByClassId(Guid classId)
        {
            var exams = await _examService.GetExamsByClassIdAsync(classId);
            return Ok(exams);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<ExamDTO>>> GetExamsByTeacherId(Guid teacherId)
        {
            var exams = await _examService.GetExamsByTeacherIdAsync(teacherId);
            return Ok(exams);
        }

        [HttpPost]
        public async Task<ActionResult<ExamDTO>> CreateExam(CreateExamDTO examDto)
        {
            var exam = await _examService.CreateExamAsync(examDto);
            if (exam == null)
                return BadRequest("Failed to create exam");

            return CreatedAtAction(nameof(GetExamById), new { id = exam.Id }, exam);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExamDTO>> UpdateExam(Guid id, CreateExamDTO examDto)
        {
            var exam = await _examService.UpdateExamAsync(id, examDto);
            if (exam == null)
                return NotFound();

            return Ok(exam);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            var result = await _examService.DeleteExamAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleExamStatus(Guid id)
        {
            var result = await _examService.ToggleExamStatusAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 