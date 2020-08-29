using EmployeeManagement.Model.Dto;
using EmployeeManagement.Services.IService;
using EmployeeManagement.Utility.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [EnableCors("AllowOrigin")]
    public class EmployeeTimeEntryController : ControllerBase
    {
        #region Private Member Variables

        /// <summary>
        /// Employee records service interface variable.
        /// </summary>
        private readonly IEmployeeEntryRecordsService _employeeEntryRecordsService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeTimeEntryController"/> class.
        /// </summary>
        /// <param name="employeeEntryRecordsService">Chart dependency.</param>
        public EmployeeTimeEntryController(IEmployeeEntryRecordsService employeeEntryRecordsService)
        {
            _employeeEntryRecordsService = employeeEntryRecordsService ?? throw new ArgumentNullException(nameof(employeeEntryRecordsService));
        }

        #endregion
        /// <summary>
        /// Gets the list of employee record.
        /// </summary>
        /// <param name="employeeId">The employee record unique identifier.</param>
        /// <returns>list of employee records.</returns>
        [Route("{employeeId}")]
        //[EnableCors("AllowOrigin")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<EmployeeRecords> GetEmployeeDetails(int employeeId)
        {
            ActionResult response;
            ResponseMessage responseMessage;
            if (employeeId <= 0)
            {
                responseMessage = new ResponseMessage(false, null, new Message(HttpStatusCode.BadRequest, "Please enter valid employee id."));
                response = BadRequest(responseMessage);
            }
            else
            {
                List<EmployeeEntryRecords> employeeRecords = _employeeEntryRecordsService.GetRecords(employeeId);
                if (employeeRecords == null || !employeeRecords.Any())
                {
                    response = NotFound(new ResponseMessage(false, null, new Message(HttpStatusCode.NotFound, $"There is no record in database against employee id {employeeId}")));
                }
                else
                {
                    response = Ok(new ResponseMessage(true, employeeRecords, new Message(HttpStatusCode.OK)));
                }
            }

            return response;
        }


        /// <summary>
        /// Updates chart options related to current live chart associated to user and saves to database.
        /// </summary>
        /// <param name="employeeEntryRecords">It contain edited values.</param>
        /// <returns>Return boolean value.</returns>
        [Route("employeeEntryRecords")]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<EmployeeEntryRecords> AddEmployeeDetails([FromBody] EmployeeEntryRecords employeeEntryRecords)
        {
            ActionResult response;
            ResponseMessage responseMessage;
            if (!ModelState.IsValid)
            {
                responseMessage = new ResponseMessage(false, null, new Message(HttpStatusCode.BadRequest, "There is invalid entry in employee record."));
                response = BadRequest(responseMessage);
            }
            else
            {
                bool isSuccess = _employeeEntryRecordsService.AddRecord(employeeEntryRecords);
                if (!isSuccess)
                {
                    response = NotFound(new ResponseMessage(false, null, new Message(HttpStatusCode.NotFound, "No record is add in database.")));
                }
                else
                {
                    response = Ok(new ResponseMessage(true, isSuccess, new Message(HttpStatusCode.OK)));
                }
            }

            return response;
        }

        /// <summary>
        /// Updates chart options related to current live chart associated to user and saves to database.
        /// </summary>
        /// <param name="employeeEntryRecords">It contain edited values.</param>
        /// <returns>Return boolean value.</returns>
        [Route("employeeEntryRecords/{id}")]
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<EmployeeEntryRecords> UpdateEmployeeDetails([FromRoute] string id, [FromBody] EmployeeEntryRecords employeeEntryRecords)
        {
            ActionResult response;
            ResponseMessage responseMessage;
            if (!ModelState.IsValid)
            {
                responseMessage = new ResponseMessage(false, null, new Message(HttpStatusCode.BadRequest, "There is invalid entry in employee record."));
                response = BadRequest(responseMessage);
            }
            else
            {
                bool isSuccess = _employeeEntryRecordsService.Update(id, employeeEntryRecords);
                if (!isSuccess)
                {
                    response = NotFound(new ResponseMessage(false, null, new Message(HttpStatusCode.NotFound, "No record is updated in database.")));
                }
                else
                {
                    response = Ok(new ResponseMessage(true, isSuccess, new Message(HttpStatusCode.OK)));
                }
            }

            return response;
        }


    }
}