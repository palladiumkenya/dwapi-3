using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Dwh;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dwapi.Controller.ExtractDetails
{
    [Produces("application/json")]
    [Route("api/ContactListing")]
    public class ContactListingController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ITempContactListingExtractRepository _tempContactListingExtractRepository;
        private readonly IContactListingExtractRepository _contactListingExtractRepository;
        private readonly ITempContactListingExtractErrorSummaryRepository _errorSummaryRepository;

        public ContactListingController(ITempContactListingExtractRepository tempContactListingExtractRepository, IContactListingExtractRepository ContactListingExtractRepository, ITempContactListingExtractErrorSummaryRepository errorSummaryRepository)
        {
            _tempContactListingExtractRepository = tempContactListingExtractRepository;
            _contactListingExtractRepository = ContactListingExtractRepository;
            _errorSummaryRepository = errorSummaryRepository;
        }

        [HttpGet("ValidCount")]
        public async Task<IActionResult> GetValidCount()
        {
            try
            {
                var count = await _contactListingExtractRepository.GetCount();
                return Ok(count);
            }
            catch (Exception e)
            {
                var msg = $"Error loading valid Patient Extracts";
                Log.Error(msg);
                Log.Error($"{e}");
                return StatusCode(500, msg);
            }
        }

        [HttpGet("LoadValid/{page}/{pageSize}")]
        public async Task<IActionResult> LoadValid(int? page,int pageSize)
        {
            try
            {
                var tempContactListingExtracts = await _contactListingExtractRepository.GetAll(page,pageSize);
                return Ok(tempContactListingExtracts.ToList());
            }
            catch (Exception e)
            {
                var msg = $"Error loading valid ContactListing Extracts";
                Log.Error(msg);
                Log.Error($"{e}");
                return StatusCode(500, msg);
            }
        }

        [HttpGet("LoadErrors")]
        public IActionResult LoadErrors()
        {
            try
            {
                var tempContactListingExtracts = _tempContactListingExtractRepository.GetAll().Where(n => n.CheckError).ToList();
                return Ok(tempContactListingExtracts);
            }
            catch (Exception e)
            {
                var msg = $"Error loading ContactListing Extracts with errors";
                Log.Error(msg);
                Log.Error($"{e}");
                return StatusCode(500, msg);
            }
        }

        [HttpGet("LoadValidations")]
        public IActionResult LoadValidations()
        {
            try
            {
                var errorSummary = _errorSummaryRepository.GetAll().OrderByDescending(x=>x.Type).ToList();
                return Ok(errorSummary);
            }
            catch (Exception e)
            {
                var msg = $"Error loading Patient Status error summary";
                Log.Error(msg);
                Log.Error($"{e}");
                return StatusCode(500, msg);
            }
        }
    }
}
