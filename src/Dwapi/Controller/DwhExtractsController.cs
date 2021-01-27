using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dwapi.ExtractsManagement.Core.Commands.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Services;
using Dwapi.Hubs.Dwh;
using Dwapi.Models;
using Dwapi.SettingsManagement.Core.Application.Metrics.Events;
using Dwapi.SettingsManagement.Core.Interfaces.Repositories;
using Dwapi.SettingsManagement.Core.Model;
using Dwapi.SharedKernel.DTOs;
using Dwapi.SharedKernel.Utility;
using Dwapi.UploadManagement.Core.Exchange.Dwh;
using Dwapi.UploadManagement.Core.Interfaces.Services.Cbs;
using Dwapi.UploadManagement.Core.Interfaces.Services.Dwh;
using Hangfire;
using Hangfire.States;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Dwapi.Controller
{
    [Produces("application/json")]
    [Route("api/DwhExtracts")]
    public class DwhExtractsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IMediator _mediator;
        private readonly IExtractStatusService _extractStatusService;
        private readonly IHubContext<ExtractActivity> _hubContext;
        private readonly IDwhSendService _dwhSendService;
        private readonly ICbsSendService _cbsSendService;
        private readonly ICTSendService _ctSendService;
        private readonly IExtractRepository _extractRepository;
        private readonly string _version;

        public DwhExtractsController(IMediator mediator, IExtractStatusService extractStatusService, IHubContext<ExtractActivity> hubContext, IDwhSendService dwhSendService,  ICbsSendService cbsSendService, ICTSendService ctSendService, IExtractRepository extractRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _extractStatusService = extractStatusService;
            _dwhSendService = dwhSendService;
            _cbsSendService = cbsSendService;
            _ctSendService = ctSendService;
            _extractRepository = extractRepository;
            Startup.HubContext= _hubContext = hubContext;
            var ver = GetType().Assembly.GetName().Version;
            _version = $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }

        [HttpPost("extract")]
        public async Task<IActionResult> Load([FromBody]ExtractPatient request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _mediator.Send(request, HttpContext.RequestAborted);
            return Ok(result);
        }

        [HttpPost("extractAll")]
        public async Task<IActionResult> Load([FromBody]LoadExtracts request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var ver = GetType().Assembly.GetName().Version;
            string version = $"{ver.Major}.{ver.Minor}.{ver.Build}";
            await _mediator.Publish(new ExtractLoaded("CareTreatment", version));

            if (!request.LoadMpi)
            {
                var result = await _mediator.Send(request.LoadFromEmrCommand, HttpContext.RequestAborted);
                return Ok(result);
            }

            var dwhExtractsTask = Task.Run(() => _mediator.Send(request.LoadFromEmrCommand, HttpContext.RequestAborted));
            var mpiExtractsTask = Task.Run(() => _mediator.Send(request.ExtractMpi, HttpContext.RequestAborted));
            var extractTasks = new List<Task<bool>> { mpiExtractsTask, dwhExtractsTask};
            // wait for both tasks but doesn't throw an error for mpi load
            var result1 = await Task.WhenAll(extractTasks);
            return Ok(dwhExtractsTask);
        }

        // GET: api/DwhExtracts/status/id
        [HttpGet("status/{id}")]
        public IActionResult GetStatus(Guid id)
        {
            if (id.IsNullOrEmpty())
                return BadRequest();
            try
            {
                var eventExtract = _extractStatusService.GetStatus(id);
                if (null == eventExtract)
                    return NotFound();

                return Ok(eventExtract);
            }
            catch (Exception e)
            {
                var msg = $"Error loading {nameof(Extract)}(s)";
                Log.Error(msg);
                Log.Error($"{e}");
                return StatusCode(500, msg);
            }
        }

        // POST: api/DwhExtracts/manifest
        [HttpPost("manifest")]
        public async Task<IActionResult> SendManifest([FromBody] CombinedSendManifestDto packageDto)
        {
            if (!packageDto.IsValid())
                return BadRequest();

            var ver = GetType().Assembly.GetName().Version;
            string version = $"{ver.Major}.{ver.Minor}.{ver.Build}";
            await _mediator.Publish(new ExtractSent("CareTreatment", version));

            try
            {
                if (!packageDto.SendMpi)
                {
                    var result = await _dwhSendService.SendManifestAsync(packageDto.DwhPackage,_version);
                    return Ok(result);
                }

                var mpiTask = await _cbsSendService.SendManifestAsync(packageDto.MpiPackage);
                var dwhTask = await _dwhSendService.SendManifestAsync(packageDto.DwhPackage,_version);
                return Ok();
            }
            catch (Exception e)
            {
                var msg = $"Error sending  Manifest {e.Message}";
                Log.Error(e, msg);
                return StatusCode(500, msg);
            }
        }

        // POST: api/DwhExtracts/diffmanifest
        [HttpPost("diffmanifest")]
        public async Task<IActionResult> SendDiffManifest([FromBody] CombinedSendManifestDto packageDto)
        {
            if (!packageDto.IsValid())
                return BadRequest();

            var ver = GetType().Assembly.GetName().Version;
            string version = $"{ver.Major}.{ver.Minor}.{ver.Build}";
            await _mediator.Publish(new ExtractSent("CareTreatment", version));

            try
            {
                if (!packageDto.SendMpi)
                {
                    var result = await _dwhSendService.SendDiffManifestAsync(packageDto.DwhPackage,_version);
                    return Ok(result);
                }

                var mpiTask = await _cbsSendService.SendManifestAsync(packageDto.MpiPackage);
                var dwhTask = await _dwhSendService.SendManifestAsync(packageDto.DwhPackage,_version);
                return Ok();
            }
            catch (Exception e)
            {
                var msg = $"Error sending  Manifest {e.Message}";
                Log.Error(e, msg);
                return StatusCode(500, msg);
            }
        }


        // POST: api/DwhExtracts/patients
        [HttpPost("patients")]
        public IActionResult SendPatientExtracts([FromBody] CombinedSendManifestDto packageDto)
        {
            if (!packageDto.IsValid())
                return BadRequest();
            try
            {
                if (!packageDto.SendMpi)
                {
                    QueueDwh(packageDto.DwhPackage);
                    return Ok();
                }
                QueueDwh(packageDto.DwhPackage);
                QueueMpi(packageDto.MpiPackage);
                return Ok();

            }
            catch (Exception e)
            {
                var msg = $"Error sending Extracts {e.Message}";
                Log.Error(e, msg);
                return StatusCode(500, msg);
            }
        }

        // POST: api/DwhExtracts/patients
        [HttpPost("diffpatients")]
        public IActionResult SendDiffPatientExtracts([FromBody] CombinedSendManifestDto packageDto)
        {
            if (!packageDto.IsValid())
                return BadRequest();
            try
            {
                if (!packageDto.SendMpi)
                {
                    QueueDwhDiff(packageDto.DwhPackage);
                    return Ok();
                }
                QueueDwh(packageDto.DwhPackage);
                QueueMpi(packageDto.MpiPackage);
                return Ok();

            }
            catch (Exception e)
            {
                var msg = $"Error sending Extracts {e.Message}";
                Log.Error(e, msg);
                return StatusCode(500, msg);
            }
        }

        [AutomaticRetry(Attempts = 0)]
        private void QueueDwh(SendManifestPackageDTO package)
        {


            var extracts = _extractRepository.GetAllRelated(package.ExtractId).ToList();

            if (extracts.Any())
                package.Extracts = extracts.Select(x => new ExtractDto() {Id = x.Id, Name = x.Name}).ToList();

            _ctSendService.NotifyPreSending();

            var job1 =
                BatchJob.StartNew(x => { SendJobBaselines(package); });

            var job2 =
                BatchJob.ContinueBatchWith(job1, x => { SendJobProfiles(package); });

            var jobEnd =
                BatchJob.ContinueBatchWith(job2, x =>
                {
                    _ctSendService.NotifyPostSending(_version);

                });
        }

        [AutomaticRetry(Attempts = 0)]
        private void QueueDwhDiff(SendManifestPackageDTO package)
        {
            var extracts = _extractRepository.GetAllRelated(package.ExtractId).ToList();

            if (extracts.Any())
                package.Extracts = extracts.Select(x => new ExtractDto() {Id = x.Id, Name = x.Name}).ToList();

            _ctSendService.NotifyPreSending();

            var job1 =
                BatchJob.StartNew(x => { SendDiffJobBaselines(package); });

            var job2 =
                BatchJob.ContinueBatchWith(job1, x => { SendDiffJobProfiles(package); });

            var jobEnd =
                BatchJob.ContinueBatchWith(job2, x => { _ctSendService.NotifyPostSending(_version); });
        }

        public void SendJobBaselines(SendManifestPackageDTO package)
        {
            var idsA =_ctSendService.SendBatchExtractsAsync(package, 500, new ArtMessageBag()).Result;
            var idsB=_ctSendService.SendBatchExtractsAsync(package, 500, new BaselineMessageBag()).Result;
            var idsC= _ctSendService.SendBatchExtractsAsync(package, 500, new StatusMessageBag()).Result;
            var idsD=_ctSendService.SendBatchExtractsAsync(package, 500, new AdverseEventsMessageBag()).Result;
        }
        public void SendDiffJobBaselines(SendManifestPackageDTO package)
        {
            var idsA =_ctSendService.SendDiffBatchExtractsAsync(package, 500, new ArtMessageBag()).Result;
            var idsB=_ctSendService.SendDiffBatchExtractsAsync(package, 500, new BaselineMessageBag()).Result;
            var idsC= _ctSendService.SendDiffBatchExtractsAsync(package, 500, new StatusMessageBag()).Result;
            var idsD=_ctSendService.SendDiffBatchExtractsAsync(package, 500, new AdverseEventsMessageBag()).Result;
        }
        public void SendJobProfiles(SendManifestPackageDTO package)
        {
            var idsA =_ctSendService.SendBatchExtractsAsync(package, 500, new PharmacyMessageBag()).Result;
            var idsB=_ctSendService.SendBatchExtractsAsync(package, 500, new LabMessageBag()).Result;
            var idsC= _ctSendService.SendBatchExtractsAsync(package, 500, new VisitsMessageBag()).Result;
        }
        public void SendDiffJobProfiles(SendManifestPackageDTO package)
        {
            var idsA =_ctSendService.SendDiffBatchExtractsAsync(package, 500, new PharmacyMessageBag()).Result;
            var idsB=_ctSendService.SendDiffBatchExtractsAsync(package, 500, new LabMessageBag()).Result;
            var idsC= _ctSendService.SendDiffBatchExtractsAsync(package, 500, new VisitsMessageBag()).Result;
        }

        [AutomaticRetry(Attempts = 0)]
        private void QueueMpi(SendManifestPackageDTO package)
        {
            var client = new BackgroundJobClient();
            var state = new EnqueuedState("mpi");
            client.Create(() => _cbsSendService.SendMpiAsync(package), state);
        }
    }
}
