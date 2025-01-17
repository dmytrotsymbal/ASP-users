﻿using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CriminalRecordController : ControllerBase
    {
        private readonly ICriminalRecordRepository _criminalRecordRepository;

        public CriminalRecordController(ICriminalRecordRepository criminalRecordRepository)
        {
            _criminalRecordRepository = criminalRecordRepository;
        }


        [HttpGet("get-all-users-crimes/{userId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetAllUsersCriminalRecords(Guid userId)
        {
            try
            {
                var criminalRecords = await _criminalRecordRepository.GetAllUsersCriminalRecords(userId);
                if (criminalRecords == null)
                {
                    return NotFound("No criminal records found for the user.");
                }
                return Ok(criminalRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("get-crime-by-id/{criminalRecordId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetCriminalRecordById(int criminalRecordId)
        {
            try
            {
                var criminalRecord = await _criminalRecordRepository.GetCriminalRecordById(criminalRecordId);
                if (criminalRecord == null)
                {
                    return NotFound("Criminal record not found.");
                }
                return Ok(criminalRecord);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut("update/{criminalRecordId}")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateCriminalRecord(int criminalRecordId, [FromBody] CrimeDTO criminalRecord)
        {
            try
            {
                if (criminalRecord.CriminalRecordID != criminalRecordId)
                {
                    return BadRequest("User ID mismatch");
                }
                await _criminalRecordRepository.UpdateCriminalRecord(criminalRecordId, criminalRecord);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("add-crime-to/{userId}")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddCrimeToUser(Guid userId, [FromBody] CriminalRecord criminalRecord)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                criminalRecord.UserID = userId;
                await _criminalRecordRepository.AddCrimeToUser(userId, criminalRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpDelete("delete/{criminalRecordId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCriminalRecord(int criminalRecordId)
        {
            try
            {
                await _criminalRecordRepository.DeleteCriminalRecord(criminalRecordId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // HALPERS==========================================================

        [HttpGet("get-all-prisons")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetAllPrisons()
        {
            try
            {
                var prisons = await _criminalRecordRepository.GetAllPrisons();
                if (prisons == null)
                {
                    return NotFound("No prisons found.");
                }
                return Ok(prisons);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

