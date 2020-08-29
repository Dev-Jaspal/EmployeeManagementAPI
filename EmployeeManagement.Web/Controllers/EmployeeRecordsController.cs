using EmployeeManagement.Model.Dto;
using EmployeeManagement.Services.IService;
using EmployeeManagement.Utility.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [EnableCors("AllowOrigin")]
    public class EmployeeRecordsController : ControllerBase
    {
        #region Private Member Variables

        /// <summary>
        /// Employee records service interface variable.
        /// </summary>
        private readonly IEmployeeRecordsService _employeeRecordsService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRecordsController"/> class.
        /// </summary>
        /// <param name="chartService">Chart dependency.</param>
        public EmployeeRecordsController(IEmployeeRecordsService employeeRecordsService)
        {
            _employeeRecordsService = employeeRecordsService ?? throw new System.ArgumentNullException(nameof(employeeRecordsService));
        }

        #endregion

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<DataTableEmployeeRecords> GetAllEmployeeDetails()
        {
            ActionResult response;
            ResponseMessage responseMessage;
            List<DataTableEmployeeRecords> employeeRecords = _employeeRecordsService.GetAllRecords();
            if (employeeRecords == null || !employeeRecords.Any())
            {
                response = NotFound(new ResponseMessage(false, null, new Message(HttpStatusCode.NotFound, $"There is no record in database.")));
            }
            else
            {
                response = Ok(new ResponseMessage(true, employeeRecords, new Message(HttpStatusCode.OK)));
            }

            return response;
        }


        /// <summary>
        /// Gets the list of employee record.
        /// </summary>
        /// <param name="employeeId">The employee record unique identifier.</param>
        /// <returns>list of employee records.</returns>
        [Route("{employeeId}")]
        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
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
                List<EmployeeRecords> employeeRecords = _employeeRecordsService.GetRecords(employeeId);
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
        /// <param name="employeeRecords">It contain edited values.</param>
        /// <returns>Return boolean value.</returns>
        [Route("employeeRecords")]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<EmployeeRecords> AddEmployeeDetails([FromBody] EmployeeRecords employeeRecords)
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
                bool isSuccess = _employeeRecordsService.AddRecord(employeeRecords);
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
        /// <param name="employeeRecords">It contain edited values.</param>
        /// <returns>Return boolean value.</returns>
        [Route("{id}")]
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<EmployeeRecords> UpdateEmployeeDetails([FromRoute] string id, [FromBody] EmployeeRecords employeeRecords)
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
                bool isSuccess = _employeeRecordsService.UpdateRecord(id, employeeRecords);
                if (!isSuccess)
                {
                    response = NotFound(new ResponseMessage(false, null, new Message(HttpStatusCode.NotFound, "No record is updated in database.")));
                }
                else
                {
                    response = Ok(new ResponseMessage(true, isSuccess, new Message(HttpStatusCode.OK, "Updated! Your record has been updated. success")));
                }
            }

            return response;
        }
        /// <summary>
        /// Delete the employee record by employee id.
        /// </summary>
        /// <param name="employeeId">The reportmployee record unique identifier.</param>
        /// <returns>Return boolean value.</returns>
        [Route("{employeeId}")]
        [HttpDelete, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 401)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public ActionResult<EmployeeRecords> DeleteEmployeeDetails(int employeeId)
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
                // method for deletion is required.
                // under development.
                bool isSuccess = _employeeRecordsService.DeleteRecord(employeeId);
                if (!isSuccess)
                {
                    response = NotFound(new ResponseMessage(false, null, new Message(HttpStatusCode.NotFound, "No record is deleted from database")));
                }
                else
                {
                    response = Ok(new ResponseMessage(true, isSuccess, new Message(HttpStatusCode.OK, "Deleted! Your record has been deleted. success")));
                }
            }

            return response;
        }

    }
}
